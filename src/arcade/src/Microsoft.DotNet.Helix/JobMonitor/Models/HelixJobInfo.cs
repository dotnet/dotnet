// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.DotNet.Helix.Client.Models;
using Microsoft.DotNet.Helix.JobMonitor.Models;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Helix.JobMonitor.Models
{
    /// <summary>
    /// Represents a Helix job and its current status.
    /// Decoupled from the Helix Client SDK's generated models.
    /// </summary>
    public sealed class HelixJobInfo
    {
        public const string PreviousHelixJobNamePropertyName = "PreviousHelixJobName";

        public HelixJobInfo(JobSummary helixJob)
        {
            JobName = helixJob.Name;
            Status = helixJob.Finished != null ? "finished" : "running";
            TestRunName = GetTestRunNameFromJob(helixJob);
            StageName = GetStringPropertyFromJob(helixJob, "System.StageName");
            QueueId = helixJob.QueueId;
            InitialWorkItemCount = helixJob.InitialWorkItemCount;
            Properties = helixJob.Properties;
        }

        public HelixJobInfo(
            string jobName,
            string status,
            string testRunName = null,
            string stageName = null,
            string submitterJobName = null,
            string submitterJobDisplayName = null,
            string queueId = null,
            string previousHelixJobName = null,
            int? initialWorkItemCount = null)
        {
            JobName = jobName ?? throw new ArgumentNullException(nameof(jobName));
            Status = status ?? throw new ArgumentNullException(nameof(status));
            TestRunName = testRunName;
            StageName = stageName;
            QueueId = queueId;
            InitialWorkItemCount = initialWorkItemCount;
            Properties = CreateProperties(testRunName, stageName, submitterJobName, submitterJobDisplayName, previousHelixJobName);
        }

        public string JobName { get; }

        public string Status { get; }

        /// <summary>
        /// The desired AzDO test run name for this job. May come from a Helix job property.
        /// Falls back to the job name if not set.
        /// </summary>
        public string TestRunName { get; }

        /// <summary>
        /// Name of the Azure DevOps pipeline stage that submitted this Helix job, taken from
        /// the "System.StageName" property stamped onto the job by <c>SendHelixJob</c>. May be
        /// null if the property is not present.
        /// </summary>
        public string StageName { get; }

        /// <summary>
        /// Helix target queue this job ran on (e.g. "Ubuntu.2204.Amd64.Open"). Comes from the
        /// Helix <c>JobSummary.QueueId</c>. May be null on synthetic jobs.
        /// </summary>
        public string QueueId { get; }

        public string SubmitterJobName => GetStringProperty(Properties, "System.JobName");

        /// <summary>
        /// Matrix-expanded Azure DevOps job display name (e.g. "Windows_NT Build_Release"),
        /// stamped onto the job from the <c>System.JobDisplayName</c> predefined variable.
        /// May be null on older Helix jobs that pre-date this property being copied.
        /// </summary>
        public string SubmitterJobDisplayName => GetStringProperty(Properties, "System.JobDisplayName");

        public string PreviousHelixJobName => GetStringProperty(Properties, PreviousHelixJobNamePropertyName);

        /// <summary>
        /// Human-readable identifier used in log messages. Combines the matrix-expanded Azure
        /// DevOps job name and the Helix queue with the Helix job GUID, e.g.
        /// "Windows_NT Build_Release - Windows.10.Amd64.Open (47edfeae-...)". Falls back to
        /// just the Helix job GUID when none of the AzDO job identifiers are available.
        /// </summary>
        public string DisplayName => FormatDisplayName();

        private string FormatDisplayName()
        {
            // Prefer the matrix-expanded display name (e.g. "Windows_NT Build_Release"), fall
            // back to the bare AzDO job name (e.g. "Build_Release") when the display name is
            // not present (older jobs / non-matrix builds).
            string azdoJobLabel = !string.IsNullOrEmpty(SubmitterJobDisplayName)
                ? SubmitterJobDisplayName
                : SubmitterJobName;

            if (string.IsNullOrEmpty(azdoJobLabel) && string.IsNullOrEmpty(QueueId))
            {
                return JobName;
            }

            string prefix = !string.IsNullOrEmpty(azdoJobLabel) && !string.IsNullOrEmpty(QueueId)
                ? $"{azdoJobLabel} - {QueueId}"
                : (azdoJobLabel ?? QueueId);

            return $"{prefix} ({JobName})";
        }

        public int? InitialWorkItemCount { get; }

        public JToken Properties { get; }

        public bool IsCompleted => Status.Equals("finished", StringComparison.OrdinalIgnoreCase)
            || Status.Equals("failed", StringComparison.OrdinalIgnoreCase);

        public string DetailsUri => GetDetailsUri(JobName);

        public static string GetDetailsUri(string jobName)
        {
            return $"https://helix.dot.net/api/2019-06-17/jobs/{jobName}/details";
        }

        /// <summary>
        /// Comparer that considers two <see cref="HelixJobInfo"/> instances equal when their
        /// <see cref="JobName"/> values match (case-insensitive).
        /// </summary>
        public static IEqualityComparer<HelixJobInfo> ByJobNameComparer { get; } = new JobNameEqualityComparer();

        private static string GetTestRunNameFromJob(JobSummary helixJob)
        {
            // The Helix SDK stamps the desired Azure DevOps test run name onto the job as a
            // "TestRunName" property when submitting (matching what StartAzurePipelinesTestRun
            // would have used). Fall back to the Helix job name if the property is missing so
            // we always produce a non-empty name.
            if (helixJob.Properties is JObject properties)
            {
                if (properties.TryGetValue("TestRunName", out JToken testRunName))
                {
                    string value = testRunName?.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        return value;
                    }
                }

                properties.TryGetValue("System.StageName", out JToken stageName);
                properties.TryGetValue("System.JobName", out JToken jobName);
                return $"{stageName} {jobName} run on {helixJob.QueueId}".Trim();
            }

            return helixJob.Name;
        }

        private static string GetStringPropertyFromJob(JobSummary helixJob, string propertyName)
            => GetStringProperty(helixJob.Properties, propertyName);

        private static string GetStringProperty(JToken propertiesToken, string propertyName)
        {
            if (propertiesToken is JObject properties
                && properties.TryGetValue(propertyName, out JToken token))
            {
                string value = token?.ToString();
                if (!string.IsNullOrEmpty(value))
                {
                    return value;
                }
            }

            return null;
        }

        private static JObject CreateProperties(
            string testRunName,
            string stageName,
            string submitterJobName,
            string submitterJobDisplayName,
            string previousHelixJobName)
        {
            var properties = new JObject();

            if (!string.IsNullOrEmpty(testRunName))
            {
                properties["TestRunName"] = testRunName;
            }

            if (!string.IsNullOrEmpty(stageName))
            {
                properties["System.StageName"] = stageName;
            }

            if (!string.IsNullOrEmpty(submitterJobName))
            {
                properties["System.JobName"] = submitterJobName;
            }

            if (!string.IsNullOrEmpty(submitterJobDisplayName))
            {
                properties["System.JobDisplayName"] = submitterJobDisplayName;
            }

            if (!string.IsNullOrEmpty(previousHelixJobName))
            {
                properties[PreviousHelixJobNamePropertyName] = previousHelixJobName;
            }

            return properties;
        }
    }
}

file class JobNameEqualityComparer : IEqualityComparer<HelixJobInfo>
{
    public bool Equals(HelixJobInfo x, HelixJobInfo y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return string.Equals(x.JobName, y.JobName, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(HelixJobInfo obj)
        => obj?.JobName == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(obj.JobName);
}
