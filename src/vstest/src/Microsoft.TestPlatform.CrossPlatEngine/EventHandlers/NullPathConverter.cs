// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

internal class NullPathConverter : IPathConverter
{
    private static readonly Lazy<NullPathConverter> LazyInstance = new(() => new NullPathConverter());

    private NullPathConverter() { }

    public static NullPathConverter Instance => LazyInstance.Value;

    Collection<AttachmentSet> IPathConverter.UpdateAttachmentSets(Collection<AttachmentSet> attachmentSets, PathConversionDirection _) => attachmentSets;

    ICollection<AttachmentSet> IPathConverter.UpdateAttachmentSets(ICollection<AttachmentSet>? attachmentSets, PathConversionDirection _) => attachmentSets!;

    DiscoveryCriteria IPathConverter.UpdateDiscoveryCriteria(DiscoveryCriteria discoveryCriteria, PathConversionDirection _) => discoveryCriteria;

    string? IPathConverter.UpdatePath(string? path, PathConversionDirection _) => path;

    IEnumerable<string> IPathConverter.UpdatePaths(IEnumerable<string> paths, PathConversionDirection _) => paths;

    TestCase IPathConverter.UpdateTestCase(TestCase testCase, PathConversionDirection _) => testCase;

    IEnumerable<TestCase> IPathConverter.UpdateTestCases(IEnumerable<TestCase>? testCases, PathConversionDirection _) => testCases!;

    TestRunChangedEventArgs IPathConverter.UpdateTestRunChangedEventArgs(TestRunChangedEventArgs? testRunChangedArgs, PathConversionDirection _) => testRunChangedArgs!;

    TestRunCompleteEventArgs IPathConverter.UpdateTestRunCompleteEventArgs(TestRunCompleteEventArgs testRunCompleteEventArgs, PathConversionDirection _) => testRunCompleteEventArgs;

    TestRunCriteriaWithSources IPathConverter.UpdateTestRunCriteriaWithSources(TestRunCriteriaWithSources testRunCriteriaWithSources, PathConversionDirection _) => testRunCriteriaWithSources;

    TestRunCriteriaWithTests IPathConverter.UpdateTestRunCriteriaWithTests(TestRunCriteriaWithTests testRunCriteriaWithTests, PathConversionDirection _) => testRunCriteriaWithTests;
}
