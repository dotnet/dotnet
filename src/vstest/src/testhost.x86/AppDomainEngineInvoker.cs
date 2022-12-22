// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace Microsoft.VisualStudio.TestPlatform.TestHost;

/// <summary>
/// Implementation for the Invoker which invokes engine in a new AppDomain
/// Type of the engine must be a marshalable object for app domain calls and also must have a parameterless constructor
/// </summary>
internal class AppDomainEngineInvoker<T> : IEngineInvoker where T : MarshalByRefObject, IEngineInvoker, new()
{
    private const string XmlNamespace = "urn:schemas-microsoft-com:asm.v1";

    protected readonly AppDomain _appDomain;
    protected readonly IEngineInvoker _actualInvoker;

    private string? _mergedTempConfigFile;

    public AppDomainEngineInvoker(string testSourcePath)
    {
        TestPlatformEventSource.Instance.TestHostAppDomainCreationStart();

        _appDomain = CreateNewAppDomain(testSourcePath);
        _actualInvoker = CreateInvokerInAppDomain(_appDomain);

        TestPlatformEventSource.Instance.TestHostAppDomainCreationStop();
    }

    /// <summary>
    /// Invokes the Engine with the arguments
    /// </summary>
    /// <param name="argsDictionary">Arguments for the engine</param>
    public void Invoke(IDictionary<string, string?> argsDictionary)
    {
        try
        {
            _actualInvoker.Invoke(argsDictionary);
        }
        finally
        {
            try
            {
                //if(appDomain != null)
                //{
                // Do not unload appdomain as there are lot is issues reported against appdomain unload
                // any ways the process is going to die off.
                // AppDomain.Unload(appDomain);
                //}

                if (!string.IsNullOrWhiteSpace(_mergedTempConfigFile) && File.Exists(_mergedTempConfigFile))
                {
                    File.Delete(_mergedTempConfigFile);
                }
            }
            catch
            {
                // ignore
            }
        }
    }

    private AppDomain CreateNewAppDomain(string testSourcePath)
    {
        var appDomainSetup = new AppDomainSetup();
        var testSourceFolder = Path.GetDirectoryName(testSourcePath);

        // Set AppBase to TestAssembly location
        appDomainSetup.ApplicationBase = testSourceFolder;
        appDomainSetup.LoaderOptimization = LoaderOptimization.MultiDomainHost;

        // Set User Config file as app domain config
        SetConfigurationFile(appDomainSetup, testSourcePath, testSourceFolder);

        // Create new AppDomain
        var appDomain = AppDomain.CreateDomain("TestHostAppDomain", null, appDomainSetup);

        return appDomain;
    }

    /// <summary>
    /// Create the Engine Invoker in new AppDomain based on test source path
    /// </summary>
    /// <param name="testSourcePath">Test Source to run/discover tests for</param>
    /// <param name="mergedConfigFile">Merged config file if there is any merging of test config and test host config</param>
    /// <returns></returns>
    private static IEngineInvoker CreateInvokerInAppDomain(AppDomain appDomain)
    {
        // Create CustomAssembly setup that sets a custom assembly resolver to be able to resolve TestPlatform assemblies
        // and also sets the correct UI culture to propagate the dotnet or VS culture to the adapters running in the app domain
        appDomain.CreateInstanceFromAndUnwrap(
            typeof(CustomAssemblySetup).Assembly.Location,
            typeof(CustomAssemblySetup).FullName,
            false,
            BindingFlags.Default,
            null,
            new object?[] { CultureInfo.DefaultThreadCurrentUICulture?.Name, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) },
            null,
            null);

        // Create Invoker object in new appdomain
        var invokerType = typeof(T);
        return (IEngineInvoker)appDomain.CreateInstanceFromAndUnwrap(
            invokerType.Assembly.Location,
            invokerType.FullName,
            false,
            BindingFlags.Default,
            null,
            null,
            null,
            null);
    }

    private void SetConfigurationFile(AppDomainSetup appDomainSetup, string testSource, string testSourceFolder)
    {
        var userConfigFile = GetConfigFile(testSource, testSourceFolder);
        var testHostAppConfigFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        if (!string.IsNullOrEmpty(userConfigFile))
        {
            var userConfigDoc = XDocument.Load(userConfigFile);
            var testHostConfigDoc = XDocument.Load(testHostAppConfigFile);

            // Merge user's config file and testHost config file and use merged one
            var mergedConfigDocument = MergeApplicationConfigFiles(userConfigDoc, testHostConfigDoc);

            // Create a temp file with config
            _mergedTempConfigFile = Path.GetTempFileName();
            mergedConfigDocument.Save(_mergedTempConfigFile);

            // Set config file to merged one
            appDomainSetup.ConfigurationFile = _mergedTempConfigFile;
        }
        else
        {
            // Use the current domains configuration setting.
            appDomainSetup.ConfigurationFile = testHostAppConfigFile;
        }
    }

    private static string? GetConfigFile(string testSource, string testSourceFolder)
    {
        string? configFile = null;

        if (File.Exists(testSource + ".config"))
        {
            // Path to config file cannot be bad: storage is already checked, and extension is valid.
            configFile = testSource + ".config";
        }
        else
        {
            var netAppConfigFile = Path.Combine(testSourceFolder, "App.Config");
            if (File.Exists(netAppConfigFile))
            {
                configFile = netAppConfigFile;
            }
        }

        return configFile;
    }

    protected static XDocument MergeApplicationConfigFiles(XDocument userConfigDoc, XDocument testHostConfigDoc)
    {
        // Start with User's config file as the base
        var mergedDoc = new XDocument(userConfigDoc);

        // Take testhost.exe Startup node
        var startupNode = testHostConfigDoc.Descendants("startup")?.FirstOrDefault();
        if (startupNode != null)
        {
            // Remove user's startup and add ours which supports NET35
            mergedDoc.Descendants("startup")?.Remove();
            mergedDoc.Root.Add(startupNode);
        }

        // Runtime node must be merged which contains assembly redirections
        var runtimeTestHostNode = testHostConfigDoc.Descendants("runtime")?.FirstOrDefault();
        if (runtimeTestHostNode != null)
        {
            var runTimeNode = mergedDoc.Descendants("runtime")?.FirstOrDefault();
            if (runTimeNode == null)
            {
                // remove test host relative probing paths' element
                // TestHost Probing Paths do not make sense since we are setting "AppBase" to user's test assembly location
                runtimeTestHostNode.Descendants().Where((element) => string.Equals(element.Name.LocalName, "probing")).Remove();

                // no runtime node exists in user's config - just add ours entirely
                mergedDoc.Root.Add(runtimeTestHostNode);
            }
            else
            {
                var assemblyBindingXName = XName.Get("assemblyBinding", XmlNamespace);
                var mergedDocAssemblyBindingNode = mergedDoc.Descendants(assemblyBindingXName)?.FirstOrDefault();
                var testHostAssemblyBindingNode = runtimeTestHostNode.Descendants(assemblyBindingXName)?.FirstOrDefault();

                if (testHostAssemblyBindingNode != null)
                {
                    if (mergedDocAssemblyBindingNode == null)
                    {
                        // add another assemblyBinding element as none exists in user's config
                        runTimeNode.Add(testHostAssemblyBindingNode);
                    }
                    else
                    {
                        var dependentAssemblyXName = XName.Get("dependentAssembly", XmlNamespace);
                        var redirections = testHostAssemblyBindingNode.Descendants(dependentAssemblyXName);

                        if (redirections != null)
                        {
                            mergedDocAssemblyBindingNode.Add(redirections);
                        }
                    }
                }
            }
        }

        return mergedDoc;
    }
}

/// <summary>
/// Custom domain setup that sets UICulture and an Assembly resolver for child app domain to resolve testplatform assemblies
/// </summary>
// The normal AppDomainInitializer api was not used to do this because it cannot load the assemblies for testhost. --JJR
internal class CustomAssemblySetup : MarshalByRefObject
{
    private readonly IDictionary<string, Assembly?> _resolvedAssemblies;

    private readonly string[] _resolverPaths;

    public CustomAssemblySetup(string uiCulture, string testPlatformPath)
    {
        if (uiCulture != null)
        {
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CreateSpecificCulture(uiCulture);
        }

        _resolverPaths = new string[] { testPlatformPath, Path.Combine(testPlatformPath, "Extensions") };
        _resolvedAssemblies = new Dictionary<string, Assembly?>();
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
    }

    private Assembly? CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        var assemblyName = new AssemblyName(args.Name);

        Assembly? assembly = null;
        lock (_resolvedAssemblies)
        {
            try
            {
                EqtTrace.Verbose("CurrentDomain_AssemblyResolve: Resolving assembly '{0}'.", args.Name);

                if (_resolvedAssemblies.TryGetValue(args.Name, out assembly))
                {
                    return assembly;
                }

                // Put it in the resolved assembly so that if below Assembly.Load call
                // triggers another assembly resolution, then we don't end up in stack overflow
                _resolvedAssemblies[args.Name] = null;

                foreach (var path in _resolverPaths)
                {
                    var testPlatformFilePath = Path.Combine(path, assemblyName.Name) + ".dll";
                    if (File.Exists(testPlatformFilePath))
                    {
                        try
                        {
                            assembly = Assembly.LoadFrom(testPlatformFilePath);
                            break;
                        }
                        catch (Exception)
                        {
                            // ignore
                        }
                    }
                }

                // Replace the value with the loaded assembly
                _resolvedAssemblies[args.Name] = assembly;

                return assembly;
            }
            finally
            {
                if (null == assembly)
                {
                    EqtTrace.Verbose("CurrentDomainAssemblyResolve: Failed to resolve assembly '{0}'.", args.Name);
                }
            }
        }
    }
}
#endif
