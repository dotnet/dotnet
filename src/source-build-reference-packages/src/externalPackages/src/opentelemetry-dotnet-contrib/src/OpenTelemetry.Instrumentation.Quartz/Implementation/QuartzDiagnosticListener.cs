// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

using System.Diagnostics;
using System.Reflection;
using OpenTelemetry.Internal;

namespace OpenTelemetry.Instrumentation.Quartz.Implementation;

internal sealed class QuartzDiagnosticListener : ListenerHandler
{
    internal static readonly Assembly Assembly = typeof(QuartzDiagnosticListener).Assembly;
    internal static readonly AssemblyName AssemblyName = Assembly.GetName();
    internal static readonly string ActivitySourceName = AssemblyName.Name!;
    internal static readonly ActivitySource ActivitySource = new(ActivitySourceName, Assembly.GetPackageVersion());
    internal readonly PropertyFetcher<object> JobDetailsPropertyFetcher = new("JobDetail");

    private readonly QuartzInstrumentationOptions options;

    public QuartzDiagnosticListener(string sourceName, QuartzInstrumentationOptions options)
        : base(sourceName)
    {
        Guard.ThrowIfNull(options);

        this.options = options;
    }

    public override void OnEventWritten(string name, object? payload)
    {
        var activity = Activity.Current;
        Guard.ThrowIfNull(activity);
        switch (name)
        {
            case "Quartz.Job.Execute.Start":
            case "Quartz.Job.Veto.Start":
                this.OnStartActivity(activity, payload);
                break;
            case "Quartz.Job.Execute.Stop":
            case "Quartz.Job.Veto.Stop":
                this.OnStopActivity(activity, payload);
                break;
            case "Quartz.Job.Execute.Exception":
            case "Quartz.Job.Veto.Exception":
                this.OnException(activity, payload);
                break;
            default:
                break;
        }
    }

    private static string GetDisplayName(Activity activity)
    {
        return activity.OperationName switch
        {
            OperationName.Job.Execute => $"execute {GetTag(activity.Tags, TagName.JobName)}",
            OperationName.Job.Veto => $"veto {GetTag(activity.Tags, TagName.JobName)}",
            _ => activity.DisplayName,
        };
    }

    private static ActivityKind GetActivityKind(Activity activity)
    {
        return activity.OperationName switch
        {
            OperationName.Job.Execute => ActivityKind.Internal,
            OperationName.Job.Veto => ActivityKind.Internal,
            _ => activity.Kind,
        };
    }

    private static string? GetTag(IEnumerable<KeyValuePair<string, string?>> tags, string tagName)
    {
        var tag = tags.SingleOrDefault(kv => kv.Key == tagName);
        return tag.Value;
    }

    private void OnStartActivity(Activity activity, object? payload)
    {
        if (activity.IsAllDataRequested)
        {
            if (!this.options.TracedOperations.Contains(activity.OperationName))
            {
                QuartzInstrumentationEventSource.Log.OperationIsFilteredOut(activity.OperationName);
                activity.IsAllDataRequested = false;
                return;
            }

            activity.DisplayName = GetDisplayName(activity);

            ActivityInstrumentationHelper.SetActivitySourceProperty(activity, ActivitySource);
            ActivityInstrumentationHelper.SetKindProperty(activity, GetActivityKind(activity));

            try
            {
                this.JobDetailsPropertyFetcher.TryFetch(payload, out var jobDetails);
                if (jobDetails != null)
                {
                    this.options.Enrich?.Invoke(activity, "OnStartActivity", jobDetails);
                }
            }
            catch (Exception ex)
            {
                QuartzInstrumentationEventSource.Log.EnrichmentException(ex);
            }
        }
    }

    private void OnStopActivity(Activity activity, object? payload)
    {
        if (activity.IsAllDataRequested)
        {
            try
            {
                this.JobDetailsPropertyFetcher.TryFetch(payload, out var jobDetails);
                if (jobDetails != null)
                {
                    this.options.Enrich?.Invoke(activity, "OnStopActivity", jobDetails);
                }
            }
            catch (Exception ex)
            {
                QuartzInstrumentationEventSource.Log.EnrichmentException(ex);
            }
        }
    }

    private void OnException(Activity activity, object? payload)
    {
        if (activity.IsAllDataRequested)
        {
            if (payload is not Exception exc)
            {
                QuartzInstrumentationEventSource.Log.NullPayload(nameof(QuartzDiagnosticListener), nameof(this.OnStopActivity));
                return;
            }

            if (this.options.RecordException)
            {
                activity.AddException(exc);
            }

            activity.SetStatus(ActivityStatusCode.Error, exc.Message);

            try
            {
                this.options.Enrich?.Invoke(activity, "OnException", exc);
            }
            catch (Exception ex)
            {
                QuartzInstrumentationEventSource.Log.EnrichmentException(ex);
            }
        }
    }
}
