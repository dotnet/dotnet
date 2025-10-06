// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#if !USES_XUNIT_3
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;
using Xunit;

namespace Microsoft.DotNet.XUnitExtensions
{
    /// <summary>
    /// This class discovers all of the tests and test classes that have
    /// applied the TestOnTargetFrameworkDiscoverer attribute
    /// </summary>
    public class SkipOnTargetFrameworkDiscoverer : ITraitDiscoverer
    {
        /// <summary>
        /// Gets the trait values from the Category attribute.
        /// </summary>
        /// <param name="traitAttribute">The trait attribute containing the trait values.</param>
        /// <returns>The trait values.</returns>
        public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
        {
            TargetFrameworkMonikers frameworks = (TargetFrameworkMonikers)traitAttribute.GetConstructorArguments().First();

            return DiscovererHelpers.TestFrameworkApplies(frameworks) ?
                new[] { new KeyValuePair<string, string>(XunitConstants.Category, XunitConstants.Failing) } :
                Array.Empty<KeyValuePair<string, string>>();
        }
    }
}
#endif
