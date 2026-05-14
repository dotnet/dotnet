// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Microsoft.VisualStudio.TestPlatform.Common.DataCollector;

/// <summary>
/// Class used by data collectors to send messages to the client
/// </summary>
internal class TestPlatformDataCollectionLogger : DataCollectionLogger
{
    /// <summary>
    /// DataCollector's config info.
    /// </summary>
    private readonly DataCollectorConfig _dataCollectorConfig;

    /// <summary>
    /// Message sink.
    /// </summary>
    private readonly IMessageSink _sink;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestPlatformDataCollectionLogger"/> class.
    /// </summary>
    /// <param name="sink">
    /// The underlying raw IMessageSink.  Cannot be null.
    /// </param>
    /// <param name="dataCollectorConfig">
    /// The data Collector Information.
    /// </param>
    internal TestPlatformDataCollectionLogger(IMessageSink sink, DataCollectorConfig dataCollectorConfig)
    {
        _sink = sink ?? throw new ArgumentNullException(nameof(sink));
        _dataCollectorConfig = dataCollectorConfig ?? throw new ArgumentNullException(nameof(dataCollectorConfig));
    }

    /// <inheritdoc/>
    public override void LogError(DataCollectionContext context, string text)
    {
        ValidateArg.NotNull(context, nameof(context));
        ValidateArg.NotNull(text, nameof(text));

        EqtTrace.Error(
            "Data collector '{0}' logged the following error: {1}",
            _dataCollectorConfig.TypeUri,
            text);

        SendTextMessage(context, text, TestMessageLevel.Error);
    }

    /// <inheritdoc/>
    public override void LogError(DataCollectionContext context, string text, Exception exception)
    {
        ValidateArg.NotNull(context, nameof(context));
        ValidateArg.NotNull(text, nameof(text));
        ValidateArg.NotNull(exception, nameof(exception));

        // Make sure the data collection context is not a derived data collection context.  This
        // is done to safeguard from 3rd parties creating their own data collection contexts.
        if (context.GetType() != typeof(DataCollectionContext))
        {
            throw new InvalidOperationException(Resources.Resources.WrongDataCollectionContextType);
        }

        EqtTrace.Error(
            "Data collector '{0}' logged the following error:" + Environment.NewLine +
            "Description:            {1}" + Environment.NewLine +
            "Exception type:         {2}" + Environment.NewLine + "Exception message:      {3}"
            + Environment.NewLine + "Exception stack trace:  {4}",
            _dataCollectorConfig.TypeUri,
            text,
            exception.GetType(),
            exception.Message,
            exception.StackTrace);

        // Currently there is one type of DataCollectionMessage sent across client for all message kind.
        // If required new type can be created for different message type.
        var message = string.Format(
            CultureInfo.CurrentCulture,
            Resources.Resources.ReportDataCollectorException,
            exception.GetType(),
            exception.Message,
            text);
        SendTextMessage(context, message, TestMessageLevel.Error);
    }

    /// <inheritdoc/>
    public override void LogWarning(DataCollectionContext context, string text)
    {
        ValidateArg.NotNull(context, nameof(context));
        ValidateArg.NotNull(text, nameof(text));

        EqtTrace.Warning(
            "Data collector '{0}' logged the following warning: {1}",
            _dataCollectorConfig.TypeUri,
            text);

        SendTextMessage(context, text, TestMessageLevel.Warning);
    }

    /// <summary>
    /// Sends text to message sink.
    /// </summary>
    /// <param name="context">
    /// The context.
    /// </param>
    /// <param name="text">
    /// The text.
    /// </param>
    /// <param name="level">
    /// The level.
    /// </param>
    /// <exception cref="InvalidOperationException">Throws InvalidOperationException.
    /// </exception>
    private void SendTextMessage(DataCollectionContext context, string text, TestMessageLevel level)
    {
        ValidateArg.NotNull(context, nameof(context));
        ValidateArg.NotNull(text, nameof(text));
        TPDebug.Assert(
            level is >= TestMessageLevel.Informational and <= TestMessageLevel.Error,
            "Invalid level: " + level);

        // Make sure the data collection context is not a derived data collection context.  This
        // is done to safeguard from 3rd parties creating their own data collection contexts.
        if (context.GetType() != typeof(DataCollectionContext))
        {
            throw new InvalidOperationException(Resources.Resources.WrongDataCollectionContextType);
        }

        var args = new DataCollectionMessageEventArgs(level, text);
        args.Uri = _dataCollectorConfig.TypeUri;
        args.FriendlyName = _dataCollectorConfig.FriendlyName;
        if (context.HasTestCase)
        {
            args.TestCaseId = context.TestExecId.Id;
        }

        _sink.SendMessage(args);
    }
}
