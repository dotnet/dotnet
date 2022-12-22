// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NuGet.Frameworks;
using NuGet.ProjectModel;

namespace NuGet.CommandLine
{
    public class DiagnosticCommands
    {
        private readonly Common.ILogger _log;

        private const string Native = "\x1b[31mnative\x1b[39m";
        private const string Runtime = "\x1b[35mruntime\x1b[39m";
        private const string Compile = "\x1b[32mcompile\x1b[39m";
        private const string Framework = "\x1b[34mframework\x1b[39m";
        private const string FrameworkReference = "\x1b[34mframeworkref\x1b[39m";
        private const string Nothing = "\x1b[33mnothing\x1b[39m";

        public DiagnosticCommands(Common.ILogger log)
        {
            _log = log;
        }

        public int Lockfile(string projectOrLockfile, string target, string library)
        {
            // Locate the lock file
            if (!string.Equals(Path.GetFileName(projectOrLockfile), LockFileFormat.LockFileName, StringComparison.Ordinal))
            {
                projectOrLockfile = Path.Combine(projectOrLockfile, LockFileFormat.LockFileName);
            }
            var lockfile = new LockFileFormat().Read(projectOrLockfile);
            _log.LogMinimal($"Viewing data from Lock File: {projectOrLockfile}");

            // Attempt to locate the project
            var projectPath = Path.Combine(Path.GetDirectoryName(projectOrLockfile), PackageSpec.PackageSpecFileName);
            PackageSpec project = null;
            if (File.Exists(projectPath))
            {
                project = JsonPackageSpecReader.GetPackageSpec(File.ReadAllText(projectPath), Path.GetFileName(Path.GetDirectoryName(projectPath)), projectPath);
            }

            if (string.IsNullOrEmpty(library))
            {
                return SummarizeLockfile(project, lockfile, target);
            }
            else
            {
                return LibraryDetail(lockfile, target, library);
            }
        }

        private int LibraryDetail(LockFile lockfile, string targetName, string library)
        {
            var lib = lockfile.Libraries.FirstOrDefault(l => l.Name.Equals(library, StringComparison.OrdinalIgnoreCase));
            if (lib == null)
            {
                _log.LogError($"Library not found: {library}");
                return -1;
            }

            _log.LogMinimal($"{lib.Name} {lib.Version}");
            _log.LogMinimal($"Servicable: {lib.IsServiceable}");
            _log.LogMinimal($"SHA512 Hash: {lib.Sha512}");
            _log.LogMinimal($"Files:");
            foreach (var file in lib.Files)
            {
                _log.LogMinimal($" * {file}");
            }

            IEnumerable<LockFileTarget> targets = lockfile.Targets;
            if (!string.IsNullOrEmpty(targetName))
            {
                var parts = targetName.Split('/');
                var tfm = NuGetFramework.Parse(parts[0]);
                var rid = parts[1];
                targets = targets.Where(t => string.Equals(rid, t.RuntimeIdentifier, StringComparison.Ordinal) && tfm.Equals(t.TargetFramework));
            }
            var libraryTargets = targets.Select(t => new { Target = t, Library = t.Libraries.FirstOrDefault(l => string.Equals(l.Name, library, StringComparison.OrdinalIgnoreCase)) });
            foreach (var libraryTarget in libraryTargets)
            {
                _log.LogMinimal($"Target: {libraryTarget.Target.TargetFramework}/{libraryTarget.Target.RuntimeIdentifier}");
                if (libraryTarget.Library == null)
                {
                    _log.LogMinimal(" Not supported");
                }
                else
                {
                    WriteList(Compile, libraryTarget.Library.CompileTimeAssemblies.Select(f => f.Path));
                    WriteList(Runtime, libraryTarget.Library.RuntimeAssemblies.Select(f => f.Path));
                    WriteList(Native, libraryTarget.Library.NativeLibraries.Select(f => f.Path));
                    WriteList(Framework, libraryTarget.Library.FrameworkAssemblies);
                    WriteList(FrameworkReference, libraryTarget.Library.FrameworkReferences);
                }
            }

            return 0;
        }

        private void WriteList(string header, IEnumerable<string> items)
        {
            _log.LogMinimal($" {header}:");
            foreach (var item in items)
            {
                _log.LogMinimal($"  * {item}");
            }
        }

        private int SummarizeLockfile(PackageSpec project, LockFile lockfile, string targetName)
        {
            if (project == null)
            {
                _log.LogMinimal($"Up-to-date: Unknown");
            }
            else
            {
                _log.LogMinimal($"Up-to-date: {lockfile.IsValidForPackageSpec(project)}");
            }

            _log.LogMinimal("Project Dependencies:");
            foreach (var group in lockfile.ProjectFileDependencyGroups)
            {
                var fxName = string.IsNullOrEmpty(group.FrameworkName) ? "All Frameworks" : group.FrameworkName;
                if (group.Dependencies.Any())
                {
                    _log.LogMinimal($" {fxName}");
                    foreach (var dep in group.Dependencies)
                    {
                        _log.LogMinimal($"  * {dep}");
                    }
                }
                else
                {
                    _log.LogMinimal($" {fxName}: none");
                }
            }

            _log.LogMinimal("All Libraries:");
            foreach (var lib in lockfile.Libraries)
            {
                _log.LogMinimal($"* {lib.Name} {lib.Version}");
            }

            IEnumerable<LockFileTarget> targets = lockfile.Targets;
            if (!string.IsNullOrEmpty(targetName))
            {
                var parts = targetName.Split('/');
                var tfm = NuGetFramework.Parse(parts[0]);
                var rid = parts[1];
                targets = targets.Where(t => string.Equals(rid, t.RuntimeIdentifier, StringComparison.Ordinal) && tfm.Equals(t.TargetFramework));
            }

            foreach (var target in targets)
            {
                _log.LogMinimal($"Target: {target.TargetFramework} {target.RuntimeIdentifier}");
                foreach (var lib in target.Libraries)
                {
                    var provides = string.Empty;
                    if (lib.NativeLibraries.Any())
                    {
                        provides += Native + ",";
                    }
                    if (lib.RuntimeAssemblies.Any())
                    {
                        provides += Runtime + ",";
                    }
                    if (lib.CompileTimeAssemblies.Any())
                    {
                        provides += Compile + ",";
                    }
                    if (lib.FrameworkAssemblies.Any())
                    {
                        provides += Framework + ",";
                    }
                    provides = provides.TrimEnd(',');
                    if (string.IsNullOrEmpty(provides))
                    {
                        provides = Nothing;
                    }
                    _log.LogMinimal($" * [{provides}] {lib.Name} {lib.Version}");
                }
            }
            return 0;
        }
    }
}
