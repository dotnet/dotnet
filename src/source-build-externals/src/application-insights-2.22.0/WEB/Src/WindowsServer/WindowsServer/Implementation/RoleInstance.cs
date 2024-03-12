#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents an instance of a role. 
    /// </summary>
    internal class RoleInstance :
        RuntimeBindingObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleInstance"/> class.
        /// </summary>
        /// <param name="targetObject">The target object.</param>
        /// <param name="loadedAssembly">The loaded assembly.</param>
        public RoleInstance(object targetObject, Assembly loadedAssembly)
            : base(loadedAssembly.GetType("Microsoft.WindowsAzure.ServiceRuntime.RoleInstance", false), loadedAssembly, targetObject)
        {
        }

        /// <summary>
        /// Gets the instance identifier (ID) of the role instance.
        /// </summary>
        public string Id
        {
            get
            {
                if (this.TargetObject == null)
                {
                    throw new NotSupportedException("Cannot get an Id when no TargetObject is set.");
                }

                return (string)this.GetProperty("Id");
            }
        }

        /// <summary>
        /// Gets the Role object that is associated with the role instance.
        /// </summary>
        public Role Role
        {
            get
            {
                if (this.TargetObject == null)
                {
                    throw new NotSupportedException("Cannot get a role when no TargetObject is set.");
                }

                object role = this.GetProperty("Role");
                return new Role(role, this.LoadedAssembly);
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