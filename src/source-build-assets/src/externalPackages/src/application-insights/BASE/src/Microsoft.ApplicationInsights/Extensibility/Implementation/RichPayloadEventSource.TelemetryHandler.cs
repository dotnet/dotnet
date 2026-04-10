#if NET452
namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Tracing;
    using System.Linq;
    using System.Reflection;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;

    /// <summary>
    /// Define the telemetry handlers.
    /// Create anonymous type for each AI telemetry data type.
    /// </summary>
    internal sealed partial class RichPayloadEventSource
    {
        private readonly string dummyPartAiKeyValue = string.Empty;
        private readonly long dummyPartAFlagsValue = 0;
        private readonly IDictionary<string, string> dummyPartATagsValue = new Dictionary<string, string>();

        private static void CopyGlobalPropertiesIfRequired(ITelemetry telemetry, IDictionary<string, string> itemProperties)
        {
            // This check avoids accessing the public accessor GlobalProperties
            // unless needed, to avoid the penality of ConcurrentDictionary instantiation.
            if (telemetry.Context.GlobalPropertiesValue != null)
            {
                Utils.CopyDictionary(telemetry.Context.GlobalProperties, itemProperties);
            }
        }

        /// <summary>
        /// Create handlers for all AI telemetry types.
        /// </summary>
        private Dictionary<Type, Action<ITelemetry>> CreateTelemetryHandlers(EventSource eventSource)
        {
            var telemetryHandlers = new Dictionary<Type, Action<ITelemetry>>();

            var eventSourceType = eventSource.GetType();

            // EventSource.Write<T> (String, EventSourceOptions, T)
            var writeGenericMethod = eventSourceType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == "Write" && m.IsGenericMethod == true)
                .Select(m => new { Method = m, Parameters = m.GetParameters() })
                .Where(m => m.Parameters.Length == 3
                            && m.Parameters[0].ParameterType.FullName == "System.String"
                            && m.Parameters[1].ParameterType.FullName == "System.Diagnostics.Tracing.EventSourceOptions"
                            && m.Parameters[2].ParameterType.FullName == null && m.Parameters[2].ParameterType.IsByRef == false)
                .Select(m => m.Method)
                .SingleOrDefault();

            if (writeGenericMethod != null)
            {
                var eventSourceOptionsType = eventSourceType.Assembly.GetType("System.Diagnostics.Tracing.EventSourceOptions");
                var eventSourceOptionsKeywordsProperty = eventSourceOptionsType.GetProperty("Keywords", BindingFlags.Public | BindingFlags.Instance);

                // Request
                telemetryHandlers.Add(typeof(RequestTelemetry), this.CreateHandlerForRequestTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));

                // Trace
                telemetryHandlers.Add(typeof(TraceTelemetry), this.CreateHandlerForTraceTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));

                // Event
                telemetryHandlers.Add(typeof(EventTelemetry), this.CreateHandlerForEventTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));

                // Dependency
                telemetryHandlers.Add(typeof(DependencyTelemetry), this.CreateHandlerForDependencyTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));

                // Metric
                telemetryHandlers.Add(typeof(MetricTelemetry), this.CreateHandlerForMetricTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));

                // Exception
                telemetryHandlers.Add(typeof(ExceptionTelemetry), this.CreateHandlerForExceptionTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));

#pragma warning disable 618
                // PerformanceCounter
                telemetryHandlers.Add(typeof(PerformanceCounterTelemetry), this.CreateHandlerForPerformanceCounterTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));
#pragma warning restore 618

                // PageView
                telemetryHandlers.Add(typeof(PageViewTelemetry), this.CreateHandlerForPageViewTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));

                // PageView
                telemetryHandlers.Add(typeof(PageViewPerformanceTelemetry), this.CreateHandlerForPageViewPerformanceTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));

#pragma warning disable 618
                // SessionState
                telemetryHandlers.Add(typeof(SessionStateTelemetry), this.CreateHandlerForSessionStateTelemetry(eventSource, writeGenericMethod, eventSourceOptionsType, eventSourceOptionsKeywordsProperty));
#pragma warning restore 618
            }
            else
            {
                CoreEventSource.Log.LogVerbose("Unable to get method: EventSource.Write<T>(String, EventSourceOptions, T)");
            }

            return telemetryHandlers;
        }

        /// <summary>
        /// Create a handler for <see cref="ProcessOperationStart(OperationTelemetry)"/> and <see cref="ProcessOperationStop(OperationTelemetry)"/>.
        /// </summary>
        private Action<OperationTelemetry, EventOpcode> CreateOperationStartStopHandler(EventSource eventSource)
        {
            var eventSourceType = eventSource.GetType();

            // EventSource.Write<T> (String, EventSourceOptions, T)
            var writeGenericMethod = eventSourceType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == "Write" && m.IsGenericMethod == true)
                .Select(m => new { Method = m, Parameters = m.GetParameters() })
                .Where(m => m.Parameters.Length == 3
                            && m.Parameters[0].ParameterType.FullName == "System.String"
                            && m.Parameters[1].ParameterType.FullName == "System.Diagnostics.Tracing.EventSourceOptions"
                            && m.Parameters[2].ParameterType.FullName == null && m.Parameters[2].ParameterType.IsByRef == false)
                .Select(m => m.Method)
                .SingleOrDefault();

            if (writeGenericMethod == null)
            {
                return null;
            }

            var eventSourceOptionsType = eventSourceType.Assembly.GetType("System.Diagnostics.Tracing.EventSourceOptions");
            var eventSourceOptionsActivityOptionsProperty = eventSourceOptionsType.GetProperty("ActivityOptions", BindingFlags.Public | BindingFlags.Instance);
            var eventSourceOptionsKeywordsProperty = eventSourceOptionsType.GetProperty("Keywords", BindingFlags.Public | BindingFlags.Instance);
            var eventSourceOptionsOpcodeProperty = eventSourceOptionsType.GetProperty("Opcode", BindingFlags.Public | BindingFlags.Instance);
            var eventSourceOptionsLevelProperty = eventSourceOptionsType.GetProperty("Level", BindingFlags.Public | BindingFlags.Instance);

            var eventActivityOptionsType = eventSourceType.Assembly.GetType("System.Diagnostics.Tracing.EventActivityOptions");
            var eventActivityOptionsRecursive = Enum.Parse(eventActivityOptionsType, "Recursive");

            var eventSourceOptionsStart = Activator.CreateInstance(eventSourceOptionsType);
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptionsStart, Keywords.Operations);
            eventSourceOptionsOpcodeProperty.SetValue(eventSourceOptionsStart, EventOpcode.Start);
            eventSourceOptionsLevelProperty.SetValue(eventSourceOptionsStart, EventLevel.Informational);

            var eventSourceOptionsStop = Activator.CreateInstance(eventSourceOptionsType);
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptionsStop, Keywords.Operations);
            eventSourceOptionsOpcodeProperty.SetValue(eventSourceOptionsStop, EventOpcode.Stop);
            eventSourceOptionsLevelProperty.SetValue(eventSourceOptionsStop, EventLevel.Informational);

            var eventSourceOptionsStartRecursive = Activator.CreateInstance(eventSourceOptionsType);
            eventSourceOptionsActivityOptionsProperty.SetValue(eventSourceOptionsStartRecursive, eventActivityOptionsRecursive);
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptionsStartRecursive, Keywords.Operations);
            eventSourceOptionsOpcodeProperty.SetValue(eventSourceOptionsStartRecursive, EventOpcode.Start);
            eventSourceOptionsLevelProperty.SetValue(eventSourceOptionsStartRecursive, EventLevel.Informational);

            var eventSourceOptionsStopRecursive = Activator.CreateInstance(eventSourceOptionsType);
            eventSourceOptionsActivityOptionsProperty.SetValue(eventSourceOptionsStartRecursive, eventActivityOptionsRecursive);
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptionsStopRecursive, Keywords.Operations);
            eventSourceOptionsOpcodeProperty.SetValue(eventSourceOptionsStopRecursive, EventOpcode.Stop);
            eventSourceOptionsLevelProperty.SetValue(eventSourceOptionsStopRecursive, EventLevel.Informational);

            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                IKey = (string)null,
                Id = (string)null,
                Name = (string)null,
                RootId = (string)null,
            }.GetType());

            return (item, opCode) =>
            {
                bool isRequest = item is RequestTelemetry;

                object eventSourceOptionsObject;
                switch (opCode)
                {
                    case EventOpcode.Start:
                        eventSourceOptionsObject = isRequest ? eventSourceOptionsStart : eventSourceOptionsStartRecursive;
                        break;

                    case EventOpcode.Stop:
                        eventSourceOptionsObject = isRequest ? eventSourceOptionsStop : eventSourceOptionsStopRecursive;
                        break;

                    default:
                        throw new ArgumentException(FormattableString.Invariant($"Unexpected EventOpcode: {opCode}. Expected: Start or Stop."), nameof(opCode));
                }

                var extendedData = new
                {
                    IKey = item.Context.InstrumentationKey,
                    Id = item.Id,
                    Name = item.Name,
                    RootId = item.Context.Operation.Id,
                };

                var parameters = new object[]
                {
                    isRequest ? RequestTelemetry.EtwEnvelopeName : OperationTelemetry.TelemetryName,
                    eventSourceOptionsObject,
                    extendedData,
                };

                writeMethod.Invoke(this.EventSourceInternal, parameters);
            };
        }

        /// <summary>
        /// Create handler for unknown telemetry that accepts EventData, InstrumentationKey, tags, flags.
        /// </summary>
        private Action<EventData, string, IDictionary<string, string>, long> CreateHandlerForUnknownTelemetry(EventSource eventSource)
        {
            var eventSourceType = eventSource.GetType();

            // EventSource.Write<T> (String, EventSourceOptions, T)
            var writeGenericMethod = eventSourceType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => m.Name == "Write" && m.IsGenericMethod == true)
                .Select(m => new { Method = m, Parameters = m.GetParameters() })
                .Where(m => m.Parameters.Length == 3
                            && m.Parameters[0].ParameterType.FullName == "System.String"
                            && m.Parameters[1].ParameterType.FullName == "System.Diagnostics.Tracing.EventSourceOptions"
                            && m.Parameters[2].ParameterType.FullName == null && m.Parameters[2].ParameterType.IsByRef == false)
                .Select(m => m.Method)
                .SingleOrDefault();

            if (writeGenericMethod == null)
            {
                return null;
            }

            var eventSourceOptionsType = eventSourceType.Assembly.GetType("System.Diagnostics.Tracing.EventSourceOptions");
            var eventSourceOptionsKeywordsProperty = eventSourceOptionsType.GetProperty("Keywords", BindingFlags.Public | BindingFlags.Instance);

            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Events;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyEventData = new EventData();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_EventData = new
                {
                    // The properties and layout should be the same as EventData_types.cs
                    dummyEventData.ver,
                    dummyEventData.name,
                    dummyEventData.properties,
                    dummyEventData.measurements,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (data, iKey, tags, flags) =>
             {
                 if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                 {
                     var extendedData = new
                     {
                         // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                         PartA_iKey = iKey,
                         PartA_Tags = tags,
                         PartB_EventData = new
                         {
                             data.ver,
                             data.name,
                             data.properties,
                             data.measurements,
                         },
                         PartA_flags = flags,
                     };

                     writeMethod.Invoke(eventSource, new object[] { EventTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                 }
             };
        }

        /// <summary>
        /// Create handler for request telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForRequestTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Requests;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyRequestData = new RequestData();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_RequestData = new
                {
                    // The properties and layout should be the same as RequestData_types.cs
                    dummyRequestData.ver,
                    dummyRequestData.id,
                    dummyRequestData.source,
                    dummyRequestData.name,
                    dummyRequestData.duration,
                    dummyRequestData.responseCode,
                    dummyRequestData.success,
                    dummyRequestData.url,
                    dummyRequestData.properties,
                    dummyRequestData.measurements,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
                    var telemetryItem = item as RequestTelemetry;                    
                    // This check avoids accessing the public accessor GlobalProperties
                    // unless needed, to avoid the penality of ConcurrentDictionary instantiation.
                    if (item.Context.GlobalPropertiesValue != null)
                    {
                        Utils.CopyDictionary(item.Context.GlobalProperties, telemetryItem.Properties);
                    }

                    item.Sanitize();
                    // Sanitize, Copying global properties is to be done before calling .Data here,
                    // as Data returns a singleton instance, which won't be updated with changes made
                    // after .Data is called.
                    var data = telemetryItem.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_RequestData = new
                        {
                            data.ver,
                            data.id,
                            data.source,
                            data.name,
                            data.duration,
                            data.responseCode,
                            data.success,
                            data.url,
                            data.properties,
                            data.measurements,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { RequestTelemetry.TelemetryName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for trace telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForTraceTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Traces;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyMessageData = new MessageData();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_MessageData = new
                {
                    // The properties and layout should be the same as MessageData_types.cs
                    dummyMessageData.ver,
                    dummyMessageData.message,
                    dummyMessageData.severityLevel,
                    dummyMessageData.properties,
                    dummyMessageData.measurements,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
                    var telemetryItem = item as TraceTelemetry;
                    CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                    item.Sanitize();
                    var data = telemetryItem.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_MessageData = new
                        {
                            data.ver,
                            data.message,
                            data.severityLevel,
                            data.properties,
                            data.measurements,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { TraceTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for event telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForEventTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Events;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyEventData = new EventData();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_EventData = new
                {
                    // The properties and layout should be the same as EventData_types.cs
                    dummyEventData.ver,
                    dummyEventData.name,
                    dummyEventData.properties,
                    dummyEventData.measurements,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
                    var telemetryItem = item as EventTelemetry;
                    CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                    item.Sanitize();
                    var data = telemetryItem.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_EventData = new
                        {
                            data.ver,
                            data.name,
                            data.properties,
                            data.measurements,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { EventTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for dependency telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForDependencyTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Dependencies;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyDependencyData = new RemoteDependencyData();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_RemoteDependencyData = new
                {
                    // The properties and layout should be the same as RemoteDependencyData_types.cs
                    dummyDependencyData.ver,
                    dummyDependencyData.name,
                    dummyDependencyData.id,
                    dummyDependencyData.resultCode,
                    dummyDependencyData.duration,
                    dummyDependencyData.success,
                    dummyDependencyData.data,
                    dummyDependencyData.target,
                    dummyDependencyData.type,
                    dummyDependencyData.properties,
                    dummyDependencyData.measurements,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
                    var telemetryItem = item as DependencyTelemetry;
                    if (item.Context.GlobalPropertiesValue != null)
                    {
                        Utils.CopyDictionary(item.Context.GlobalProperties, telemetryItem.Properties);
                    }

                    item.Sanitize();
                    var data = telemetryItem.InternalData;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_RemoteDependencyData = new
                        {
                            data.ver,
                            data.name,
                            data.id,
                            data.resultCode,
                            data.duration,
                            data.success,
                            data.data,
                            data.target,
                            data.type,
                            data.properties,
                            data.measurements,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { DependencyTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for metric telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForMetricTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Metrics;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyMetricData = new MetricData();
            var dummyDataPoint = new DataPoint();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_MetricData = new
                {
                    // The properties and layout should be the same as MetricData_types.cs
                    dummyMetricData.ver,
                    metrics = new[]
                    {
                        new
                        {
                            // The properties and layout should be the same as DataPoint_types.cs
                            dummyDataPoint.ns,
                            dummyDataPoint.name,
                            dummyDataPoint.kind,
                            dummyDataPoint.value,
                            dummyDataPoint.count,
                            dummyDataPoint.min,
                            dummyDataPoint.max,
                            dummyDataPoint.stdDev,
                        },
                    }.AsEnumerable(),
                    dummyMetricData.properties,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                  
                    var telemetryItem = item as MetricTelemetry;
                    CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                    item.Sanitize();
                    var data = telemetryItem.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_MetricData = new
                        {
                            data.ver,
                            metrics = data.metrics.Select(i => new
                            {
                                i.ns,
                                i.name,
                                i.kind,
                                i.value,
                                i.count,
                                i.min,
                                i.max,
                                i.stdDev,
                            }),
                            data.properties,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { MetricTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for exception telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForExceptionTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Exceptions;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyExceptionData = new ExceptionData();
            var dummyExceptionDetails = new ExceptionDetails();
            var dummyStackFrame = new External.StackFrame();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_ExceptionData = new
                {
                    // The properties and layout should be the same as ExceptionData_types.cs
                    dummyExceptionData.ver,
                    exceptions = new[]
                    {
                        new
                        {
                            // The properties and layout should be the same as ExceptionDetails_types.cs
                            dummyExceptionDetails.id,
                            dummyExceptionDetails.outerId,
                            dummyExceptionDetails.typeName,
                            dummyExceptionDetails.message,
                            dummyExceptionDetails.hasFullStack,
                            dummyExceptionDetails.stack,
                            parsedStack = new[]
                            {
                                new
                                {
                                    // The properties and layout should be the same as StackFrame_types.cs
                                    dummyStackFrame.level,
                                    dummyStackFrame.method,
                                    dummyStackFrame.assembly,
                                    dummyStackFrame.fileName,
                                    dummyStackFrame.line,
                                },
                            }.AsEnumerable(),
                        },
                    }.AsEnumerable(),
                    dummyExceptionData.severityLevel,
                    dummyExceptionData.problemId,
                    dummyExceptionData.properties,
                    dummyExceptionData.measurements,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
                    var telemetryItem = item as ExceptionTelemetry;
                    CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                    item.Sanitize();
                    var data = telemetryItem.Data.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_ExceptionData = new
                        {
                            data.ver,
                            exceptions = data.exceptions.Select(i => new
                            {
                                i.id,
                                i.outerId,
                                i.typeName,
                                i.message,
                                i.hasFullStack,
                                i.stack,
                                parsedStack = i.parsedStack.Select(j => new
                                {
                                    j.level,
                                    j.method,
                                    j.assembly,
                                    j.fileName,
                                    j.line,
                                }),
                            }),
                            data.severityLevel,
                            data.problemId,
                            data.properties,
                            data.measurements,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { ExceptionTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for performance counter telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForPerformanceCounterTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Metrics;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyMetricData = new MetricData();
            var dummyDataPoint = new DataPoint();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_MetricData = new
                {
                    // The properties and layout should be the same as MetricData_types.cs
                    dummyMetricData.ver,
                    metrics = new[]
                    {
                        new
                        {
                            // The properties and layout should be the same as DataPoint_types.cs
                            dummyDataPoint.ns,
                            dummyDataPoint.name,
                            dummyDataPoint.kind,
                            dummyDataPoint.value,
                            dummyDataPoint.count,
                            dummyDataPoint.min,
                            dummyDataPoint.max,
                            dummyDataPoint.stdDev,
                        },
                    }.AsEnumerable(),
                    dummyMetricData.properties,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
#pragma warning disable 618
                    var telemetryItem = (item as PerformanceCounterTelemetry).Data;
                    CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
#pragma warning restore 618
                    item.Sanitize();
                    var data = telemetryItem.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_MetricData = new
                        {
                            data.ver,
                            metrics = data.metrics.Select(i => new
                            {
                                i.ns,
                                i.name,
                                i.kind,
                                i.value,
                                i.count,
                                i.min,
                                i.max,
                                i.stdDev,
                            }),
                            data.properties,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { MetricTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for page view telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForPageViewTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.PageViews;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyPageViewData = new PageViewData();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_PageViewData = new
                {
                    // The properties and layout should be the same as PageViewData_types.cs (EventData_types.cs)
                    dummyPageViewData.url,
                    dummyPageViewData.duration,
                    dummyPageViewData.id,
                    dummyPageViewData.ver,
                    dummyPageViewData.name,
                    dummyPageViewData.properties,
                    dummyPageViewData.measurements,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
                    var telemetryItem = item as PageViewTelemetry;
                    CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                    item.Sanitize();
                    var data = telemetryItem.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_PageViewData = new
                        {
                            data.url,
                            data.duration,
                            data.id,
                            data.ver,
                            data.name,
                            data.properties,
                            data.measurements,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { PageViewTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for page view performance telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForPageViewPerformanceTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.PageViews;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyPageViewPerfData = new PageViewPerfData();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_PageViewPerfData = new
                {
                    // The properties and layout should be the same as PageViewPerfData_types.cs (EventData_types.cs)
                    dummyPageViewPerfData.perfTotal,
                    dummyPageViewPerfData.networkConnect,
                    dummyPageViewPerfData.sentRequest,
                    dummyPageViewPerfData.receivedResponse,
                    dummyPageViewPerfData.domProcessing,
                    dummyPageViewPerfData.url,
                    dummyPageViewPerfData.duration,
                    dummyPageViewPerfData.ver,
                    dummyPageViewPerfData.name,
                    dummyPageViewPerfData.properties,
                    dummyPageViewPerfData.measurements,
                },
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
                    var telemetryItem = item as PageViewTelemetry;
                    CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                    item.Sanitize();
                    var data = telemetryItem.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_PageViewPerfData = new
                        {
                            data.url,
                            data.duration,
                            data.ver,
                            data.name,
                            data.properties,
                            data.measurements,
                        },
                    };

                    writeMethod.Invoke(eventSource, new object[] { PageViewPerformanceTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }

        /// <summary>
        /// Create handler for session state telemetry.
        /// </summary>
        private Action<ITelemetry> CreateHandlerForSessionStateTelemetry(EventSource eventSource, MethodInfo writeGenericMethod, Type eventSourceOptionsType, PropertyInfo eventSourceOptionsKeywordsProperty)
        {
            var eventSourceOptions = Activator.CreateInstance(eventSourceOptionsType);
            var keywords = Keywords.Events;
            eventSourceOptionsKeywordsProperty.SetValue(eventSourceOptions, keywords);
            var dummyEventData = new EventData();
            var writeMethod = writeGenericMethod.MakeGenericMethod(new
            {
                PartA_iKey = this.dummyPartAiKeyValue,
                PartA_Tags = this.dummyPartATagsValue,
                PartB_EventData = new
                {
                    // The properties and layout should be the same as EventData_types.cs
                    dummyEventData.ver,
                    dummyEventData.name,
                    dummyEventData.properties,
                    dummyEventData.measurements,
                },
                PartA_flags = this.dummyPartAFlagsValue,
            }.GetType());

            return (item) =>
            {
                if (this.EventSourceInternal.IsEnabled(EventLevel.Verbose, keywords))
                {                    
#pragma warning disable 618
                    var telemetryItem = (item as SessionStateTelemetry).Data;
#pragma warning restore 618
                    CopyGlobalPropertiesIfRequired(item, telemetryItem.Properties);
                    item.Sanitize();
                    var data = telemetryItem.Data;
                    var extendedData = new
                    {
                        // The properties and layout should be the same as the anonymous type in the above MakeGenericMethod
                        PartA_iKey = telemetryItem.Context.InstrumentationKey,
                        PartA_Tags = telemetryItem.Context.SanitizedTags,
                        PartB_EventData = new
                        {
                            data.ver,
                            data.name,
                            data.properties,
                            data.measurements,
                        },
                        PartA_flags = telemetryItem.Context.Flags,
                    };

                    writeMethod.Invoke(eventSource, new object[] { EventTelemetry.EtwEnvelopeName, eventSourceOptions, extendedData });
                }
            };
        }
    }
}
#endif