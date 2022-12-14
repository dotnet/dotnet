// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Internal;

namespace System.ComponentModel.Composition.ReflectionModel
{
    public static class ReflectionModelServices
    {
        public static Lazy<Type> GetPartType(ComposablePartDefinition partDefinition)
        {
            Requires.NotNull(partDefinition, nameof(partDefinition));

            ReflectionComposablePartDefinition? reflectionPartDefinition = partDefinition as ReflectionComposablePartDefinition;
            if (reflectionPartDefinition == null)
            {
                throw ExceptionBuilder.CreateReflectionModelInvalidPartDefinition(nameof(partDefinition), partDefinition.GetType());
            }

            return reflectionPartDefinition.GetLazyPartType();
        }

        public static bool IsDisposalRequired(ComposablePartDefinition partDefinition)
        {
            Requires.NotNull(partDefinition, nameof(partDefinition));

            ReflectionComposablePartDefinition? reflectionPartDefinition = partDefinition as ReflectionComposablePartDefinition;
            if (reflectionPartDefinition == null)
            {
                throw ExceptionBuilder.CreateReflectionModelInvalidPartDefinition(nameof(partDefinition), partDefinition.GetType());
            }

            return reflectionPartDefinition.IsDisposalRequired;
        }

        public static LazyMemberInfo GetExportingMember(ExportDefinition exportDefinition)
        {
            Requires.NotNull(exportDefinition, nameof(exportDefinition));

            ReflectionMemberExportDefinition? reflectionExportDefinition = exportDefinition as ReflectionMemberExportDefinition;
            if (reflectionExportDefinition == null)
            {
                throw new ArgumentException(
                    SR.Format(SR.ReflectionModel_InvalidExportDefinition, exportDefinition.GetType()),
                    nameof(exportDefinition));
            }

            return reflectionExportDefinition.ExportingLazyMember;
        }

        public static LazyMemberInfo GetImportingMember(ImportDefinition importDefinition)
        {
            Requires.NotNull(importDefinition, nameof(importDefinition));

            ReflectionMemberImportDefinition? reflectionMemberImportDefinition = importDefinition as ReflectionMemberImportDefinition;
            if (reflectionMemberImportDefinition == null)
            {
                throw new ArgumentException(
                    SR.Format(SR.ReflectionModel_InvalidMemberImportDefinition, importDefinition.GetType()),
                    nameof(importDefinition));
            }

            return reflectionMemberImportDefinition.ImportingLazyMember;
        }

        public static Lazy<ParameterInfo> GetImportingParameter(ImportDefinition importDefinition)
        {
            Requires.NotNull(importDefinition, nameof(importDefinition));

            ReflectionParameterImportDefinition? reflectionParameterImportDefinition = importDefinition as ReflectionParameterImportDefinition;
            if (reflectionParameterImportDefinition == null)
            {
                throw new ArgumentException(
                    SR.Format(SR.ReflectionModel_InvalidParameterImportDefinition, importDefinition.GetType()),
                    nameof(importDefinition));
            }

            Debug.Assert(reflectionParameterImportDefinition.ImportingLazyParameter != null);
            return reflectionParameterImportDefinition.ImportingLazyParameter;
        }

        public static bool IsImportingParameter(ImportDefinition importDefinition)
        {
            Requires.NotNull(importDefinition, nameof(importDefinition));

            ReflectionImportDefinition? reflectionImportDefinition = importDefinition as ReflectionImportDefinition;
            if (reflectionImportDefinition == null)
            {
                throw new ArgumentException(
                    SR.Format(SR.ReflectionModel_InvalidImportDefinition, importDefinition.GetType()),
                    nameof(importDefinition));
            }

            return (importDefinition is ReflectionParameterImportDefinition);
        }

        public static bool IsExportFactoryImportDefinition(ImportDefinition importDefinition)
        {
            Requires.NotNull(importDefinition, nameof(importDefinition));

            return (importDefinition is IPartCreatorImportDefinition);
        }

        public static ContractBasedImportDefinition GetExportFactoryProductImportDefinition(ImportDefinition importDefinition)
        {
            Requires.NotNull(importDefinition, nameof(importDefinition));

            IPartCreatorImportDefinition? partCreatorImportDefinition = importDefinition as IPartCreatorImportDefinition;
            if (partCreatorImportDefinition == null)
            {
                throw new ArgumentException(
                    SR.Format(SR.ReflectionModel_InvalidImportDefinition, importDefinition.GetType()),
                    nameof(importDefinition));
            }

            return partCreatorImportDefinition.ProductImportDefinition;
        }

        public static ComposablePartDefinition CreatePartDefinition(
            Lazy<Type> partType,
            bool isDisposalRequired,
            Lazy<IEnumerable<ImportDefinition>>? imports,
            Lazy<IEnumerable<ExportDefinition>>? exports,
            Lazy<IDictionary<string, object?>>? metadata,
            ICompositionElement? origin)
        {
            Requires.NotNull(partType, nameof(partType));

            return new ReflectionComposablePartDefinition(
                new ReflectionPartCreationInfo(
                    partType,
                    isDisposalRequired,
                    imports,
                    exports,
                    metadata,
                    origin));
        }

        public static ExportDefinition CreateExportDefinition(
            LazyMemberInfo exportingMember,
            string contractName,
            Lazy<IDictionary<string, object?>> metadata,
            ICompositionElement? origin)
        {
            Requires.NotNullOrEmpty(contractName, nameof(contractName));
            Requires.IsInMembertypeSet(exportingMember.MemberType, nameof(exportingMember), MemberTypes.Field | MemberTypes.Property | MemberTypes.NestedType | MemberTypes.TypeInfo | MemberTypes.Method);

            return new ReflectionMemberExportDefinition(
                exportingMember,
                new LazyExportDefinition(contractName, metadata),
                origin);
        }

        public static ContractBasedImportDefinition CreateImportDefinition(
            LazyMemberInfo importingMember,
            string contractName,
            string? requiredTypeIdentity,
            IEnumerable<KeyValuePair<string, Type>>? requiredMetadata,
            ImportCardinality cardinality,
            bool isRecomposable,
            CreationPolicy requiredCreationPolicy,
            ICompositionElement? origin)
        {
            return CreateImportDefinition(importingMember, contractName, requiredTypeIdentity, requiredMetadata, cardinality, isRecomposable, requiredCreationPolicy, MetadataServices.EmptyMetadata, false, origin);
        }

        public static ContractBasedImportDefinition CreateImportDefinition(
            LazyMemberInfo importingMember,
            string contractName,
            string? requiredTypeIdentity,
            IEnumerable<KeyValuePair<string, Type>>? requiredMetadata,
            ImportCardinality cardinality,
            bool isRecomposable,
            CreationPolicy requiredCreationPolicy,
            IDictionary<string, object?> metadata,
            bool isExportFactory,
            ICompositionElement? origin)
        {
            return CreateImportDefinition(
                importingMember,
                contractName,
                requiredTypeIdentity,
                requiredMetadata,
                cardinality,
                isRecomposable,
                false,
                requiredCreationPolicy,
                metadata,
                isExportFactory,
                origin);
        }

        public static ContractBasedImportDefinition CreateImportDefinition(
            LazyMemberInfo importingMember,
            string contractName,
            string? requiredTypeIdentity,
            IEnumerable<KeyValuePair<string, Type>>? requiredMetadata,
            ImportCardinality cardinality,
            bool isRecomposable,
            bool isPreRequisite,
            CreationPolicy requiredCreationPolicy,
            IDictionary<string, object?> metadata,
            bool isExportFactory,
            ICompositionElement? origin)
        {
            Requires.NotNullOrEmpty(contractName, nameof(contractName));
            Requires.IsInMembertypeSet(importingMember.MemberType, nameof(importingMember), MemberTypes.Property | MemberTypes.Field);

            if (isExportFactory)
            {
                return new PartCreatorMemberImportDefinition(
                    importingMember,
                    origin,
                    new ContractBasedImportDefinition(
                        contractName,
                        requiredTypeIdentity,
                        requiredMetadata,
                        cardinality,
                        isRecomposable,
                        isPreRequisite,
                        CreationPolicy.NonShared,
                        metadata));
            }
            else
            {
                return new ReflectionMemberImportDefinition(
                    importingMember,
                    contractName,
                    requiredTypeIdentity,
                    requiredMetadata,
                    cardinality,
                    isRecomposable,
                    isPreRequisite,
                    requiredCreationPolicy,
                    metadata,
                    origin);
            }
        }

        public static ContractBasedImportDefinition CreateImportDefinition(
            Lazy<ParameterInfo> parameter,
            string contractName,
            string? requiredTypeIdentity,
            IEnumerable<KeyValuePair<string, Type>>? requiredMetadata,
            ImportCardinality cardinality,
            CreationPolicy requiredCreationPolicy,
            ICompositionElement? origin)
        {
            return CreateImportDefinition(parameter, contractName, requiredTypeIdentity, requiredMetadata, cardinality, requiredCreationPolicy, MetadataServices.EmptyMetadata, false, origin);
        }

        public static ContractBasedImportDefinition CreateImportDefinition(
            Lazy<ParameterInfo> parameter,
            string contractName,
            string? requiredTypeIdentity,
            IEnumerable<KeyValuePair<string, Type>>? requiredMetadata,
            ImportCardinality cardinality,
            CreationPolicy requiredCreationPolicy,
            IDictionary<string, object?> metadata,
            bool isExportFactory,
            ICompositionElement? origin)
        {
            Requires.NotNull(parameter, nameof(parameter));
            Requires.NotNullOrEmpty(contractName, nameof(contractName));

            if (isExportFactory)
            {
                return new PartCreatorParameterImportDefinition(
                    parameter,
                    origin,
                    new ContractBasedImportDefinition(
                        contractName,
                        requiredTypeIdentity,
                        requiredMetadata,
                        cardinality,
                        false,
                        true,
                        CreationPolicy.NonShared,
                        metadata));
            }
            else
            {
                return new ReflectionParameterImportDefinition(
                    parameter,
                    contractName,
                    requiredTypeIdentity,
                    requiredMetadata,
                    cardinality,
                    requiredCreationPolicy,
                    metadata,
                    origin);
            }
        }

        public static bool TryMakeGenericPartDefinition(ComposablePartDefinition partDefinition, IEnumerable<Type> genericParameters, [NotNullWhen(true)] out ComposablePartDefinition? specialization)
        {
            Requires.NotNull(partDefinition, nameof(partDefinition));

            specialization = null;
            ReflectionComposablePartDefinition? reflectionPartDefinition = partDefinition as ReflectionComposablePartDefinition;
            if (reflectionPartDefinition == null)
            {
                throw ExceptionBuilder.CreateReflectionModelInvalidPartDefinition(nameof(partDefinition), partDefinition.GetType());
            }

            return reflectionPartDefinition.TryMakeGenericPartDefinition(genericParameters.ToArray(), out specialization);
        }
    }

    internal sealed class ReflectionPartCreationInfo : IReflectionPartCreationInfo
    {
        private readonly Lazy<Type> _partType;
        private readonly Lazy<IEnumerable<ImportDefinition>>? _imports;
        private readonly Lazy<IEnumerable<ExportDefinition>>? _exports;
        private readonly Lazy<IDictionary<string, object?>>? _metadata;
        private readonly ICompositionElement? _origin;
        private ConstructorInfo? _constructor;
        private readonly bool _isDisposalRequired;

        public ReflectionPartCreationInfo(
            Lazy<Type> partType,
            bool isDisposalRequired,
            Lazy<IEnumerable<ImportDefinition>>? imports,
            Lazy<IEnumerable<ExportDefinition>>? exports,
            Lazy<IDictionary<string, object?>>? metadata,
            ICompositionElement? origin)
        {
            ArgumentNullException.ThrowIfNull(partType);

            _partType = partType;
            _isDisposalRequired = isDisposalRequired;
            _imports = imports;
            _exports = exports;
            _metadata = metadata;
            _origin = origin;
        }

        public Type GetPartType()
        {
            return _partType.GetNotNullValue("type");
        }

        public Lazy<Type> GetLazyPartType()
        {
            return _partType;
        }

        public ConstructorInfo? GetConstructor()
        {
            if (_constructor == null)
            {
                ConstructorInfo[]? constructors = null;
                constructors = GetImports()
                    .OfType<ReflectionParameterImportDefinition>()
                    .Select(parameterImport => parameterImport.ImportingLazyParameter.Value.Member)
                    .OfType<ConstructorInfo>()
                    .Distinct()
                    .ToArray();

                if (constructors.Length == 1)
                {
                    _constructor = constructors[0];
                }
                else if (constructors.Length == 0)
                {
                    _constructor = GetPartType().GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                }
            }
            return _constructor;
        }

        public bool IsDisposalRequired
        {
            get
            {
                return _isDisposalRequired;
            }
        }

        public bool IsIdentityComparison
        {
            get
            {
                return true;
            }
        }

        public IDictionary<string, object?>? GetMetadata()
        {
            return _metadata?.Value;
        }

        public IEnumerable<ExportDefinition> GetExports()
        {
            if (_exports == null)
            {
                yield break;
            }

            IEnumerable<ExportDefinition> exports = _exports.Value;

            if (exports == null)
            {
                yield break;
            }

            foreach (ExportDefinition export in exports)
            {
                ReflectionMemberExportDefinition? reflectionExport = export as ReflectionMemberExportDefinition;
                if (reflectionExport == null)
                {
                    throw new InvalidOperationException(
                        SR.Format(SR.ReflectionModel_InvalidExportDefinition, export.GetType()));
                }
                yield return reflectionExport;
            }
        }

        public IEnumerable<ImportDefinition> GetImports()
        {
            if (_imports == null)
            {
                yield break;
            }

            IEnumerable<ImportDefinition> imports = _imports.Value;

            if (imports == null)
            {
                yield break;
            }

            foreach (ImportDefinition import in imports)
            {
                ReflectionImportDefinition? reflectionImport = import as ReflectionImportDefinition;
                if (reflectionImport == null)
                {
                    throw new InvalidOperationException(
                        SR.Format(SR.ReflectionModel_InvalidMemberImportDefinition, import.GetType()));
                }
                yield return reflectionImport;
            }
        }

        public string DisplayName
        {
            get { return GetPartType().GetDisplayName(); }
        }

        public ICompositionElement? Origin
        {
            get { return _origin; }
        }
    }

    internal sealed class LazyExportDefinition : ExportDefinition
    {
        private readonly Lazy<IDictionary<string, object?>> _metadata;

        public LazyExportDefinition(string contractName, Lazy<IDictionary<string, object?>> metadata)
            : base(contractName, (IDictionary<string, object?>?)null)
        {
            _metadata = metadata;
        }

        public override IDictionary<string, object?> Metadata
        {
            get
            {
                return _metadata.Value ?? MetadataServices.EmptyMetadata;
            }
        }
    }
}
