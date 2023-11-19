// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Build.Framework;
using Moq;
using Xunit;

namespace NuGet.Build.Tasks.Test
{
    public class GetReferenceNearestTargetFrameworkTaskTest
    {
        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_NoReferences()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "net46"
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().BeNullOrEmpty();

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }

        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_BadSourceTF()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "abadframework",
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeFalse();

            task.AssignedProjects.Should().BeNullOrEmpty();

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }

        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_NoCompatibleTargetTF()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns("net461");
            reference.Setup(e => e.GetMetadata("HasSingleTargetFramework")).Returns("true");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "net46",
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeFalse();

            task.AssignedProjects.Should().HaveCount(1);

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }

        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_NoTargetTF()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "net46",
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().HaveCount(1);

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }

        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_SingleTargetTF()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns("net46");
            reference.Setup(e => e.GetMetadata("HasSingleTargetFramework")).Returns("true");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "net46",
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().HaveCount(1);
            task.AssignedProjects[0].GetMetadata("NearestTargetFramework").Should().Be("net46");

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }

        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_MultipleTargetTF()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns("net20;netstandard20");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "net46",
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().HaveCount(1);
            task.AssignedProjects[0].GetMetadata("NearestTargetFramework").Should().Be("net20");

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }

        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_BadTargetTF()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns("abadframework");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "net46",
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeFalse();

            task.AssignedProjects.Should().HaveCount(1);

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }

        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_BadAtf()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns("netcoreapp2.0");
            reference.Setup(e => e.GetMetadata("HasSingleTargetFramework")).Returns("true");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "netcoreapp2.0",
                FallbackTargetFrameworks = new string[] { "abcdef" },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();

            result.Should().BeFalse();
            task.AssignedProjects.Should().BeNullOrEmpty();
            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }

        [Fact]
        public void GetReferenceNearestTargetFrameworkTask_BadMultipleAtf()
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns("netcoreapp2.0");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "netcoreapp2.0",
                FallbackTargetFrameworks = new string[] { "net46", "abcdef" },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();

            result.Should().BeFalse();
            task.AssignedProjects.Should().BeNullOrEmpty();
            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }

        [Theory]
        [InlineData("netcoreapp2.0", "netcoreapp2.0", false)]
        [InlineData("net46", "net46", true)]
        public void GetReferenceNearestTargetFrameworkTask_WithSingleTfmAndSingleAtf(string referenceProjectFramework, string nearestMatchingFramework, bool willGenerateWarning)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(referenceProjectFramework);
            reference.Setup(e => e.GetMetadata("HasSingleTargetFramework")).Returns("true");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "netcoreapp2.0",
                FallbackTargetFrameworks = new string[] { "net46" },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().HaveCount(1);
            task.AssignedProjects[0].GetMetadata("NearestTargetFramework").Should().Be(nearestMatchingFramework);

            testLogger.Warnings.Should().Be(willGenerateWarning ? 1 : 0);
            testLogger.Errors.Should().Be(0);
        }

        [Theory]
        [InlineData("netcoreapp2.0", "netcoreapp2.0", false)]
        [InlineData("net46", "net46", true)]
        public void GetReferenceNearestTargetFrameworkTask_WithSingleTfmAndMultipleAtf(string referenceProjectFramework, string nearestMatchingFramework, bool willGenerateWarning)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(referenceProjectFramework);
            reference.Setup(e => e.GetMetadata("HasSingleTargetFramework")).Returns("true");
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "netcoreapp2.0",
                FallbackTargetFrameworks = new string[] { "net46", "net461" },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().HaveCount(1);
            task.AssignedProjects[0].GetMetadata("NearestTargetFramework").Should().Be(nearestMatchingFramework);

            testLogger.Warnings.Should().Be(willGenerateWarning ? 1 : 0);
            testLogger.Errors.Should().Be(0);
        }

        [Theory]
        [InlineData("netcoreapp2.0; netcoreapp1.0", "netcoreapp2.0", false)]
        [InlineData("net45; net46", "net46", true)]
        public void GetReferenceNearestTargetFrameworkTask_WithMultipleTfmAndSingleAtf(string referenceProjectFramework, string nearestMatchingFramework, bool willGenerateWarning)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(referenceProjectFramework);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "netcoreapp2.0",
                FallbackTargetFrameworks = new string[] { "net46" },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().HaveCount(1);
            task.AssignedProjects[0].GetMetadata("NearestTargetFramework").Should().Be(nearestMatchingFramework);

            testLogger.Warnings.Should().Be(willGenerateWarning ? 1 : 0);
            testLogger.Errors.Should().Be(0);
        }

        [Theory]
        [InlineData("netcoreapp2.0; netcoreapp1.0", "netcoreapp2.0", false)]
        [InlineData("net45; net46", "net46", true)]
        public void GetReferenceNearestTargetFrameworkTask_WithMultipleTfmAndMultipleAtf(string referenceProjectFramework, string nearestMatchingFramework, bool willGenerateWarning)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(referenceProjectFramework);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "netcoreapp2.0",
                FallbackTargetFrameworks = new string[] { "net46", "net45", "net461" },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().HaveCount(1);
            task.AssignedProjects[0].GetMetadata("NearestTargetFramework").Should().Be(nearestMatchingFramework);

            testLogger.Warnings.Should().Be(willGenerateWarning ? 1 : 0);
            testLogger.Errors.Should().Be(0);
        }


        [Theory]
        [InlineData("netcoreapp2.0; netcoreapp1.1")]
        [InlineData("net45; net46")]
        public void GetReferenceNearestTargetFrameworkTask_WithMultipleTfmAndMultipleAtfAndNoMatch(string referenceProjectFramework)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(referenceProjectFramework);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "netcoreapp1.0",
                FallbackTargetFrameworks = new string[] { "net40", "net41" },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeFalse();

            task.AssignedProjects.Should().HaveCount(1);

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }

        [Theory]
        [InlineData("netcoreapp2.0")]
        [InlineData("net46")]
        public void GetReferenceNearestTargetFrameworkTask_WithSingleTfmAndSingleAtfAndNoMatch(string referenceProjectFramework)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(referenceProjectFramework);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = "netcoreapp1.0",
                FallbackTargetFrameworks = new string[] { "net45" },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeFalse();

            task.AssignedProjects.Should().HaveCount(1);

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }


        [Theory]
        [InlineData(".NETFramework,Version=v4.7.2", "", "net46", ".NETFramework,Version=v4.6", "None", "net46")]
        [InlineData(".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "net5.0-windows", ".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "net5.0-windows")]
        [InlineData(".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "net5.0", ".NETCoreApp,Version=v5.0", "None", "net5.0")]
        [InlineData(".NETCoreApp,Version=v6.0", "Windows,Version=7.0", "net5.0;netcoreapp3.0", ".NETCoreApp,Version=v5.0;.NETCoreApp,Version=v3.0", "None;None", "net5.0")]
        [InlineData(".NETCoreApp,Version=v6.0", "Android,Version=22.0", "net5.0;net5.0-android", ".NETCoreApp,Version=v5.0;.NETCoreApp,Version=v5.0", "None;android,Version=21.0", "net5.0-android")]
        [InlineData(".NETCoreApp,Version=v5.0", null, "latestnet;latestnetstandard", ".NETFramework,Version=v4.7.2;.NETStandard,Version=v2.1", "None;None", "latestnetstandard")]
        [InlineData(".NETCoreApp,Version=v5.0", null, "net5.0;net5.0-windows", ".NETCoreApp,Version=v5.0;.NETCoreApp,Version=v5.0", "None;Windows,Version=7.0", "net5.0")]
        [InlineData(".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "net;net-windows", ".NETCoreApp,Version=v5.0;.NETCoreApp,Version=v5.0", "None;Windows,Version=7.0", "net-windows")]
        [InlineData(".NETCoreApp,Version=v5.0", null, "net5.0;netcoreapp3.1", ".NETCoreApp,Version=v5.0;.NETCoreApp,Version=v3.1", "None;Windows,Version=7.0", "net5.0")]
        [InlineData(".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "net5.0-windows;netcoreapp3.1", ".NETCoreApp,Version=v5.0;.NETCoreApp,Version=v3.1", "Windows,Version=7.0;Windows,Version=7.0", "net5.0-windows")]
        [InlineData(".NETCoreApp,Version=v5.0", "None", "net;net-windows", ".NETCoreApp,Version=v5.0;.NETCoreApp,Version=v5.0", "None;Windows,Version=7.0", "net")]
        [InlineData(".NETCoreApp,Version=v3.1", "Windows,Version=7.0", "net50;netstandard20", ".NETCoreApp,Version=v5.0;NETStandard,Version=v2.0", "Windows,Version=7.0;None", "netstandard20")]
        public void GetReferenceNearestTargetFrameworkTask_WithTargetFrameworkInformation_ReturnsCompatibleAlias(
            string currentProjectTFM, string currentProjectTPM, string refTargetFrameworks, string refTargetFrameworkMonikers, string refTargetPlatformMonikers, string expected)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(refTargetFrameworks);
            reference.Setup(e => e.GetMetadata("TargetFrameworkMonikers")).Returns(refTargetFrameworkMonikers);
            reference.Setup(e => e.GetMetadata("TargetPlatformMonikers")).Returns(refTargetPlatformMonikers);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = currentProjectTFM,
                CurrentProjectTargetPlatform = currentProjectTPM,
                FallbackTargetFrameworks = new string[] { },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue(because: testLogger.ShowMessages());

            task.AssignedProjects.Should().HaveCount(1);
            task.AssignedProjects[0].GetMetadata("NearestTargetFramework").Should().Be(expected);

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(0);
        }

        [Theory]
        [InlineData(".NETCoreApp,Version=v1.0", "", "netcoreapp2.0;net472", ".NETCoreApp,Version=v2.0;.NETFramework,Version=v4.7.2", "None;None")]
        [InlineData(".NETCoreApp,Version=v5.0", "", "net5.0-android;net5.0-windows", ".NETCoreApp,Version=v5.0;NETCoreApp,Version=v5.0", "android,Version=21.0;Windows,Version=7.0")]
        public void GetReferenceNearestTargetFrameworkTask_WithTargetFrameworkInformation_WithoutMatchingFrameworks_Errors(
    string currentProjectTFM, string currentProjectTPM, string refTargetFrameworks, string refTargetFrameworkMonikers, string refTargetPlatformMonikers)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(refTargetFrameworks);
            reference.Setup(e => e.GetMetadata("TargetFrameworkMonikers")).Returns(refTargetFrameworkMonikers);
            reference.Setup(e => e.GetMetadata("TargetPlatformMonikers")).Returns(refTargetPlatformMonikers);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = currentProjectTFM,
                CurrentProjectTargetPlatform = currentProjectTPM,
                FallbackTargetFrameworks = new string[] { },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();

            result.Should().BeFalse();

            task.AssignedProjects.Should().HaveCount(1);

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }


        [Theory]
        [InlineData(".NETFramework,Version=v4.7.2", null, "net46;net472", ".NETFramework,Version=v4.6", "None")]
        [InlineData(".NETFramework,Version=v4.7.2", null, "net46", ".NETFramework,Version=v4.6;.NETFramework,Version=v4.7.2", "None")]
        [InlineData(".NETFramework,Version=v4.7.2", null, "net46;net472", ".NETFramework,Version=v4.6", "None;Windows,Version=77.0")]
        public void GetReferenceNearestTargetFrameworkTask_WithInvalidParameters_Errors(
    string currentProjectTFM, string currentProjectTPM, string refTargetFrameworks, string refTargetFrameworkMonikers, string refTargetPlatformMonikers)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(refTargetFrameworks);
            reference.Setup(e => e.GetMetadata("TargetFrameworkMonikers")).Returns(refTargetFrameworkMonikers);
            reference.Setup(e => e.GetMetadata("TargetPlatformMonikers")).Returns(refTargetPlatformMonikers);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = currentProjectTFM,
                CurrentProjectTargetPlatform = currentProjectTPM,
                FallbackTargetFrameworks = new string[] { },
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeFalse();
            task.AssignedProjects.Should().HaveCount(1);
            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }

        [Theory]
        [InlineData(".NETCoreApp,Version=v1.0", "", "net46", ".NETFramework,Version=v4.6", "None", "net45")]
        [InlineData(".NETCoreApp,Version=v1.0", "", "netcoreapp2.0", ".NETCoreApp,Version=v2.0", "None", "net45")]
        [InlineData(".NETCoreApp,Version=v1.0", "", "netcoreapp2.0", ".NETCoreApp,Version=v2.0", "None", "net45;net461")]
        [InlineData(".NETCoreApp,Version=v1.0", "", "netcoreapp2.0;net472", ".NETCoreApp,Version=v2.0;.NETFramework,Version=v4.7.2", "None;None", "net45;net461")]
        public void GetReferenceNearestTargetFrameworkTask_WithTargetFrameworkInformation_WithAssetTargetFallback_NoMatch(
            string currentProjectTFM, string currentProjectTPM, string refTargetFrameworks, string refTargetFrameworkMonikers, string refTargetPlatformMonikers, string atf)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(refTargetFrameworks);
            reference.Setup(e => e.GetMetadata("TargetFrameworkMonikers")).Returns(refTargetFrameworkMonikers);
            reference.Setup(e => e.GetMetadata("TargetPlatformMonikers")).Returns(refTargetPlatformMonikers);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = currentProjectTFM,
                CurrentProjectTargetPlatform = currentProjectTPM,
                FallbackTargetFrameworks = atf.Split(';'),
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();

            result.Should().BeFalse();

            task.AssignedProjects.Should().HaveCount(1);

            testLogger.Warnings.Should().Be(0);
            testLogger.Errors.Should().Be(1);
        }


        [Theory]
        [InlineData(".NETCoreApp,Version=v2.0", "", "net45;net46", ".NETFramework,Version=v4.5;.NETFramework,Version=v4.6", "None;None", "net46;net45;net461", "net46")]
        [InlineData(".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "net472", ".NETFramework,Version=v4.7.2", "None", "net472;net471;net47;net462;net461;net46;net45", "net472")]
        [InlineData(".NETCoreApp,Version=v5.0", "Windows,Version=7.0", "actualResolvedAlias", ".NETFramework,Version=v4.7.2", "None", "net472;net471;net47;net462;net461;net46;net45", "actualResolvedAlias")]

        public void GetReferenceNearestTargetFrameworkTask_WithTargetFrameworkInformation_WhenATFMatches_Warns(
            string currentProjectTFM, string currentProjectTPM, string refTargetFrameworks, string refTargetFrameworkMonikers, string refTargetPlatformMonikers, string atf, string expected)
        {
            var buildEngine = new TestBuildEngine();
            var testLogger = buildEngine.TestLogger;

            var references = new List<ITaskItem>();
            var reference = new Mock<ITaskItem>();
            reference.SetupGet(e => e.ItemSpec).Returns("a.csproj");
            reference.Setup(e => e.GetMetadata("TargetFrameworks")).Returns(refTargetFrameworks);
            reference.Setup(e => e.GetMetadata("TargetFrameworkMonikers")).Returns(refTargetFrameworkMonikers);
            reference.Setup(e => e.GetMetadata("TargetPlatformMonikers")).Returns(refTargetPlatformMonikers);
            references.Add(reference.Object);

            var task = new GetReferenceNearestTargetFrameworkTask
            {
                BuildEngine = buildEngine,
                CurrentProjectTargetFramework = currentProjectTFM,
                CurrentProjectTargetPlatform = currentProjectTPM,
                FallbackTargetFrameworks = atf.Split(';'),
                AnnotatedProjectReferences = references.ToArray()
            };

            var result = task.Execute();
            result.Should().BeTrue();

            task.AssignedProjects.Should().HaveCount(1);
            task.AssignedProjects[0].GetMetadata("NearestTargetFramework").Should().Be(expected);

            testLogger.Warnings.Should().Be(1);
            testLogger.Errors.Should().Be(0);
        }
    }
}
