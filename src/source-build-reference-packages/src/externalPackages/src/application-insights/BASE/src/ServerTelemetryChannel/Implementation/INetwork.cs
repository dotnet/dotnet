namespace Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel.Implementation
{
    using System;
    using System.Net.NetworkInformation;

    /// <summary>
    /// Encapsulates platform-specific behavior of network information APIs.
    /// </summary>
    internal interface INetwork
    {
        /// <summary>
        /// Adds <see cref="NetworkChange.NetworkAddressChanged"/> event handler.
        /// </summary>
        /// <remarks>
        /// Defined as a method instead of an event in this interface because C# compiler 
        /// changes signature of event in a Windows Runtime component, making it very hard 
        /// to implement properly.
        /// </remarks>
        void AddAddressChangedEventHandler(NetworkAddressChangedEventHandler handler);

        /// <summary>
        /// Removes <see cref="NetworkChange.NetworkAddressChanged"/> event handler.
        /// </summary>
        /// <param name="handler">Address changed event handler.</param>
        void RemoveAddressChangeEventHandler(NetworkAddressChangedEventHandler handler);

        bool IsAvailable();
    }
}
