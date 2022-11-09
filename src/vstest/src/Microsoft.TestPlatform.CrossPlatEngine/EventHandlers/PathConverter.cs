// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
/// <summary>
/// Converts paths in received and sent objects, to make testhost seem like it run a local test,
/// while it was in fact running a test on a remote system, in a totally different path. This is for UWP which
/// does testhost deployment.
/// The modifications here rely on combination of side-effects, and actually replacing the values, because
/// we cannot modify the properties on our public objects, and add setters.
/// </summary>
internal class PathConverter : IPathConverter
{
    // The path on this computer to which we deployed the test dll and test runner
    private readonly string _deploymentPath = "";
    // The path on the remote system where test dll was originally placed, and from which we
    // copied it to this system. For vstest.console, which is on the other side of this, the names
    // are inverted, it sends us their local path, and thinks about our local path as remote.
    private readonly string _originalPath = "";

    public PathConverter(string originalPath, string deploymentPath, IFileHelper fileHelper)
    {
        ValidateArg.NotNull(originalPath, nameof(originalPath));
        ValidateArg.NotNull(deploymentPath, nameof(deploymentPath));
        ValidateArg.NotNull(fileHelper, nameof(fileHelper));

        string unquotedOriginalPath = originalPath.Trim('\"');
        string normalizedLocalPath = fileHelper.GetFullPath(unquotedOriginalPath).TrimEnd('\\').TrimEnd('/') + Path.DirectorySeparatorChar;
        _originalPath = normalizedLocalPath;

        string unquotedDeploymentPath = deploymentPath.Trim('\"');
        string normalizedDeploymentPath = fileHelper.GetFullPath(unquotedDeploymentPath).TrimEnd('\\').TrimEnd('/') + Path.DirectorySeparatorChar;
        _deploymentPath = normalizedDeploymentPath;
    }

    [return: NotNullIfNotNull("path")]
    public string? UpdatePath(string? path, PathConversionDirection updateDirection)
    {
        if (path == null)
            return null;

        string find;
        string replaceWith;
        if (updateDirection == PathConversionDirection.Receive)
        {
            // Request is incoming, the path that is local to the sender (for us that is "remote" path)
            // needs to be replaced with our path
            find = _originalPath;
            replaceWith = _deploymentPath;
        }
        else
        {
            find = _deploymentPath;
            replaceWith = _originalPath;
        }

        var result = path.Replace(find, replaceWith);
        return result;
    }

    public IEnumerable<string> UpdatePaths(IEnumerable<string> paths, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(paths, nameof(paths));
        return paths.Select(p => UpdatePath(p, updateDirection)).ToList();
    }

    public TestCase UpdateTestCase(TestCase testCase, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(testCase, nameof(testCase));
        testCase.CodeFilePath = UpdatePath(testCase.CodeFilePath, updateDirection);
        testCase.Source = UpdatePath(testCase.Source, updateDirection);
        return testCase;
    }

    public IEnumerable<TestCase> UpdateTestCases(IEnumerable<TestCase>? testCases, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(testCases, nameof(testCases));
        testCases!.ToList().ForEach(tc => UpdateTestCase(tc, updateDirection));
        return testCases!;
    }

    public TestRunCompleteEventArgs UpdateTestRunCompleteEventArgs(TestRunCompleteEventArgs testRunCompleteEventArgs, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(testRunCompleteEventArgs, nameof(testRunCompleteEventArgs));
        UpdateAttachmentSets(testRunCompleteEventArgs.AttachmentSets, updateDirection);
        return testRunCompleteEventArgs;
    }

    public TestRunChangedEventArgs UpdateTestRunChangedEventArgs(TestRunChangedEventArgs? testRunChangedArgs, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(testRunChangedArgs, nameof(testRunChangedArgs));
        UpdateTestResults(testRunChangedArgs!.NewTestResults!, updateDirection);
        UpdateTestCases(testRunChangedArgs.ActiveTests, updateDirection);
        return testRunChangedArgs;
    }

    public Collection<AttachmentSet> UpdateAttachmentSets(Collection<AttachmentSet> attachmentSets, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(attachmentSets, nameof(attachmentSets));
        attachmentSets.ToList().ForEach(i => UpdateAttachmentSet(i, updateDirection));
        return attachmentSets;
    }

    public ICollection<AttachmentSet> UpdateAttachmentSets(ICollection<AttachmentSet>? attachmentSets, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(attachmentSets, nameof(attachmentSets));
        attachmentSets!.ToList().ForEach(i => UpdateAttachmentSet(i, updateDirection));
        return attachmentSets!;
    }

    private static AttachmentSet UpdateAttachmentSet(AttachmentSet attachmentSet, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(attachmentSet, nameof(attachmentSet));
        attachmentSet.Attachments.ToList().ForEach(a => UpdateAttachment(a, updateDirection));
        return attachmentSet;
    }

    private static UriDataAttachment UpdateAttachment(UriDataAttachment attachment, PathConversionDirection _)
    {
        ValidateArg.NotNull(attachment, nameof(attachment));
        // todo: convert uri? https://github.com/microsoft/vstest/issues/3367
        return attachment;
    }

    private IEnumerable<TestResult> UpdateTestResults(IEnumerable<TestResult> testResults, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(testResults, nameof(testResults));
        foreach (var tr in testResults)
        {
            UpdateAttachmentSets(tr.Attachments, updateDirection);
            UpdateTestCase(tr.TestCase, updateDirection);
        }
        return testResults;
    }

    public DiscoveryCriteria UpdateDiscoveryCriteria(DiscoveryCriteria discoveryCriteria, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(discoveryCriteria, nameof(discoveryCriteria));
        discoveryCriteria.Package = UpdatePath(discoveryCriteria.Package, updateDirection);
        foreach (var adapter in discoveryCriteria.AdapterSourceMap.ToList())
        {
            var updatedPaths = UpdatePaths(adapter.Value, updateDirection);
            discoveryCriteria.AdapterSourceMap[adapter.Key] = updatedPaths;
        }
        return discoveryCriteria;
    }

    public TestRunCriteriaWithSources UpdateTestRunCriteriaWithSources(TestRunCriteriaWithSources testRunCriteriaWithSources, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(testRunCriteriaWithSources, nameof(testRunCriteriaWithSources));
        testRunCriteriaWithSources.AdapterSourceMap.ToList().ForEach(adapter =>
            testRunCriteriaWithSources.AdapterSourceMap[adapter.Key] = UpdatePaths(adapter.Value, updateDirection)!);
        var package = UpdatePath(testRunCriteriaWithSources.Package, updateDirection)!;
        return new TestRunCriteriaWithSources(testRunCriteriaWithSources.AdapterSourceMap, package, testRunCriteriaWithSources.RunSettings, testRunCriteriaWithSources.TestExecutionContext);
    }

    public TestRunCriteriaWithTests UpdateTestRunCriteriaWithTests(TestRunCriteriaWithTests testRunCriteriaWithTests, PathConversionDirection updateDirection)
    {
        ValidateArg.NotNull(testRunCriteriaWithTests, nameof(testRunCriteriaWithTests));
        var tests = UpdateTestCases(testRunCriteriaWithTests.Tests, updateDirection);
        var package = UpdatePath(testRunCriteriaWithTests.Package, updateDirection)!;
        return new TestRunCriteriaWithTests(tests, package, testRunCriteriaWithTests.RunSettings, testRunCriteriaWithTests.TestExecutionContext);
    }
}
