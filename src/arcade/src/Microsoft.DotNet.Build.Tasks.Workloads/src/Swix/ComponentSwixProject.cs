// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;

namespace Microsoft.DotNet.Build.Tasks.Workloads.Swix
{
    /// <summary>
    /// Creates a SWIX project used to author a Visual Studio component package.
    /// </summary>
    internal class ComponentSwixProject : SwixProjectBase
    {
        private SwixComponent _component;

        protected override string ProjectFile
        {
            get;
        }

        /// <inheritdoc />
        protected override string ProjectSourceDirectory
        {
            get;
        }

        public ComponentSwixProject(SwixComponent component, string baseIntermediateOutputPath, string baseOutputPath, bool outOfSupport = false) :
            base(component.Name, component.Version, baseIntermediateOutputPath, baseOutputPath, outOfSupport)
        {
            _component = component;
            ValidateRelativePackagePath(GetRelativePackagePath());

            // Components must have 1 or more dependencies.
            if (!component.HasDependencies)
            {
                throw new ArgumentException(string.Format(Strings.ComponentMustHaveAtLeastOneDependency, component.Name));
            }

            ProjectSourceDirectory = Path.Combine(SwixDirectory, $"{component.SdkFeatureBand}",
                $"{Path.GetRandomFileName()}");

            ReplacementTokens[SwixTokens.__VS_COMPONENT_TITLE__] = component.Title;
            ReplacementTokens[SwixTokens.__VS_COMPONENT_DESCRIPTION__] = component.Description;
            ReplacementTokens[SwixTokens.__VS_COMPONENT_CATEGORY__] = component.Category;
            ReplacementTokens[SwixTokens.__VS_IS_UI_GROUP__] = component.IsUiGroup ? "yes" : "no";
            ReplacementTokens[SwixTokens.__VS_PACKAGE_OUT_OF_SUPPORT__] = OutOfSupport ? "yes" : "no";
            ReplacementTokens[SwixTokens.__VS_IS_ADVERTISED_PACKAGE__] = component.Advertise ? "yes" : "no";
        }

        /// <inheritdoc />
        public override string Create()
        {
            string swixProj = EmbeddedTemplates.Extract("component.swixproj", ProjectSourceDirectory, $"{Id}.{Version.ToString(2)}.swixproj");
            string componentSwr = EmbeddedTemplates.Extract("component.swr", ProjectSourceDirectory);

            ReplaceTokens(swixProj);
            ReplaceTokens(EmbeddedTemplates.Extract("component.res.swr", ProjectSourceDirectory));
            ReplaceTokens(componentSwr);

            // SWIX is indentation sensitive. The dependencies should be written as 
            //
            // vs.dependencies
            //   vs.dependency id=<package Id>
            //                 version=<version range>
            using StreamWriter swrWriter = File.AppendText(componentSwr);

            foreach (SwixDependency dependency in _component.Dependencies)
            {
                swrWriter.WriteLine($"  vs.dependency id={dependency.Id}");
                swrWriter.WriteLine($"                version={dependency.GetVersionRange()}");
                swrWriter.WriteLine($"                behaviors=IgnoreApplicabilityFailures");
            }

            return swixProj;
        }

        /// <inheritdoc />
        protected override string GetRelativePackagePath() =>
            Path.Combine(base.GetRelativePackagePath(), "_package.json");
    }
}
