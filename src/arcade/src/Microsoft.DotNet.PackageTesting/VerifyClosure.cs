// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.DotNet.Build.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Xml.Linq;

namespace Microsoft.DotNet.PackageTesting
{
    /// <summary>
    /// Verifies the closure of a set of DLLs, making sure all files are present and no cycles exist
    /// </summary>
    public class VerifyClosure : BuildTask
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
        public ITaskItem[] IgnoredReferences { get; set; }

        public bool CheckModuleReferences { get; set; }

        public string DependencyGraphFilePath { get; set; }

        private Dictionary<string, AssemblyInfo> assemblies = new Dictionary<string, AssemblyInfo>();
        private HashSet<string> otherFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, Version> ignoredReferences = new Dictionary<string, Version>(StringComparer.OrdinalIgnoreCase);
        
        public override bool Execute()
        {
            if (Sources == null || Sources.Length == 0)
            {
                Log.LogError("No sources to scan.");
                return false;
            }

            LoadSources();
            LoadIgnoredReferences();

            foreach(var assembly in assemblies.Values)
            {
                CheckDependencies(assembly);
            }

            foreach(var assembly in assemblies.Values)
            {
                DumpCycles(assembly);
            }

            if (!String.IsNullOrEmpty(DependencyGraphFilePath))
            {
                WriteDependencyGraph(DependencyGraphFilePath, assemblies.Values);
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

            if (assemblyInfo == null)
            {
                otherFiles.Add(Path.GetFileName(file));
            }
            else
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

        private void LoadIgnoredReferences()
        {
            if (IgnoredReferences == null || IgnoredReferences.Length == 0) return;
        
            foreach (var ignoredReference in IgnoredReferences)
            {
                var name = ignoredReference.ItemSpec;
                var versionString = ignoredReference.GetMetadata("Version");
                Version version = null;

                if (!string.IsNullOrEmpty(versionString))
                {
                    version = Version.Parse(versionString);
                }

                Version existingVersion;

                if (!ignoredReferences.TryGetValue(name, out existingVersion) ||
                    (existingVersion != null && (version == null || version > existingVersion)))
                {
                    ignoredReferences[name] = version;
                }
            }
        }

        private bool ShouldIgnore(string moduleReference)
        {
            return ignoredReferences.ContainsKey(moduleReference);
        }

        private bool ShouldIgnore(AssemblyReference reference)
        {
            Version toIgnore;
            return ignoredReferences.TryGetValue(reference.Name, out toIgnore) && (toIgnore == null || toIgnore >= reference.Version);
        }

        void CheckDependencies(AssemblyInfo assembly)
        {
            if (assembly.State == CheckState.Unchecked)
            {
                var depStack = new Stack<AssemblyInfo>();
                depStack.Push(assembly);

                CheckDependencies(depStack);
            }
        }

        void CheckDependencies(Stack<AssemblyInfo> depStack)
        {
            AssemblyInfo assm = depStack.Peek();

            // check module references
            if (assm.State == CheckState.Unchecked && CheckModuleReferences && assm.ModuleReferences != null)
            {
                foreach(var moduleReference in assm.ModuleReferences)
                {
                    if (ShouldIgnore(moduleReference))
                    {
                        continue;
                    }

                    if (!otherFiles.Contains(moduleReference))
                    {
                        Log.LogError($"Assembly '{assm.Name}' is missing module dependency '{moduleReference}'");
                    }
                }
            }
            
            // walk dependencies
            foreach (var dep in assm.References)
            {
                if (ShouldIgnore(dep))
                {
                    continue;
                }

                AssemblyInfo depAssm;

                assemblies.TryGetValue(dep.Name, out depAssm);

                if (assm.State == CheckState.Unchecked)
                {
                    // first time we've seen this assembly, validate that its dependencies are satisfied
                    if (depAssm == null)
                    {
                        Log.LogError($"Assembly '{assm.Name}' is missing dependency '{dep.Name}'");
                    }
                    else if (depAssm.Version < dep.Version)
                    {
                        Log.LogError($"Assembly '{assm.Name}' has insufficient version for dependency '{dep.Name}' : {depAssm.Version} < {dep.Version}.");
                    }
                }

                // missing dependency
                if (depAssm == null)
                {
                    continue;
                }

                if (depAssm.State == CheckState.HasCycle)
                {
                    // Don't bother finding multiple cycles for the same assembly.
                    // We'll do a second pass to dump all cycles.
                    continue;
                }

                if (depStack.Contains(depAssm))
                {
                    depAssm.State = CheckState.HasCycle;

                }
                else
                {
                    depStack.Push(depAssm);
                    CheckDependencies(depStack);
                }
            }

            if (assm.State == CheckState.Unchecked)
            {
                Log.LogMessage(LogImportance.Low, $"Checked {assm.Path}");
                assm.State = CheckState.Checked;
            }

            depStack.Pop();
        }

        void DumpCycles(AssemblyInfo assembly)
        {
            if (assembly.State == CheckState.HasCycle)
            {
                var depStack = new Stack<AssemblyInfo>();
                depStack.Push(assembly);

                var suspectCycles = new Dictionary<AssemblyInfo, AssemblyInfo[]>();
                
                DumpCycles(depStack, assembly, suspectCycles);
                
                StringBuilder cycleError = new StringBuilder();
                Log.LogError($"Cycle detected for {assembly.Path}.");

                foreach(var suspectCycle in suspectCycles)
                {
                    Log.LogError(PrintCycle(suspectCycle.Value));
                }
            }
        }

        void DumpCycles(Stack<AssemblyInfo> depStack, AssemblyInfo root, Dictionary<AssemblyInfo, AssemblyInfo[]> suspectCycles)
        {
            AssemblyInfo assm = depStack.Peek();

            foreach (var dep in assm.References)
            {
                if (ShouldIgnore(dep))
                {
                    continue;
                }

                AssemblyInfo depAssm;

                if (!assemblies.TryGetValue(dep.Name, out depAssm))
                {
                    continue;
                }

                if (depAssm != root && depStack.Contains(depAssm))
                {
                    // ignore other cycles but halt traversal
                    continue;
                }

                depStack.Push(depAssm);

                if (depAssm == root)
                {
                    // we found a cycle for the specified root
                    var cycle = depStack.Reverse().ToArray();
                    Log.LogMessage($"Cycle detected for {depAssm.Path} : {PrintCycle(cycle)}.");

                    var suspectAssembly = cycle.FirstOrDefault(a => a != root) ?? root;

                    AssemblyInfo[] existingCycle;
                    if (!suspectCycles.TryGetValue(suspectAssembly, out existingCycle) ||
                        existingCycle.Length > cycle.Length)
                    {
                        // keep the shortest cycle for a unique dependency from the root.
                        suspectCycles[suspectAssembly] = cycle;
                    }
                }
                else
                {
                    // not a cycle, continue traversal
                    DumpCycles(depStack, root, suspectCycles);
                }
                depStack.Pop();
            }
        }

        private string PrintCycle(AssemblyInfo[] cycleStack)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var assmebly in cycleStack)
            {
                if (builder.Length > 0)
                {
                    builder.Append(" > ");
                }
                builder.Append(assmebly.Name);
            }

            return builder.ToString();
        }

        private static XNamespace s_dgmlns = @"http://schemas.microsoft.com/vs/2009/dgml";
        private static void WriteDependencyGraph(string dependencyGraphFilePath, IEnumerable<AssemblyInfo> assemblies)
        {

            var doc = new XDocument(new XElement(s_dgmlns + "DirectedGraph"));
            var nodesElement = new XElement(s_dgmlns + "Nodes");
            var linksElement = new XElement(s_dgmlns + "Links");
            doc.Root.Add(nodesElement);
            doc.Root.Add(linksElement);

            var nodeIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach(var assembly in assemblies)
            {
                TryAddNode(nodeIds, nodesElement, assembly.Name);

                foreach (var reference in assembly.References)
                {
                    linksElement.Add(new XElement(s_dgmlns + "Link",
                        new XAttribute("Source", assembly.Name),
                        new XAttribute("Target", reference.Name)));

                    TryAddNode(nodeIds, nodesElement, reference.Name);
                }

                foreach (var moduleReference in assembly.ModuleReferences)
                {
                    linksElement.Add(new XElement(s_dgmlns + "Link",
                        new XAttribute("Source", assembly.Name),
                        new XAttribute("Target", moduleReference)));

                    TryAddNode(nodeIds, nodesElement, moduleReference, isNative: true);
                }
            }

            var categoriesElement = new XElement(s_dgmlns + "Categories");
            doc.Root.Add(categoriesElement);

            categoriesElement.Add(new XElement(s_dgmlns + "Category",
                new XAttribute("Id", "native"),
                new XAttribute("Background", "Blue")
                ));

            categoriesElement.Add(new XElement(s_dgmlns + "Category",
                new XAttribute("Id", "managed"),
                new XAttribute("Background", "Green")
                ));

            using (var file = File.Create(dependencyGraphFilePath))
            {
                doc.Save(file);
            }
        }

        private static bool TryAddNode(ICollection<string> existing, XElement parent, string id, bool isNative = false)
        {
            if (!existing.Contains(id))
            {
                parent.Add(new XElement(s_dgmlns + "Node",
                        new XAttribute("Id", id),
                        new XAttribute("Category", isNative ? "native": "managed")));
                return true;
            }
            return false;
        }

        class AssemblyInfo
        {
            public AssemblyInfo(string path, string name, Version version, AssemblyReference[] references, string[] moduleReferences)
            {
                Path = path;
                Name = name;
                Version = version;
                References = references;
                ModuleReferences = moduleReferences;
                State = CheckState.Unchecked;
            }

            public string Path { get; }
            public string Name { get; }
            public Version Version { get; }
            public AssemblyReference[] References { get; }
            public string[] ModuleReferences { get; }
            public CheckState State { get; set; }

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
                            var version = assembly.Version;
                            var references = GetAssemblyReferences(contractReader);
                            var moduleReferences = GetModuleReferences(contractReader);

                            return new AssemblyInfo(path, name, version, references, moduleReferences);
                        }
                    }
                }
                catch(BadImageFormatException)
                {
                    //not a PE
                }

                return null;
            }

            private static AssemblyReference[] GetAssemblyReferences(MetadataReader reader)
            {
                var count = reader.GetTableRowCount(TableIndex.AssemblyRef);
                var references = new AssemblyReference[count];

                for (int i = 0; i < count; i++)
                {
                    var reference = reader.GetAssemblyReference(MetadataTokens.AssemblyReferenceHandle(i + 1));
                    references[i] = new AssemblyReference(reader.GetString(reference.Name), reference.Version);
                }

                return references.ToArray();
            }

            private static string[] GetModuleReferences(MetadataReader reader)
            {
                var count = reader.GetTableRowCount(TableIndex.ModuleRef);
                var references = new string[count];

                for (int i = 0; i < count; i++)
                {
                    var moduleRef = reader.GetModuleReference(MetadataTokens.ModuleReferenceHandle(i + 1));
                    var moduleName = reader.GetString(moduleRef.Name);

                    references[i] = moduleName;
                }

                return references;
            }
        }

        class AssemblyReference
        {
            public AssemblyReference(string name, Version version)
            {
                Name = name;
                Version = version;
            }

            public string Name { get; }
            public Version Version { get; }
        }
        
        enum CheckState
        {
            Unchecked,
            HasCycle,
            Checked
        }
    }
}
