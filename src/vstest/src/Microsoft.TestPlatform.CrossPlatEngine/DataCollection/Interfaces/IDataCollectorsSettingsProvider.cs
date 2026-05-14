// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.DataCollection.Interfaces;

/// <summary>
/// The DataCollectorsSettingsProvider interface.
/// </summary>
public interface IDataCollectorsSettingsProvider : ISettingsProvider
{
    /// <summary>
    /// Gets run specific data collection settings.
    /// </summary>
    DataCollectionRunSettings Settings { get; }
}
