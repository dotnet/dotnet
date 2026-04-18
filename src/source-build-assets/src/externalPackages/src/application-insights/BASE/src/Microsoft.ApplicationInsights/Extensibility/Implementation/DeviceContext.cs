namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Encapsulates information about a device where an application is running.
    /// </summary>
    public sealed class DeviceContext
    {
        private readonly IDictionary<string, string> properties;

        private string type;
        private string id;
        private string operatingSystem;
        private string oemName;
        private string model;

        internal DeviceContext(IDictionary<string, string> properties)
        {
            this.properties = properties;
        }

        /// <summary>
        /// Gets or sets the type for the current device.
        /// </summary>
        public string Type
        {
            get { return string.IsNullOrEmpty(this.type) ? null : this.type; }
            set { this.type = value; }
        }

        /// <summary>
        /// Gets or sets a device unique ID.
        /// </summary>
        public string Id
        {
            get { return string.IsNullOrEmpty(this.id) ? null : this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// Gets or sets the operating system name.
        /// </summary>
        public string OperatingSystem
        {
            get { return string.IsNullOrEmpty(this.operatingSystem) ? null : this.operatingSystem; }
            set { this.operatingSystem = value; }
        }

        /// <summary>
        /// Gets or sets the device OEM for the current device.
        /// </summary>
        public string OemName
        {
            get { return string.IsNullOrEmpty(this.oemName) ? null : this.oemName; }
            set { this.oemName = value; }
        }

        /// <summary>
        /// Gets or sets the device model for the current device.
        /// </summary>
        public string Model
        {
            get { return string.IsNullOrEmpty(this.model) ? null : this.model; }
            set { this.model = value; }
        }

        /// <summary>
        /// Gets or sets the <a href="http://www.iana.org/assignments/ianaiftype-mib/ianaiftype-mib">IANA interface type</a>
        /// for the internet connected network adapter.
        /// </summary>
        [Obsolete("Use custom properties.")]
        public string NetworkType
        {
            get { return this.properties.GetTagValueOrNull("ai.device.network"); }
            set { this.properties.SetTagValueOrRemove("ai.device.network", value); }
        }

        /// <summary>
        /// Gets or sets the current application screen resolution.
        /// </summary>
        [Obsolete("Use custom properties.")]
        public string ScreenResolution
        {
            get { return this.properties.GetTagValueOrNull("ai.device.screenResolution"); }
            set { this.properties.SetStringValueOrRemove("ai.device.screenResolution", value); }
        }

        /// <summary>
        /// Gets or sets the current display language of the operating system.
        /// </summary>
        [Obsolete("Use custom properties.")]
        public string Language
        {
            get { return this.properties.GetTagValueOrNull("ai.device.language"); }
            set { this.properties.SetStringValueOrRemove("ai.device.language", value); }
        }

        internal void UpdateTags(IDictionary<string, string> tags)
        {
            tags.UpdateTagValue(ContextTagKeys.Keys.DeviceType, this.Type);
            tags.UpdateTagValue(ContextTagKeys.Keys.DeviceId, this.Id);
            tags.UpdateTagValue(ContextTagKeys.Keys.DeviceOSVersion, this.OperatingSystem);
            tags.UpdateTagValue(ContextTagKeys.Keys.DeviceOEMName, this.OemName);
            tags.UpdateTagValue(ContextTagKeys.Keys.DeviceModel, this.Model);
        }
        
        internal void CopyTo(DeviceContext target)
        {
            Tags.CopyTagValue(this.Type, ref target.type);
            Tags.CopyTagValue(this.Id, ref target.id);
            Tags.CopyTagValue(this.OperatingSystem, ref target.operatingSystem);
            Tags.CopyTagValue(this.OemName, ref target.oemName);
            Tags.CopyTagValue(this.Model, ref target.model);
        }
    }
}
