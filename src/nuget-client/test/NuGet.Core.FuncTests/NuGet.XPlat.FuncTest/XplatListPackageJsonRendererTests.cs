// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NuGet.CommandLine.XPlat;
using NuGet.CommandLine.XPlat.ListPackage;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Configuration.Test;
using NuGet.Protocol;
using NuGet.Test.Utility;
using NuGet.Versioning;
using Xunit;

namespace NuGet.XPlat.FuncTest
{
    [Collection("NuGet XPlat Test Collection")]
    public class XplatListPackageJsonRendererTests
    {
        [Fact]
        public void JsonRenderer_ListPackage_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Default;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork5 = "net5.0";
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");
                var projectBPath = Path.Combine(pathContext.SolutionRoot, "projectB.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: false,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "A",
                                            requestedVersion : "2.0.0",
                                            resolvedVersion : "2.0.0")
                                    },
                                    // Below transitive packages shouldn't be in json output because this report doesn't have --include-transive option.
                                    TransitivePackages = new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "C",
                                            requestedVersion : "2.0.0",
                                            resolvedVersion : "3.1.0",
                                            autoReference : true)
                                    }
                                }
                            },
                            projectProblems: null
                        ),
                        (
                            projectBPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "B",
                                            requestedVersion : "3.0.0",
                                            resolvedVersion : "3.1.0")
                                    }
                                },
                                new ListPackageReportFrameworkPackage(frameWork5)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "B",
                                            requestedVersion : "3.0.0",
                                            resolvedVersion : "3.1.0")
                                    }
                                }
                          },
                          projectProblems: null
                      )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                // Below one doesn't include any transitive packages.
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                  'version': 1,
                  'parameters': '',
                  'projects': [
                    {{
                      'path': '{projectAPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                            {{
                              'id': 'A',
                              'requestedVersion': '2.0.0',
                              'resolvedVersion': '2.0.0'
                            }}
                          ]
                        }}
                      ]
                    }},
                    {{
                      'path': '{projectBPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                            {{
                              'id': 'B',
                              'requestedVersion': '3.0.0',
                              'resolvedVersion': '3.1.0'
                            }}
                          ]
                        }},
                        {{
                          'framework': 'net5.0',
                          'topLevelPackages': [
                            {{
                              'id': 'B',
                              'requestedVersion': '3.0.0',
                              'resolvedVersion': '3.1.0'
                            }}
                          ]
                        }}
                      ]
                    }}
                  ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_PackageWithAutoReference_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Default;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: false,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "A",
                                            requestedVersion : "2.0.0",
                                            resolvedVersion : "2.0.0",
                                            autoReference : true  // this one should be detected.
                                        ),
                                        new ListReportPackage(
                                            packageId : "B",
                                            requestedVersion : "1.0.0",
                                            resolvedVersion : "1.3.0"
                                        )
                                    }
                                }
                           },
                           projectProblems: null
                       )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                // autoReferenced is set to true
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                  'version': 1,
                  'parameters': '',
                  'projects': [
                    {{
                      'path': '{projectAPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                            {{
                              'id': 'A',
                              'requestedVersion': '2.0.0',
                              'resolvedVersion': '2.0.0',
                              'autoReferenced': 'true'
                            }},
                            {{
                              'id': 'B',
                              'requestedVersion': '1.0.0',
                              'resolvedVersion': '1.3.0'
                            }}
                          ]
                        }}
                      ]
                    }}
                  ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_Outdated_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Outdated;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: false,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "A",
                                            requestedVersion : "[1.0.0,1.3.0]",
                                            resolvedVersion : "1.0.0",
                                            latestVersion : "2.0.0")
                                    }
                                }
                            },
                            projectProblems: null
                      )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                  'version': 1,
                  'parameters': '--outdated',
                  'sources': [
                    '{pathContext.PackageSource}'
                  ],
                  'projects': [
                    {{
                      'path': '{projectAPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                            {{
                              'id': 'A',
                              'requestedVersion': '[1.0.0,1.3.0]',
                              'resolvedVersion': '1.0.0',
                              'latestVersion': '2.0.0'
                            }}
                          ]
                        }}
                      ]
                    }}
                  ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_Deprecated_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Deprecated;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: false,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "A",
                                            requestedVersion : "[1.0.0,1.3.0]",
                                            resolvedVersion : "1.0.0",
                                            deprecationReasons : new PackageDeprecationMetadata
                                            {
                                                Reasons = new List<string>() { "Other", "Legacy"}.AsEnumerable()
                                            },
                                            alternativePackage : new AlternatePackageMetadata()
                                            {
                                                PackageId = "betterPackage",
                                                Range = VersionRange.Parse("[*,)")
                                            })
                                    }
                                }
                            },
                            projectProblems: null
                      )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                  'version': 1,
                  'parameters': '--deprecated',
                  'sources': [
                    '{pathContext.PackageSource}'
                  ],
                  'projects': [
                    {{
                      'path': '{projectAPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                            {{
                              'id': 'A',
                              'requestedVersion': '[1.0.0,1.3.0]',
                              'resolvedVersion': '1.0.0',
                              'deprecationReasons': [
                                'Other','Legacy'
                              ],
                              'alternativePackage': {{
                                'id': 'betterPackage',
                                'versionRange': '>= 0.0.0'
                              }}
                            }}
                          ]
                        }}
                      ]
                    }}
                  ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_Vulnerable_WithVulnerability_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Vulnerable;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: false,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "A",
                                            requestedVersion : "[1.0.0,1.3.0]",
                                            resolvedVersion : "1.0.0",
                                            latestVersion : null,
                                            vulnerabilities : new List<PackageVulnerabilityMetadata>
                                            {
                                                new PackageVulnerabilityMetadata()
                                                {
                                                    Severity = 2,
                                                    AdvisoryUrl = new Uri("https://github.com/advisories/GHSA-g8j6-m4p7-5rfq")
                                                },
                                                new PackageVulnerabilityMetadata()
                                                {
                                                    Severity = 1,
                                                    AdvisoryUrl = new Uri("https://github.com/advisories/GHSA-v76m-f5cx-8rg4")
                                                }
                                            })
                                    }
                                }
                            },
                            projectProblems: null
                       )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                  'version': 1,
                  'parameters': '--vulnerable',
                  'sources': [
                    '{pathContext.PackageSource}'
                  ],
                  'projects': [
                    {{
                      'path': '{projectAPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                            {{
                              'id': 'A',
                              'requestedVersion': '[1.0.0,1.3.0]',
                              'resolvedVersion': '1.0.0',
                              'vulnerabilities': [
                                {{
                                  'severity': 'High',
                                  'advisoryurl': 'https://github.com/advisories/GHSA-g8j6-m4p7-5rfq'
                                }},
                                {{
                                  'severity': 'Moderate',
                                  'advisoryurl': 'https://github.com/advisories/GHSA-v76m-f5cx-8rg4'
                                }}
	                          ]
                            }}
                          ]
                        }}
                      ]
                    }}
                  ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_Vulnerable_WithoutVulnerability_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Vulnerable;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: false,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    { }
                                }
                            },
                            new List<ReportProblem>() { }
                       )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                  'version': 1,
                  'parameters': '--vulnerable',
                  'sources': [
                    '{pathContext.PackageSource}'
                  ],
                  'projects': [
                    {{
                      'path': '{projectAPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                          ]
                        }}
                      ]
                    }}
                  ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_IncludeTransitives_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Default;
            var includeTransitive = true;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork5 = "net5.0";
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");
                var projectBPath = Path.Combine(pathContext.SolutionRoot, "projectB.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: includeTransitive,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "A",
                                            requestedVersion : "2.0.0",
                                            resolvedVersion : "2.0.0")
                                    },
                                    // Below transitive packages should be in json output because this report has --include-transive option.
                                    TransitivePackages = new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "C",
                                            requestedVersion : "2.0.0",  // This is ignored for Transitive packages
                                            resolvedVersion : "3.1.0",
                                            autoReference : true  // This is ignored for Transitive packages
                                        )
                                    }
                                }
                            },
                            projectProblems: null
                         ),
                         (
                            projectBPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "B",
                                            requestedVersion : "3.0.0",
                                            resolvedVersion : "3.1.0")
                                    }
                                },
                                new ListPackageReportFrameworkPackage(frameWork5)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "B",
                                            requestedVersion : "3.0.0",
                                            resolvedVersion : "3.1.0")
                                    },
                                    TransitivePackages = new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "D",
                                            requestedVersion : "1.0.0",  // This is ignored for Transitive packages
                                            resolvedVersion : "1.1.0",
                                            autoReference : true  // This is ignored for Transitive packages
                                        )
                                    }
                                }
                            },
                            projectProblems: null
                        )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                // Below one doesn't include any transitive packages.
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                    'version': 1,
                    'parameters': '--include-transitive',
                    'projects': [
                    {{
                        'path': '{projectAPath}',
                        'frameworks': [
                        {{
                            'framework': 'netcoreapp3.1',
                            'topLevelPackages': [
                            {{
                                'id': 'A',
                                'requestedVersion': '2.0.0',
                                'resolvedVersion': '2.0.0'
                            }}
                            ],
                            'transitivePackages': [
                            {{
                                'id': 'C',
                                'resolvedVersion': '3.1.0'
                            }}
                            ]
                        }}
                        ]
                    }},
                    {{
                        'path': '{projectBPath}',
                        'frameworks': [
                        {{
                            'framework': 'netcoreapp3.1',
                            'topLevelPackages': [
                            {{
                                'id': 'B',
                                'requestedVersion': '3.0.0',
                                'resolvedVersion': '3.1.0'
                            }}
                            ]
                        }},
                        {{
                            'framework': 'net5.0',
                            'topLevelPackages': [
                            {{
                                'id': 'B',
                                'requestedVersion': '3.0.0',
                                'resolvedVersion': '3.1.0'
                            }}
                            ],
                            'transitivePackages': [
                            {{
                                'id': 'D',
                                'resolvedVersion': '1.1.0'
                            }}
                            ]
                        }}
                        ]
                    }}
                    ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_Outdated_IncludeTransitive_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Outdated;
            var includeTransitive = true;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork31 = "netcoreapp3.1";
                string frameWork5 = "net5.0";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: includeTransitive,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "A",
                                            requestedVersion : "[1.0.0,1.3.0]",
                                            resolvedVersion : "1.0.0",
                                            latestVersion : "2.0.0")
                                    }
                                },
                                new ListPackageReportFrameworkPackage(frameWork5)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "B",
                                            requestedVersion : "[1.0.0,1.3.0]",
                                            resolvedVersion : "1.0.0",
                                            latestVersion : "2.0.0")
                                    },
                                    TransitivePackages = new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "D",
                                            requestedVersion : "1.0.0",  // This is ignored for Transitive packages
                                            resolvedVersion : "1.1.0",
                                            latestVersion : "3.1.0",
                                            autoReference : true  // This is ignored for Transitive packages
                                            )
                                    }
                                }
                            },
                            projectProblems: null
                         )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                // Transitive packages have `latestVersion` property.
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                  'version': 1,
                  'parameters': '--outdated --include-transitive',
                  'sources': [
                    '{pathContext.PackageSource}'
                  ],
                  'projects': [
                    {{
                      'path': '{projectAPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                            {{
                              'id': 'A',
                              'requestedVersion': '[1.0.0,1.3.0]',
                              'resolvedVersion': '1.0.0',
                              'latestVersion': '2.0.0'
                            }}
                          ]
                        }},
                        {{
                          'framework': 'net5.0',
                          'topLevelPackages': [
                            {{
                              'id': 'B',
                              'requestedVersion': '[1.0.0,1.3.0]',
                              'resolvedVersion': '1.0.0',
                              'latestVersion': '2.0.0'
                            }}
                          ],
                          'transitivePackages': [
                            {{
                              'id': 'D',
                              'resolvedVersion': '1.1.0',
                              'latestVersion': '3.1.0'
                            }}
                          ]
                        }}
                      ]
                    }}
                  ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_Vulnerable_IncludeTransitive_SucceedsAsync()
        {
            // Arrange
            var reportType = ReportType.Vulnerable;
            var includeTransitive = true;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: includeTransitive,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId: "A",
                                            requestedVersion: "[1.0.0,1.3.0]",
                                            resolvedVersion : "1.0.0",
                                            latestVersion: null,
                                            vulnerabilities: new List<PackageVulnerabilityMetadata>
                                            {
                                                new PackageVulnerabilityMetadata()
                                                {
                                                    Severity = 2,
                                                    AdvisoryUrl = new Uri("https://github.com/advisories/GHSA-g8j6-m4p7-5rfq")
                                                },
                                                new PackageVulnerabilityMetadata()
                                                {
                                                    Severity = 1,
                                                    AdvisoryUrl = new Uri("https://github.com/advisories/GHSA-v76m-f5cx-8rg4")
                                                }
                                            }
                                        )
                                    },
                                    TransitivePackages = new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId: "D",
                                            requestedVersion: null,
                                            resolvedVersion: "1.1.0",
                                            latestVersion: "3.1.0",
                                            vulnerabilities : new List<PackageVulnerabilityMetadata>
                                            {
                                                new PackageVulnerabilityMetadata()
                                                {
                                                    Severity = 3,
                                                    AdvisoryUrl = new Uri("https://github.com/advisories/GHSA-5c66-x4wm-rjfx")
                                                }
                                            })
                                    }
                                }
                            },
                            projectProblems: null
                         )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                // Vulnerabilities in transitive dependencies are detected.
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                {{
                  'version': 1,
                  'parameters': '--vulnerable --include-transitive',
                  'sources': [
                    '{pathContext.PackageSource}'
                  ],
                  'projects': [
                    {{
                      'path': '{projectAPath}',
                      'frameworks': [
                        {{
                          'framework': 'netcoreapp3.1',
                          'topLevelPackages': [
                            {{
                              'id': 'A',
                              'requestedVersion': '[1.0.0,1.3.0]',
                              'resolvedVersion': '1.0.0',
                              'vulnerabilities': [
                                {{
                                  'severity': 'High',
                                  'advisoryurl': 'https://github.com/advisories/GHSA-g8j6-m4p7-5rfq'
                                }},
                                {{
                                  'severity': 'Moderate',
                                  'advisoryurl': 'https://github.com/advisories/GHSA-v76m-f5cx-8rg4'
                                }}
                              ]
                            }}
                          ],
                          'transitivePackages': [
                            {{
                              'id': 'D',
                              'resolvedVersion': '1.1.0',
                              'vulnerabilities': [
                                {{
                                  'severity': 'Critical',
                                  'advisoryurl': 'https://github.com/advisories/GHSA-5c66-x4wm-rjfx'
                                }}
                              ]
                            }}
                          ]
                        }}
                      ]
                    }}
                  ]
                }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        [Fact]
        public void JsonRenderer_ListPackage_NoAssetFile_FailsAsync()
        {
            // Arrange
            var reportType = ReportType.Default;
            using (var pathContext = new SimpleTestPathContext())
            {
                string consoleOutputFileName = Path.Combine(pathContext.SolutionRoot, "consoleOutput.txt");
                string frameWork31 = "netcoreapp3.1";
                var projectAPath = Path.Combine(pathContext.SolutionRoot, "projectA.csproj");
                var projectBPath = Path.Combine(pathContext.SolutionRoot, "projectB.csproj");

                using (FileStream stream = new FileStream(consoleOutputFileName, FileMode.Create))
                {
                    using StreamWriter writer = new StreamWriter(stream);
                    writer.AutoFlush = true;

                    ListPackageJsonRenderer jsonRenderer = new ListPackageJsonRenderer(writer);
                    var packageRefArgs = new ListPackageArgs(
                                path: pathContext.SolutionRoot,
                                packageSources: new List<PackageSource>() { new PackageSource(pathContext.PackageSource) },
                                frameworks: new List<string>() { },
                                reportType: reportType,
                                renderer: jsonRenderer,
                                includeTransitive: false,
                                prerelease: false,
                                highestPatch: false,
                                highestMinor: false,
                                NullLogger.Instance,
                                CancellationToken.None);

                    ListPackageReportModel listPackageReportModel = CreateListReportModel(packageRefArgs,
                        (
                            projectAPath,
                            new List<ListPackageReportFrameworkPackage>()
                            {
                                new ListPackageReportFrameworkPackage(frameWork31)
                                {
                                    TopLevelPackages =  new List<ListReportPackage>()
                                    {
                                        new ListReportPackage(
                                            packageId : "A",
                                            requestedVersion : "2.0.0",
                                            resolvedVersion : "2.0.0")
                                    }
                                }
                            },
                            projectProblems: null
                        ),
                        (
                            projectBPath,
                            null,
                            new List<ReportProblem>() { new ReportProblem(ProblemType.Error, projectBPath, $"No assets file was found for `{projectBPath}`. Please run restore before running this command.") }
                        )
                    );

                    // Act
                    jsonRenderer.Render(listPackageReportModel);
                }

                // Assert
                // autoReferenced is set to true
                var expected = SettingsTestUtils.RemoveWhitespace($@"
                    {{
                      'version': 1,
                      'parameters': '',
                      'problems': [
                        {{
                          'project': '{projectBPath}',
                          'level': 'error',
                          'text': 'No assets file was found for `{projectBPath}`. Please run restore before running this command.'
                        }}
                      ],
                      'projects': [
                        {{
                          'path': '{projectAPath}',
                          'frameworks': [
                            {{
                              'framework': 'netcoreapp3.1',
                              'topLevelPackages': [
                                {{
                                  'id': 'A',
                                  'requestedVersion': '2.0.0',
                                  'resolvedVersion': '2.0.0'
                                }}
                              ]
                            }}
                          ]
                        }},
                        {{
                          'path': '{projectBPath}'
                        }}
                      ]
                    }}
                ".Replace("'", "\""));

                var actual = SettingsTestUtils.RemoveWhitespace(File.ReadAllText(consoleOutputFileName));
                actual.Should().Be(PathUtility.GetPathWithForwardSlashes(expected));
            }
        }

        internal ListPackageReportModel CreateListReportModel(ListPackageArgs packageRefArgs,
            params (string projectPath, List<ListPackageReportFrameworkPackage> projectPackages, List<ReportProblem> projectProblems)[] projects)

        {
            var listPackageReportModel = new ListPackageReportModel(packageRefArgs);
            foreach ((string projectPath, List<ListPackageReportFrameworkPackage> listPackageReportFrameworks, List<ReportProblem> projectProblems) project in projects)
            {
                var projectModel = new ListPackageProjectModel(project.projectPath);
                projectModel.TargetFrameworkPackages = project.listPackageReportFrameworks;

                if (project.projectProblems != null)
                {
                    foreach (var projectProblem in project.projectProblems)
                    {
                        projectModel.AddProjectInformation(projectProblem.ProblemType, projectProblem.Text);
                    }
                }

                listPackageReportModel.Projects.Add(projectModel);
            }
            return listPackageReportModel;
        }
    }
}
