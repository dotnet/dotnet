// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Microsoft.DotNet.XUnitExtensions;
using Xunit.Sdk;

namespace Xunit
{
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.SkipOnCIDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class SkipOnCIAttribute : Attribute, ITraitAttribute
    {
        public string Reason { get; private set; }

        public SkipOnCIAttribute(string reason)
        {
            Reason = reason;
        }

#if USES_XUNIT_3
        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_CI")) ||
                !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("HELIX_WORKITEM_ROOT")) ||
                !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AGENT_OS")))
            {
                return [new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing)];
            }

            return [];
        }
#endif
    }
}
