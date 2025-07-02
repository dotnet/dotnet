namespace Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse
{
    using System.Linq;
    using System.Net.Http.Headers;
    using Microsoft.ApplicationInsights.Common.Internal;

    internal static class QuickPulseServiceClientHelpers
    {
        public static string GetValueSafe(this HttpHeaders headers, string name)
        {
            string value = default(string);

            if (headers?.Contains(name) ?? false)
            {
                value = headers.GetValues(name).First();
                value = StringUtilities.EnforceMaxLength(value, InjectionGuardConstants.QuickPulseResponseHeaderMaxLength);
            }
            
            return value;
        }
    }
}
