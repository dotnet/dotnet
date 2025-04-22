// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Runtime.InteropServices;
using Microsoft.DotNet.XUnitExtensions;

namespace Xunit
{
    /// <summary>
    ///  This test should be run only on Windows on .NET Framework.
    /// </summary>
    public class WindowsFullFrameworkOnlyFactAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsFullFrameworkOnlyFactAttribute"/> class.
        /// </summary>
        /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
        public WindowsFullFrameworkOnlyFactAttribute(string? additionalMessage = null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.Skip = "This test only runs on Windows on .NET Framework.".AppendAdditionalMessage(additionalMessage);
                return;
            }
            if (!DiscovererHelpers.IsRunningOnNetFramework)
            {
                this.Skip = "This test only runs on .NET Framework.".AppendAdditionalMessage(additionalMessage);
            }
        }
    }
}
