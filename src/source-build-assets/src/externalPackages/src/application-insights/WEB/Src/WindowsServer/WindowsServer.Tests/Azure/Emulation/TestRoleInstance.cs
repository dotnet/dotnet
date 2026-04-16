#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Azure.Emulation
{
    using System;
    using System.Globalization;

    using Azure = Microsoft.WindowsAzure.ServiceRuntime;

    [Serializable]
    internal class TestRoleInstance :
        Azure.RoleInstance
    {
        /// <summary>
        /// The ID format for instance names.
        /// </summary>
        public const string IdFormat = "{0}_IN_{1}";

        /// <summary>
        /// The parent role for our instance.
        /// </summary>
        private readonly Azure.Role parentRole;

        /// <summary>
        /// The ordinal for our instance.
        /// </summary>
        private readonly int ordinal;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRoleInstance" /> class.
        /// </summary>
        /// <param name="parentRole">The parent role.</param>
        /// <param name="ordinal">The ordinal.</param>
        public TestRoleInstance(Azure.Role parentRole, int ordinal)
        {
            this.parentRole = parentRole;
            this.ordinal = ordinal;
        }
        
        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Id
        {
            get { return string.Format(CultureInfo.InvariantCulture, TestRoleInstance.IdFormat, this.parentRole.Name, this.ordinal); }
        }

        /// <summary>
        /// Gets the role.
        /// </summary>
        public override Azure.Role Role
        {
            get { return this.parentRole; }
        }
    }
}
#endif