// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

internal interface IPathConverter
{
    string? UpdatePath(string? path, PathConversionDirection updateDirection);

    IEnumerable<string> UpdatePaths(IEnumerable<string> paths, PathConversionDirection updateDirection);

    TestCase UpdateTestCase(TestCase testCase, PathConversionDirection updateDirection);

    IEnumerable<TestCase> UpdateTestCases(IEnumerable<TestCase>? testCases, PathConversionDirection updateDirection);

    TestRunCompleteEventArgs UpdateTestRunCompleteEventArgs(TestRunCompleteEventArgs testRunCompleteEventArgs, PathConversionDirection updateDirection);

    TestRunChangedEventArgs UpdateTestRunChangedEventArgs(TestRunChangedEventArgs? testRunChangedArgs, PathConversionDirection updateDirection);

    Collection<AttachmentSet> UpdateAttachmentSets(Collection<AttachmentSet> attachmentSets, PathConversionDirection updateDirection);

    ICollection<AttachmentSet> UpdateAttachmentSets(ICollection<AttachmentSet>? attachmentSets, PathConversionDirection updateDirection);

    DiscoveryCriteria UpdateDiscoveryCriteria(DiscoveryCriteria discoveryCriteria, PathConversionDirection updateDirection);

    TestRunCriteriaWithSources UpdateTestRunCriteriaWithSources(TestRunCriteriaWithSources testRunCriteriaWithSources, PathConversionDirection updateDirection);

    TestRunCriteriaWithTests UpdateTestRunCriteriaWithTests(TestRunCriteriaWithTests testRunCriteriaWithTests, PathConversionDirection updateDirection);
}
