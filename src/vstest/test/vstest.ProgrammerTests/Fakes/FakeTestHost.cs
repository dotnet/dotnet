// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace vstest.ProgrammerTests.Fakes;

internal class FakeTestHostFixture : IDisposable
{
    public int Id { get; }
    public List<FakeTestDllFile> Dlls { get; }
    public FakeTestRuntimeProvider FakeTestRuntimeProvider { get; }
    public FakeCommunicationEndpoint FakeCommunicationEndpoint { get; }
    public FakeCommunicationChannel<FakeTestHostFixture> FakeCommunicationChannel { get; }
    public List<RequestResponsePair<string, FakeMessage, FakeTestHostFixture>> Responses { get; }
    public FakeProcess Process { get; internal set; }

    public FakeTestHostFixture(
        int id, List<FakeTestDllFile> dlls,
        FakeTestRuntimeProvider fakeTestRuntimeProvider,
        FakeCommunicationEndpoint fakeCommunicationEndpoint,
        FakeCommunicationChannel<FakeTestHostFixture> fakeCommunicationChannel,
        FakeProcess process,
        List<RequestResponsePair<string, FakeMessage, FakeTestHostFixture>> responses)
    {
        Id = id;
        Dlls = dlls;
        FakeTestRuntimeProvider = fakeTestRuntimeProvider;
        FakeCommunicationEndpoint = fakeCommunicationEndpoint;
        FakeCommunicationChannel = fakeCommunicationChannel;
        Process = process;
        Responses = responses;

        // The channel will pass back this whole fixture as context for every processed request so we can
        // refer back to any part of testhost in message responses. E.g. to abort the channel, or exit
        // testhost before or after answering.
        fakeCommunicationChannel.Start(this);
    }

    public void Dispose()
    {
        try { FakeCommunicationChannel.Dispose(); } catch (ObjectDisposedException) { }
    }
}
