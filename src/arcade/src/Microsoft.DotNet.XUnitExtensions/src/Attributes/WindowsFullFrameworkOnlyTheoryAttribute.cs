// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Runtime.InteropServices;
using Microsoft.DotNet.XUnitExtensions;

namespace Xunit
{
    /// <summary>
    ///  This test should be run only on Windows on full .NET Framework.
    /// </summary>
    public class WindowsFullFrameworkOnlyTheoryAttribute : TheoryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsFullFrameworkOnlyTheoryAttribute"/> class.
        /// Creates the attribute.
        /// </summary>
        /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
        public WindowsFullFrameworkOnlyTheoryAttribute(string? additionalMessage = null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.Skip = "This test only runs on Windows on full framework.".AppendAdditionalMessage(additionalMessage);
                return;
            }
            if (!DiscovererHelpers.IsRunningOnNetFramework) 
            {
                this.Skip = "This test only runs on full framework.".AppendAdditionalMessage(additionalMessage);
            }
        }
    }
}
