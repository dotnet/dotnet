using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.DotNet.Helix.Client
{
    internal class WorkItemDefinition : IWorkItemDefinitionWithCommand,
        IWorkItemDefinitionWithPayload,
        IWorkItemDefinition
    {
        public WorkItemDefinition(JobDefinition jobDefinition, string workItemName)
        {
            JobDefinition = jobDefinition;
            WorkItemName = workItemName;
            Timeout = DefaultTimeout;
        }

        public static TimeSpan DefaultTimeout { get; } = TimeSpan.FromSeconds(300);
        public JobDefinition JobDefinition { get; }
        public string Command { get; private set; }
        public string WorkItemName { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public IPayload Payload { get; private set; }

        public IWorkItemDefinitionWithPayload WithCommand(string command)
        {
            Command = command;
            return this;
        }

        public IWorkItemDefinition WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public IJobDefinition AttachToJob()
        {
            JobDefinition.AddWorkItem(this);
            return JobDefinition;
        }

        public IWorkItemDefinition WithPayloadUri(Uri payloadUri)
        {
            Payload = new UriPayload(payloadUri);
            return this;
        }

        public IWorkItemDefinition WithFiles(params string[] files)
        {
            Payload = new AdhocPayload(files);
            return this;
        }

        public IWorkItemDefinition WithDirectoryPayload(string directory)
        {
            return WithDirectoryPayload(directory, false);
        }

        public IWorkItemDefinition WithDirectoryPayload(string directory, bool includeDirectoryName)
        {
            string archiveEntryPrefix = null;
            if (includeDirectoryName)
            {
                archiveEntryPrefix = new DirectoryInfo(directory).Name;
            }
            return WithDirectoryPayload(directory, archiveEntryPrefix);
        }

        public IWorkItemDefinition WithDirectoryPayload(string directory, string archiveEntryPrefix)
        {
            Payload = new DirectoryPayload(directory, archiveEntryPrefix);
            return this;
        }

        public IWorkItemDefinition WithArchivePayload(string archive)
        {
            Payload = new ArchivePayload(archive);
            return this;
        }

        public IWorkItemDefinition WithSingleFilePayload(string name, string content)
        {
            Payload = new SingleFilePayload(name, content);
            return this;
        }

        public IWorkItemDefinition WithSingleFilePayload(string name, string content, Encoding encoding)
        {
            Payload = new SingleFilePayload(name, content, encoding);
            return this;
        }

        public IWorkItemDefinition WithSingleFilePayload(string name, byte[] content)
        {
            Payload = new SingleFilePayload(name, content);
            return this;
        }

        public IWorkItemDefinition WithEmptyPayload()
        {
            Payload = EmptyPayload.Instance;
            return this;
        }

        public async Task<JobListEntry> SendAsync(
            IBlobContainer payloadStorage,
            string containerName,
            Action<string> log,
            CancellationToken cancellationToken)
        {
            return new JobListEntry
            {
                WorkItemId = WorkItemName,
                Command = Command,
                TimeoutInSeconds = (int) Timeout.TotalSeconds,
                PayloadUri = await Payload.UploadAsync(payloadStorage, log, cancellationToken),
            };
        }
    }
}
