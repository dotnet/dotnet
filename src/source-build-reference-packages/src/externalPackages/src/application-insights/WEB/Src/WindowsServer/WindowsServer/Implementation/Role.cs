#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents a role that is defined as part of a hosted service. 
    /// </summary>
    internal class Role :
        RuntimeBindingObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Role"/> class.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="loadedAssembly">The loaded assembly.</param>
        public Role(object targetObject, Assembly loadedAssembly)
            : base(loadedAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.Role", false), loadedAssembly, targetObject)
        {
        }

        /// <summary>
        /// Gets the name of the role as it is declared in the service definition file.
        /// </summary>
        public string Name
        {
            get
            {
                if (this.TargetObject == null)
                {
                    throw new NotSupportedException();
                }

                return (string)this.GetProperty("Name");
            }
        }

        /// <summary>
        /// Gets the target object instance.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="activationArgs">The activation arguments.</param>
        /// <returns>
        /// The activated instance is one is required.
        /// </returns>
        protected override object GetTargetObjectInstance(Type targetType, object[] activationArgs)
        {
            return activationArgs[0];
        }
    }
}
#endif