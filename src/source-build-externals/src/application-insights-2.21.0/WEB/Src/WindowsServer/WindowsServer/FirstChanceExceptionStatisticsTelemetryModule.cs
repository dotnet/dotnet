#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading;

    using Microsoft.ApplicationInsights.Common;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
    using Microsoft.ApplicationInsights.WindowsServer.Implementation;

    /// <summary>
    /// The module subscribed to AppDomain.CurrentDomain.FirstChanceException to send exceptions statistics to ApplicationInsights.
    /// </summary>
    public sealed class FirstChanceExceptionStatisticsTelemetryModule : ITelemetryModule, IDisposable
    {
        internal const int OperationNameCacheSize = 100;
        internal const int ProblemIdCacheSize = 10000;

        internal const double CurrentWeight = .7;
        internal const double NewWeight = .3;
        internal const long TicksMovingAverage = 100000000; // 10 seconds

        internal const string OperationNameTag = "ai.operation.name";

        internal long MovingAverageTimeout;
        internal double TargetMovingAverage = 5000;

        // cheap dimension capping
        internal long DimCapTimeout;

        internal MetricManager MetricManager;

        private const int LOCKED = 1;
        private const int UNLOCKED = 0;

        /// <summary>
        /// This object prevents double entry into the exception callback.
        /// </summary>
        [ThreadStatic]
        private static int executionSyncObject;

        private readonly Action<EventHandler<FirstChanceExceptionEventArgs>> registerAction;
        private readonly Action<EventHandler<FirstChanceExceptionEventArgs>> unregisterAction;
        private readonly object lockObject = new object();
        private readonly object movingAverageLockObject = new object();

        private TelemetryClient telemetryClient;

        private bool isInitialized = false;

        private long newThreshold = 0;
        private long newProcessed = 0;
        private double currentMovingAverage = 0;

        private long cacheLifetime = 300000000; // 30 seconds in ticks

        private HashCache<string> operationNameValues = new HashCache<string>();
        private HashCache<string> problemIdValues = new HashCache<string>();
        private HashCache<string> exceptionKeyValues = new HashCache<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstChanceExceptionStatisticsTelemetryModule" /> class.
        /// </summary>
        public FirstChanceExceptionStatisticsTelemetryModule() : this(
            action => AppDomain.CurrentDomain.FirstChanceException += action,
            action => AppDomain.CurrentDomain.FirstChanceException -= action)
        {
        }

        internal FirstChanceExceptionStatisticsTelemetryModule(
            Action<EventHandler<FirstChanceExceptionEventArgs>> registerAction,
            Action<EventHandler<FirstChanceExceptionEventArgs>> unregisterAction)
        {
            this.DimCapTimeout = DateTime.UtcNow.Ticks + this.cacheLifetime;
            this.MovingAverageTimeout = DateTime.UtcNow.Ticks - 1; // Setting the timeout to be expired

            this.registerAction = registerAction;
            this.unregisterAction = unregisterAction;
        }

        /// <summary>
        /// Initializes the telemetry module.
        /// </summary>
        /// <param name="configuration">Telemetry Configuration used for creating TelemetryClient for sending exception statistics to Application Insights.</param>
        public void Initialize(TelemetryConfiguration configuration)
        {
            // Core SDK creates 1 instance of a module but calls Initialize multiple times
            if (!this.isInitialized)
            {
                lock (this.lockObject)
                {
                    if (!this.isInitialized)
                    {
                        this.telemetryClient = new TelemetryClient(configuration);
                        this.telemetryClient.Context.GetInternalContext().SdkVersion = SdkVersionUtils.GetSdkVersion("exstat:");

                        this.MetricManager = new MetricManager(this.telemetryClient, configuration);

                        this.registerAction(this.CalculateStatistics);

                        this.isInitialized = true;
                    }
                }
            }
        }

        /// <summary>
        /// Disposing TaskSchedulerOnUnobservedTaskException instance. This class doesn't have the finalize method as we expect it 
        /// live for a duration of the process and be disposed by AI infrastructure.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static bool WasExceptionTracked(Exception exception)
        {
            // some exceptions like MemoryOverflow, ThreadAbort or ExecutionEngine are pre-instantiated 
            // so the .Data is not writable. Also it can be null in certain cases.
            if (exception.Data != null && !exception.Data.IsReadOnly)
            {
                string trackingId = "MS." + Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture);

                if (exception.Data.Contains(trackingId) == true)
                {
                    return true;
                }
                else
                {
                    // mark exception as tracked
                    exception.Data[trackingId] = null; // The value is unimportant. It's just a sentinel.
                }
            }

            return false;

            //// This is temporarily being commented out to capture outer exceptions. It will be modified later. 
            ////if (!wasTracked)
            ////{
            ////    var innerException = exception.InnerException;
            ////    if (innerException != null)
            ////    {
            ////        wasTracked = IsTracked(innerException);
            ////    }
            ////}

            ////if (!wasTracked && exception is AggregateException)
            ////{
            ////    foreach (var innerException in ((AggregateException)exception).InnerExceptions)
            ////    {
            ////        if (innerException != null)
            ////        {
            ////            wasTracked = IsTracked(innerException);

            ////            if (wasTracked == true)
            ////            {
            ////                break;
            ////            }
            ////        }
            ////    }
            ////}
        }

        private static string GetDimCappedString(string dimensionValue, HashCache<string> hashCache, int cacheSize)
        {
            hashCache.RwLock.EnterReadLock();

            if (hashCache.ValueCache.Contains(dimensionValue) == true)
            {
                hashCache.RwLock.ExitReadLock();

                return dimensionValue;
            }

            if (hashCache.ValueCache.Count > cacheSize)
            {
                hashCache.RwLock.ExitReadLock();

                return null;
            }

            hashCache.RwLock.ExitReadLock();
            hashCache.RwLock.EnterWriteLock();

            hashCache.ValueCache.Add(dimensionValue);

            hashCache.RwLock.ExitWriteLock();

            return dimensionValue;
        }

        /// <summary>
        /// IDisposable implementation.
        /// </summary>
        /// <param name="disposing">The method has been called directly or indirectly by a user's code.</param>
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.unregisterAction(this.CalculateStatistics);
                this.operationNameValues.Dispose();
                this.problemIdValues.Dispose();
                this.exceptionKeyValues.Dispose();
                this.MetricManager.Dispose();
            }
        }

        private void CalculateStatistics(object sender, FirstChanceExceptionEventArgs firstChanceExceptionArgs)
        {
            // this is thread local variable. No need to lock
            if (executionSyncObject == LOCKED)
            {
                return;
            }

            try
            {
                Exception exception;
                string exceptionType;
                System.Diagnostics.StackFrame exceptionStackFrame;
                string problemId;
                string methodName = "UnknownMethod";
                int methodOffset = System.Diagnostics.StackFrame.OFFSET_UNKNOWN;
                bool getOperationName = false;

                executionSyncObject = LOCKED;

                if (this.MovingAverageTimeout < DateTime.UtcNow.Ticks)
                {
                    lock (this.movingAverageLockObject)
                    {
                        if (this.MovingAverageTimeout < DateTime.UtcNow.Ticks)
                        {
                            if (this.MovingAverageTimeout + TicksMovingAverage < DateTime.UtcNow.Ticks)
                            {
                                this.currentMovingAverage = 0;
                            }
                            else
                            {
                                this.currentMovingAverage = (this.currentMovingAverage * CurrentWeight) +
                                (((double)this.newProcessed) * NewWeight);
                            }

                            this.newThreshold = (long)((this.TargetMovingAverage - (this.currentMovingAverage * CurrentWeight)) / NewWeight);

                            this.newProcessed = 0;

                            this.MovingAverageTimeout = DateTime.UtcNow.Ticks + TicksMovingAverage;
                        }
                    }
                }

                exception = firstChanceExceptionArgs?.Exception;

                if (exception == null)
                {
                    WindowsServerEventSource.Log.FirstChanceExceptionCallbackExeptionIsNull();
                    return;
                }

                if (WasExceptionTracked(exception) == true)
                {
                    return;
                }

                exceptionType = exception.GetType().FullName;

                exceptionStackFrame = new System.Diagnostics.StackFrame(1);

                if (exceptionStackFrame != null)
                {
                    MethodBase methodBase = exceptionStackFrame.GetMethod();

                    if (methodBase != null)
                    {
                        methodName = (methodBase.DeclaringType?.FullName ?? "Global") + "." + methodBase.Name;
                        methodOffset = exceptionStackFrame.GetILOffset();
                    }
                }

                if (methodOffset == System.Diagnostics.StackFrame.OFFSET_UNKNOWN)
                {
                    problemId = exceptionType + " at " + methodName;
                }
                else
                {
                    problemId = exceptionType + " at " + methodName + ":" + methodOffset.ToString(CultureInfo.InvariantCulture);
                }

                if (this.newProcessed < this.newThreshold)
                {
                    Interlocked.Increment(ref this.newProcessed);

                    getOperationName = true;
                }

                this.TrackStatistics(getOperationName, problemId, exception);
            }
            catch (Exception exc)
            {
                try
                {
                    WindowsServerEventSource.Log.FirstChanceExceptionCallbackException(exc.ToInvariantString());
                }
                catch (Exception)
                {
                    // this is absolutely critical to not throw out of this method
                    // Otherwise it will affect the customer application behavior significantly
                }
            }
            finally
            {
                executionSyncObject = UNLOCKED;
            }
        }

        private void TrackStatistics(bool getOperationName, string problemId, Exception exception)
        {
            ExceptionTelemetry exceptionTelemetry = null;
            var dimensions = new Dictionary<string, string>();
            string operationName = null;
            string refinedOperationName = null;
            string refinedProblemId = null;

            if (this.DimCapTimeout < DateTime.UtcNow.Ticks)
            {
                this.ResetDimCapCaches(this.operationNameValues, this.problemIdValues, this.exceptionKeyValues);
            }

            refinedProblemId = GetDimCappedString(problemId, this.problemIdValues, ProblemIdCacheSize);

            if (string.IsNullOrEmpty(refinedProblemId) == true)
            {
                refinedProblemId = "MaxProblemIdValues";
            }

            dimensions.Add("problemId", refinedProblemId);

            if (SdkInternalOperationsMonitor.IsEntered())
            {
                refinedOperationName = "AI (Internal)";
                dimensions.Add(OperationNameTag, refinedOperationName);
            }
            else
            {
                if (getOperationName == true)
                {
                    exceptionTelemetry = new ExceptionTelemetry(exception);
                    this.telemetryClient.Initialize(exceptionTelemetry);
                    operationName = exceptionTelemetry.Context.Operation.Name;
                }

                if (string.IsNullOrEmpty(operationName) == false)
                {
                    refinedOperationName = GetDimCappedString(operationName, this.operationNameValues, OperationNameCacheSize);

                    dimensions.Add(OperationNameTag, refinedOperationName);
                }
            }

            this.SendException(refinedOperationName, refinedProblemId, exceptionTelemetry, exception);

            var metric = this.MetricManager.CreateMetric("Exceptions thrown", dimensions);

            metric.Track(1);
        }

        private void SendException(string operationName, string problemId, ExceptionTelemetry exceptionTelemetry, Exception exception)
        {
            string exceptionKey;

            if (string.IsNullOrEmpty(operationName) == true)
            {
                exceptionKey = problemId;
            }
            else
            {
                exceptionKey = operationName + problemId;
            }

            if (this.exceptionKeyValues.Contains(exceptionKey) == true)
            {
                return;
            }

            this.exceptionKeyValues.RwLock.EnterWriteLock();

            if (this.exceptionKeyValues.ValueCache.Contains(exceptionKey) == false)
            {
                this.exceptionKeyValues.ValueCache.Add(exceptionKey);

                this.exceptionKeyValues.RwLock.ExitWriteLock();

                if (exceptionTelemetry == null)
                {
                    exceptionTelemetry = new ExceptionTelemetry(exception);
                    exceptionTelemetry.Context.Operation.Name = operationName;
                    this.telemetryClient.Initialize(exceptionTelemetry);
                }

                StackTrace st = new StackTrace(3, true);
                exceptionTelemetry.SetParsedStack(st.GetFrames());

                if (string.IsNullOrEmpty(exceptionTelemetry.ProblemId) == true)
                {
                    exceptionTelemetry.ProblemId = problemId;
                }

                // this property allows to differentiate examples from regular exceptions tracked using TrackException
                exceptionTelemetry.Properties.Add("_MS.Example", "(Name: Exceptions, Ver: 1.0)");

                ((ISupportSampling)exceptionTelemetry).SamplingPercentage = 100;

                this.telemetryClient.TrackException(exceptionTelemetry);
            }
            else
            {
                this.exceptionKeyValues.RwLock.ExitWriteLock();
            }
        }

        private void ResetDimCapCaches(HashCache<string> cache1, HashCache<string> cache2, HashCache<string> cache3)
        {
            cache1.RwLock.EnterWriteLock();

            if (this.DimCapTimeout < DateTime.UtcNow.Ticks)
            {
                cache2.RwLock.EnterWriteLock();

                if (this.DimCapTimeout < DateTime.UtcNow.Ticks)
                {
                    cache3.RwLock.EnterWriteLock();

                    if (this.DimCapTimeout < DateTime.UtcNow.Ticks)
                    {
                        cache1.ValueCache.Clear();
                        cache2.ValueCache.Clear();
                        cache3.ValueCache.Clear();

                        this.DimCapTimeout = DateTime.UtcNow.Ticks + this.cacheLifetime;
                    }

                    cache3.RwLock.ExitWriteLock();
                }

                cache2.RwLock.ExitWriteLock();
            }

            cache1.RwLock.ExitWriteLock();
        }

        internal class HashCache<T> : IDisposable
        {
            internal ReaderWriterLockSlim RwLock;
            internal HashSet<T> ValueCache = new HashSet<T>();

            private bool disposedValue = false; // To detect redundant calls

            internal HashCache()
            {
                this.RwLock = new ReaderWriterLockSlim();
                this.ValueCache = new HashSet<T>();
            }

            void IDisposable.Dispose()
            {
                this.Dispose(true);
            }

            // This code added to correctly implement the disposable pattern.
            internal void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                this.Dispose(true);
            }

            internal bool Contains(T value)
            {
                bool rc;

                this.RwLock.EnterReadLock();
                rc = this.ValueCache.Contains(value);
                this.RwLock.ExitReadLock();

                return rc;
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!this.disposedValue)
                {
                    if (disposing)
                    {
                        this.RwLock.Dispose();
                    }

                    this.disposedValue = true;
                }
            }
        }
    }
}
#endif