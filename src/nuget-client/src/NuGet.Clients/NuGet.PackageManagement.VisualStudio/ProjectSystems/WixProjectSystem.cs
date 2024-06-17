// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    public class WixProjectSystem : VsMSBuildProjectSystem
    {
        public WixProjectSystem(IVsProjectAdapter vsProjectAdapter, INuGetProjectContext nuGetProjectContext)
            : base(vsProjectAdapter, nuGetProjectContext)
        {
        }

        private const string RootNamespace = "RootNamespace";
        private const string OutputName = "OutputName";
        private const string DefaultNamespace = "WiX";

        public override Task AddReferenceAsync(string referencePath)
        {
            // References aren't allowed for WiX projects
            return Task.CompletedTask;
        }

        public override void AddGacReference(string name)
        {
            // GAC references aren't allowed for WiX projects
        }

        protected override bool ExcludeFile(string path)
        {
            // Exclude nothing from WiX projects
            return false;
        }

        [Obsolete("New properties should use IVsProjectBuildProperties.GetPropertyValue instead. Ideally we should migrate existing properties to stop using DTE as well.")]
        public override dynamic GetPropertyValue(string propertyName)
        {
            if (propertyName.Equals(RootNamespace, StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    return base.GetPropertyValue(OutputName);
                }
                catch
                {
                    return DefaultNamespace;
                }
            }
            return base.GetPropertyValue(propertyName);
        }

        public override Task RemoveReferenceAsync(string name)
        {
            // References aren't allowed for WiX projects
            return Task.CompletedTask;
        }

        public override Task<bool> ReferenceExistsAsync(string name)
        {
            // References aren't allowed for WiX projects
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
            return TaskResult.True;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
        }

        public override bool IsSupportedFile(string path)
        {
            return true;
        }
    }
}
