// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.Execution;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.DataCollection;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.TestPlatform.Extensions.BlameDataCollector;

/// <summary>
/// The blame collector.
/// </summary>
[DataCollectorFriendlyName("Blame")]
[DataCollectorTypeUri("datacollector://Microsoft/TestPlatform/Extensions/Blame/v1")]
public class BlameCollector : DataCollector, ITestExecutionEnvironmentSpecifier
{
    private const int DefaultInactivityTimeInMinutes = 60;

    private DataCollectionSink? _dataCollectionSink;
    private DataCollectionEnvironmentContext? _context;
    private DataCollectionEvents? _events;
    private DataCollectionLogger? _logger;
    private readonly IProcessDumpUtility _processDumpUtility;
    private List<Guid>? _testSequence;
    private Dictionary<Guid, BlameTestObject>? _testObjectDictionary;
    private readonly IBlameReaderWriter _blameReaderWriter;
    private readonly IFileHelper _fileHelper;
    private readonly IProcessHelper _processHelper;
    private XmlElement? _configurationElement;
    private int _testStartCount;
    private int _testEndCount;
    private bool _collectProcessDumpOnCrash;
    private bool _collectProcessDumpOnHang;
    private bool _monitorPostmortemDumpFolder;
    private bool _collectDumpAlways;
    private string? _attachmentGuid;

    private CrashDumpType _crashDumpType;
    private HangDumpType? _hangDumpType;

    private bool _inactivityTimerAlreadyFired;
    private IInactivityTimer? _inactivityTimer;
    private TimeSpan _inactivityTimespan = TimeSpan.FromMinutes(DefaultInactivityTimeInMinutes);

    private int _testHostProcessId;
    private string? _testHostProcessName;
    private string? _targetFramework;
    private readonly List<KeyValuePair<string, string>> _environmentVariables = new();
    private bool _uploadDumpFiles;
    private string? _tempDirectory;
    private string? _monitorPostmortemDumpFolderPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlameCollector"/> class.
    /// Using XmlReaderWriter by default
    /// </summary>
    public BlameCollector()
        : this(new XmlReaderWriter(), new ProcessDumpUtility(), null, new FileHelper(), new ProcessHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BlameCollector"/> class.
    /// </summary>
    /// <param name="blameReaderWriter">
    /// BlameReaderWriter instance.
    /// </param>
    /// <param name="processDumpUtility">
    /// IProcessDumpUtility instance.
    /// </param>
    /// <param name="inactivityTimer">
    /// InactivityTimer instance.
    /// </param>
    /// <param name="fileHelper">
    /// Filehelper instance.
    /// </param>
    /// <param name="processHelper">Process helper instance.</param>
    internal BlameCollector(
        IBlameReaderWriter blameReaderWriter,
        IProcessDumpUtility processDumpUtility,
        IInactivityTimer? inactivityTimer,
        IFileHelper fileHelper,
        IProcessHelper processHelper)
    {
        _blameReaderWriter = blameReaderWriter;
        _processDumpUtility = processDumpUtility;
        _inactivityTimer = inactivityTimer;
        _fileHelper = fileHelper;
        _processHelper = processHelper;
    }

    /// <summary>
    /// Gets environment variables that should be set in the test execution environment
    /// </summary>
    /// <returns>Environment variables that should be set in the test execution environment</returns>
    public IEnumerable<KeyValuePair<string, string>> GetTestExecutionEnvironmentVariables()
    {
        return _environmentVariables;
    }

    /// <summary>
    /// Initializes parameters for the new instance of the class <see cref="BlameDataCollector"/>
    /// </summary>
    /// <param name="configurationElement">The Xml Element to save to</param>
    /// <param name="events">Data collection events to which methods subscribe</param>
    /// <param name="dataSink">A data collection sink for data transfer</param>
    /// <param name="logger">Data Collection Logger to send messages to the client </param>
    /// <param name="environmentContext">Context of data collector environment</param>
    [MemberNotNull(nameof(_events), nameof(_dataCollectionSink), nameof(_testSequence), nameof(_testObjectDictionary), nameof(_logger))]
    public override void Initialize(
        XmlElement? configurationElement,
        DataCollectionEvents events,
        DataCollectionSink dataSink,
        DataCollectionLogger logger,
        DataCollectionEnvironmentContext? environmentContext)
    {
        DebuggerBreakpoint.WaitForDebugger(WellKnownDebugEnvironmentVariables.VSTEST_BLAMEDATACOLLECTOR_DEBUG);

        _events = events;
        _dataCollectionSink = dataSink;
        _context = environmentContext;
        _configurationElement = configurationElement;
        _testSequence = new List<Guid>();
        _testObjectDictionary = new Dictionary<Guid, BlameTestObject>();
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Subscribing to events
        _events.TestHostLaunched += TestHostLaunchedHandler;
        _events.SessionEnd += SessionEndedHandler;
        _events.TestCaseStart += EventsTestCaseStart;
        _events.TestCaseEnd += EventsTestCaseEnd;

        if (_configurationElement != null)
        {
            if (_configurationElement[Constants.DumpModeKey] is XmlElement collectDumpNode)
            {
                _collectProcessDumpOnCrash = true;
                ValidateAndAddCrashProcessDumpParameters(collectDumpNode);

                // enabling dumps on MacOS needs to be done explicitly https://github.com/dotnet/runtime/pull/40105
                _environmentVariables.Add(new KeyValuePair<string, string>("COMPlus_DbgEnableElfDumpOnMacOS", "1"));
                _environmentVariables.Add(new KeyValuePair<string, string>("COMPlus_DbgEnableMiniDump", "1"));

                // https://github.com/dotnet/coreclr/blob/master/Documentation/botr/xplat-minidump-generation.md
                // 2   MiniDumpWithPrivateReadWriteMemory
                // 4   MiniDumpWithFullMemory
                _environmentVariables.Add(new KeyValuePair<string, string>("COMPlus_DbgMiniDumpType", _crashDumpType == CrashDumpType.Full ? "4" : "2"));
                var dumpDirectory = GetDumpDirectory();
                var dumpPath = Path.Combine(dumpDirectory, $"%e_%p_%t_crashdump.dmp");
                _environmentVariables.Add(new KeyValuePair<string, string>("COMPlus_DbgMiniDumpName", dumpPath));
            }
            else
            {
                _collectProcessDumpOnCrash = false;
            }

            if (_configurationElement[Constants.CollectDumpOnTestSessionHang] is XmlElement collectHangBasedDumpNode)
            {
                _collectProcessDumpOnHang = true;
                // enabling dumps on MacOS needs to be done explicitly https://github.com/dotnet/runtime/pull/40105
                _environmentVariables.Add(new KeyValuePair<string, string>("COMPlus_DbgEnableElfDumpOnMacOS", "1"));

                ValidateAndAddHangProcessDumpParameters(collectHangBasedDumpNode!);
            }
            else
            {
                _collectProcessDumpOnHang = false;
            }

            _monitorPostmortemDumpFolder = _configurationElement[Constants.MonitorPostmortemDebugger] is XmlElement monitorPostmortemNode &&
                ValidateMonitorPostmortemDebuggerParameters(monitorPostmortemNode);
            EqtTrace.Info($"[MonitorPostmortemDump]Monitor enabled: '{_monitorPostmortemDumpFolder}'");

            var tfm = _configurationElement[Constants.TargetFramework]?.InnerText;
            if (!tfm.IsNullOrWhiteSpace())
            {
                _targetFramework = tfm;
            }
        }

        _attachmentGuid = Guid.NewGuid().ToString("N");

        if (_collectProcessDumpOnHang)
        {
            _inactivityTimer ??= new InactivityTimer(CollectDumpAndAbortTesthost);
            ResetInactivityTimer();
        }
    }

    /// <summary>
    /// Disposes of the timer when called to prevent further calls.
    /// Kills the other instance of proc dump if launched for collecting crash dumps.
    /// Starts and waits for a new proc dump process to collect a single dump and then
    /// kills the testhost process.
    /// </summary>
    private void CollectDumpAndAbortTesthost()
    {
        TPDebug.Assert(_logger != null && _context != null && _dataCollectionSink != null, "Initialize must be called before calling this method");
        _inactivityTimerAlreadyFired = true;

        string value;
        string unit;

        if (_inactivityTimespan.TotalSeconds <= 90)
        {
            value = ((int)_inactivityTimespan.TotalSeconds).ToString(CultureInfo.InvariantCulture);
            unit = Resources.Resources.Seconds;
        }
        else
        {
            value = Math.Round(_inactivityTimespan.TotalMinutes, 2).ToString(CultureInfo.InvariantCulture);
            unit = Resources.Resources.Minutes;
        }

        var message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.InactivityTimeout, value, unit);

        EqtTrace.Warning(message);
        _logger.LogWarning(_context.SessionDataCollectionContext, message);

        try
        {
            EqtTrace.Verbose("Calling dispose on Inactivity timer.");
            _inactivityTimer?.Dispose();
        }
        catch
        {
            EqtTrace.Verbose("Inactivity timer is already disposed.");
        }

        if (_collectProcessDumpOnCrash)
        {
            // Detach the dumper from the testhost process to prevent crashing testhost process. When the dumper is procdump.exe
            // it must be detached before we try to dump the process, and simply killing it would take down the testhost process.
            //
            // Detaching also prevents creating an extra dump at the exit of the testhost process.
            _processDumpUtility.DetachFromTargetProcess(_testHostProcessId);
        }

        // Skip creating the dump if the option is set to none, and just kill the process.
        if ((_hangDumpType ?? HangDumpType.Full) != HangDumpType.None)
        {
            try
            {
                Action<string> logWarning = m => _logger.LogWarning(_context.SessionDataCollectionContext, m);
                var dumpDirectory = GetDumpDirectory();
                _processDumpUtility.StartHangBasedProcessDump(_testHostProcessId, dumpDirectory, _hangDumpType == HangDumpType.Full, _targetFramework!, logWarning);
            }
            catch (Exception ex)
            {
                _logger.LogError(_context.SessionDataCollectionContext, $"Blame: Creating hang dump failed with error.", ex);
            }

            if (_uploadDumpFiles)
            {
                try
                {
                    var dumpFiles = _processDumpUtility.GetDumpFiles(true,
                        true /* Get all dumps that there are, if crashdump was created before we hangdumped, get it. It probably has interesting info. */);
                    foreach (var dumpFile in dumpFiles)
                    {
                        try
                        {
                            if (!dumpFile.IsNullOrEmpty())
                            {
                                var fileTransferInformation = new FileTransferInformation(_context.SessionDataCollectionContext, dumpFile, true, _fileHelper);
                                _dataCollectionSink.SendFileAsync(fileTransferInformation);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Eat up any exception here and log it but proceed with killing the test host process.
                            EqtTrace.Error(ex);
                        }

                        if (!dumpFiles.Any())
                        {
                            EqtTrace.Error("BlameCollector.CollectDumpAndAbortTesthost: blame:CollectDumpOnHang was enabled but dump file was not generated.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(_context.SessionDataCollectionContext, $"Blame: Collecting hang dump failed with error.", ex);
                }
            }
            else
            {
                EqtTrace.Info("BlameCollector.CollectDumpAndAbortTesthost: Custom path to dump directory was provided via VSTEST_DUMP_PATH. Skipping attachment upload, the caller is responsible for collecting and uploading the dumps themselves.");
            }
        }

        try
        {
            var p = Process.GetProcessById(_testHostProcessId);
            try
            {
                if (!p.HasExited)
                {
                    p.Kill();
                }
            }
            catch (InvalidOperationException)
            {
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error(ex);
        }
    }

    private bool ValidateMonitorPostmortemDebuggerParameters(XmlElement collectDumpNode)
    {
        TPDebug.Assert(_logger != null && _context != null, "Initialize must be called before calling this method");
        if (StringUtils.IsNullOrEmpty(_monitorPostmortemDumpFolderPath = collectDumpNode.GetAttribute("DumpDirectoryPath")))
        {
            _logger.LogWarning(_context.SessionDataCollectionContext, Resources.Resources.MonitorPostmortemDebuggerInvalidDumpDirectoryPathParameter);
            return false;
        }

        if (!_fileHelper.DirectoryExists(_monitorPostmortemDumpFolderPath))
        {
            _logger.LogWarning(_context.SessionDataCollectionContext, Resources.Resources.MonitorPostmortemDebuggerInvalidDumpDirectoryPathParameter);
            return false;
        }

        return true;
    }

    private void ValidateAndAddCrashProcessDumpParameters(XmlElement collectDumpNode)
    {
        TPDebug.Assert(_logger != null && _context != null, "Initialize must be called before calling this method");
        foreach (XmlAttribute blameAttribute in collectDumpNode.Attributes)
        {
            switch (blameAttribute)
            {
                case XmlAttribute attribute when string.Equals(attribute.Name, Constants.CollectDumpAlwaysKey, StringComparison.OrdinalIgnoreCase):

                    if ((!string.Equals(attribute.Value, Constants.TrueConfigurationValue, StringComparison.OrdinalIgnoreCase)
                        && !string.Equals(attribute.Value, Constants.FalseConfigurationValue, StringComparison.OrdinalIgnoreCase))
                        || !bool.TryParse(attribute.Value, out _collectDumpAlways))
                    {
                        _logger.LogWarning(_context.SessionDataCollectionContext, FormatBlameParameterValueIncorrectMessage(attribute, [Constants.TrueConfigurationValue, Constants.FalseConfigurationValue]));
                    }

                    break;

                case XmlAttribute attribute when string.Equals(attribute.Name, Constants.DumpTypeKey, StringComparison.OrdinalIgnoreCase):

                    if (Enum.TryParse(attribute.Value, ignoreCase: true, out CrashDumpType value) && Enum.IsDefined(typeof(CrashDumpType), value))
                    {
                        _crashDumpType = value;
                    }
                    else
                    {
                        _logger.LogWarning(_context.SessionDataCollectionContext, FormatBlameParameterValueIncorrectMessage(attribute, Enum.GetNames(typeof(CrashDumpType))));
                    }

                    break;

                default:

                    _logger.LogWarning(_context.SessionDataCollectionContext, string.Format(CultureInfo.CurrentCulture, Resources.Resources.BlameParameterKeyIncorrect, blameAttribute.Name));
                    break;
            }
        }
    }

    internal static string FormatBlameParameterValueIncorrectMessage(XmlAttribute attribute, params string[] validValues)
    {
        return string.Format(CultureInfo.CurrentCulture, Resources.Resources.BlameParameterValueIncorrect, attribute.Name, attribute.Value, string.Join(", ", validValues));
    }

    private void ValidateAndAddHangProcessDumpParameters(XmlElement collectDumpNode)
    {
        TPDebug.Assert(_logger != null && _context != null, "Initialize must be called before calling this method");
        foreach (XmlAttribute blameAttribute in collectDumpNode.Attributes)
        {
            switch (blameAttribute)
            {
                case XmlAttribute attribute when string.Equals(attribute.Name, Constants.TestTimeout, StringComparison.OrdinalIgnoreCase):

                    if (!attribute.Value.IsNullOrWhiteSpace() && TimeSpanParser.TryParse(attribute.Value, out var timeout))
                    {
                        _inactivityTimespan = timeout;
                    }
                    else
                    {
                        _logger.LogWarning(_context.SessionDataCollectionContext, string.Format(CultureInfo.CurrentCulture, Resources.Resources.UnexpectedValueForInactivityTimespanValue, attribute.Value));
                    }

                    break;

                // allow HangDumpType attribute to be used on the hang dump this is the prefered way
                case XmlAttribute attribute when string.Equals(attribute.Name, Constants.HangDumpTypeKey, StringComparison.OrdinalIgnoreCase):

                    if (Enum.TryParse(attribute.Value, ignoreCase: true, out HangDumpType value) && Enum.IsDefined(typeof(HangDumpType), value))
                    {
                        _hangDumpType = value;
                    }
                    else
                    {
                        _logger.LogWarning(_context.SessionDataCollectionContext, FormatBlameParameterValueIncorrectMessage(attribute, Enum.GetNames(typeof(HangDumpType))));
                    }

                    break;

                // allow DumpType attribute to be used on the hang dump for backwards compatibility
                case XmlAttribute attribute when string.Equals(attribute.Name, Constants.DumpTypeKey, StringComparison.OrdinalIgnoreCase):
                    // DumpType and HangDumpType are both valid ways to define the dump type. In case we get HangDumpType and DumpType in the command we want HangDumpType to win.
                    if (Enum.TryParse(attribute.Value, ignoreCase: true, out HangDumpType value2) && Enum.IsDefined(typeof(HangDumpType), value2))
                    {
                        _hangDumpType = value2;
                    }
                    else
                    {
                        // This error is using CrashDumpType on purpose, because the option we are providing is actually supposed to take only CrashDumpType values. We are parsing it into
                        // HangDumpType to have easier time converting.
                        _logger.LogWarning(_context.SessionDataCollectionContext, FormatBlameParameterValueIncorrectMessage(attribute, Enum.GetNames(typeof(CrashDumpType))));
                    }

                    break;

                default:

                    _logger.LogWarning(_context.SessionDataCollectionContext, string.Format(CultureInfo.CurrentCulture, Resources.Resources.BlameParameterKeyIncorrect, blameAttribute.Name));
                    break;
            }
        }
    }

    /// <summary>
    /// Called when Test Case Start event is invoked
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">TestCaseStartEventArgs</param>
    private void EventsTestCaseStart(object? sender, TestCaseStartEventArgs e)
    {
        TPDebug.Assert(_testSequence != null && _testObjectDictionary != null, "Initialize must be called before calling this method");
        ResetInactivityTimer();

        EqtTrace.Info("BlameCollector.EventsTestCaseStart: Test Case Start");

        TPDebug.Assert(e.TestElement is not null, "e.TestElement is null");
        var blameTestObject = new BlameTestObject(e.TestElement);

        // Add guid to list of test sequence to maintain the order.
        _testSequence.Add(blameTestObject.Id);

        // Add the test object to the dictionary.
        _testObjectDictionary.Add(blameTestObject.Id, blameTestObject);

        // Increment test start count.
        _testStartCount++;
    }

    /// <summary>
    /// Called when Test Case End event is invoked
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">TestCaseEndEventArgs</param>
    private void EventsTestCaseEnd(object? sender, TestCaseEndEventArgs e)
    {
        TPDebug.Assert(_testObjectDictionary != null, "Initialize must be called before calling this method");
        ResetInactivityTimer();

        EqtTrace.Info("BlameCollector.EventsTestCaseEnd: Test Case End");

        _testEndCount++;

        // Update the test object in the dictionary as the test has completed.
        TPDebug.Assert(e.TestElement is not null, "e.TestElement is null");
        if (_testObjectDictionary.ContainsKey(e.TestElement.Id))
        {
            _testObjectDictionary[e.TestElement.Id].IsCompleted = true;
        }
    }

    /// <summary>
    /// Called when Session End event is invoked
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="args">SessionEndEventArgs</param>
    private void SessionEndedHandler(object? sender, SessionEndEventArgs args)
    {
        TPDebug.Assert(_testSequence != null && _testObjectDictionary != null && _context != null && _dataCollectionSink != null && _logger != null, "Initialize must be called before calling this method");
        ResetInactivityTimer();

        EqtTrace.Info("Blame Collector: Session End");

        try
        {
            // If the last test crashes, it will not invoke a test case end and therefore
            // In case of crash testStartCount will be greater than testEndCount and we need to write the sequence
            // And send the attachment. This won't indicate failure if there are 0 tests in the assembly, or when it fails in setup.
            var processCrashedWhenRunningTests = _testStartCount > _testEndCount;
            if (processCrashedWhenRunningTests)
            {
                var filepath = Path.Combine(GetTempDirectory(), Constants.AttachmentFileName + "_" + _attachmentGuid);

                filepath = _blameReaderWriter.WriteTestSequence(_testSequence, _testObjectDictionary, filepath);
                var fti = new FileTransferInformation(_context.SessionDataCollectionContext, filepath, true);
                _dataCollectionSink.SendFileAsync(fti);
            }
            else
            {
                if (_collectProcessDumpOnHang)
                {
                    _logger.LogWarning(_context.SessionDataCollectionContext, Resources.Resources.NotGeneratingSequenceFile);
                }
            }

            if (_uploadDumpFiles)
            {
                try
                {
                    var dumpFiles = _processDumpUtility.GetDumpFiles(warnOnNoDumpFiles: _collectDumpAlways, processCrashedWhenRunningTests);
                    foreach (var dumpFile in dumpFiles)
                    {
                        if (!dumpFile.IsNullOrEmpty())
                        {
                            try
                            {
                                var fileTransferInformation = new FileTransferInformation(_context.SessionDataCollectionContext, dumpFile, true);
                                _dataCollectionSink.SendFileAsync(fileTransferInformation);
                            }
                            catch (FileNotFoundException ex)
                            {
                                EqtTrace.Warning(ex.ToString());
                                _logger.LogWarning(args.Context, ex.ToString());
                            }
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    EqtTrace.Warning(ex.ToString());
                    _logger.LogWarning(args.Context, ex.ToString());
                }
            }
            else
            {
                EqtTrace.Info("BlameCollector.CollectDumpAndAbortTesthost: Custom path to dump directory was provided via VSTEST_DUMP_PATH. Skipping attachment upload, the caller is responsible for collecting and uploading the dumps themselves.");
            }

            if (_monitorPostmortemDumpFolder)
            {
                if (!_fileHelper.DirectoryExists(_monitorPostmortemDumpFolderPath))
                {
                    _logger.LogWarning(_context.SessionDataCollectionContext, Resources.Resources.MonitorPostmortemDebuggerInvalidDumpDirectoryPathParameter);
                }
                else
                {
                    // We do ToArray() because we're moving files and we cannot move file and enumerate at the same time
                    foreach (var dumpFileNameFullPath in _fileHelper.GetFiles(_monitorPostmortemDumpFolderPath, "*.dmp", SearchOption.TopDirectoryOnly).ToArray())
                    {
                        EqtTrace.Info($"[MonitorPostmortemDump]'{dumpFileNameFullPath}' dump file found during postmortem monitoring");
                        // Ensure exclusive access to the dump file, it can happen if we run more test module in parallel.
                        // We cannot ensure that we'll move only "our" dump because procdump -i produce a name that doesn't have the pid in it(because PID is reusable).
                        // The name of the file starts with the process name, that's the only filtering we can do.
                        // So there's one possible benign race condition when another test is dumping an host and we take lock on the name but the dump is not finished.
                        // In that case we'll fail for file locking but it's fine. The correct or subsequent "SessionEndedHandler" will move that one.
                        using SHA256 hashedLockName = SHA256.Create();
                        // LPCSTR An LPCSTR is a 32-bit pointer to a constant null-terminated string of 8-bit Windows (ANSI) characters.
                        var toGuid = new byte[16];
                        Array.Copy(hashedLockName.ComputeHash(Encoding.UTF8.GetBytes(dumpFileNameFullPath)), toGuid, 16);
                        Guid id = new(toGuid);
                        string muxerName = @$"Global\{id}";
                        using Mutex lockFile = new(true, muxerName, out bool createdNew);
                        EqtTrace.Info($"[MonitorPostmortemDump]Acquired global muxer '{muxerName}' for {dumpFileNameFullPath}");
                        if (createdNew)
                        {
                            string dumpFileName = Path.GetFileNameWithoutExtension(dumpFileNameFullPath);
                            TPDebug.Assert(_testHostProcessName != null, $"TestHostLaunchedHandler must run before this method and set the _testHostProcessName");
                            // Expected format testhost.exe_221004_123127.dmp processName.exe_yyMMdd_HHmmss.dmp
                            if (dumpFileName.StartsWith(_testHostProcessName, StringComparison.OrdinalIgnoreCase))
                            {
                                EqtTrace.Info($"[MonitorPostmortemDump]Valid pattern start with '{_testHostProcessName}' found for {dumpFileNameFullPath}");
                                try
                                {
                                    var fileTranferInformation = new FileTransferInformation(_context.SessionDataCollectionContext, dumpFileNameFullPath, true);
                                    EqtTrace.Info($"[MonitorPostmortemDump]Transferring {dumpFileNameFullPath}");
                                    _dataCollectionSink.SendFileAsync(fileTranferInformation);
                                }
                                catch (IOException ex)
                                {
                                    // In case of race condition explained in the comment above we simply log a warning.
                                    EqtTrace.Warning(ex.ToString());
                                    _logger.LogWarning(args.Context, ex.ToString());
                                }
                            }
                        }
                    }
                }
            }
        }
        finally
        {
            // Attempt to terminate the proc dump process if proc dump was enabled
            if (_collectProcessDumpOnCrash)
            {
                _processDumpUtility.DetachFromTargetProcess(_testHostProcessId);
            }

            DeregisterEvents();
        }
    }

    /// <summary>
    /// Called when Test Host Initialized is invoked
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="args">TestHostLaunchedEventArgs</param>
    private void TestHostLaunchedHandler(object? sender, TestHostLaunchedEventArgs args)
    {
        ResetInactivityTimer();
        _testHostProcessId = args.TestHostProcessId;
        _testHostProcessName = _processHelper.GetProcessName(args.TestHostProcessId);

        if (!_collectProcessDumpOnCrash)
        {
            return;
        }

        TPDebug.Assert(_logger != null && _context != null, "Initialize must be called before calling this method");

        try
        {
            var dumpDirectory = GetDumpDirectory();
            Action<string> logWarning = m => _logger.LogWarning(_context.SessionDataCollectionContext, m);
            _processDumpUtility.StartTriggerBasedProcessDump(args.TestHostProcessId, dumpDirectory, _crashDumpType == CrashDumpType.Full, _targetFramework!, _collectDumpAlways, logWarning);
        }
        catch (TestPlatformException e)
        {
            EqtTrace.Warning("BlameCollector.TestHostLaunchedHandler: Could not start process dump. {0}", e);
            _logger.LogWarning(args.Context, string.Format(CultureInfo.CurrentCulture, Resources.Resources.ProcDumpCouldNotStart, e.Message));
        }
        catch (Exception e)
        {
            EqtTrace.Warning("BlameCollector.TestHostLaunchedHandler: Could not start process dump. {0}", e);
            _logger.LogWarning(args.Context, string.Format(CultureInfo.CurrentCulture, Resources.Resources.ProcDumpCouldNotStart, e.ToString()));
        }
    }

    /// <summary>
    /// Resets the inactivity timer
    /// </summary>
    private void ResetInactivityTimer()
    {
        if (!_collectProcessDumpOnHang || _inactivityTimerAlreadyFired)
        {
            return;
        }

        EqtTrace.Verbose("Reset the inactivity timer since an event was received.");
        try
        {
            _inactivityTimer?.ResetTimer(_inactivityTimespan);
        }
        catch (Exception e)
        {
            EqtTrace.Warning($"Failed to reset the inactivity timer with error {e}");
        }
    }

    /// <summary>
    /// Method to de-register handlers and cleanup
    /// </summary>
    private void DeregisterEvents()
    {
        TPDebug.Assert(_events != null, "Initialize must be called before calling this method");
        _events.SessionEnd -= SessionEndedHandler;
        _events.TestCaseStart -= EventsTestCaseStart;
        _events.TestCaseEnd -= EventsTestCaseEnd;
    }

    private string GetTempDirectory()
    {
        if (_tempDirectory.IsNullOrWhiteSpace())
        {
            // DUMP_TEMP_PATH will be used as temporary storage location
            // for the dumps, this won't affect the dump uploads. Just the place where
            // we store them before moving them to the final folder.

            // AGENT_TEMPDIRECTORY is AzureDevops variable, which is set to path
            // that is cleaned up after every job. This is preferable to use over
            // just the normal temp.
            var temp = Environment.GetEnvironmentVariable("VSTEST_DUMP_TEMP_PATH") ?? Environment.GetEnvironmentVariable("AGENT_TEMPDIRECTORY") ?? Path.GetTempPath();
            _tempDirectory = Path.Combine(temp, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
            return _tempDirectory;
        }

        return _tempDirectory;
    }

    private string GetDumpDirectory()
    {
        TPDebug.Assert(_logger != null && _context != null, "Initialize must be called before calling this method");

        // Using a custom dump path for scenarios where we want to upload the
        // dump files ourselves, such as when running in Helix.
        // This will save into the directory specified via VSTEST_DUMP_PATH, and
        //  skip uploading dumps via attachments.
        var dumpDirectoryOverride = Environment.GetEnvironmentVariable("VSTEST_DUMP_PATH");
        var dumpDirectoryOverrideHasValue = !dumpDirectoryOverride.IsNullOrWhiteSpace();
        _uploadDumpFiles = !dumpDirectoryOverrideHasValue;

        var dumpDirectory = dumpDirectoryOverrideHasValue ? dumpDirectoryOverride! : GetTempDirectory();
        Directory.CreateDirectory(dumpDirectory);
        var dumpPath = Path.Combine(Path.GetFullPath(dumpDirectory));

        if (!_uploadDumpFiles)
        {
            _logger.LogWarning(_context.SessionDataCollectionContext, $"VSTEST_DUMP_PATH is specified. Dump files will be saved in: {dumpPath}, and won't be added to attachments.");
        }

        return dumpPath;
    }
}
