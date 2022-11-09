// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using NuGet.Frameworks;

namespace NuGet.Test.Utility
{
    internal static class AssemblyReader
    {
        internal static NuGetFramework GetTargetFramework(string assemblyPath)
        {
            using (var fileStream = File.OpenRead(assemblyPath))
            using (var peReader = new PEReader(fileStream))
            {
                var metadataReader = peReader.GetMetadataReader();
                var targetFrameworkAttribute = GetTargetFrameworkAttribute(metadataReader);
                var targetFrameworkString = GetTargetFrameworkString(targetFrameworkAttribute, metadataReader);
                var targetFramework = NuGetFramework.Parse(targetFrameworkString);
                return targetFramework;
            }
        }

        private static CustomAttribute GetTargetFrameworkAttribute(MetadataReader metadataReader)
        {
            foreach (CustomAttributeHandle customAttributeHandle in metadataReader.CustomAttributes)
            {
                CustomAttribute customAttribute = metadataReader.GetCustomAttribute(customAttributeHandle);

                if (customAttribute.Constructor.Kind == HandleKind.MemberReference)
                {
                    MemberReference memberReference = metadataReader.GetMemberReference((MemberReferenceHandle)customAttribute.Constructor);
                    string name = GetMemberName(memberReference, metadataReader);

                    if (name == "System.Runtime.Versioning.TargetFrameworkAttribute..ctor")
                    {
                        return customAttribute;
                    }
                }
            }

            throw new Exception("Assembly doesn't have a TargetFrameworkAttribute");
        }

        private static string GetMemberName(MemberReference memberReference, MetadataReader metadataReader)
        {
            if (memberReference.Parent.Kind != HandleKind.TypeReference)
            {
                throw new NotSupportedException("Didn't expect member reference parent to be of kind " + memberReference.Parent.Kind);
            }

            var typeReference = metadataReader.GetTypeReference((TypeReferenceHandle)memberReference.Parent);

            var memberName = metadataReader.GetString(memberReference.Name);
            var typeName = metadataReader.GetString(typeReference.Name);
            var namespaceName = metadataReader.GetString(typeReference.Namespace);

            return namespaceName + "." + typeName + "." + memberName;
        }

        private static string GetTargetFrameworkString(CustomAttribute targetFrameworkAttribute, MetadataReader metadataReader)
        {
            var blobReader = metadataReader.GetBlobReader(targetFrameworkAttribute.Value);

            // I'm not sure if this is IL, or just a list of arguments, but the first value is a 16-bit number with the value 1
            blobReader.ReadInt16();

            // Next is the string passed to the TargetFrameworkAttribute constructor
            return blobReader.ReadSerializedString();
        }
    }
}
