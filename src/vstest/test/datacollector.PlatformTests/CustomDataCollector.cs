// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.DataCollector.PlatformTests;

[DataCollectorFriendlyName("CustomDataCollector")]
[DataCollectorTypeUri("my://custom/datacollector")]
public class CustomDataCollector : ObjectModel.DataCollection.DataCollector, ITestExecutionEnvironmentSpecifier
{
    private DataCollectionSink? _dataCollectionSink;
    private DataCollectionEnvironmentContext? _context;
    private DataCollectionLogger? _logger;

    public override void Initialize(
        XmlElement? configurationElement,
        DataCollectionEvents events,
        DataCollectionSink dataSink,
        DataCollectionLogger logger,
        DataCollectionEnvironmentContext? environmentContext)
    {
        _dataCollectionSink = dataSink;
        _context = environmentContext;
        _logger = logger;
        events.SessionStart += SessionStarted_Handler;
        events.SessionEnd += SessionEnded_Handler;
    }

    private void SessionStarted_Handler(object? sender, SessionStartEventArgs args)
    {
        var filename = Path.Combine(Path.GetTempPath(), "filename.txt");
        File.WriteAllText(filename, string.Empty);
        _dataCollectionSink!.SendFileAsync(_context!.SessionDataCollectionContext, filename, true);
        _logger!.LogWarning(_context.SessionDataCollectionContext, "SessionEnded");
    }

    private void SessionEnded_Handler(object? sender, SessionEndEventArgs args)
    {
        //logger.LogError(this.context.SessionDataCollectionContext, new Exception("my exception"));
        //logger.LogWarning(this.context.SessionDataCollectionContext, "my arning");
        //logger.LogException(context.SessionDataCollectionContext, new Exception("abc"), DataCollectorMessageLevel.Error);

        _logger!.LogWarning(_context!.SessionDataCollectionContext, "SessionEnded");
    }

    public IEnumerable<KeyValuePair<string, string>> GetTestExecutionEnvironmentVariables()
    {
        return new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("key", "value") };
    }
}
