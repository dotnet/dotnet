// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;

/// <summary>
/// Expose methods to be used by data collection process to send messages to Test Platform Client.
/// </summary>
internal interface IMessageSink
{
    /// <summary>
    /// Data collection message as sent by DataCollectionLogger.
    /// </summary>
    /// <param name="args">Data collection message event args.</param>
    void SendMessage(DataCollectionMessageEventArgs args);
}
