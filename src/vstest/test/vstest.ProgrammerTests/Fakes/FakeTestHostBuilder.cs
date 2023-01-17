// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

# if DEBUG
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
# endif

namespace vstest.ProgrammerTests.Fakes;

internal class FakeTestHostFixtureBuilder
{
    // This will be also used as a port number, don't start from 0
    // it skips some paths in the real code, because port 0 has special meaning.
    private static readonly SequentialId Id = new(1000);

    private readonly Fixture _fixture;

    // TODO: this would correctly be any test holding container, but let's not get ahead of myself.
    private readonly List<FakeTestDllFile> _dlls = new();
    private FakeProcess? _process;
    private List<RequestResponsePair<string, FakeMessage, FakeTestHostFixture>>? _responses;

    public FakeTestHostFixtureBuilder(Fixture fixture)
    {
        _fixture = fixture;
    }

    internal FakeTestHostFixtureBuilder WithTestDll(FakeTestDllFile dll)
    {
        _dlls.Add(dll);
        return this;
    }

    internal FakeTestHostFixture Build()
    {

        if (_responses == null)
            throw new InvalidOperationException("Add some reponses to the testhost by using WithResponses.");

        if (_process == null)
            throw new InvalidOperationException("Add some process to the testhost by using WithProcess.");

        var id = Id.Next();
        var fakeCommunicationChannel = new FakeCommunicationChannel<FakeTestHostFixture>(_responses, _fixture.ErrorAggregator, id);
        var fakeCommunicationEndpoint = new FakeCommunicationEndpoint(fakeCommunicationChannel, _fixture.ErrorAggregator);
        var fakeTestRuntimeProvider = new FakeTestRuntimeProvider(_fixture.ProcessHelper, _process, _fixture.FileHelper, _dlls, fakeCommunicationEndpoint, _fixture.ErrorAggregator);

#if DEBUG
        // This registers the endpoint so we can look it up later using the address, the Id from here is propagated to
        // testhost connection info, and is used as port in 127.0.0.1:<id>, address so we can lookup the correct channel.
        TestServiceLocator.Register<ICommunicationEndPoint>(fakeCommunicationEndpoint.TestHostConnectionInfo.Endpoint, fakeCommunicationEndpoint);
# endif

        return new FakeTestHostFixture(id, _dlls, fakeTestRuntimeProvider, fakeCommunicationEndpoint, fakeCommunicationChannel, _process, _responses);
    }

    internal FakeTestHostFixtureBuilder WithProcess(FakeProcess process)
    {
        _process = process;
        return this;
    }

    internal FakeTestHostFixtureBuilder WithResponses(List<RequestResponsePair<string, FakeMessage, FakeTestHostFixture>> responses)
    {
        _responses = responses;
        return this;
    }
}
