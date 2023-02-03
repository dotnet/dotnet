// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using Microsoft.DotNet.XUnitExtensions;

namespace Xunit
{
    /// <summary>
    /// This test should be run only on .NET (.NET Core).
    /// </summary>
    public class DotNetOnlyFactAttribute : FactAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetOnlyFactAttribute"/> class.
        /// </summary>
        /// <param name="additionalMessage">The additional message that is appended to skip reason, when test is skipped.</param>
        public DotNetOnlyFactAttribute(string? additionalMessage = null)
        {
            if (!DiscovererHelpers.IsRunningOnNetCoreApp)
            {
                this.Skip = "This test only runs on .NET.".AppendAdditionalMessage(additionalMessage);
            }
        }
    }
}
