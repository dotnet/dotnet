// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

/// <summary>
/// The blame logger.
/// </summary>
[FriendlyName(FriendlyName)]
[ExtensionUri(ExtensionUri)]
public class BlameLogger : ITestLogger
{
    /// <summary>
    /// Uri used to uniquely identify the Blame logger.
    /// </summary>
    public const string ExtensionUri = "logger://Microsoft/TestPlatform/Extensions/Blame/v1";

    /// <summary>
    /// Alternate user friendly string to uniquely identify the Blame logger.
    /// </summary>
    public const string FriendlyName = "Blame";

    /// <summary>
    /// The blame reader writer.
    /// </summary>
    private readonly IBlameReaderWriter _blameReaderWriter;

    /// <summary>
    /// The output.
    /// </summary>
    private readonly IOutput _output;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlameLogger"/> class.
    /// </summary>
    public BlameLogger()
        : this(ConsoleOutput.Instance, new XmlReaderWriter())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlameLogger"/> class.
    /// Constructor added for testing purpose
    /// </summary>
    /// <param name="output">Output Instance</param>
    /// <param name="blameReaderWriter">BlameReaderWriter Instance</param>
    protected BlameLogger(IOutput output, IBlameReaderWriter blameReaderWriter)
    {
        _output = output;
        _blameReaderWriter = blameReaderWriter;
    }


    #region ITestLogger

    /// <summary>
    /// Initializes the Logger.
    /// </summary>
    /// <param name="events">Events that can be registered for.</param>
    /// <param name="testRunDictionary">Test Run Directory</param>
    public void Initialize(TestLoggerEvents events, string? testRunDictionary)
    {
        ValidateArg.NotNull(events, nameof(events));
        events.TestRunComplete += TestRunCompleteHandler;
    }

    /// <summary>
    /// Called when a test run is complete.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">TestRunCompleteEventArgs</param>
    private void TestRunCompleteHandler(object? sender, TestRunCompleteEventArgs e)
    {
        ValidateArg.NotNull(sender, nameof(sender));
        ValidateArg.NotNull(e, nameof(e));

        if (!e.IsAborted)
        {
            return;
        }

        _output.WriteLine(string.Empty, OutputLevel.Information);

        // Gets the faulty test cases if test aborted
        var testCaseNames = GetFaultyTestCaseNames(e);
        if (!testCaseNames.Any())
        {
            return;
        }

        var sb = new StringBuilder();
        foreach (var tcn in testCaseNames)
        {
            sb.Append(tcn).Append(Environment.NewLine);
        }

        _output.Error(false, Resources.Resources.AbortedTestRun, sb.ToString());
    }

    #endregion

    #region Faulty test case fetch

    /// <summary>
    /// Fetches faulty test case
    /// </summary>
    /// <param name="e">
    /// The TestRunCompleteEventArgs.
    /// </param>
    /// <returns>
    /// Faulty test cases name
    /// </returns>
    private IEnumerable<string> GetFaultyTestCaseNames(TestRunCompleteEventArgs e)
    {
        var faultyTestCaseNames = new List<string>();
        foreach (var attachmentSet in e.AttachmentSets)
        {
            if (!attachmentSet.DisplayName.Equals(Constants.BlameDataCollectorName))
            {
                continue;
            }

            // Process only Sequence_<GUID>.xml attachments
            var uriDataAttachment = attachmentSet.Attachments.LastOrDefault((attachment) => attachment.Uri.ToString().EndsWith(".xml"));

            if (uriDataAttachment == null)
            {
                continue;
            }

            var filepath = uriDataAttachment.Uri.LocalPath;
            var testCaseList = _blameReaderWriter.ReadTestSequence(filepath);
            if (testCaseList.Count > 0)
            {
                var testcases = testCaseList.Where(t => !t.IsCompleted).Select(t => t.FullyQualifiedName!).ToList();
                faultyTestCaseNames.AddRange(testcases);
            }
        }

        return faultyTestCaseNames;
    }

    #endregion
}
