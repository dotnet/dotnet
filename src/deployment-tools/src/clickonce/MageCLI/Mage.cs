// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Microsoft.Build.Tasks.Deployment.ManifestUtilities;
using Microsoft.Deployment.Utilities;

#if !RUNTIME_TYPE_NETCORE
using System.Diagnostics;
#endif

namespace Microsoft.Deployment.MageCLI
{
    /// <summary>
    /// Processor types
    /// </summary>
    internal enum Processors
    {
        // Note that the LAST constant MUST be "Undefined"
        msil, x86, amd64, Undefined
    }

    /// <summary>
    /// True, False, or Undefined
    /// </summary>
    internal enum TriStateBool
    {
        True, False, Undefined
    }

    /// <summary>
    /// This class wraps a few static methods.
    /// </summary>
    internal class Mage
    {
        private const string defaultCulture = "neutral";
        private static readonly Version defaultVersion = new Version(1, 0, 0, 0);

        /// <summary>
        /// Generates Application manifest
        /// </summary>
        /// <param name="filesToIgnore">List of files to ignore</param>
        /// <param name="nameArgument">Product name</param>
        /// <param name="appName">Application name</param>
        /// <param name="version">Version</param>
        /// <param name="processor">Processor type</param>
        /// <param name="trustLevel">Trust level</param>
        /// <param name="fromDirectory">Directory from which to harvest files.</param>
        /// <param name="iconFile">Icon file</param>
        /// <param name="useApplicationManifestForTrustInfo">Specifies if Application manifest is used for trust info</param>
        /// <param name="publisherName">Publisher name</param>
        /// <param name="supportUrl">Support URL</param>
        /// <param name="targetFrameworkVersion">Target Framework version</param>
        /// <returns>ApplicationManifest object</returns>
        public static ApplicationManifest GenerateApplicationManifest(List<string> filesToIgnore, string nameArgument, string appName, Version version,
            Processors processor, Command.TrustLevels trustLevel, string fromDirectory, 
            string iconFile, TriStateBool useApplicationManifestForTrustInfo, string publisherName, string supportUrl, string targetFrameworkVersion)
        {
            ApplicationManifest manifest = new ApplicationManifest();

            // Default to full trust for manifest generation
            if (trustLevel == Command.TrustLevels.None)
            {
                trustLevel = Command.TrustLevels.FullTrust;
            }

            // Use a default name for generated manifests
            if (appName == null)
            {
                appName = Application.Resources.GetString("DefaultAppName");
            }

            // Use a default version for generated manifests
            if (version == null)
            {
                version = defaultVersion;
            }

            // Use a default processor for generated manifests
            if (processor == Processors.Undefined)
            {
                processor = Processors.msil;
            }

            UpdateApplicationManifest(filesToIgnore, manifest, nameArgument, appName, version,
                processor, trustLevel, fromDirectory, iconFile, useApplicationManifestForTrustInfo, 
                publisherName, supportUrl, targetFrameworkVersion);

            return manifest;
        }

        /// <summary>
        /// Generates Deployment manifest
        /// </summary>
        /// <param name="deploymentManifestPath">Path of generated deployment manifest</param>
        /// <param name="appName">Application name</param>
        /// <param name="version">Version</param>
        /// <param name="processor">Processor type</param>
        /// <param name="applicationManifest">Application manifest from which to extract deployment details</param>
        /// <param name="applicationManifestPath">Path to application manifest</param>
        /// <param name="appCodeBase">Application codebase</param>
        /// <param name="appProviderUrl">Application provider URL</param>
        /// <param name="minVersion">Minimum version</param>
        /// <param name="install">Install state</param>
        /// <param name="includeDeploymentProviderUrl"></param>
        /// <param name="publisherName">Publisher name</param>
        /// <param name="supportUrl">Support URL</param>
        /// <param name="targetFrameworkVersion">Target Framework version</param>
        /// <param name="trustUrlParameters">Specifies if URL parameters should be trusted</param>
        /// <returns>DeploymentManifest object</returns>
        public static DeployManifest GenerateDeploymentManifest(string deploymentManifestPath,
            string appName, Version version, Processors processor,
            ApplicationManifest applicationManifest, string applicationManifestPath, string appCodeBase,
            string appProviderUrl, string minVersion, TriStateBool install, TriStateBool includeDeploymentProviderUrl,
            string publisherName, string supportUrl, string targetFrameworkVersion, TriStateBool trustUrlParameters)
        {
            /*
              Mage running on Core cannot obtain .NET FX version for targeting.
              Set default minimum version to v4.5, that Launcher targets,
              which is a good default for .NET FX Mage as well.

              As always, version can be modified in deployment manifest, if needed.
            */
            Version shortVersion = new Version(4, 5);
            string frameworkIdentifier = ".NETFramework";

            DeployManifest manifest = new DeployManifest((new FrameworkName(frameworkIdentifier, shortVersion)).FullName);

            // Set default manifest publish options
            if (install == TriStateBool.True)
            {
                manifest.Install = true;
                manifest.UpdateEnabled = true;
                manifest.UpdateMode = UpdateMode.Foreground;
            }
            else
            {
                manifest.Install = false;
            }

            // Use default application name if none was specified
            if (appName == null)
            {
                appName = Application.Resources.GetString("DefaultAppName");
            }

            // Use default version if none was specified
            if (version == null)
            {
                version = defaultVersion;
            }

            // Use default processor if none was specified
            if (processor == Processors.Undefined)
            {
                processor = Processors.msil;
            }

            // Use default provider URL if none was specified
            if (appProviderUrl == null)
            {
                appProviderUrl = "";
            }

            UpdateDeploymentManifest(manifest, deploymentManifestPath, appName, version, processor,
                applicationManifest, applicationManifestPath, appCodeBase,
                appProviderUrl, minVersion, install, includeDeploymentProviderUrl, publisherName, supportUrl, targetFrameworkVersion, trustUrlParameters);

            return manifest;
        }

        /// <summary>
        /// Updates Application manifest
        /// </summary>
        /// <param name="filesToIgnore">List of files to ignore</param>
        /// <param name="manifest">ApplicationManifest object</param>
        /// <param name="nameArgument">Product name</param>
        /// <param name="appName">Application name</param>
        /// <param name="version">Version</param>
        /// <param name="processor">Processor type</param>
        /// <param name="trustLevel">Trust level</param>
        /// <param name="fromDirectory">Directory from where to harvest the files.</param>
        /// <param name="iconFile">Icon file</param>
        /// <param name="useApplicationManifestForTrustInfo">Specifies if ApplicationManifest should be used for trust info</param>
        /// <param name="publisherName">Publisher name</param>
        /// <param name="supportUrl">Support URL</param>
        /// <param name="targetFrameworkVersion">Target Framework version</param>
        public static void UpdateApplicationManifest(List<string> filesToIgnore, ApplicationManifest manifest, string nameArgument, string appName, Version version, Processors processor, Command.TrustLevels trustLevel, string fromDirectory,
            string iconFile, TriStateBool useApplicationManifestForTrustInfo, string publisherName, string supportUrl, string targetFrameworkVersion)
        {
            if (appName != null)
            {
                manifest.AssemblyIdentity.Name = appName;
            }
            else if (!string.IsNullOrEmpty(manifest.AssemblyIdentity.Name))
            {
                appName = manifest.AssemblyIdentity.Name;
            }

            if (version != null)
            {
                manifest.AssemblyIdentity.Version = version.ToString();
            }

            if (processor != Processors.Undefined)
            {
                manifest.AssemblyIdentity.ProcessorArchitecture = processor.ToString();
            }

            // Culture is not supported by command-line arguments, but it gets defaulted if not already present in the manifest
            if ((manifest.AssemblyIdentity.Culture == null) || (manifest.AssemblyIdentity.Culture.Length == 0))
            {
                manifest.AssemblyIdentity.Culture = defaultCulture;
            }

#if RUNTIME_TYPE_NETCORE
            // TrustInfo is always Full-trust on .NET (Core)
            if (manifest.TrustInfo == null)
            {
                // TrustInfo object is initialized as Full-trust for all apps running on .NET (Core)
                manifest.TrustInfo = new Microsoft.Build.Tasks.Deployment.ManifestUtilities.TrustInfo();
            }
#else
            if (trustLevel != Command.TrustLevels.None)
            {
                SetTrustLevel(manifest, trustLevel);
            }
#endif

            if (iconFile != null)
            {
                manifest.IconFile = iconFile;
            }

            if (useApplicationManifestForTrustInfo == TriStateBool.True)
            {
                manifest.UseApplicationTrust = true;
                if (!string.IsNullOrEmpty(nameArgument))
                {
                    manifest.Product = nameArgument;
                }
                else if (appName != null)
                {
                    if (appName.ToLower().EndsWith(".exe"))
                    {
                        // remove the trailing .exe extension
                        manifest.Product = appName.Substring(0, appName.Length - ".exe".Length);
                    }
                    else
                    {
                        manifest.Product = appName;
                    }
                }

                if (!string.IsNullOrEmpty(publisherName))
                {
                    manifest.Publisher = publisherName;
                }
                else
                {
#if RUNTIME_TYPE_NETCORE
                    // GetRegisteredOrganization() is a Windows-only API
                    manifest.Publisher = string.Empty;
                    if (OperatingSystem.IsWindows())
#endif
                    // Get the default publisher name
                    manifest.Publisher = Utilities.Misc.GetRegisteredOrganization();
                }

                if (!string.IsNullOrEmpty(supportUrl))
                {
                    manifest.SupportUrl = supportUrl;
                }
            }

            if (fromDirectory != null)
            {
                Utilities.AppMan.AddReferences(manifest, false, fromDirectory, filesToIgnore, new AppMan.LockedFileReporter(LockedFileReporter), null, null, null, new ArrayList());

                // Update file sizes and hashes for the new references.
                // This uses Manifest methods rather than Utilities.AppMan.UpdateReferenceInfo
                // because in this case we want to report any missing files in the manifest.
                manifest.OutputMessages.Clear();
                manifest.ResolveFiles();
                manifest.UpdateFileInfo(targetFrameworkVersion);

                // Set entry point and config files
                Utilities.AppMan.SetSpecialFiles(manifest);
            }
            else
            {
                // -FromDirectory was not specified, but try to update 
                // existing references if possible.  
                                
                Utilities.AppMan.UpdateReferenceInfo(manifest, "", null, null, targetFrameworkVersion);
            }
        }

        /// <summary>
        /// Updates Deployment manifest
        /// </summary>
        /// <param name="manifest">DeploymentManifest object</param>
        /// <param name="deploymentManifestPath">Path of generated deployment manifest</param>
        /// <param name="appName">Application name</param>
        /// <param name="version">Version</param>
        /// <param name="processor">Processor type</param>
        /// <param name="applicationManifest">Application manifest from which to extract deployment details</param>
        /// <param name="applicationManifestPath">Path to application manifest</param>
        /// <param name="appCodeBase">Application codebase</param>
        /// <param name="appProviderUrl">Application provider URL</param>
        /// <param name="minVersion">Minimum version</param>
        /// <param name="install">Install state</param>
        /// <param name="includeDeploymentProviderUrl"></param>
        /// <param name="publisherName">Publisher name</param>
        /// <param name="supportUrl">Support URL</param>
        /// <param name="targetFrameworkVersion">Target Framework version</param>
        /// <param name="trustUrlParameters">Specifies if URL parameters should be trusted</param>
        public static void UpdateDeploymentManifest(DeployManifest manifest, string deploymentManifestPath,
            string appName, Version version, Processors processor,
            ApplicationManifest applicationManifest, string applicationManifestPath,
            string appCodeBase, string appProviderUrl, string minVersion,
            TriStateBool install,
            TriStateBool includeDeploymentProviderUrl,
            string publisherName, string supportUrl, string targetFrameworkVersion,
            TriStateBool trustUrlParameters)
        {
            if (install != TriStateBool.Undefined)
            {
                if (install == TriStateBool.False)
                {
                    manifest.Install = false;
                    manifest.DisallowUrlActivation = false;
                }
                else
                {
                    manifest.Install = true;
                    manifest.UpdateEnabled = true;
                    manifest.UpdateMode = UpdateMode.Foreground;

                    // We need to activate the set method on these fields since the XML
                    // representation might not be in sync with the propery value.
                    manifest.UpdateInterval = manifest.UpdateInterval;
                    manifest.UpdateUnit = manifest.UpdateUnit;
                }
            }

            if (appName != null)
            {
                manifest.AssemblyIdentity.Name = appName;
                if (appName.EndsWith(".app") && appName.Length > 4)
                {
                    manifest.Product = appName.Substring(0, appName.Length - 4);
                }
                else
                {
                    manifest.Product = appName;
                }
            }

            if (!string.IsNullOrEmpty(publisherName))
            {
                manifest.Publisher = publisherName;
            }
            else
            {
#if RUNTIME_TYPE_NETCORE
                // GetRegisteredOrganization() is a Windows-only API
                manifest.Publisher = string.Empty;
                if (OperatingSystem.IsWindows())
#endif
                // Get the default publisher name
                manifest.Publisher = Utilities.Misc.GetRegisteredOrganization();
            }

            // Ensure Publisher is not empty (otherwise ClickOnce runtime will refuse to load the manifest)
            if (string.IsNullOrEmpty(manifest.Publisher)) 
            {
                manifest.Publisher = manifest.Product;
            }

            if (!string.IsNullOrEmpty(supportUrl))
            {
                manifest.SupportUrl = supportUrl;
            }

            if (version != null)
            {
                manifest.AssemblyIdentity.Version = version.ToString();
            }

            if (processor != Processors.Undefined)
            {
                manifest.AssemblyIdentity.ProcessorArchitecture = processor.ToString();
            }

            // Culture is not supported by command-line arguments, but it gets defaulted if not already present in the manifest
            if ((manifest.AssemblyIdentity.Culture == null) || (manifest.AssemblyIdentity.Culture.Length == 0))
            {
                manifest.AssemblyIdentity.Culture = defaultCulture;
            }
            if (includeDeploymentProviderUrl == TriStateBool.False)
            {
                manifest.DeploymentUrl = null;
            }
            else if (appProviderUrl != null)
            {
                manifest.DeploymentUrl = appProviderUrl;
            }

            if (minVersion != null)
            {
                if (minVersion != string.Empty)
                {
                    manifest.Install = true;
                    manifest.MinimumRequiredVersion = minVersion;
                }
                else
                {
                    manifest.MinimumRequiredVersion = null;
                }
            }

            if (applicationManifest != null)
            {
                SetApplicationManifestReference(manifest, deploymentManifestPath, applicationManifest, applicationManifestPath, targetFrameworkVersion);
            }

            if (appCodeBase != null)
            {
                SetApplicationCodeBase(manifest, appCodeBase);
            }

            if (trustUrlParameters == TriStateBool.True)
            {
                manifest.TrustUrlParameters = true;
            }
        }

        /// <summary>
        /// Notifies the user when the reference update process skips a locked file.
        /// </summary>
        /// <param name="filePath">Path of locked file.</param>
        public static void LockedFileReporter(string filePath)
        {
            Application.PrintErrorMessage(ErrorMessages.LockedFile, filePath);
        }


        /// <summary>
        /// Add application manifest details to a deployment manifest
        /// </summary>
        /// <param name="deploymentManifest">DeploymentManifest object</param>
        /// <param name="deploymentManifestPath">Deployment manifest path</param>
        /// <param name="applicationManifest">ApplicationManifest object</param>
        /// <param name="applicationManifestPath">Application manifest path</param>
        /// <param name="targetFrameworkVersion">Target Framework version</param>
        private static void SetApplicationManifestReference(
            DeployManifest deploymentManifest,
            string deploymentManifestPath,
            ApplicationManifest applicationManifest,
            string applicationManifestPath,
            string targetFrameworkVersion)
        {
            // Validate parameters
            if ((deploymentManifest == null) || (applicationManifest == null) ||
                (deploymentManifestPath == null) || (applicationManifestPath == null))
            {
                Application.PrintErrorMessage(ErrorMessages.InternalError);
                return;
            }

            // Get absolute directory paths for both path parameters
            if (!Path.IsPathRooted(deploymentManifestPath))
            {
                deploymentManifestPath = Path.Combine(Environment.CurrentDirectory, deploymentManifestPath);
            }

            if (!Path.IsPathRooted(applicationManifestPath))
            {
                applicationManifestPath = Path.Combine(Environment.CurrentDirectory, applicationManifestPath);
            }

            string deploymentManifestDir = Path.GetDirectoryName(deploymentManifestPath);
            string applicationManifestDir = Path.GetDirectoryName(applicationManifestPath);

            // Codebase defaults to a canonical string from the application manifest identity
            string codebase = applicationManifest.AssemblyIdentity.Version + "\\" + applicationManifest.AssemblyIdentity.Name + ".manifest";

            // Use relative codebase if possible
            if (applicationManifestDir.StartsWith(deploymentManifestDir, StringComparison.OrdinalIgnoreCase))
            {
                codebase = applicationManifestPath.Substring(deploymentManifestDir.Length);

                if (codebase.StartsWith("\\"))
                {
                    codebase = codebase.Substring(1);
                }
            }

            // Create a new assembly reference for the application manifest
            AssemblyReference ar = new AssemblyReference(applicationManifestPath)
            {
                AssemblyIdentity = applicationManifest.AssemblyIdentity
            };

            // Update the deployment manifest
            deploymentManifest.AssemblyReferences.Clear();
            deploymentManifest.AssemblyReferences.Add(ar);
            deploymentManifest.EntryPoint = ar;
            deploymentManifest.ResolveFiles();
            deploymentManifest.UpdateFileInfo(targetFrameworkVersion);

            // Update the codebase so the canonical one gets used even if the application manifest
            // happens to live somewhere else at the moment.
            ar.TargetPath = codebase;
        }

        /// <summary>
        /// Sets the codebase of the application manifest reference in the given deployment manifest.
        /// </summary>
        /// <param name="deploymentManifest">DeploymentManifest object</param>
        /// <param name="appCodeBase">Codebase</param>
        private static void SetApplicationCodeBase(DeployManifest deploymentManifest, string appCodeBase)
        {
            if (deploymentManifest == null)
            {
                return;
            }

            AssemblyReferenceCollection collection = deploymentManifest.AssemblyReferences;

            AssemblyReference ar;
            if (collection.Count > 0)
            {
                ar = collection[0];
                ar.TargetPath = appCodeBase;
            }
            else
            {
                ar = new AssemblyReference(appCodeBase)
                {
                    TargetPath = appCodeBase
                };
                collection.Add(ar);
            }
        }

#if !RUNTIME_TYPE_NETCORE
        /// <summary>
        /// Set the application's trust information
        /// </summary>
        /// <param name="manifest">ApplicationManifest object</param>
        /// <param name="trustLevel">Trust level</param>
        private static void SetTrustLevel(ApplicationManifest manifest, Command.TrustLevels trustLevel)
        {
            if (trustLevel != Command.TrustLevels.None)
            {
                TrustInfo ti = new Microsoft.Build.Tasks.Deployment.ManifestUtilities.TrustInfo();
                manifest.TrustInfo = ti;

                if (trustLevel == Command.TrustLevels.FullTrust)
                {
                    ti.IsFullTrust = true;
                }
                else
                {
                    ti.PermissionSet = SecurityUtilities.ComputeZonePermissionSet(trustLevel.ToString(), null, null);
                    ti.IsFullTrust = false;
                }
            }
        }
#endif
    }
}
