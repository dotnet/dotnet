// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Composition.Hosting;
using System.Composition.Hosting.Core;
using System.Linq;

namespace Microsoft.DotNet.AsmDiff
{
    static class MefHelpers
    {
        public static ContainerConfiguration WithExport<T>(this ContainerConfiguration configuration, T exportedInstance, string contractName = null, IDictionary<string, object> metadata = null)
        {
            return WithExport(configuration, exportedInstance, typeof(T), contractName, metadata);
        }

        public static ContainerConfiguration WithExport(this ContainerConfiguration configuration, object exportedInstance, Type contractType, string contractName = null, IDictionary<string, object> metadata = null)
        {
            return configuration.WithProvider(new InstanceExportDescriptorProvider(
                exportedInstance, contractType, contractName, metadata));
        }

        abstract class SinglePartExportDescriptorProvider : ExportDescriptorProvider
        {
            readonly Type _contractType;
            readonly string _contractName;

            protected SinglePartExportDescriptorProvider(Type contractType, string contractName, IDictionary<string, object> metadata)
            {
                if (contractType == null) throw new ArgumentNullException("contractType");

                _contractType = contractType;
                _contractName = contractName;
                Metadata = metadata ?? new Dictionary<string, object>();
            }

            protected bool IsSupportedContract(CompositionContract contract)
            {
                if (contract.ContractType != _contractType ||
                    contract.ContractName != _contractName)
                    return false;

                if (contract.MetadataConstraints != null)
                {
                    var subsetOfConstraints = contract.MetadataConstraints.Where(c => Metadata.ContainsKey(c.Key)).ToDictionary(c => c.Key, c => Metadata[c.Key]);
                    var constrainedSubset = new CompositionContract(contract.ContractType, contract.ContractName,
                        subsetOfConstraints.Count == 0 ? null : subsetOfConstraints);

                    if (!contract.Equals(constrainedSubset))
                        return false;
                }

                return true;
            }

            protected IDictionary<string, object> Metadata { get; }
        }

        // This one-instance-per-provider design is not efficient for more than a few instances;
        // we're just aiming to show the mechanics here.
        class InstanceExportDescriptorProvider : SinglePartExportDescriptorProvider
        {
            object _exportedInstance;

            public InstanceExportDescriptorProvider(object exportedInstance, Type contractType, string contractName, IDictionary<string, object> metadata)
                : base(contractType, contractName, metadata)
            {
                if (exportedInstance == null) throw new ArgumentNullException("exportedInstance");
                _exportedInstance = exportedInstance;
            }

            public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
            {
                if (IsSupportedContract(contract))
                    yield return new ExportDescriptorPromise(contract, _exportedInstance.ToString(), true, NoDependencies, _ =>
                        ExportDescriptor.Create((c, o) => _exportedInstance, Metadata));
            }
        }
    }
}
