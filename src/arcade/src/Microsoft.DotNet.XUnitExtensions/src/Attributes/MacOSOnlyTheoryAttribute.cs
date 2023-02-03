// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Runtime.InteropServices;
using Microsoft.DotNet.XUnitExtensions;

namespace Xunit
{
    /// <summary>
    /// This test should be run only on OSX.
    /// </summary>
    public class MacOSOnlyTheoryAttribute : TheoryAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MacOSOnlyTheoryAttribute"/> class.
        /// </summary>
        /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
        public MacOSOnlyTheoryAttribute(string? additionalMessage = null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                this.Skip = "This test requires macOS to run.".AppendAdditionalMessage(additionalMessage);
            }
        }
    }
}
