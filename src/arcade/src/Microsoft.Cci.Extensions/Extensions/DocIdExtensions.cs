// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using Microsoft.Cci;

namespace Microsoft.Cci.Extensions
{
    public static class DocIdExtensions
    {
        public static string DocId(this ICustomAttribute attribute)
        {
            FakeCustomAttribute fca = attribute as FakeCustomAttribute;
            if (fca != null)
                return fca.DocId;

            return attribute.Type.DocId();
        }

        public static string DocId(this ITypeReference type)
        {
            type = type.UnWrap();
            return TypeHelper.GetTypeName(type, NameFormattingOptions.DocumentationId);
        }

        public static string DocId(this ITypeMemberReference member)
        {
            //Do we need to unwrap members?
            //member = member.UnWrapMember();
            return MemberHelper.GetMemberSignature(member, NameFormattingOptions.DocumentationId);
        }

        public static string DocId(this INamespaceDefinition ns)
        {
            return DocId((IUnitNamespaceReference)ns);
        }

        public static string DocId(this IUnitNamespaceReference ns)
        {
            return "N:" + TypeHelper.GetNamespaceName(ns, NameFormattingOptions.None);
        }

        public static string DocId(this IAssemblyReference assembly)
        {
            return DocId(assembly.AssemblyIdentity);
        }

        public static string DocId(this AssemblyIdentity assembly)
        {
            return string.Format("A:{0}", assembly.Name.Value);
        }

        public static string DocId(this IPlatformInvokeInformation platformInvoke)
        {
            //return string.Format("I:{0}.{1}", platformInvoke.ImportModule.Name.Value, platformInvoke.ImportName.Value);

            // For now so we can use this to match up with the modern sdk names only include the pinvoke name in the identifier.
            return string.Format("{0}", platformInvoke.ImportName.Value);
        }

        public static string RefDocId(this IReference reference)
        {
            Contract.Requires(reference != null);

            ITypeReference type = reference as ITypeReference;
            if (type != null)
                return type.DocId();

            ITypeMemberReference member = reference as ITypeMemberReference;
            if (member != null)
                return member.DocId();

            IUnitNamespaceReference ns = reference as IUnitNamespaceReference;
            if (ns != null)
                return ns.DocId();

            IAssemblyReference assembly = reference as IAssemblyReference;
            if (assembly != null)
                return assembly.DocId();

            Contract.Assert(false, string.Format("Fell through cases in TypeExtensions.RefDocId() Type of reference: {0}", reference.GetType()));
            return "<Unknown Reference Type>";
        }

        public static IEnumerable<string> ReadDocIds(string docIdsFile)
        {
            if (!File.Exists(docIdsFile))
                yield break;

            foreach (string id in File.ReadAllLines(docIdsFile))
            {
                if (string.IsNullOrWhiteSpace(id) || id.StartsWith("#") || id.StartsWith("//"))
                    continue;

                yield return id.Trim();
            }
        }
    }
}
