#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Azure.Emulation
{
    using System;
    
    using Azure = Microsoft.WindowsAzure.ServiceRuntime;

    [Serializable]
    internal class TestRole : Azure.Role
    {
        /// <summary>
        /// The role name.
        /// </summary>
        private readonly string roleName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRole" /> class.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        public TestRole(string roleName)
        {
            this.roleName = roleName;
        }
        
        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get { return this.roleName; }
        }
    }
}
#endif