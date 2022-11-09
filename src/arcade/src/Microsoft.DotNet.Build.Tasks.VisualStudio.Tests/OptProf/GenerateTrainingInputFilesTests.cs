// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Compression;
using Microsoft.Arcade.Test.Common;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio.UnitTests
{
    public class GenerateTrainingInputFilesTests
    {
        private readonly string s_optProfJson = @"
{
  ""products"": [
    {
      ""name"": ""Setup.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""filteredTestCases"" : [
            {
                ""filename"" : ""/x/y/z/Microsoft.CodeAnalysis.CSharp.dll"",
                ""testCases"": [
                    ""DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner""
                 ]
            },
            {
                ""filename"" : ""/x/y/z/Microsoft.CodeAnalysis.dll"",
                ""testCases"": [
                    ""DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging""
                 ]
            }
           ]
        },
        {
          ""container"": ""TeamEng"",
          ""testCases"" : [
            ""TeamEng.OptProfTest.vs_debugger_start_no_build_cs_scribble""
           ]
        }
      ]
    }
  ],
  ""assemblies"": [
    {
      ""assembly"": ""System.Collections.Immutable.dll"",
      ""instrumentationArguments"": [
        {
          ""relativeInstallationFolder"": ""Common7/IDE/PrivateAssemblies"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/15.0/Bin/Roslyn"",
          ""instrumentationExecutable"": ""Common7/IDE/zzz.exe""
        }
      ],
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging""
          ]
        }
      ]
    }
  ]
}
";
        private readonly string s_manifestJson = @"
{
  ""id"": ""Setup"",
  ""version"": ""42.42.42.4242424"",
  ""type"": ""Vsix"",
  ""vsixId"": ""0b5e8ddb-f12d-4131-a71d-77acc26a798f"",
  ""extensionDir"": ""[installdir]\\Common7\\IDE\\CommonExtensions\\Microsoft\\ManagedLanguages\\VBCSharp\\LanguageServices"",
  ""files"": [
    {
      ""fileName"": ""/extension.vsixmanifest"",
    },
    {
      ""fileName"": ""/SQLitePCLRaw.batteries_green.dll"",
    },
    {
      ""fileName"": ""/ko/Microsoft.CodeAnalysis.CSharp.resources.dll"",
      ""ngen"": true,
      ""ngenArchitecture"": ""All"",
      ""ngenApplication"": """",
      ""ngenPriority"": 3
    },
    {
      ""fileName"": ""/x/y/z/Microsoft.CodeAnalysis.CSharp.dll"",
      ""ngen"": true,
      ""ngenArchitecture"": ""All"",
      ""ngenApplication"": """",
      ""ngenPriority"": 3
    },
    {
      ""fileName"": ""/x/y/z/Microsoft.CodeAnalysis.dll"",
      ""ngen"": true,
      ""ngenArchitecture"": ""All"",
      ""ngenApplication"": """",
      ""ngenPriority"": 3
    },
    {
      ""fileName"": ""/x/y/z/Microsoft.CodeAnalysis.VisualBasic.dll"",
      ""ngen"": true,
      ""ngenArchitecture"": ""All"",
      ""ngenApplication"": """",
      ""ngenPriority"": 3
    }
  ]
}
";

        private static void CreateVsix(string vsixPath, string manifestContent)
        {
            using (var fileStream = new FileStream(vsixPath, FileMode.CreateNew))
            {
                using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    var entry = archive.CreateEntry("manifest.json");
                    using (var writer = new StreamWriter(entry.Open()))
                    {
                        writer.Write(manifestContent);
                    }
                }
            }
        }

        [Fact]
        public void Execute()
        {
            var temp = Path.GetTempPath();
            var dir = Path.Combine(temp, Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            var configPath = Path.Combine(dir, "OptProf.json");
            File.WriteAllText(configPath, s_optProfJson);

            var insertionDir = Path.Combine(dir, "Insertion");
            Directory.CreateDirectory(insertionDir);
            CreateVsix(Path.Combine(insertionDir, "Setup.vsix"), manifestContent: s_manifestJson);

            var outputDir = Path.Combine(dir, "Output");

            var task = new GenerateTrainingInputFiles()
            {
                ConfigurationFile = configPath,
                InsertionDirectory = insertionDir,
                OutputDirectory = outputDir
            };

            bool result = task.Execute();

            var entries = Directory.GetFileSystemEntries(outputDir, "*.*", SearchOption.AllDirectories);
            AssertEx.SetEqual(new[] 
            {
                Path.Combine(outputDir, @"DDRIT.RPS.CSharp"),
                Path.Combine(outputDir, @"TeamEng"),
                Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations"),
                Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging"),
                Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner"),
                Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging\System.Collections.Immutable.0.IBC.json"),
                Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging\System.Collections.Immutable.1.IBC.json"),
                Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging\xyzMicrosoft.CodeAnalysis.0.IBC.json"),
                Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner\xyzMicrosoft.CodeAnalysis.CSharp.0.IBC.json"),
                Path.Combine(outputDir, @"TeamEng\Configurations"),
                Path.Combine(outputDir, @"TeamEng\Configurations\TeamEng.OptProfTest.vs_debugger_start_no_build_cs_scribble"),
                Path.Combine(outputDir, @"TeamEng\Configurations\TeamEng.OptProfTest.vs_debugger_start_no_build_cs_scribble\xyzMicrosoft.CodeAnalysis.0.IBC.json"),
                Path.Combine(outputDir, @"TeamEng\Configurations\TeamEng.OptProfTest.vs_debugger_start_no_build_cs_scribble\xyzMicrosoft.CodeAnalysis.CSharp.0.IBC.json"),
                Path.Combine(outputDir, @"TeamEng\Configurations\TeamEng.OptProfTest.vs_debugger_start_no_build_cs_scribble\xyzMicrosoft.CodeAnalysis.VisualBasic.0.IBC.json")
            }, entries);


            var json = File.ReadAllText(Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging\System.Collections.Immutable.0.IBC.json"));
            Assert.Equal(
@"{
  ""Technology"": ""IBC"",
  ""RelativeInstallationPath"": ""Common7\\IDE\\PrivateAssemblies\\System.Collections.Immutable.dll"",
  ""InstrumentationArguments"": ""/ExeConfig:\""%VisualStudio.InstallationUnderTest.Path%\\Common7\\IDE\\vsn.exe\""""
}
", json);

            JObject.Parse(json);

            json = File.ReadAllText(Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging\System.Collections.Immutable.1.IBC.json"));
            Assert.Equal(
@"{
  ""Technology"": ""IBC"",
  ""RelativeInstallationPath"": ""MSBuild\\15.0\\Bin\\Roslyn\\System.Collections.Immutable.dll"",
  ""InstrumentationArguments"": ""/ExeConfig:\""%VisualStudio.InstallationUnderTest.Path%\\Common7\\IDE\\zzz.exe\""""
}
", json);

            JObject.Parse(json);

            json = File.ReadAllText(Path.Combine(outputDir, @"DDRIT.RPS.CSharp\Configurations\DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner\xyzMicrosoft.CodeAnalysis.CSharp.0.IBC.json"));
            Assert.Equal(
@"{
  ""Technology"": ""IBC"",
  ""RelativeInstallationPath"": ""Common7\\IDE\\CommonExtensions\\Microsoft\\ManagedLanguages\\VBCSharp\\LanguageServices\\x\\y\\z\\Microsoft.CodeAnalysis.CSharp.dll"",
  ""InstrumentationArguments"": ""/ExeConfig:\""%VisualStudio.InstallationUnderTest.Path%\\Common7\\IDE\\vsn.exe\""""
}
", json);
            JObject.Parse(json);

            json = File.ReadAllText(Path.Combine(outputDir, @"TeamEng\Configurations\TeamEng.OptProfTest.vs_debugger_start_no_build_cs_scribble\xyzMicrosoft.CodeAnalysis.VisualBasic.0.IBC.json"));
            Assert.Equal(
@"{
  ""Technology"": ""IBC"",
  ""RelativeInstallationPath"": ""Common7\\IDE\\CommonExtensions\\Microsoft\\ManagedLanguages\\VBCSharp\\LanguageServices\\x\\y\\z\\Microsoft.CodeAnalysis.VisualBasic.dll"",
  ""InstrumentationArguments"": ""/ExeConfig:\""%VisualStudio.InstallationUnderTest.Path%\\Common7\\IDE\\vsn.exe\""""
}
", json);
            JObject.Parse(json);

            Assert.True(result);
            Directory.Delete(dir, recursive: true);
        }
    }
}
