// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.SolutionPersistence.Model;

// Constants for the project type table.
internal sealed partial class ProjectTypeTable
{
    internal static readonly ConfigurationRule[] NoBuildRules = [ModelHelper.CreateNoBuildRule()];

    internal static readonly Guid VCXProj = new Guid("8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942");
    internal static readonly Guid SolutionFolder = new Guid("2150E333-8FDC-42A3-9474-1A3956D46DE8");

    private static readonly ConfigurationRule[] ClrBuildRules = [ModelHelper.CreatePlatformRule(string.Empty, PlatformNames.AnyCPU)];

    private static ProjectTypeTable? implicitProjectTypes;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "Creating multi-item table.")]
    internal static ProjectTypeTable BuiltInTypes => implicitProjectTypes ??= new ProjectTypeTable(
        isBuiltIn: true,
        projectTypes: [

            // Base rules that apply to all project types.
            new ProjectType(
                Guid.Empty,
                rules: [

                    // Sets the project build type to be the same as the solution build type.
                    new ConfigurationRule(BuildDimension.BuildType, string.Empty, string.Empty, BuildTypeNames.All),

                    // Sets the project platform to be the same as the solution platform.
                    new ConfigurationRule(BuildDimension.Platform, string.Empty, string.Empty, PlatformNames.All),

                    // Sets the project build to true and deploy to false.
                    new ConfigurationRule(BuildDimension.Build, string.Empty, string.Empty, bool.TrueString),
                    new ConfigurationRule(BuildDimension.Deploy, string.Empty, string.Empty, bool.FalseString),
                ]),

            // Common Project System CLR projects.
            new ProjectType(new Guid("9A19103F-16F7-4668-BE54-9A1E7A4F7556"), ClrBuildRules) { Name = "Common C#" },
            new ProjectType(new Guid("778DAE3C-4631-46EA-AA77-85C1314464D9"), ClrBuildRules) { Name = "Common VB" },
            new ProjectType(new Guid("6EC3EE1D-3C4E-46DD-8F32-0CC8E7565705"), ClrBuildRules) { Name = "Common F#" },

            // Default CLR projects.
            new ProjectType(new Guid("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC"), ClrBuildRules) { Name = "C#", Extension = ".csproj" },
            new ProjectType(new Guid("F184B08F-C81C-45F6-A57F-5ABD9991F28F"), ClrBuildRules) { Name = "VB", Extension = ".vbproj" },
            new ProjectType(new Guid("F2A71F9B-5D33-465A-A702-920D77279786"), ClrBuildRules) { Name = "F#", Extension = ".fsproj" },

            // CLR shared code project
            new ProjectType(new Guid("D954291E-2A0B-460D-934E-DC6B0785DB48"), NoBuildRules) { Name = "Shared", Extension = ".shproj" },

            // Website project
            new ProjectType(new Guid("E24C65DC-7377-472B-9ABA-BC803B73C61A"), ClrBuildRules) { Name = "Website", Extension = ".webproj" },

            // Visual C++ project
            new ProjectType(
                VCXProj,
                rules: [
                    ModelHelper.CreatePlatformRule(PlatformNames.AnyCPU, PlatformNames.x64),
                    ModelHelper.CreatePlatformRule(PlatformNames.x86, PlatformNames.Win32),
                ])
            { Name = "VC", Extension = ".vcxproj" },

            // Visual C++ shared code project.
            // This is a special project type that is used to represent shared items in C++ projects.
            // It uses the same project type id as vcxproj, but doesn't have configurations.
            // It does not specify a name since it shares a project type id with vcxproj,
            // that way 'VC' is always used as the friendly name for the VCXProj guid.
            new ProjectType(VCXProj, NoBuildRules) { Extension = ".vcxitems" },

            // Exe project type
            new ProjectType(new Guid("911E67C6-3D85-4FCE-B560-20A9C3E3FF48"), NoBuildRules) { Name = "Exe", Extension = ".exe" },

            // This probably won't get used, but adding to make sure it doesn't see configurations.
            new ProjectType(SolutionFolder, NoBuildRules) { Name = "Folder" },

            // JavaScript project types
            new ProjectType(new Guid("54A90642-561A-4BB1-A94E-469ADEE60C69"), NoBuildRules) { Name = "Javascript", Extension = ".esproj" },
            new ProjectType(new Guid("9092AA53-FB77-4645-B42D-1CCCA6BD08BD"), NoBuildRules) { Name = "Node.js", Extension = ".njsproj" },

            // Setup project types
            new ProjectType(new Guid("151D2E53-A2C4-4D7D-83FE-D05416EBD58E"), NoBuildRules) { Name = "Deploy", Extension = ".deployproj" },
            new ProjectType(new Guid("54435603-DBB4-11D2-8724-00A0C9A8B90C"), NoBuildRules) { Name = "Installer", Extension = ".vsproj" },
            new ProjectType(new Guid("930C7802-8A8C-48F9-8165-68863BCCD9DD"), NoBuildRules) { Name = "Wix", Extension = ".wixproj" },

            // SQL project types
            new ProjectType(new Guid("00D1A9C2-B5F0-4AF3-8072-F6C62B433612"), NoBuildRules) { Name = "SQL", Extension = ".sqlproj" },
            new ProjectType(new Guid("0C603C2C-620A-423B-A800-4F3E2F6281F1"), NoBuildRules) { Name = "U-SQL-DB", Extension = ".usqldbproj" },
            new ProjectType(new Guid("182E2583-ECAD-465B-BB50-91101D7C24CE"), NoBuildRules) { Name = "U-SQL", Extension = ".usqlproj" },

            // Azure project types
            new ProjectType(new Guid("A07B5EB6-E848-4116-A8D0-A826331D98C6"), NoBuildRules) { Name = "Fabric", Extension = ".sfproj" },
            new ProjectType(new Guid("CC5FD16D-436D-48AD-A40C-5A424C6E3E79"), NoBuildRules) { Name = "Cloud Computing", Extension = ".ccproj" },
            new ProjectType(new Guid("E53339B2-1760-4266-BCC7-CA923CBCF16C"), NoBuildRules) { Name = "Docker", Extension = ".dcproj" },
        ]);
}
