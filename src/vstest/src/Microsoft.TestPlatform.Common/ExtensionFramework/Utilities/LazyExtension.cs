// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionDecorators;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

/// <summary>
/// Class to hold a test extension type
/// </summary>
/// <typeparam name="TExtension">Test extension type</typeparam>
/// <typeparam name="TMetadata">Test extension metadata</typeparam>
public class LazyExtension<TExtension, TMetadata>
{
    private static readonly object Synclock = new();
    private readonly Type? _metadataType;
    private readonly Func<TExtension>? _extensionCreator;
    private readonly ExtensionDecoratorFactory _extensionDecoratorFactory = new(FeatureFlag.Instance);
    private TExtension? _extension;
    private TMetadata? _metadata;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="instance">Test extension Instance</param>
    /// <param name="metadata">test extension metadata</param>
    public LazyExtension(TExtension instance, TMetadata metadata)
    {
        _extension = instance ?? throw new ArgumentNullException(nameof(instance));
        _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        IsExtensionCreated = true;
    }

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="pluginInfo">Test plugin to instantiated on demand.</param>
    /// <param name="metadataType">Metadata type to instantiate on demand</param>
    public LazyExtension(TestPluginInformation pluginInfo, Type metadataType)
    {
        TestPluginInfo = pluginInfo ?? throw new ArgumentNullException(nameof(pluginInfo));
        _metadataType = metadataType ?? throw new ArgumentNullException(nameof(metadataType));
        IsExtensionCreated = false;
    }

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="pluginInfo">Test plugin to instantiated on demand</param>
    /// <param name="metadata">Test extension metadata</param>
    public LazyExtension(TestPluginInformation pluginInfo, TMetadata metadata)
    {
        TestPluginInfo = pluginInfo ?? throw new ArgumentNullException(nameof(pluginInfo));
        _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        IsExtensionCreated = false;
    }

    /// <summary>
    /// Delegate Constructor
    /// </summary>
    /// <param name="creator">Test extension creator delegate</param>
    /// <param name="metadata">test extension metadata</param>
    public LazyExtension(Func<TExtension> creator, TMetadata metadata)
    {
        _extensionCreator = creator ?? throw new ArgumentNullException(nameof(creator));
        _metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        IsExtensionCreated = false;
    }

    /// <summary>
    /// Gets a value indicating whether is extension created.
    /// </summary>
    internal bool IsExtensionCreated { get; private set; }

    internal TestPluginInformation? TestPluginInfo { get; }

    /// <summary>
    /// Gets the test extension instance.
    /// </summary>
    public TExtension Value
    {
        get
        {
            if (!IsExtensionCreated)
            {
                if (_extensionCreator != null)
                {
                    _extension = _extensionCreator();
                }
                else if (_extension == null)
                {
                    lock (Synclock)
                    {
                        if (_extension == null && TestPluginInfo != null)
                        {
                            TPDebug.Assert(TestPluginInfo.AssemblyQualifiedName is not null, "TestPluginInfo.AssemblyQualifiedName is null");
                            var pluginType = TestPluginManager.GetTestExtensionType(TestPluginInfo.AssemblyQualifiedName);
                            TPDebug.Assert(pluginType is not null, "pluginType is null");

                            // If the extension is a test executor we decorate the adapter to augment the test platform capabilities.
                            var extension = TestPluginManager.CreateTestExtension<TExtension>(pluginType);
                            if (typeof(ITestExecutor).IsAssignableFrom(typeof(TExtension)))
                            {
                                extension = (TExtension)_extensionDecoratorFactory.Decorate((ITestExecutor)extension!);
                            }

                            _extension = extension;
                        }
                    }
                }

                IsExtensionCreated = true;
            }

            TPDebug.Assert(_extension is not null, "_extension is null");
            return _extension;
        }
    }

    /// <summary>
    /// Gets the test extension metadata
    /// </summary>
    public TMetadata Metadata
    {
        get
        {
            if (_metadata == null)
            {
                lock (Synclock)
                {
                    if (_metadata == null && TestPluginInfo != null)
                    {
                        var parameters = TestPluginInfo.Metadata?.ToArray();
                        TPDebug.Assert(_metadataType is not null, "_metadataType is null");
                        var dataObject = Activator.CreateInstance(_metadataType, parameters);
                        TPDebug.Assert(dataObject is not null, "dataObject is null");
                        _metadata = (TMetadata)dataObject;
                    }
                }
            }

            TPDebug.Assert(_metadata is not null, "_metadata is null");
            return _metadata;
        }
    }

}
