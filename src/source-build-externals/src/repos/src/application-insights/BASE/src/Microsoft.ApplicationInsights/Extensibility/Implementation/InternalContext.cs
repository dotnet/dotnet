namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Encapsulates Internal information.
    /// </summary>
    public sealed class InternalContext
    {
        private string sdkVersion;
        private string agentVersion;
        private string nodeName;

        internal InternalContext()
        {
        }

        /// <summary>
        /// Gets or sets application insights SDK version.
        /// </summary>
        public string SdkVersion
        {
            get { return string.IsNullOrEmpty(this.sdkVersion) ? null : this.sdkVersion; }
            set { this.sdkVersion = value; }
        }

        /// <summary>
        /// Gets or sets application insights agent version.
        /// </summary>
        public string AgentVersion
        {
            get { return string.IsNullOrEmpty(this.agentVersion) ? null : this.agentVersion; }
            set { this.agentVersion = value; }
        }

        /// <summary>
        /// Gets or sets node name for the billing purposes. Use this filed to override the standard way node names got detected.
        /// </summary>
        public string NodeName
        {
            get { return string.IsNullOrEmpty(this.nodeName) ? null : this.nodeName; }
            set { this.nodeName = value; }
        }

        internal void UpdateTags(IDictionary<string, string> tags)
        {
            tags.UpdateTagValue(ContextTagKeys.Keys.InternalSdkVersion, this.SdkVersion);
            tags.UpdateTagValue(ContextTagKeys.Keys.InternalAgentVersion, this.AgentVersion);
            tags.UpdateTagValue(ContextTagKeys.Keys.InternalNodeName, this.NodeName);
        }
        
        internal void CopyTo(InternalContext target)
        {
            Tags.CopyTagValue(this.SdkVersion, ref target.sdkVersion);
            Tags.CopyTagValue(this.AgentVersion, ref target.agentVersion);
            Tags.CopyTagValue(this.NodeName, ref target.nodeName);
        }
    }
}
