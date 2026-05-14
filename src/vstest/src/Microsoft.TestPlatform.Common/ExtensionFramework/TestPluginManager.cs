// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;

/// <summary>
/// Manages test plugins information.
/// </summary>
internal class TestPluginManager
{
    private static TestPluginManager? s_instance;

    /// <summary>
    /// Gets the singleton instance of TestPluginManager.
    /// </summary>
    public static TestPluginManager Instance
        => s_instance ??= new TestPluginManager();

    /// <summary>
    /// Gets data type of test extension with given assembly qualified name.
    /// </summary>
    /// <param name="extensionTypeName">Assembly qualified name of the test extension</param>
    /// <returns>Data type of the test extension</returns>
    public static Type? GetTestExtensionType(string extensionTypeName)
    {
        Type? extensionType;
        try
        {
            extensionType = Type.GetType(extensionTypeName, true);
        }
        catch (Exception ex)
        {
            EqtTrace.Error(
                "GetTestExtensionType: Failed to get type for test extension '{0}': {1}",
                extensionTypeName,
                ex);
            throw;
        }

        return extensionType;
    }

    /// <summary>
    /// Instantiates a given data type.
    /// </summary>
    /// <typeparam name="T">Return type of the test extension</typeparam>
    /// <param name="extensionType">Data type of the extension to be instantiated</param>
    /// <returns>Test extension instance</returns>
    public static T CreateTestExtension<T>(Type extensionType)
    {
        ValidateArg.NotNull(extensionType, nameof(extensionType));
        EqtTrace.Info("TestPluginManager.CreateTestExtension: Attempting to load test extension: " + extensionType);

        try
        {
            object? rawPlugin = Activator.CreateInstance(extensionType);

            T testExtension = (T)rawPlugin!;
            return testExtension;
        }
        catch (Exception ex)
        {
            if (ex is TargetInvocationException)
            {
                EqtTrace.Error("TestPluginManager.CreateTestExtension: Could not create instance of type: " + extensionType.ToString() + "  Exception: " + ex);
                throw;
            }
#if NETFRAMEWORK
            else if (ex is SystemException)
            {
                EqtTrace.Error("TestPluginManager.CreateTestExtension: Could not create instance of type: " + extensionType.ToString() + "  Exception: " + ex);
                throw;
            }
#endif
            EqtTrace.Error("TestPluginManager.CreateTestExtension: Could not create instance of type: " + extensionType.ToString() + "  Exception: " + ex);

            throw;
        }
    }

    /// <summary>
    /// Retrieves the test extension collections of given extension type.
    /// </summary>
    /// <typeparam name="TPluginInfo">
    /// </typeparam>
    /// <typeparam name="TExtension">
    /// Type of the required extensions
    /// </typeparam>
    /// <typeparam name="IMetadata">
    /// Type of metadata of required extensions
    /// </typeparam>
    /// <typeparam name="TMetadata">
    /// Concrete type of metadata
    /// </typeparam>
    /// <param name="endsWithPattern">
    /// Pattern used to select files using String.EndsWith
    /// </param>
    /// <param name="unfiltered">
    /// Receives unfiltered list of test extensions
    /// </param>
    /// <param name="filtered">
    /// Receives test extensions filtered by Identifier data
    /// </param>
    public static void GetSpecificTestExtensions<TPluginInfo, TExtension, IMetadata, TMetadata>(
        string endsWithPattern,
        out IEnumerable<LazyExtension<TExtension, Dictionary<string, object>>> unfiltered,
        out IEnumerable<LazyExtension<TExtension, IMetadata>> filtered) where TMetadata : IMetadata where TPluginInfo : TestPluginInformation
    {
        var extensions = TestPluginCache.Instance.DiscoverTestExtensions<TPluginInfo, TExtension>(endsWithPattern);
        TPDebug.Assert(extensions is not null, "extensions is null");
        GetExtensions<TPluginInfo, TExtension, IMetadata, TMetadata>(extensions, out unfiltered, out filtered);
    }

    /// <summary>
    /// Retrieves the test extension collections of given extension type for the provided extension assembly.
    /// </summary>
    /// <param name="extensionAssembly">
    /// The extension assembly.
    /// </param>
    /// <typeparam name="TPluginInfo">
    /// </typeparam>
    /// <typeparam name="TExtension">
    /// Type of the required extensions
    /// </typeparam>
    /// <typeparam name="IMetadata">
    /// Type of metadata of required extensions
    /// </typeparam>
    /// <typeparam name="TMetadata">
    /// Concrete type of metadata
    /// </typeparam>
    /// <param name="unfiltered">
    /// Receives unfiltered list of test extensions
    /// </param>
    /// <param name="filtered">
    /// Receives test extensions filtered by Identifier data
    /// </param>
    /// <param name="skipCache">
    /// Skip the extensions cache.
    /// </param>
    public static void GetTestExtensions<TPluginInfo, TExtension, IMetadata, TMetadata>(
        string extensionAssembly,
        out IEnumerable<LazyExtension<TExtension, Dictionary<string, object>>> unfiltered,
        out IEnumerable<LazyExtension<TExtension, IMetadata>> filtered,
        bool skipCache = false) where TMetadata : IMetadata where TPluginInfo : TestPluginInformation
    {
        var extensions = TestPluginCache.Instance.GetTestExtensions<TPluginInfo, TExtension>(extensionAssembly, skipCache);
        GetExtensions<TPluginInfo, TExtension, IMetadata, TMetadata>(extensions, out unfiltered, out filtered);
    }

    /// <summary>
    /// Prepares a List of TestPluginInformation&gt;
    /// </summary>
    /// <typeparam name="T"> Type of TestPluginIInformation. </typeparam>
    /// <param name="dictionary"> The dictionary containing plugin identifier data and its info. </param>
    /// <returns> Collection of test plugins information </returns>
    private static IEnumerable<TestPluginInformation> GetValuesFromDictionary<T>(Dictionary<string, T> dictionary)
        where T : TestPluginInformation
    {
        var values = new List<TestPluginInformation>();

        foreach (var key in dictionary.Keys)
        {
            values.Add(dictionary[key]);
        }

        return values;
    }

    /// <summary>
    /// Gets unfiltered and filtered extensions from the provided test extension collection.
    /// </summary>
    /// <typeparam name="TPluginInfo">
    /// </typeparam>
    /// <typeparam name="TExtension">
    /// Type of the required extensions
    /// </typeparam>
    /// <typeparam name="IMetadata">
    /// Type of metadata of required extensions
    /// </typeparam>
    /// <typeparam name="TMetadata">
    /// Concrete type of metadata
    /// </typeparam>
    /// <param name="testPluginInfo">
    /// The test extension dictionary.
    /// </param>
    /// <param name="unfiltered">
    /// Receives unfiltered list of test extensions
    /// </param>
    /// <param name="filtered">
    /// Receives test extensions filtered by Identifier data
    /// </param>
    private static void GetExtensions<TPluginInfo, TExtension, IMetadata, TMetadata>(
        Dictionary<string, TPluginInfo> testPluginInfo,
        out IEnumerable<LazyExtension<TExtension, Dictionary<string, object>>> unfiltered,
        out IEnumerable<LazyExtension<TExtension, IMetadata>> filtered)
        where TMetadata : IMetadata
        where TPluginInfo : TestPluginInformation
    {
        var unfilteredExtensions = new List<LazyExtension<TExtension, Dictionary<string, object>>>();
        var filteredExtensions = new List<LazyExtension<TExtension, IMetadata>>();

        var testPlugins = GetValuesFromDictionary(testPluginInfo);
        foreach (var plugin in testPlugins)
        {
            if (!plugin.IdentifierData.IsNullOrEmpty())
            {
                var testExtension = new LazyExtension<TExtension, IMetadata>(plugin, typeof(TMetadata));
                filteredExtensions.Add(testExtension);
            }

            unfilteredExtensions.Add(new LazyExtension<TExtension, Dictionary<string, object>>(plugin, new Dictionary<string, object>()));
        }

        unfiltered = unfilteredExtensions;
        filtered = filteredExtensions;
    }

}
