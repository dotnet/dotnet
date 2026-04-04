// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Trace;

namespace OpenTelemetry.Instrumentation.Hangfire.Implementation;

internal sealed class HangfireInstrumentationJobFilterAttribute : JobFilterAttribute, IServerFilter, IClientFilter
{
    private readonly HangfireInstrumentationOptions options;

#pragma warning disable CA1019 // Define accessors for attribute arguments
    public HangfireInstrumentationJobFilterAttribute(HangfireInstrumentationOptions options)
#pragma warning restore CA1019 // Define accessors for attribute arguments
    {
        this.options = options;
    }

    public void OnPerforming(PerformingContext performingContext)
    {
        // Short-circuit if nobody is listening
        if (!HangfireInstrumentation.ActivitySource.HasListeners())
        {
            return;
        }

        var activityContextData = performingContext.GetJobParameter<Dictionary<string, string>>(HangfireInstrumentationConstants.ActivityContextKey);
        ActivityContext parentContext = default;
        if (activityContextData is not null)
        {
            var propagationContext = Propagators.DefaultTextMapPropagator.Extract(default, activityContextData, ExtractActivityProperties);
            parentContext = propagationContext.ActivityContext;
            Baggage.Current = propagationContext.Baggage;
        }

        var activity = HangfireInstrumentation.ActivitySource
            .StartActivity(HangfireInstrumentationConstants.ActivityName, ActivityKind.Internal, parentContext);

        if (activity != null)
        {
            var displayNameFunc = this.options.DisplayNameFunc;

            activity.DisplayName = displayNameFunc(performingContext.BackgroundJob);

            if (activity.IsAllDataRequested)
            {
                try
                {
                    if (this.options.Filter?.Invoke(performingContext.BackgroundJob) == false)
                    {
                        activity.IsAllDataRequested = false;
                        activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
                        return;
                    }
                }
                catch (Exception)
                {
                    activity.IsAllDataRequested = false;
                    activity.ActivityTraceFlags &= ~ActivityTraceFlags.Recorded;
                    return;
                }

                activity.SetTag(HangfireInstrumentationConstants.JobIdTag, performingContext.BackgroundJob.Id);
                activity.SetTag(HangfireInstrumentationConstants.JobCreatedAtTag, performingContext.BackgroundJob.CreatedAt.ToString("O"));
            }

            performingContext.Items.Add(HangfireInstrumentationConstants.ActivityKey, activity);
        }
    }

    public void OnPerformed(PerformedContext performedContext)
    {
        // Short-circuit if nobody is listening
        if (!HangfireInstrumentation.ActivitySource.HasListeners() || !performedContext.Items.TryGetValue(HangfireInstrumentationConstants.ActivityKey, out var value))
        {
            return;
        }

        if (value is Activity activity)
        {
            if (performedContext.Exception != null)
            {
                this.SetStatusAndRecordException(activity, performedContext.Exception);
            }

            activity.Dispose();
        }
    }

    public void OnCreating(CreatingContext creatingContext)
    {
        // Short-circuit if nobody is listening
        if (!HangfireInstrumentation.ActivitySource.HasListeners())
        {
            return;
        }

        var activity = Activity.Current;
        if (activity == null)
        {
            return;
        }

        var activityContextData = new Dictionary<string, string>();
        Propagators.DefaultTextMapPropagator.Inject(new PropagationContext(activity.Context, Baggage.Current), activityContextData, InjectActivityProperties);
        creatingContext.SetJobParameter(HangfireInstrumentationConstants.ActivityContextKey, activityContextData);
    }

    public void OnCreated(CreatedContext filterContext)
    {
    }

    private static void InjectActivityProperties(IDictionary<string, string> jobParams, string key, string value)
    {
        jobParams[key] = value;
    }

    private static IEnumerable<string> ExtractActivityProperties(Dictionary<string, string> telemetryData, string key)
    {
        return telemetryData.TryGetValue(key, out var value) ? [value] : [];
    }

    private void SetStatusAndRecordException(Activity activity, Exception exception)
    {
        activity.SetStatus(ActivityStatusCode.Error, exception.Message);

        if (this.options.RecordException)
        {
            activity.AddException(exception);
        }
    }
}
