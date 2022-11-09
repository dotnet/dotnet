// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Xml;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// Stores information about a test settings.
/// </summary>
public class RunConfiguration : TestRunSettings
{
    /// <summary>
    /// Platform architecture which rocksteady should use for discovery/execution
    /// </summary>
    private Architecture _platform;

    private Architecture? _defaultPlatform;

    /// <summary>
    /// Maximum number of cores that the engine can use to run tests in parallel
    /// </summary>
    private int _maxCpuCount;

    /// <summary>
    /// .Net framework which rocksteady should use for discovery/execution
    /// </summary>
    private Framework? _framework;

    /// <summary>
    /// Specifies the frequency of the runStats/discoveredTests event
    /// </summary>
    private long _batchSize;

    /// <summary>
    /// Directory in which rocksteady/adapter should keep their run specific data.
    /// </summary>
    private string _resultsDirectory;

    /// <summary>
    /// Paths at which rocksteady should look for test adapters
    /// </summary>
    private string? _testAdaptersPaths;

    /// <summary>
    /// Indication to adapters to disable app domain.
    /// </summary>
    private bool _disableAppDomain;

    /// <summary>
    /// Indication to adapters to disable parallelization.
    /// </summary>
    private bool _disableParallelization;

    /// <summary>
    /// True if test run is triggered
    /// </summary>
    private bool _designMode;

    /// <summary>
    /// False indicates that the test adapter should not collect source information for discovered tests
    /// </summary>
    private bool _shouldCollectSourceInformation;

    /// <summary>
    /// Initializes a new instance of the <see cref="RunConfiguration"/> class.
    /// </summary>
    public RunConfiguration() : base(Constants.RunConfigurationSettingsName)
    {
        // Set defaults for target platform, framework version type and results directory.
        _platform = Constants.DefaultPlatform;
        _framework = Framework.DefaultFramework;
        _resultsDirectory = Constants.DefaultResultsDirectory;
        SolutionDirectory = null;
        TreatTestAdapterErrorsAsWarnings = Constants.DefaultTreatTestAdapterErrorsAsWarnings;
        BinariesRoot = null;
        _testAdaptersPaths = null;
        _maxCpuCount = Constants.DefaultCpuCount;
        _batchSize = Constants.DefaultBatchSize;
        TestSessionTimeout = 0;
        _disableAppDomain = false;
        _disableParallelization = false;
        _designMode = false;
        InIsolation = false;
        _shouldCollectSourceInformation = false;
        TargetDevice = null;
        ExecutionThreadApartmentState = Constants.DefaultExecutionThreadApartmentState;
    }

    /// <summary>
    /// Gets or sets the solution directory.
    /// </summary>
    public string? SolutionDirectory
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the results directory.
    /// </summary>
    public string ResultsDirectory
    {
        get => _resultsDirectory;

        set
        {
            _resultsDirectory = value;
            ResultsDirectorySet = true;
        }
    }

    /// <summary>
    /// Gets or sets the Parallel execution option. Should be non-negative integer.
    /// </summary>
    public int MaxCpuCount
    {
        get => _maxCpuCount;
        set
        {
            _maxCpuCount = value;
            MaxCpuCountSet = true;
        }
    }

    /// <summary>
    /// Gets or sets the frequency of the runStats/discoveredTests event. Should be non-negative integer.
    /// </summary>
    public long BatchSize
    {
        get => _batchSize;
        set
        {
            _batchSize = value;
            BatchSizeSet = true;
        }
    }

    /// <summary>
    /// Gets or sets the testSessionTimeout. Should be non-negative integer.
    /// </summary>
    public long TestSessionTimeout { get; set; }

    /// <summary>
    /// Gets or sets the design mode value.
    /// </summary>
    public bool DesignMode
    {
        get => _designMode;

        set
        {
            _designMode = value;
            DesignModeSet = true;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to run tests in isolation or not.
    /// </summary>
    public bool InIsolation { get; set; }

    /// <summary>
    /// Gets a value indicating whether test adapter needs to collect source information for discovered tests
    /// </summary>
    public bool ShouldCollectSourceInformation
    {
        get => (CollectSourceInformationSet) ? _shouldCollectSourceInformation : _designMode;

        set
        {
            _shouldCollectSourceInformation = value;
            CollectSourceInformationSet = true;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether app domain creation should be disabled.
    /// </summary>
    public bool DisableAppDomain
    {
        get => _disableAppDomain;

        set
        {
            _disableAppDomain = value;
            DisableAppDomainSet = true;
        }
    }

    /// <summary>
    /// Gets or sets the test adapter loading strategy.
    /// </summary>
    internal TestAdapterLoadingStrategy TestAdapterLoadingStrategy { get; set; }

    /// <summary>
    /// Gets a value indicating whether parallelism needs to be disabled by the adapters.
    /// </summary>
    public bool DisableParallelization
    {
        get => _disableParallelization;

        set
        {
            _disableParallelization = value;
            DisableParallelizationSet = true;
        }
    }

    /// <summary>
    /// Gets or sets the Target platform this run is targeting. Possible values are <see cref="Architecture"/> except for AnyCPU and Default.
    /// </summary>
    public Architecture TargetPlatform
    {
        get => _platform;

        set
        {
            _platform = value;
            TargetPlatformSet = true;
        }
    }

    /// <summary>
    /// Gets or sets the default platform that will be used for AnyCPU sources, or non-dll sources. Possible values are <see cref="Architecture"/> except for AnyCPU and Default.
    /// </summary>
    public Architecture? DefaultPlatform
    {
        get => _defaultPlatform;

        set
        {
            _defaultPlatform = value;
            DefaultPlatformSet = true;
        }
    }

    /// <summary>
    /// Gets or sets the target Framework this run is targeting.
    /// </summary>
    public Framework? TargetFramework
    {
        get => _framework;

        set
        {
            _framework = value;
            TargetFrameworkSet = true;
        }
    }
    /// <summary>
    /// Gets or sets value indicating exit code when no tests are discovered or executed
    /// </summary>
    public bool TreatNoTestsAsError
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the target Framework this run is targeting. Possible values are Framework3.5|Framework4.0|Framework4.5
    /// </summary>
    [Obsolete("Use TargetFramework instead")]
    public FrameworkVersion TargetFrameworkVersion
    {
        get => _framework?.Name switch
        {
            Constants.DotNetFramework35 => FrameworkVersion.Framework35,
            Constants.DotNetFramework40 => FrameworkVersion.Framework40,
            Constants.DotNetFramework45 => FrameworkVersion.Framework45,
            Constants.DotNetFrameworkCore10 => FrameworkVersion.FrameworkCore10,
            Constants.DotNetFrameworkUap10 => FrameworkVersion.FrameworkUap10,
            _ => Constants.DefaultFramework,
        };

        set
        {
            _framework = Framework.FromString(value.ToString());
            TargetFrameworkSet = true;
        }
    }

    /// <summary>
    /// Gets or sets the target device IP. For Phone this value is Device, for emulators "Mobile Emulator 10.0.15063.0 WVGA 4 inch 1GB"
    /// </summary>
    public string? TargetDevice { get; set; }

    /// <summary>
    /// Gets or sets the paths used for test adapters lookup in test platform.
    /// </summary>
    public string? TestAdaptersPaths
    {
        get => _testAdaptersPaths;

        set
        {
            _testAdaptersPaths = value;

            if (_testAdaptersPaths != null)
            {
                TestAdaptersPathsSet = true;
            }
        }
    }

    /// <summary>
    /// Gets or sets the execution thread apartment state.
    /// </summary>
    [CLSCompliant(false)]
    public PlatformApartmentState ExecutionThreadApartmentState { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to treat the errors from test adapters as warnings.
    /// </summary>
    public bool TreatTestAdapterErrorsAsWarnings
    {
        get;
        set;
    }

    /// <summary>
    /// Gets a value indicating whether target platform set.
    /// </summary>
    public bool TargetPlatformSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether default platform is set.
    /// </summary>
    public bool DefaultPlatformSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether maximum parallelization count is set.
    /// </summary>
    public bool MaxCpuCountSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating batch size is set
    /// </summary>
    public bool BatchSizeSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether design mode is set.
    /// </summary>
    public bool DesignModeSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether disable appdomain is set.
    /// </summary>
    public bool DisableAppDomainSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether parallelism needs to be disabled by the adapters.
    /// </summary>
    public bool DisableParallelizationSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether target framework set.
    /// </summary>
    public bool TargetFrameworkSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether test adapters paths set.
    /// </summary>
    [MemberNotNullWhen(true, nameof(TestAdaptersPaths))]
    public bool TestAdaptersPathsSet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets a value indicating whether results directory is set.
    /// </summary>
    public bool ResultsDirectorySet
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the binaries root.
    /// </summary>
    public string? BinariesRoot { get; private set; }

    /// <summary>
    /// Collect source information
    /// </summary>
    public bool CollectSourceInformationSet { get; private set; }

    /// <summary>
    /// Default filter to use to filter tests
    /// </summary>
    public string? TestCaseFilter { get; private set; }

    /// Path to dotnet executable to be used to invoke testhost.dll. Specifying this will skip looking up testhost.exe and will force usage of the testhost.dll.
    /// </summary>
    public string? DotnetHostPath { get; private set; }

    /// <inheritdoc/>
    public override XmlElement ToXml()
    {
        XmlDocument doc = new();

        XmlElement root = doc.CreateElement(Constants.RunConfigurationSettingsName);

        XmlElement resultDirectory = doc.CreateElement("ResultsDirectory");
        resultDirectory.InnerXml = ResultsDirectory;
        root.AppendChild(resultDirectory);

        XmlElement targetPlatform = doc.CreateElement("TargetPlatform");
        targetPlatform.InnerXml = TargetPlatform.ToString();
        root.AppendChild(targetPlatform);

        if (DefaultPlatform != null)
        {
            XmlElement defaultPlatform = doc.CreateElement("DefaultPlatform");
            defaultPlatform.InnerXml = DefaultPlatform.ToString()!;
            root.AppendChild(defaultPlatform);
        }

        XmlElement maxCpuCount = doc.CreateElement("MaxCpuCount");
        maxCpuCount.InnerXml = MaxCpuCount.ToString(CultureInfo.CurrentCulture);
        root.AppendChild(maxCpuCount);

        XmlElement batchSize = doc.CreateElement("BatchSize");
        batchSize.InnerXml = BatchSize.ToString(CultureInfo.CurrentCulture);
        root.AppendChild(batchSize);

        XmlElement testSessionTimeout = doc.CreateElement("TestSessionTimeout");
        testSessionTimeout.InnerXml = TestSessionTimeout.ToString(CultureInfo.CurrentCulture);
        root.AppendChild(testSessionTimeout);

        XmlElement designMode = doc.CreateElement("DesignMode");
        designMode.InnerXml = DesignMode.ToString();
        root.AppendChild(designMode);

        XmlElement inIsolation = doc.CreateElement("InIsolation");
        inIsolation.InnerXml = InIsolation.ToString();
        root.AppendChild(inIsolation);

        XmlElement collectSourceInformation = doc.CreateElement("CollectSourceInformation");
        collectSourceInformation.InnerXml = ShouldCollectSourceInformation.ToString();
        root.AppendChild(collectSourceInformation);

        XmlElement disableAppDomain = doc.CreateElement("DisableAppDomain");
        disableAppDomain.InnerXml = DisableAppDomain.ToString();
        root.AppendChild(disableAppDomain);

        XmlElement disableParallelization = doc.CreateElement("DisableParallelization");
        disableParallelization.InnerXml = DisableParallelization.ToString();
        root.AppendChild(disableParallelization);

        XmlElement targetFrameworkVersion = doc.CreateElement("TargetFrameworkVersion");
        targetFrameworkVersion.InnerXml = TargetFramework?.ToString()!;
        root.AppendChild(targetFrameworkVersion);

        XmlElement executionThreadApartmentState = doc.CreateElement("ExecutionThreadApartmentState");
        executionThreadApartmentState.InnerXml = ExecutionThreadApartmentState.ToString();
        root.AppendChild(executionThreadApartmentState);

        if (TestAdaptersPaths != null)
        {
            XmlElement testAdaptersPaths = doc.CreateElement("TestAdaptersPaths");
            testAdaptersPaths.InnerXml = TestAdaptersPaths;
            root.AppendChild(testAdaptersPaths);
        }

        if (TestAdapterLoadingStrategy != TestAdapterLoadingStrategy.Default)
        {
            XmlElement adapterLoadingStrategy = doc.CreateElement("TestAdapterLoadingStrategy");
            adapterLoadingStrategy.InnerXml = TestAdapterLoadingStrategy.ToString();
            root.AppendChild(adapterLoadingStrategy);
        }

        XmlElement treatTestAdapterErrorsAsWarnings = doc.CreateElement("TreatTestAdapterErrorsAsWarnings");
        treatTestAdapterErrorsAsWarnings.InnerXml = TreatTestAdapterErrorsAsWarnings.ToString();
        root.AppendChild(treatTestAdapterErrorsAsWarnings);

        if (BinariesRoot != null)
        {
            XmlElement binariesRoot = doc.CreateElement("BinariesRoot");
            binariesRoot.InnerXml = BinariesRoot;
            root.AppendChild(binariesRoot);
        }

        if (!StringUtils.IsNullOrEmpty(TargetDevice))
        {
            XmlElement targetDevice = doc.CreateElement("TargetDevice");
            targetDevice.InnerXml = TargetDevice;
            root.AppendChild(targetDevice);
        }

        if (!StringUtils.IsNullOrEmpty(TestCaseFilter))
        {
            XmlElement testCaseFilter = doc.CreateElement(nameof(TestCaseFilter));
            testCaseFilter.InnerXml = TestCaseFilter;
            root.AppendChild(testCaseFilter);
        }

        if (!StringUtils.IsNullOrEmpty(DotnetHostPath))
        {
            XmlElement dotnetHostPath = doc.CreateElement(nameof(DotnetHostPath));
            dotnetHostPath.InnerXml = DotnetHostPath;
            root.AppendChild(dotnetHostPath);
        }

        if (TreatNoTestsAsError)
        {
            XmlElement treatAsError = doc.CreateElement(nameof(TreatNoTestsAsError));
            treatAsError.InnerText = TreatNoTestsAsError.ToString();
            root.AppendChild(treatAsError);
        }

        return root;
    }

    /// <summary>
    /// Loads RunConfiguration from XmlReader.
    /// </summary>
    /// <param name="reader">XmlReader having run configuration node.</param>
    /// <returns></returns>
    public static RunConfiguration FromXml(XmlReader reader)
    {
        ValidateArg.NotNull(reader, nameof(reader));
        var runConfiguration = new RunConfiguration();
        var empty = reader.IsEmptyElement;

        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

        // Process the fields in Xml elements
        reader.Read();
        if (!empty)
        {
            while (reader.NodeType == XmlNodeType.Element)
            {
                string elementName = reader.Name;
                // TODO: make run settings nodes case insensitive?
                switch (elementName)
                {
                    case "ResultsDirectory":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

                        string resultsDir = reader.ReadElementContentAsString();
                        if (StringUtils.IsNullOrEmpty(resultsDir))
                        {
                            throw new SettingsException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Resources.Resources.InvalidSettingsIncorrectValue,
                                    Constants.RunConfigurationSettingsName,
                                    resultsDir,
                                    elementName));
                        }

                        runConfiguration.ResultsDirectory = resultsDir;
                        break;

                    case "CollectSourceInformation":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        string collectSourceInformationStr = reader.ReadElementContentAsString();

                        bool bCollectSourceInformation;
                        if (!bool.TryParse(collectSourceInformationStr, out bCollectSourceInformation))
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, bCollectSourceInformation, elementName));
                        }

                        runConfiguration.ShouldCollectSourceInformation = bCollectSourceInformation;
                        break;

                    case "MaxCpuCount":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

                        string cpuCount = reader.ReadElementContentAsString();
                        if (!int.TryParse(cpuCount, out int count) || count < 0)
                        {
                            throw new SettingsException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Resources.Resources.InvalidSettingsIncorrectValue,
                                    Constants.RunConfigurationSettingsName,
                                    cpuCount,
                                    elementName));
                        }

                        runConfiguration.MaxCpuCount = count;
                        break;

                    case "BatchSize":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

                        string batchSize = reader.ReadElementContentAsString();
                        if (!long.TryParse(batchSize, out long size) || size < 0)
                        {
                            throw new SettingsException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Resources.Resources.InvalidSettingsIncorrectValue,
                                    Constants.RunConfigurationSettingsName,
                                    batchSize,
                                    elementName));
                        }

                        runConfiguration.BatchSize = size;
                        break;

                    case "TestSessionTimeout":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

                        string testSessionTimeout = reader.ReadElementContentAsString();
                        if (!long.TryParse(testSessionTimeout, out long sessionTimeout) || sessionTimeout < 0)
                        {
                            throw new SettingsException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Resources.Resources.InvalidSettingsIncorrectValue,
                                    Constants.RunConfigurationSettingsName,
                                    testSessionTimeout,
                                    elementName));
                        }

                        runConfiguration.TestSessionTimeout = sessionTimeout;
                        break;

                    case "DesignMode":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

                        string designModeValueString = reader.ReadElementContentAsString();
                        if (!bool.TryParse(designModeValueString, out bool designMode))
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, designModeValueString, elementName));
                        }
                        runConfiguration.DesignMode = designMode;
                        break;

                    case "InIsolation":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

                        string inIsolationValueString = reader.ReadElementContentAsString();
                        if (!bool.TryParse(inIsolationValueString, out bool inIsolation))
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, inIsolationValueString, elementName));
                        }
                        runConfiguration.InIsolation = inIsolation;
                        break;

                    case "DisableAppDomain":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

                        string disableAppDomainValueString = reader.ReadElementContentAsString();
                        if (!bool.TryParse(disableAppDomainValueString, out bool disableAppDomainCheck))
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, disableAppDomainValueString, elementName));
                        }
                        runConfiguration.DisableAppDomain = disableAppDomainCheck;
                        break;

                    case "DisableParallelization":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);

                        string disableParallelizationValueString = reader.ReadElementContentAsString();
                        if (!bool.TryParse(disableParallelizationValueString, out bool disableParallelizationCheck))
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, disableParallelizationValueString, elementName));
                        }
                        runConfiguration.DisableParallelization = disableParallelizationCheck;
                        break;

                    case "TargetPlatform":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        Architecture archType;
                        string value = reader.ReadElementContentAsString();
                        try
                        {
                            archType = (Architecture)Enum.Parse(typeof(Architecture), value, true);
                            // Ensure that the parsed value is actually in the enum, and that Default or AnyCpu are not provided.
                            if (!Enum.IsDefined(typeof(Architecture), archType) || Architecture.Default == archType || Architecture.AnyCPU == archType)
                            {
                                throw new SettingsException(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        Resources.Resources.InvalidSettingsIncorrectValue,
                                        Constants.RunConfigurationSettingsName,
                                        value,
                                        elementName));
                            }
                        }
                        catch (ArgumentException)
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, value, elementName));
                        }

                        runConfiguration.TargetPlatform = archType;
                        break;

                    case nameof(DefaultPlatform):
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        Architecture defaultArchType;
                        string defaultPlatformValue = reader.ReadElementContentAsString();
                        try
                        {
                            defaultArchType = (Architecture)Enum.Parse(typeof(Architecture), defaultPlatformValue, true);
                            // Ensure that the parsed value is actually in the enum, and that Default or AnyCpu are not provided.
                            if (!Enum.IsDefined(typeof(Architecture), defaultArchType) || Architecture.Default == defaultArchType || Architecture.AnyCPU == defaultArchType)
                            {
                                throw new SettingsException(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        Resources.Resources.InvalidSettingsIncorrectValue,
                                        Constants.RunConfigurationSettingsName,
                                        defaultPlatformValue,
                                        elementName));
                            }
                        }
                        catch (ArgumentException)
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, defaultPlatformValue, elementName));
                        }

                        runConfiguration.DefaultPlatform = defaultArchType;
                        break;

                    case "TargetFrameworkVersion":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        Framework? frameworkType;
                        value = reader.ReadElementContentAsString();
                        try
                        {
                            frameworkType = Framework.FromString(value);

                            if (frameworkType == null)
                            {
                                throw new SettingsException(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        Resources.Resources.InvalidSettingsIncorrectValue,
                                        Constants.RunConfigurationSettingsName,
                                        value,
                                        elementName));
                            }
                        }
                        catch (ArgumentException)
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, value, elementName));
                        }

                        runConfiguration.TargetFramework = frameworkType;
                        break;

                    case "TestAdaptersPaths":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        runConfiguration.TestAdaptersPaths = reader.ReadElementContentAsString();
                        break;

                    case "TestAdapterLoadingStrategy":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        value = reader.ReadElementContentAsString();
                        runConfiguration.TestAdapterLoadingStrategy = Enum.TryParse<TestAdapterLoadingStrategy>(value, out var loadingStrategy)
                            ? loadingStrategy
                            : throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                    Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, value, elementName));

                        break;

                    case "TreatTestAdapterErrorsAsWarnings":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        bool treatTestAdapterErrorsAsWarnings;

                        value = reader.ReadElementContentAsString();

                        try
                        {
                            treatTestAdapterErrorsAsWarnings = bool.Parse(value);
                        }
                        catch (ArgumentException)
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, value, elementName));
                        }
                        catch (FormatException)
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, value, elementName));
                        }

                        runConfiguration.TreatTestAdapterErrorsAsWarnings = treatTestAdapterErrorsAsWarnings;
                        break;

                    case "SolutionDirectory":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        string? solutionDirectory = reader.ReadElementContentAsString();
                        solutionDirectory = Environment.ExpandEnvironmentVariables(solutionDirectory);

                        if (solutionDirectory.IsNullOrEmpty()
                            || !System.IO.Directory.Exists(solutionDirectory))
                        {
                            EqtTrace.Error(string.Format(CultureInfo.CurrentCulture, Resources.Resources.SolutionDirectoryNotExists, solutionDirectory));
                            solutionDirectory = null;
                        }

                        runConfiguration.SolutionDirectory = solutionDirectory;

                        break;

                    case "BinariesRoot":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        runConfiguration.BinariesRoot = reader.ReadElementContentAsString();
                        break;

                    case "ExecutionThreadApartmentState":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        string executionThreadApartmentState = reader.ReadElementContentAsString();
                        if (!Enum.TryParse(executionThreadApartmentState, out PlatformApartmentState apartmentState))
                        {
                            throw new SettingsException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    Resources.Resources.InvalidSettingsIncorrectValue,
                                    Constants.RunConfigurationSettingsName,
                                    executionThreadApartmentState,
                                    elementName));
                        }

                        runConfiguration.ExecutionThreadApartmentState = apartmentState;
                        break;

                    case "TargetDevice":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        runConfiguration.TargetDevice = reader.ReadElementContentAsString();
                        break;

                    case "TestCaseFilter":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        runConfiguration.TestCaseFilter = reader.ReadElementContentAsString();
                        break;

                    case "DotNetHostPath":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        string? dotnetHostPath = reader.ReadElementContentAsString();

#if !NETSTANDARD1_0
                        dotnetHostPath = Environment.ExpandEnvironmentVariables(dotnetHostPath);
#endif

                        runConfiguration.DotnetHostPath = dotnetHostPath;
                        break;
                    case "TreatNoTestsAsError":
                        XmlRunSettingsUtilities.ThrowOnHasAttributes(reader);
                        string treatNoTestsAsErrorValueString = reader.ReadElementContentAsString();
                        if (!bool.TryParse(treatNoTestsAsErrorValueString, out bool treatNoTestsAsError))
                        {
                            throw new SettingsException(string.Format(CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsIncorrectValue, Constants.RunConfigurationSettingsName, treatNoTestsAsErrorValueString, elementName));
                        }
                        runConfiguration.TreatNoTestsAsError = treatNoTestsAsError;
                        break;

                    default:
                        // Ignore a runsettings element that we don't understand. It could occur in the case
                        // the test runner is of a newer version, but the test host is of an earlier version.
                        EqtTrace.Warning(
                            string.Format(
                                CultureInfo.CurrentCulture,
                                Resources.Resources.InvalidSettingsXmlElement,
                                Constants.RunConfigurationSettingsName,
                                reader.Name));
                        reader.Skip();
                        break;
                }
            }

            reader.ReadEndElement();
        }

        return runConfiguration;
    }
}
