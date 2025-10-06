// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Client;
using NuGet.Common;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.RuntimeModel;

#nullable enable

namespace NuGet.Packaging.Rules
{
    internal class AccidentallyCompatibleWithAllFrameworksRule : IPackageRule
    {
        public string MessageFormat { get; }

        public AccidentallyCompatibleWithAllFrameworksRule()
        {
            MessageFormat = AnalysisResources.NoRefOrLibFolderInPackage;
        }

        public IEnumerable<PackagingLogMessage> Validate(PackageArchiveReader package)
        {
            var files = package.GetFiles();
            return Validate(files);
        }

        internal IEnumerable<PackagingLogMessage> Validate(IEnumerable<string> files)
        {
            var managedCodeConventions = new ManagedCodeConventions(RuntimeGraph.Empty);
            var collection = new ContentItemCollection();
            collection.Load(files);

            List<ContentItemGroup> items = new();
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.CompileLibAssemblies, items);
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.CompileRefAssemblies, items);
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.MSBuildMultiTargetingFiles, items);

            // Packages with the above files should not warn
            if (items.Count != 0)
            {
                return [];
            }

            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.MSBuildFiles, items);
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.MSBuildTransitiveFiles, items);

            var buildFrameworks = ContentExtractor.GetGroupFrameworks(items).ToArray();
            if (buildFrameworks.Length == 0)
            {
                return [];
            }

            List<string>? suggestedFrameworks = null;
            foreach (var targetFramework in buildFrameworks)
            {
                if (!FrameworkNameValidatorUtility.IsValidFrameworkName(targetFramework))
                {
                    // files like /build/bin/some.dll will cause a framework with name bin to created.
                    continue;
                }

                // /build/any/* maps to NetPlatform for some reason
                if (targetFramework.Framework == FrameworkConstants.FrameworkIdentifiers.NetPlatform
                    || targetFramework.Framework == FrameworkConstants.CommonFrameworks.Native.Framework)
                {
                    // If the package is already compatible with all frameworks, then the warning is not needed.
                    // Packages for C/C++ projects don't use lib/ or ref/ folders. While these packages will prevent NuGet
                    // from warning about compatibility when referenced by .NET projects, there's lower likelihood of .NET
                    // projects attempting to reference the package, and adding the warning will be impactful to native
                    // package authors, so prevent the warning.
                    return [];
                }

                if (suggestedFrameworks == null)
                {
                    suggestedFrameworks = new List<string>(buildFrameworks.Length);
                }
                suggestedFrameworks.Add(targetFramework.GetShortFolderName());
            }

            if (suggestedFrameworks?.Count > 0)
            {
                (var tfmNames, var suggestedDirectories) = GenerateWarningString(suggestedFrameworks);

                var issue = new List<PackagingLogMessage>();
                issue.Add(PackagingLogMessage.CreateWarning(string.Format(CultureInfo.CurrentCulture, MessageFormat, tfmNames, suggestedDirectories),
                    NuGetLogCode.NU5127));
                return issue;
            }

            return Array.Empty<PackagingLogMessage>();
        }

        private static (string, string) GenerateWarningString(List<string> possibleFrameworks)
        {
            possibleFrameworks.Sort();

            string tfmNames = possibleFrameworks.Count > 1
                ? string.Join(", ", possibleFrameworks)
                : possibleFrameworks[0];

            string suggestedDirectories = possibleFrameworks.Count > 1
                ? CreateDirectoriesMessage(possibleFrameworks)
                : string.Format(CultureInfo.CurrentCulture, "-lib/{0}/_._", possibleFrameworks[0]);

            return (tfmNames, suggestedDirectories);
        }

        private static string CreateDirectoriesMessage(List<string> possibleFrameworks)
        {
            int estimatedLength = possibleFrameworks.Count * "-lib//_.)".Length + possibleFrameworks.Sum(f => f.Length);
            var suggestedDirectories = StringBuilderPool.Shared.Rent(estimatedLength);
            foreach (var framework in possibleFrameworks)
            {
                suggestedDirectories.AppendFormat(CultureInfo.CurrentCulture, "-lib/{0}/_._", framework).AppendLine();
            }
            return StringBuilderPool.Shared.ToStringAndReturn(suggestedDirectories);
        }

    }
}
