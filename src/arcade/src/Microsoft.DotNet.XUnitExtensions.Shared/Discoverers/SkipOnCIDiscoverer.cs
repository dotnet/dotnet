// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !USES_XUNIT_3

using System;
using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Microsoft.DotNet.XUnitExtensions
{
    /// <summary>
    /// This class discovers all of the tests and test classes that have
    /// applied the SkipOnCIAttribute
    /// </summary>
    public class SkipOnCIDiscoverer : ITraitDiscoverer
    {
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_CI")) ||
                !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HELIX_WORKITEM_ROOT")) ||
                !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AGENT_OS")))
            {
                yield return new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing);
            }
        }
    }
}
#endif
