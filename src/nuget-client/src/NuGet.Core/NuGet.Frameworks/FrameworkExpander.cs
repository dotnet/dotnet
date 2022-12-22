// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace NuGet.Frameworks
{
    /// <summary>
    /// FrameworkExpander finds all equivalent and compatible frameworks for a NuGetFramework
    /// </summary>
    public class FrameworkExpander
    {
        private readonly IFrameworkNameProvider _mappings;

        public FrameworkExpander()
            : this(DefaultFrameworkNameProvider.Instance)
        {
        }

        public FrameworkExpander(IFrameworkNameProvider mappings)
        {
            _mappings = mappings;
        }

        /// <summary>
        /// Return all possible equivalent, subset, and known compatible frameworks.
        /// </summary>
        public IEnumerable<NuGetFramework> Expand(NuGetFramework framework)
        {
            var seen = new HashSet<NuGetFramework>() { framework };
            var toExpand = new Stack<NuGetFramework>();
            toExpand.Push(framework);

            while (toExpand.Count > 0)
            {
                foreach (var expansion in ExpandInternal(toExpand.Pop()))
                {
                    // only return distinct frameworks
                    if (seen.Add(expansion))
                    {
                        yield return expansion;

                        toExpand.Push(expansion);
                    }
                }
            }

            // This PCL check is done outside of the loop because we do not want
            // to recurse (via the stack above) on this PCL equivalence. The
            // intent here is to ensure that PCL should expand to netstandard,
            // but NOT to dotnet (which is deprecated).
            if (framework.IsPCL)
            {
                int profileNumber;
                IEnumerable<FrameworkRange> ranges;
                if (_mappings.TryGetPortableProfileNumber(framework.Profile, out profileNumber)
                    && _mappings.TryGetPortableCompatibilityMappings(profileNumber, out ranges))
                {
                    foreach (var range in ranges)
                    {
                        yield return range.Min;

                        if (!range.Min.Equals(range.Max))
                        {
                            yield return range.Max;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Finds all expansions using the mapping provider
        /// </summary>
        private IEnumerable<NuGetFramework> ExpandInternal(NuGetFramework framework)
        {
            // check the framework directly, this includes profiles which the range doesn't return
            IEnumerable<NuGetFramework> directlyEquivalent = null;
            if (_mappings.TryGetEquivalentFrameworks(framework, out directlyEquivalent))
            {
                foreach (var eqFw in directlyEquivalent)
                {
                    yield return eqFw;
                }
            }

            // 0.0 through the current framework
            var frameworkRange = new FrameworkRange(
                new NuGetFramework(framework.Framework, new Version(0, 0), framework.Profile),
                framework);

            IEnumerable<NuGetFramework> equivalent = null;
            if (_mappings.TryGetEquivalentFrameworks(frameworkRange, out equivalent))
            {
                foreach (var eqFw in equivalent)
                {
                    yield return eqFw;
                }
            }

            // find all possible sub set frameworks if no profile is used
            if (!framework.HasProfile)
            {
                IEnumerable<string> subSetFrameworks = null;
                if (_mappings.TryGetSubSetFrameworks(framework.Framework, out subSetFrameworks))
                {
                    foreach (var subFramework in subSetFrameworks)
                    {
                        // clone the framework but use the sub framework instead
                        yield return new NuGetFramework(subFramework, framework.Version, framework.Profile);
                    }
                }
            }

            // explicit compatiblity mappings
            IEnumerable<FrameworkRange> ranges = null;
            if (_mappings.TryGetCompatibilityMappings(framework, out ranges))
            {
                foreach (var range in ranges)
                {
                    yield return range.Min;

                    if (!range.Min.Equals(range.Max))
                    {
                        yield return range.Max;
                    }
                }
            }
        }
    }
}
