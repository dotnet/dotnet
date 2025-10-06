// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.SlnV12;

internal static class SlnConstants
{
    internal const string ProjectSeparators = " ()=\",";
    internal const string SectionSeparators = " \t()=";
    internal const string SectionSeparators2 = "\t()=";
    internal const string VersionSeparators = " =";
    internal const char DoubleQuote = '"';

    internal const string SLNFileHeaderNoVersion = "Microsoft Visual Studio Solution File, Format Version";
    internal const string SLNFileHeaderVersion = " 12.00";

    // Special property Visual Studio property names
    internal const string HideSolutionNode = nameof(HideSolutionNode);
    internal const string SolutionGuid = nameof(SolutionGuid);

    // Special property names
    internal const string Description = nameof(Description);

    // Used in .SLN to determine with version of VS to open when opening from explorer.
    internal const string OpenWithPrefix = "# ";

    internal const string TagProjectStart = "Project(";
    internal const string TagProjectSectionStart = "ProjectSection(";
    internal const string TagGlobalSectionStart = "GlobalSection(";

    internal const string TagProject = "Project";
    internal const string TagGlobal = "Global";
    internal const string TagSection = "Section";
    internal const string TagGlobalSection = "GlobalSection";
    internal const string TagProjectSection = "ProjectSection";

    internal const string TagEndProject = "EndProject";
    internal const string TagEndGlobal = "EndGlobal";
    internal const string TagEndGlobalSection = "EndGlobalSection";
    internal const string TagEndProjectSection = "EndProjectSection";

    internal const string TagPreSolution = "preSolution";
    internal const string TagPostSolution = "postSolution";
    internal const string TagPreProject = "preProject";
    internal const string TagPostProject = "postProject";

    internal const string TagVisualStudioVersion = "VisualStudioVersion";
    internal const string TagMinimumVisualStudioVersion = "MinimumVisualStudioVersion";
    internal const string TagAssignValue = " = ";
    internal const string TagQuoteCommaQuote = "\", \"";
    internal const string TagTabTab = "\t\t";

    // This should only be use in SLN files.
    internal static string ToSlnString(this Guid guid) => guid.ToString("B").ToUpperInvariant();
}
