// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;

namespace NuGet.Frameworks
{
    /// <summary>
    /// Creates a table of compatible frameworks.
    /// </summary>
    public class CompatibilityTable
    {
        private readonly IFrameworkNameProvider _mappings;
        private readonly IFrameworkCompatibilityProvider _compat;
        private readonly Dictionary<NuGetFramework, HashSet<NuGetFramework>> _table;
        private readonly FrameworkReducer _reducer;

        public CompatibilityTable(IEnumerable<NuGetFramework> frameworks)
            : this(frameworks,
                DefaultFrameworkNameProvider.Instance,
                DefaultCompatibilityProvider.Instance)
        {
        }

        public CompatibilityTable(IEnumerable<NuGetFramework> frameworks, IFrameworkNameProvider mappings, IFrameworkCompatibilityProvider compat)
        {
            _compat = compat;
            _mappings = mappings;
            _table = GetTable(frameworks, _compat);
            _reducer = new FrameworkReducer(_mappings, _compat);
        }

        /// <summary>
        /// True if the framework is in the table.
        /// </summary>
        public bool HasFramework(NuGetFramework framework)
        {
            return _table.ContainsKey(framework);
        }

        /// <summary>
        /// Gives the smallest set of frameworks from the table that cover everything the given framework would cover.
        /// </summary>
        public IEnumerable<NuGetFramework> GetNearest(NuGetFramework framework)
        {
            // start with everything compatible with the framework
            var allCompatible = _table.Keys.Where(f => _compat.IsCompatible(framework, f));

            return _reducer.ReduceUpwards(allCompatible);
        }

        /// <summary>
        /// Returns the list of all frameworks compatible with the given framework
        /// </summary>
        public bool TryGetCompatible(NuGetFramework framework, out IEnumerable<NuGetFramework> compatible)
        {
            HashSet<NuGetFramework> frameworks = null;
            if (_table.TryGetValue(framework, out frameworks))
            {
                compatible = new HashSet<NuGetFramework>(frameworks);
                return true;
            }

            compatible = null;
            return false;
        }

        private static Dictionary<NuGetFramework, HashSet<NuGetFramework>> GetTable(IEnumerable<NuGetFramework> frameworks, IFrameworkCompatibilityProvider compat)
        {
            // get the distinct set of frameworks, ignoring all special frameworks like Any, and Unsupported
            var input = new HashSet<NuGetFramework>(frameworks.Where(f => f.IsSpecificFramework));
            var table = new Dictionary<NuGetFramework, HashSet<NuGetFramework>>();

            foreach (var framework in input)
            {
                var compatFrameworks = new HashSet<NuGetFramework>();
                table.Add(framework, compatFrameworks);

                foreach (var testFramework in input)
                {
                    if (compat.IsCompatible(framework, testFramework))
                    {
                        compatFrameworks.Add(testFramework);
                    }
                }
            }

            return table;
        }
    }
}
