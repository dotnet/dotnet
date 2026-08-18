// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.DotNet.Helix.Client;
using Moq;
using Xunit;

namespace Microsoft.DotNet.Helix.JobSender.Test
{
    public class SendAsyncCallbackRoutingTests
    {
        /// <summary>
        /// A legacy <see cref="IJobDefinition"/> implementer that only implements the original
        /// single-callback <c>SendAsync</c> overload and relies on the default interface
        /// implementation for the new two-callback overload. It records the callback and
        /// cancellation token it receives so the delegation can be asserted.
        /// </summary>
        private sealed class LegacySingleCallbackJobDefinition : IJobDefinition
        {
            public Action<string> ReceivedLog { get; private set; }
            public CancellationToken ReceivedToken { get; private set; }
            public int SendInvocations { get; private set; }
            public ISentJob JobToReturn { get; set; }

            public Task<ISentJob> SendAsync(Action<string> log = null, CancellationToken cancellationToken = default)
            {
                ReceivedLog = log;
                ReceivedToken = cancellationToken;
                SendInvocations++;
                return Task.FromResult(JobToReturn);
            }

            // Remaining members are irrelevant to this test.
            public IWorkItemDefinitionWithCommand DefineWorkItem(string workItemName) => throw new NotImplementedException();
            public IJobDefinition WithCorrelationPayloadUris(params Uri[] payloadUris) => throw new NotImplementedException();
            public IJobDefinition WithCorrelationPayloadUris(IDictionary<Uri, string> payloadUrisWithDestinations) => throw new NotImplementedException();
            public IJobDefinition WithCorrelationPayloadDirectory(string directory, string destination = "") => throw new NotImplementedException();
            public IJobDefinition WithCorrelationPayloadDirectory(string directory, bool includeDirectoryName, string destination = "") => throw new NotImplementedException();
            public IJobDefinition WithCorrelationPayloadDirectory(string directory, string archiveEntryPrefix, string destination) => throw new NotImplementedException();
            public IJobDefinition WithCorrelationPayloadArchive(string archive, string destination = "") => throw new NotImplementedException();
            public IJobDefinition WithCorrelationPayloadFiles(params string[] files) => throw new NotImplementedException();
            public IJobDefinition WithCorrelationPayloadFiles(IList<string> files, string destination) => throw new NotImplementedException();
            public IJobDefinition WithSource(string source) => throw new NotImplementedException();
            public IJobDefinition WithProperty(string key, string value) => throw new NotImplementedException();
            public IJobDefinition WithCreator(string creator) => throw new NotImplementedException();
            public IJobDefinition WithContainerName(string targetContainerName) => throw new NotImplementedException();
            public IJobDefinition WithStorageAccountConnectionString(string accountConnectionString) => throw new NotImplementedException();
            public IJobDefinition WithResultsContainerName(string resultsContainerName) => throw new NotImplementedException();
            public IJobDefinition WithMaxRetryCount(int? maxRetryCount) => throw new NotImplementedException();
            public IJobDefinition WithQueueStats() => throw new NotImplementedException();
        }

        /// <summary>
        /// The two-callback <c>SendAsync</c> overload is a new member on the public
        /// <see cref="IJobDefinition"/> interface. Its default interface implementation must keep
        /// existing external implementers working by delegating to the single-callback overload
        /// (dropping the separate queue-stats callback), forwarding the caller's log and
        /// cancellation token unchanged.
        /// </summary>
        [Fact]
        public async Task DefaultInterfaceImplementation_DelegatesToSingleCallbackOverload()
        {
            var sentJob = new Mock<ISentJob>().Object;
            Action<string> log = _ => { };
            Action<string> queueStatsLog = _ => { };
            using var cts = new CancellationTokenSource();

            var def = new LegacySingleCallbackJobDefinition { JobToReturn = sentJob };

            // Invoke through the interface so the default interface implementation is exercised.
            ISentJob result = await ((IJobDefinition)def).SendAsync(log, queueStatsLog, cts.Token);

            Assert.Equal(1, def.SendInvocations);
            Assert.Same(log, def.ReceivedLog);
            Assert.Equal(cts.Token, def.ReceivedToken);
            Assert.Same(sentJob, result);
        }
    }
}
