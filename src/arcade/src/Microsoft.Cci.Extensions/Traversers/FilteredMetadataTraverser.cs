// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Cci.Filters;

namespace Microsoft.Cci.Traversers
{
    public class FilteredMetadataTraverser : MetadataTraverser
    {
        private readonly ICciFilter _filter;

        public FilteredMetadataTraverser(ICciFilter filter)
        {
            _filter = filter ?? new IncludeAllFilter();
        }

        public ICciFilter Filter { get { return _filter; } }

        #region Filter Namespaces
        public override void TraverseChildren(INamespaceDefinition namespaceDefinition)
        {
            if (!_filter.Include(namespaceDefinition))
                return;
            base.TraverseChildren(namespaceDefinition);
        }

        public override void TraverseChildren(IRootUnitNamespace rootUnitNamespace)
        {
            if (!_filter.Include(rootUnitNamespace))
                return;
            base.TraverseChildren(rootUnitNamespace);
        }

        public override void TraverseChildren(IRootUnitSetNamespace rootUnitSetNamespace)
        {
            if (!_filter.Include(rootUnitSetNamespace))
                return;
            base.TraverseChildren(rootUnitSetNamespace);
        }

        public override void TraverseChildren(INestedUnitNamespace nestedUnitNamespace)
        {
            if (!_filter.Include(nestedUnitNamespace))
                return;
            base.TraverseChildren(nestedUnitNamespace);
        }
        #endregion

        #region Filter Types
        public override void TraverseChildren(INamedTypeDefinition namedTypeDefinition)
        {
            if (!_filter.Include(namedTypeDefinition))
                return;
            base.TraverseChildren(namedTypeDefinition);
        }

        public override void TraverseChildren(ITypeDefinition typeDefinition)
        {
            if (!_filter.Include(typeDefinition))
                return;
            base.TraverseChildren(typeDefinition);
        }

        public override void TraverseChildren(INamespaceTypeDefinition namespaceTypeDefinition)
        {
            if (!_filter.Include(namespaceTypeDefinition))
                return;
            base.TraverseChildren(namespaceTypeDefinition);
        }

        public override void TraverseChildren(INestedTypeDefinition nestedTypeDefinition)
        {
            if (!_filter.Include((ITypeDefinition)nestedTypeDefinition))
                return;
            base.TraverseChildren(nestedTypeDefinition);
        }
        #endregion

        #region Filter Members
        public override void TraverseChildren(ITypeDefinitionMember typeMember)
        {
            if (!_filter.Include(typeMember))
                return;
            base.TraverseChildren(typeMember);
        }

        public override void TraverseChildren(IEventDefinition eventDefinition)
        {
            if (!_filter.Include(eventDefinition))
                return;
            base.TraverseChildren(eventDefinition);
        }

        public override void TraverseChildren(IFieldDefinition fieldDefinition)
        {
            if (!_filter.Include(fieldDefinition))
                return;
            base.TraverseChildren(fieldDefinition);
        }

        public override void TraverseChildren(IMethodDefinition method)
        {
            if (!_filter.Include(method))
                return;
            base.TraverseChildren(method);
        }

        public override void TraverseChildren(IPropertyDefinition propertyDefinition)
        {
            if (!_filter.Include(propertyDefinition))
                return;
            base.TraverseChildren(propertyDefinition);
        }
        #endregion

        #region Filter Attributes
        public override void TraverseChildren(ICustomAttribute customAttribute)
        {
            if (!_filter.Include(customAttribute))
                return;
            base.TraverseChildren(customAttribute);
        }
        #endregion
    }
}
