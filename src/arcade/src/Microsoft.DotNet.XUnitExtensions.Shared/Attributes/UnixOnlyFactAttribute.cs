// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.Runtime.InteropServices;
using Microsoft.DotNet.XUnitExtensions;

namespace Xunit
{
    /// <summary>
    /// This test should be run only on Unix (Linux, OSX platforms).
    /// </summary>
    public class UnixOnlyFactAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnixOnlyFactAttribute"/> class.
        /// </summary>
        /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
        public UnixOnlyFactAttribute(string? additionalMessage = null)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                this.Skip = "This test requires Unix to run.".AppendAdditionalMessage(additionalMessage);
            }
        }
    }
}
