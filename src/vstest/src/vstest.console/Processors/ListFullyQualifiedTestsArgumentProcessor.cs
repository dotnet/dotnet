// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestPlatform.Client.RequestHelper;
using Microsoft.VisualStudio.TestPlatform.CommandLine.Internal;
using Microsoft.VisualStudio.TestPlatform.CommandLine.TestPlatformHelpers;
using Microsoft.VisualStudio.TestPlatform.Common;
using Microsoft.VisualStudio.TestPlatform.Common.Filtering;
using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.Utilities;

using CommandLineResources = Microsoft.VisualStudio.TestPlatform.CommandLine.Resources.Resources;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors;

/// <summary>
/// Argument Executor for the "--ListFullyQualifiedTests|/ListFullyQualifiedTests" command line argument.
/// </summary>
internal class ListFullyQualifiedTestsArgumentProcessor : IArgumentProcessor
{
    /// <summary>
    /// The name of the command line argument that the ListFullyQualifiedTestsArgumentProcessor handles.
    /// </summary>
    public const string CommandName = "/ListFullyQualifiedTests";

    private Lazy<IArgumentProcessorCapabilities>? _metadata;
    private Lazy<IArgumentExecutor>? _executor;

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public Lazy<IArgumentProcessorCapabilities> Metadata
        => _metadata ??= new Lazy<IArgumentProcessorCapabilities>(() =>
            new ListFullyQualifiedTestsArgumentProcessorCapabilities());

    /// <summary>
    /// Gets or sets the executor.
    /// </summary>
    public Lazy<IArgumentExecutor>? Executor
    {
        get => _executor ??= new Lazy<IArgumentExecutor>(() =>
            new ListFullyQualifiedTestsArgumentExecutor(
                CommandLineOptions.Instance,
                RunSettingsManager.Instance,
                TestRequestManager.Instance));

        set => _executor = value;
    }
}

internal class ListFullyQualifiedTestsArgumentProcessorCapabilities : BaseArgumentProcessorCapabilities
{
    public override string CommandName => ListFullyQualifiedTestsArgumentProcessor.CommandName;

    public override bool AllowMultiple => false;

    public override bool IsAction => true;

    public override ArgumentProcessorPriority Priority => ArgumentProcessorPriority.Normal;
}

/// <summary>
/// Argument Executor for the "/ListTests" command line argument.
/// </summary>
internal class ListFullyQualifiedTestsArgumentExecutor : IArgumentExecutor
{
    /// <summary>
    /// Used for getting sources.
    /// </summary>
    private readonly CommandLineOptions _commandLineOptions;

    /// <summary>
    /// Used for getting tests.
    /// </summary>
    private readonly ITestRequestManager _testRequestManager;

    /// <summary>
    /// Used for sending output.
    /// </summary>
    internal IOutput Output;

    /// <summary>
    /// RunSettingsManager to get currently active run settings.
    /// </summary>
    private readonly IRunSettingsProvider _runSettingsManager;

    /// <summary>
    /// Registers for discovery events during discovery
    /// </summary>
    private readonly ITestDiscoveryEventsRegistrar _discoveryEventsRegistrar;

    /// <summary>
    /// List to store the discovered tests
    /// </summary>
    private readonly List<string> _discoveredTests = new();

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    public ListFullyQualifiedTestsArgumentExecutor(
        CommandLineOptions options,
        IRunSettingsProvider runSettingsProvider,
        ITestRequestManager testRequestManager) :
        this(options, runSettingsProvider, testRequestManager, ConsoleOutput.Instance)
    {
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="options">
    /// The options.
    /// </param>
    internal ListFullyQualifiedTestsArgumentExecutor(
        CommandLineOptions options,
        IRunSettingsProvider runSettingsProvider,
        ITestRequestManager testRequestManager,
        IOutput output)
    {
        ValidateArg.NotNull(options, nameof(options));

        _commandLineOptions = options;
        Output = output;
        _testRequestManager = testRequestManager;

        _runSettingsManager = runSettingsProvider;
        _discoveryEventsRegistrar = new DiscoveryEventsRegistrar(_discoveredTests, _commandLineOptions);
    }

    #region IArgumentExecutor

    /// <summary>
    /// Initializes with the argument that was provided with the command.
    /// </summary>
    /// <param name="argument">Argument that was provided with the command.</param>
    public void Initialize(string? argument)
    {
        if (!argument.IsNullOrWhiteSpace())
        {
            _commandLineOptions.AddSource(argument);
        }
    }

    /// <summary>
    /// Lists out the available discoverers.
    /// </summary>
    public ArgumentProcessorResult Execute()
    {
        TPDebug.Assert(Output != null);
        TPDebug.Assert(_commandLineOptions != null);
        TPDebug.Assert(!StringUtils.IsNullOrWhiteSpace(_runSettingsManager?.ActiveRunSettings?.SettingsXml));

        if (!_commandLineOptions.Sources.Any())
        {
            throw new CommandLineException(CommandLineResources.MissingTestSourceFile);
        }

        if (!EqtTrace.LogFile.IsNullOrEmpty())
        {
            Output.Information(false, CommandLineResources.VstestDiagLogOutputPath, EqtTrace.LogFile);
        }

        var runSettings = _runSettingsManager.ActiveRunSettings.SettingsXml;

        _testRequestManager.DiscoverTests(
            new DiscoveryRequestPayload { Sources = _commandLineOptions.Sources, RunSettings = runSettings },
            _discoveryEventsRegistrar, Constants.DefaultProtocolConfig);

        if (_commandLineOptions.ListTestsTargetPath.IsNullOrEmpty())
        {
            // This string does not need to go to Resources. Reason - only internal consumption
            throw new CommandLineException("Target Path should be specified for listing FQDN tests!");
        }

        File.WriteAllLines(_commandLineOptions.ListTestsTargetPath, _discoveredTests);
        return ArgumentProcessorResult.Success;
    }

    #endregion

    private class DiscoveryEventsRegistrar : ITestDiscoveryEventsRegistrar
    {
        private readonly List<string> _discoveredTests;
        private readonly CommandLineOptions _options;

        public DiscoveryEventsRegistrar(List<string> discoveredTests, CommandLineOptions cmdOptions)
        {
            _discoveredTests = discoveredTests;
            _options = cmdOptions;
        }

        public void LogWarning(string message)
        {
            ConsoleLogger.RaiseTestRunWarning(message);
        }
        public void RegisterDiscoveryEvents(IDiscoveryRequest discoveryRequest)
        {
            discoveryRequest.OnDiscoveredTests += DiscoveryRequest_OnDiscoveredTests;
        }

        public void UnregisterDiscoveryEvents(IDiscoveryRequest discoveryRequest)
        {
            discoveryRequest.OnDiscoveredTests -= DiscoveryRequest_OnDiscoveredTests;
        }

        private void DiscoveryRequest_OnDiscoveredTests(object? sender, DiscoveredTestsEventArgs args)
        {
            if (args == null)
            {
                throw new TestPlatformException("DiscoveredTestsEventArgs cannot be null.");
            }

            // Initializing the test case filter here because the filter value is read late.
            TestCaseFilter.Initialize(_options.TestCaseFilterValue);
            var discoveredTests = args.DiscoveredTestCases!.ToList();
            var filteredTests = TestCaseFilter.FilterTests(discoveredTests).ToList();

            // remove any duplicate tests
            filteredTests = filteredTests.Select(test => test.FullyQualifiedName)
                .Distinct()
                .Select(fqdn => filteredTests.First(test => test.FullyQualifiedName == fqdn))
                .ToList();
            _discoveredTests.AddRange(filteredTests.Select(test => test.FullyQualifiedName));
        }
    }

    private static class TestCaseFilter
    {
        private static TestCaseFilterExpression? s_filterExpression;
        private const string TestCategory = "TestCategory";
        private const string Category = "Category";
        private const string Traits = "Traits";

        public static void Initialize(string? filterString)
        {
            ValidateFilter(filterString);
        }

        /// <summary>
        /// Filter tests
        /// </summary>
        public static IEnumerable<TestCase> FilterTests(IEnumerable<TestCase> testCases)
        {
            EqtTrace.Verbose("TestCaseFilter.FilterTests : Test Filtering invoked.");

            List<TestCase> filteredList;

            try
            {
                filteredList = GetFilteredTestCases(testCases);
            }
            catch (Exception ex)
            {
                EqtTrace.Error("TestCaseFilter.FilterTests : Exception during filtering : {0}", ex.ToString());
                throw;
            }

            return filteredList;
        }

        private static void ValidateFilter(string? filterString)
        {
            if (filterString.IsNullOrEmpty())
            {
                s_filterExpression = null;
                return;
            }

            var filterWrapper = new FilterExpressionWrapper(filterString);

            if (filterWrapper.ParseError != null)
            {
                var fe = new FormatException($"Invalid Test Case Filter: {filterString}");
                EqtTrace.Error("TestCaseFilter.ValidateFilter : Filtering failed with exception : " + fe.Message);
                throw fe;
            }

            s_filterExpression = new TestCaseFilterExpression(filterWrapper);

        }

        /// <summary>
        /// get list of test cases that satisfy the filter criteria
        /// </summary>
        private static List<TestCase> GetFilteredTestCases(IEnumerable<TestCase> testCases)
        {
            var filteredList = new List<TestCase>();

            if (s_filterExpression == null)
            {
                filteredList = testCases.ToList();
                return filteredList;
            }

            foreach (var testCase in testCases)
            {
                var traitDictionary = GetTestPropertiesInTraitDictionary(testCase);// Dictionary with trait key to value mapping
                traitDictionary = GetTraitsInTraitDictionary(traitDictionary, testCase.Traits);

                // Skip test if not fitting filter criteria.
                if (!s_filterExpression.MatchTestCase(testCase, p => PropertyValueProvider(p, traitDictionary)))
                {
                    continue;
                }

                filteredList.Add(testCase);
            }

            return filteredList;
        }

        /// <summary>
        /// fetch the test properties on this test method as traits and populate a trait dictionary
        /// </summary>
        private static Dictionary<string, List<string>> GetTestPropertiesInTraitDictionary(TestCase testCase)
        {
            var traitDictionary = new Dictionary<string, List<string>>();
            foreach (var testProperty in testCase.Properties)
            {
                string testPropertyKey = testProperty.Label;

                if (testPropertyKey.Equals(Traits))
                {
                    // skip the "Traits" property. traits to be set separately
                    continue;
                }

                var testPropertyValue = testCase.GetPropertyValue(testProperty);

                if (testPropertyKey.Equals(TestCategory))
                {
                    if (testPropertyValue is string[] testPropertyValueArray)
                    {
                        var testPropertyValueList = new List<string>(testPropertyValueArray);
                        traitDictionary.Add(testPropertyKey, testPropertyValueList);
                        continue;
                    }
                }

                //always return value as a list of string
                if (testPropertyValue != null)
                {
                    var multiValue = new List<string> { testPropertyValue.ToString()! };
                    traitDictionary.Add(testPropertyKey, multiValue);
                }
            }

            return traitDictionary;
        }

        /// <summary>
        /// fetch the traits on this test method and populate a trait dictionary
        /// </summary>
        private static Dictionary<string, List<string>> GetTraitsInTraitDictionary(Dictionary<string, List<string>> traitDictionary, TraitCollection traits)
        {
            foreach (var trait in traits)
            {
                var newTraitValueList = new List<string> { trait.Value };
                if (!traitDictionary.TryGetValue(trait.Name, out List<string>? currentTraitValue))
                {
                    // if the current trait's key is not already present, add the current trait key-value pair
                    traitDictionary.Add(trait.Name, newTraitValueList);
                }
                else
                {
                    if (null == currentTraitValue)
                    {
                        // if the current trait's value is null, replace the previous value with the current value
                        traitDictionary[trait.Name] = newTraitValueList;
                    }
                    else
                    {
                        // if the current trait's key is already present and is not null, append current value to the previous value list
                        List<string> traitValueList = currentTraitValue;
                        traitValueList.Add(trait.Value);
                    }
                }
            }

            //This is hack for NUnit, XUnit to understand test category -> This method is called only for NUnit/XUnit
            if (!traitDictionary.ContainsKey(TestCategory) && traitDictionary.ContainsKey(Category))
            {
                traitDictionary.TryGetValue(Category, out var categoryValue);
                traitDictionary.Add(TestCategory, categoryValue!);
            }

            return traitDictionary;
        }

        /// <summary>
        /// Provides value for property name 'propertyName' as used in filter.
        /// </summary>
        private static string[]? PropertyValueProvider(string propertyName, Dictionary<string, List<string>> traitDictionary)
        {
            traitDictionary.TryGetValue(propertyName, out List<string>? propertyValueList);
            if (propertyValueList != null)
            {
                var propertyValueArray = propertyValueList.ToArray();

                return propertyValueArray;
            }
            return null;
        }
    }
}
