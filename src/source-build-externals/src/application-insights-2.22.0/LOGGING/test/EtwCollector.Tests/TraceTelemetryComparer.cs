//-----------------------------------------------------------------------
// <copyright file="TraceTelemetryComparer.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Microsoft.ApplicationInsights.EtwTelemetryCollector.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.DataContracts;

    internal class TraceTelemetryComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            TraceTelemetry template = x as TraceTelemetry;
            TraceTelemetry actual = y as TraceTelemetry;

            if (template == null || actual == null)
            {
                return Comparer.DefaultInvariant.Compare(x, y);
            }

            bool equal = string.Equals(template.Message, actual.Message, System.StringComparison.Ordinal)
                && template.SeverityLevel == actual.SeverityLevel
                && HaveProperties(template.Properties, actual.Properties);
            if (equal)
            {
                return 0;
            }

            return template.GetHashCode() < actual.GetHashCode() ? -1 : 1;
        }

        private bool HaveProperties(IDictionary<string, string> template, IDictionary<string, string> actual)
        {
            if (template.Count > actual.Count)
            {
                return false;
            }

            foreach (var kvp in template)
            {
                string actualValue;
                if (!actual.TryGetValue(kvp.Key, out actualValue))
                {
                    return false;
                }

                if (!string.Equals(kvp.Value, actualValue, System.StringComparison.Ordinal))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
