// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;

/// <summary>
/// An <see cref="ITestRequestManager"/> that defers construction of the real
/// <see cref="TestRequestManager"/> until the first member is used. This lets the composition root
/// own a single request manager instead of reaching for a process-wide singleton, while still
/// building it lazily: the real manager reads the parsed command line (for example
/// <c>IsDesignMode</c>) and loads the test platform, so it must be created only after the command
/// line has been parsed, and never for commands that neither run nor discover tests (for example
/// <c>--Help</c>).
/// </summary>
internal sealed class LazyTestRequestManager : ITestRequestManager
{
    private readonly Lazy<ITestRequestManager> _inner;

    public LazyTestRequestManager(Func<ITestRequestManager> factory)
    {
        _inner = new Lazy<ITestRequestManager>(factory);
    }

    public void InitializeExtensions(IEnumerable<string>? pathToAdditionalExtensions, bool skipExtensionFilters)
        => _inner.Value.InitializeExtensions(pathToAdditionalExtensions, skipExtensionFilters);

    public void ResetOptions()
        => _inner.Value.ResetOptions();

    public void DiscoverTests(DiscoveryRequestPayload discoveryPayload, ITestDiscoveryEventsRegistrar disoveryEventsRegistrar, ProtocolConfig protocolConfig)
        => _inner.Value.DiscoverTests(discoveryPayload, disoveryEventsRegistrar, protocolConfig);

    public void RunTests(TestRunRequestPayload testRunRequestPayLoad, ITestHostLauncher3? customTestHostLauncher, ITestRunEventsRegistrar testRunEventsRegistrar, ProtocolConfig protocolConfig)
        => _inner.Value.RunTests(testRunRequestPayLoad, customTestHostLauncher, testRunEventsRegistrar, protocolConfig);

    public void ProcessTestRunAttachments(TestRunAttachmentsProcessingPayload testRunAttachmentsProcessingPayload, ITestRunAttachmentsProcessingEventsHandler testRunAttachmentsProcessingEventsHandler, ProtocolConfig protocolConfig)
        => _inner.Value.ProcessTestRunAttachments(testRunAttachmentsProcessingPayload, testRunAttachmentsProcessingEventsHandler, protocolConfig);

    public void CancelTestRun()
        => _inner.Value.CancelTestRun();

    public void AbortTestRun()
        => _inner.Value.AbortTestRun();

    public void CancelDiscovery()
        => _inner.Value.CancelDiscovery();

    public void CancelTestRunAttachmentsProcessing()
        => _inner.Value.CancelTestRunAttachmentsProcessing();

    public void Dispose()
    {
        if (_inner.IsValueCreated)
        {
            _inner.Value.Dispose();
        }
    }
}
