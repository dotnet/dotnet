namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    internal static class HeartbeatDefaultPayload
    {
        internal static readonly IHeartbeatDefaultPayloadProvider[] DefaultPayloadProviders =
        {
            new BaseDefaultHeartbeatPropertyProvider(),
        };

        public static bool IsDefaultKeyword(string keyword)
        {
            foreach (var payloadProvider in DefaultPayloadProviders)
            {
                if (payloadProvider.IsKeyword(keyword))
                {
                    return true;
                }
            }

            return false;
        }

        public static async Task<bool> PopulateDefaultPayload(IEnumerable<string> disabledFields, IEnumerable<string> disabledProviders, IHeartbeatProvider provider)
        {
            bool populatedFields = false;

            foreach (var payloadProvider in DefaultPayloadProviders)
            {
                if (disabledProviders != null && disabledProviders.Contains(payloadProvider.Name, StringComparer.OrdinalIgnoreCase))
                {
                    // skip any azure specific modules here
                    continue;
                }

                bool fieldsAreSet = await payloadProvider.SetDefaultPayload(disabledFields, provider).ConfigureAwait(false);
                populatedFields = populatedFields || fieldsAreSet;
            }

            return populatedFields;
        }
    }
}
