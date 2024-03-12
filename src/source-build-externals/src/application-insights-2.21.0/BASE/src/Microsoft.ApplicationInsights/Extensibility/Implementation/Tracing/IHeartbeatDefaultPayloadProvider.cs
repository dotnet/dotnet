namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides default properties for the heartbeat.
    /// </summary>
    internal interface IHeartbeatDefaultPayloadProvider
    {
        /// <summary>
        /// Gets the name of this heartbeat payload provider. Users can disable default payload providers 
        /// altogether by specifying them in the ApplicationInsights.config file, or by setting properties
        /// on the HeartbeatProvider at runtime.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Assess if a given string contains a keyword that this default payload provider supplies to the
        /// heartbeat payload. This is primarly used to dissallow users from adding or setting a conflicting
        /// property into the heartbeat.
        /// </summary>
        /// <param name="keyword">string to test against supplied property names.</param>
        /// <returns>True if the given keyword conflicts with this default payload provider's properties.</returns>
        bool IsKeyword(string keyword);

        /// <summary>
        /// Call to initiate the setting of properties in the the given heartbeat provider.
        /// </summary>
        /// <param name="disabledFields">List of any default heartbeat payload field names that the user has specified as being disabled.</param>
        /// <param name="provider">The heartbeat provider to set the values into.</param>
        /// <returns>True if any fields were set into the provider, false if none were.</returns>
        Task<bool> SetDefaultPayload(IEnumerable<string> disabledFields, IHeartbeatProvider provider);
    }
}
