// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.Utilities;
/* Expected attribute shape

    namespace Microsoft.VisualStudio.TestPlatform
    {
        using System;

        [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
        internal sealed class TestExtensionTypesV2Attribute : Attribute
        {
            public string ExtensionType { get; }
            public string ExtensionIdentifier { get; }
            public Type ExtensionImplementation { get; }
            public int Version { get; }

            public TestExtensionTypesV2Attribute(string extensionType, string extensionIdentifier, Type extensionImplementation, int version)
            {
                ExtensionType = extensionType;
                ExtensionIdentifier = extensionIdentifier;
                ExtensionImplementation = extensionImplementation;
                Version = version;
            }
        }
    }
*/
internal static class MetadataReaderExtensionsHelper
{
    private const string TestExtensionTypesAttributeV2 = "Microsoft.VisualStudio.TestPlatform.TestExtensionTypesV2Attribute";
    private static readonly ConcurrentDictionary<string, Type[]> AssemblyCache = new();
    private static readonly Type[] EmptyTypeArray = new Type[0];

    public static Type[] DiscoverTestExtensionTypesV2Attribute(Assembly loadedAssembly, string assemblyFilePath)
        => AssemblyCache.GetOrAdd(assemblyFilePath, DiscoverTestExtensionTypesV2AttributeInternal(loadedAssembly, assemblyFilePath));

    private static Type[] DiscoverTestExtensionTypesV2AttributeInternal(Assembly loadedAssembly, string assemblyFilePath)
    {
        EqtTrace.Verbose($"MetadataReaderExtensionsHelper: Discovering extensions inside assembly '{loadedAssembly.FullName}' file path '{assemblyFilePath}'");

        // We don't cache the load because this method is used by DiscoverTestExtensionTypesV2Attribute that caches the outcome Type[]
        Assembly assemblyToAnalyze;
        try
        {
            assemblyToAnalyze = Assembly.LoadFile(assemblyFilePath);
        }
        catch (Exception ex)
        {
            EqtTrace.Verbose($"MetadataReaderExtensionsHelper: Failure during assembly file load '{assemblyFilePath}', fallback to the loaded assembly.\n{FormatException(ex)}");
            assemblyToAnalyze = loadedAssembly;
        }

        List<Tuple<int, Type>>? extensions = null;
        using (var stream = new FileStream(assemblyFilePath, FileMode.Open, FileAccess.Read))
        using (var reader = new PEReader(stream, PEStreamOptions.Default))
        {
            MetadataReader metadataReader = reader.GetMetadataReader();

            // Search for the custom attribute TestExtensionTypesAttributeV2 - ECMA-335 II.22.10 CustomAttribute : 0x0C
            foreach (var customAttributeHandle in metadataReader.CustomAttributes)
            {
                string? attributeFullName = null;
                try
                {
                    if (customAttributeHandle.IsNil)
                    {
                        continue;
                    }

                    // Read custom attribute metadata row
                    var customAttribute = metadataReader.GetCustomAttribute(customAttributeHandle);

                    // We expect that the attribute is defined inside current assembly by the extension owner
                    // and so ctor should point to the MethodDefinition table - ECMA-335 II.22.26 MethodDef : 0x06
                    // and parent scope should be the current assembly [assembly:...] - ECMA-335 II.22.2 Assembly : 0x20
                    if (customAttribute.Constructor.Kind != HandleKind.MethodDefinition || customAttribute.Parent.Kind != HandleKind.AssemblyDefinition)
                    {
                        continue;
                    }

                    // Read MethodDef metadata row
                    var methodDefinition = metadataReader.GetMethodDefinition((MethodDefinitionHandle)customAttribute.Constructor);

                    // Check that name is .ctor
                    if (metadataReader.GetString(methodDefinition.Name) != ".ctor")
                    {
                        continue;
                    }

                    // Get the custom attribute TypeDef handle
                    var typeDefinitionHandle = methodDefinition.GetDeclaringType();

                    // Read TypeDef metadata row
                    var typeDef = metadataReader.GetTypeDefinition(typeDefinitionHandle);

                    // Check the attribute type full name
                    attributeFullName = $"{metadataReader.GetString(typeDef.Namespace)}.{metadataReader.GetString(typeDef.Name)}";
                    if (attributeFullName == TestExtensionTypesAttributeV2)
                    {
                        // We don't do any signature verification to allow future possibility to add new parameter in a back compat way.
                        // Get signature blob using methodDefinition.Signature index into Blob heap - ECMA-335 II.22.26 MethodDef : 0x06, 'Signature' column
                        // BlobReader signatureReader = metadataReader.GetBlobReader(methodDefinition.Signature);
                        // var decoder = new SignatureDecoder<string, object>(new SignatureDecoder(), metadataReader, genericContext: null);
                        // var ctorDecodedSignature = decoder.DecodeMethodSignature(ref signatureReader);
                        // Log the signature for analysis purpose
                        // EqtTrace.Verbose($"MetadataReaderExtensionsHelper: Find possible good attribute candidate '{attributeFullName}' with ctor signature: '{ctorDecodedSignature.ReturnType}" +
                        //    $" ctor ({(ctorDecodedSignature.ParameterTypes.Length > 0 ? ctorDecodedSignature.ParameterTypes.Aggregate((a, b) => $"{a},{b}").Trim(',') : "(")})'");

                        // Read the ctor signature values - ECMA-335 II.23.3 Custom attributes
                        BlobReader valueReader = metadataReader.GetBlobReader(customAttribute.Value);
                        // Verify the prolog
                        if (valueReader.ReadUInt16() == 1)
                        {
                            // Expected ctor shape ctor(string,string,System.Type,Int32)
                            // [assembly: TestExtensionTypesV2(ExtensionMetadata.ExtensionType, ExtensionMetadata.Uri, typeof(ExtensionImplementation), 1)]

                            // If the parameter kind is string, (middle line in above diagram) then the blob contains
                            // a SerString – a PackedLen count of bytes, followed by the UTF8 characters.If the
                            // string is null, its PackedLen has the value 0xFF(with no following characters).If
                            // the string is empty(“”), then PackedLen has the value 0x00(with no following
                            // characters).
                            string? extension = valueReader.ReadSerializedString();
                            string? extensionIdentifier = valueReader.ReadSerializedString();

                            // If the parameter kind is System.Type, (also, the middle line in above diagram) its
                            // value is stored as a SerString(as defined in the previous paragraph), representing its
                            // canonical name. The canonical name is its full type name, followed optionally by
                            // the assembly where it is defined, its version, culture and public-key-token.If the
                            // assembly name is omitted, the CLI looks first in the current assembly, and then in
                            // the system library(mscorlib); in these two special cases, it is permitted to omit the
                            // assembly-name, version, culture and public-key-token.
                            string? extensionImplementation = valueReader.ReadSerializedString();

                            // If the parameter kind is simple(first line in the above diagram) (bool, char, float32,
                            // float64, int8, int16, int32, int64, unsigned int8, unsigned int16, unsigned int32 or
                            // unsigned int64) then the 'blob' contains its binary value(Val). (A bool is a single
                            // byte with value 0(false) or 1(true); char is a two-byte Unicode character; and the
                            // others have their obvious meaning.) This pattern is also used if the parameter kind is
                            // an enum -- simply store the value of the enum's underlying integer type.
                            int version = valueReader.ReadInt32();
                            try
                            {
                                TPDebug.Assert(extensionImplementation is not null, "extensionImplementation is null");
                                var extensionType = assemblyToAnalyze.GetType(extensionImplementation);
                                if (extensionType is null)
                                {
                                    EqtTrace.Verbose($"MetadataReaderExtensionsHelper: Unable to get extension type for '{extensionImplementation}'");
                                    continue;
                                }

                                extensions ??= new List<Tuple<int, Type>>();
                                extensions.Add(Tuple.Create(version, extensionType));
                                EqtTrace.Verbose($"MetadataReaderExtensionsHelper: Valid extension found: extension type '{extension}' identifier '{extensionIdentifier}' implementation '{extensionType}' version '{version}'");
                            }
                            catch (Exception ex)
                            {
                                EqtTrace.Verbose($"MetadataReaderExtensionsHelper: Failure during type creation, extension full name: '{extensionImplementation}'\n{FormatException(ex)}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    EqtTrace.Verbose($"MetadataReaderExtensionsHelper: Failure during custom attribute analysis, attribute full name: {attributeFullName}\n{FormatException(ex)}");
                }
            }
        }

        return extensions?.OrderByDescending(t => t.Item1).Select(t => t.Item2).ToArray() ?? EmptyTypeArray;
    }

    private static string FormatException(Exception ex)
    {
        StringBuilder log = new();
        Exception? current = ex;
        while (current != null)
        {
            log.AppendLine(current.ToString());
            current = current.InnerException;
        }

        return log.ToString();
    }
}
