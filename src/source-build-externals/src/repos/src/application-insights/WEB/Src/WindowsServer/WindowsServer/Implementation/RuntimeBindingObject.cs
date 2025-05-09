#if NETFRAMEWORK
namespace Microsoft.ApplicationInsights.WindowsServer.Implementation
{
    using System;
    using System.Globalization;
    using System.Reflection;

    /// <summary>
    /// A runtime bound object for a given .NET type.
    /// </summary>
    internal abstract class RuntimeBindingObject :
        MarshalByRefObject
    {
        /// <summary>
        /// The target type for our object.
        /// </summary>
        private Type targetType;

        /// <summary>
        /// The target object.
        /// </summary>
        private object targetObject;

        /// <summary>
        /// The assembly which is loaded reflectively.
        /// </summary>
        private Assembly loadedAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeBindingObject"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="loadedAssembly">The loaded assembly.</param>
        /// <param name="activationArgs">The activation arguments.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Safe to override")]
        protected RuntimeBindingObject(Type targetType, Assembly loadedAssembly, params object[] activationArgs)
        {
            this.targetType = targetType;
            this.targetObject = this.GetTargetObjectInstance(targetType, activationArgs);
            this.loadedAssembly = loadedAssembly;
        }
        
        /// <summary>
        /// Gets or sets the type of the target.
        /// </summary>
        protected internal Type TargetType
        {
            get { return this.targetType; }
            set { this.targetType = value; }
        }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        protected internal object TargetObject
        {
            get { return this.targetObject; }
            set { this.targetObject = value; }
        }

        /// <summary>
        /// Gets or sets the loaded assembly.
        /// </summary>
        protected internal Assembly LoadedAssembly
        {
            get { return this.loadedAssembly; }
            set { this.loadedAssembly = value; }
        }

        /// <summary>
        /// Gets the target object instance.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="activationArgs">The activation arguments.</param>
        /// <returns>The activated instance is one is required.</returns>
        protected abstract object GetTargetObjectInstance(Type targetType, object[] activationArgs);

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The value for our property.</returns>
        protected object GetProperty(string name, params object[] args)
        {
            return this.GetProperty(name, (Type[])null, args);
        }
        
        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The value for our property.</returns>
        private object GetProperty(string name, Type[] parameterTypes, object[] args)
        {
            return this.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, parameterTypes, args);
        }

        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="args">The arguments.</param>
        /// <returns>The value for our property.</returns>
        private object GetProperty(string name, BindingFlags bindingFlags, Type[] parameterTypes, object[] args)
        {
            if (string.IsNullOrEmpty(name) == true)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (parameterTypes == null)
            {
                return this.InvokeHelper(name, bindingFlags | BindingFlags.GetProperty, args, null);
            }

            PropertyInfo info = this.targetType.GetProperty(name, bindingFlags, null, null, parameterTypes, null);
            if (info == null)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Could not get property info for '{0}' with the specified parameters.", name));
            }

            return info.GetValue(this.targetObject, args);
        }
        
        /// <summary>
        /// Invocation helper for calling any member on our target object.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="bindingFlags">The binding flags.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The return value for our invocation.</returns>
        private object InvokeHelper(string name, BindingFlags bindingFlags, object[] args, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(name) == true)
            {
                throw new ArgumentNullException(nameof(name));
            }

            object output;
            try
            {
                output = this.targetType.InvokeMember(name, bindingFlags, null, this.targetObject, args, culture);
            }
            catch (TargetInvocationException exception)
            {
                if (exception.InnerException == null)
                {
                    throw;
                }

                throw exception.InnerException;
            }

            return output;
        }
    }
}
#endif