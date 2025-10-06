// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.SolutionPersistence.Model;
using Microsoft.VisualStudio.SolutionPersistence.Utilities;

namespace Microsoft.VisualStudio.SolutionPersistence.Serializer.Xml;

internal enum Keyword
{
    Unknown,

    // Root element
    Solution,

    // Solution properties
    Description,
    Version,

    // Solution sections
    Configurations,

    // item (folders and project) properties
    Folder,
    Project,
    Id,
    Name,
    Path,
    Type,
    DisplayName,
    File,
    BuildDependency,

    // ProjectType properties
    ProjectType,
    TypeId,
    Extension,
    BasedOn,
    IsBuildable,

    // Configuration properties
    Configuration,
    Dimension,
    BuildType,
    Platform,
    Build,
    Deploy,

    // Properties
    Property,
    Properties,
    Scope,
    PostLoad,
    PreLoad,
    Value,

    MaxProp,
}

internal static class Keywords
{
    internal const string XmlTrue = "true";
    internal const string XmlFalse = "false";

    private static readonly string[] KeywordToString;
    private static readonly Lictionary<string, Keyword> StringToKeyword;

    static Keywords()
    {
        StringToKeyword = new Lictionary<string, Keyword>(
            [
                new(nameof(Keyword.Solution), Keyword.Solution),
                new(nameof(Keyword.Description), Keyword.Description),
                new(nameof(Keyword.Version), Keyword.Version),
                new(nameof(Keyword.Configurations), Keyword.Configurations),
                new(nameof(Keyword.Folder), Keyword.Folder),
                new(nameof(Keyword.Project), Keyword.Project),
                new(nameof(Keyword.Id), Keyword.Id),
                new(nameof(Keyword.Name), Keyword.Name),
                new(nameof(Keyword.Path), Keyword.Path),
                new(nameof(Keyword.Type), Keyword.Type),
                new(nameof(Keyword.DisplayName), Keyword.DisplayName),
                new(nameof(Keyword.File), Keyword.File),
                new(nameof(Keyword.BuildDependency), Keyword.BuildDependency),
                new(nameof(Keyword.ProjectType), Keyword.ProjectType),
                new(nameof(Keyword.TypeId), Keyword.TypeId),
                new(nameof(Keyword.Extension), Keyword.Extension),
                new(nameof(Keyword.BasedOn), Keyword.BasedOn),
                new(nameof(Keyword.IsBuildable), Keyword.IsBuildable),
                new(nameof(Keyword.Configuration), Keyword.Configuration),
                new(nameof(Keyword.Dimension), Keyword.Dimension),
                new(nameof(Keyword.BuildType), Keyword.BuildType),
                new(nameof(Keyword.Platform), Keyword.Platform),
                new(nameof(Keyword.Build), Keyword.Build),
                new(nameof(Keyword.Deploy), Keyword.Deploy),
                new(nameof(Keyword.Property), Keyword.Property),
                new(nameof(Keyword.Properties), Keyword.Properties),
                new(nameof(Keyword.Scope), Keyword.Scope),
                new(nameof(Keyword.PostLoad), Keyword.PostLoad),
                new(nameof(Keyword.PreLoad), Keyword.PreLoad),
                new(nameof(Keyword.Value), Keyword.Value),
            ],
            StringComparer.OrdinalIgnoreCase);

        KeywordToString = new string[(int)Keyword.MaxProp];
        foreach ((string keywordStr, Keyword keyword) in StringToKeyword)
        {
            KeywordToString[(int)keyword] = keywordStr;
        }
    }

    internal static string ToXmlString(this Keyword keyword) => KeywordToString[(int)keyword];  // let it throw

    internal static string ToXmlBool(this bool value) => value ? XmlTrue : XmlFalse;

    internal static Keyword ToKeyword(string name) =>
        !string.IsNullOrEmpty(name) && StringToKeyword.TryGetValue(name, out Keyword ret) ? ret : Keyword.Unknown;

    // Adds common solution constants to string table.
    internal static StringTable WithSolutionConstants(this StringTable stringTable)
    {
        // Try to use the interned strings for common solution values.
        stringTable.AddString(XmlTrue);
        stringTable.AddString(XmlFalse);
        stringTable.AddString(BuildTypeNames.Debug);
        stringTable.AddString(BuildTypeNames.Release);
        stringTable.AddString(PlatformNames.All);
        stringTable.AddString(PlatformNames.AnyCPU);
        stringTable.AddString(PlatformNames.AnySpaceCPU);
        stringTable.AddString(PlatformNames.Win32);
        stringTable.AddString(PlatformNames.x64);
        stringTable.AddString(PlatformNames.x86);
        stringTable.AddString(PlatformNames.arm);
        stringTable.AddString(PlatformNames.arm64);
        stringTable.AddString(PlatformNames.ARM);
        stringTable.AddString(PlatformNames.ARM64);

        foreach (string propertyName in KeywordToString)
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                stringTable.AddString(propertyName);
            }
        }

        return stringTable;
    }
}
