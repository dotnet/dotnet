namespace Microsoft.ApplicationInsights.DataContracts
{
    using System;

    /// <summary>
    /// Wrapper class for <see cref="Extensibility.Implementation.External.StackFrame"/> for API exposure.
    /// </summary>
    public sealed class StackFrame
    {
        /// <summary>
        /// Constructs an instance.
        /// </summary>
        public StackFrame(string assembly, string fileName, int level, int line, string method)
        {
            this.Data = new Extensibility.Implementation.External.StackFrame()
            {
                assembly = assembly,
                fileName = fileName,
                level = level,
                line = line,
                method = method,
            };
        }

        internal Extensibility.Implementation.External.StackFrame Data { get; private set; } = null;
    }
}