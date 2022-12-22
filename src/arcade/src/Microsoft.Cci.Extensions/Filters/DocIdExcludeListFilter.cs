// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Cci.Extensions;
using System.IO;
using Microsoft.Cci.Extensions.CSharp;

namespace Microsoft.Cci.Filters
{
    public class DocIdExcludeListFilter : ICciFilter
    {
        private readonly HashSet<string> _docIds;
        private readonly bool _excludeMembers;

        public DocIdExcludeListFilter(IEnumerable<string> docIds, bool excludeMembers)
        {
            _docIds = new HashSet<string>(docIds);
            _excludeMembers = excludeMembers;
        }

        public DocIdExcludeListFilter(string excludeListFilePath, bool excludeMembers)
        {
            _docIds = new HashSet<string>(DocIdExtensions.ReadDocIds(excludeListFilePath));
            _excludeMembers = excludeMembers;
        }

        public bool IncludeForwardedTypes { get; set; }

        public bool Include(INamespaceDefinition ns)
        {
            // Only include non-empty namespaces
            if (!ns.GetTypes(IncludeForwardedTypes).Any(Include))
                return false;

            string namespaceId = ns.DocId();

            // include so long as it isn't in the exclude list.
            return !_docIds.Contains(namespaceId);
        }

        public bool Include(ITypeDefinition type)
        {
            string typeId = type.DocId();

            // include so long as it isn't in the exclude list.
            return !_docIds.Contains(typeId);
        }

        public bool Include(ITypeDefinitionMember member)
        {
            if (_excludeMembers)
            {
                // if return type is an excluded type
                ITypeReference returnType = member.GetReturnType();
                if (returnType != null && !IncludeTypeReference(returnType))
                    return false;
                
                // if any parameter is an excluded type
                IMethodDefinition method = member as IMethodDefinition;
                if (method != null && method.Parameters.Any(param => !IncludeTypeReference(param.Type)))
                    return false;
            }

            string memberId = member.DocId();
            // include so long as it isn't in the exclude list.
            return !_docIds.Contains(memberId);
        }

        private bool IncludeTypeReference(ITypeReference type)
        {
            // if a generic type and one of the generic arguments are excluded
            IGenericTypeInstanceReference genericType = type as IGenericTypeInstanceReference;
            if (genericType != null && genericType.GenericArguments.Any(genArg => _docIds.Contains(genArg.DocId())))
                return false;

            // if the type itself is excluded
            string typeId = type.DocId();
            return !_docIds.Contains(typeId);

        }

        public bool Include(ICustomAttribute attribute)
        {
            string typeId = attribute.DocId();
            string removeUsages = "RemoveUsages:" + typeId;

            // special case: attribute usage can be removed without removing 
            //               the attribute itself
            if (_docIds.Contains(removeUsages))
                return false;

            if (_excludeMembers)
            {
                foreach(var argument in attribute.Arguments)
                {
                    // if the argument is an excluded type
                    if (!IncludeTypeReference(argument.Type))
                        return false;

                    // if the argument is typeof of an excluded type
                    IMetadataTypeOf typeOf = argument as IMetadataTypeOf;
                    if (typeOf != null && !IncludeTypeReference(typeOf.TypeToGet))
                        return false;
                }
            }

            // include so long as it isn't in the exclude list.
            return !_docIds.Contains(typeId);
        }
    }
}
