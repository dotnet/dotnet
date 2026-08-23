// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Microsoft.DotNet.UnifiedBuild.Tasks
{
    // Creates a symbols layout that matches the SDK layout
    [MSBuildMultiThreadableTask]
    public class CreateSdkSymbolsLayout : Task, IMultiThreadableTask
    {
        /// <summary>Injected by MSBuild so paths resolve against the project directory in multithreaded builds.</summary>
        public TaskEnvironment TaskEnvironment { get; set; } = TaskEnvironment.Fallback;

        /// <summary>
        /// Path to SDK layout.
        /// </summary>
        [Required]
        public string SdkLayoutPath { get; set; }

        /// <summary>
        /// Path to all source-built symbols, flat or with folder hierarchy.
        /// </summary>
        [Required]
        public string AllSymbolsPath { get; set; }

        /// <summary>
        /// Path to SDK symbols layout - will be created if it doesn't exist.
        /// </summary>
        [Required]
        public string SdkSymbolsLayoutPath { get; set; }

        /// <summary>
        /// If true, fails the build if any PDBs are missing.
        /// </summary>
        public bool FailOnMissingPDBs { get; set; }

        public override bool Execute()
        {
            IList<string> filesWithoutPDBs = GenerateSymbolsLayout(IndexAllSymbols());
            foreach (string file in filesWithoutPDBs)
            {
                string message = "Did not find a PDB for the following SDK file: " + file;
                if (FailOnMissingPDBs)
                {
                    Log.LogError(message);
                }
                else
                {
                    Log.LogMessage(MessageImportance.High, message);
                }
            }

            return !Log.HasLoggedErrors;
        }

        private IList<string> GenerateSymbolsLayout(Hashtable allPdbGuids)
        {
            List<string> filesWithoutPDBs = new List<string>();

            if (!string.IsNullOrEmpty(SdkSymbolsLayoutPath) && Directory.Exists(TaskEnvironment.GetAbsolutePath(SdkSymbolsLayoutPath)))
            {
                Directory.Delete(TaskEnvironment.GetAbsolutePath(SdkSymbolsLayoutPath), true);
            }

            // Resolve once and use for both enumeration and the relative-path math below, so that a
            // relative SdkLayoutPath cannot produce a mismatched prefix length.
            string absoluteSdkLayoutPath = TaskEnvironment.GetAbsolutePath(SdkLayoutPath);

            foreach (string file in Directory.GetFiles(absoluteSdkLayoutPath, "*", SearchOption.AllDirectories))
            {
                if (PdbUtilities.FileInSdkLayoutRequiresAPdb(TaskEnvironment.GetAbsolutePath(file), out string guid))
                {
                    string debugId = GetDebugId(guid, file);
                    if (!allPdbGuids.ContainsKey(debugId))
                    {
                        filesWithoutPDBs.Add(file.Substring(absoluteSdkLayoutPath.Length));
                    }
                    else
                    {
                        // Copy matching pdb to symbols path, preserving sdk binary's hierarchy
                        string sourcePath = (string)allPdbGuids[debugId]!;
                        string fileRelativePath = file.Substring(absoluteSdkLayoutPath.Length);
                        string destinationPath =
                            Path.Combine(SdkSymbolsLayoutPath, fileRelativePath)
                                .Replace(Path.GetFileName(file), Path.GetFileName(sourcePath));

                        Directory.CreateDirectory(TaskEnvironment.GetAbsolutePath(Path.GetDirectoryName(destinationPath)!));
                        File.Copy(TaskEnvironment.GetAbsolutePath(sourcePath), TaskEnvironment.GetAbsolutePath(destinationPath), true);
                    }
                }
            }

            return filesWithoutPDBs;
        }

        public Hashtable IndexAllSymbols()
        {
            Hashtable allPdbGuids = new Hashtable();

            foreach (string file in Directory.GetFiles(TaskEnvironment.GetAbsolutePath(AllSymbolsPath), "*.pdb", SearchOption.AllDirectories))
            {
                using var pdbFileStream = File.OpenRead(TaskEnvironment.GetAbsolutePath(file));
                var metadataProvider = MetadataReaderProvider.FromPortablePdbStream(pdbFileStream);
                var metadataReader = metadataProvider.GetMetadataReader();
                if (metadataReader.DebugMetadataHeader == null)
                {
                    continue;
                }

                var id = new BlobContentId(metadataReader.DebugMetadataHeader.Id);
                string guid = $"{id.Guid:N}";
                string debugId = GetDebugId(guid, file);
                if (!string.IsNullOrEmpty(guid) && !allPdbGuids.ContainsKey(debugId))
                {
                    allPdbGuids.Add(debugId, file);
                }
            }

            return allPdbGuids;
        }

        /// <summary>
        /// Calculates a debug Id from debug guid and filename. We use this as a key
        /// in PDB hashtable. Guid is not enough due to collisions in several PDBs.
        /// </summary>
        private string GetDebugId(string guid, string file) =>
            $"{guid}.{Path.GetFileNameWithoutExtension(file)}".ToLower();
    }
}
