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
/// It exposes four tests, mirroring the MSTest asset so results are directly comparable:
///   - TestAddPasses      : passes  (exercises Calculator.Add)
///   - TestMultiplyPasses : passes  (exercises Calculator.Multiply)
///   - TestFails          : fails   (throws)
///   - TestSkipped        : skipped
/// Expected: Passed 2, Failed 1, Skipped 1, Total 4.
/// </summary>
internal sealed class PureTestFramework : ITestFramework, IDataProducer
{
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
