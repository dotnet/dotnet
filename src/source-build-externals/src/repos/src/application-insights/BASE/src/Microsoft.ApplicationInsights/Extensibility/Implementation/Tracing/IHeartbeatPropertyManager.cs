namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines an implementation for management of the heartbeat feature of the 
    /// Application Insights SDK meant for public consumption. Add/Set properties, 
    /// disable/enable the heartbeat, and set the interval between heartbeat pulses.
    /// <remarks>
    /// If a module is present in the SDK at runtime that implements this interface, the
    /// heartbeat feature can be extended or configured as necessary.
    /// </remarks>
    /// </summary>
    public interface IHeartbeatPropertyManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not the Heartbeat feature is disabled.
        /// </summary>
        bool IsHeartbeatEnabled { get; set; }

        /// <summary>
        /// Gets a list of default heartbeat property providers that are disabled and will not contribute to the
        /// default heartbeat properties.
        /// </summary>
        IList<string> ExcludedHeartbeatPropertyProviders { get; }

        /// <summary>
        /// Gets or sets the delay between heartbeats.
        /// </summary>
        TimeSpan HeartbeatInterval { get; set; }

        /// <summary>
        /// Gets a list of property names that are not to be sent with the heartbeats.
        /// </summary>
        IList<string> ExcludedHeartbeatProperties { get; }

        /// <summary>
        /// Add a new Heartbeat property to the payload sent with each heartbeat.
        /// </summary>
        bool AddHeartbeatProperty(string propertyName, string propertyValue, bool isHealthy);

        /// <summary>
        /// Set an updated value into an existing property of the heartbeat.
        /// </summary>
        bool SetHeartbeatProperty(string propertyName, string propertyValue = null, bool? isHealthy = null);
    }
}
