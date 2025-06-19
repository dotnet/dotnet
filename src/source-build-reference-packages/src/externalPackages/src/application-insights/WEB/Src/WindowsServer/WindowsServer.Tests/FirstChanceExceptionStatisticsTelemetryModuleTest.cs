#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;
    using DataContracts;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Web.TestFramework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestFramework;
    using Assert = Xunit.Assert;

    [TestClass]
    public class FirstChanceExceptionStatisticsTelemetryModuleTest : IDisposable
    {
        internal static readonly string TestOperationName = "FirstChanceExceptionTestOperation";

        private TelemetryConfiguration configuration;
        private ConcurrentBag<ITelemetry> items;

        [TestInitialize]
        public void TestInitialize()
        {
            this.items = new ConcurrentBag<ITelemetry>();

            this.configuration = new TelemetryConfiguration();

            this.configuration.TelemetryChannel = new StubTelemetryChannel
            {
                OnSend = telemetry => this.items.Add(telemetry),
                EndpointAddress = "http://test.com"
            };
            this.configuration.InstrumentationKey = "MyKey";
        }

        [TestCleanup]
        public void TestCleanup()
        {
            this.configuration = null;
            this.items = new ConcurrentBag<ITelemetry>();
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleDoNotThrowOnNullException()
        {
            EventHandler<FirstChanceExceptionEventArgs> handler = null;
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule(
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.configuration);
                handler.Invoke(null, new FirstChanceExceptionEventArgs(null));
            }
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleDoNotCauseStackOverflow()
        {
            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    throw new Exception("this exception may cause stack overflow as will be thrown during the processing of another exception");
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

                try
                {
                    // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                    throw new Exception("test");
                }
                catch (Exception exc)
                {
                    // make sure it is the same exception as was initially thrown
                    Assert.Equal("test", exc.Message);
                }
            }
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleTracksMetricWithTypeAndMethodOnException()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                try
                {
                    // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                    throw new Exception("test");
                }
                catch (Exception exc)
                {
                    // code to prevent compiler optimizations
                    Assert.Equal("test", exc.Message);
                }
            }

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            var dims = metrics[0].Key.Dimensions;
            Assert.Equal(1, dims.Count);

            Assert.StartsWith(typeof(Exception).FullName, dims["problemId"], StringComparison.Ordinal);

            int nameStart = dims["problemId"].IndexOf(" at ", StringComparison.OrdinalIgnoreCase) + 4;

            Assert.StartsWith(typeof(FirstChanceExceptionStatisticsTelemetryModuleTest).FullName + "." + nameof(this.FirstChanceExceptionStatisticsTelemetryModuleTracksMetricWithTypeAndMethodOnException), dims["problemId"].Substring(nameStart), StringComparison.Ordinal);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleUsesSetsInternalOperationName()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = TestOperationName;
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                SdkInternalOperationsMonitor.Enter();
                try
                {
                    try
                    {
                        // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                        throw new Exception("test");
                    }
                    catch (Exception exc)
                    {
                        // code to prevent profiler optimizations
                        Assert.Equal("test", exc.Message);
                    }
                }
                finally
                {
                    SdkInternalOperationsMonitor.Exit();
                }
            }

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            var dims = metrics[0].Key.Dimensions;
            Assert.Equal(2, dims.Count);

            Assert.True(dims.Contains(new KeyValuePair<string, string>(FirstChanceExceptionStatisticsTelemetryModule.OperationNameTag, "AI (Internal)")));
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleUsesOperationNameAsDimension()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = TestOperationName;
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                try
                {
                    // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                    throw new Exception("test");
                }
                catch (Exception exc)
                {
                    // code to prevent profiler optimizations
                    Assert.Equal("test", exc.Message);
                }
            }

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            var dims = metrics[0].Key.Dimensions;
            Assert.Equal(2, dims.Count);

            Assert.True(dims.Contains(new KeyValuePair<string, string>(FirstChanceExceptionStatisticsTelemetryModule.OperationNameTag, TestOperationName)));
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleMarksOperationAsInternal()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                try
                {
                    SdkInternalOperationsMonitor.Enter();

                    // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                    throw new Exception("test");
                }
                catch (Exception exc)
                {
                    // code to prevent profiler optimizations
                    Assert.Equal("test", exc.Message);
                }
                finally
                {
                    SdkInternalOperationsMonitor.Exit();
                }
            }

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            var dims = metrics[0].Key.Dimensions;
            Assert.Equal(2, dims.Count);

            string operationName;
            Assert.True(dims.TryGetValue(FirstChanceExceptionStatisticsTelemetryModule.OperationNameTag, out operationName));
            Assert.Equal("AI (Internal)", operationName);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleWillDimCapOperationName()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            int operationId = 0;

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = "operationName " + (operationId++);
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                for (int i = 0; i < 200; i++)
                {
                    try
                    {
                        // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                        throw new Exception("test");
                    }
                    catch (Exception exc)
                    {
                        // code to prevent profiler optimizations
                        Assert.Equal("test", exc.Message);
                    }
                }
            }

            Assert.Equal(200, metrics.Count);
            Assert.Equal(204, this.items.Count);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleWillNotDimCapTheSameOperationName()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = TestOperationName;
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                for (int i = 0; i < 200; i++)
                {
                    try
                    {
                        // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                        throw new Exception("test");
                    }
                    catch (Exception exc)
                    {
                        // code to prevent profiler optimizations
                        Assert.Equal("test", exc.Message);
                    }
                }
            }

            Assert.Equal(200, metrics.Count);
            Assert.Equal(2, this.items.Count);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleWillDimCapAfterCacheTimeout()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            int operationId = 0;

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = "operationName " + (operationId++);
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                module.DimCapTimeout = DateTime.UtcNow.Ticks - 1;

                for (int i = 0; i < 200; i++)
                {
                    if (i == 101)
                    {
                        module.DimCapTimeout = DateTime.UtcNow.Ticks - 1;
                    }

                    try
                    {
                        // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                        throw new Exception("test");
                    }
                    catch (Exception exc)
                    {
                        // code to prevent profiler optimizations
                        Assert.Equal("test", exc.Message);
                    }
                }
            }

            Assert.Equal(200, metrics.Count);
            Assert.Equal(400, this.items.Count);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryExceptionsAreThrottled()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            this.configuration.TelemetryInitializers.Add(new StubTelemetryInitializer()
            {
                OnInitialize = (item) =>
                {
                    item.Context.Operation.Name = TestOperationName;
                }
            });

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                module.TargetMovingAverage = 50;

                for (int i = 0; i < 200; i++)
                {
                    try
                    {
                        // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                        throw new Exception("test");
                    }
                    catch (Exception exc)
                    {
                        // code to prevent profiler optimizations
                        Assert.Equal("test", exc.Message);
                    }
                }
            }

            int countProcessed = 0;
            int countThrottled = 0;

            foreach (KeyValuePair<Metric, double> items in metrics)
            {
                if (items.Key.Dimensions.Count == 1)
                {
                    countThrottled++;
                }
                else
                {
                    countProcessed++;
                }
            }

            // The test starts with the current moving average being 0. With the setting of the
            // weight for the new sample being .3 and the target moving average being 50 (as
            // set in the test), this means 50 / .3 = 166 becomes the throttle limit for this window.
            Assert.Equal(166, countProcessed);
            Assert.Equal(34, countThrottled);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleDoNotIncrementOnRethrow()
        {
            var metrics = new List<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                try
                {
                    try
                    {
                        // FirstChanceExceptionStatisticsTelemetryModule will process this exception
                        throw new Exception("test");
                    }
                    catch (Exception ex)
                    {
                        // this assert is neede to avoid code optimization
                        Assert.Equal("test", ex.Message);
                        throw;
                    }
                }
                catch (Exception exc)
                {
                    Assert.Equal("test", exc.Message);
                }
            }

            Assert.Single(metrics);
            Assert.Equal("Exceptions thrown", metrics[0].Key.Name);

            Assert.Equal(1, metrics[0].Value, 15);

            Assert.Equal(2, this.items.Count);

            ITelemetry[] testItems = this.items.ToArray();

            foreach (ITelemetry i in testItems)
            {
                if (i is MetricTelemetry)
                {
                    Assert.Equal(1, ((MetricTelemetry)i).Count);
                }
            }
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleThrowFromTaskAsync()
        {
            var metrics = new ConcurrentBag<KeyValuePair<Metric, double>>();
            StubMetricProcessor stub = new StubMetricProcessor()
            {
                OnTrack = (m, v) =>
                {
                    metrics.Add(new KeyValuePair<Metric, double>(m, v));
                }
            };

            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);
                module.MetricManager.MetricProcessors.Add(stub);

                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        Task<int> task1 = Task<int>.Factory.StartNew(() => this.Method5(0));
                        Task<int> task2 = Task<int>.Factory.StartNew(() => this.Method4(0));

                        try
                        {
                            task1.Wait();
                        }
                        catch (Exception)
                        {
                            throw;
                        }

                        try
                        {
                            task2.Wait();
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            Assert.Equal(30, metrics.Count);

            foreach (KeyValuePair<Metric, double> m in metrics)
            {
                Assert.Equal("Exceptions thrown", m.Key.Name);
                Assert.Equal(1, m.Value, 15);
            }

            // There should be 3 telemetry items and 3 metric items
            Assert.Equal(6, this.items.Count);

            int numMetricTelemetry = 0;
            int numExceptionTelemetry = 0;

            foreach (var i in this.items)
            {
                if (i is MetricTelemetry)
                {
                    numMetricTelemetry++;
                }

                if (i is ExceptionTelemetry)
                {
                    numExceptionTelemetry++;
                }
            }

            Assert.Equal(3, numMetricTelemetry);
            Assert.Equal(3, numExceptionTelemetry);
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleWasTrackedReturnsTrueForTheSameException()
        {
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new Exception();

                Assert.False(FirstChanceExceptionStatisticsTelemetryModule.WasExceptionTracked(exception));
                Assert.True(FirstChanceExceptionStatisticsTelemetryModule.WasExceptionTracked(exception));
                Assert.True(FirstChanceExceptionStatisticsTelemetryModule.WasExceptionTracked(exception));
                Assert.True(FirstChanceExceptionStatisticsTelemetryModule.WasExceptionTracked(exception));
            }
        }

        //// This test is temporarily removed until WasExceptionTracked method is enhanced.
        ////[TestMethod]
        ////public void FirstChanceExceptionStatisticsTelemetryModuleWasTrackedReturnsTrueForInnerException()
        ////{
        ////    using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
        ////    {
        ////        module.Initialize(this.configuration);

        ////        var exception = new Exception();

        ////        Assert.False(module.WasExceptionTracked(exception));

        ////        var wrapper = new Exception("wrapper", exception);

        ////        Assert.True(module.WasExceptionTracked(wrapper));
        ////        Assert.True(module.WasExceptionTracked(wrapper));
        ////    }
        ////}

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleWasTrackedReturnsFalseForInnerExceptionTwoLevelsUp()
        {
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new Exception();

                Assert.False(FirstChanceExceptionStatisticsTelemetryModule.WasExceptionTracked(exception));

                var wrapper1 = new Exception("wrapper 1", exception);
                var wrapper2 = new Exception("wrapper 2", wrapper1);

                Assert.False(FirstChanceExceptionStatisticsTelemetryModule.WasExceptionTracked(wrapper2));
            }
        }

        //// Temporarily removing the test until WasExceptionTracked is refined.
        ////[TestMethod]
        ////public void FirstChanceExceptionStatisticsTelemetryModuleWasTrackedReturnsTrueForAggExc()
        ////{
        ////    using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
        ////    {
        ////        module.Initialize(this.configuration);

        ////        var exception = new Exception();

        ////        Assert.False(module.WasExceptionTracked(exception));

        ////        var aggExc = new AggregateException(exception);
        ////        Assert.True(module.WasExceptionTracked(aggExc));
        ////    }
        ////}

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryModuleWasTrackedReturnsFalseForAggExcWithNotTrackedInnerExceptions()
        {
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

                var exception = new Exception();

                var aggExc = new AggregateException(exception);
                Assert.False(FirstChanceExceptionStatisticsTelemetryModule.WasExceptionTracked(aggExc));
            }
        }

        [TestMethod]
        public void FirstChanceExceptionStatisticsTelemetryCallMultipleMethods()
        {
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule())
            {
                module.Initialize(this.configuration);

                for (int i = 0; i < 200; i++)
                {
                    try
                    {
                        this.Method1();
                    }
                    catch
                    {
                    }

                    try
                    {
                        throw new Exception("New exception from Method1");
                    }
                    catch
                    {
                    }
                }
            }

            // There should be three unique TrackExceptions and three Metrics
            Assert.Equal(6, this.items.Count);
        }

        [TestMethod]
        public void ModuleConstructorCallsRegister()
        {
            EventHandler<FirstChanceExceptionEventArgs> handler = null;
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule(
                h => handler = h,
                _ => { }))
            {
                module.Initialize(this.configuration);
            }

            Assert.NotNull(handler);
        }

        [TestMethod]
        public void DisposeCallsUnregister()
        {
            EventHandler<FirstChanceExceptionEventArgs> handler = null;
            using (var module = new FirstChanceExceptionStatisticsTelemetryModule(
                _ => { },
                h => handler = h))
            {
                module.Initialize(this.configuration);
            }

            Assert.NotNull(handler);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.configuration != null)
                {
                    this.configuration.Dispose();
                }
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Method1()
        {
            try
            {
                this.Method2();
            }
            catch
            {
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Method2()
        {
            try
            {
                this.Method3();
            }
            catch
            {
                throw new Exception("exception from Method 2");
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Method3()
        {
            throw new Exception("exception from Method 3");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int Method4(int value)
        {
            try
            {
                int x = 1 / value;

                return x;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int Method5(int value)
        {
            try
            {
                int x = 1 / value;

                return x;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

    [SuppressMessage("Microsoft.StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "This class is for the temporary MetricManager")]
    internal class StubMetricProcessor : IMetricProcessor
    {
        public Action<Metric, double> OnTrack = (metric, value) => { };

        public void Track(Metric metric, double value)
        {
            this.OnTrack(metric, value);
        }
    }
}
#endif