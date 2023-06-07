// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


#if NETFRAMEWORK
using System.Threading;
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework.Utilities;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;

/// <summary>
/// The test plugin cache.
/// </summary>
/// <remarks>Making this a singleton to offer better unit testing.</remarks>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "Would cause a breaking change if users are inheriting this class and implement IDisposable")]
public class TestPluginCache
{
    private readonly Dictionary<string, Assembly?> _resolvedAssemblies = new();

    private List<string> _filterableExtensionPaths;
    private List<string> _unfilterableExtensionPaths;

    /// <summary>
    /// Assembly resolver used to resolve the additional extensions
    /// </summary>
    private AssemblyResolver? _assemblyResolver;

    /// <summary>
    /// Lock for extensions update
    /// </summary>
    private readonly object _lockForExtensionsUpdate;

    private static TestPluginCache? s_instance;

    private readonly List<string> _defaultExtensionPaths = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TestPluginCache"/> class.
    /// </summary>
    protected TestPluginCache()
    {
        _filterableExtensionPaths = new List<string>();
        _unfilterableExtensionPaths = new List<string>();
        _lockForExtensionsUpdate = new object();
        TestExtensions = null;
    }

    [AllowNull]
    public static TestPluginCache Instance
    {
        get
        {
            return s_instance ??= new TestPluginCache();
        }

        internal set
        {
            s_instance = value;
        }
    }

    /// <summary>
    /// Gets the test extensions discovered by the cache until now.
    /// </summary>
    /// <remarks>Returns null if discovery of extensions is not done.</remarks>
    internal TestExtensions? TestExtensions { get; private set; }

    /// <summary>
    /// Gets a list of all extension paths filtered by input string.
    /// </summary>
    /// <param name="endsWithPattern">Pattern to filter extension paths.</param>
    public List<string> GetExtensionPaths(string endsWithPattern, bool skipDefaultExtensions = false)
    {
        var extensions = GetFilteredExtensions(_filterableExtensionPaths, endsWithPattern);

        EqtTrace.Verbose(
            "TestPluginCache.GetExtensionPaths: Filtered extension paths: {0}", string.Join(Environment.NewLine, extensions));

        if (!skipDefaultExtensions)
        {
            extensions = extensions.Concat(_defaultExtensionPaths);
            EqtTrace.Verbose(
                "TestPluginCache.GetExtensionPaths: Added default extension paths: {0}", string.Join(Environment.NewLine, _defaultExtensionPaths));
        }

        EqtTrace.Verbose(
            "TestPluginCache.GetExtensionPaths: Added unfilterableExtensionPaths: {0}", string.Join(Environment.NewLine, _unfilterableExtensionPaths));

        return extensions.Concat(_unfilterableExtensionPaths).ToList();
    }

    /// <summary>
    /// Performs discovery of specific type of test extensions in files ending with the specified pattern.
    /// </summary>
    /// <typeparam name="TPluginInfo">
    /// Type of Plugin info.
    /// </typeparam>
    /// <typeparam name="TExtension">
    /// Type of extension.
    /// </typeparam>
    /// <param name="endsWithPattern">
    /// Pattern used to select files using String.EndsWith
    /// </param>
    /// <returns>
    /// The <see cref="Dictionary"/>. of test plugin info.
    /// </returns>
    public Dictionary<string, TPluginInfo>? DiscoverTestExtensions<TPluginInfo, TExtension>(
        string endsWithPattern)
        where TPluginInfo : TestPluginInformation
    {
        EqtTrace.Verbose("TestPluginCache.DiscoverTestExtensions: finding test extensions in assemblies ends with: {0} TPluginInfo: {1} TExtension: {2}", endsWithPattern, typeof(TPluginInfo), typeof(TExtension));
        // Return the cached value if cache is valid.
        if (TestExtensions != null && TestExtensions.AreTestExtensionsCached<TPluginInfo>())
        {
            return TestExtensions.GetTestExtensionCache<TPluginInfo>();
        }

        Dictionary<string, TPluginInfo>? pluginInfos;
        SetupAssemblyResolver(null);

        // Some times TestPlatform.core.dll assembly fails to load in the current appdomain (from devenv.exe).
        // Reason for failures are not known. Below handler, again calls assembly.load() in failing assembly
        // and that succeeds.
        // Because of this assembly failure, below domain.CreateInstanceAndUnwrap() call fails with error
        // "Unable to cast transparent proxy to type 'Microsoft.VisualStudio.TestPlatform.Core.TestPluginsFramework.TestPluginDiscoverer"
        var platformAssemblyResolver = new PlatformAssemblyResolver();
        platformAssemblyResolver.AssemblyResolve += CurrentDomainAssemblyResolve;

        try
        {
            EqtTrace.Verbose("TestPluginCache.DiscoverTestExtensions: Discovering the extensions using extension path.");

            // Combine all the possible extensions - both default and additional.
            var allExtensionPaths = GetExtensionPaths(endsWithPattern);

            EqtTrace.Verbose(
                "TestPluginCache.DiscoverTestExtensions: Discovering the extensions using allExtensionPaths: {0}", string.Join(Environment.NewLine, allExtensionPaths));

            // Discover the test extensions from candidate assemblies.
            pluginInfos = GetTestExtensions<TPluginInfo, TExtension>(allExtensionPaths);

            TestExtensions ??= new TestExtensions();

            TestExtensions.AddExtension(pluginInfos);

            // Set the cache bool to true.
            TestExtensions.SetTestExtensionsCacheStatusToTrue<TPluginInfo>();

            if (EqtTrace.IsVerboseEnabled)
            {
                var extensionString = _filterableExtensionPaths != null
                    ? string.Join(",", _filterableExtensionPaths.ToArray())
                    : null;
                EqtTrace.Verbose(
                    "TestPluginCache: Discovered the extensions using extension path '{0}'.",
                    extensionString);
            }

            LogExtensions();
        }
#if NETFRAMEWORK
        catch (ThreadAbortException)
        {
            // Nothing to do here, we just do not want to do an EqtTrace.Fail for this thread
            // being aborted as it is a legitimate exception to receive.
            EqtTrace.Verbose("TestPluginCache.DiscoverTestExtensions: Data extension discovery is being aborted due to a thread abort.");
            return null;
        }
#endif
        catch (Exception e)
        {
            EqtTrace.Error("TestPluginCache: Discovery failed! {0}", e);
            throw;
        }
        finally
        {
            if (platformAssemblyResolver != null)
            {
                platformAssemblyResolver.AssemblyResolve -= CurrentDomainAssemblyResolve;
                platformAssemblyResolver.Dispose();
            }

            // clear the assemblies
            lock (_resolvedAssemblies)
            {
                _resolvedAssemblies?.Clear();
            }
        }

        return pluginInfos;
    }

    /// <summary>
    /// Use the parameter path to extensions
    /// </summary>
    /// <param name="additionalExtensionsPath">List of extension paths</param>
    /// <param name="skipExtensionFilters">Skip extension name filtering (if true)</param>
    public void UpdateExtensions(IEnumerable<string>? additionalExtensionsPath, bool skipExtensionFilters)
    {
        lock (_lockForExtensionsUpdate)
        {
            EqtTrace.Verbose("TestPluginCache: Update extensions started. Skip filter = " + skipExtensionFilters);

            var extensions = additionalExtensionsPath?.ToList();
            if (extensions == null || extensions.Count == 0)
            {
                return;
            }

            if (skipExtensionFilters)
            {
                // Add the extensions to un-filter list. These extensions will never be filtered
                // based on file name (e.g. *.testadapter.dll etc.).
                if (TryMergeExtensionPaths(_unfilterableExtensionPaths, extensions,
                        out _unfilterableExtensionPaths))
                {
                    // Set the extensions discovered to false so that the next time anyone tries
                    // to get the additional extensions, we rediscover.
                    TestExtensions?.InvalidateCache();
                }
            }
            else
            {
                if (TryMergeExtensionPaths(_filterableExtensionPaths, extensions,
                        out _filterableExtensionPaths))
                {
                    TestExtensions?.InvalidateCache();
                }
            }

            if (EqtTrace.IsVerboseEnabled)
            {
                var directories = _filterableExtensionPaths.Concat(_unfilterableExtensionPaths).Select(e => Path.GetDirectoryName(Path.GetFullPath(e))).Distinct();
                var directoryString = string.Join(",", directories);
                EqtTrace.Verbose(
                    "TestPluginCache: Using directories for assembly resolution '{0}'.",
                    directoryString);

                var extensionString = string.Join(",", _filterableExtensionPaths.Concat(_unfilterableExtensionPaths));
                EqtTrace.Verbose("TestPluginCache: Updated the available extensions to '{0}'.", extensionString);
            }
        }
    }

    /// <summary>
    /// Clear the previously cached extensions
    /// </summary>
    public void ClearExtensions()
    {
        _filterableExtensionPaths?.Clear();
        _unfilterableExtensionPaths?.Clear();
        TestExtensions?.InvalidateCache();
    }

    /// <summary>
    /// Add search directories to assembly resolver
    /// </summary>
    /// <param name="directories"></param>
    public void AddResolverSearchDirectories(string[] directories)
    {
        _assemblyResolver?.AddSearchDirectories(directories);
    }

    internal IEnumerable<string> DefaultExtensionPaths
    {
        get
        {
            return _defaultExtensionPaths;
        }

        set
        {
            if (value != null)
            {
                _defaultExtensionPaths.AddRange(value);
            }
        }
    }

    /// <summary>
    /// The get test extensions.
    /// </summary>
    /// <param name="extensionAssembly">
    /// The extension assembly.
    /// </param>
    /// <param name="skipCache">
    /// Skip the extensions cache.
    /// </param>
    /// <typeparam name="TPluginInfo">
    /// Type of Test plugin info.
    /// </typeparam>
    /// <typeparam name="TExtension">
    /// Type of extension.
    /// </typeparam>
    /// <returns>
    /// The <see cref="Dictionary"/>.
    /// </returns>
    internal Dictionary<string, TPluginInfo> GetTestExtensions<TPluginInfo, TExtension>(
        string extensionAssembly,
        bool skipCache = false)
        where TPluginInfo : TestPluginInformation
    {
        if (skipCache)
        {
            return GetTestExtensions<TPluginInfo, TExtension>(new List<string>() { extensionAssembly });
        }

        // Check if extensions from this assembly have already been discovered.
        var extensions = TestExtensions.GetExtensionsDiscoveredFromAssembly(
            TestExtensions?.GetTestExtensionCache<TPluginInfo>(),
            extensionAssembly);

        if (extensions?.Count > 0)
        {
            return extensions;
        }

        var pluginInfos = GetTestExtensions<TPluginInfo, TExtension>(new List<string>() { extensionAssembly });

        // Add extensions discovered to the cache.
        TestExtensions ??= new TestExtensions();

        TestExtensions.AddExtension(pluginInfos);

        return pluginInfos;
    }

    /// <summary>
    /// Gets the resolution paths for the extension assembly to facilitate assembly resolution.
    /// </summary>
    /// <param name="extensionAssembly">The extension assembly.</param>
    /// <returns>Resolution paths for the assembly.</returns>
    internal static IList<string> GetResolutionPaths(string extensionAssembly)
    {
        var resolutionPaths = new List<string>();

        var extensionDirectory = Path.GetDirectoryName(Path.GetFullPath(extensionAssembly))!;
        resolutionPaths.Add(extensionDirectory);

        var currentDirectory = Path.GetDirectoryName(typeof(TestPluginCache).Assembly.GetAssemblyLocation())!;
        if (!resolutionPaths.Contains(currentDirectory))
        {
            resolutionPaths.Add(currentDirectory);
        }

        return resolutionPaths;
    }

    /// <summary>
    /// Gets the default set of resolution paths for the assembly resolution
    /// </summary>
    /// <returns>List of paths.</returns>
    internal IList<string> GetDefaultResolutionPaths()
    {
        var resolutionPaths = new List<string>();

        // Add the extension directories for assembly resolution
        var extensionDirectories = GetExtensionPaths(string.Empty).Select(e => Path.GetDirectoryName(Path.GetFullPath(e))!).Distinct().ToList();
        if (extensionDirectories.Count != 0)
        {
            resolutionPaths.AddRange(extensionDirectories);
        }

        // Keep current directory for resolution
        var currentDirectory = Path.GetDirectoryName(typeof(TestPluginCache).Assembly.GetAssemblyLocation())!;
        if (!resolutionPaths.Contains(currentDirectory))
        {
            resolutionPaths.Add(currentDirectory);
        }

        // If running in Visual Studio context, add well known directories for resolution
        var installContext = new InstallationContext(new FileHelper());
        if (installContext.TryGetVisualStudioDirectory(out string vsInstallPath))
        {
            resolutionPaths.AddRange(installContext.GetVisualStudioCommonLocations(vsInstallPath));
        }

        return resolutionPaths;
    }

    /// <summary>
    /// Get the files which match the regex pattern
    /// </summary>
    /// <param name="extensions">
    /// The extensions.
    /// </param>
    /// <param name="endsWithPattern">
    /// Pattern used to select files using String.EndsWith
    /// </param>
    /// <returns>
    /// The list of files which match the regex pattern
    /// </returns>
    protected virtual IEnumerable<string> GetFilteredExtensions(List<string> extensions, string endsWithPattern)
    {
        return endsWithPattern.IsNullOrEmpty()
            ? extensions
            : extensions.Where(ext => ext.EndsWith(endsWithPattern, StringComparison.OrdinalIgnoreCase));
    }

    private static bool TryMergeExtensionPaths(List<string> extensionsList, List<string> additionalExtensions, out List<string> mergedExtensionsList)
    {
        if (additionalExtensions.Count == extensionsList.Count && additionalExtensions.All(extensionsList.Contains))
        {
            EqtTrace.Verbose(
                "TestPluginCache: Ignoring extensions merge as there is no change. Current additionalExtensions are '{0}'.",
                string.Join(",", extensionsList));

            mergedExtensionsList = extensionsList;
            return false;
        }

        // Don't do a strict check for existence of the extension path. The extension paths may or may
        // not exist on the disk. In case of .net core, the paths are relative to the nuget packages
        // directory. The path to nuget directory is automatically setup for CLR to resolve.
        // Test platform tries to load every extension by assembly name. If it is not resolved, we don't throw
        // an error.
        additionalExtensions.AddRange(extensionsList);
        mergedExtensionsList = additionalExtensions.Select(Path.GetFullPath)
            .Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        return true;
    }

    /// <summary>
    /// Gets the test extensions defined in the extension assembly list.
    /// </summary>
    /// <typeparam name="TPluginInfo">
    /// Type of PluginInfo.
    /// </typeparam>
    /// <typeparam name="TExtension">
    /// Type of Extension.
    /// </typeparam>
    /// <param name="extensionPaths">
    /// Extension assembly paths.
    /// </param>
    /// <returns>
    /// List of extensions.
    /// </returns>
    /// <remarks>
    /// Added to mock out dependency from the actual test plugin discovery as such.
    /// </remarks>
    private Dictionary<string, TPluginInfo> GetTestExtensions<TPluginInfo, TExtension>(IEnumerable<string> extensionPaths) where TPluginInfo : TestPluginInformation
    {
        foreach (var extensionPath in extensionPaths)
        {
            SetupAssemblyResolver(extensionPath);
        }

        return TestPluginDiscoverer.GetTestExtensionsInformation<TPluginInfo, TExtension>(extensionPaths);
    }

    protected void SetupAssemblyResolver(string? extensionAssembly)
    {
        // If we don't load the resource for Microsoft.TestPlatform.Common before
        // to set the assembly resolver we won't be able to use the resources.
        // This should be the algorithm followed during the satellite assembly resolution
        // https://learn.microsoft.com/dotnet/core/extensions/package-and-deploy-resources#net-framework-resource-fallback-process
        // BUT for some unknown reason the point 10 is not working as explained.
        // Satellite resolution should fallback to the NeutralResourcesLanguageAttribute
        // that we set to en-US but don't and we fail with FileNotFoundException.
        _ = Resources.Resources.FailedToLoadAdapaterFile;

        IList<string> resolutionPaths = extensionAssembly.IsNullOrEmpty()
            ? GetDefaultResolutionPaths()
            : GetResolutionPaths(extensionAssembly);

        // Add assembly resolver which can resolve the extensions from the specified directory.
        if (_assemblyResolver == null)
        {
            _assemblyResolver = new AssemblyResolver(resolutionPaths);
        }
        else
        {
            _assemblyResolver.AddSearchDirectories(resolutionPaths);
        }
    }

    private Assembly? CurrentDomainAssemblyResolve(object? sender, AssemblyResolveEventArgs? args)
    {
        // TODO: Avoid ArgumentNullException
        var assemblyName = new AssemblyName(args?.Name!);
        TPDebug.Assert(args?.Name is not null);

        Assembly? assembly = null;
        lock (_resolvedAssemblies)
        {
            try
            {
                EqtTrace.Verbose("CurrentDomainAssemblyResolve: Resolving assembly '{0}'.", args.Name);

                if (_resolvedAssemblies.TryGetValue(args.Name, out assembly))
                {
                    return assembly;
                }

                // Put it in the resolved assembly so that if below Assembly.Load call
                // triggers another assembly resolution, then we don't end up in stack overflow
                _resolvedAssemblies[args.Name] = null;

                assembly = Assembly.Load(assemblyName);

                // Replace the value with the loaded assembly
                _resolvedAssemblies[args.Name] = assembly;

                return assembly;
            }
            finally
            {
                if (assembly == null)
                {
                    EqtTrace.Verbose("CurrentDomainAssemblyResolve: Failed to resolve assembly '{0}'.", args.Name);
                }
            }
        }
    }

    /// <summary>
    /// Log the extensions
    /// </summary>
    private void LogExtensions()
    {
        if (!EqtTrace.IsVerboseEnabled)
        {
            return;
        }

        TPDebug.Assert(TestExtensions is not null, "TestExtensions is null");

        var discoverers = TestExtensions.TestDiscoverers != null ? string.Join(",", TestExtensions.TestDiscoverers.Keys.ToArray()) : null;
        EqtTrace.Verbose("TestPluginCache: Discoverers are '{0}'.", discoverers);

        var executors = TestExtensions.TestExecutors != null ? string.Join(",", TestExtensions.TestExecutors.Keys.ToArray()) : null;
        EqtTrace.Verbose("TestPluginCache: Executors are '{0}'.", executors);

        var executors2 = TestExtensions.TestExecutors2 != null ? string.Join(",", TestExtensions.TestExecutors2.Keys.ToArray()) : null;
        EqtTrace.Verbose("TestPluginCache: Executors2 are '{0}'.", executors2);

        var settingsProviders = TestExtensions.TestSettingsProviders != null ? string.Join(",", TestExtensions.TestSettingsProviders.Keys.ToArray()) : null;
        EqtTrace.Verbose("TestPluginCache: Setting providers are '{0}'.", settingsProviders);

        var loggers = TestExtensions.TestLoggers != null ? string.Join(",", TestExtensions.TestLoggers.Keys.ToArray()) : null;
        EqtTrace.Verbose("TestPluginCache: Loggers are '{0}'.", loggers);

        var testhosts = TestExtensions.TestHosts != null ? string.Join(",", TestExtensions.TestHosts.Keys.ToArray()) : null;
        EqtTrace.Verbose("TestPluginCache: TestHosts are '{0}'.", testhosts);

        var dataCollectors = TestExtensions.DataCollectors != null ? string.Join(",", TestExtensions.DataCollectors.Keys.ToArray()) : null;
        EqtTrace.Verbose("TestPluginCache: DataCollectors are '{0}'.", dataCollectors);
    }

}
