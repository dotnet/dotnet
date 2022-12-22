// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Build.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Microsoft.DotNet.GenFacades
{
    public class GenPartialFacadeSourceGenerator
    {
        public static bool Execute(
            string[] seeds,
            string contractAssembly,
            string[] compileFiles,
            string defineConstants,
            string langVersion,
            string outputSourcePath,
            ILog logger,
            bool ignoreMissingTypes = false,
            string[] ignoreMissingTypesList = null,
            string[] OmitTypes = null,
            ITaskItem[] seedTypePreferencesList = null)
        {
            Dictionary<string, string> seedTypePreferences = ParseSeedTypePreferences(seedTypePreferencesList, logger);

            IEnumerable<string> referenceTypes = GetPublicVisibleTypes(contractAssembly, includeTypeForwards: true);

            // Normalizing and Removing Relative Segments from the seed paths.
            string[] distinctSeeds = seeds.Select(seed => Path.GetFullPath(seed)).Distinct().ToArray();
            string[] seedNames = distinctSeeds.Select(seed => Path.GetFileName(seed)).ToArray();

            if (distinctSeeds.Count() != seedNames.Distinct(StringComparer.InvariantCultureIgnoreCase).Count())
            {
                IEnumerable<string> duplicates = seedNames.GroupBy(x => x)
                    .Where(g => g.Count() > 1)
                    .Select(y => y.Key);

                logger.LogError("There are multiple versions of these assemblies: {0}. Please provide a single version.", string.Join(", ", duplicates));
                return false;
            }

            IReadOnlyDictionary<string, IList<string>> seedTypes = GenerateTypeTable(distinctSeeds);

            if (OmitTypes != null)
                referenceTypes = referenceTypes.Where(type => !OmitTypes.Contains(type));

            var sourceGenerator = new SourceGenerator(referenceTypes, seedTypes, seedTypePreferences, outputSourcePath, ignoreMissingTypesList, logger);
            return sourceGenerator.GenerateSource(compileFiles, ParseDefineConstants(defineConstants), langVersion, ignoreMissingTypes);
        }

        private static IEnumerable<string> ParseDefineConstants(string defineConstants)
        {
            return defineConstants?.Split(';', ',').Where(t => !string.IsNullOrEmpty(t)).ToArray();
        }

        private static Dictionary<string, string> ParseSeedTypePreferences(ITaskItem[] preferences, ILog logger)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.Ordinal);

            if (preferences != null)
            {
                foreach (var item in preferences)
                {
                    string key = item.ItemSpec;
                    string value = item.GetMetadata("Aliases");

                    if (value != null)
                    {
                        string existingValue;

                        if (dictionary.TryGetValue(key, out existingValue))
                        {
                            logger.LogWarning($"Overriding SeedType{existingValue} for type {key} with {value}");
                        }

                        dictionary[key] = value;
                    }
                    else
                    {
                        logger.LogWarning($"No Alias has been provided for type {key}");
                    }
                }
            }

            return dictionary;
        }

        private static IEnumerable<string> GetPublicVisibleTypes(string assembly, bool includeTypeForwards = false)
        {
            using (var peReader = new PEReader(new FileStream(assembly, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read)))
            {
                if (peReader.HasMetadata)
                {
                    MetadataReader reader = peReader.GetMetadataReader();

                    // Enumerating typeDefinatons
                    foreach (var typeDefinationHandle in reader.TypeDefinitions)
                    {
                        TypeDefinition typeDefination = reader.GetTypeDefinition(typeDefinationHandle);

                        // Ignoring Nested types
                        if (!typeDefination.IsNested && IsPublic(typeDefination))
                        {
                            yield return GetTypeName(typeDefination.Namespace, typeDefination.Name, reader);
                        }
                    }
                    
                    if (includeTypeForwards)
                    {
                        // Enumerating typeforwards
                        foreach (var exportedTypeHandle in reader.ExportedTypes)
                        {
                            ExportedType exportedType = reader.GetExportedType(exportedTypeHandle);
                            if (exportedType.IsForwarder)
                            {
                                yield return GetTypeName(exportedType.Namespace, exportedType.Name, reader);
                            }
                        }
                    }
                }
            }
        }

        private static string GetTypeName(StringHandle namespaceHandle, StringHandle typeHandle, MetadataReader reader)
        {            
            return namespaceHandle.IsNil ? reader.GetString(typeHandle) : reader.GetString(namespaceHandle) + "." + reader.GetString(typeHandle);
        }

        // This acts as a filter for public types.
        private static bool IsPublic(TypeDefinition typeDefination)
        {
            return (typeDefination.Attributes & TypeAttributes.Public) != 0;
        }

        private static IReadOnlyDictionary<string, IList<string>> GenerateTypeTable(IEnumerable<string> seedAssemblies)
        {
            var typeTable = new Dictionary<string, IList<string>>();
            foreach(string assembly in seedAssemblies)
            {                
                IEnumerable<string> types = GetPublicVisibleTypes(assembly);
                foreach (string type in types)
                {
                    AddTypeToTable(typeTable, type, Path.GetFileName(assembly));
                }
            }
            return typeTable;
        }

        private static void AddTypeToTable(Dictionary<string, IList<string>> typeTable, string type, string assemblyName)
        {
            if (type != null)
            {
                IList<string> assemblyListForTypes;
                if (!typeTable.TryGetValue(type, out assemblyListForTypes))
                {
                    assemblyListForTypes = new List<string>(1);
                    typeTable.Add(type, assemblyListForTypes);
                }
                assemblyListForTypes.Add(assemblyName);
            }
        }
    }
}
