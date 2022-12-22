// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.DataCollector;
using Microsoft.VisualStudio.TestPlatform.Common.Telemetry;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

using SimpleJSON;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;

/// <summary>
/// The test extension information.
/// </summary>
public class TestExtensions
{
    /// <summary>
    /// Gets or sets test discoverer extensions.
    /// </summary>
    internal Dictionary<string, TestDiscovererPluginInformation>? TestDiscoverers { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether are test discoverers cached.
    /// </summary>
    internal bool AreTestDiscoverersCached { get; set; }

    /// <summary>
    /// Gets or sets test executor extensions.
    /// </summary>
    internal Dictionary<string, TestExecutorPluginInformation>? TestExecutors { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether are test executors cached.
    /// </summary>
    internal bool AreTestExecutorsCached { get; set; }

    /// <summary>
    /// Gets or sets test executor 2 extensions.
    /// </summary>
    internal Dictionary<string, TestExecutorPluginInformation2>? TestExecutors2 { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether are test executors 2 cached.
    /// </summary>
    internal bool AreTestExecutors2Cached { get; set; }

    /// <summary>
    /// Gets or sets test setting provider extensions.
    /// </summary>
    internal Dictionary<string, TestSettingsProviderPluginInformation>? TestSettingsProviders { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether are test settings providers cached.
    /// </summary>
    internal bool AreTestSettingsProvidersCached { get; set; }

    /// <summary>
    /// Gets or sets test logger extensions.
    /// </summary>
    internal Dictionary<string, TestLoggerPluginInformation>? TestLoggers { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether are test loggers cached.
    /// </summary>
    internal bool AreTestLoggersCached { get; set; }

    /// <summary>
    /// Gets or sets test logger extensions.
    /// </summary>
    internal Dictionary<string, TestRuntimePluginInformation>? TestHosts { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether are test hosts cached.
    /// </summary>
    internal bool AreTestHostsCached { get; set; }

    /// <summary>
    /// Gets or sets data collectors extensions.
    /// </summary>
    internal Dictionary<string, DataCollectorConfig>? DataCollectors { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether are test hosts cached.
    /// </summary>
    internal bool AreDataCollectorsCached { get; set; }

    /// <summary>
    /// Merge two extension dictionaries.
    /// </summary>
    ///
    /// <param name="first">First extension dictionary.</param>
    /// <param name="second">Second extension dictionary.</param>
    ///
    /// <returns>
    /// A dictionary representing the merger between the two input dictionaries.
    /// </returns>
    internal static Dictionary<string, HashSet<string>> CreateMergedDictionary(
        Dictionary<string, HashSet<string>>? first,
        Dictionary<string, HashSet<string>>? second)
    {
        var isFirstNullOrEmpty = first == null || first.Count == 0;
        var isSecondNullOrEmpty = second == null || second.Count == 0;

        // Sanity checks.
        if (isFirstNullOrEmpty && isSecondNullOrEmpty)
        {
            return new Dictionary<string, HashSet<string>>();
        }

        if (isFirstNullOrEmpty)
        {
            return new Dictionary<string, HashSet<string>>(second!);
        }

        if (isSecondNullOrEmpty)
        {
            return new Dictionary<string, HashSet<string>>(first!);
        }

        // Copy all the keys in the first dictionary into the resulting dictionary.
        var result = first!.Where(kvp => kvp.Value != null && kvp.Value.Count > 0)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        foreach (var kvp in second!)
        {
            // If the "source" set is empty there's no reason to continue merging for this key.
            if (kvp.Value == null || kvp.Value.Count == 0)
            {
                continue;
            }

            // If there's no key-value pair entry in the "destination" dictionary for the current
            // key in the "source" dictionary, we copy the "source" set wholesale.
            if (!result.ContainsKey(kvp.Key))
            {
                result.Add(kvp.Key, kvp.Value);
                continue;
            }

            // Getting here means there's already an entry for the "source" key in the "destination"
            // dictionary which means we need to copy individual set elements from the "source" set
            // to the "destination" set.
            result[kvp.Key] = MergeSets(result[kvp.Key], kvp.Value);
        }

        return result;
    }

    /// <summary>
    /// Add extension-related telemetry.
    /// </summary>
    ///
    /// <param name="metrics">A collection representing the telemetry data.</param>
    /// <param name="extensions">The input extension collection.</param>
    internal static void AddExtensionTelemetry(
        IDictionary<string, object> metrics,
        Dictionary<string, HashSet<string>> extensions)
    {
        metrics.Add(
            TelemetryDataConstants.DiscoveredExtensions,
            SerializeExtensionDictionary(extensions));
    }

    /// <summary>
    /// Adds the extensions specified to the current set of extensions.
    /// </summary>
    /// <typeparam name="TPluginInfo">
    /// Type of plugin info.
    /// </typeparam>
    /// <param name="newExtensions">
    /// The info about new extensions discovered
    /// </param>
    /// <returns>
    /// The <see cref="Dictionary"/> of extensions discovered
    /// </returns>
    internal Dictionary<string, TPluginInfo>? AddExtension<TPluginInfo>(Dictionary<string, TPluginInfo>? newExtensions)
        where TPluginInfo : TestPluginInformation
    {
        var existingExtensions = GetTestExtensionCache<TPluginInfo>();
        if (newExtensions == null)
        {
            return existingExtensions;
        }

        if (existingExtensions == null)
        {
            SetTestExtensionCache(newExtensions);
            return newExtensions;
        }

        foreach (var extension in newExtensions)
        {
            if (existingExtensions.ContainsKey(extension.Key))
            {
                EqtTrace.Warning(
                    "TestExtensions.AddExtensions: Attempt to add multiple test extensions with identifier data '{0}'",
                    extension.Key);
            }
            else
            {
                existingExtensions.Add(extension.Key, extension.Value);
            }
        }

        return existingExtensions;
    }

    /// <summary>
    /// Gets the extensions already discovered that are defined in the specified assembly.
    /// </summary>
    /// <param name="extensionAssembly"> The extension assembly.</param>
    /// <returns> The test extensions defined the extension assembly if it is already discovered. null if not.</returns>
    internal TestExtensions? GetExtensionsDiscoveredFromAssembly(string? extensionAssembly)
    {
        var testExtensions = new TestExtensions();

        testExtensions.TestDiscoverers =
            GetExtensionsDiscoveredFromAssembly(TestDiscoverers, extensionAssembly);
        testExtensions.TestExecutors =
            GetExtensionsDiscoveredFromAssembly(TestExecutors, extensionAssembly);
        testExtensions.TestExecutors2 =
            GetExtensionsDiscoveredFromAssembly(TestExecutors2, extensionAssembly);
        testExtensions.TestSettingsProviders =
            GetExtensionsDiscoveredFromAssembly(TestSettingsProviders, extensionAssembly);
        testExtensions.TestLoggers =
            GetExtensionsDiscoveredFromAssembly(TestLoggers, extensionAssembly);
        testExtensions.TestHosts =
            GetExtensionsDiscoveredFromAssembly(TestHosts, extensionAssembly);
        testExtensions.DataCollectors =
            GetExtensionsDiscoveredFromAssembly(DataCollectors, extensionAssembly);

        if (testExtensions.TestDiscoverers.Any()
            || testExtensions.TestExecutors.Any()
            || testExtensions.TestExecutors2.Any()
            || testExtensions.TestSettingsProviders.Any()
            || testExtensions.TestLoggers.Any()
            || testExtensions.TestHosts.Any()
            || testExtensions.DataCollectors.Any())
        {
            // This extension has already been discovered.
            return testExtensions;
        }

        return null;
    }

    internal Dictionary<string, TPluginInfo>? GetTestExtensionCache<TPluginInfo>() where TPluginInfo : TestPluginInformation
    {
        Type type = typeof(TPluginInfo);

        if (type == typeof(TestDiscovererPluginInformation))
        {
            return (Dictionary<string, TPluginInfo>?)(object?)TestDiscoverers;
        }
        else if (type == typeof(TestExecutorPluginInformation))
        {
            return (Dictionary<string, TPluginInfo>?)(object?)TestExecutors;
        }
        else if (type == typeof(TestExecutorPluginInformation2))
        {
            return (Dictionary<string, TPluginInfo>?)(object?)TestExecutors2;
        }
        else if (type == typeof(TestLoggerPluginInformation))
        {
            return (Dictionary<string, TPluginInfo>?)(object?)TestLoggers;
        }
        else if (type == typeof(TestSettingsProviderPluginInformation))
        {
            return (Dictionary<string, TPluginInfo>?)(object?)TestSettingsProviders;
        }
        else if (type == typeof(TestRuntimePluginInformation))
        {
            return (Dictionary<string, TPluginInfo>?)(object?)TestHosts;
        }
        else if (type == typeof(DataCollectorConfig))
        {
            return (Dictionary<string, TPluginInfo>?)(object?)DataCollectors;
        }

        return null;
    }

    /// <summary>
    /// The are test extensions cached.
    /// </summary>
    /// <typeparam name="TPluginInfo">
    /// </typeparam>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    internal bool AreTestExtensionsCached<TPluginInfo>() where TPluginInfo : TestPluginInformation
    {
        Type type = typeof(TPluginInfo);

        if (type == typeof(TestDiscovererPluginInformation))
        {
            return AreTestDiscoverersCached;
        }
        else if (type == typeof(TestExecutorPluginInformation))
        {
            return AreTestExecutorsCached;
        }
        else if (type == typeof(TestExecutorPluginInformation2))
        {
            return AreTestExecutors2Cached;
        }
        else if (type == typeof(TestLoggerPluginInformation))
        {
            return AreTestLoggersCached;
        }
        else if (type == typeof(TestSettingsProviderPluginInformation))
        {
            return AreTestSettingsProvidersCached;
        }
        else if (type == typeof(TestRuntimePluginInformation))
        {
            return AreTestHostsCached;
        }
        else if (type == typeof(DataCollectorConfig))
        {
            return AreDataCollectorsCached;
        }

        return false;
    }

    /// <summary>
    /// The set test extensions cache status.
    /// </summary>
    /// <typeparam name="TPluginInfo">
    /// </typeparam>
    internal void SetTestExtensionsCacheStatusToTrue<TPluginInfo>() where TPluginInfo : TestPluginInformation
    {
        Type type = typeof(TPluginInfo);

        if (type == typeof(TestDiscovererPluginInformation))
        {
            AreTestDiscoverersCached = true;
        }
        else if (type == typeof(TestExecutorPluginInformation))
        {
            AreTestExecutorsCached = true;
        }
        else if (type == typeof(TestExecutorPluginInformation2))
        {
            AreTestExecutors2Cached = true;
        }
        else if (type == typeof(TestLoggerPluginInformation))
        {
            AreTestLoggersCached = true;
        }
        else if (type == typeof(TestSettingsProviderPluginInformation))
        {
            AreTestSettingsProvidersCached = true;
        }
        else if (type == typeof(TestRuntimePluginInformation))
        {
            AreTestHostsCached = true;
        }
        else if (type == typeof(DataCollectorConfig))
        {
            AreDataCollectorsCached = true;
        }
    }

    /// <summary>
    /// Gets the cached extensions for the current process.
    /// </summary>
    ///
    /// <returns>A dictionary representing the cached extensions for the current process.</returns>
    internal Dictionary<string, HashSet<string>> GetCachedExtensions()
    {
        var extensions = new Dictionary<string, HashSet<string>>();

        // Write all "known" cached extension.
        AddCachedExtensionToDictionary(extensions, "TestDiscoverers", TestDiscoverers?.Values);
        AddCachedExtensionToDictionary(extensions, "TestExecutors", TestExecutors?.Values);
        AddCachedExtensionToDictionary(extensions, "TestExecutors2", TestExecutors2?.Values);
        AddCachedExtensionToDictionary(extensions, "TestSettingsProviders", TestSettingsProviders?.Values);
        AddCachedExtensionToDictionary(extensions, "TestLoggers", TestLoggers?.Values);
        AddCachedExtensionToDictionary(extensions, "TestHosts", TestHosts?.Values);
        AddCachedExtensionToDictionary(extensions, "DataCollectors", DataCollectors?.Values);

        return extensions;
    }

    /// <summary>
    /// The invalidate cache of plugin infos.
    /// </summary>
    internal void InvalidateCache()
    {
        AreTestDiscoverersCached = false;
        AreTestExecutorsCached = false;
        AreTestExecutors2Cached = false;
        AreTestLoggersCached = false;
        AreTestSettingsProvidersCached = false;
        AreTestHostsCached = false;
        AreDataCollectorsCached = false;
    }

    /// <summary>
    /// Gets extensions discovered from assembly.
    /// </summary>
    /// <param name="extensionCollection">
    /// The extension collection.
    /// </param>
    /// <param name="extensionAssembly">
    /// The extension assembly.
    /// </param>
    /// <typeparam name="TPluginInfo">
    /// </typeparam>
    /// <returns>
    /// The <see cref="Dictionary"/>. of extensions discovered in assembly
    /// </returns>
    internal static Dictionary<string, TPluginInfo> GetExtensionsDiscoveredFromAssembly<TPluginInfo>(
        Dictionary<string, TPluginInfo>? extensionCollection,
        string? extensionAssembly)
    {
        var extensions = new Dictionary<string, TPluginInfo>();
        if (extensionCollection != null)
        {
            foreach (var extension in extensionCollection)
            {
                var testPluginInformation = extension.Value as TestPluginInformation;
                // TODO: Avoid ArgumentNullException here
                var extensionType = Type.GetType(testPluginInformation?.AssemblyQualifiedName!);
                if (string.Equals(extensionType?.GetTypeInfo().Assembly.GetAssemblyLocation(), extensionAssembly))
                {
                    extensions.Add(extension.Key, extension.Value);
                }
            }
        }

        return extensions;
    }

    private void SetTestExtensionCache<TPluginInfo>(Dictionary<string, TPluginInfo> testPluginInfos) where TPluginInfo : TestPluginInformation
    {
        Type type = typeof(TPluginInfo);

        if (type == typeof(TestDiscovererPluginInformation))
        {
            TestDiscoverers = (Dictionary<string, TestDiscovererPluginInformation>)(object)testPluginInfos;
        }
        else if (type == typeof(TestExecutorPluginInformation))
        {
            TestExecutors = (Dictionary<string, TestExecutorPluginInformation>)(object)testPluginInfos;
        }
        else if (type == typeof(TestExecutorPluginInformation2))
        {
            TestExecutors2 = (Dictionary<string, TestExecutorPluginInformation2>)(object)testPluginInfos;
        }
        else if (type == typeof(TestLoggerPluginInformation))
        {
            TestLoggers = (Dictionary<string, TestLoggerPluginInformation>)(object)testPluginInfos;
        }
        else if (type == typeof(TestSettingsProviderPluginInformation))
        {
            TestSettingsProviders = (Dictionary<string, TestSettingsProviderPluginInformation>)(object)testPluginInfos;
        }
        else if (type == typeof(TestRuntimePluginInformation))
        {
            TestHosts = (Dictionary<string, TestRuntimePluginInformation>)(object)testPluginInfos;
        }
        else if (type == typeof(DataCollectorConfig))
        {
            DataCollectors = (Dictionary<string, DataCollectorConfig>)(object)testPluginInfos;
        }
    }

    private static void AddCachedExtensionToDictionary<T>(
        Dictionary<string, HashSet<string>> extensionDict,
        string extensionType,
        IEnumerable<T>? extensions)
        where T : TestPluginInformation
    {
        if (extensions == null)
        {
            return;
        }

        extensionDict.Add(extensionType, new HashSet<string>(extensions.Select(e => e.IdentifierData!)));
    }

    private static string SerializeExtensionDictionary(IDictionary<string, HashSet<string>> extensions)
    {
        var jsonObject = new JSONObject();
        foreach (var kvp in extensions)
        {
            if (kvp.Value?.Count > 0)
            {
                var jsonArray = new JSONArray();
                foreach (var extension in kvp.Value)
                {
                    jsonArray.Add(new JSONString(extension));
                }

                jsonObject.Add(kvp.Key, jsonArray);
            }
        }

        return jsonObject.ToString();
    }

    private static HashSet<string> MergeSets(HashSet<string> firstSet, HashSet<string> secondSet)
    {
        var mergedSet = new HashSet<string>(firstSet);

        // No need to worry about duplicates as the set implementation handles this already.
        foreach (var key in secondSet)
        {
            mergedSet.Add(key);
        }

        return mergedSet;
    }
}
