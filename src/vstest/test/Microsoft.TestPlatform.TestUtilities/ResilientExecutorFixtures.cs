// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace Microsoft.TestPlatform.TestUtilities;

/// <summary>
/// Test executors used to verify that the extension managers stay resilient when a single
/// <see cref="ITestExecutor2"/> cannot be instantiated.
///
/// <para>
/// These types intentionally live in this non test assembly so that they are only discovered when a
/// test explicitly points discovery at this assembly (via
/// <c>GetExecutionExtensionManager(&lt;this assembly&gt;)</c>). They are not named
/// <c>*TestAdapter.dll</c> so the default extension discovery does not pick them up, and the unit
/// tests that assert every discovered executor is created point at their own assemblies, so hosting
/// a deliberately broken executor here does not break them.
/// </para>
/// </summary>
[ExtensionUri(GoodResilientTestExecutor.ExecutorUri)]
public class GoodResilientTestExecutor : ITestExecutor2
{
    /// <summary>
    /// The extension URI of this executor.
    /// </summary>
    public const string ExecutorUri = "executor://resilient.good";

    public void Cancel()
    {
    }

    public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
    {
    }

    public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
    {
    }

    public bool ShouldAttachToTestHost(IEnumerable<string>? sources, IRunContext runContext)
    {
        return false;
    }

    public bool ShouldAttachToTestHost(IEnumerable<TestCase>? tests, IRunContext runContext)
    {
        return false;
    }
}

/// <summary>
/// An <see cref="ITestExecutor2"/> that cannot be instantiated because it has no parameterless
/// constructor. Discovery finds this type (discovery does not require a parameterless constructor),
/// but instantiating it throws <see cref="MissingMethodException"/>. This reproduces the "rogue
/// executor" scenario that used to tear down the whole executor extension manager.
/// </summary>
[ExtensionUri(RogueResilientTestExecutor.ExecutorUri)]
public class RogueResilientTestExecutor : ITestExecutor2
{
    /// <summary>
    /// The extension URI of this executor.
    /// </summary>
    public const string ExecutorUri = "executor://resilient.rogue";

    /// <summary>
    /// Initializes a new instance of the <see cref="RogueResilientTestExecutor"/> class.
    /// The required parameter means there is no parameterless constructor, so the extension
    /// framework cannot activate this type.
    /// </summary>
    /// <param name="required">A required parameter that prevents parameterless activation.</param>
    public RogueResilientTestExecutor(string required)
    {
        _ = required;
    }

    public void Cancel()
    {
    }

    public void RunTests(IEnumerable<TestCase>? tests, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
    {
    }

    public void RunTests(IEnumerable<string>? sources, IRunContext? runContext, IFrameworkHandle? frameworkHandle)
    {
    }

    public bool ShouldAttachToTestHost(IEnumerable<string>? sources, IRunContext runContext)
    {
        return false;
    }

    public bool ShouldAttachToTestHost(IEnumerable<TestCase>? tests, IRunContext runContext)
    {
        return false;
    }
}
