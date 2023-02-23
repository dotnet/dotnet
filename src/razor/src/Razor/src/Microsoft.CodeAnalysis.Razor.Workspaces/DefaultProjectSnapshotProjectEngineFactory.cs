// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor;

internal class DefaultProjectSnapshotProjectEngineFactory : ProjectSnapshotProjectEngineFactory
{
    private readonly static RazorConfiguration s_defaultConfiguration = FallbackRazorConfiguration.Latest;

    private readonly IFallbackProjectEngineFactory _fallback;
    private readonly Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>[] _factories;

    public DefaultProjectSnapshotProjectEngineFactory(
        IFallbackProjectEngineFactory fallback,
        Lazy<IProjectEngineFactory, ICustomProjectEngineFactoryMetadata>[] factories)
    {
        if (fallback is null)
        {
            throw new ArgumentNullException(nameof(fallback));
        }

        if (factories is null)
        {
            throw new ArgumentNullException(nameof(factories));
        }

        _fallback = fallback;
        _factories = factories;
    }

#nullable enable
    public override RazorProjectEngine? Create(RazorConfiguration configuration, RazorProjectFileSystem fileSystem, Action<RazorProjectEngineBuilder> configure)
    {
        if (fileSystem is null)
        {
            throw new ArgumentNullException(nameof(fileSystem));
        }

        // When we're running in the editor, the editor provides a configure delegate that will include
        // the editor settings and tag helpers.
        //
        // This service is only used in process in Visual Studio, and any other callers should provide these
        // things also.
        configure ??= ((b) => { });

        // The default configuration currently matches the newest MVC configuration.
        //
        // We typically want this because the language adds features over time - we don't want to a bunch of errors
        // to show up when a document is first opened, and then go away when the configuration loads, we'd prefer the opposite.
        configuration ??= s_defaultConfiguration;

        // If there's no factory to handle the configuration then fall back to a very basic configuration.
        //
        // This will stop a crash from happening in this case (misconfigured project), but will still make
        // it obvious to the user that something is wrong.
        var factory = SelectFactory(configuration) ?? _fallback;
        return factory.Create(configuration, fileSystem, configure);
    }
#nullable disable

    public override IProjectEngineFactory FindFactory(IProjectSnapshot project)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        return SelectFactory(project.Configuration ?? s_defaultConfiguration, requireSerializable: false);
    }

    public override IProjectEngineFactory FindSerializableFactory(IProjectSnapshot project)
    {
        if (project is null)
        {
            throw new ArgumentNullException(nameof(project));
        }

        return SelectFactory(project.Configuration ?? s_defaultConfiguration, requireSerializable: true);
    }

    private IProjectEngineFactory SelectFactory(RazorConfiguration configuration, bool requireSerializable = false)
    {
        for (var i = 0; i < _factories.Length; i++)
        {
            var factory = _factories[i];
            if (string.Equals(configuration.ConfigurationName, factory.Metadata.ConfigurationName, StringComparison.Ordinal))
            {
                return requireSerializable && !factory.Metadata.SupportsSerialization ? null : factory.Value;
            }
        }

        return null;
    }
}
