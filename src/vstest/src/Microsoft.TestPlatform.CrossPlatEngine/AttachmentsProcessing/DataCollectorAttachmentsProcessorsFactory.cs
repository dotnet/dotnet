// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector;
using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;
using Microsoft.VisualStudio.TestPlatform.Common.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;

internal class DataCollectorAttachmentsProcessorsFactory : IDataCollectorAttachmentsProcessorsFactory
{
    private const string CoverageFriendlyName = "Code Coverage";
    private static readonly ConcurrentDictionary<string, DataCollectorExtensionManager> DataCollectorExtensionManagerCache = new();

    public DataCollectorAttachmentProcessor[] Create(InvokedDataCollector[]? invokedDataCollectors, IMessageLogger? logger)
    {
        IDictionary<string, Tuple<string, IDataCollectorAttachmentProcessor>> datacollectorsAttachmentsProcessors = new Dictionary<string, Tuple<string, IDataCollectorAttachmentProcessor>>();

        if (invokedDataCollectors?.Length > 0)
        {
            // We order files by filename descending so in case of the same collector from the same nuget but with different versions, we'll run the newer version.
            // i.e. C:\Users\xxx\.nuget\packages\coverlet.collector
            // /3.0.2
            // /3.0.3
            // /3.1.0
            foreach (var invokedDataCollector in invokedDataCollectors.OrderByDescending(d => d.FilePath))
            {
                // We'll merge using only one AQN in case of more "same processors" in different assembly.
                if (!invokedDataCollector.HasAttachmentProcessor)
                {
                    continue;
                }

                EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: Analyzing data collector attachment processor Uri: {invokedDataCollector.Uri} AssemblyQualifiedName: {invokedDataCollector.AssemblyQualifiedName} FilePath: {invokedDataCollector.FilePath} HasAttachmentProcessor: {invokedDataCollector.HasAttachmentProcessor}");

                var canUseAppDomains =
#if NETFRAMEWORK
                    true;
#else
                    false;
#endif

                // If we're in design mode we need to load the extension inside a different AppDomain to avoid to lock extension file containers.
                if (canUseAppDomains && RunSettingsHelper.Instance.IsDesignMode)
                {
#if NETFRAMEWORK
                    try
                    {
                        var wrapper = new DataCollectorAttachmentProcessorAppDomain(invokedDataCollector, logger);
                        if (wrapper.LoadSucceded && wrapper.HasAttachmentProcessor)
                        {
                            if (!datacollectorsAttachmentsProcessors.ContainsKey(wrapper.AssemblyQualifiedName!))
                            {
                                datacollectorsAttachmentsProcessors.Add(wrapper.AssemblyQualifiedName!, new Tuple<string, IDataCollectorAttachmentProcessor>(wrapper.FriendlyName!, wrapper));
                                EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: Collector attachment processor '{wrapper.AssemblyQualifiedName}' from file '{invokedDataCollector.FilePath}' added to the 'run list'");
                            }
                            else
                            {
                                // If we already registered same IDataCollectorAttachmentProcessor we need to unload the unused AppDomain.
                                EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: Unloading unused AppDomain for '{wrapper.FriendlyName}'");
                                wrapper.Dispose();
                            }
                        }
                        else
                        {
                            EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: DataCollectorExtension not found for uri '{invokedDataCollector.Uri}'");
                        }
                    }
                    catch (Exception ex)
                    {
                        EqtTrace.Error($"DataCollectorAttachmentsProcessorsFactory: Failed during the creation of data collector attachment processor '{invokedDataCollector.AssemblyQualifiedName}'\n{ex}");
                        logger?.SendMessage(TestMessageLevel.Error, $"DataCollectorAttachmentsProcessorsFactory: Failed during the creation of data collector attachment processor '{invokedDataCollector.AssemblyQualifiedName}'\n{ex}");
                    }
#endif
                }
                else
                {
                    // We cache extension locally by file path
                    var dataCollectorExtensionManager = DataCollectorExtensionManagerCache.GetOrAdd(invokedDataCollector.FilePath, DataCollectorExtensionManager.Create(invokedDataCollector.FilePath, true, TestSessionMessageLogger.Instance));
                    var dataCollectorExtension = dataCollectorExtensionManager.TryGetTestExtension(invokedDataCollector.Uri);
                    if ((dataCollectorExtension?.Metadata.HasAttachmentProcessor) != true)
                    {
                        EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: DataCollectorExtension not found for uri '{invokedDataCollector.Uri}'");
                        continue;
                    }

                    TPDebug.Assert(dataCollectorExtension.TestPluginInfo is not null, "dataCollectorExtension.TestPluginInfo is null");
                    Type attachmentProcessorType = ((DataCollectorConfig)dataCollectorExtension.TestPluginInfo!).AttachmentsProcessorType!;
                    IDataCollectorAttachmentProcessor? dataCollectorAttachmentProcessorInstance = null;
                    try
                    {
                        dataCollectorAttachmentProcessorInstance = TestPluginManager.CreateTestExtension<IDataCollectorAttachmentProcessor>(attachmentProcessorType);
                        EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: Creation of collector attachment processor '{attachmentProcessorType.AssemblyQualifiedName}' from file '{invokedDataCollector.FilePath}' succeded");
                    }
                    catch (Exception ex)
                    {
                        EqtTrace.Error($"DataCollectorAttachmentsProcessorsFactory: Failed during the creation of data collector attachment processor '{attachmentProcessorType.AssemblyQualifiedName}'\n{ex}");
                        logger?.SendMessage(TestMessageLevel.Error, $"DataCollectorAttachmentsProcessorsFactory: Failed during the creation of data collector attachment processor '{attachmentProcessorType.AssemblyQualifiedName}'\n{ex}");
                    }

                    var attachmentQualifiedName = attachmentProcessorType.AssemblyQualifiedName;
                    TPDebug.Assert(attachmentQualifiedName is not null, "attachmentQualifiedName is null");
                    if (dataCollectorAttachmentProcessorInstance is not null && !datacollectorsAttachmentsProcessors.ContainsKey(attachmentQualifiedName))
                    {
                        datacollectorsAttachmentsProcessors.Add(attachmentQualifiedName, new Tuple<string, IDataCollectorAttachmentProcessor>(dataCollectorExtension.Metadata.FriendlyName, dataCollectorAttachmentProcessorInstance));
                        EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: Collector attachment processor '{attachmentProcessorType.AssemblyQualifiedName}' from file '{invokedDataCollector.FilePath}' added to the 'run list'");
                    }
                }
            }
        }

        // We provide the implementation of CodeCoverageDataAttachmentsHandler through nuget package, but in case of absent registration or if for some reason
        // the attachment processor from package fails we fallback to the default implementation.
        if (!datacollectorsAttachmentsProcessors.ContainsKey(typeof(CodeCoverageDataAttachmentsHandler).AssemblyQualifiedName!))
        {
            datacollectorsAttachmentsProcessors.Add(typeof(CodeCoverageDataAttachmentsHandler).AssemblyQualifiedName!, new Tuple<string, IDataCollectorAttachmentProcessor>(CoverageFriendlyName, new CodeCoverageDataAttachmentsHandler()));
            EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: Collector attachment processor '{typeof(CodeCoverageDataAttachmentsHandler).AssemblyQualifiedName}' for the data collector with friendly name '{CoverageFriendlyName}' added to the 'run list'");
        }

        var finalDatacollectorsAttachmentsProcessors = new List<DataCollectorAttachmentProcessor>();
        foreach (var attachementProcessor in datacollectorsAttachmentsProcessors)
        {
            EqtTrace.Info($"DataCollectorAttachmentsProcessorsFactory: Valid data collector attachment processor found: '{attachementProcessor.Key}'");
            finalDatacollectorsAttachmentsProcessors.Add(new DataCollectorAttachmentProcessor(attachementProcessor.Value.Item1, attachementProcessor.Value.Item2));
        }

        return finalDatacollectorsAttachmentsProcessors.ToArray();
    }
}
