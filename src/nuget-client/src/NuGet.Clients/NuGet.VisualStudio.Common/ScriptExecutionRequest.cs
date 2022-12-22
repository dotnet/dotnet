// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NuGet.Common;
using NuGet.Packaging.Core;
using EnvDTEProject = EnvDTE.Project;

namespace NuGet.PackageManagement.VisualStudio
{
    public class ScriptExecutionRequest
    {
        public ScriptExecutionRequest(string scriptPath, string installPath, PackageIdentity identity, EnvDTEProject project)
        {
            if (scriptPath == null)
            {
                throw new ArgumentNullException(nameof(scriptPath));
            }

            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            Identity = identity;
            ScriptPath = scriptPath;
            InstallPath = installPath;

            ToolsPath = Path.GetDirectoryName(ScriptPath);
            ScriptPackage = new ScriptPackage(Identity.Id, Identity.Version.ToString(), InstallPath);
            Project = project;
        }

        public PackageIdentity Identity { get; }
        public string InstallPath { get; }
        public string ToolsPath { get; }
        public string ScriptPath { get; }
        public ScriptPackage ScriptPackage { get; }
        public EnvDTEProject Project { get; }

        public string BuildCommand()
        {
            var escapedScriptPath = PathUtility.EscapePSPath(ScriptPath);
            var command = new StringBuilder(
                "$__pc_args=@(); " +
                "$input|%{$__pc_args+=$_}; " +
                "& " + escapedScriptPath + " $__pc_args[0] $__pc_args[1] $__pc_args[2]");
            command.Append(Project != null ? " $__pc_args[3]; " : "; ");
            command.Append("Remove-Variable __pc_args -Scope 0");

            return command.ToString();
        }

        public object[] BuildInput()
        {
            var input = new List<object>
            {
                InstallPath,
                ToolsPath,
                ScriptPackage
            };

            if (Project != null)
            {
                input.Add(Project);
            }

            return input.ToArray();
        }
    }
}
