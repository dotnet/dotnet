// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
#if NETFRAMEWORK
using System.IO;
#endif
using System.Linq;
using System.Reflection;

#if NETFRAMEWORK
using Microsoft.VisualStudio.TestPlatform.CoreUtilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
#endif

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

/// <summary>
/// Implementation of finding assembly references using "managed route", i.e. Assembly.Load.
/// </summary>
public static class AssemblyHelper
{
#if NETFRAMEWORK
    private static readonly Version DefaultVersion = new();
    private static readonly Version Version45 = new("4.5");

    /// <summary>
    /// Checks whether the source assembly directly references given assembly or not.
    /// Only assembly name and public key token are match. Version is ignored for matching.
    /// Returns null if not able to check if source references assembly.
    /// </summary>
    public static bool? DoesReferencesAssembly(string source, AssemblyName referenceAssembly)
    {
        if (source.IsNullOrEmpty() || referenceAssembly is null)
        {
            return null;
        }

        try
        {
            var referenceAssemblyName = referenceAssembly.Name;
            var referenceAssemblyPublicKeyToken = referenceAssembly.GetPublicKeyToken();

            var setupInfo = new AppDomainSetup();
            setupInfo.ApplicationBase = Path.GetDirectoryName(Path.GetFullPath(source));

            // In Dev10 by devenv uses its own app domain host which has default optimization to share everything.
            // Set LoaderOptimization to MultiDomainHost which means:
            //   Indicates that the application will probably host unique code in multiple domains,
            //   and the loader must share resources across application domains only for globally available (strong-named)
            //   assemblies that have been added to the global assembly cache.
            setupInfo.LoaderOptimization = LoaderOptimization.MultiDomainHost;

            AppDomain? ad = null;
            try
            {
                ad = AppDomain.CreateDomain("Dependency finder domain", null, setupInfo);

                var assemblyLoadWorker = typeof(AssemblyLoadWorker);
                AssemblyLoadWorker? worker = null;
                if (assemblyLoadWorker.Assembly.GlobalAssemblyCache)
                {
                    worker = (AssemblyLoadWorker)ad.CreateInstanceAndUnwrap(
                        assemblyLoadWorker.Assembly.FullName,
                        assemblyLoadWorker.FullName,
                        false, BindingFlags.Default, null,
                        null, null, null);
                }
                else
                {
                    // This has to be LoadFrom, otherwise we will have to use AssemblyResolver to find self.
                    worker = (AssemblyLoadWorker)ad.CreateInstanceFromAndUnwrap(
                        assemblyLoadWorker.Assembly.Location,
                        assemblyLoadWorker.FullName,
                        false, BindingFlags.Default, null,
                        null, null, null);
                }

                return AssemblyLoadWorker.CheckAssemblyReference(source, referenceAssemblyName, referenceAssemblyPublicKeyToken);
            }
            finally
            {
                if (ad != null)
                {
                    AppDomain.Unload(ad);
                }
            }
        }
        catch
        {
            return null; // Return null if something goes wrong.
        }
    }

    /// <summary>
    /// Finds platform and .Net framework version for a given test container.
    /// If there is an error while inferring this information, defaults (AnyCPU, None) are returned
    /// for faulting container.
    /// </summary>
    /// <param name="testSource"></param>
    /// <returns></returns>
    public static KeyValuePair<Architecture, FrameworkVersion> GetFrameworkVersionAndArchitectureForSource(string testSource)
    {
        ValidateArg.NotNullOrEmpty(testSource, nameof(testSource));

        var sourceDirectory = Path.GetDirectoryName(testSource);
        var setupInfo = new AppDomainSetup();
        setupInfo.ApplicationBase = sourceDirectory;
        setupInfo.LoaderOptimization = LoaderOptimization.MultiDomainHost;
        AppDomain? ad = null;
        try
        {
            ad = AppDomain.CreateDomain("Multiargeting settings domain", null, setupInfo);

            Type assemblyLoadWorker = typeof(AssemblyLoadWorker);
            AssemblyLoadWorker? worker = null;

            // This has to be LoadFrom, otherwise we will have to use AssemblyResolver to find self.
            worker = (AssemblyLoadWorker)ad.CreateInstanceFromAndUnwrap(
                assemblyLoadWorker.Assembly.Location,
                assemblyLoadWorker.FullName,
                false, BindingFlags.Default, null,
                null, null, null);

            AssemblyLoadWorker.GetPlatformAndFrameworkSettings(testSource, out var procArchType, out var frameworkVersion);

            Architecture targetPlatform = (Architecture)Enum.Parse(typeof(Architecture), procArchType);
            var targetFramework = frameworkVersion.ToUpperInvariant() switch
            {
                "V4.5" => FrameworkVersion.Framework45,
                "V4.0" => FrameworkVersion.Framework40,
                "V3.5" or "V2.0" => FrameworkVersion.Framework35,
                _ => FrameworkVersion.None,
            };

            EqtTrace.Verbose("Inferred Multi-Targeting settings:{0} Platform:{1} FrameworkVersion:{2}", testSource, targetPlatform, targetFramework);

            return new KeyValuePair<Architecture, FrameworkVersion>(targetPlatform, targetFramework);

        }
        finally
        {
            if (ad != null)
            {
                AppDomain.Unload(ad);
            }
        }
    }

    /// <summary>
    /// Returns the full name (AssemblyName.FullName) of the referenced assemblies by the assembly on the specified path.
    ///
    /// Returns null on failure and an empty array if there is no reference in the project.
    /// </summary>
    /// <param name="source">Full path to the assembly to get dependencies for.</param>
    public static string[]? GetReferencedAssemblies(string source)
    {
        TPDebug.Assert(!source.IsNullOrEmpty());

        var setupInfo = new AppDomainSetup();
        setupInfo.ApplicationBase = Path.GetDirectoryName(Path.GetFullPath(source));

        // In Dev10 by devenv uses its own app domain host which has default optimization to share everything.
        // Set LoaderOptimization to MultiDomainHost which means:
        //   Indicates that the application will probably host unique code in multiple domains,
        //   and the loader must share resources across application domains only for globally available (strong-named)
        //   assemblies that have been added to the global assembly cache.
        setupInfo.LoaderOptimization = LoaderOptimization.MultiDomainHost;

        AppDomain? ad = null;
        try
        {
            ad = AppDomain.CreateDomain("Dependency finder domain", null, setupInfo);

            var assemblyLoadWorker = typeof(AssemblyLoadWorker);
            AssemblyLoadWorker? worker = null;
            if (assemblyLoadWorker.Assembly.GlobalAssemblyCache)
            {
                worker = (AssemblyLoadWorker)ad.CreateInstanceAndUnwrap(
                    assemblyLoadWorker.Assembly.FullName,
                    assemblyLoadWorker.FullName,
                    false, BindingFlags.Default, null,
                    null, null, null);
            }
            else
            {
                // This has to be LoadFrom, otherwise we will have to use AssemblyResolver to find self.
                worker = (AssemblyLoadWorker)ad.CreateInstanceFromAndUnwrap(
                    assemblyLoadWorker.Assembly.Location,
                    assemblyLoadWorker.FullName,
                    false, BindingFlags.Default, null,
                    null, null, null);
            }

            return AssemblyLoadWorker.GetReferencedAssemblies(source);
        }
        finally
        {
            if (ad != null)
            {
                AppDomain.Unload(ad);
            }
        }
    }

    /// <summary>
    /// Set the target framework for app domain setup if target framework of dll is > 4.5
    /// </summary>
    /// <param name="setup">AppdomainSetup for app domain creation</param>
    /// <param name="testSource">path of test file</param>
    public static void SetAppDomainFrameworkVersionBasedOnTestSource(AppDomainSetup setup, string testSource)
    {
        string assemblyVersionString = GetTargetFrameworkVersionString(testSource);

        if (GetTargetFrameworkVersionFromVersionString(assemblyVersionString).CompareTo(Version45) > 0)
        {
            var pInfo = typeof(AppDomainSetup).GetProperty(Constants.TargetFrameworkName);
            if (null != pInfo)
            {
                pInfo.SetValue(setup, assemblyVersionString, null);
            }
        }
    }

    /// <summary>
    /// Get the target dot net framework string for the assembly
    /// </summary>
    /// <param name="path">Path of the assembly file</param>
    /// <returns>String representation of the target dot net framework e.g. .NETFramework,Version=v4.0 </returns>
    internal static string GetTargetFrameworkVersionString(string path)
    {
        TPDebug.Assert(!path.IsNullOrEmpty());

        var setupInfo = new AppDomainSetup();
        setupInfo.ApplicationBase = Path.GetDirectoryName(Path.GetFullPath(path));

        // In Dev10 by devenv uses its own app domain host which has default optimization to share everything.
        // Set LoaderOptimization to MultiDomainHost which means:
        //   Indicates that the application will probably host unique code in multiple domains,
        //   and the loader must share resources across application domains only for globally available (strong-named)
        //   assemblies that have been added to the global assembly cache.
        setupInfo.LoaderOptimization = LoaderOptimization.MultiDomainHost;

        if (!File.Exists(path))
        {
            return string.Empty;
        }

        AppDomain? ad = null;
        try
        {
            ad = AppDomain.CreateDomain("Framework Version String Domain", null, setupInfo);

            var assemblyLoadWorker = typeof(AssemblyLoadWorker);
            AssemblyLoadWorker? worker = null;
            if (assemblyLoadWorker.Assembly.GlobalAssemblyCache)
            {
                worker = (AssemblyLoadWorker)ad.CreateInstanceAndUnwrap(
                    assemblyLoadWorker.Assembly.FullName,
                    assemblyLoadWorker.FullName,
                    false, BindingFlags.Default, null,
                    null, null, null);
            }
            else
            {
                // This has to be LoadFrom, otherwise we will have to use AssemblyResolver to find self.
                worker = (AssemblyLoadWorker)ad.CreateInstanceFromAndUnwrap(
                    assemblyLoadWorker.Assembly.Location,
                    assemblyLoadWorker.FullName,
                    false, BindingFlags.Default, null,
                    null, null, null);
            }

            return AssemblyLoadWorker.GetTargetFrameworkVersionStringFromPath(path);
        }
        finally
        {
            if (ad != null)
            {
                AppDomain.Unload(ad);
            }
        }
    }

    /// <summary>
    /// Get the Version for the target framework version string
    /// </summary>
    /// <param name="version">Target framework string</param>
    /// <returns>Framework Version</returns>
    internal static Version GetTargetFrameworkVersionFromVersionString(string version)
    {
        if (version.Length > Constants.DotNetFrameWorkStringPrefix.Length + 1)
        {
            string versionPart = version.Substring(Constants.DotNetFrameWorkStringPrefix.Length + 1);
            return new Version(versionPart);
        }

        return DefaultVersion;
    }

    /// <summary>
    /// When test run is targeted for .Net4.0, set target framework of test appdomain to be v4.0.
    /// With this done tests would be executed in 4.0 compatibility mode even when .Net4.5 is installed.
    /// </summary>
    internal static void SetNETFrameworkCompatiblityMode(AppDomainSetup setup, IRunContext runContext)
    {
        try
        {
            RunConfiguration runConfiguration = XmlRunSettingsUtilities.GetRunConfigurationNode(runContext.RunSettings?.SettingsXml);
            if (null != runConfiguration && (Equals(runConfiguration.TargetFramework, FrameworkVersion.Framework40) ||
                string.Equals(runConfiguration.TargetFramework?.ToString(), Constants.DotNetFramework40, StringComparison.OrdinalIgnoreCase)))
            {
                EqtTrace.Verbose("AssemblyHelper.SetNETFrameworkCompatiblityMode: setting .NetFramework,Version=v4.0 compatibility mode.");
                setup.TargetFrameworkName = Constants.DotNetFramework40;
            }
        }
        catch (Exception e)
        {
            EqtTrace.Error("AssemblyHelper:SetNETFrameworkCompatiblityMode: Caught an exception:{0}", e);
        }
    }
#endif

    public static IEnumerable<Attribute> GetCustomAttributes(this Assembly assembly, string fullyQualifiedName)
    {
        ValidateArg.NotNull(assembly, nameof(assembly));
        ValidateArg.NotNullOrWhiteSpace(fullyQualifiedName, nameof(fullyQualifiedName));

        return assembly.GetType(fullyQualifiedName) is Type attribute
            ? assembly.GetCustomAttributes(attribute)
            : assembly
                .GetCustomAttributes()
                .Where(i => i.GetType().FullName == fullyQualifiedName);
    }
}
