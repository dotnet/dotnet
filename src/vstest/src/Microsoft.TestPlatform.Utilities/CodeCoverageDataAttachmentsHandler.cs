// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.Utilities;

public class CodeCoverageDataAttachmentsHandler : IDataCollectorAttachmentProcessor
{
    private const string CoverageUri = "datacollector://microsoft/CodeCoverage/2.0";
    private const string CoverageFileExtension = ".coverage";
    private const string XmlFileExtension = ".xml";
    private const string CoverageFriendlyName = "Code Coverage";

    private const string CodeCoverageIoAssemblyName = "Microsoft.CodeCoverage.IO";
    private const string CoverageFileUtilityTypeName = "CoverageFileUtility";
    private const string MergeMethodName = "MergeCoverageReportsAsync";
    private const string CoverageMergeOperationName = "CoverageMergeOperation";

    private static readonly Uri CodeCoverageDataCollectorUri = new(CoverageUri);
    private static Assembly? s_codeCoverageAssembly;
    private static object? s_classInstance;
    private static MethodInfo? s_mergeMethodInfo;
    private static Array? s_mergeOperationEnumValues;

    public bool SupportsIncrementalProcessing => true;

    public IEnumerable<Uri>? GetExtensionUris()
    {
        yield return CodeCoverageDataCollectorUri;
    }

    public async Task<ICollection<AttachmentSet>> ProcessAttachmentSetsAsync(XmlElement configurationElement, ICollection<AttachmentSet>? attachments, IProgress<int> progressReporter, IMessageLogger? logger, CancellationToken cancellationToken)
    {
        if (attachments?.Any() != true)
            return new Collection<AttachmentSet>();

        var coverageReportFilePaths = new List<string>();
        var coverageOtherFilePaths = new List<string>();

        foreach (var attachmentSet in attachments)
        {
            foreach (var attachment in attachmentSet.Attachments)
            {
                if (attachment.Uri.LocalPath.EndsWith(CoverageFileExtension, StringComparison.OrdinalIgnoreCase) ||
                    attachment.Uri.LocalPath.EndsWith(XmlFileExtension, StringComparison.OrdinalIgnoreCase))
                {
                    coverageReportFilePaths.Add(attachment.Uri.LocalPath);
                }
                else
                {
                    coverageOtherFilePaths.Add(attachment.Uri.LocalPath);
                }
            }
        }

        if (coverageReportFilePaths.Count > 1)
        {
            var resultAttachmentSet = new AttachmentSet(CodeCoverageDataCollectorUri, CoverageFriendlyName);

            var mergedCoverageReports = await MergeCodeCoverageFilesAsync(coverageReportFilePaths, progressReporter, cancellationToken).ConfigureAwait(false);
            if (mergedCoverageReports is not null)
            {
                foreach (var coverageReport in mergedCoverageReports)
                {
                    resultAttachmentSet.Attachments.Add(UriDataAttachment.CreateFrom(coverageReport, CoverageFriendlyName));
                }
            }

            foreach (var coverageOtherFilePath in coverageOtherFilePaths)
            {
                resultAttachmentSet.Attachments.Add(UriDataAttachment.CreateFrom(coverageOtherFilePath, string.Empty));
            }

            return new Collection<AttachmentSet> { resultAttachmentSet };
        }

        return attachments;
    }

    private static async Task<IList<string>?> MergeCodeCoverageFilesAsync(IList<string> files, IProgress<int> progressReporter, CancellationToken cancellationToken)
    {
        try
        {
            // Warning: Don't remove this method call.
            //
            // We took a dependency on Coverage.CoreLib.Net. In the unlikely case it cannot be
            // resolved, this method call will throw an exception that will be caught and
            // absorbed here.
            var result = await MergeCodeCoverageFilesAsync(files, cancellationToken).ConfigureAwait(false);
            progressReporter?.Report(100);
            return result;
        }
        catch (OperationCanceledException)
        {
            // Occurs due to cancellation, ok to re-throw.
            throw;
        }
        catch (Exception ex)
        {
            EqtTrace.Error(
                "CodeCoverageDataCollectorAttachmentsHandler: Failed to load datacollector. Error: {0}",
                ex.ToString());
        }

        return null;
    }

    private static async Task<IList<string>?> MergeCodeCoverageFilesAsync(IList<string> files, CancellationToken cancellationToken)
    {
        TPDebug.Assert(s_mergeOperationEnumValues != null);

        cancellationToken.ThrowIfCancellationRequested();

        // Invoke methods
        LoadCodeCoverageAssembly();
        var task = (Task)s_mergeMethodInfo.Invoke(s_classInstance, new object[] { files[0], files, s_mergeOperationEnumValues.GetValue(0)!, true, cancellationToken })!;
        await task.ConfigureAwait(false);

        if (task.GetType().GetProperty("Result")!.GetValue(task, null) is not IList<string> mergedResults)
        {
            EqtTrace.Error("CodeCoverageDataCollectorAttachmentsHandler: Failed to merge code coverage files.");
            return files;
        }

        // Delete original files and keep merged file only
        foreach (var file in files)
        {
            if (mergedResults.Contains(file))
                continue;

            try
            {
                File.Delete(file);
            }
            catch (Exception ex)
            {
                EqtTrace.Error($"CodeCoverageDataCollectorAttachmentsHandler: Failed to remove {file}. Error: {ex}");
            }
        }

        return mergedResults;
    }

    [MemberNotNull(nameof(s_codeCoverageAssembly), nameof(s_classInstance), nameof(s_mergeOperationEnumValues), nameof(s_mergeMethodInfo))]
    private static void LoadCodeCoverageAssembly()
    {
        if (s_codeCoverageAssembly != null)
        {
            TPDebug.Assert(s_classInstance != null && s_mergeOperationEnumValues != null && s_mergeMethodInfo != null);
            return;
        }

        var dataAttachmentAssemblyLocation = typeof(CodeCoverageDataAttachmentsHandler).GetTypeInfo().Assembly.GetAssemblyLocation()!;
        var assemblyPath = Path.Combine(Path.GetDirectoryName(dataAttachmentAssemblyLocation)!, CodeCoverageIoAssemblyName + ".dll");
        s_codeCoverageAssembly = new PlatformAssemblyLoadContext().LoadAssemblyFromPath(assemblyPath);

        var classType = s_codeCoverageAssembly.GetType($"{CodeCoverageIoAssemblyName}.{CoverageFileUtilityTypeName}")!;
        s_classInstance = Activator.CreateInstance(classType)!;

        var types = s_codeCoverageAssembly.GetTypes();
        var mergeOperationEnum = Array.Find(types, d => d.Name == CoverageMergeOperationName)!;
        s_mergeOperationEnumValues = Enum.GetValues(mergeOperationEnum);
        s_mergeMethodInfo = classType.GetMethod(MergeMethodName, new[] { typeof(string), typeof(IList<string>), mergeOperationEnum, typeof(bool), typeof(CancellationToken) })!;
    }
}
