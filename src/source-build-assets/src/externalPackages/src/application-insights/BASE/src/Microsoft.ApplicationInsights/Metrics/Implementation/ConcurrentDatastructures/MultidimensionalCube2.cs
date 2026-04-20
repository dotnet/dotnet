namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.ExceptionServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using static System.FormattableString;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001: Types that own disposable fields should be disposable", Justification = "OK not to explicitly dispose a released SemaphoreSlim.")]
    internal class MultidimensionalCube2<TPoint>
    {
        /// <summary>
        /// We are using a recursive implementation for points creation so we are limiting the max dimension count to prevent stack overflow.
        /// In practice this is unlikely to be ever reached.
        /// If it nevertheless becomes an issue, we can change the implementation to be iterative and increase this limit.
        /// </summary>
        private const int DimensionsCountLimit = 50;

        private const string PointMonikerSeparator = "\0";
        private static readonly string[] PointMonikerSeparatorAsArray = new string[1] { PointMonikerSeparator };

        private readonly SemaphoreSlim pointCreationLock = new SemaphoreSlim(1);

        private readonly int totalPointsCountLimit;
        private readonly int[] dimensionValuesCountLimits;
        private readonly HashSet<string>[] dimensionValues;
        private readonly ConcurrentDictionary<string, TPoint> points;
        private readonly Func<string[], TPoint> pointsFactory;

        private int totalPointsCount;
        private bool applyDimensionCapping;
        private string dimensionCapValue;

        public MultidimensionalCube2(Func<string[], TPoint> pointsFactory, params int[] dimensionValuesCountLimits)
            : this(Int32.MaxValue, pointsFactory, dimensionValuesCountLimits)
        {
        }

        public MultidimensionalCube2(int totalPointsCountLimit, Func<string[], TPoint> pointsFactory, params int[] dimensionValuesCountLimits)
        {
            if (totalPointsCountLimit < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(totalPointsCountLimit), Invariant($"{nameof(totalPointsCountLimit)} must be 1 or larger. Typically much larger."));
            }

            Util.ValidateNotNull(pointsFactory, nameof(pointsFactory));

            Util.ValidateNotNull(dimensionValuesCountLimits, nameof(dimensionValuesCountLimits));

            if (dimensionValuesCountLimits.Length == 0)
            {
                throw new ArgumentException("Cube must have 1 or more dimensions.", nameof(dimensionValuesCountLimits));
            }

            if (dimensionValuesCountLimits.Length > DimensionsCountLimit)
            {
                throw new ArgumentException(Invariant($"Cube may not have more than ${MultidimensionalCube2<TPoint>.DimensionsCountLimit} dimensions,")
                                          + Invariant($" but {dimensionValuesCountLimits.Length} dimensions were specified."));
            }

            for (int d = 0; d < dimensionValuesCountLimits.Length; d++)
            {
                if (dimensionValuesCountLimits[d] < 1)
                {
                    throw new ArgumentException(Invariant($"The limit of distinct dimension values must be 1 or larger, but the limit specified for dimension {d} is {dimensionValuesCountLimits[d]}."));
                }
            }

            this.totalPointsCountLimit = totalPointsCountLimit;
            this.dimensionValuesCountLimits = dimensionValuesCountLimits;
            this.dimensionValues = new HashSet<string>[dimensionValuesCountLimits.Length];
            this.points = new ConcurrentDictionary<string, TPoint>();
            this.pointsFactory = pointsFactory;

            this.totalPointsCount = 0;

            for (int i = 0; i < this.dimensionValues.Length; i++)
            {
                this.dimensionValues[i] = new HashSet<string>();
            }
        }

        public MultidimensionalCube2(int totalPointsCountLimit, Func<string[], TPoint> pointsFactory, bool applyDimensionCapping, string dimensionCapValue, params int[] dimensionValuesCountLimits)
            : this(totalPointsCountLimit, pointsFactory, dimensionValuesCountLimits)
        {
            this.dimensionCapValue = dimensionCapValue;
            this.applyDimensionCapping = applyDimensionCapping;
        }

        public int DimensionsCount
        {
            get { return this.dimensionValuesCountLimits.Length; }
        }

        public int TotalPointsCountLimit
        {
            get { return this.totalPointsCountLimit; }
        }

        public int TotalPointsCount
        {
            get { return Volatile.Read(ref this.totalPointsCount); }
        }

        public int GetDimensionValuesCountLimit(int dimension)
        {
            this.ValidateDimensionIndex(dimension);
            return this.dimensionValuesCountLimits[dimension];
        }

        public IReadOnlyCollection<string> GetDimensionValues(int dimension)
        {
            this.ValidateDimensionIndex(dimension);
            return (IReadOnlyCollection<string>)this.dimensionValues[dimension];
        }

        public IReadOnlyList<KeyValuePair<string[], TPoint>> GetAllPoints()
        {
            List<KeyValuePair<string[], TPoint>> currentPoints = new List<KeyValuePair<string[], TPoint>>(this.TotalPointsCount);
            this.GetAllPoints(currentPoints);
            return currentPoints;
        }

        public int GetAllPoints(ICollection<KeyValuePair<string[], TPoint>> pointContainer)
        {
            int count = 0;
            foreach (KeyValuePair<string, TPoint> storedPoint in this.points)
            {
                string[] coordinates = ParsePointMoniker(storedPoint.Key);
                KeyValuePair<string[], TPoint> parsedPoint = new KeyValuePair<string[], TPoint>(coordinates, storedPoint.Value);
                pointContainer.Add(parsedPoint);
                count++;
            }

            return count;
        }

        public MultidimensionalPointResult<TPoint> TryGetOrCreatePoint(params string[] coordinates)
        {
            string pointMoniker = this.GetPointMoniker(coordinates);

            TPoint point;
            bool hasPoint = this.points.TryGetValue(pointMoniker, out point);

            if (hasPoint)
            {
                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, point);
                return result;
            }

            if (this.totalPointsCount >= this.totalPointsCountLimit)
            {
                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached, -1);
                return result;
            }

            this.pointCreationLock.Wait();
            try
            {
                MultidimensionalPointResult<TPoint> result = this.TryCreatePoint(coordinates, pointMoniker);
                return result;
            }
            finally
            {
                this.pointCreationLock.Release();
            }
        }

        public Task<MultidimensionalPointResult<TPoint>> TryGetOrCreatePointAsync(params string[] coordinates)
        {
            return this.TryGetOrCreatePointAsync(CancellationToken.None, coordinates);
        }

        public async Task<MultidimensionalPointResult<TPoint>> TryGetOrCreatePointAsync(CancellationToken cancelToken, params string[] coordinates)
        {
            string pointMoniker = this.GetPointMoniker(coordinates);

            TPoint point;
            bool hasPoint = this.points.TryGetValue(pointMoniker, out point);

            if (hasPoint)
            {
                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, point);
                return result;
            }

            if (this.totalPointsCount >= this.totalPointsCountLimit)
            {
                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached, -1);
                return result;
            }

            await this.pointCreationLock.WaitAsync(cancelToken).ConfigureAwait(continueOnCapturedContext: false);
            try
            {
                MultidimensionalPointResult<TPoint> result = this.TryCreatePoint(coordinates, pointMoniker);
                return result;
            }
            finally
            {
                this.pointCreationLock.Release();
            }
        }

        public MultidimensionalPointResult<TPoint> TryGetPoint(params string[] coordinates)
        {
            string pointMoniker = this.GetPointMoniker(coordinates);

            TPoint point;
            bool hasPoint = this.points.TryGetValue(pointMoniker, out point);

            if (hasPoint)
            {
                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, point);
                return result;
            }
            else
            {
                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, -1);
                return result;
            }
        }

        private static string[] ParsePointMoniker(string pointMoniker)
        {
            string[] coordinates = pointMoniker.Split(PointMonikerSeparatorAsArray, StringSplitOptions.None);
            return coordinates;
        }

        private static string BuildPointMoniker(string[] coordinates)
        {
            if (coordinates.Length == 0)
            {
                return String.Empty;
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < coordinates.Length; i++)
            {
                if (coordinates[i] == null)
                {
                    throw new ArgumentNullException(
                                        Invariant($"{nameof(coordinates)}[{i}]"), 
                                        Invariant($"The specified {nameof(coordinates)}-vector contains null at index {i}."));
                }

                if (coordinates[i].Contains(PointMonikerSeparator))
                {
                    throw new ArgumentException(Invariant($"The value at index {i} of the specified {nameof(coordinates)}-vector contains")
                                              + Invariant($" an invalid character sub-sequence. Complete coordinate value: \"{coordinates[i]}\".")
                                              + Invariant($" Invalid sub-sequence: \"{PointMonikerSeparator}\"."));
                }

                if (i > 0)
                {
#pragma warning disable CA1834 // Consider using 'StringBuilder.Append(char)' when applicable
                    // TODO: check if we can change from string to char, this will have some work
                    // since it's used as separator in split and contains
                    builder.Append(PointMonikerSeparator);
#pragma warning restore CA1834 // Consider using 'StringBuilder.Append(char)' when applicable
                }

                builder.Append(coordinates[i]);
            }

            return builder.ToString();
        }

        private MultidimensionalPointResult<TPoint> TryCreatePoint(string[] coordinates, string pointMoniker)
        {
#pragma warning disable SA1509 // Opening braces must not be preceded by blank line

            // We already have tried getting the existing point and failed.
            // We also checked that _totalPointsCountLimit was not reached (outside the lock).
            // Lastly, we took a lock.
            // Now we can begin the slow path.

            // First, we need to try retrieving the point again, now under the lock:
            TPoint point;
            bool hasPoint = this.points.TryGetValue(pointMoniker, out point);
            if (hasPoint)
            {
                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, point);
                return result;
            }

            // Then, check total count again now that we are under lock:
            if (this.totalPointsCount >= this.totalPointsCountLimit)
            {
                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached, -1);
                return result;
            }

            // Examine each dimension and see if it reached values count limit. If not, track the new value:

            int reachedValsLimitDim = -1;
            BitArray valueAddedToDims = new BitArray(length: coordinates.Length, defaultValue: false);
            bool dimensionCappingApplied = false;

            for (int i = 0; i < coordinates.Length; i++)
            {
                HashSet<string> dimVals = this.dimensionValues[i];
                string coordinateVal = coordinates[i];

                if ((dimVals.Count >= this.dimensionValuesCountLimits[i]) && (false == dimVals.Contains(coordinateVal)))
                {
                    if (this.applyDimensionCapping)
                    {
                        // throwing away the actual dimension value here as we are asked to apply dimensioncapping.
                        coordinates[i] = this.dimensionCapValue;
                        dimensionCappingApplied = true;
                    }
                    else
                    {
                        reachedValsLimitDim = i;
                        break;
                    }
                }

                bool added = dimVals.Add(coordinates[i]);
                valueAddedToDims.Set(i, added);
            }

            // We hit the _dimensionValuesCountLimits at some dimension.
            // Remove what we just added to dim value sets and give up.

            if (reachedValsLimitDim != -1)
            {
                for (int i = 0; i <= reachedValsLimitDim; i++)
                {
                    if (valueAddedToDims.Get(i))
                    {
                        this.dimensionValues[i].Remove(coordinates[i]);
                    }
                }

                var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_SubdimensionsCountLimitReached, reachedValsLimitDim);
                return result;
            }

            if (dimensionCappingApplied)
            {
                // We did dimension capping - i.e one or more dimension values were replaced, requiring us to reconstruct the moniker with new values.
                pointMoniker = BuildPointMoniker(coordinates);

                // Check again (with new moniker) if the point already exists.
                hasPoint = this.points.TryGetValue(pointMoniker, out point);
                if (hasPoint)
                {
                    var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, point);
                    return result;
                }

                // Continue with point creation using new moniker.
            }

            // Create new point:
            try
            {
                point = this.pointsFactory(coordinates);
            }
            catch (Exception ex)
            {
                // User code in _pointsFactory may throw. In that case we need to clean up from the added value containers:
                for (int i = 0; i <= reachedValsLimitDim; i++)
                {
                    if (valueAddedToDims.Get(i))
                    {
                        this.dimensionValues[i].Remove(coordinates[i]);
                    }
                }

                ExceptionDispatchInfo.Capture(ex).Throw();
                throw;  // This line will never be reached
            }

            { 
                bool added = this.points.TryAdd(pointMoniker, point);
                if (false == added)
                {
                    throw new InvalidOperationException(Invariant($"Internal SDK bug. Please report this! (pointMoniker: {pointMoniker})")
                                                      + Invariant($" Info: Failed to add a point to the {nameof(this.points)}-collection in")
                                                      + Invariant($" class {nameof(MultidimensionalCube2<TPoint>)} despite passing all the cerfification checks."));
                }
            }

            // Inc total points count.
            this.totalPointsCount++;

            {
                var result = new MultidimensionalPointResult<TPoint>(
                    dimensionCappingApplied ? MultidimensionalPointResultCodes.Success_NewPointCreatedAboveDimCapLimit : MultidimensionalPointResultCodes.Success_NewPointCreated,
                    point);
                return result;
            }
#pragma warning restore SA1509 // Opening braces must not be preceded by blank line
        }

        private void ValidateDimensionIndex(int dimension)
        {
            if (dimension < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension), "Dimension index may not be negative.");
            }

            if (dimension >= this.DimensionsCount)
            {
                throw new ArgumentOutOfRangeException(nameof(dimension), Invariant($"Dimension index (zero-based) exceeds the number of dimensions of this cube ({this.DimensionsCount})."));
            }
        }

        private string GetPointMoniker(string[] coordinates)
        {
            Util.ValidateNotNull(coordinates, nameof(coordinates));

            if (coordinates.Length != this.DimensionsCount)
            {
                throw new ArgumentException(
                            Invariant($"The specified {nameof(coordinates)}-vector has {coordinates.Length} dimensions.")
                          + Invariant($" However this has {this.DimensionsCount} dimensions."),
                            nameof(coordinates));
            }

            return BuildPointMoniker(coordinates);
        }
    }
}
