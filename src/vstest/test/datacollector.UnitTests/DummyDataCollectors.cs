// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollectorUnitTests;

[DataCollectorFriendlyName("CustomDataCollector")]
[DataCollectorTypeUri("my://custom/datacollector")]
public class CustomDataCollector : ObjectModel.DataCollection.DataCollector, ITestExecutionEnvironmentSpecifier
{
    public DataCollectionEnvironmentContext? DataCollectionEnvironmentContext { get; set; }

    public DataCollectionSink? DataSink { get; set; }

    public override void Initialize(
        XmlElement? configurationElement,
        DataCollectionEvents events,
        DataCollectionSink dataSink,
        DataCollectionLogger logger,
        DataCollectionEnvironmentContext? environmentContext)
    {
    }

    public IEnumerable<KeyValuePair<string, string>> GetTestExecutionEnvironmentVariables()
    {
        return default!;
    }
}

[DataCollectorFriendlyName("CustomDataCollector")]
public class CustomDataCollectorWithoutUri : ObjectModel.DataCollection.DataCollector
{
    public override void Initialize(
        XmlElement? configurationElement,
        DataCollectionEvents events,
        DataCollectionSink dataSink,
        DataCollectionLogger logger,
        DataCollectionEnvironmentContext? environmentContext)
    {
    }
}

[DataCollectorTypeUri("my://custom/datacollector")]
public class CustomDataCollectorWithoutFriendlyName : ObjectModel.DataCollection.DataCollector
{
    public override void Initialize(
        XmlElement? configurationElement,
        DataCollectionEvents events,
        DataCollectionSink dataSink,
        DataCollectionLogger logger,
        DataCollectionEnvironmentContext? environmentContext)
    {
    }
}

[DataCollectorFriendlyName("")]
[DataCollectorTypeUri("my://custom/datacollector")]
public class CustomDataCollectorWithEmptyFriendlyName : ObjectModel.DataCollection.DataCollector
{
    public override void Initialize(
        XmlElement? configurationElement,
        DataCollectionEvents events,
        DataCollectionSink dataSink,
        DataCollectionLogger logger,
        DataCollectionEnvironmentContext? environmentContext)
    {
    }
}
