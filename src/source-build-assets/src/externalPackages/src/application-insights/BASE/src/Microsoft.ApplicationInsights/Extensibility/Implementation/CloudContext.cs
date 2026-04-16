namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.External;

    /// <summary>
    /// Encapsulates information about a cloud where an application is running.
    /// </summary>
    public sealed class CloudContext
    {
        private string roleName;
        private string roleInstance;

        internal CloudContext()
        {
        }

        /// <summary>
        /// Gets or sets the role name.
        /// </summary>
        public string RoleName
        {
            get { return string.IsNullOrEmpty(this.roleName) ? null : this.roleName; }
            set { this.roleName = value; }
        }

        /// <summary>
        /// Gets or sets the role instance.
        /// </summary>
        public string RoleInstance
        {
            get { return string.IsNullOrEmpty(this.roleInstance) ? null : this.roleInstance; }
            set { this.roleInstance = value; }
        }

        internal void UpdateTags(IDictionary<string, string> tags)
        {
            tags.UpdateTagValue(ContextTagKeys.Keys.CloudRole, this.RoleName);
            tags.UpdateTagValue(ContextTagKeys.Keys.CloudRoleInstance, this.RoleInstance);
        }
        
        internal void CopyTo(CloudContext target)
        {
            Tags.CopyTagValue(this.RoleName, ref target.roleName);
            Tags.CopyTagValue(this.RoleInstance, ref target.roleInstance);
        }
    }
}
