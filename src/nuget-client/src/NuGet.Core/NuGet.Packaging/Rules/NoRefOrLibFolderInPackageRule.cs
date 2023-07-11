using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NuGet.Client;
using NuGet.Common;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.Packaging.Core;
using NuGet.RuntimeModel;

namespace NuGet.Packaging.Rules
{
    internal class NoRefOrLibFolderInPackageRule : IPackageRule
    {
        public string MessageFormat { get; }

        public NoRefOrLibFolderInPackageRule(string messageFormat)
        {
            MessageFormat = messageFormat;
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

            List<ContentItemGroup> libItems = new();
            List<ContentItemGroup> refItems = new();
            List<ContentItemGroup> buildItems = new();
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.CompileLibAssemblies, libItems);
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.CompileRefAssemblies, refItems);
            ContentExtractor.GetContentForPattern(collection, managedCodeConventions.Patterns.MSBuildFiles, buildItems);

            var libFrameworks = ContentExtractor.GetGroupFrameworks(libItems).ToArray();
            var refFrameworks = ContentExtractor.GetGroupFrameworks(refItems).ToArray();
            var buildFrameworks = ContentExtractor.GetGroupFrameworks(buildItems).ToArray();

            if (libFrameworks.Length == 0 && refFrameworks.Length == 0)
            {
                //if you can't find the ref and lib folder, then find the build folder
                if (buildFrameworks.Length != 0)
                {
                    //if you can find any folders other than native or any, raise an NU5127
                    if (buildFrameworks.Any(t => (FrameworkConstants.DotNetAll.Satisfies(t) || FrameworkNameValidatorUtility.IsValidFrameworkName(t))
                                            && t.GetShortFolderName() != FrameworkConstants.FrameworkIdentifiers.DotNet
                                            && t.GetShortFolderName() != FrameworkConstants.FrameworkIdentifiers.Native))
                    {
                        var possibleFrameworks = buildFrameworks.
                            Where(t => t.IsSpecificFramework
                                    && t.GetShortFolderName() != FrameworkConstants.FrameworkIdentifiers.DotNet
                                    && t.GetShortFolderName() != FrameworkConstants.FrameworkIdentifiers.Native).
                            Select(t => t.GetShortFolderName()).ToArray();

                        (var tfmNames, var suggestedDirectories) = GenerateWarningString(possibleFrameworks);

                        var issue = new List<PackagingLogMessage>();
                        issue.Add(PackagingLogMessage.CreateWarning(string.Format(CultureInfo.CurrentCulture, MessageFormat, tfmNames, suggestedDirectories),
                            NuGetLogCode.NU5127));
                        return issue;
                    }
                }
            }

            return Array.Empty<PackagingLogMessage>();
        }

        private (string, string) GenerateWarningString(string[] possibleFrameworks)
        {
            string tfmNames = possibleFrameworks.Length > 1
                ? string.Join(", ", possibleFrameworks)
                : possibleFrameworks[0];

            string suggestedDirectories = possibleFrameworks.Length > 1
                ? CreateDirectoriesMessage(possibleFrameworks)
                : string.Format(CultureInfo.CurrentCulture, "-lib/{0}/_._", possibleFrameworks[0]);

            return (tfmNames, suggestedDirectories);
        }

        private static string CreateDirectoriesMessage(string[] possibleFrameworks)
        {
            var suggestedDirectories = new StringBuilder();
            foreach (var framework in possibleFrameworks)
            {
                suggestedDirectories.AppendFormat(CultureInfo.CurrentCulture, "-lib/{0}/_._", framework).AppendLine();
            }
            return suggestedDirectories.ToString();
        }

    }
}
