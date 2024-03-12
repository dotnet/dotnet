  namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading;

    using static System.FormattableString;

    internal class MultidimensionalCubeDimension<TDimensionValue, TPoint>
    {
        private readonly MultidimensionalCube<TDimensionValue, TPoint> ownerCube;
        private readonly int subdimensionsCountLimit;
        private readonly bool isLastDimensionLevel;
        private ConcurrentDictionary<TDimensionValue, object> elements = new ConcurrentDictionary<TDimensionValue, object>();

        private int subdimensionsCount = 0;

        public MultidimensionalCubeDimension(MultidimensionalCube<TDimensionValue, TPoint> ownerCube, int subdimensionsCountLimit, bool isLastDimensionLevel)
        {
            this.ownerCube = ownerCube;
            this.subdimensionsCountLimit = subdimensionsCountLimit;
            this.isLastDimensionLevel = isLastDimensionLevel;
        }

        public MultidimensionalPointResult<TPoint> TryGetOrAddVector(TDimensionValue[] coordinates)
        {
            Util.ValidateNotNull(coordinates, nameof(coordinates));

            if (coordinates.Length != this.ownerCube.DimensionsCount)
            {
                throw new ArgumentException(
                            Invariant($"The specified {nameof(coordinates)}-vector has {coordinates.Length} dimensions.")
                          + Invariant($" However {nameof(this.ownerCube)} has {this.ownerCube.DimensionsCount} dimensions."),
                            nameof(coordinates));
            }

            MultidimensionalPointResult<TPoint> result = this.TryGetOrAddVectorInternal(coordinates, currentDim: 0, createIfNotExists: true);
            return result;
        }

        public MultidimensionalPointResult<TPoint> TryGetVector(TDimensionValue[] coordinates)
        {
            Util.ValidateNotNull(coordinates, nameof(coordinates));

            if (coordinates.Length != this.ownerCube.DimensionsCount)
            {
                throw new ArgumentException(
                            Invariant($"The specified {nameof(coordinates)}-vector has {coordinates.Length} dimensions.")
                          + Invariant($" However {nameof(this.ownerCube)} has {this.ownerCube.DimensionsCount} dimensions."),
                            nameof(coordinates));
            }

            MultidimensionalPointResult<TPoint> result = this.TryGetOrAddVectorInternal(coordinates, currentDim: 0, createIfNotExists: false);
            return result;
        }

        public IReadOnlyCollection<KeyValuePair<IList<TDimensionValue>, TPoint>> GetAllPointsReversed()
        {
            List<KeyValuePair<IList<TDimensionValue>, TPoint>> pointDescriptions = new List<KeyValuePair<IList<TDimensionValue>, TPoint>>();

            if (this.isLastDimensionLevel)
            {
                foreach (KeyValuePair<TDimensionValue, object> element in this.elements)
                {
                    var pointDesc = new KeyValuePair<IList<TDimensionValue>, TPoint>(new List<TDimensionValue>(), (TPoint)element.Value);
                    pointDesc.Key.Add(element.Key);
                    pointDescriptions.Add(pointDesc);
                }
            }
            else
            {
                foreach (KeyValuePair<TDimensionValue, object> element in this.elements)
                {
                    var elementValue = (MultidimensionalCubeDimension<TDimensionValue, TPoint>)element.Value;
                    IReadOnlyCollection<KeyValuePair<IList<TDimensionValue>, TPoint>> subCube = elementValue.GetAllPointsReversed();
                    foreach (KeyValuePair<IList<TDimensionValue>, TPoint> subVector in subCube)
                    {
                        subVector.Key.Add(element.Key);
                        pointDescriptions.Add(subVector);
                    }
                }
            }

            return pointDescriptions;
        }

        private MultidimensionalPointResult<TPoint> TryGetOrAddVectorInternal(TDimensionValue[] coordinates, int currentDim, bool createIfNotExists)
        {
            TDimensionValue subElementKey = coordinates[currentDim];

            // Try and get the referenced element:
            object subElement;
            bool subElementExists = this.elements.TryGetValue(subElementKey, out subElement);

            // If the referenced element exists, we can simply proceed:
            if (subElementExists)
            {
                if (this.isLastDimensionLevel)
                {
                    var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, (TPoint)subElement);
                    return result;
                }
                else
                {
                    MultidimensionalCubeDimension<TDimensionValue, TPoint> subDim = (MultidimensionalCubeDimension<TDimensionValue, TPoint>)subElement;
                    MultidimensionalPointResult<TPoint> result = subDim.TryGetOrAddVectorInternal(coordinates, currentDim + 1, createIfNotExists);
                    return result;
                }
            }
            else
            {
            // so - subElement does NOT exist:
                // If we are not to create new elements, we are done:
                if (false == createIfNotExists)
                {
                    var result = new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_PointDoesNotExistCreationNotRequested, currentDim);
                    return result;
                }
                else
                {
                    MultidimensionalPointResult<TPoint> result = this.isLastDimensionLevel
                                                        ? this.TryAddPoint(coordinates, currentDim)
                                                        : this.TryAddSubvector(coordinates, currentDim);
                    return result;
                }
            }
        }

        private MultidimensionalPointResult<TPoint> TryAddPoint(TDimensionValue[] coordinates, int currentDim)
        {
            // We need to create a new sub-element, but we need to make sure that we do not exceed max dimension count in a concurrent situation.
            // To avoid locking as much as possible (it will be inevitable in some situations inside of the concurrent dictionary), we use an
            // interlocked increment of the dimension count early in the process to pre-book the new element. If all creation steps do not
            // complete succesfully, we will undo the increment.
            // Fortunately, it is a relatively rare case:
            // The increment will be successful only if the specified coordinates vector does not (yet) exist, but the max dimension limit is not yet exhaustet.
            // This can occur no more than (approx.) Cm = Md1 * Md2 * ... * Mdn times, where Mdi is the max dimension limit for dimension i. 

            // Check if we reached the dimensions count limit. If we did, we give up. Otherwise we start tracking whether we need to undo the increment later:
            if (false == this.TryIncSubdimensionsCount())
            {
                return new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_SubdimensionsCountLimitReached, currentDim);
            }

            bool mustRestoreSubdimensionsCount = true;
            try
            {
                // We are on the last level and we need to create the actual point. However, before doing that we need to check and pre-book the total
                // count limit using the same pattern as the dimension values limit:
                if (false == this.ownerCube.TryIncTotalPointsCount())
                {
                    return new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached, failureCoordinateIndex: -1);
                }

                bool mustRestoreTotalPointsCount = true;
                try
                {
                    TPoint newPoint = this.ownerCube.InvokePointsFactory(coordinates);

                    TDimensionValue subElementKey = coordinates[currentDim];
                    bool couldInsert = this.elements.TryAdd(subElementKey, newPoint);

                    // There is a race on someone calling GetOrAddVector(..) with the same coordinates. So point may or may not already be in the list.
                    if (couldInsert)
                    {
                        mustRestoreTotalPointsCount = false;
                        mustRestoreSubdimensionsCount = false;
                        return new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Success_NewPointCreated, newPoint);
                    }
                    else
                    {
                        // If the point was already in the list, then that other point created by the race winner is the one we want.
                        TPoint existingPoint = (TPoint)this.elements[subElementKey];
                        return new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Success_ExistingPointRetrieved, existingPoint);
                    }
                }
                finally
                {
                    if (mustRestoreTotalPointsCount)
                    {
                        this.ownerCube.DecTotalPointsCount();
                    }
                }
            }
            finally
            {
                if (mustRestoreSubdimensionsCount)
                {
                    this.DecSubdimensionsCount();
                }
            }
        }

        private MultidimensionalPointResult<TPoint> TryAddSubvector(TDimensionValue[] coordinates, int currentDim)
        {
            // Note the comment near the top if TryAddPoint(..) about the applied minimal locking strategy.

            // Check if we reached the dimensions count limit. If we did, we give up. Otherwise we start tracking whether we need to undo the increment later:
            if (false == this.TryIncSubdimensionsCount())
            {
                return new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_SubdimensionsCountLimitReached, currentDim);
            }

            bool mustRestoreSubdimensionsCount = true;
            try
            {
                TDimensionValue subElementKey = coordinates[currentDim];

                // Do a soft-check to see if we reached the total points limit. If we do, there is no need to bother:
                // (We will do a hard check and pre-booking later when we actually about to create the point.)
                if (this.ownerCube.TotalPointsCount >= this.ownerCube.TotalPointsCountLimit)
                {
                    return new MultidimensionalPointResult<TPoint>(MultidimensionalPointResultCodes.Failure_TotalPointsCountLimitReached, failureCoordinateIndex: -1);
                }

                // We are not at the last level. Create the subdimension. Note, we are not under lock, so someone might be creating the same dimention concurrently:
                int nextDim = currentDim + 1;
                bool isLastDimensionLevel = nextDim == (coordinates.Length - 1);
                var newSubDim = new MultidimensionalCubeDimension<TDimensionValue, TPoint>(
                                                                                           this.ownerCube, 
                                                                                           this.ownerCube.GetSubdimensionsCountLimit(nextDim),
                                                                                           isLastDimensionLevel);
                MultidimensionalPointResult<TPoint> newSubDimResult = newSubDim.TryGetOrAddVectorInternal(coordinates, nextDim, createIfNotExists: true);

                // Becasue we have not yet inserted newSubDim into _elements, any operations on newSubDim are not under concurrency.
                // There are no point-vectors yet pointing to the sub-space of newSubDim, so no DimensionValuesCountLimit can be reached.
                // So, hasNewPoint can be false only if TotalPointsCountLimit was reached. We just bail out:
                if (false == newSubDimResult.IsSuccess)
                {
                    return newSubDimResult;
                }

                // The new point has been created and we need to add its sub-space to the list.
                // However, there is a race on someone calling GetOrAddVector(..) with the same coordinates. So newSubDim may or may not already be in the list.
                bool couldInsert = this.elements.TryAdd(subElementKey, newSubDim);

                if (couldInsert)
                {
                    // Success. We created and inserted a new sub-space:
                    mustRestoreSubdimensionsCount = false;
                    return newSubDimResult;
                }
                else
                {
                    // The point was already in the list. It's that other point (created by the race winner) that we want.
                    // We need to discard the sub-space and the point we just created: Decrement total points count and Decrement the dimension value count.
                    // After that we just call TryGetOrAddVectorInternal(..) again on the same recursion level.
                    this.ownerCube.DecTotalPointsCount();
                }
            }
            finally
            {
                if (mustRestoreSubdimensionsCount)
                {
                    this.DecSubdimensionsCount();
                }
            }

            MultidimensionalPointResult<TPoint> retryResult = this.TryGetOrAddVectorInternal(coordinates, currentDim, createIfNotExists: true);
            return retryResult;
        }

        private bool TryIncSubdimensionsCount()
        {
            int newSubdimensionsCount = Interlocked.Increment(ref this.subdimensionsCount);

            if (newSubdimensionsCount <= this.subdimensionsCountLimit)
            {
                return true;
            }

            Interlocked.Decrement(ref this.subdimensionsCount);
            return false;
        }

        private int DecSubdimensionsCount()
        {
            int newSubdimensionsCount = Interlocked.Decrement(ref this.subdimensionsCount);
            return newSubdimensionsCount;
        }
    }
}
