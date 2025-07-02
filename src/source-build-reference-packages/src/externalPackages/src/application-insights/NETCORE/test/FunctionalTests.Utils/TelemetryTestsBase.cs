namespace FunctionalTests.Utils
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using AI;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using Xunit.Abstractions;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector;

    public abstract class TelemetryTestsBase
    {
        public const int TestListenerTimeoutInMs = 10000;
        protected readonly ITestOutputHelper output;
        
        public TelemetryTestsBase(ITestOutputHelper output)
        {
            this.output = output;
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public TelemetryItem<RequestData> ValidateBasicRequest(InProcessServer server, string requestPath, RequestTelemetry expected, bool expectRequestContextInResponse = true)
        {
            return ValidateRequestWithHeaders(server, requestPath, null, expected, expectRequestContextInResponse);
        }

        [MethodImpl(MethodImplOptions.NoOptimization)]
        public TelemetryItem<RequestData> ValidateRequestWithHeaders(InProcessServer server, string requestPath, Dictionary<string, string> requestHeaders, RequestTelemetry expected, bool expectRequestContextInResponse = true)
        {
            // Subtract 50 milliseconds to hack around strange behavior on build server where the RequestTelemetry.Timestamp is somehow sometimes earlier than now by a few milliseconds.
            expected.Timestamp = DateTimeOffset.Now.Subtract(TimeSpan.FromMilliseconds(50));
            Stopwatch timer = Stopwatch.StartNew();

            var response = this.ExecuteRequest(server.BaseHost + requestPath, requestHeaders);

            var actual = server.Listener.ReceiveItemsOfType<TelemetryItem<RequestData>>(1, TestListenerTimeoutInMs);
            timer.Stop();

            this.DebugTelemetryItems(actual);
            this.output.WriteLine("Response headers: " + string.Join(",", response.Headers.Select(kvp => $"{kvp.Key} = {kvp.Value.First()}")));

            var item = actual.OfType<TelemetryItem<RequestData>>().FirstOrDefault();
            Assert.NotNull(item);
            var data = ((TelemetryItem<RequestData>)item).data.baseData;

            Assert.Equal(expected.ResponseCode, data.responseCode);
            Assert.Equal(expected.Name, data.name);
            Assert.Equal(expected.Success, data.success);
            Assert.Equal(expected.Url, new Uri(data.url));
            Assert.Equal(expectRequestContextInResponse, response.Headers.Contains("Request-Context"));
            if (expectRequestContextInResponse)
            {
                Assert.True(response.Headers.TryGetValues("Request-Context", out var appIds));
                Assert.Equal($"appId={InProcessServer.AppId}", appIds.Single());
            }

            output.WriteLine("actual.Duration: " + data.duration);
            output.WriteLine("timer.Elapsed: " + timer.Elapsed);
            Assert.True(TimeSpan.Parse(data.duration) < timer.Elapsed.Add(TimeSpan.FromMilliseconds(20)), "duration");

            return item;
        }

        public void ValidateBasicException(InProcessServer server, string requestPath, ExceptionTelemetry expected)
        {
            this.ExecuteRequest(server.BaseHost + requestPath);

            var actual = server.Listener.ReceiveItemsOfType<TelemetryItem<ExceptionData>>(1, TestListenerTimeoutInMs);
            this.DebugTelemetryItems(actual);
            
            var item = actual.OfType<TelemetryItem<ExceptionData>>().FirstOrDefault();
            Assert.NotNull(item);
            var data = ((TelemetryItem<ExceptionData>)item).data.baseData;

            Assert.Equal(expected.Exception.GetType().ToString(), data.exceptions[0].typeName);
            Assert.NotEmpty(data.exceptions[0].parsedStack);
        }

        public (TelemetryItem<RequestData>, TelemetryItem<RemoteDependencyData>) ValidateBasicDependency(InProcessServer server, string requestPath, DependencyTelemetry expected)
        {
            var response = this.ExecuteRequest(server.BaseHost + requestPath);

            var actual = server.Listener.ReceiveItems(TestListenerTimeoutInMs);
            this.DebugTelemetryItems(actual);

            var dependencyTelemetry = actual.OfType<TelemetryItem<RemoteDependencyData>>().FirstOrDefault();
            Assert.NotNull(dependencyTelemetry);
            var dependencyData = ((TelemetryItem<RemoteDependencyData>)dependencyTelemetry).data.baseData;
            Assert.Equal(expected.Data, dependencyData.data);
            Assert.Equal(expected.ResultCode, dependencyData.resultCode);
            Assert.Equal(expected.Success, dependencyData.success);

            var requestTelemetry = actual.OfType<TelemetryItem<RequestData>>().FirstOrDefault();
            Assert.NotNull(requestTelemetry);

            Assert.Equal(requestTelemetry.tags["ai.operation.id"], dependencyTelemetry.tags["ai.operation.id"]);
            Assert.Contains(dependencyTelemetry.data.baseData.id, requestTelemetry.tags["ai.operation.parentId"]);

            return (requestTelemetry, dependencyTelemetry);
        }

        public void ValidatePerformanceCountersAreCollected(string assemblyName)
        {
            using (var server = new InProcessServer(assemblyName, this.output))
            {
                // Reconfigure the PerformanceCollectorModule timer.
                Type perfModuleType = typeof(PerformanceCollectorModule);
                PerformanceCollectorModule perfModule = (PerformanceCollectorModule)server.ApplicationServices.GetServices<ITelemetryModule>().FirstOrDefault(m => m.GetType() == perfModuleType);
                FieldInfo timerField = perfModuleType.GetField("timer", BindingFlags.NonPublic | BindingFlags.Instance);
                var timer = timerField.GetValue(perfModule);
                timerField.FieldType.InvokeMember("ScheduleNextTick", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, timer, new object[] { TimeSpan.FromMilliseconds(10) });

                var actual = server.Listener.ReceiveItems(TestListenerTimeoutInMs);
                this.DebugTelemetryItems(actual);
                Assert.True(actual.Length > 0);
            }
        }

        protected HttpResponseMessage ExecuteRequest(string requestPath, Dictionary<string, string> headers = null)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseDefaultCredentials = true;

            using (HttpClient httpClient = new HttpClient(httpClientHandler, true))
            {
                this.output.WriteLine($"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt}: Executing request: {requestPath}");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestPath);
                if (headers != null)
                {
                    foreach (var h in headers)
                    {
                        request.Headers.Add(h.Key, h.Value);
                    }
                }

                var task = httpClient.SendAsync(request);
                task.Wait(TestListenerTimeoutInMs);
                this.output.WriteLine($"{DateTime.Now:MM/dd/yyyy hh:mm:ss.fff tt}: Ended request: {requestPath}");

                return task.Result;
            }
        }

        protected void DebugTelemetryItems(Envelope[] telemetries)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Envelope telemetry in telemetries)
            {
                TelemetryItem<RemoteDependencyData> dependency = telemetry as TelemetryItem<RemoteDependencyData>;
                if (dependency != null)
                {                    
                    var data = ((TelemetryItem<RemoteDependencyData>)dependency).data.baseData;
                    builder.AppendLine($"{dependency} - {data.data} - {((TelemetryItem<RemoteDependencyData>)dependency).time} - {data.duration} - {data.id} - {data.name} - {data.resultCode} - {data.success} - {data.target} - {data.type}");
                }
                else
                {
                    TelemetryItem<RequestData> request = telemetry as TelemetryItem<RequestData>;
                    if (request != null)
                    {
                        var data = ((TelemetryItem<RequestData>)request).data.baseData;
                        builder.AppendLine($"{request} - {data.url} - {((TelemetryItem<RequestData>)request).time} - {data.duration} - {data.id} - {data.name} - {data.success} - {data.responseCode}");
                    }
                    else
                    {
                        TelemetryItem<ExceptionData> exception = telemetry as TelemetryItem<ExceptionData>;
                        if (exception != null)
                        {
                            var data = ((TelemetryItem<ExceptionData>)exception).data.baseData;
                            builder.AppendLine($"{exception} - {data.exceptions[0].message} - {data.exceptions[0].stack} - {data.exceptions[0].typeName} - {data.severityLevel}");
                        }
                        else
                        {
                            TelemetryItem<MessageData> message = telemetry as TelemetryItem<MessageData>;
                            if (message != null)
                            {
                                var data = ((TelemetryItem<MessageData>)message).data.baseData;
                                builder.AppendLine($"{message} - {data.message} - {data.severityLevel}");
                            }
                            else
                            {
                                TelemetryItem<MetricData> metric = telemetry as TelemetryItem<MetricData>;
                                if (metric != null)
                                {
                                    var data = ((TelemetryItem<MetricData>)metric).data.baseData;
                                    builder.AppendLine($"{metric.ToString()} - {metric.data}- {metric.name} - {data.metrics.Count}");
                                    foreach (var metricVal in data.metrics)
                                    {
                                        builder.AppendLine($"{metricVal.name} {metricVal.value}");
                                    }
                                }     
                                else
                                {
                                    builder.AppendLine($"{telemetry.ToString()} - {telemetry.time}");
                                }
                            }
                        }
                    }
                }
            }

            this.output.WriteLine(builder.ToString());
        }
    }
}