// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.UnitTests.EventHandlers;

/// <summary>
/// Regression tests for PathConverter attachment handling.
/// </summary>
[TestClass]
public class PathConverterAttachmentRegressionTests
{
    private readonly PathConverter _pathConverter;

    public PathConverterAttachmentRegressionTests()
    {
        _pathConverter = new PathConverter(
            @"C:\Remote\Project",
            @"C:\Local\Deploy",
            new FileHelper());
    }

    // Regression test for #3367 — PathConverter does not convert uris
    [TestMethod]
    public void UpdateAttachmentSets_Collection_ShouldNotThrow()
    {
        var attachments = new Collection<AttachmentSet>
        {
            new(new Uri("datacollector://Microsoft/TestPlatform/Coverage"), "Coverage")
        };

        // Should not throw
        _pathConverter.UpdateAttachmentSets(attachments, PathConversionDirection.Receive);
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdateAttachmentSets_ICollection_ShouldNotThrow()
    {
        ICollection<AttachmentSet> attachments = new List<AttachmentSet>
        {
            new(new Uri("datacollector://Microsoft/TestPlatform/Coverage"), "Coverage")
        };

        // Should not throw
        _pathConverter.UpdateAttachmentSets(attachments, PathConversionDirection.Receive);
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdateAttachmentSets_EmptyCollection_ShouldNotThrow()
    {
        var attachments = new Collection<AttachmentSet>();

        // Should not throw with empty collection
        _pathConverter.UpdateAttachmentSets(attachments, PathConversionDirection.Receive);
    }

    // Regression test for #3367
    [TestMethod]
    public void UpdateTestRunCompleteEventArgs_ShouldHandleEmptyAttachments()
    {
        var attachments = new Collection<AttachmentSet>();
        var args = new TestRunCompleteEventArgs(
            stats: null,
            isCanceled: false,
            isAborted: false,
            error: null,
            attachmentSets: attachments,
            elapsedTime: TimeSpan.Zero);

        // Should not throw
        _pathConverter.UpdateTestRunCompleteEventArgs(args, PathConversionDirection.Send);
    }
}
