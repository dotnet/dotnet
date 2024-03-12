using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.ApplicationInsights.WorkerService.Tests
{
    public class FunctionalTests
    {
        protected readonly ITestOutputHelper output;

        public FunctionalTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact(Skip = "Temporarily skipping. This fails when run inside same process as DepCollector ignores the 2nd host on the same process. validated locally.")]
        public void BasicCollectionTest()
        {
            ConcurrentBag<ITelemetry> sentItems = new ConcurrentBag<ITelemetry>();

            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ITelemetryChannel>(new StubChannel()
                    {
                        OnSend = (item) => sentItems.Add(item)
                    }); ;
                    services.AddApplicationInsightsTelemetryWorkerService("ikey");
                    services.AddHostedService<Worker>();
                }).Build();

            host.Start();

            // Run the worker for ~5 secs.
            Task.Delay(5000).Wait();

            host.StopAsync();

            // The worker would have completed 5 loops in 5 sec,
            // each look making dependency call and some ilogger logs,
            // inside "myoperation"
            Assert.True(sentItems.Count > 0);
            PrintItems(sentItems);

            // Validate
            var reqs = GetTelemetryOfType<RequestTelemetry>(sentItems);
            Assert.True(reqs.Count >= 1);
            var traces = GetTelemetryOfType<TraceTelemetry>(sentItems);
            Assert.True(traces.Count >= 1);
            var deps = GetTelemetryOfType<DependencyTelemetry>(sentItems);
            Assert.True(deps.Count >= 1);

            // Pick one RequestTelemetry and validate that trace/deps are found which are child of the parent request.
            var reqOperationId = reqs[0].Context.Operation.Id;
            var reqId = reqs[0].Id;
            var trace = traces.Find((tr) => tr.Context.Operation.Id != null && tr.Context.Operation.Id.Equals(reqOperationId));
            Assert.NotNull(trace);
            trace = traces.Find((tr) => tr.Context.Operation.ParentId != null && tr.Context.Operation.ParentId.Equals(reqId));
            Assert.NotNull(trace);

            var dep = deps.Find((de) => de.Context.Operation.Id.Equals(reqOperationId));
            Assert.NotNull(dep);
            dep = deps.Find((de) => de.Context.Operation.ParentId.Equals(reqId));
            Assert.NotNull(dep);
        }

        private List<T> GetTelemetryOfType<T>(ConcurrentBag<ITelemetry> items)
        {
            List<T> foundItems = new List<T>();
            foreach (var item in items)
            {
                if (item is T)
                {
                    foundItems.Add((T)item);
                }
            }

            return foundItems;
        }

        private void PrintItems(ConcurrentBag<ITelemetry> items)
        {
            int i = 1;
            foreach (var item in items)
            {
                this.output.WriteLine("Item " + (i++) + ".");

                if (item is RequestTelemetry req)
                {
                    this.output.WriteLine("RequestTelemetry");
                    this.output.WriteLine(req.Name);
                    this.output.WriteLine(req.Id);
                    PrintOperation(item);
                    this.output.WriteLine(req.Duration.ToString());
                }
                else if (item is DependencyTelemetry dep)
                {
                    this.output.WriteLine("DependencyTelemetry");
                    this.output.WriteLine(dep.Name);
                    this.output.WriteLine(dep.Data);
                    PrintOperation(item);
                }
                else if (item is TraceTelemetry trace)
                {
                    this.output.WriteLine("TraceTelemetry");
                    this.output.WriteLine(trace.Message);
                    PrintOperation(item);
                }
                else if (item is ExceptionTelemetry exc)
                {
                    this.output.WriteLine("ExceptionTelemetry");
                    this.output.WriteLine(exc.Message);
                    PrintOperation(item);
                }
                else if (item is MetricTelemetry met)
                {
                    this.output.WriteLine("MetricTelemetry");
                    this.output.WriteLine(met.Name + "" + met.Sum);
                    PrintOperation(item);
                }

                PrintProperties(item as ISupportProperties);
                this.output.WriteLine("----------------------------");
            }
        }

        private void PrintProperties(ISupportProperties itemProps)
        {
            foreach (var prop in itemProps.Properties)
            {
                this.output.WriteLine(prop.Key + ":" + prop.Value);
            }
        }

        private void PrintOperation(ITelemetry item)
        {
            if(item.Context.Operation.Id != null)
            this.output.WriteLine(item.Context.Operation.Id);
            if(item.Context.Operation.ParentId != null)
            this.output.WriteLine(item.Context.Operation.ParentId);
        }
    }

    internal class StubChannel : ITelemetryChannel
    {
        public Action<ITelemetry> OnSend = t => { };

        public string EndpointAddress
        {
            get;
            set;
        }

        public bool? DeveloperMode { get; set; }

        public void Dispose()
        {
        }

        public void Flush()
        {
        }

        public void Send(ITelemetry item)
        {
            this.OnSend(item);
        }
    }

    

}
