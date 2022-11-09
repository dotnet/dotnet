#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Globalization;
    using System.Management;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Threading;

    /// <summary>
    /// The reader is platform specific and applies to .NET applications only.
    /// </summary>
    internal class DeviceContextReader
    {
        private static DeviceContextReader instance;
        private string deviceId;
        private string deviceManufacturer;
        private string deviceName;
        private string networkType;

        /// <summary>
        /// Gets or sets the singleton instance for our application context reader.
        /// </summary>
        public static DeviceContextReader Instance
        {
            get
            {
                if (DeviceContextReader.instance != null)
                {
                    return DeviceContextReader.instance;
                }

                Interlocked.CompareExchange(ref DeviceContextReader.instance, new DeviceContextReader(), null);
                return DeviceContextReader.instance;
            }

            // allow for the replacement for the context reader to allow for testability
            internal set
            {
                DeviceContextReader.instance = value;
            }
        }

        /// <summary>
        /// Gets the host system locale.
        /// </summary>
        /// <returns>The discovered locale.</returns>
        public virtual string GetHostSystemLocale()
        {
            return CultureInfo.CurrentCulture.Name;
        }

        /// <summary>
        /// Gets the type of the device.
        /// </summary>
        /// <returns>The type for this device as a hard-coded string.</returns>
        public virtual string GetDeviceType()
        {
            return "PC";
        }

        /// <summary>
        /// Gets the device unique ID, or uses the fallback if none is available due to application configuration.
        /// </summary>
        /// <returns>
        /// The discovered device identifier.
        /// </returns>
        public virtual string GetDeviceUniqueId()
        {
            if (this.deviceId != null)
            {
                return this.deviceId;
            }

            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();
            
            if (hostName.EndsWith(domainName, StringComparison.OrdinalIgnoreCase) == false)
            {
                hostName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", hostName, domainName);
            }

            return this.deviceId = hostName;
        }

        /// <summary>
        /// Gets the device OEM.
        /// </summary>
        /// <returns>The discovered OEM.</returns>
        public virtual string GetOemName()
        {
            if (this.deviceManufacturer != null)
            {
                return this.deviceManufacturer;
            }

            return this.deviceManufacturer = RunWmiQuery("Win32_ComputerSystem", "Manufacturer", string.Empty);
        }

        /// <summary>
        /// Gets the device model.
        /// </summary>
        /// <returns>The discovered device model.</returns>
        public virtual string GetDeviceModel()
        {
            if (this.deviceName != null)
            {
                return this.deviceName;
            }

            return this.deviceName = RunWmiQuery("Win32_ComputerSystem", "Model", string.Empty);
        }

        /// <summary>
        /// Gets the network type.
        /// </summary>
        /// <returns>The discovered network type.</returns>
        public string GetNetworkType()
        {
            if (string.IsNullOrEmpty(this.networkType))
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                    {
                        if (networkInterface.OperationalStatus == OperationalStatus.Up)
                        {
                            this.networkType = networkInterface.NetworkInterfaceType.ToString();
                            return this.networkType;
                        }
                    }
                }

                this.networkType = NetworkInterfaceType.Unknown.ToString();
            }

            return this.networkType;
        }

        /// <summary>
        /// Runs a single WMI query for a property.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="property">The property.</param>
        /// <param name="defaultValue">The default value of the property if WMI fails.</param>
        /// <returns>The value if found, Unknown otherwise.</returns>
        private static string RunWmiQuery(string table, string property, string defaultValue)
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(string.Format(CultureInfo.InvariantCulture, "SELECT {0} FROM {1}", property, table)))
                {
                    foreach (ManagementObject currentObj in searcher.Get())
                    {
                        object data = currentObj[property];
                        if (data != null)
                        {
                            return data.ToString();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                WindowsServerEventSource.Log.DeviceContextWmiFailureWarning(exp.ToString());
            }

            return defaultValue;
        }
    }
}
#endif