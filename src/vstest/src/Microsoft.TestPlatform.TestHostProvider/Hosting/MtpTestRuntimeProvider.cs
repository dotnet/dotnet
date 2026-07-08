// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Client.MTP;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Host;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Hosting;

/// <summary>
/// Runtime provider for Microsoft.Testing.Platform (MTP) test applications.
/// </summary>
/// <remarks>
/// An MTP test application is its own test host: it is launched in server mode and driven directly over the
/// Microsoft.Testing.Platform JSON-RPC protocol, not the vstest testhost protocol. This provider therefore does
/// not launch a vstest testhost at all — the launch-related members of <see cref="ITestRuntimeProvider"/> throw
/// <see cref="NotSupportedException"/>. Instead it:
/// <list type="bullet">
///   <item><description>
///     claims MTP sources via <see cref="ISourceAwareTestRuntimeProvider"/> (source-aware detection using the
///     build-time Microsoft.Testing.Platform marker), so it is selected ahead of the generic testhost providers
///     that match only by target framework; and
///   </description></item>
///   <item><description>
///     supplies its own discovery/execution proxy managers via <see cref="IProxyManagerFactory"/>, so the
///     engine drives MTP sources over the MTP proxies without any protocol-specific branching in the engine.
///   </description></item>
/// </list>
/// This is why the provider is registered like a normal runtime provider yet routes to
/// <see cref="MtpProxyDiscoveryManager"/>/<see cref="MtpProxyExecutionManager"/> instead of a vstest testhost.
/// </remarks>
[ExtensionUri(MicrosoftTestingPlatformHostUri)]
[FriendlyName(MicrosoftTestingPlatformHostFriendlyName)]
public class MtpTestRuntimeProvider : ISourceAwareTestRuntimeProvider, IProxyManagerFactory
{
    private const string MicrosoftTestingPlatformHostUri = "HostProvider://MicrosoftTestingPlatformHost";
    private const string MicrosoftTestingPlatformHostFriendlyName = "MicrosoftTestingPlatformHost";

    // The launch-related members are not supported because an MTP application hosts itself; it is never
    // launched as a vstest testhost. Discovery and execution go through the MTP proxy managers instead.
    private const string NotSupportedMessage = "Microsoft.Testing.Platform applications are hosted over the MTP protocol and are not launched as a vstest test host.";

    event EventHandler<HostProviderEventArgs>? ITestRuntimeProvider.HostLaunched
    {
        add { }
        remove { }
    }

    event EventHandler<HostProviderEventArgs>? ITestRuntimeProvider.HostExited
    {
        add { }
        remove { }
    }

    bool ITestRuntimeProvider.Shared => false;

    void ITestRuntimeProvider.Initialize(IMessageLogger? logger, string runsettingsXml)
    {
    }

    // Source-blind resolution can never own a run: without the sources we cannot tell whether they are MTP
    // applications, so we decline and let the source-aware path below make the decision.
    bool ITestRuntimeProvider.CanExecuteCurrentRunConfiguration(string? runsettingsXml) => false;

    // Source-aware resolution: this provider owns the run only when every source is a Microsoft.Testing.Platform
    // application. A mixed set (some MTP, some classic) is split into separate configurations upstream, so each
    // group asked here is homogeneous.
    bool ISourceAwareTestRuntimeProvider.CanExecuteCurrentRunConfiguration(string? runsettingsXml, IEnumerable<string> sources)
        => AllSourcesAreMicrosoftTestingPlatform(sources);

    void ITestRuntimeProvider.SetCustomLauncher(ITestHostLauncher customLauncher)
    {
    }

    TestHostConnectionInfo ITestRuntimeProvider.GetTestHostConnectionInfo() => throw new NotSupportedException(NotSupportedMessage);

    Task<bool> ITestRuntimeProvider.LaunchTestHostAsync(TestProcessStartInfo testHostStartInfo, CancellationToken cancellationToken) => throw new NotSupportedException(NotSupportedMessage);

    TestProcessStartInfo ITestRuntimeProvider.GetTestHostProcessStartInfo(IEnumerable<string> sources, IDictionary<string, string?>? environmentVariables, TestRunnerConnectionInfo connectionInfo) => throw new NotSupportedException(NotSupportedMessage);

    IEnumerable<string> ITestRuntimeProvider.GetTestPlatformExtensions(IEnumerable<string> sources, IEnumerable<string> extensions) => Enumerable.Empty<string>();

    IEnumerable<string> ITestRuntimeProvider.GetTestSources(IEnumerable<string> sources) => sources;

    Task ITestRuntimeProvider.CleanTestHostAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    IProxyDiscoveryManager IProxyManagerFactory.CreateDiscoveryManager() => MtpProxyManagerFactory.CreateDiscoveryManager();

    IProxyExecutionManager IProxyManagerFactory.CreateExecutionManager(IProxyDataCollectionManager? dataCollectionManager)
        => MtpProxyManagerFactory.CreateExecutionManager(dataCollectionManager);

    private static bool AllSourcesAreMicrosoftTestingPlatform(IEnumerable<string> sources)
    {
        var any = false;
        foreach (var source in sources)
        {
            any = true;

            // The detector never throws: for a null/empty/unreadable source it returns false, which correctly
            // disqualifies the group.
            if (!MicrosoftTestingPlatformDetector.IsMicrosoftTestingPlatformApp(source))
            {
                return false;
            }
        }

        return any;
    }
}
