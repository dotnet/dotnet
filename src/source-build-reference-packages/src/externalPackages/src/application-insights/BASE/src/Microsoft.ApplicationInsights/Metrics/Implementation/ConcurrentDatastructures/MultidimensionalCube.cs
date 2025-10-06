namespace Microsoft.ApplicationInsights.Metrics.ConcurrentDatastructures
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using System.Threading.Tasks;

    using static System.FormattableString;

    /// <summary>
    /// Represents a multi-dimensional, discrete data cube.
    /// An N-dimensional discrete cube is a data structure containing elements of type TPoint.
    /// Each point can be addressed using a coordinate-vector containing N entries of type TDimensionValue.
    /// <br />
    /// For example, the elements in the cube may be colors, space vectors or sold items (with its properties as coordinates).
    /// However, the key usage in this library are multidimensional metrics. In such usage the coordinates are metric
    /// dimensions (TDimensionValue is String) and the cube elements (TPoint) are metric data series.
    /// <p>
    /// The cube refers to dimensions using their index: 0th, 1st, 2nd dimension and so on. For each dimension, the cube has a <c>SubdimensionsCountLimit</c>
    /// parameter. This limits the number of distinct values that may be specified for that specific sub-dimension (given concrete values for lower dimensions).
    /// Once the <c>SubdimensionsCountLimit</c> for some particular dimension is reached, the cube will no longer be able to create points whose coordinate
    /// vectors contain values for that particular dimension that do not occur in coordinate vectors for points already in the cube.
    /// The cube also has a <c>TotalPointsCountLimit</c> parameter that limits the total number of points in the cube.
    /// </p>
    /// <p>
    /// The elements of the coordinate vectors (i.e. the values of the dimensions of a point in the cube) are discrete. Even if their type implies a non-discrete
    /// space (e.g.when TDimensionValue is Double) they are always treated as discrete and separate.
    /// </p>
    /// <p>
    /// The cube is designed to work in concurrent scenarios while minimizing the number, the duration and the scope of locks taken. However, some locks are
    /// necessary to correctly impose the SubdimensionsCountLimits and the TotalPointsCountLimit constrains.
    /// </p>
    /// <p>
    /// This implementation assumes that creation and storage of TPoint-elements is resource intensive. It creates points lazily, only when requested.
    /// However, to minimize locking it uses pessimistic and optimistic concurrency mechanisms where possible. Two artifacts occur as a result:
    /// </p>
    /// <list type="bullet">
    ///   <item><description>The specified <c>pointsFactory</c> delegate may be executed more than once for a particular coordinates vector.
    ///     However, once a point for the specified coordinates-vector has been actually returned to the caller, always the same instance of
    ///     that point will be returned by the cube. This behaviour is consistent with the ConcurrentDictionary in the .NET Framework.</description></item>
    ///   <item><description>The <c>TryGetOrCreatePoint(..)</c> may return <c>false</c>, and then return <c>true</c> moments later when called with the
    ///     same parameters. This is because in order to avoid locking the cube pre-books dimension value counts (and total points counts) and later
    ///     frees them up if the creation of a new point did not complete.
    ///     Notably, this artifact does not represent any problems in practice: It occurs only in concurrent races when the number of values of a
    ///     dimension (or the total number of points) is close to the limit, where applications should not rely on a particular outcome of adding a
    ///     point anyway. In common cases one can assume that the result of <c>TryGetOrCreatePoint(..)</c> is, indeed, stable.
    ///     In order to control potential instability use <c>TryGetOrCreatePointAsync(..)</c> overloads.
    ///     Note, however, that those overloads do not guarantee that the result of requesting a new point is completely stable. They merely make it
    ///     very unlikely for it to change by re-trying the operation several times.</description></item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <p>** Color Example: **</p>
    /// <p>
    /// An example of a cube is an RGB color space.
    /// Assume that we use 8 bits (= 256 values) per color and that is a color class such as this:
    /// </p>
    /// <code>
    /// class Color
    /// {
    ///     public Color(byte red, byte green, byte blue) { }
    ///     //...
    /// }
    /// </code>
    /// <p>
    /// It is possible to represent the entire space using a cube as follows:
    /// </p>
    /// <code>
    /// Func{int[], Color} colorFactory = (rgbValues) =} new Color(red: rgbValues[0], green: rgbValues[1], blue: rgbValues[2]);
    /// MultidimensionalCube{int, Color} colorCube = new MultidimensionalCube{int, Color}(colorFactory, 256, 256, 256);
    /// </code>
    /// <p>
    /// We can now address specific color objects we follows:
    /// </p>
    /// <code>
    /// MultidimensionalPointResult{Color} greenResult = colorCube.TryGetOrCreatePoint(0, 255, 0);
    /// Color green = greenResult.Point;
    /// 
    /// MultidimensionalPointResult{Color} yellowResult = colorCube.TryGetOrCreatePoint(255, 255, 0);
    /// Color yellow = yellowResult.Point;
    /// 
    /// MultidimensionalPointResult{Color} azureResult = colorCube.TryGetOrCreatePoint(0, 127, 255);
    /// Color azure = azureResult.Point;
    /// </code>
    /// 
    /// <p>** Metrics Example: **</p>
    /// <p>
    /// A multi-dimensional cube is used to address data time series of a multidimensional metric.
    /// Consider that we have a metric representing the number of milliseconds required to process a request to a web-service.
    /// We collect that metric according to a number of possible groupings:
    /// </p>
    /// <list type="number">
    ///   <item><description>By the type of the request (the string “synthetic” (i.e.created by a test) or “organic” (created by a legitimate client)).</description></item>
    ///   <item><description>By the response generated (an HTTP response code encoded as a string).</description></item>
    ///   <item><description>By the name of the instance that executed the request (in case of a web service scaled over multiple instances).</description></item>
    ///   <item><description>By the name of the API invoked (any URL string).</description></item>
    /// </list>
    /// <p>
    /// Each view represents a separate dimension. There is a distinct data time series for each combination of dimension-values. For example:
    /// </p>
    /// <list type="bullet">
    ///   <item><description>Type=<c>"Organic"</c>, Response=<c>"200"</c>, Instance=<c>"Instance_01"</c>, API=<c>"http://myservice.com/API1?paramA=X"</c>.</description></item>
    ///   <item><description>Type=<c>"Organic"</c>, Response=<c>"500"</c>, Instance=<c>"Instance_01"</c>, API=<c>"http://myservice.com/API2?paramB=Y"</c>.</description></item>
    /// </list>
    /// <p>
    /// So a data series representation might look like this:
    /// </p>
    /// <code>
    /// class ResponseTimeMetricSeries
    /// {
    ///     public ResponseTimeMetricSeries(string kind, string responseCode, string instanceName, string url) { }
    ///     //...
    /// }
    /// </code>
    /// <p>
    /// Note also that while some dimensions can naturally take a small amount of values(e.g.Type can only take 2 values in this example), URL can essentially
    /// take an unbounded number of values. In our example we want to limit the number of dimension values for specific dimensions in order to control resource
    /// usage. We can create a cube as follows. (Note how we used different max dimension values counts that are appropriate for each respective dimension.)
    /// </p>
    /// <code>
    /// Func{string[], ResponseTimeMetricSeries} metricFactory = (dimValues) =} new ResponseTimeMetricSeries(
    ///                                                                                         kind:         dimValues[0],
    ///                                                                                         responseCode: dimValues[1],
    ///                                                                                         instanceName: dimValues[2],
    ///                                                                                         url:          dimValues[3]);
    /// MultidimensionalCube{string, ResponseTimeMetricSeries} metricsCube = new MultidimensionalCube{string, ResponseTimeMetricSeries}(metricFactory, 2, 50, 100, 1000);
    /// 
    /// MultidimensionalPointResult{ResponseTimeMetricSeries} result;
    /// 
    /// result = metricsCube.TryGetOrCreatePoint("Organic", "200", "Instance_01", "http://myservice.com/API1?paramA=X");
    /// if (! result.Success)
    /// {
    ///     throw new SomeAppropriateException("Cannot create metric data series. Dimension cap is potentially reached.");
    /// }
    /// 
    /// ResponseTimeMetricDataSeries responseSeries01 = result.Point;
    /// 
    /// 
    /// result = metricsCube.TryGetOrCreatePoint("Organic", "500", "Instance_01", "http://myservice.com/API2?paramB=Y");
    /// if (! result.Success)
    /// {
    ///     throw new SomeAppropriateException("Cannot create metric data series. Dimension cap is potentially reached.");
    /// }
    /// 
    /// ResponseTimeMetricSeries responseSeries02 = result.Point;
    /// </code>
    /// </remarks>
    /// <typeparam name="TDimensionValue">Type of dimension values. For common metrics, it's <c>string</c>.</typeparam>
    /// <typeparam name="TPoint">Type of the item addreses by the dimension-values. For metrics it's a metric series.</typeparam>
    internal class MultidimensionalCube<TDimensionValue, TPoint>
    {
        /// <summary>
        /// We are using a recursive implementation for points creation so we are limiting the max dimension count to prevent strack overflow.
        /// In practice this is unlikely to be ever reached.
        /// If it nevertheless becomes an issue, we can change the implementation to be iterative and increase this limit.
        /// </summary>
        private const int DimensionsCountLimit = 50;

        private const string ExceptionThrownByPointsFactoryKey = "Microsoft.ApplicationInsights.ConcurrentDatastructures.MultidimensionalCube.ExceptionThrownByPointsFactory";

        private readonly int[] subdimensionsCountLimits;
        private readonly MultidimensionalCubeDimension<TDimensionValue, TPoint> points;
        private readonly Func<TDimensionValue[], TPoint> pointsFactory;
        private readonly int totalPointsCountLimit;

        private int totalPointsCount;

        public MultidimensionalCube(Func<TDimensionValue[], TPoint> pointsFactory, IEnumerable<int> subdimensionsCountLimits)
            : this(Int32.MaxValue, pointsFactory, subdimensionsCountLimits)
        {
        }

        public MultidimensionalCube(int totalPointsCountLimit, Func<TDimensionValue[], TPoint> pointsFactory, IEnumerable<int> subdimensionsCountLimits)
            : this(totalPointsCountLimit, pointsFactory, subdimensionsCountLimits?.ToArray())
        {
        }

        public MultidimensionalCube(Func<TDimensionValue[], TPoint> pointsFactory, params int[] subdimensionsCountLimits)
            : this(Int32.MaxValue, pointsFactory, subdimensionsCountLimits)
        {
        }

        public MultidimensionalCube(int totalPointsCountLimit, Func<TDimensionValue[], TPoint> pointsFactory, params int[] subdimensionsCountLimits)
        {
            if (totalPointsCountLimit < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(totalPointsCountLimit), Invariant($"{nameof(totalPointsCountLimit)} must be 1 or larger. Typically much larger."));
            }

            Util.ValidateNotNull(pointsFactory, nameof(pointsFactory));

            Util.ValidateNotNull(subdimensionsCountLimits, nameof(subdimensionsCountLimits));

            if (subdimensionsCountLimits.Length == 0)
            {
                throw new ArgumentException("Cube must have 1 or more dimensions.", nameof(subdimensionsCountLimits));
            }

            if (subdimensionsCountLimits.Length > DimensionsCountLimit)
            {
                throw new ArgumentException(Invariant($"Cube may not have more than ${MultidimensionalCube<TDimensionValue, TPoint>.DimensionsCountLimit} dimensions,")
                                          + Invariant($" but {subdimensionsCountLimits.Length} dimensions were specified."));
            }

            for (int d = 0; d < subdimensionsCountLimits.Length; d++)
            {
                if (subdimensionsCountLimits[d] < 1)
                {
                    throw new ArgumentException(Invariant($"The limit of distinct dimension values must be 1 or larger, but the limit specified for dimension {d} is {subdimensionsCountLimits[d]}."));
                }
            }

            this.totalPointsCountLimit = totalPointsCountLimit;

            this.subdimensionsCountLimits = subdimensionsCountLimits;
            this.points = new MultidimensionalCubeDimension<TDimensionValue, TPoint>(this, subdimensionsCountLimits[0], subdimensionsCountLimits.Length == 1);
            this.pointsFactory = pointsFactory;
        }

        public int DimensionsCount
        {
            get { return this.subdimensionsCountLimits.Length; }
        }

        public int TotalPointsCountLimit
        {
            get { return this.totalPointsCountLimit; }
        }

        public int TotalPointsCount
        {
            get { return Volatile.Read(ref this.totalPointsCount); }
        }

        public int GetSubdimensionsCountLimit(int dimension)
        {
            return this.subdimensionsCountLimits[dimension];
        }

        public IReadOnlyCollection<KeyValuePair<TDimensionValue[], TPoint>> GetAllPoints()
        {
            var vectors = new List<KeyValuePair<TDimensionValue[], TPoint>>();
            this.GetAllPoints(vectors);
            return vectors;
        }

        public void GetAllPoints(ICollection<KeyValuePair<TDimensionValue[], TPoint>> pointContainer)
        {
            var vectors = new List<KeyValuePair<TDimensionValue[], TPoint>>();

            IReadOnlyCollection<KeyValuePair<IList<TDimensionValue>, TPoint>> reversedVectors = this.points.GetAllPointsReversed();
            foreach (KeyValuePair<IList<TDimensionValue>, TPoint> rv in reversedVectors)
            {
                var v = new KeyValuePair<TDimensionValue[], TPoint>(new TDimensionValue[rv.Key.Count], rv.Value);
                int lastI = rv.Key.Count - 1;
                for (int i = lastI; i >= 0; i--)
                {
                    v.Key[lastI - i] = rv.Key[i];
                }

                pointContainer.Add(v);
            }
        }

        public MultidimensionalPointResult<TPoint> TryGetOrCreatePoint(params TDimensionValue[] coordinates)
        {
            MultidimensionalPointResult<TPoint> result = this.points.TryGetOrAddVector(coordinates);
            return result;
        }

        public MultidimensionalPointResult<TPoint> TryGetPoint(params TDimensionValue[] coordinates)
        {
            MultidimensionalPointResult<TPoint> result = this.points.TryGetVector(coordinates);
            return result;
        }

        public Task<MultidimensionalPointResult<TPoint>> TryGetOrCreatePointAsync(params TDimensionValue[] coordinates)
        {
            return this.TryGetOrCreatePointAsync(
                        sleepDuration: TimeSpan.FromMilliseconds(2),
                        timeout: TimeSpan.FromMilliseconds(11),
                        cancelToken: CancellationToken.None,
                        coordinates: coordinates);
        }

        public async Task<MultidimensionalPointResult<TPoint>> TryGetOrCreatePointAsync(
                                TimeSpan sleepDuration,
                                TimeSpan timeout,
                                CancellationToken cancelToken,
                                params TDimensionValue[] coordinates)
        {
            MultidimensionalPointResult<TPoint> result;

            try
            {
                result = this.TryGetOrCreatePoint(coordinates);
                if (result.IsSuccess)
                {
                    return result;
                }

                if (timeout == TimeSpan.Zero)
                {
                    result.SetAsyncTimeoutReachedFailure();
                    return result;
                }
            }
            catch (Exception ex)
            {
                if ((false == IsThrownByPointsFactoryKey(ex)) || (timeout == TimeSpan.Zero))
                {
                    ExceptionDispatchInfo.Capture(ex).Throw();
                }
            }

            bool infiniteTimeout = timeout == Timeout.InfiniteTimeSpan;

            if (false == infiniteTimeout)
            { 
                if (Math.Round(timeout.TotalMilliseconds) >= (double)Int32.MaxValue)
                {
                    throw new ArgumentOutOfRangeException(nameof(timeout), Invariant($"{nameof(timeout)} must be smaller than {Int32.MaxValue} msec, but it is {timeout}."));
                }

                if (Math.Round(timeout.TotalMilliseconds) < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(timeout), Invariant($"{nameof(timeout)} must be zero, positive or Infinite, but it is {timeout}."));
                }
            }

            if (Math.Round(sleepDuration.TotalMilliseconds) > (double)Int32.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(sleepDuration), Invariant($"{nameof(sleepDuration)} must be smaller than {Int32.MaxValue} msec, but it is {sleepDuration}."));
            }

            if (Math.Round(sleepDuration.TotalMilliseconds) < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(sleepDuration), Invariant($"{nameof(sleepDuration)} must be non-negative, but it is {sleepDuration}."));
            }

            int timeoutMillis = (int)Math.Round(timeout.TotalMilliseconds);
            int sleepMillis = (int)Math.Round(sleepDuration.TotalMilliseconds);

            int startMillis = Environment.TickCount;
            int stopMillis = startMillis + timeoutMillis;

            while (true)
            {
                cancelToken.ThrowIfCancellationRequested();

                int delayMillis;
                try
                {
                    result = this.TryGetOrCreatePoint(coordinates);
                    if (result.IsSuccess)
                    {
                        return result;
                    }

                    delayMillis = infiniteTimeout
                                        ? sleepMillis
                                        : Math.Min(stopMillis - Environment.TickCount, sleepMillis);

                    if (delayMillis < 0)
                    {
                        result.SetAsyncTimeoutReachedFailure();
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    if (false == IsThrownByPointsFactoryKey(ex))
                    {
                        ExceptionDispatchInfo.Capture(ex).Throw();
                    }

                    delayMillis = infiniteTimeout
                                        ? sleepMillis
                                        : Math.Min(stopMillis - Environment.TickCount, sleepMillis);
                    if (delayMillis < 0)
                    {
                        ExceptionDispatchInfo.Capture(ex).Throw();
                    }
                }

                await Task.Delay(delayMillis, cancelToken).ConfigureAwait(true);
            }
        }

        internal TPoint InvokePointsFactory(TDimensionValue[] coordinates)
        {
            try
            {
                TPoint point = this.pointsFactory(coordinates);
                return point;
            }
            catch (Exception ex)
            {
                ex.Data[ExceptionThrownByPointsFactoryKey] = Boolean.TrueString;
                ExceptionDispatchInfo.Capture(ex).Throw();
                return default(TPoint); // never reached
            }
        }

        internal bool TryIncTotalPointsCount()
        {
            int newTotalPointsCount = Interlocked.Increment(ref this.totalPointsCount);

            if (newTotalPointsCount <= this.totalPointsCountLimit)
            {
                return true;
            }

            Interlocked.Decrement(ref this.totalPointsCount);
            return false;
        }

        internal int DecTotalPointsCount()
        {
            int newTotalPointsCount = Interlocked.Decrement(ref this.totalPointsCount);
            return newTotalPointsCount;
        }

        private static bool IsThrownByPointsFactoryKey(Exception exception)
        {
            IDictionary exceptionData = exception?.Data;
            if (exceptionData == null)
            {
                return false;
            }

            object marker = exceptionData[ExceptionThrownByPointsFactoryKey];
            if (marker == null)
            {
                return false;
            }

            return bool.TrueString.Equals(marker);
        }
    }
}
