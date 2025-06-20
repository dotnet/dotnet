namespace Microsoft.ApplicationInsights.Shared.Internals
{
    using System;
    using System.Reflection;

    /// <summary>
    /// This class provides the assembly name for the EventSource implementations.
    /// </summary>
    internal sealed class ApplicationNameProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationNameProvider"/> class.
        /// </summary>
        public ApplicationNameProvider()
        {
            this.Name = GetApplicationName();
        }

        /// <summary>
        /// Gets name of the current assembly.
        /// </summary>
        public string Name { get; private set; }

        private static string GetApplicationName()
        {
            try
            {
                return Assembly.GetEntryAssembly().GetName().Name;
            }
            catch (Exception exp)
            {
                return "Undefined " + exp.Message;
            }
        }
    }
}
