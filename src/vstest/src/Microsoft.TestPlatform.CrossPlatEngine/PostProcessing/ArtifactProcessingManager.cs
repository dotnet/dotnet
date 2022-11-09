// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Interfaces;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.TestRunAttachmentsProcessing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.ArtifactProcessing;

internal class ArtifactProcessingManager : IArtifactProcessingManager
{
    private const string RunsettingsFileName = "runsettings.xml";
    private const string ExecutionCompleteFileName = "executionComplete.json";

    private readonly string? _testSessionCorrelationId;
    private readonly IFileHelper _fileHelper;
    private readonly ITestRunAttachmentsProcessingManager _testRunAttachmentsProcessingManager;
    private readonly string? _testSessionProcessArtifactFolder;
    private readonly string? _processArtifactFolder;
    private readonly IDataSerializer _dataSerialized;
    private readonly ITestRunAttachmentsProcessingEventsHandler _testRunAttachmentsProcessingEventsHandler;
    private readonly IFeatureFlag _featureFlag;

    public ArtifactProcessingManager(string? testSessionCorrelationId) :
        this(testSessionCorrelationId,
            new FileHelper(),
            new TestRunAttachmentsProcessingManager(TestPlatformEventSource.Instance, new DataCollectorAttachmentsProcessorsFactory()),
            JsonDataSerializer.Instance,
            new PostProcessingTestRunAttachmentsProcessingEventsHandler(ConsoleOutput.Instance),
            FeatureFlag.Instance)
    { }

    public ArtifactProcessingManager(string? testSessionCorrelationId,
        IFileHelper fileHelper,
        ITestRunAttachmentsProcessingManager testRunAttachmentsProcessingManager,
        IDataSerializer dataSerialized,
        ITestRunAttachmentsProcessingEventsHandler testRunAttachmentsProcessingEventsHandler,
        IFeatureFlag featureFlag)
    {
        _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));
        _testRunAttachmentsProcessingManager = testRunAttachmentsProcessingManager ?? throw new ArgumentNullException(nameof(testRunAttachmentsProcessingManager));
        _dataSerialized = dataSerialized ?? throw new ArgumentNullException(nameof(dataSerialized));
        _testRunAttachmentsProcessingEventsHandler = testRunAttachmentsProcessingEventsHandler ?? throw new ArgumentNullException(nameof(testRunAttachmentsProcessingEventsHandler));
        _featureFlag = featureFlag ?? throw new ArgumentNullException(nameof(featureFlag));

        // We don't validate for null, it's expected, we'll have testSessionCorrelationId only in case of .NET SDK run.
        if (testSessionCorrelationId is not null)
        {
            _testSessionCorrelationId = testSessionCorrelationId;
            _processArtifactFolder = Path.Combine(_fileHelper.GetTempPath(), _testSessionCorrelationId);
#if NET5_0_OR_GREATER
            var pid = Environment.ProcessId;
#else
            int pid;
            using (var p = Process.GetCurrentProcess())
                pid = p.Id;
#endif
            _testSessionProcessArtifactFolder = Path.Combine(_processArtifactFolder, $"{pid}_{Guid.NewGuid()}");
        }
    }

    public void CollectArtifacts(TestRunCompleteEventArgs testRunCompleteEventArgs, string runSettingsXml)
    {
        ValidateArg.NotNull(testRunCompleteEventArgs, nameof(testRunCompleteEventArgs));
        ValidateArg.NotNull(runSettingsXml, nameof(runSettingsXml));

        if (_featureFlag.IsSet(FeatureFlag.DISABLE_ARTIFACTS_POSTPROCESSING))
        {
            EqtTrace.Verbose("ArtifactProcessingManager.CollectArtifacts: Feature disabled");
            return;
        }

        if (_testSessionCorrelationId.IsNullOrEmpty())
        {
            EqtTrace.Verbose("ArtifactProcessingManager.CollectArtifacts: null testSessionCorrelationId");
            return;
        }

        try
        {
            // We need to save in case of attachements, we'll show these at the end on console.
            if ((testRunCompleteEventArgs?.AttachmentSets.Count) <= 0)
            {
                return;
            }

            EqtTrace.Verbose($"ArtifactProcessingManager.CollectArtifacts: Saving data collectors artifacts for post process into {_processArtifactFolder}");
            Stopwatch watch = Stopwatch.StartNew();
            TPDebug.Assert(_testSessionProcessArtifactFolder is not null, "_testSessionProcessArtifactFolder is null");
            _fileHelper.CreateDirectory(_testSessionProcessArtifactFolder);
            EqtTrace.Verbose($"ArtifactProcessingManager.CollectArtifacts: Persist runsettings \n{runSettingsXml}");
            _fileHelper.WriteAllTextToFile(Path.Combine(_testSessionProcessArtifactFolder, RunsettingsFileName), runSettingsXml);
            var serializedExecutionComplete = _dataSerialized.SerializePayload(MessageType.ExecutionComplete, testRunCompleteEventArgs);
            EqtTrace.Verbose($"ArtifactProcessingManager.CollectArtifacts: Persist ExecutionComplete message \n{serializedExecutionComplete}");
            _fileHelper.WriteAllTextToFile(Path.Combine(_testSessionProcessArtifactFolder, ExecutionCompleteFileName), serializedExecutionComplete);
            EqtTrace.Verbose($"ArtifactProcessingManager.CollectArtifacts: Artifacts saved in {watch.Elapsed}");
        }
        catch (Exception e)
        {
            EqtTrace.Error("ArtifactProcessingManager.CollectArtifacts: Exception during artifact post processing: " + e);
        }
    }

    public async Task PostProcessArtifactsAsync()
    {
        if (_featureFlag.IsSet(FeatureFlag.DISABLE_ARTIFACTS_POSTPROCESSING))
        {
            EqtTrace.Verbose("ArtifactProcessingManager.PostProcessArtifacts: Feature disabled");
            return;
        }

        // This is not expected, anyway we prefer avoid exception for post processing
        if (_testSessionCorrelationId.IsNullOrEmpty())
        {
            EqtTrace.Error("ArtifactProcessingManager.PostProcessArtifacts: Unexpected null testSessionCorrelationId");
            return;
        }

        TPDebug.Assert(_processArtifactFolder is not null, "_processArtifactFolder is null");
        if (!_fileHelper.DirectoryExists(_processArtifactFolder))
        {
            EqtTrace.Verbose("ArtifactProcessingManager.PostProcessArtifacts: There are no artifacts to postprocess");
            return;
        }

        var testArtifacts = LoadTestArtifacts();
        if (testArtifacts?.Length > 0)
        {
            try
            {
                await DataCollectorsAttachmentsPostProcessing(testArtifacts);
            }
            finally
            {
                try
                {
                    _fileHelper.DeleteDirectory(_processArtifactFolder, true);
                    EqtTrace.Verbose($"ArtifactProcessingManager.PostProcessArtifacts: Directory '{_processArtifactFolder}' removed.");
                }
                catch (Exception ex)
                {
                    EqtTrace.Error($"ArtifactProcessingManager.PostProcessArtifacts: Unable to removed directory the '{_processArtifactFolder}'.\n{ex}");
                }
            }
        }
        else
        {
            EqtTrace.Warning($"ArtifactProcessingManager.PostProcessArtifacts: There are no artifacts to postprocess also if the artifact directory '{_processArtifactFolder}' exits");
        }
    }

    // We don't put everything inside a try/catch, we prefer get the exceptions because
    // we don't want partial results, it could confuse the user, better have a "failure".
    private async Task DataCollectorsAttachmentsPostProcessing(TestArtifacts[] testArtifacts)
    {
        // We take the biggest runsettings in size, it should be the one with more configuration.
        // In future we can think to merge...but it's not easy for custom config, we could break something.
        string? runsettingsFile = testArtifacts
            .SelectMany(x => x.Artifacts.Where(x => x.Type == ArtifactType.Runsettings))
            .OrderByDescending(x => _fileHelper.GetFileLength(x.FileName))
            .FirstOrDefault()?.FileName;

        string? runsettingsXml = null;
        if (runsettingsFile is not null)
        {
            using var artifactStream = _fileHelper.GetStream(runsettingsFile, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(artifactStream);
            runsettingsXml = await streamReader.ReadToEndAsync();
            EqtTrace.Verbose($"ArtifactProcessingManager.MergeDataCollectorAttachments: Chosen runsettings \n{runsettingsXml}");
        }
        else
        {
            EqtTrace.Verbose($"ArtifactProcessingManager.MergeDataCollectorAttachments: Null runsettings");
        }

        HashSet<InvokedDataCollector> invokedDataCollectors = new();
        List<AttachmentSet> attachments = new();
        foreach (var artifact in testArtifacts
            .SelectMany(x => x.Artifacts)
            .Where(x => x.Type == ArtifactType.ExecutionComplete))
        {
            using var artifactStream = _fileHelper.GetStream(artifact.FileName, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(artifactStream);
            string executionCompleteMessage = await streamReader.ReadToEndAsync();
            EqtTrace.Verbose($"ArtifactProcessingManager.MergeDataCollectorAttachments: ExecutionComplete message \n{executionCompleteMessage}");
            TestRunCompleteEventArgs? eventArgs = _dataSerialized.DeserializePayload<TestRunCompleteEventArgs>(_dataSerialized.DeserializeMessage(executionCompleteMessage));
            foreach (var invokedDataCollector in eventArgs?.InvokedDataCollectors ?? Enumerable.Empty<InvokedDataCollector>())
            {
                invokedDataCollectors.Add(invokedDataCollector);
            }
            foreach (var attachmentSet in eventArgs?.AttachmentSets ?? Enumerable.Empty<AttachmentSet>())
            {
                attachments.Add(attachmentSet);
            }
        }

        await _testRunAttachmentsProcessingManager.ProcessTestRunAttachmentsAsync(runsettingsXml,
            new RequestData()
            {
                IsTelemetryOptedIn = IsTelemetryOptedIn(),
                ProtocolConfig = ObjectModel.Constants.DefaultProtocolConfig
            },
            attachments,
            invokedDataCollectors,
            _testRunAttachmentsProcessingEventsHandler,
            CancellationToken.None);
    }

    private TestArtifacts[] LoadTestArtifacts()
    {
        TPDebug.Assert(_processArtifactFolder is not null, "_processArtifactFolder is null");
        return _fileHelper.GetFiles(_processArtifactFolder, "*.*", SearchOption.AllDirectories)
            .Select(file => new { TestSessionId = Path.GetFileName(Path.GetDirectoryName(file)), Artifact = file })
            .GroupBy(grp => grp.TestSessionId)
            .Select(testSessionArtifact => new TestArtifacts(testSessionArtifact.Key!, testSessionArtifact.Select(x => ParseArtifact(x.Artifact)).Where(x => x is not null).ToArray()!)) // Bang because null dataflow doesn't yet backport learning from the `Where` clause
            .ToArray();
    }

    private static Artifact? ParseArtifact(string fileName)
    {
        ValidateArg.NotNull(fileName, nameof(fileName));

        return Path.GetFileName(fileName) switch
        {
            RunsettingsFileName => new Artifact(fileName, ArtifactType.Runsettings),
            ExecutionCompleteFileName => new Artifact(fileName, ArtifactType.ExecutionComplete),
            _ => null
        };
    }

    private static bool IsTelemetryOptedIn() => Environment.GetEnvironmentVariable("VSTEST_TELEMETRY_OPTEDIN")?.Equals("1", StringComparison.Ordinal) == true;
}
