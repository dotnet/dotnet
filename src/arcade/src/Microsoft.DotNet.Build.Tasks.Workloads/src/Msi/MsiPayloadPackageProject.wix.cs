// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.Build.Framework;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Msi
{
    /// <summary>
    /// Describes a project to package an MSI and its JSON manifest into a NuGet package.
    /// </summary>
    internal class MsiPayloadPackageProject : WorkloadTemplateBase
    {
        private string ProjectFile
        {
            get;
        }

        //  Key: path to file, value: path in package
        public Dictionary<string, string> PackageContents { get; set; } = new();

        public MsiPayloadPackageProject(IWorkloadPackageMetadata packageMetadata, ITaskItem msi, string baseIntermediateOutputPath, string baseOutputPath, Dictionary<string, string> packageContents) :
            base(baseIntermediateOutputPath)
        {
            string platform = msi.GetMetadata(Metadata.Platform);
            SourcePath = Path.Combine(SourcePath, "msiPackage", platform, packageMetadata.Id);
            ProjectFile = "msi.csproj";

            PackageContents = packageContents;

            ReplacementTokens[PayloadPackageTokens.__AUTHORS__] = packageMetadata.Authors;
            ReplacementTokens[PayloadPackageTokens.__COPYRIGHT__] = packageMetadata.Copyright;
            ReplacementTokens[PayloadPackageTokens.__DESCRIPTION__] = packageMetadata.Description;
            ReplacementTokens[PayloadPackageTokens.__PACKAGE_ID__] = $"{packageMetadata.Id}.Msi.{platform}";
            ReplacementTokens[PayloadPackageTokens.__PACKAGE_PROJECT_URL__] = packageMetadata.ProjectUrl;
            ReplacementTokens[PayloadPackageTokens.__PACKAGE_VERSION__] = $"{packageMetadata.PackageVersion}";
        }

        /// <inheritdoc />
        public override string Create()
        {
            AddFile("Icon.png", replaceTokens: false);
            AddFile("LICENSE.TXT", replaceTokens: false);
            string msiCsproj = AddFile("msi.csproj");

            var projectDocument = XDocument.Load(msiCsproj);
            var itemGroup = projectDocument.Root.Element("ItemGroup");
            foreach (var packageFile in PackageContents)
            {
                itemGroup.Add(new XElement("None",
                    new XAttribute("Include", packageFile.Key),
                    new XAttribute("Pack", "true"),
                    new XAttribute("PackagePath", packageFile.Value)));
            }
            projectDocument.Save(msiCsproj);

            return msiCsproj;
        }
    }
}
