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
    internal class MsiPayloadPackageProject : ProjectTemplateBase
    {
        /// <inheritdoc />
        protected override string ProjectSourceDirectory
        {
            get;
        }

        /// <inheritdoc />
        protected override string ProjectFile
        {
            get;
        }

        //  Key: path to file, value: path in package
        public Dictionary<string, string> PackageContents { get; set; } = new();

        public MsiPayloadPackageProject(MsiMetadata package, ITaskItem msi, string baseIntermediateOutputPath, string baseOutputPath, Dictionary<string, string> packageContents) :
            base(baseIntermediateOutputPath, baseOutputPath)
        {
            string platform = msi.GetMetadata(Metadata.Platform);
            ProjectSourceDirectory = Path.Combine(SourceDirectory, "msiPackage", platform, package.Id);
            ProjectFile = "msi.csproj";

            PackageContents = packageContents;

            ReplacementTokens[PayloadPackageTokens.__AUTHORS__] = package.Authors;
            ReplacementTokens[PayloadPackageTokens.__COPYRIGHT__] = package.Copyright;
            ReplacementTokens[PayloadPackageTokens.__DESCRIPTION__] = package.Description;
            ReplacementTokens[PayloadPackageTokens.__PACKAGE_ID__] = $"{package.Id}.Msi.{platform}";
            ReplacementTokens[PayloadPackageTokens.__PACKAGE_PROJECT_URL__] = package.ProjectUrl;
            ReplacementTokens[PayloadPackageTokens.__PACKAGE_VERSION__] = $"{package.PackageVersion}";
        }

        /// <inheritdoc />
        public override string Create()
        {
            string msiCsproj = EmbeddedTemplates.Extract("msi.csproj", ProjectSourceDirectory);

            Utils.StringReplace(msiCsproj, ReplacementTokens, Encoding.UTF8);

            var proj = XDocument.Load(msiCsproj);
            var itemGroup = proj.Root.Element("ItemGroup");
            foreach (var packageFile in PackageContents)
            {
                itemGroup.Add(new XElement("None",
                    new XAttribute("Include", packageFile.Key),
                    new XAttribute("Pack", "true"),
                    new XAttribute("PackagePath", packageFile.Value)));
            }
            proj.Save(msiCsproj);

            EmbeddedTemplates.Extract("Icon.png", ProjectSourceDirectory);
            EmbeddedTemplates.Extract("LICENSE.TXT", ProjectSourceDirectory);

            return msiCsproj;
        }
    }
}
