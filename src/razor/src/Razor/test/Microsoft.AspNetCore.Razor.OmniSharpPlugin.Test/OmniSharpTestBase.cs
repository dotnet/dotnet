// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.LanguageServer.Common;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.AspNetCore.Razor.Test.Common;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.AspNetCore.Razor.OmniSharpPlugin;

[Collection("MSBuildLocator")]
public abstract class OmniSharpTestBase : LanguageServerTestBase
{
    private readonly MethodInfo _createTestProjectSnapshotMethod;
    private readonly MethodInfo _createWithDocumentsTestProjectSnapshotMethod;
    private readonly MethodInfo _createProjectSnapshotManagerMethod;
    private readonly PropertyInfo _allowNotifyListenersProperty;
    private readonly PropertyInfo _dispatcherProperty;
    private readonly ConstructorInfo _omniSharpProjectSnapshotMangerConstructor;
    private readonly ConstructorInfo _omniSharpSnapshotConstructor;

    internal OmniSharpProjectSnapshotManagerDispatcher Dispatcher { get; }

    protected OmniSharpTestBase(ITestOutputHelper testOutput)
        : base(testOutput)
    {
        var commonTestAssembly = Assembly.Load("Microsoft.AspNetCore.Razor.LanguageServer.Test.Common");
        var testProjectSnapshotType = commonTestAssembly.GetType("Microsoft.AspNetCore.Razor.Test.Common.TestProjectSnapshot");

        var testProjectSnapshotManagerType = commonTestAssembly.GetType("Microsoft.AspNetCore.Razor.Test.Common.TestProjectSnapshotManager");
        var strongNamedAssembly = Assembly.Load("Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp");
        var defaultSnapshotManagerType = strongNamedAssembly.GetType("Microsoft.AspNetCore.Razor.ExternalAccess.OmniSharp.Project.OmniSharpProjectSnapshotManager");

        _createTestProjectSnapshotMethod = testProjectSnapshotType.GetMethod("Create", new[] { typeof(string), typeof(ProjectWorkspaceState) });
        _createWithDocumentsTestProjectSnapshotMethod = testProjectSnapshotType.GetMethod("Create", new[] { typeof(string), typeof(string[]), typeof(ProjectWorkspaceState) });
        _createProjectSnapshotManagerMethod = testProjectSnapshotManagerType.GetMethod("Create");
        _allowNotifyListenersProperty = testProjectSnapshotManagerType.GetProperty("AllowNotifyListeners");
        _dispatcherProperty = typeof(OmniSharpProjectSnapshotManagerDispatcher).GetProperty("InternalDispatcher", BindingFlags.NonPublic | BindingFlags.Instance);
        _omniSharpProjectSnapshotMangerConstructor = defaultSnapshotManagerType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();
        _omniSharpSnapshotConstructor = typeof(OmniSharpProjectSnapshot).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single();

        Dispatcher = new OmniSharpProjectSnapshotManagerDispatcher();
        AddDisposable((IDisposable)Dispatcher.DispatcherScheduler);
    }

    private protected OmniSharpProjectSnapshot CreateProjectSnapshot(string projectFilePath)
    {
        var projectWorkspaceState = new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, CodeAnalysis.CSharp.LanguageVersion.Default);
        var projectSnapshot = _createTestProjectSnapshotMethod.Invoke(null, new object[] { projectFilePath, projectWorkspaceState });
        var omniSharpProjectSnapshot = (OmniSharpProjectSnapshot)_omniSharpSnapshotConstructor.Invoke(new[] { projectSnapshot });

        return omniSharpProjectSnapshot;
    }

    private protected OmniSharpProjectSnapshot CreateProjectSnapshot(string projectFilePath, string[] documentFilePaths)
    {
        var projectWorkspaceState = new ProjectWorkspaceState(ImmutableArray<TagHelperDescriptor>.Empty, CodeAnalysis.CSharp.LanguageVersion.Default);
        var projectSnapshot = _createWithDocumentsTestProjectSnapshotMethod.Invoke(null, new object[] { projectFilePath, documentFilePaths, projectWorkspaceState });
        var omniSharpProjectSnapshot = (OmniSharpProjectSnapshot)_omniSharpSnapshotConstructor.Invoke(new[] { projectSnapshot });

        return omniSharpProjectSnapshot;
    }

    private protected OmniSharpProjectSnapshotManager CreateProjectSnapshotManager(bool allowNotifyListeners = false)
    {
        var dispatcher = _dispatcherProperty.GetValue(Dispatcher);
        var testSnapshotManager = _createProjectSnapshotManagerMethod.Invoke(null, new object[] { dispatcher, ErrorReporter });
        _allowNotifyListenersProperty.SetValue(testSnapshotManager, allowNotifyListeners);
        var remoteTextLoaderFactory = new DefaultRemoteTextLoaderFactory();
        var snapshotManager = (OmniSharpProjectSnapshotManager)_omniSharpProjectSnapshotMangerConstructor.Invoke(new[] { testSnapshotManager, remoteTextLoaderFactory });

        return snapshotManager;
    }

    protected Task RunOnDispatcherThreadAsync(Action action)
    {
        return Task.Factory.StartNew(
            action,
            DisposalToken,
            TaskCreationOptions.None,
            Dispatcher.DispatcherScheduler);
    }

    protected Task<TReturn> RunOnDispatcherThreadAsync<TReturn>(Func<TReturn> func)
    {
        return Task.Factory.StartNew(
            func,
            DisposalToken,
            TaskCreationOptions.None,
            Dispatcher.DispatcherScheduler);
    }

    protected Task RunOnDispatcherThreadAsync(Func<Task> func)
    {
        return Task.Factory.StartNew(
            func,
            DisposalToken,
            TaskCreationOptions.None,
            Dispatcher.DispatcherScheduler);
    }
}
