// <copyright file="IDebugOutput.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

namespace Microsoft.ApplicationInsights.Extensibility
{
    using Microsoft.ApplicationInsights.Channel;

    /// <summary>
    /// Encapsulates method call that has to be compiled with DEBUG compiler constant.
    /// </summary>
    internal interface IDebugOutput
    {
        /// <summary>
        /// Write the message to the VisualStudio output window.
        /// </summary>
        void WriteLine(string message);

        /// <summary>
        /// Checks to see if logging is enabled by an attached debugger. 
        /// </summary>
        /// <returns>true if a debugger is attached and logging is enabled; otherwise, false.</returns>
        bool IsLogging();

        /// <summary>
        /// Checks to see if debugger is attached.
        /// </summary>
        /// <returns>true if debugger is attached.</returns>
        bool IsAttached();
    }
}
