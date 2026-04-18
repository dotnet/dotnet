#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System.Reflection;

    /// <summary>
    /// The wrapper for the Azure Service Runtime.
    /// </summary>
    internal class ServiceRuntime
    {
        private Assembly loadedAssembly;

        public ServiceRuntime(Assembly loadedAssembly)
        {
            this.loadedAssembly = loadedAssembly;
        }

        /// <summary>
        /// Gets the role environment.
        /// </summary>        
        /// <returns>
        /// The role environment object.
        /// </returns>
        public RoleEnvironment GetRoleEnvironment()
        {
            // TODO: remove factory
            return new RoleEnvironment(this.loadedAssembly);
        }        
    }
}
#endif