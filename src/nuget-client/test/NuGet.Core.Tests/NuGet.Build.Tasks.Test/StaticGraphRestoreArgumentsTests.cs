// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class StaticGraphRestoreArgumentsTests
    {
        /// <summary>
        /// Verifies that the <see cref="StaticGraphRestoreArguments.Write(TextWriter)" /> and <see cref="StaticGraphRestoreArguments.Read(TextReader)" /> support global properties with complex characters like line breaks, XML, and JSON.
        /// </summary>
        [Fact]
        public void Read_WhenGLobalPropertiesContainComplexCharacters_CanBeRead()
        {
            var expected = new StaticGraphRestoreArguments
            {
                GlobalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    ["PublishDir"] = @"D:\RPublish",
                    ["SolutionFileName"] = "isomething.sln",
                    ["LangName"] = "en-US",
                    ["PublishTrimmed"] = "false",
                    ["CurrentSolutionConfigurationContents"] = @"<SolutionConfiguration></SolutionConfiguration>",
                    ["Configuration"] = "Release",
                    ["RuntimeIdentifier"] = "win-x64",
                    ["LangID"] = "1033",
                    ["PublishProtocol"] = "FileSystem",
                    ["ProjectExtensionsPathForSpecifiedProject"] = @"D:\src\isomething\_Build\obj\RawDataAnonymizer\publish\win-x64\",
                    ["_TargetId"] = "Folder",
                    ["SolutionDir"] = @"D:\src\isomething\",
                    ["SolutionExt"] = ".sln",
                    ["BuildingInsideVisualStudio"] = "true",
                    ["EnableBaseIntermediateOutputPathMismatchWarning"] = "false",
                    ["UseHostCompilerIfAvailable"] = "false",
                    ["SelfContained"] = "true",
                    ["ProjectToOverrideProjectExtensionsPath"] = @"D:\src\isomething\Apps\RawDataAnonymizer\RawDataAnonymizer.csproj",
                    ["DefineExplicitDefaults"] = "true",
                    ["PublishReadyToRun"] = "false",
                    ["Platform"] = "x64",
                    ["SolutionPath"] = @"D:\src\isomething\isomething.sln",
                    ["SolutionName"] = "isomething",
                    ["VSIDEResolvedNonMSBuildProjectOutputs"] = @"<VSIDEResolvedNonMSBuildProjectOutputs />",
                    ["PublishSingleFile"] = "false",
                    ["DevEnvDir"] = @"C:\Program Files\Microsoft Visual Studio\2022\Preview\Common7\IDE\",
                    ["ExcludeRestorePackageImports"] = "true",
                    ["OriginalMSBuildStartupDirectory"] = @"D:\src\isomething"
                },
                Options = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    [nameof(RestoreTaskEx.CleanupAssetsForUnsupportedProjects)] = bool.TrueString,
                    [nameof(RestoreTaskEx.Recursive)] = bool.TrueString,
                },
            };

            using var stream = new MemoryStream();

            expected.Write(stream);

            stream.Position = 0;

            StaticGraphRestoreArguments actual = StaticGraphRestoreArguments.Read(stream);

            actual.GlobalProperties.Should().BeEquivalentTo(expected.GlobalProperties);
            actual.Options.Should().BeEquivalentTo(expected.Options);
        }
    }
}
