// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MSTestv2IntelliTestExtension
{
    using Microsoft.ExtendedReflection.Metadata.Names;
    using Microsoft.ExtendedReflection.Utilities.Safe.Diagnostics;

    /// <summary>
    /// The metadata.
    /// </summary>
    internal static class MSTestv2TestFrameworkMetadata
    {
        internal static readonly ShortAssemblyName AssemblyName = ShortAssemblyName.FromName("Microsoft.VisualStudio.TestPlatform.TestFramework");
        internal static readonly string RootNamespace = "Microsoft.VisualStudio.TestTools.UnitTesting";

        private static TypeName assertType;
        private static TypeName collectionAssertType;

        public static TypeDefinitionName AssertTypeDefinition
        {
            get
            {
                if (assertType == null)
                {
                    assertType = TypeName("Assert");
                }

                return assertType.Definition;
            }
        }

        public static TypeDefinitionName CollectionAssertTypeDefinition
        {
            get
            {
                if (collectionAssertType == null)
                {
                    collectionAssertType = TypeName("CollectionAssert");
                }

                return collectionAssertType.Definition;
            }
        }

        public static TypeName AttributeName(string name)
        {
            SafeDebug.AssumeNotNullOrEmpty(name, "name");

            return TypeDefinitionName.FromName(
                AssemblyName,
                -1,
                false,
                RootNamespace,
                name + "Attribute").SelfInstantiation;
        }

        private static TypeName TypeName(string name)
        {
            SafeDebug.AssumeNotNullOrEmpty(name, "name");
            return TypeDefinitionName.FromName(
                AssemblyName,
                -1,
                false,
                RootNamespace,
                name).SelfInstantiation;
        }
    }
}
