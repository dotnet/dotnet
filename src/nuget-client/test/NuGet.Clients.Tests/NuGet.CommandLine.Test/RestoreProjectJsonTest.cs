// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.ProjectModel;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.CommandLine.Test
{
    public class RestoreProjectJsonTest
    {
        [Fact]
        public async Task RestoreProjectJson_MinClientVersionFailAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);

                var packageContext = new SimpleTestPackageContext()
                {
                    Id = "packageA",
                    Version = "1.0.0",
                    MinClientVersion = "9.9.9"
                };

                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageContext);

                var projectJson = @"{
                                                    'dependencies': {
                                                    'packageA': '1.0.0'
                                                    },
                                                    'frameworks': {
                                                            'uap10.0': { }
                                                        }
                                                 }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Contains("'packageA 1.0.0' package requires NuGet client version '9.9.9' or above", r.Errors);
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreFolder_VerifyFailure()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir1 = Path.Combine(workingPath, "test1");
                var projectDir2 = Path.Combine(workingPath, "test2");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(projectDir2);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                    }");

                Util.CreateFile(projectDir2, "project.json",
                                    @"{
                                        'version': '1.0.0-*',
                                        'dependencies': {
                                        },
                                        'frameworks': {
                                                    'uap10.0': { }
                                                }
                                        }");

                var args = new string[] {
                    "restore",
                    workingPath,
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));
                var test2Lock = new FileInfo(Path.Combine(projectDir2, "project.lock.json"));

                Assert.False(test1Lock.Exists);
                Assert.False(test2Lock.Exists);
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreForSingleProject()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir1 = Path.Combine(workingPath, "test1");
                var projectDir2 = Path.Combine(workingPath, "test2");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(projectDir2);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                    }");

                var project1Path = Path.Combine(projectDir1, "test1.csproj");
                Util.CreateFile(projectDir1, "test1.csproj", Util.GetCSProjXML("test1"));

                Util.CreateFile(projectDir2, "project.json",
                                    @"{
                                        'version': '1.0.0-*',
                                        'dependencies': {
                                        },
                                        'frameworks': {
                                                    'uap10.0': { }
                                                }
                                        }");

                Util.CreateFile(projectDir2, "test2.csproj", Util.GetCSProjXML("test2"));

                var slnPath = Path.Combine(workingPath, "xyz.sln");

                Util.CreateFile(workingPath, "xyz.sln",
                           @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{AAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test1"", ""test1\test1.csproj"", ""{AA6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{BBE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test2"", ""test2\test2.csproj"", ""{BB6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                        ");

                var args = new string[] {
                    "restore",
                    project1Path,
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));
                var test2Lock = new FileInfo(Path.Combine(projectDir2, "project.lock.json"));

                Assert.True(test1Lock.Exists);
                Assert.False(test2Lock.Exists);
            }
        }

        [Fact]
        public async Task RestoreProjectJson_RestoreWithFallbackFolderAsync()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var globalPath = Path.Combine(workingPath, "global");
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var fallbackFolder = Path.Combine(workingPath, "fallback");
                var projectDir1 = Path.Combine(workingPath, "test1");
                var projectDir2 = Path.Combine(workingPath, "test2");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(projectDir2);
                Directory.CreateDirectory(fallbackFolder);
                Directory.CreateDirectory(globalPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                var config = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <config>
        <add key=""globalPackagesFolder"" value=""{globalPath}"" />
    </config>
    <fallbackPackageFolders>
        <clear />
        <add key=""a"" value=""{fallbackFolder}"" />
    </fallbackPackageFolders>
    <packageSources>
        <clear />
        <add key=""a"" value=""{repositoryPath}"" />
    </packageSources>
</configuration>";

                File.WriteAllText(Path.Combine(workingPath, "NuGet.Config"), config);

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                    }");

                var project1Path = Path.Combine(projectDir1, "test1.csproj");
                Util.CreateFile(projectDir1, "test1.csproj", Util.GetCSProjXML("test1"));

                Util.CreateFile(projectDir2, "project.json",
                                    @"{
                                        'version': '1.0.0-*',
                                        'dependencies': {
                                            'packageA': '1.0.0',
                                            'packageB': '1.0.0'
                                        },
                                        'frameworks': {
                                                    'uap10.0': { }
                                                }
                                        }");

                await SimpleTestPackageUtility.CreateFolderFeedV3Async(
                    fallbackFolder,
                    new PackageIdentity("packageA", NuGetVersion.Parse("1.0.0")),
                    new PackageIdentity("packageB", NuGetVersion.Parse("1.0.0")));

                var args = new string[] {
                    "restore",
                    project1Path
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));

                Assert.True(test1Lock.Exists);
                Assert.Equal(0, Directory.GetDirectories(globalPath).Length);
                Assert.Equal(2, Directory.GetDirectories(fallbackFolder).Length);
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreFromSlnWithCsproj()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir1 = Path.Combine(workingPath, "test1");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                    }");

                Util.CreateFile(projectDir1, "test1.csproj", Util.GetCSProjXML("test1"));

                var slnPath = Path.Combine(workingPath, "xyz.sln");

                Util.CreateFile(workingPath, "xyz.sln",
                           @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{AAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test1"", ""test1\test1.csproj"", ""{AA6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                        ");

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    slnPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.True(test1Lock.Exists);
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreFromSlnWithCsproj_InconsitentCaseForProjectRef()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir1 = Path.Combine(workingPath, "FolderA", "test1");
                var projectDir2 = Path.Combine(workingPath, "FolderB", "test2");
                var projectDir3 = Path.Combine(workingPath, "FolderB", "test3");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(Path.Combine(workingPath, "FolderA"));
                Directory.CreateDirectory(Path.Combine(workingPath, "FolderB"));
                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(projectDir2);
                Directory.CreateDirectory(projectDir3);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'net45': { }
                                                            }
                                                    }");
                Util.CreateFile(projectDir2, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'net45': { }
                                                            }
                                                    }");
                Util.CreateFile(projectDir3, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'net45': { }
                                                            }
                                                    }");

                Util.CreateFile(projectDir1, "test1.csproj", Util.GetCSProjXML("test1"));
                Util.CreateFile(projectDir2, "test2.csproj", Util.GetCSProjXML("test2"));
                Util.CreateFile(projectDir3, "test3.csproj", Util.GetCSProjXML("test3"));
                using (var stream = new FileStream(Path.Combine(projectDir2, "test2.csproj"), FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>
                    {
                        { "Project", "AA6279C1-B5EE-4C6B-9FA3-A794CE195136" },
                        { "Name", "test1" }
                    };
                    ProjectFileUtils.AddItem(
                            xml,
                            "ProjectReference",
                            @"..\..\folderA\Test1\Test1.csproj",
                            string.Empty,
                            properties,
                            attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }
                using (var stream = new FileStream(Path.Combine(projectDir3, "test3.csproj"), FileMode.Open, FileAccess.ReadWrite))
                {
                    var xml = XDocument.Load(stream);

                    var attributes = new Dictionary<string, string>();

                    var properties = new Dictionary<string, string>
                    {
                        { "Project", "AA6279C1-B5EE-4C6B-9FA3-A794CE195136" },
                        { "Name", "test1" }
                    };
                    ProjectFileUtils.AddItem(
                            xml,
                            "ProjectReference",
                            @"..\..\FolderA\Test1\Test1.csproj",
                            string.Empty,
                            properties,
                            attributes);

                    ProjectFileUtils.WriteXmlToFile(xml, stream);
                }


                var slnPath = Path.Combine(workingPath, "xyz.sln");

                Util.CreateFile(workingPath, "xyz.sln",
                           @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test1"", ""FolderA\test1\test1.csproj"", ""{AA6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test2"", ""FolderB\test2\test2.csproj"", ""{CBDF173F-6E99-44A7-937F-E4A87BF3A3E0}""
                        EndProject
                        Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test3"", ""FolderB\test3\test3.csproj"", ""{D39F3B98-4518-4204-858F-3B7BF88D6DC1}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {CBDF173F-6E99-44A7-937F-E4A87BF3A3E0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {CBDF173F-6E99-44A7-937F-E4A87BF3A3E0}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {D39F3B98-4518-4204-858F-3B7BF88D6DC1}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {D39F3B98-4518-4204-858F-3B7BF88D6DC1}.Debug|Any CPU.Build.0 = Debug|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                        ");

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    slnPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                Assert.True(test1Lock.Exists);

            }
        }

        [Theory]
        [InlineData(null, 1, 2 * 60 * 1000)]
        [InlineData(null, 2, 2 * 60 * 1000)]
        [InlineData(null, 40, 4 * 60 * 1000)]
        [InlineData(null, 30, 3 * 60 * 1000)]
        [InlineData("0", 1, 2 * 60 * 1000)]
        [InlineData("-1", 1, 2 * 60 * 1000)]
        [InlineData("10", 1, 10000)]
        [InlineData("10", 2, 10000)]
        public void RestoreProjectJson_P2PTimeouts(string timeout, int projectCount, int expectedTimeOut)
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                string getProjectDir(int i) => Path.Combine(workingPath, "test" + i);

                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                for (var i = 1; i <= projectCount; i++)
                {
                    var projectDir = getProjectDir(i);

                    Directory.CreateDirectory(projectDir);

                    Util.CreateFile(projectDir, "project.json",
                                                    @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                    }");

                    Util.CreateFile(projectDir, $"test{i}.csproj", Util.GetCSProjXML($"test{i}"));
                }

                var slnPath = Path.Combine(workingPath, "xyz.sln");

                var sln = new StringBuilder();

                sln.AppendLine(@"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1");

                var guids = new string[projectCount + 1];

                for (var i = 1; i <= projectCount; i++)
                {
                    guids[i] = Guid.NewGuid().ToString().ToUpper();
                    var projGuid = guids[i];

                    sln.AppendLine(
@"                        Project(""{" + Guid.NewGuid().ToString().ToUpper() + @"}"") = ""test" + i +
                            @""", ""test" + i + @"\test" + i + @".csproj"", ""{" + projGuid + @"}""
                        EndProject");
                }

                sln.AppendLine(
@"                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution");

                for (var i = 0; i < projectCount; i++)
                {
                    sln.AppendLine($"                                {guids[i]}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                    sln.AppendLine($"                                {guids[i]}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                }

                sln.AppendLine(@"                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal");

                var solution = sln.ToString();

                Util.CreateFile(workingPath, "xyz.sln", solution);

                string args;

                if (timeout == null)
                {
                    args = $"restore -verbosity detailed -Source {repositoryPath} -solutionDir {workingPath} {slnPath}";
                }
                else
                {
                    args = $"restore -verbosity detailed -Source {repositoryPath} -solutionDir {workingPath} -Project2ProjectTimeOut {timeout} {slnPath}";
                }
                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    args);

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + Environment.NewLine + r.Errors);

                var lines = r.Output.Split(
                                new[] { Environment.NewLine },
                                StringSplitOptions.RemoveEmptyEntries);

                var prefix = "MSBuild P2P timeout [ms]: ";

                var timeoutLineResult = lines.Where(line => line.Contains(prefix)).SingleOrDefault();

                Assert.NotNull(timeoutLineResult);

                var timeoutResult = timeoutLineResult.Substring(timeoutLineResult.IndexOf(prefix) + prefix.Length);
                Assert.Equal(expectedTimeOut, int.Parse(timeoutResult));

                for (var i = 1; i < projectCount + 1; i++)
                {
                    var test1Lock = new FileInfo(Path.Combine(getProjectDir(i), "project.lock.json"));

                    Assert.True(test1Lock.Exists);
                }
            }
        }

        [Fact]
        public async Task RestoreProjectJson_RestoreFromSlnWithReferenceOutputAssemblyFalse()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir1 = Path.Combine(workingPath, "test1");
                var projectDir2 = Path.Combine(workingPath, "test2");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(projectDir2);
                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                    }");

                var test1Xml = Util.GetCSProjXML("test1");
                var doc = XDocument.Parse(test1Xml);
                var projectNode = doc.Root;

                var projectRef = XElement.Parse(@"<ItemGroup Label=""Project References"">
                            <ProjectReference Include=""" + projectDir2 + @"\\test2.csproj"">
                              <Project>{BB6279C1-B5EE-4C6B-9FA3-A794CE195136}</Project>
                              <Name>Test2</Name>
                              <ReferenceOutputAssembly>false</ReferenceOutputAssembly>
                            </ProjectReference>
                            </ItemGroup>");

                projectNode.Add(projectRef);
                var xml = doc.ToString().Replace("xmlns=\"\"", "");

                Util.CreateFile(projectDir1, "test1.csproj", xml);

                Util.CreateFile(projectDir2, "project.json",
                                    @"{
                                        'version': '1.0.0-*',
                                        'dependencies': {
                                            ""packageA"": ""1.0.0""
                                        },
                                        'frameworks': {
                                                    'uap10.0': { }
                                                }
                                        }");

                Util.CreateFile(projectDir2, "test2.csproj", Util.GetCSProjXML("test2"));

                var slnPath = Path.Combine(workingPath, "xyz.sln");

                Util.CreateFile(workingPath, "xyz.sln",
                           @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{AAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test1"", ""test1\test1.csproj"", ""{AA6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{BBE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test2"", ""test2\test2.csproj"", ""{BB6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                        ");

                var packageA = new SimpleTestPackageContext("packageA", "1.0.0");
                packageA.AddFile("lib/uap/a.dll", "a");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    slnPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));
                var test2Lock = new FileInfo(Path.Combine(projectDir2, "project.lock.json"));

                var format = new LockFileFormat();
                var lockFile1 = format.Read(test1Lock.FullName);
                var lockFile2 = format.Read(test2Lock.FullName);

                var a1 = lockFile1.Libraries
                    .Where(lib => lib.Name.Equals("packageA", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                var a2 = lockFile2.Libraries
                    .Where(lib => lib.Name.Equals("packageA", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

                Assert.True(test1Lock.Exists);
                Assert.True(test2Lock.Exists);

                // Verify the package does exist in 2
                Assert.NotNull(a2);

                // Verify the package does not flow to 1
                Assert.Null(a1);
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreProjectFileNotFound()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                var projectFilePath = Path.Combine(workingPath, "test.fsproj");

                var args = new string[] {
                    "restore",
                    projectFilePath,
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var test1Lock = new FileInfo(Path.Combine(workingPath, "project.lock.json"));

                // Assert
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.False(test1Lock.Exists);
                Assert.Contains("input file does not exist", r.Errors, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreProjectJsonFileNotFound()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                var projectJsonPath = Path.Combine(workingPath, "project.json");

                var args = new string[] {
                "restore",
                projectJsonPath,
                "-Source",
                repositoryPath,
                "-solutionDir",
                workingPath
            };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var test1Lock = new FileInfo(Path.Combine(workingPath, "project.lock.json"));

                // Assert
                Assert.True(1 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.False(test1Lock.Exists);
                Assert.Contains("input file does not exist", r.Errors, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreCSProj()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir1 = Path.Combine(workingPath, "test1");
                var project1Path = Path.Combine(projectDir1, "test1.csproj");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                            'dependencies': {
                                            },
                                            'frameworks': {
                                                        'uap10.0': { }
                                                    }
                                            }");

                Util.CreateFile(projectDir1, "test1.csproj", Util.GetCSProjXML("test1"));

                var args = new string[] {
                    "restore",
                    project1Path,
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(test1Lock.Exists);
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreUnknownProj()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir1 = Path.Combine(workingPath, "test1");
                var project1Path = Path.Combine(projectDir1, "test1.abcproj");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateConfigForGlobalPackagesFolder(workingPath);

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                            'dependencies': {
                                            },
                                            'frameworks': {
                                                        'uap10.0': { }
                                                    }
                                            }");

                Util.CreateFile(projectDir1, "test1.abcproj", Util.GetCSProjXML("test1"));

                var args = new string[] {
                    "restore",
                    project1Path,
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(test1Lock.Exists);
            }
        }

        [Fact]
        public async Task RestoreProjectJson_RestoreFromSlnWithUnknownProjAndCsproj()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir1 = Path.Combine(workingPath, "test1");
                var projectDir2 = Path.Combine(workingPath, "test2");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(projectDir2);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));

                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                packageA.AddFile("lib/uap/a.dll", "a");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA);

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                            'dependencies': {
                                            'packageA': '1.1.0-beta-*'
                                            },
                                            'frameworks': {
                                                        'uap10.0': { }
                                                    }
                                            }");

                Util.CreateFile(projectDir1, "test1.csproj", Util.GetCSProjXML("test1"));

                Util.CreateFile(projectDir2, "project.json",
                                    @"{
                                        'version': '1.0.0-*',
                                        'dependencies': {
                                        'packageA': '1.1.0-beta-*'
                                        },
                                        'frameworks': {
                                                    'uap10.0': { }
                                                }
                                        }");

                Util.CreateFile(projectDir2, "test2.abcproj", Util.GetCSProjXML("test2"));

                var slnPath = Path.Combine(workingPath, "xyz.sln");

                Util.CreateFile(workingPath, "xyz.sln",
                        @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{AAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test1"", ""test1\test1.csproj"", ""{AA6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{BBE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test2"", ""test2\test2.abcproj"", ""{BB6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                    ");

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    slnPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));
                var test2Lock = new FileInfo(Path.Combine(projectDir2, "project.lock.json"));

                Assert.True(test1Lock.Exists);
                Assert.True(test2Lock.Exists);
            }
        }

        // Verify that the settings for the solution are used for all projects
        [Fact]
        public async Task RestoreProjectJson_RestoreFromSlnUsesNuGetFolderSettingsAsync()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");

                var solutionDir = Path.Combine(workingPath, "a", "b", "solution");
                var nugetDir = Path.Combine(solutionDir, ".nuget");

                // Project 1 is under the solution
                var projectDir1 = Path.Combine(solutionDir, "test1");

                // Project 2 is above
                var projectDir2 = Path.Combine(workingPath, "test2");

                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(projectDir1);
                Directory.CreateDirectory(projectDir2);
                Directory.CreateDirectory(nugetDir);

                // Write the config to the .nuget folder, this contains the source needed for restore
                Util.CreateNuGetConfig(workingPath, new List<string>() { repositoryPath });

                // Move the NuGet.Config file down into the .nuget folder
                File.Move(Path.Combine(workingPath, "NuGet.Config"), Path.Combine(nugetDir, "NuGet.Config"));

                Util.CreateFile(projectDir1, "project.json",
                                                @"{
                                                    'dependencies': {
                                                        'packageA': '1.0.0'
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                    }");

                Util.CreateFile(projectDir1, "test1.csproj", Util.GetCSProjXML("test1"));

                Util.CreateFile(projectDir2, "project.json",
                                    @"{
                                        'version': '1.0.0-*',
                                        'dependencies': {
                                            'packageB': '1.0.0'
                                        },
                                        'frameworks': {
                                                    'uap10.0': { }
                                                }
                                        }");

                Util.CreateFile(projectDir2, "test2.csproj", Util.GetCSProjXML("test2"));

                // Create bad configs in the project directories, this will cause
                // the restore to fail if they are used (they shouldn't be used)
                Util.CreateFile(projectDir1, "NuGet.Config", "<badXml");
                Util.CreateFile(projectDir2, "NuGet.Config", "<badXml");

                var slnPath = Path.Combine(solutionDir, "xyz.sln");

                Util.CreateFile(solutionDir, "xyz.sln",
                           @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{AAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test1"", ""$TEST1DIR$\test1.csproj"", ""{AA6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{BBE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test2"", ""$TEST2DIR$\test2.csproj"", ""{BB6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {AA6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {BB6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                        ".Replace("$TEST1DIR$", projectDir1).Replace("$TEST2DIR$", projectDir2));

                var args = new string[] {
                    "restore",
                    "-solutionDir",
                    workingPath,
                    slnPath
                };

                // Create the packages needed by the projects
                await SimpleTestPackageUtility.CreateFullPackageAsync(repositoryPath, "packageA", "1.0.0");
                await SimpleTestPackageUtility.CreateFullPackageAsync(repositoryPath, "packageB", "1.0.0");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                // Verify restore worked, this requires finding the packages from the repository, which is in 
                // the solution level nuget.config.
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);

                var test1Lock = new FileInfo(Path.Combine(projectDir1, "project.lock.json"));
                var test2Lock = new FileInfo(Path.Combine(projectDir2, "project.lock.json"));

                Assert.True(test1Lock.Exists);
                Assert.True(test2Lock.Exists);
            }
        }

        [Fact]
        public void RestoreProjectJson_FloatReleaseLabelHighestPrelease()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateTestPackage("packageA", "1.0.0-alpha", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.0-beta-01", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.0-beta-02", repositoryPath);
                var projectJson = @"{
                                        'dependencies': {
                                        'packageA': '1.0.0-*'
                                        },
                                        'frameworks': {
                                                'uap10.0': { }
                                            }
                                        }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectPath,
                    "-nocache"
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var lockFilePath = Path.Combine(workingPath, "project.lock.json");
                var lockFileFormat = new LockFileFormat();

                var lockFile = lockFileFormat.Read(lockFilePath);

                var installedA = lockFile.Targets.First().Libraries.Single(package => package.Name == "packageA");

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal("1.0.0-beta-02", installedA.Version.ToNormalizedString());
            }
        }

        [Fact]
        public void RestoreProjectJson_FloatReleaseLabelTakesStable()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.0", repositoryPath);
                Util.CreateTestPackage("packageA", "2.0.0", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.0-alpha", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.0-beta-01", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.0-beta-02", repositoryPath);
                var projectJson = @"{
                                                    'dependencies': {
                                                    'packageA': '1.0.0-*'
                                                    },
                                                    'frameworks': {
                                                            'uap10.0': { }
                                                        }
                                                  }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var lockFilePath = Path.Combine(workingPath, "project.lock.json");
                var lockFileFormat = new LockFileFormat();

                var lockFile = lockFileFormat.Read(lockFilePath);

                var installedA = lockFile.Targets.First().Libraries.Single(package => package.Name == "packageA");

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal("1.0.0", installedA.Version.ToNormalizedString());
            }
        }

        [Fact]
        public void RestoreProjectJson_FloatIncludesStableOnly()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateTestPackage("packageA", "1.0.0", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.9", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.10", repositoryPath);
                Util.CreateTestPackage("packageA", "1.1.15", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.15-beta", repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.9-beta", repositoryPath);

                var projectJson = @"{
                                        ""dependencies"": {
                                        ""packageA"": ""1.0.*""
                                        },
                                        ""frameworks"": {
                                                ""uap10.0"": { }
                                            }
                                        }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var lockFilePath = Path.Combine(workingPath, "project.lock.json");
                var lockFileFormat = new LockFileFormat();

                var lockFile = lockFileFormat.Read(lockFilePath);

                var installedA = lockFile.Targets.First().Libraries.Single(package => package.Name == "packageA");

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal("1.0.10", installedA.Version.ToNormalizedString());
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreFiltersToStablePackages()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Util.CreateTestPackage("packageA", "1.0.0", repositoryPath, "win8", "packageB", "1.0.0");
                Util.CreateTestPackage("packageB", "1.0.0-beta", repositoryPath);
                Util.CreateTestPackage("packageB", "2.0.0-beta", repositoryPath);
                Util.CreateTestPackage("packageB", "3.0.0", repositoryPath);
                var projectJson = @"{
                                                    'dependencies': {
                                                    'packageA': '1.0.0'
                                                    },
                                                    'frameworks': {
                                                            'uap10.0': { }
                                                        }
                                                 }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var lockFilePath = Path.Combine(workingPath, "project.lock.json");
                var lockFileFormat = new LockFileFormat();

                var lockFile = lockFileFormat.Read(lockFilePath);

                var installedB = lockFile.Targets.First().Libraries.Where(package => package.Name == "packageB").ToList();

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal(1, installedB.Count);
                Assert.Equal("3.0.0", installedB.Single().Version.ToNormalizedString());
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreBumpsFromStableToPrereleaseWhenNeeded()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateTestPackage("packageA", "1.0.0", repositoryPath, "win8", "packageC", "1.0.0");
                Util.CreateTestPackage("packageB", "1.0.0-beta", repositoryPath, "win8", "packageC", "2.0.0-beta");
                Util.CreateTestPackage("packageC", "1.0.0", repositoryPath);
                Util.CreateTestPackage("packageC", "2.0.0-beta", repositoryPath);
                var projectJson = @"{
                                        'dependencies': {
                                        'packageA': '1.0.0',
                                        'packageB': '1.0.0-*'
                                        },
                                        'frameworks': {
                                                'uap10.0': { }
                                            }
                                        }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var lockFilePath = Path.Combine(workingPath, "project.lock.json");
                var lockFileFormat = new LockFileFormat();

                var lockFile = lockFileFormat.Read(lockFilePath);

                var installedC = lockFile.Targets.First().Libraries.Single(package => package.Name == "packageC");

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal("2.0.0-beta", installedC.Version.ToNormalizedString());
            }
        }

        [Fact]
        public void RestoreProjectJson_RestoreDowngradesStableDependency()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateTestPackage("packageA", "1.0.0", repositoryPath, "win8", "packageC", "1.0.0");
                Util.CreateTestPackage("packageB", "1.0.0", repositoryPath, "win8", "packageC", "[2.1.0]");
                Util.CreateTestPackage("packageC", "3.0.0", repositoryPath);
                Util.CreateTestPackage("packageC", "2.1.0", repositoryPath);
                var projectJson = @"{
                                                    'dependencies': {
                                                    'packageA': '1.0.0',
                                                    'packageB': '1.0.0'
                                                    },
                                                    'frameworks': {
                                                            'uap10.0': { }
                                                        }
                                                }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectPath
                };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var lockFilePath = Path.Combine(workingPath, "project.lock.json");
                var lockFileFormat = new LockFileFormat();

                var lockFile = lockFileFormat.Read(lockFilePath);

                var installedC = lockFile.Targets.First().Libraries.Single(package => package.Name == "packageC");

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal("2.1.0", installedC.Version.ToNormalizedString());
            }
        }

        [Fact(Skip = "https://github.com/NuGet/Home/issues/1330")]
        public void RestoreProjectJson_RestoreDowngradesFromStableToPrereleaseWhenNeeded()
        {
            // Arrange
            using (var workingPath = TestDirectory.Create())
            {
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateTestPackage("packageA", "1.0.0", repositoryPath, "win8", "packageC", "1.0.0");
                Util.CreateTestPackage("packageB", "1.0.0-beta", repositoryPath, "win8", "packageC", "[2.0.0-beta]");
                Util.CreateTestPackage("packageC", "3.0.0", repositoryPath);
                Util.CreateTestPackage("packageC", "2.0.0-beta", repositoryPath);
                Util.CreateConfigForGlobalPackagesFolder(workingPath);
                var projectJson = @"{
                                                    'dependencies': {
                                                    'packageA': '1.0.0',
                                                    'packageB': '1.0.0-*'
                                                    },
                                                    'frameworks': {
                                                            'uap10.0': { }
                                                        }
                                                  }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                        "restore",
                        "-Source",
                        repositoryPath,
                        "-solutionDir",
                        workingPath,
                        projectPath
                    };

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var lockFilePath = Path.Combine(workingPath, "project.lock.json");
                var lockFileFormat = new LockFileFormat();

                var lockFile = lockFileFormat.Read(lockFilePath);

                var installedC = lockFile.Targets.First().Libraries.Single(package => package.Name == "packageC");

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal("2.0.0-beta", installedC.Version.ToNormalizedString());
            }
        }

        [Fact]
        public async Task RestoreProjectJson_SolutionFileWithAllProjectsInOneFolder()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir = Path.Combine(workingPath, "abc");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));

                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageA.AddFile("build/uap/packageA.targets", targetContent);
                packageA.AddFile("lib/uap/a.dll", "a");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA);

                Util.CreateFile(projectDir, "testA.project.json",
                                                @"{
                                            'dependencies': {
                                            'packageA': '1.1.0-beta-*'
                                            },
                                            'frameworks': {
                                                        'uap10.0': { }
                                                    }
                                            }");

                Util.CreateFile(projectDir, "testB.project.json",
                                                @"{
                                                    'dependencies': {
                                                    'packageA': '1.1.0-beta-*'
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                 }");

                Util.CreateFile(projectDir, "packages.testC.config",
                          @"<packages>
                                <package id=""packageA"" version=""1.1.0-beta-01"" targetFramework=""net45"" />
                            </packages>");

                Util.CreateFile(projectDir, "testA.csproj", Util.GetCSProjXML("testA")); // testA.project.json
                Util.CreateFile(projectDir, "testB.csproj", Util.GetCSProjXML("testB")); // testB.project.json
                Util.CreateFile(projectDir, "testC.csproj", Util.GetCSProjXML("testC")); // packages.testC.config
                Util.CreateFile(projectDir, "testD.csproj", Util.GetCSProjXML("testD")); // Non-nuget

                var slnPath = Path.Combine(workingPath, "xyz.sln");

                Util.CreateFile(workingPath, "xyz.sln",
                        @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{AAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""testA"", ""abc\testA.csproj"", ""{6A6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{ABE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""testB"", ""abc\testB.csproj"", ""{6A6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{ACE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""testC"", ""abc\testC.csproj"", ""{6A6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Project(""{ADE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""testD"", ""abc\testD.csproj"", ""{6A6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {6A6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {6A6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {6A6279C1-B5EE-4C6B-9FA3-A794CE195136}.Release|Any CPU.ActiveCfg = Release|Any CPU
                                {6A6279C1-B5EE-4C6B-9FA3-A794CE195136}.Release|Any CPU.Build.0 = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                        ");

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    slnPath
                };

                var targetFileA = Path.Combine(projectDir, "testA.nuget.targets");
                var targetFileB = Path.Combine(projectDir, "testB.nuget.targets");
                var lockFileA = Path.Combine(projectDir, "testA.project.lock.json");
                var lockFileB = Path.Combine(projectDir, "testB.project.lock.json");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFileA));
                Assert.True(File.Exists(targetFileB));
                Assert.True(File.Exists(lockFileA));
                Assert.True(File.Exists(lockFileB));
                Assert.True(File.Exists(Path.Combine(workingPath, "globalPackages", "packageA", "1.1.0-beta-01", "packageA.1.1.0-beta-01.nupkg")));
            }
        }

        [Fact]
        public async Task RestoreProjectJson_GenerateFilesWithProjectNameFromCSProj()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                packageA.AddFile("build/uap/packageA.targets", targetContent);
                packageA.AddFile("lib/uap/a.dll", "a");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA);

                Util.CreateFile(workingPath, "test.project.json",
                                                @"{
                                                        'dependencies': {
                                                        'packageA': '1.1.0-beta-*'
                                                        },
                                                        'frameworks': {
                                                                    'uap10.0': { }
                                                        }
                                                   }");

                Util.CreateFile(workingPath, "test.csproj", Util.GetCSProjXML("test"));

                var csprojPath = Path.Combine(workingPath, "test.csproj");

                var args = new string[] {
                "restore",
                "-Source",
                repositoryPath,
                "-solutionDir",
                workingPath,
                csprojPath
            };

                var targetFilePath = Path.Combine(workingPath, "test.nuget.targets");
                var lockFilePath = Path.Combine(workingPath, $"test.project.lock.json");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(lockFilePath));
                Assert.True(File.Exists(targetFilePath));
            }
        }

        [Fact]
        public async Task RestoreProjectJson_GenerateTargetsFileFromSln()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var nugetexe = Util.GetNuGetExePath();

                var repositoryPath = Path.Combine(workingPath, "Repository");
                var projectDir = Path.Combine(workingPath, "abc");

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));

                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageA.AddFile("build/uap/packageA.targets", targetContent);
                packageA.AddFile("lib/uap/a.dll", "a");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA);

                Util.CreateFile(projectDir, "project.json",
                                                @"{
                                            'dependencies': {
                                            'packageA': '1.1.0-beta-*'
                                            },
                                            'frameworks': {
                                                        'uap10.0': { }
                                                    }
                                            }");

                Util.CreateFile(projectDir, "test.csproj", Util.GetCSProjXML("test"));

                var slnPath = Path.Combine(workingPath, "xyz.sln");

                Util.CreateFile(workingPath, "xyz.sln",
                           @"
                        Microsoft Visual Studio Solution File, Format Version 12.00
                        # Visual Studio 14
                        VisualStudioVersion = 14.0.23107.0
                        MinimumVisualStudioVersion = 10.0.40219.1
                        Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""test"", ""abc\test.csproj"", ""{6A6279C1-B5EE-4C6B-9FA3-A794CE195136}""
                        EndProject
                        Global
                            GlobalSection(SolutionConfigurationPlatforms) = preSolution
                                Debug|Any CPU = Debug|Any CPU
                                Release|Any CPU = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(ProjectConfigurationPlatforms) = postSolution
                                {6A6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                                {6A6279C1-B5EE-4C6B-9FA3-A794CE195136}.Debug|Any CPU.Build.0 = Debug|Any CPU
                                {6A6279C1-B5EE-4C6B-9FA3-A794CE195136}.Release|Any CPU.ActiveCfg = Release|Any CPU
                                {6A6279C1-B5EE-4C6B-9FA3-A794CE195136}.Release|Any CPU.Build.0 = Release|Any CPU
                            EndGlobalSection
                            GlobalSection(SolutionProperties) = preSolution
                                HideSolutionNode = FALSE
                            EndGlobalSection
                        EndGlobal
                        ");

                var csprojPath = Path.Combine(projectDir, "test.csproj");

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    slnPath
                };

                var targetFilePath = Path.Combine(projectDir, "test.nuget.targets");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                var targetsFile = File.ReadAllText(targetFilePath);
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageA.targets")) > -1);
            }
        }

        [Fact]
        public async Task RestoreProjectJson_GenerateTargetsFileFromCSProj()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));

                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageA.AddFile("build/uap/packageA.targets", targetContent);
                packageA.AddFile("lib/uap/a.dll", "a");
                var packageB = new SimpleTestPackageContext("packageB", "2.2.0-beta-02");
                targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageB.AddFile("build/uap/packageB.targets", targetContent);
                packageB.AddFile("lib/uap/b.dll", "b");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA, packageB);


                Util.CreateFile(workingPath, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    'packageA': '1.1.0-beta-*',
                                                    'packageB': '2.2.0-beta-*'
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                }");

                Util.CreateFile(workingPath, "test.csproj", Util.GetCSProjXML("test"));

                var csprojPath = Path.Combine(workingPath, "test.csproj");

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    csprojPath
                };

                var targetFilePath = Path.Combine(workingPath, "test.nuget.targets");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                var targetsFile = File.ReadAllText(targetFilePath);
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageA.targets")) > -1);
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageB.targets")) > -1);
            }
        }

        [Fact]
        public async Task RestoreProjectJson_GenerateTargetsForFallbackFolderAsync()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var globalPath = Path.Combine(workingPath, "global");
                var fallback1 = Path.Combine(workingPath, "fallback1");
                var fallback2 = Path.Combine(workingPath, "fallback2");
                var projectDir = Path.Combine(workingPath, "project");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(globalPath);
                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(fallback1);
                Directory.CreateDirectory(fallback2);

                var config = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration>
    <config>
        <add key=""globalPackagesFolder"" value=""{globalPath}"" />
    </config>
    <fallbackPackageFolders>
        <clear />
        <add key=""a"" value=""{fallback1}"" />
        <add key=""b"" value=""{fallback2}"" />
    </fallbackPackageFolders>
    <packageSources>
        <clear />
        <add key=""a"" value=""{repositoryPath}"" />
    </packageSources>
    <packageSourceMapping>
      <clear />
    </packageSourceMapping>
</configuration>";

                File.WriteAllText(Path.Combine(workingPath, "NuGet.Config"), config);

                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageA.AddFile("build/uap/packageA.targets", targetContent);
                packageA.AddFile("lib/uap/a.dll", "a");
                var packageB = new SimpleTestPackageContext("packageB", "2.2.0-beta-02");
                targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageB.AddFile("build/uap/packageB.targets", targetContent);
                packageB.AddFile("lib/uap/b.dll", "b");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA);
                var saveMode = PackageSaveMode.Defaultv3;
                await SimpleTestPackageUtility.CreateFolderFeedV3Async(fallback2, saveMode, Directory.GetFiles(repositoryPath));
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageB);

                Util.CreateFile(projectDir, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    'packageA': '1.1.0-beta-*',
                                                    'packageB': '2.2.0-beta-*'
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                }");

                Util.CreateFile(projectDir, "test.csproj", Util.GetCSProjXML("test"));

                var csprojPath = Path.Combine(projectDir, "test.csproj");

                var args = new string[] {
                    "restore",
                    csprojPath
                };

                var targetFilePath = Path.Combine(projectDir, "test.nuget.targets");

                // A comes from the fallback folder
                var packageAPath = Path.Combine("fallback2", "packagea", "1.1.0-beta-01", "build", "uap", "packageA.targets");

                // B is installed to the user folder
                var packageBPath = "$(NuGetPackageRoot)"
                    + Path.DirectorySeparatorChar
                    + Path.Combine("packageb", "2.2.0-beta-02", "build", "uap", "packageB.targets");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                var targetsFile = File.ReadAllText(targetFilePath);
                Assert.True(targetsFile.IndexOf(packageAPath) > -1);
                Assert.True(targetsFile.IndexOf(packageBPath) > -1);
            }
        }

        [Fact]
        public async Task RestoreProjectJson_GenerateTargetsFileFromNuProj()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                var workingPath = pathContext.WorkingDirectory;
                var repositoryPath = pathContext.PackageSource;
                var nugetexe = Util.GetNuGetExePath();

                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageA.AddFile("build/uap/packageA.targets", targetContent);
                packageA.AddFile("lib/uap/a.dll", "a");
                var packageB = new SimpleTestPackageContext("packageB", "2.2.0-beta-02");
                targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageB.AddFile("build/uap/packageB.targets", targetContent);
                packageB.AddFile("lib/uap/b.dll", "b");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA, packageB);

                Util.CreateFile(workingPath, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    'packageA': '1.1.0-beta-*',
                                                    'packageB': '2.2.0-beta-*'
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                }");

                Util.CreateFile(workingPath, "test.nuproj", Util.GetCSProjXML("test"));

                var nuProjPath = Path.Combine(workingPath, "test.nuproj");

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    nuProjPath
                };

                var targetFilePath = Path.Combine(workingPath, "test.nuget.targets");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                var targetsFile = File.ReadAllText(targetFilePath);
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageA.targets")) > -1);
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageB.targets")) > -1);
            }
        }

        [Fact]
        public async Task RestoreProjectJson_GenerateTargetsFileWithFolder()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var folderName = Path.GetFileName(workingPath);

                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageA.AddFile("build/uap/packageA.targets", targetContent);
                packageA.AddFile("lib/uap/a.dll", "a");
                var packageB = new SimpleTestPackageContext("packageB", "2.2.0-beta-02");
                packageB.AddFile("build/uap/packageB.targets", targetContent);
                packageB.AddFile("lib/uap/b.dll", "b");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA, packageB);

                Util.CreateFile(workingPath, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    'packageA': '1.1.0-beta-*',
                                                    'packageB': '2.2.0-beta-*'
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                  }");

                Util.CreateFile(workingPath, "test.csproj", Util.GetCSProjXML("test"));

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    "test.csproj"
                };

                var targetFilePath = Path.Combine(workingPath, "test.nuget.targets");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                var targetsFile = File.ReadAllText(targetFilePath);
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageA.targets")) > -1);
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageB.targets")) > -1);
            }
        }

        [Fact]
        public async Task RestoreProjectJson_GenerateTargetsForRootBuildFolderIgnoreSubFolders()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var folderName = Path.GetFileName(workingPath);

                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));

                var packageA = new SimpleTestPackageContext("packageA", "3.1.0");
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageA.AddFile("build/net45/packageA.targets", targetContent);
                packageA.AddFile("build/packageA.targets", targetContent);
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA);


                Util.CreateFile(workingPath, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    'packageA': '3.1.0',
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                  }
                                               }");

                Util.CreateFile(workingPath, "test.csproj", Util.GetCSProjXML("test"));


                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    "test.csproj"
                };

                var targetFilePath = Path.Combine(workingPath, "test.nuget.targets");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                var targetsFile = File.ReadAllText(targetFilePath);
                // Verify the target was added
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "packageA.targets")) > -1);

                // Verify sub directories were not used
                Assert.True(targetsFile.IndexOf(Path.Combine("build", "net45", "packageA.targets")) < 0);
            }
        }

        [Fact]
        public async Task RestoreProjectJson_GenerateTargetsPersistsWithMultipleRestores()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var folderName = Path.GetFileName(workingPath);

                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                var packageA = new SimpleTestPackageContext("packageA", "1.1.0-beta-01");
                var targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageA.AddFile("build/uap/packageA.targets", targetContent);
                packageA.AddFile("lib/uap/a.dll", "a");
                var packageB = new SimpleTestPackageContext("packageB", "2.2.0-beta-02");
                targetContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Project ToolsVersion=\"12.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"></Project>";
                packageB.AddFile("build/uap/packageB.targets", targetContent);
                packageB.AddFile("lib/uap/b.dll", "b");
                await SimpleTestPackageUtility.CreatePackagesAsync(repositoryPath, packageA, packageB);

                Util.CreateFile(workingPath, "project.json",
                                                @"{
                                                    'dependencies': {
                                                    'packageA': '1.1.0-beta-*',
                                                    'packageB': '2.2.0-beta-*'
                                                    },
                                                    'frameworks': {
                                                                'uap10.0': { }
                                                            }
                                                }");

                Util.CreateFile(workingPath, "test.csproj", Util.GetCSProjXML("test"));

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    "test.csproj"
                };

                var targetFilePath = Path.Combine(workingPath, "test.nuget.targets");

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                using (var stream = File.OpenText(targetFilePath))
                {
                    var targetsFile = stream.ReadToEnd();
                    Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageA.targets")) > -1);
                    Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageB.targets")) > -1);
                }

                // Act 2
                r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert 2
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                using (var stream = File.OpenText(targetFilePath))
                {
                    var targetsFile = stream.ReadToEnd();
                    Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageA.targets")) > -1);
                    Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageB.targets")) > -1);
                }

                // Act 3
                r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                // Assert 3
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.True(File.Exists(targetFilePath));

                using (var stream = File.OpenText(targetFilePath))
                {
                    var targetsFile = stream.ReadToEnd();
                    Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageA.targets")) > -1);
                    Assert.True(targetsFile.IndexOf(Path.Combine("build", "uap", "packageB.targets")) > -1);
                }
            }
        }

        [Fact]
        public void RestoreProjectJson_CorruptedLockFile()
        {
            // Arrange
            using (var pathContext = new SimpleTestPathContext())
            {
                string workingPath = pathContext.WorkingDirectory;
                var repositoryPath = Path.Combine(workingPath, "Repository");
                var nugetexe = Util.GetNuGetExePath();

                Directory.CreateDirectory(repositoryPath);
                Directory.CreateDirectory(Path.Combine(workingPath, ".nuget"));
                Util.CreateTestPackage("packageA", "1.1.0", repositoryPath);
                Util.CreateTestPackage("packageB", "2.2.0", repositoryPath);
                var projectJson = @"{
                                        'dependencies': {
                                        'packageA': '1.1.0',
                                        'packageB': '2.2.0'
                                        },
                                        'frameworks': {
                                                    'uap10.0': { }
                                                }
                                        }";

                var projectPath = Util.CreateUAPProject(workingPath, projectJson);

                var args = new string[] {
                    "restore",
                    "-Source",
                    repositoryPath,
                    "-solutionDir",
                    workingPath,
                    projectPath
                };

                var lockFilePath = Path.Combine(workingPath, "project.lock.json");
                var lockFileFormat = new LockFileFormat();
                using (var writer = new StreamWriter(lockFilePath))
                {
                    writer.WriteLine("{ \"CORRUPTED!\": \"yep\"");
                }

                // Act
                var r = CommandRunner.Run(
                    nugetexe,
                    workingPath,
                    string.Join(" ", args));

                var lockFile = lockFileFormat.Read(lockFilePath);

                // Assert
                // If the library count can be obtained then a new lock file was created
                Assert.True(0 == r.ExitCode, r.Output + " " + r.Errors);
                Assert.Equal(2, lockFile.Libraries.Count);
            }
        }
    }
}
