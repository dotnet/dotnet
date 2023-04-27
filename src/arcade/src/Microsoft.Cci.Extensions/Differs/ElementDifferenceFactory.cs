// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Composition;
using System.Composition.Hosting;
using CompositionContainer = System.Composition.Hosting.CompositionHost;
using Microsoft.Cci.Mappings;

namespace Microsoft.Cci.Differs
{
    public class ElementDifferenceFactory : IElementDifferenceFactory
    {
        private CompositionContainer _container;
        private IDifferenceRule[] _diffRules;
        private Func<IDifferenceRuleMetadata, bool> _ruleFilter;

        public ElementDifferenceFactory()
        {
        }

        public ElementDifferenceFactory(CompositionContainer container, Func<IDifferenceRuleMetadata, bool> ruleFilter = null)
        {
            Contract.Requires(container != null);
            _container = container;
            _ruleFilter = ruleFilter;
        }

        public IDifferences GetDiffer<T>(ElementMapping<T> element) where T : class
        {
            return new ElementDiffer<T>(element, GetDifferenceRules<T>());
        }

        private IDifferenceRule[] GetDifferenceRules<T>() where T : class
        {
            EnsureContainer();

            if (_diffRules == null)
            {
                var rules = _container.GetExports<ExportFactory<IDifferenceRule, DifferenceRuleMetadata>>();
                if (_ruleFilter != null)
                {
                    rules = rules.Where(r => _ruleFilter(r.Metadata));
                }
                _diffRules = rules.Select(r => r.CreateExport().Value).ToArray();
            }

            return _diffRules;
        }

        private void EnsureContainer()
        {
            if (_container != null)
                return;

            var configuration = new ContainerConfiguration().WithAssembly(typeof(ElementDifferenceFactory).GetTypeInfo().Assembly);
            _container = configuration.CreateContainer();
        }
    }
}
