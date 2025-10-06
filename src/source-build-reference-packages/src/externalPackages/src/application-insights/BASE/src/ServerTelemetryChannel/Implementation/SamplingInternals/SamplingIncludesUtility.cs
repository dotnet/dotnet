namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation.SamplingInternals
{
    using System;
    using System.Collections.Generic;

    using Microsoft.ApplicationInsights.DataContracts;

    /// <summary>
    /// This utility will calculate the IncludedTypes bitmask based on the delimited input string.
    /// </summary>
    internal static class SamplingIncludesUtility
    {
        private const string DependencyTelemetryName = "DEPENDENCY";
        private const string EventTelemetryName = "EVENT";
        private const string ExceptionTelemetryName = "EXCEPTION";
        private const string PageViewTelemetryName = "PAGEVIEW";
        private const string RequestTelemetryName = "REQUEST";
        private const string TraceTelemetryName = "TRACE";

        private static readonly char[] ListSeparators = { ';' };

        /// <summary>
        /// Calculate an Included Bitmask based on the input string.
        /// Starts with Enum.None value and adds types.
        /// </summary>
        /// <param name="includesString">Delimited string of types to be included.</param>
        /// <returns>Bitmask representing types to include.</returns>
        public static SamplingTelemetryItemTypes CalculateFromIncludes(string includesString)
        {
            return Calculate(operation: IncludeOperator, flags: SamplingTelemetryItemTypes.None, input: includesString);
        }

        /// <summary>
        /// Calculate an Included Bitmask based on the input string.
        /// Starts with Enum.ALL (~None) and removes types.
        /// </summary>
        /// <param name="excludesString">Delimited string of types to be excluded.</param>
        /// <returns>Bitmask representing types to include.</returns>
        public static SamplingTelemetryItemTypes CalculateFromExcludes(string excludesString)
        {
            return Calculate(operation: ExcludeOperator, flags: ~SamplingTelemetryItemTypes.None, input: excludesString);
        }

        private static IDictionary<string, SamplingTelemetryItemTypes> GetAllowedTypes()
        {
            return new Dictionary<string, SamplingTelemetryItemTypes>(6, StringComparer.OrdinalIgnoreCase)
            {
                { DependencyTelemetryName, SamplingTelemetryItemTypes.RemoteDependency }, // DependencyTelemetry
                { EventTelemetryName, SamplingTelemetryItemTypes.Event }, // EventTelemetry
                { ExceptionTelemetryName, SamplingTelemetryItemTypes.Exception }, // ExceptionTelemetry
                { PageViewTelemetryName, SamplingTelemetryItemTypes.PageView }, // PageViewTelemetry
                { RequestTelemetryName, SamplingTelemetryItemTypes.Request }, // RequestTelemetry
                { TraceTelemetryName, SamplingTelemetryItemTypes.Message }, // TraceTelemetry
            };
        }

        private static SamplingTelemetryItemTypes Calculate(Func<SamplingTelemetryItemTypes, SamplingTelemetryItemTypes, SamplingTelemetryItemTypes> operation, SamplingTelemetryItemTypes flags, string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var allowedTypes = GetAllowedTypes();

                foreach (string item in SplitInput(input))
                {
                    if (allowedTypes.TryGetValue(item, out SamplingTelemetryItemTypes value))
                    {
                        flags = operation(flags, value);
                    }
                }
            }

            return flags;
        }

        private static string[] SplitInput(string input) => input.Split(ListSeparators, StringSplitOptions.RemoveEmptyEntries);

        private static SamplingTelemetryItemTypes IncludeOperator(SamplingTelemetryItemTypes flags, SamplingTelemetryItemTypes value) => flags |= value;

        private static SamplingTelemetryItemTypes ExcludeOperator(SamplingTelemetryItemTypes flags, SamplingTelemetryItemTypes value) => flags &= ~value;
    }
}
