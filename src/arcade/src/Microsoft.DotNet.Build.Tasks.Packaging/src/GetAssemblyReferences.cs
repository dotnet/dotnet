// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace Microsoft.DotNet.Build.Tasks.Packaging
{
    public class GetAssemblyReferences : BuildTask
    {
        [Required]
        public ITaskItem[] Assemblies
        {
            get;
            set;
        }

        [Output]
        public ITaskItem[] ReferencedAssemblies
        {
            get;
            set;
        }

        [Output]
        public ITaskItem[] ReferencedNativeLibraries
        {
            get;
            set;
        }

        public override bool Execute()
        {
            if (Assemblies == null || Assemblies.Length == 0)
                return true;

            List<ITaskItem> references = new List<ITaskItem>();
            List<ITaskItem> nativeLibs = new List<ITaskItem>();

            foreach (var assemblyItem in Assemblies)
            {
                try
                {
                    if (!File.Exists(assemblyItem.ItemSpec))
                    {
                        Log.LogError($"File {assemblyItem.ItemSpec} does not exist, ensure you have built libraries before building the package.");
                        continue;
                    }

                    using (PEReader peReader = new PEReader(new FileStream(assemblyItem.ItemSpec, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read)))
                    {
                        MetadataReader reader = peReader.GetMetadataReader();
                        foreach (var handle in reader.AssemblyReferences)
                        {
                            AssemblyReference reference = reader.GetAssemblyReference(handle);
                            TaskItem referenceItem = new TaskItem(reader.GetString(reference.Name));
                            assemblyItem.CopyMetadataTo(referenceItem);
                            referenceItem.SetMetadata("Version", reference.Version.ToString());
                            referenceItem.SetMetadata("AssemblyVersion", reference.Version.ToString());
                            references.Add(referenceItem);
                        }

                        for (int i = 1, count = reader.GetTableRowCount(TableIndex.ModuleRef); i <= count; i++)
                        {
                            var moduleRef = reader.GetModuleReference(MetadataTokens.ModuleReferenceHandle(i));
                            var moduleName = reader.GetString(moduleRef.Name);

                            TaskItem nativeLib = new TaskItem(moduleName);
                            assemblyItem.CopyMetadataTo(nativeLib);
                            nativeLibs.Add(nativeLib);
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    // Ignore invalid assemblies
                }
            }

            ReferencedAssemblies = references.ToArray();
            ReferencedNativeLibraries = nativeLibs.ToArray();

            return true;
        }
    }
}
