// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using Xunit;

namespace Microsoft.DotNet.Build.Tasks.VisualStudio.UnitTests
{
    public class GetRunSettingsSessionConfigurationTests
    {
        private const string products_only_expectedContainerString = "  <TestContainer FileName=\"DDRIT.RPS.CSharp.dll\" />\r\n  <TestContainer FileName=\"VSPE.dll\" />";
        private const string products_only_expectedTestCaseFilterString = "FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner|FullyQualifiedName=VSPE.OptProfTests.vs_perf_designtime_ide_searchtest|FullyQualifiedName=VSPE.OptProfTests.vs_perf_designtime_editor_intellisense_globalcompletionlist_cs|FullyQualifiedName=VSPE.OptProfTests.vs_asl_cs_scenario|FullyQualifiedName=VSPE.OptProfTests.vs_ddbvtqa_vbwi|FullyQualifiedName=VSPE.OptProfTests.vs_asl_vb_scenario|FullyQualifiedName=VSPE.OptProfTests.vs_env_solution_createnewproject_vb_winformsapp|FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging";
        private const string products_only = @"
{
  ""products"": [
    {
      ""name"": ""Roslyn.VisualStudio.Setup.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner""
          ]
    },
        {
          ""container"": ""VSPE"",
          ""testCases"": [
            ""VSPE.OptProfTests.vs_perf_designtime_ide_searchtest"",
            ""VSPE.OptProfTests.vs_perf_designtime_editor_intellisense_globalcompletionlist_cs"",
            ""VSPE.OptProfTests.vs_asl_cs_scenario"",
            ""VSPE.OptProfTests.vs_ddbvtqa_vbwi"",
            ""VSPE.OptProfTests.vs_asl_vb_scenario"",
            ""VSPE.OptProfTests.vs_env_solution_createnewproject_vb_winformsapp""
          ]
}
      ]
    },
    {
      ""name"": ""ExpressionEvaluatorPackage.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging""
          ]
        }
      ]
    },
    {
      ""name"": ""Microsoft.CodeAnalysis.Compilers.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging""
          ]
        }
      ]
    },
    {
      ""name"": ""Roslyn.VisualStudio.InteractiveComponents.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner""
          ]
        }
      ]
    }
  ]
}
";

        private const string assemblies_only_expectedContainerString = "  <TestContainer FileName=\"DDRIT.RPS.CSharp.dll\" />";
        private const string assemblies_only_expectedTestCaseFilterString = "FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging";
        private const string assemblies_only = @"
{
  ""assemblies"" : [
    {
      ""assembly"": ""System.Collections.Immutable.dll"",
      ""instrumentationArguments"": [
        {
          ""relativeInstallationFolder"": ""Common7/IDE/PrivateAssemblies"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/15.0/Bin/Roslyn"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/Current/Bin"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/Current/Bin/amd64"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""Common7/IDE/Extensions/TestPlatform"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
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
    },
    {
      ""assembly"": ""System.Reflection.Metadata.dll"",
      ""instrumentationArguments"": [
        {
          ""relativeInstallationFolder"": ""Common7/IDE/PrivateAssemblies"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/15.0/Bin/Roslyn"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""Common7/IDE/Extensions/TestPlatform"",
          ""instrumentationExecutable"": ""Common7/IDE/vsn.exe""
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

        private const string products_and_assemblies_expectedContainerString = "  <TestContainer FileName=\"DDRIT.RPS.CSharp.dll\" />\r\n  <TestContainer FileName=\"VSPE.dll\" />";
        private const string products_and_assemblies_expectedTestCaseFilterString = "FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner|FullyQualifiedName=VSPE.OptProfTests.vs_perf_designtime_ide_searchtest|FullyQualifiedName=VSPE.OptProfTests.vs_perf_designtime_editor_intellisense_globalcompletionlist_cs|FullyQualifiedName=VSPE.OptProfTests.vs_asl_cs_scenario|FullyQualifiedName=VSPE.OptProfTests.vs_ddbvtqa_vbwi|FullyQualifiedName=VSPE.OptProfTests.vs_asl_vb_scenario|FullyQualifiedName=VSPE.OptProfTests.vs_env_solution_createnewproject_vb_winformsapp|FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging";
        private const string products_and_assemblies = @"
{
  ""products"": [
    {
      ""name"": ""Roslyn.VisualStudio.Setup.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner""
          ]
    },
        {
          ""container"": ""VSPE"",
          ""testCases"": [
            ""VSPE.OptProfTests.vs_perf_designtime_ide_searchtest"",
            ""VSPE.OptProfTests.vs_perf_designtime_editor_intellisense_globalcompletionlist_cs"",
            ""VSPE.OptProfTests.vs_asl_cs_scenario"",
            ""VSPE.OptProfTests.vs_ddbvtqa_vbwi"",
            ""VSPE.OptProfTests.vs_asl_vb_scenario"",
            ""VSPE.OptProfTests.vs_env_solution_createnewproject_vb_winformsapp""
          ]
}
      ]
    },
    {
      ""name"": ""ExpressionEvaluatorPackage.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging""
          ]
        }
      ]
    },
    {
      ""name"": ""Microsoft.CodeAnalysis.Compilers.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging""
          ]
        }
      ]
    },
    {
      ""name"": ""Roslyn.VisualStudio.InteractiveComponents.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner""
          ]
        }
      ]
    }
  ],
  ""assemblies"" : [
    {
      ""assembly"": ""System.Collections.Immutable.dll"",
      ""instrumentationArguments"": [
        {
          ""relativeInstallationFolder"": ""Common7/IDE/PrivateAssemblies"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/15.0/Bin/Roslyn"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/Current/Bin"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/Current/Bin/amd64"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""Common7/IDE/Extensions/TestPlatform"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
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
    },
    {
      ""assembly"": ""System.Reflection.Metadata.dll"",
      ""instrumentationArguments"": [
        {
          ""relativeInstallationFolder"": ""Common7/IDE/PrivateAssemblies"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/15.0/Bin/Roslyn"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""Common7/IDE/Extensions/TestPlatform"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
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

        private const string filtered_products_and_assemblies_expectedContainerString = "  <TestContainer FileName=\"DDRIT.RPS.CSharp.dll\" />\r\n  <TestContainer FileName=\"VSPE.dll\" />";
        private const string filtered_products_and_assemblies_expectedTestCaseFilterString = "FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner|FullyQualifiedName=VSPE.OptProfTests.vs_perf_designtime_ide_searchtest|FullyQualifiedName=VSPE.OptProfTests.vs_perf_designtime_editor_intellisense_globalcompletionlist_cs|FullyQualifiedName=VSPE.OptProfTests.vs_asl_cs_scenario|FullyQualifiedName=VSPE.OptProfTests.vs_ddbvtqa_vbwi|FullyQualifiedName=VSPE.OptProfTests.vs_asl_vb_scenario|FullyQualifiedName=VSPE.OptProfTests.vs_env_solution_createnewproject_vb_winformsapp|FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging";
        private const string filtered_products_and_assemblies = @"
{
  ""products"": [
    {
      ""name"": ""Roslyn.VisualStudio.Setup.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner""
          ]
        },
        {
          ""container"": ""VSPE"",
          ""filteredTestCases"": [
            {
              ""fileName"": ""Microsoft.CodeAnalysis.CSharp.dll"",
              ""testCases"": [
                ""VSPE.OptProfTests.vs_perf_designtime_ide_searchtest"",
                ""VSPE.OptProfTests.vs_perf_designtime_editor_intellisense_globalcompletionlist_cs"",
                ""VSPE.OptProfTests.vs_asl_cs_scenario"",
                ""VSPE.OptProfTests.vs_ddbvtqa_vbwi"",
                ""VSPE.OptProfTests.vs_asl_vb_scenario"",
                ""VSPE.OptProfTests.vs_env_solution_createnewproject_vb_winformsapp""
              ]
            }
          ]
        }
      ]
    },
    {
      ""name"": ""ExpressionEvaluatorPackage.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging""
          ]
        }
      ]
    },
    {
      ""name"": ""Microsoft.CodeAnalysis.Compilers.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging""
          ]
        }
      ]
    },
    {
      ""name"": ""Roslyn.VisualStudio.InteractiveComponents.vsix"",
      ""tests"": [
        {
          ""container"": ""DDRIT.RPS.CSharp"",
          ""testCases"": [
            ""DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner""
          ]
        }
      ]
    }
  ],
  ""assemblies"" : [
    {
      ""assembly"": ""System.Collections.Immutable.dll"",
      ""instrumentationArguments"": [
        {
          ""relativeInstallationFolder"": ""Common7/IDE/PrivateAssemblies"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/15.0/Bin/Roslyn"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/Current/Bin"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/Current/Bin/amd64"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""Common7/IDE/Extensions/TestPlatform"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
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
    },
    {
      ""assembly"": ""System.Reflection.Metadata.dll"",
      ""instrumentationArguments"": [
        {
          ""relativeInstallationFolder"": ""Common7/IDE/PrivateAssemblies"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""MSBuild/15.0/Bin/Roslyn"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
        },
        {
          ""relativeInstallationFolder"": ""Common7/IDE/Extensions/TestPlatform"",
          ""instrumentationExecutable"" : ""Common7/IDE/vsn.exe""
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

        [Theory]
        [InlineData(@"[{""BuildDrop"": ""https://vsdrop.corp.microsoft.com/file/v1/Products/42.42.42.42/42.42.42.42""}]", "Tests/42.42.42.42/42.42.42.42")]
        public static void TestsCorrectJsonFiles(string jsonString, string expectedUrl)
        {
            Assert.Equal(expectedUrl, GetRunSettingsSessionConfiguration.GetTestsDropName(jsonString));
        }

        [Theory]
        [InlineData("")]
        [InlineData(@"[{""BuildDrop"": ""https://vsdrop.corp.microsoft.com/file/v1/Tests/42.42.42.42/42.42.42.42""}]")]
        [InlineData(@"Products/42.42.42.42/42.42.42.42")]
        public static void TestsIncorrectJsonFiles(string jsonString)
        {
            Assert.Throws<InvalidDataException>(() => GetRunSettingsSessionConfiguration.GetTestsDropName(jsonString));
        }

        [Fact]
        public void Execute()
        {
            var temp = Path.GetTempPath();
            var dir = Path.Combine(temp, Guid.NewGuid().ToString());
            Directory.CreateDirectory(dir);

            var configPath = Path.Combine(dir, "OptProf.json");
            File.WriteAllText(configPath, products_only);

            var bootstrapperPath = Path.Combine(dir, "BootstrapperInfo.json");
            File.WriteAllText(bootstrapperPath, @"[{""BuildDrop"": ""https://vsdrop.corp.microsoft.com/file/v1/Products/42.42.42.42/42.42.42.42""}]");

            var task = new GetRunSettingsSessionConfiguration()
            {
                ConfigurationFile = configPath,
                ProductDropName = "Products/abc",
                BootstrapperInfoPath = bootstrapperPath
            };

            bool result = task.Execute();
            Assert.Equal(
$@"<TestStores>
  <TestStore Uri=""vstsdrop:ProfilingInputs/abc"" />
  <TestStore Uri=""vstsdrop:Tests/42.42.42.42/42.42.42.42"" />
</TestStores>
<TestContainers>
  <TestContainer FileName=""DDRIT.RPS.CSharp.dll"" />
  <TestContainer FileName=""VSPE.dll"" />
</TestContainers>
<TestCaseFilter>FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.EditingAndDesigner|FullyQualifiedName=VSPE.OptProfTests.vs_perf_designtime_ide_searchtest|FullyQualifiedName=VSPE.OptProfTests.vs_perf_designtime_editor_intellisense_globalcompletionlist_cs|FullyQualifiedName=VSPE.OptProfTests.vs_asl_cs_scenario|FullyQualifiedName=VSPE.OptProfTests.vs_ddbvtqa_vbwi|FullyQualifiedName=VSPE.OptProfTests.vs_asl_vb_scenario|FullyQualifiedName=VSPE.OptProfTests.vs_env_solution_createnewproject_vb_winformsapp|FullyQualifiedName=DDRIT.RPS.CSharp.CSharpTest.BuildAndDebugging</TestCaseFilter>", task.SessionConfiguration);

            Assert.True(result);

            Directory.Delete(dir, recursive: true);
        }

        [Theory]
        [InlineData(products_only, products_only_expectedContainerString, products_only_expectedTestCaseFilterString)]
        [InlineData(assemblies_only, assemblies_only_expectedContainerString, assemblies_only_expectedTestCaseFilterString)]
        [InlineData(products_and_assemblies, products_and_assemblies_expectedContainerString, products_and_assemblies_expectedTestCaseFilterString)]
        [InlineData(filtered_products_and_assemblies, filtered_products_and_assemblies_expectedContainerString, filtered_products_and_assemblies_expectedTestCaseFilterString)]
        public void TestProductsOnly(string configJson, string expectedContainerString, string expectedTestCaseFilterString)
        {
            var (actualContainerString, actualTestCaseFilterString) = GetRunSettingsSessionConfiguration.GetTestContainersAndFilters(configJson, "config.json");
            Assert.Equal(expectedContainerString, actualContainerString);
            Assert.Equal(expectedTestCaseFilterString, actualTestCaseFilterString);
        }
    }
}
