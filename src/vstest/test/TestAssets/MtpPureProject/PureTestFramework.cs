// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.Testing.Platform.Extensions.Messages;
using Microsoft.Testing.Platform.Extensions.TestFramework;
using Microsoft.Testing.Platform.Requests;
using Microsoft.Testing.Platform.TestHost;

namespace MtpPureProject;

/// <summary>
/// A hand-rolled Microsoft.Testing.Platform test framework. It has no knowledge of vstest, MSTest,
/// or Microsoft.TestPlatform.ObjectModel. It publishes test nodes over the MTP protocol directly,
/// which is exactly what vstest's MTP provider consumes.
///
/// It exposes five tests, mirroring the MSTest asset so results are directly comparable:
///   - TestAddPasses          : passes  (exercises Calculator.Add)
///   - TestMultiplyPasses     : passes  (exercises Calculator.Multiply)
///   - TestFails              : fails   (throws)
///   - TestSkipped            : skipped
///   - TestNonAsciiDisplayName: passes, and carries a multi-byte UTF-8 display name
/// Expected: Passed 3, Failed 1, Skipped 1, Total 5.
/// </summary>
internal sealed class PureTestFramework : ITestFramework, IDataProducer
{
    /// <summary>
    /// A display name whose UTF-8 byte count exceeds its character count: German umlauts (2 bytes
    /// each), Japanese (3 bytes each) and a Czech caron (2 bytes).
    ///
    /// The MTP frame header declares Content-Length in bytes, so a client that consumes that number
    /// of characters instead under-reads the frame and desynchronizes the connection from the next
    /// message onward. Test names are user-authored and flow server-to-client on every node update,
    /// which makes this the realistic trigger.
    ///
    /// Note this asset is not currently referenced by any test in the repo - the acceptance coverage
    /// lives on MtpMSTestProject, which every MTP scenario uses. This name is kept in step with that
    /// asset so the two stay comparable if this one is ever wired up.
    /// </summary>
    internal const string NonAsciiTestName = "TestGrüße日本語Čau";

    private static readonly SessionUid SessionUid = new("PureMtpSession");

    private static readonly TestDefinition[] Tests =
    [
        new("TestAddPasses", "TestAddPasses", static () =>
        {
            if (Calculator.Add(2, 3) != 5)
            {
                throw new InvalidOperationException("Add returned the wrong value.");
            }
        }),
        new("TestMultiplyPasses", "TestMultiplyPasses", static () =>
        {
            if (Calculator.Multiply(4, 3) != 12)
            {
                throw new InvalidOperationException("Multiply returned the wrong value.");
            }
        }),
        new("TestFails", "TestFails", static () =>
            throw new InvalidOperationException("This test fails on purpose.")),
        new("TestSkipped", "TestSkipped", Body: null, Skip: true),

        // Deliberately last: a framing desynchronization caused by this node's multi-byte payload
        // corrupts whatever the server sends next, so the run-complete handshake is the victim.
        new(NonAsciiTestName, NonAsciiTestName, static () =>
        {
            if (Calculator.Add(1, 1) != 2)
            {
                throw new InvalidOperationException("Add returned the wrong value.");
            }
        }),
    ];

    public string Uid => nameof(PureTestFramework);

    public string Version => "1.0.0";

    public string DisplayName => "Pure MTP Test Framework";

    public string Description => "A minimal Microsoft.Testing.Platform test framework with no vstest dependency.";

    public Type[] DataTypesProduced => [typeof(TestNodeUpdateMessage)];

    public Task<bool> IsEnabledAsync() => Task.FromResult(true);

    public Task<CreateTestSessionResult> CreateTestSessionAsync(CreateTestSessionContext context)
        => Task.FromResult(new CreateTestSessionResult { IsSuccess = true });

    public Task<CloseTestSessionResult> CloseTestSessionAsync(CloseTestSessionContext context)
        => Task.FromResult(new CloseTestSessionResult { IsSuccess = true });

    public async Task ExecuteRequestAsync(ExecuteRequestContext context)
    {
        switch (context.Request)
        {
            case DiscoverTestExecutionRequest:
                foreach (TestDefinition test in Tests)
                {
                    await PublishAsync(context, test.Uid, test.DisplayName, new DiscoveredTestNodeStateProperty());
                }

                break;

            case RunTestExecutionRequest:
                foreach (TestDefinition test in Tests)
                {
                    IProperty state = RunOne(test);
                    await PublishAsync(context, test.Uid, test.DisplayName, state);
                }

                break;
        }

        context.Complete();
    }

    private static IProperty RunOne(TestDefinition test)
    {
        if (test.Skip)
        {
            return new SkippedTestNodeStateProperty();
        }

        try
        {
            test.Body!();
            return new PassedTestNodeStateProperty();
        }
        catch (Exception ex)
        {
            return new FailedTestNodeStateProperty(ex);
        }
    }

    private Task PublishAsync(ExecuteRequestContext context, string uid, string displayName, IProperty state)
        => context.MessageBus.PublishAsync(
            this,
            new TestNodeUpdateMessage(
                SessionUid,
                new TestNode
                {
                    Uid = uid,
                    DisplayName = displayName,
                    Properties = new PropertyBag(state),
                }));

    private sealed record TestDefinition(string Uid, string DisplayName, Action? Body, bool Skip = false);
}
