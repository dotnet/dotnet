// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Reflection;

namespace System.ComponentModel.Composition.AttributedModel
{
    internal sealed class AttributedExportDefinition : ExportDefinition
    {
        private readonly AttributedPartCreationInfo _partCreationInfo;
        private readonly MemberInfo _member;
        private readonly ExportAttribute _exportAttribute;
        private readonly Type? _typeIdentityType;

        private IDictionary<string, object?>? _metadata;

        public AttributedExportDefinition(AttributedPartCreationInfo partCreationInfo, MemberInfo member, ExportAttribute exportAttribute, Type? typeIdentityType, string contractName)
            : base(contractName, (IDictionary<string, object?>?)null)
        {
            ArgumentNullException.ThrowIfNull(partCreationInfo);
            ArgumentNullException.ThrowIfNull(member);
            ArgumentNullException.ThrowIfNull(exportAttribute);

            _partCreationInfo = partCreationInfo;
            _member = member;
            _exportAttribute = exportAttribute;
            _typeIdentityType = typeIdentityType;
        }

        public override IDictionary<string, object?> Metadata
        {
            get
            {
                if (_metadata == null)
                {
                    _member.TryExportMetadataForMember(out IDictionary<string, object?> metadata);

                    string typeIdentity = _exportAttribute.IsContractNameSameAsTypeIdentity() ?
                        ContractName :
                        _member.GetTypeIdentityFromExport(_typeIdentityType);

                    metadata.Add(CompositionConstants.ExportTypeIdentityMetadataName, typeIdentity);

                    var partMetadata = _partCreationInfo.GetMetadata();
                    if (partMetadata != null && partMetadata.TryGetValue(CompositionConstants.PartCreationPolicyMetadataName, out object? value))
                    {
                        metadata.Add(CompositionConstants.PartCreationPolicyMetadataName, value);
                    }

                    if ((_typeIdentityType != null) && (_member.MemberType != MemberTypes.Method) && _typeIdentityType.ContainsGenericParameters)
                    {
                        metadata.Add(CompositionConstants.GenericExportParametersOrderMetadataName, GenericServices.GetGenericParametersOrder(_typeIdentityType));
                    }

                    _metadata = metadata;
                }
                return _metadata;
            }
        }
    }

}
