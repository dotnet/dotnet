// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using Xunit.Sdk;

namespace Xunit
{
    /// <summary>
    /// Apply this attribute to your test method to specify the test category.
    /// </summary>
#if !USES_XUNIT_3
    [TraitDiscoverer("Microsoft.DotNet.XUnitExtensions.TestCategoryDiscoverer", "Microsoft.DotNet.XUnitExtensions")]
#endif
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class TestCategoryAttribute : Attribute, ITraitAttribute
    {
#if USES_XUNIT_3
        private readonly string _category;
#endif

        public TestCategoryAttribute(string category)
        {
#if USES_XUNIT_3
            _category = category;
#endif
        }

#if USES_XUNIT_3
        public IReadOnlyCollection<KeyValuePair<string, string>> GetTraits()
        {
            return [new KeyValuePair<string, string>("Category", _category)];
        }
#endif
    }
}
