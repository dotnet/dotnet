namespace Microsoft.ApplicationInsights.Extensibility.Implementation.ApplicationId
{
    using System.Collections.Generic;

    /// <summary>
    /// Application Id Provider that holds a dictionary of Instrumentation Keys to Application Ids.
    /// </summary>
    public class DictionaryApplicationIdProvider : IApplicationIdProvider
    {
        /// <summary>
        /// Gets or sets a dictionary of Instrumentation Keys to Application Ids.
        /// </summary>
        public IReadOnlyDictionary<string, string> Defined { get; set; }

        /// <summary>
        /// Gets or sets an <see cref="IApplicationIdProvider" /> to use to lookup an Instrumentation Key not found in the dictionary.
        /// </summary>
        /// <remarks>
        /// This property is optional. If this is NULL, additional lookups will not be performed.
        /// </remarks>
        public IApplicationIdProvider Next { get; set; }

        /// <summary>
        /// Provides an Application Id based on an Instrumentation Key.
        /// </summary>
        /// <param name="instrumentationKey">Instrumentation Key string used to lookup associated Application Id.</param>
        /// <param name="applicationId">Application Id corresponding to the Instrumentation Key. Returns NULL if a match was not found.</param>
        /// <returns>TRUE if Application Id was successfully retrieved, FALSE otherwise.</returns>
        public bool TryGetApplicationId(string instrumentationKey, out string applicationId)
        {
            applicationId = null;

            var found = this.Defined?.TryGetValue(instrumentationKey, out applicationId) ?? false;

            if (!found)
            {
                found = this.Next?.TryGetApplicationId(instrumentationKey, out applicationId) ?? false;
            }

            return found;
        }
    }
}
