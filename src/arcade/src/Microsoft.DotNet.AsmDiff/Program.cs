// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace Microsoft.DotNet.AsmDiff
{
    public class Program
    {
        [Option("-os|--OldSet", "Provide path to an assembly or directory for an assembly set to gather the old set of types. These types will be the baseline for the compare.", CommandOptionType.SingleValue)]
        public string OldSet { get; set; }
        [Option("-ns|--NewSet", "Provide path to an assembly or directory for an assembly set to gather the new set of types. If this parameter is not provided the API's for the oldset will be printed instead of the diff.", CommandOptionType.SingleValue)]
        public string NewSet { get; set; }

        [Option("-nsn|--NewSetName", "Provide a name for the new set in output. If this parameter is not provided the file or directory name will be used.", CommandOptionType.SingleValue)]
        public string NewSetName { get; set; }
        [Option("-osn|--OldSetName", "Provide a name for the old set in output. If this parameter is not provided the file or directory name will be used.", CommandOptionType.SingleValue)]
        public string OldSetName { get; set; }

        [Option("-u|--Unchanged", "Include members, types, and namespaces that are unchanged.", CommandOptionType.NoValue)]
        public bool Unchanged { get; set; }
        [Option("-r|--Removed", "Include members, types, and namespaces that were removed. (default is removed and added)", CommandOptionType.NoValue)]
        public bool Removed { get; set; }
        [Option("-a|--Added", "Include members, types, and namespaces that were removed. (default is removed and added)", CommandOptionType.NoValue)]
        public bool Added { get; set; }
        [Option("-c|--Changed", "Include members, types, and namespaces that were removed. (default is removed and added)", CommandOptionType.NoValue)]
        public bool Changed { get; set; }

        [Option("-to|--TypesOnly", "Only show down to the type level not the member level.", CommandOptionType.NoValue)]
        public bool TypesOnly { get; set; }
        [Option("-sr|--StrikeRemoved", "For removed API's also strike them out. This option currently only works with the HTML writer which is the default.", CommandOptionType.NoValue)]
        public bool StrikeRemoved { get; set; }
        [Option("-da|--DiffAttributes", "Enables diffing of the attributes as well, by default all attributes are ignored. For CSV writer causes the assembly name to be included in the column for types.", CommandOptionType.NoValue)]
        public bool DiffAttributes { get; set; }
        [Option("-dai|--DiffAssemblyInfo", "Enables diffing of the assembly level information like version, key, etc.", CommandOptionType.NoValue)]
        public bool DiffAssemblyInfo { get; set; }
        [Option("-adm|--AlwaysDiffMembers", "By default if an entire class is added or removed we don't show the members, setting this option forces all the members to be shown instead.", CommandOptionType.NoValue)]
        public bool AlwaysDiffMembers { get; set; }
        [Option("-hbm|--HighlightBaseMembers", "Highlight members that are interface implementations or overrides of a base member.", CommandOptionType.NoValue)]
        public bool HighlightBaseMembers { get; set; }

        [Option("-ft|--FlattenTypes", "Will flatten types so that all members available on the type show on the type not just the implemented ones.", CommandOptionType.NoValue)]
        public bool FlattenTypes { get; set; }
        [Option("-gba|--GroupByAssembly", "Group the differences by assembly instead of flattening the namespaces.", CommandOptionType.NoValue)]
        public bool GroupByAssembly { get; set; }
        [Option("-eat|--ExcludeAddedTypes", "Do not show types that have been added to the new set of types.", CommandOptionType.NoValue)]
        public bool ExcludeAddedTypes { get; set; }
        [Option("-ert|--ExcludeRemovedTypes", "Do not show types that have been removed from the new set of types.", CommandOptionType.NoValue)]
        public bool ExcludeRemovedTypes { get; set; }
        [Option("-iia|--IncludeInternalApis", "Include internal types and members as part of the diff.", CommandOptionType.NoValue)]
        public bool IncludeInternalApis { get; set; }
        [Option("-ipa|--IncludePrivateApis", "Include private types and members as part of the diff.", CommandOptionType.NoValue)]
        public bool IncludePrivateApis { get; set; }        
        
        [Option("-itc|--IncludeTableOfContents", "Include table of contents as part of the diff.", CommandOptionType.NoValue)]
        public bool IncludeTableOfContents { get; set; }
        [Option("-cfn|--CreateFilePerNamespace", "Create files per namespace.", CommandOptionType.NoValue)]
        public bool CreateFilePerNamespace { get; set; }

        [Option("-w|--DiffWriter", "Type of difference writer to use, either CSharp code diffs or flat list of compat violations (default).", CommandOptionType.SingleValue)]
        public DiffWriterType DiffWriter { get; set; }
        [Option("-s|--SyntaxWriter", "Specific the syntax writer type. Only used if the writer is CSDecl", CommandOptionType.SingleValue)]
        public SyntaxWriterType SyntaxWriter { get; set; }

        [Option("-o|--OutFile", "Output file path. Default is the console.", CommandOptionType.SingleValue)]
        public string OutFile { get; set; }

        [Option("-l|--Language", "Provide a languagetag for localized content. If this parameter is not provided the environments default language will be used. Currently language specific content is only available in Markdown Writer.", CommandOptionType.SingleValue)]
        public string Language { get; set; }

        public void OnExecute()
        {         
            if (string.IsNullOrEmpty(NewSet))
            {
                // Reset the filter to be unchanged if we only have a single set so that it will
                // simply output the contents of the set.
                Removed = Added = false;
                Unchanged = true;
            }

            if (!Added && !Removed && !Changed && !Unchanged)
            {
                // If the user didn't explicitly specify what to include we default to changes only.
                Added = Removed = Changed = true;
            }

            if (!string.IsNullOrEmpty(Language))
            {
                var cultureInfo = System.Globalization.CultureInfo.GetCultureInfo(Language);
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
            }

            DiffConfigurationOptions options = GetDiffOptions();
            DiffFormat diffFormat = GetDiffFormat();

            AssemblySet oldAssemblies = AssemblySet.FromPaths(OldSetName, OldSet);
            AssemblySet newAssemblies = AssemblySet.FromPaths(NewSetName, NewSet);

            DiffConfiguration diffConfiguration = new DiffConfiguration(oldAssemblies, newAssemblies, options);

            if (diffFormat == DiffFormat.Md)
            {
                DiffDocument diffDocument = DiffEngine.BuildDiffDocument(diffConfiguration);
                var markdownDiffExporter = new MarkdownDiffExporter(diffDocument, OutFile, IncludeTableOfContents, CreateFilePerNamespace);
                markdownDiffExporter.Export();
            }
            else
            {
                using (TextWriter output = GetOutput())
                    DiffEngine.Export(diffConfiguration, null, diffFormat, output);
            }
        }

        public static int Main(string[] args) => CommandLineApplication.Execute<Program>(args);

        private  DiffConfigurationOptions GetDiffOptions()
        {
            var result = DiffConfigurationOptions.IncludeAddedTypes | DiffConfigurationOptions.IncludeRemovedTypes;

            if (Added)
                result |= DiffConfigurationOptions.IncludeAdded;

            if (Removed)
                result |= DiffConfigurationOptions.IncludeRemoved;

            if (Changed)
                result |= DiffConfigurationOptions.IncludeChanged;

            if (Unchanged)
                result |= DiffConfigurationOptions.IncludeUnchanged;

            if (AlwaysDiffMembers)
                result |= DiffConfigurationOptions.AlwaysDiffMembers;

            if (TypesOnly)
                result |= DiffConfigurationOptions.TypesOnly;

            if (FlattenTypes)
                result |= DiffConfigurationOptions.FlattenTypes;

            if (GroupByAssembly)
                result |= DiffConfigurationOptions.GroupByAssembly;

            if (HighlightBaseMembers)
                result |= DiffConfigurationOptions.HighlightBaseMembers;

            if (DiffAssemblyInfo)
                result |= DiffConfigurationOptions.DiffAssemblyInfo;

            if (StrikeRemoved)
                result |= DiffConfigurationOptions.StrikeRemoved;

            if (DiffAttributes)
                result |= DiffConfigurationOptions.DiffAttributes;

            if (ExcludeAddedTypes)
                result &= ~DiffConfigurationOptions.IncludeAddedTypes;

            if (ExcludeRemovedTypes)
                result &= ~DiffConfigurationOptions.IncludeRemovedTypes;

            if (IncludeInternalApis)
                result |= DiffConfigurationOptions.IncludeInternals;

            if (IncludePrivateApis)
                result |= DiffConfigurationOptions.IncludePrivates;

            return result;
        }

        private  DiffFormat GetDiffFormat()
        {
            switch (DiffWriter)
            {
                case DiffWriterType.CSharp:
                    switch (SyntaxWriter)
                    {
                        case SyntaxWriterType.Html:
                            return DiffFormat.Html;
                        case SyntaxWriterType.Text:
                            return DiffFormat.Text;
                        case SyntaxWriterType.Diff:
                            return DiffFormat.UnifiedDiff;
                        case SyntaxWriterType.Xml:
                            return DiffFormat.WordXml;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case DiffWriterType.CSV:
                    return DiffFormat.Csv;
                case DiffWriterType.Markdown:
                    return DiffFormat.Md;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public TextWriter GetOutput()
        {
            if (string.IsNullOrWhiteSpace(OutFile))
                return Console.Out;

            return new StreamWriter(OutFile, false, Encoding.UTF8);
        }
    }

    public enum SyntaxWriterType
    {
        Html,
        Text,
        Diff,
        Xml,
    }

    public enum DiffWriterType
    {
        CSharp,
        CSV,
        Markdown
    }
}
