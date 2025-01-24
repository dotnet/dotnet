// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Immutable;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Composition;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.CodeAnalysis.Remote.Razor;

// Inspired by https://github.com/dotnet/roslyn/blob/25aa74d725e801b8232dbb3e5abcda0fa72da8c5/src/Workspaces/Remote/ServiceHub/Host/RemoteWorkspaceManager.cs#L77

internal sealed class RemoteMefComposition
{
    public static readonly ImmutableArray<Assembly> Assemblies = [typeof(RemoteMefComposition).Assembly];

    private static readonly AsyncLazy<CompositionConfiguration> s_lazyConfiguration = new(
        static () => CreateConfigurationAsync(CancellationToken.None),
        joinableTaskFactory: null);

    private static readonly AsyncLazy<ExportProvider> s_lazyExportProvider = new(
        static () => CreateExportProviderAsync(CancellationToken.None),
        joinableTaskFactory: null);

    /// <summary>
    ///  Gets a <see cref="CompositionConfiguration"/> built from <see cref="Assemblies"/>. Note that the
    ///  same <see cref="CompositionConfiguration"/> instance is returned for subsequent calls to this method.
    /// </summary>
    public static Task<CompositionConfiguration> GetConfigurationAsync(CancellationToken cancellationToken)
        => s_lazyConfiguration.GetValueAsync(cancellationToken);

    /// <summary>
    ///  Gets an <see cref="ExportProvider"/> for the shared MEF composition. Note that the
    ///  same <see cref="ExportProvider"/> instance is returned for subsequent calls to this method.
    /// </summary>
    public static Task<ExportProvider> GetSharedExportProviderAsync(CancellationToken cancellationToken)
        => s_lazyExportProvider.GetValueAsync(cancellationToken);

    private static async Task<CompositionConfiguration> CreateConfigurationAsync(CancellationToken cancellationToken)
    {
        var resolver = new Resolver(SimpleAssemblyLoader.Instance);
        var discovery = new AttributedPartDiscovery(resolver, isNonPublicSupported: true); // MEFv2 only
        var parts = await discovery.CreatePartsAsync(Assemblies, cancellationToken: cancellationToken).ConfigureAwait(false);
        var catalog = ComposableCatalog.Create(resolver).AddParts(parts);

        return CompositionConfiguration.Create(catalog).ThrowOnErrors();
    }

    /// <summary>
    ///  Creates a new MEF composition and returns an <see cref="ExportProvider"/>. The catalog and configuration
    ///  are reused for subsequent calls to this method.
    /// </summary>
    public static async Task<ExportProvider> CreateExportProviderAsync(CancellationToken cancellationToken)
    {
        var configuration = await s_lazyConfiguration.GetValueAsync(cancellationToken).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();

        var runtimeComposition = RuntimeComposition.CreateRuntimeComposition(configuration);
        var exportProviderFactory = runtimeComposition.CreateExportProviderFactory();

        return exportProviderFactory.CreateExportProvider();
    }

    private sealed class SimpleAssemblyLoader : IAssemblyLoader
    {
        public static readonly IAssemblyLoader Instance = new SimpleAssemblyLoader();

        public Assembly LoadAssembly(AssemblyName assemblyName)
            => Assembly.Load(assemblyName);

        public Assembly LoadAssembly(string assemblyFullName, string? codeBasePath)
        {
            var assemblyName = new AssemblyName(assemblyFullName);
            if (!string.IsNullOrEmpty(codeBasePath))
            {
#pragma warning disable SYSLIB0044 // https://github.com/dotnet/roslyn/issues/71510
                assemblyName.CodeBase = codeBasePath;
#pragma warning restore SYSLIB0044
            }

            return LoadAssembly(assemblyName);
        }
    }
}
