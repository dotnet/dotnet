// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using Microsoft.VisualStudio.Extensibility.Testing;
using Xunit;
using Xunit.Sdk;
using Task = System.Threading.Tasks.Task;

namespace Microsoft.VisualStudio.Razor.IntegrationTests;

// TODO: Start collecting LogFiles on failure

/// <remarks>
/// The following is the xunit execution order:
///
/// <list type="number">
/// <item><description>Instance constructor</description></item>
/// <item><description><see cref="IAsyncLifetime.InitializeAsync"/></description></item>
/// <item><description><see cref="BeforeAfterTestAttribute.Before"/></description></item>
/// <item><description>Test method</description></item>
/// <item><description><see cref="BeforeAfterTestAttribute.After"/></description></item>
/// <item><description><see cref="IAsyncLifetime.DisposeAsync"/></description></item>
/// <item><description><see cref="IDisposable.Dispose"/></description></item>
/// </list>
/// </remarks>
[IdeSettings(MinVersion = VisualStudioVersion.VS2022, RootSuffix = "RoslynDev", MaxAttempts = 2)]
public abstract class AbstractIntegrationTest : AbstractIdeIntegrationTest
{
    protected const string ProjectName = "TestProj";
    protected const string SolutionName = "TestSolution";

    private readonly static TimeSpan s_shortHangMitigatingTimeout = new(hours: 0, minutes: 1, seconds: 0);
    private readonly CancellationTokenSource _shortHangMitigatingCancellationTokenSource = new(s_shortHangMitigatingTimeout);

    protected CancellationToken ControlledHangMitigatingCancellationToken => HangMitigatingCancellationToken;

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();
    }
}
