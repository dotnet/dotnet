// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NuGet.Commands;
using NuGet.Packaging.Core;
using NuGet.ProjectManagement;
using NuGet.ProjectModel;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using Xunit;

namespace NuGet.PackageManagement.Test.BuildIntegration
{
    public class BuildIntegratedProjectActionTests
    {
        private readonly NuGetProject _nuGetProject = Mock.Of<NuGetProject>();
        private readonly PackageIdentity _packageIdentity = new("a", new NuGetVersion(1, 0, 0));
        private readonly LockFile _lockFile = Mock.Of<LockFile>();
        private readonly RestoreResultPair _restoreResultPair = new(null, null);
        private readonly BuildIntegratedInstallationContext _installationContext = new(null, null, null);
        private readonly VersionRange _versionRange = new(new NuGetVersion(1, 0, 0));
        private readonly List<(NuGetProjectAction, BuildIntegratedInstallationContext)> _originalActionAndProjectContexts = new();

#pragma warning disable CS0618 // Type or member is obsolete
        [Fact]
        public void Constructor_WithNullProjectIdentity_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: null,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActions: new List<NuGetProjectAction>(),
                installationContext: _installationContext,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("project");
        }

        [Fact]
        public void Constructor_WithNullPackageIdentity_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: null,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActions: new List<NuGetProjectAction>(),
                installationContext: _installationContext,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("packageIdentity");
        }

        [Fact]
        public void Constructor_WithNullLockFile_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: null,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActions: new List<NuGetProjectAction>(),
                installationContext: _installationContext,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("originalLockFile");
        }

        [Fact]
        public void Constructor_WithNullRestoreResultPair_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: null,
                sources: new List<SourceRepository>(),
                originalActions: new List<NuGetProjectAction>(),
                installationContext: _installationContext,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("restoreResultPair");
        }

        [Fact]
        public void Constructor_WithNullSources_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: null,
                originalActions: new List<NuGetProjectAction>(),
                installationContext: _installationContext,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("sources");
        }

        [Fact]
        public void Constructor_WithNullOriginalActions_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActions: null,
                installationContext: _installationContext,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("originalActions");
        }

        [Fact]
        public void Constructor_WithNullInstallationContext_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActions: new List<NuGetProjectAction>(),
                installationContext: null,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("installationContext");
        }
#pragma warning restore CS0618 // Type or member is obsolete

        [Fact]
        public void Constructor_WithActionAndContextList_WithNullProjectIdentity_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: null,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActionsAndInstallationContexts: _originalActionAndProjectContexts,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("project");
        }

        [Fact]
        public void Constructor_WithActionAndContextList_WithNullPackageIdentity_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: null,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActionsAndInstallationContexts: _originalActionAndProjectContexts,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("packageIdentity");
        }

        [Fact]
        public void Constructor_WithActionAndContextList_WithNullLockFile_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: null,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActionsAndInstallationContexts: _originalActionAndProjectContexts,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("originalLockFile");
        }

        [Fact]
        public void Constructor_WithActionAndContextList_WithNullRestoreResultPair_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: null,
                sources: new List<SourceRepository>(),
                originalActionsAndInstallationContexts: _originalActionAndProjectContexts,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("restoreResultPair");
        }

        [Fact]
        public void Constructor_WithActionAndContextList_WithNullSources_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: null,
                originalActionsAndInstallationContexts: _originalActionAndProjectContexts,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("sources");
        }

        [Fact]
        public void Constructor_WithActionAndContextList_WithNullOriginalActionsAndInstallationContexts_Throws()
        {
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActionsAndInstallationContexts: null,
                versionRange: _versionRange));
            exception.ParamName.Should().Be("originalActionsAndInstallationContexts");
        }

        [Fact]
        public void Constructor_WithActionAndContextList_WithEmptyOriginalActionsAndInstallationContexts_Throws()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActionsAndInstallationContexts: new List<(NuGetProjectAction, BuildIntegratedInstallationContext)>(),
                versionRange: _versionRange));
            exception.ParamName.Should().Be("originalActionsAndInstallationContexts");
        }

        [Fact]
        public void Constructor_WithActionAndContextList_SetsOriginalActionsAndInstallationContextToFirst()
        {
            var firstProjectAction = NuGetProjectAction.CreateInstallProjectAction(_packageIdentity, Mock.Of<SourceRepository>(), _nuGetProject);
            var firstInstallationContext = new BuildIntegratedInstallationContext(null, null, null);
            var actionsAndContextsList = new List<(NuGetProjectAction, BuildIntegratedInstallationContext)>
            {
                (firstProjectAction, firstInstallationContext),
                (NuGetProjectAction.CreateUninstallProjectAction(_packageIdentity, _nuGetProject), new BuildIntegratedInstallationContext(null, null, null))
            };

            var action = new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActionsAndInstallationContexts: actionsAndContextsList,
                versionRange: _versionRange);

            action.ActionAndContextList.Should().HaveCount(2);
#pragma warning disable CS0618 // Type or member is obsolete
            action.OriginalActions.Should().HaveCount(2);
            action.OriginalActions[0].Should().Be(firstProjectAction);
            action.InstallationContext.Should().Be(firstInstallationContext);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        [Fact]
        public void Constructor_SetsOriginalActionsAndInstallationContextsList()
        {
            var firstProjectAction = NuGetProjectAction.CreateInstallProjectAction(_packageIdentity, Mock.Of<SourceRepository>(), _nuGetProject);
#pragma warning disable CS0618 // Type or member is obsolete
            var action = new BuildIntegratedProjectAction(
                project: _nuGetProject,
                packageIdentity: _packageIdentity,
                nuGetProjectActionType: NuGetProjectActionType.Install,
                originalLockFile: _lockFile,
                restoreResultPair: _restoreResultPair,
                sources: new List<SourceRepository>(),
                originalActions: new List<NuGetProjectAction>() { firstProjectAction },
                installationContext: _installationContext,
                versionRange: _versionRange);
#pragma warning restore CS0618 // Type or member is obsolete
            action.ActionAndContextList.Should().HaveCount(1);
            action.ActionAndContextList[0].Item1.Should().Be(firstProjectAction);
            action.ActionAndContextList[0].Item2.Should().Be(_installationContext);
        }
    }
}
