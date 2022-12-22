// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace MSTestv2IntelliTestExtension
{
    using Microsoft.ExtendedReflection.Asserts;
    using Microsoft.ExtendedReflection.Metadata;
    using Microsoft.ExtendedReflection.Utilities.Safe.Diagnostics;

    /// <summary>
    /// The MSTestv2 assert method filer.
    /// </summary>
    internal sealed class MSTestv2AssertMethodFilter : IAssertMethodFilter
    {
        private static MSTestv2AssertMethodFilter instance = new MSTestv2AssertMethodFilter();

        private MSTestv2AssertMethodFilter()
        {
        }

        public static MSTestv2AssertMethodFilter Instance
        {
            get
            {
                return instance;
            }
        }

        public bool IsAssertMethod(MethodDefinition method, out int usefulParameters)
        {
            SafeDebug.AssumeNotNull(method, "method");
            TypeDefinition type;
            if (method.TryGetDeclaringType(out type))
            {
                if (type.SerializableName == MSTestv2TestFrameworkMetadata.AssertTypeDefinition)
                {
                    switch (method.ShortName)
                    {
                        case "IsFalse":
                        case "IsTrue":
                        case "IsNull":
                        case "IsNotNull":
                        case "IsInstanceOfType":
                        case "IsNotInstanceOfType":
                            usefulParameters = 1;
                            return true;
                        case "AreEqual":
                        case "AreNotEqual":
                        case "AreSame":
                        case "AreNotSame":
                            usefulParameters = 2;
                            return true;
                    }
                }
            }

            usefulParameters = -1;
            return false;
        }
    }
}
