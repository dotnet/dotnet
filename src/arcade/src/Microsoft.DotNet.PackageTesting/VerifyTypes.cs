// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

namespace Microsoft.DotNet.PackageTesting
{
    /// <summary>
    /// Verifies no type overlap in a set of DLLs
    /// </summary>
    public class VerifyTypes : BuildTask
    {
        /// <summary>
        /// Sources to scan.  Items can be directories or files.
        /// </summary>
        [Required]
        public ITaskItem[] Sources { get; set; }
        
        /// <summary>
        /// Dependencies to ignore.
        ///     Identity: FileName
        ///     Version: Maximum version to ignore, if not specified all versions will be ignored.
        /// </summary>
        public ITaskItem[] IgnoredTypes { get; set; }

        private Dictionary<string, AssemblyInfo> assemblies = new Dictionary<string, AssemblyInfo>();
        private HashSet<string> ignoredTypes = new HashSet<string>();

        public override bool Execute()
        {
            if (Sources == null || Sources.Length == 0)
            {
                Log.LogError("No sources to scan.");
                return false;
            }

            LoadIgnoredTypes();
            LoadSources();

            var types = new Dictionary<string, AssemblyInfo>();

            foreach(var assembly in assemblies.Values)
            {
                foreach (var type in assembly.Types)
                {
                    AssemblyInfo existingAssembly;

                    if (types.TryGetValue(type, out existingAssembly))
                    {
                        if (ignoredTypes.Contains(type))
                        {
                            Log.LogMessage($"Ignored duplicate type {type} in both {existingAssembly.Path} and {assembly.Path}.");
                        }
                        else
                        {
                            Log.LogError($"Duplicate type {type} in both {existingAssembly.Path} and {assembly.Path}.");
                        }
                    }
                    else
                    {
                        types.Add(type, assembly);
                    }
                }
            }
            
            return !Log.HasLoggedErrors;
        }

        private void LoadSources()
        {
            foreach (var source in Sources)
            {
                var path = source.GetMetadata("FullPath");

                if (Directory.Exists(path))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        AddSourceFile(file);
                    }
                }
                else
                {
                    AddSourceFile(path);
                }
            }
        }

        private void AddSourceFile(string file)
        {
            var assemblyInfo = AssemblyInfo.GetAssemblyInfo(file);

            if (assemblyInfo != null)
            {
                AssemblyInfo existingInfo;
                if (assemblies.TryGetValue(assemblyInfo.Name, out existingInfo))
                {
                    var fileName = Path.GetFileName(assemblyInfo.Path);
                    var existingFileName = Path.GetFileName(existingInfo.Path);

                    if (fileName.Equals(existingFileName))
                    {
                        Log.LogError($"Duplicate entries for {assemblyInfo.Name} : {assemblyInfo.Path} & {existingInfo.Path}");
                    }
                    else
                    {
                        // tolerate mismatched filenames, eg: foo.dll and foo.ni.dll
                        Log.LogMessage($"Duplicate entries for {assemblyInfo.Name}, but different filenames : preferring {existingInfo.Path} over {assemblyInfo.Path}.");
                    }
                }
                else
                {
                    assemblies[assemblyInfo.Name] = assemblyInfo;
                }
            }
        }

        private void LoadIgnoredTypes()
        {
            if (IgnoredTypes == null || IgnoredTypes.Length == 0) return;

            foreach(var ignoredType in IgnoredTypes)
            {
                ignoredTypes.Add(ignoredType.ItemSpec);
            }
        }

        class AssemblyInfo
        {
            public AssemblyInfo(string path, string name, string[] types)
            {
                Path = path;
                Name = name;
                Types = types;
            }

            public string Path { get; }
            public string Name { get; }
            public string[] Types { get; }

            public static AssemblyInfo GetAssemblyInfo(string path)
            {
                try
                {
                    using (PEReader peReader = new PEReader(new FileStream(path, FileMode.Open, FileAccess.Read)))
                    {
                        if (peReader.HasMetadata)
                        {
                            MetadataReader contractReader = peReader.GetMetadataReader();
                            AssemblyDefinition assembly = contractReader.GetAssemblyDefinition();

                            var name = contractReader.GetString(assembly.Name);
                            var publicTypes = GetPublicTypes(contractReader);

                            return new AssemblyInfo(path, name, publicTypes);
                        }
                    }
                }
                catch (BadImageFormatException)
                {
                    //not a PE
                }

                return null;
            }

            private static string[] GetPublicTypes(MetadataReader reader)
            {
                var types = new List<string>();

                foreach (var typeDefHandle in reader.TypeDefinitions)
                {
                    bool isPublic;
                    var typeName = GetTypeFromDefinition(reader, typeDefHandle, out isPublic);
                    if (isPublic)
                    {
                        types.Add(typeName);
                    }
                }

                return types.ToArray();
            }

            private static string GetTypeFromDefinition(MetadataReader reader, TypeDefinitionHandle handle, out bool isPublic)
            {
                TypeDefinition definition = reader.GetTypeDefinition(handle);
                isPublic = IsPublic(definition.Attributes);

                string name = definition.Namespace.IsNil
                    ? reader.GetString(definition.Name)
                    : reader.GetString(definition.Namespace) + "." + reader.GetString(definition.Name);

                if (IsNested(definition.Attributes))
                {
                    TypeDefinitionHandle declaringTypeHandle = definition.GetDeclaringType();

                    bool parentIsPublic;
                    name = GetTypeFromDefinition(reader, declaringTypeHandle, out parentIsPublic) + "/" + name;
                    isPublic &= parentIsPublic;
                }

                return name;
            }

            private static bool IsPublic(TypeAttributes attr)
            {
                var typeVisibility = attr & TypeAttributes.VisibilityMask;

                switch (typeVisibility)
                {
                    case TypeAttributes.Public:
                    case TypeAttributes.NestedPublic:
                    case TypeAttributes.NestedFamily:
                    case TypeAttributes.NestedFamORAssem:
                        return true;
                    default:
                        return false;
                }
            }

            private const TypeAttributes NestedMask = (TypeAttributes)0x00000006;
            private static bool IsNested(TypeAttributes attr)
            {
                return (attr & NestedMask) != 0;
            }
        }
    }
}
