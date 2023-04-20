// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.AspNetCore.Razor.ProjectEngineHost.Serialization;
using Microsoft.AspNetCore.Razor.Telemetry;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Razor;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;
using Microsoft.ServiceHub.Framework;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal abstract class RazorServiceBase : IDisposable
{
    protected readonly ServiceBrokerClient ServiceBrokerClient;

    public RazorServiceBase(IServiceBroker serviceBroker, ITelemetryReporter telemetryReporter)
    {
        RazorServices = new RazorServices(telemetryReporter);

#pragma warning disable VSTHRD012 // Provide JoinableTaskFactory where allowed
        ServiceBrokerClient = new ServiceBrokerClient(serviceBroker);
#pragma warning restore
    }

    protected RazorServices RazorServices { get; }

    public void Dispose()
    {
        ServiceBrokerClient.Dispose();
    }

    protected virtual Task<IProjectSnapshot> GetProjectSnapshotAsync(ProjectSnapshotHandle projectHandle, CancellationToken cancellationToken)
    {
        if (projectHandle is null)
        {
            throw new ArgumentNullException(nameof(projectHandle));
        }

        var snapshot = new SerializedProjectSnapshot(projectHandle.FilePath, projectHandle.Configuration, projectHandle.RootNamespace);
        return Task.FromResult<IProjectSnapshot>(snapshot);
    }

    private class SerializedProjectSnapshot : IProjectSnapshot
    {
        public SerializedProjectSnapshot(string filePath, RazorConfiguration? configuration, string? rootNamespace)
        {
            FilePath = filePath;
            Configuration = configuration;
            RootNamespace = rootNamespace;

            Version = VersionStamp.Default;
        }

        public RazorConfiguration? Configuration { get; }

        public IEnumerable<string> DocumentFilePaths => Array.Empty<string>();

        public string FilePath { get; }

        public string? RootNamespace { get; }

        public VersionStamp Version { get; }

        public LanguageVersion CSharpLanguageVersion => LanguageVersion.Default;

        public IReadOnlyList<TagHelperDescriptor> TagHelpers => Array.Empty<TagHelperDescriptor>();

        public ProjectWorkspaceState? ProjectWorkspaceState => null;

        public IDocumentSnapshot? GetDocument(string filePath)
        {
            if (filePath is null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return null;
        }

        public bool IsImportDocument(IDocumentSnapshot document) => throw new NotImplementedException();

        public ImmutableArray<IDocumentSnapshot> GetRelatedDocuments(IDocumentSnapshot document)
            => throw new NotImplementedException();

        public RazorProjectEngine GetProjectEngine()
            => throw new NotImplementedException();
    }
}
