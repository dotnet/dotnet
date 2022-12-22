// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TemplateWizard;
using Microsoft.VisualStudio.Threading;
using NuGet.Common;
using NuGet.PackageManagement;
using NuGet.PackageManagement.VisualStudio;
using NuGet.ProjectManagement;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using NuGet.VisualStudio.Implementation.Extensibility;
using NuGet.VisualStudio.Implementation.Resources;
using Task = System.Threading.Tasks.Task;
using XmlUtility = NuGet.Shared.XmlUtility;

namespace NuGet.VisualStudio
{
    [Export(typeof(IVsTemplateWizard))]
    public class VsTemplateWizard : IVsTemplateWizard
    {
        private const string DefaultRepositoryDirectory = "packages";
        private const string WizardDataElementName = "WizardData";
        private const string IsPreunzippedAttributeName = "isPreunzipped";
        private const string ForceDesignTimeBuildAttributeName = "forceDesignTimeBuild";

        private readonly IVsPackageInstaller _installer;
        private IEnumerable<PreinstalledPackageConfiguration> _configurations;

        private DTE _dte;
        private Lazy<PreinstalledPackageInstaller> _preinstalledPackageInstaller;
#pragma warning disable CS0618 // Type or member is obsolete
        private readonly IVsPackageInstallerServices _packageServices;
#pragma warning restore CS0618 // Type or member is obsolete
        private readonly IOutputConsoleProvider _consoleProvider;
        private readonly IVsSolutionManager _solutionManager;
        private readonly Configuration.ISettings _settings;
        private readonly ISourceRepositoryProvider _sourceProvider;
        private readonly IVsProjectAdapterProvider _vsProjectAdapterProvider;

        private JoinableTaskFactory PumpingJTF { get; }

        [ImportingConstructor]
        public VsTemplateWizard(
            IVsPackageInstaller installer,
#pragma warning disable CS0618 // Type or member is obsolete
            IVsPackageInstallerServices packageServices,
#pragma warning restore CS0618 // Type or member is obsolete
            IOutputConsoleProvider consoleProvider,
            IVsSolutionManager solutionManager,
            Configuration.ISettings settings,
            ISourceRepositoryProvider sourceProvider,
            IVsProjectAdapterProvider vsProjectAdapterProvider
            )
        {
            _installer = installer;
            _packageServices = packageServices;
            _consoleProvider = consoleProvider;
            _solutionManager = solutionManager;
            _settings = settings;
            _sourceProvider = sourceProvider;
            _vsProjectAdapterProvider = vsProjectAdapterProvider;
            _preinstalledPackageInstaller = new Lazy<PreinstalledPackageInstaller>(() =>
                                            {
                                                return new PreinstalledPackageInstaller(_packageServices, _solutionManager, _settings, _sourceProvider, (VsPackageInstaller)_installer, _vsProjectAdapterProvider);
                                            });
            PumpingJTF = new PumpingJTF(NuGetUIThreadHelper.JoinableTaskFactory);
        }

        private PreinstalledPackageInstaller PreinstalledPackageInstaller
        {
            get
            {
                return _preinstalledPackageInstaller.Value;
            }
        }

        private IEnumerable<PreinstalledPackageConfiguration> GetConfigurationsFromVsTemplateFile(string vsTemplatePath)
        {
            XDocument document = LoadDocument(vsTemplatePath);

            return GetConfigurationsFromXmlDocument(document, vsTemplatePath);
        }

        internal IEnumerable<PreinstalledPackageConfiguration> GetConfigurationsFromXmlDocument(
            XDocument document,
            string vsTemplatePath,
            object vsExtensionManager = null,
            IEnumerable<IRegistryKey> registryKeys = null)
        {
            // Ignore XML namespaces since VS does not check them either when loading vstemplate files.
            var packagesElements = document.Root
                .ElementsNoNamespace(WizardDataElementName)
                .ElementsNoNamespace("packages");

            foreach (var packagesElement in packagesElements)
            {
                IList<PreinstalledPackageInfo> packages = Array.Empty<PreinstalledPackageInfo>();
                string repositoryPath = null;
                var isPreunzipped = false;
                var forceDesignTimeBuild = false;

                var isPreunzippedString = packagesElement.GetOptionalAttributeValue(IsPreunzippedAttributeName);
                if (!string.IsNullOrEmpty(isPreunzippedString))
                {
                    _ = bool.TryParse(isPreunzippedString, out isPreunzipped);
                }

                var forceDesignTimeBuildString =
                    packagesElement.GetOptionalAttributeValue(ForceDesignTimeBuildAttributeName);

                if (!string.IsNullOrEmpty(forceDesignTimeBuildString))
                {
                    _ = bool.TryParse(forceDesignTimeBuildString, out forceDesignTimeBuild);
                }

                packages = GetPackages(packagesElement).ToList();

                if (packages.Count > 0)
                {
                    var repositoryType = GetRepositoryType(packagesElement);
                    repositoryPath = GetRepositoryPath(
                        packagesElement,
                        repositoryType,
                        vsTemplatePath,
                        vsExtensionManager,
                        registryKeys);
                }

                yield return new PreinstalledPackageConfiguration(
                    repositoryPath,
                    packages,
                    isPreunzipped,
                    forceDesignTimeBuild);
            }
        }

        private IEnumerable<PreinstalledPackageInfo> GetPackages(XElement packagesElement)
        {
            var declarations = (from packageElement in packagesElement.ElementsNoNamespace("package")
                                let id = packageElement.GetOptionalAttributeValue("id")
                                let version = packageElement.GetOptionalAttributeValue("version")
                                let skipAssemblyReferences = packageElement.GetOptionalAttributeValue("skipAssemblyReferences")
                                let includeDependencies = packageElement.GetOptionalAttributeValue("includeDependencies")
                                select new { id, version, skipAssemblyReferences, includeDependencies }).ToList();

            NuGetVersion semVer = null;
            bool skipAssemblyReferencesValue;
            bool includeDependenciesValue;
            var missingOrInvalidAttributes = from declaration in declarations
                                             where
                                                 String.IsNullOrWhiteSpace(declaration.id) ||
                                                 String.IsNullOrWhiteSpace(declaration.version) ||
                                                 !NuGetVersion.TryParse(declaration.version, out semVer) ||
                                                 (declaration.skipAssemblyReferences != null &&
                                                  !Boolean.TryParse(declaration.skipAssemblyReferences, out skipAssemblyReferencesValue)) ||
                                                 (declaration.includeDependencies != null &&
                                                  !Boolean.TryParse(declaration.includeDependencies, out includeDependenciesValue))
                                             select declaration;

            if (missingOrInvalidAttributes.Any())
            {
                ShowErrorMessage(
                    VsResources.TemplateWizard_InvalidPackageElementAttributes);
                throw new WizardBackoutException();
            }

            return from declaration in declarations
                   select new PreinstalledPackageInfo(
                       declaration.id,
                       declaration.version,
                       skipAssemblyReferences: declaration.skipAssemblyReferences != null && Boolean.Parse(declaration.skipAssemblyReferences),

                       // Note that the declaration uses "includeDependencies" but we need to invert it to become ignoreDependencies
                       // The declaration uses includeDependencies so that the default value can be 'false'
                       ignoreDependencies: !(declaration.includeDependencies != null && Boolean.Parse(declaration.includeDependencies))
                       );
        }

        private string GetRepositoryPath(
            XElement packagesElement,
            RepositoryType repositoryType,
            string vsTemplatePath,
            object vsExtensionManager,
            IEnumerable<IRegistryKey> registryKeys)
        {
            switch (repositoryType)
            {
                case RepositoryType.Template:
                    return Path.GetDirectoryName(vsTemplatePath);

                case RepositoryType.Extension:
                    return GetExtensionRepositoryPath(packagesElement, vsExtensionManager);

                case RepositoryType.Registry:
                    return GetRegistryRepositoryPath(packagesElement, registryKeys);
            }
            // should not happen
            return null;
        }

        private string GetExtensionRepositoryPath(XElement packagesElement, object vsExtensionManager)
        {
            string repositoryId = packagesElement.GetOptionalAttributeValue("repositoryId");
            if (repositoryId == null)
            {
                ShowErrorMessage(VsResources.TemplateWizard_MissingExtensionId);
                throw new WizardBackoutException();
            }

            return PreinstalledPackageInstaller.GetExtensionRepositoryPath(repositoryId, vsExtensionManager, ThrowWizardBackoutError);
        }

        private string GetRegistryRepositoryPath(XElement packagesElement, IEnumerable<IRegistryKey> registryKeys)
        {
            string keyName = packagesElement.GetOptionalAttributeValue("keyName");
            if (String.IsNullOrEmpty(keyName))
            {
                ShowErrorMessage(VsResources.TemplateWizard_MissingRegistryKeyName);
                throw new WizardBackoutException();
            }

            return PreinstalledPackageInstaller.GetRegistryRepositoryPath(keyName, registryKeys, ThrowWizardBackoutError);
        }

        private RepositoryType GetRepositoryType(XElement packagesElement)
        {
            string repositoryAttributeValue = packagesElement.GetOptionalAttributeValue("repository");
            switch (repositoryAttributeValue)
            {
                case "extension":
                    return RepositoryType.Extension;

                case "registry":
                    return RepositoryType.Registry;

                case "template":
                case null:
                    return RepositoryType.Template;

                default:
                    ShowErrorMessage(string.Format(CultureInfo.CurrentCulture, VsResources.TemplateWizard_InvalidRepositoryAttribute,
                        repositoryAttributeValue));
                    throw new WizardBackoutException();
            }
        }

        internal virtual XDocument LoadDocument(string path)
        {
            return XmlUtility.Load(path, LoadOptions.PreserveWhitespace);
        }

        private Task ProjectFinishedGeneratingAsync(Project project)
        {
            return TemplateFinishedGeneratingAsync(project);
        }

        private async Task ProjectItemFinishedGeneratingAsync(ProjectItem projectItem)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            await TemplateFinishedGeneratingAsync(projectItem.ContainingProject);
        }

        private async Task TemplateFinishedGeneratingAsync(Project project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var forceDesignTimeBuild = false;
            foreach (var configuration in _configurations)
            {
                if (configuration.Packages.Any())
                {
                    var packageManagementFormat = new PackageManagementFormat(_settings);
                    // 1 means PackageReference
                    var preferPackageReference = packageManagementFormat.SelectedPackageManagementFormat == 1;
                    await PreinstalledPackageInstaller.PerformPackageInstallAsync(
                        project,
                        configuration,
                        preferPackageReference,
                        ShowWarningMessage,
                        ShowErrorMessage);
                }

                if (configuration.ForceDesignTimeBuild)
                {
                    forceDesignTimeBuild = true;
                }
            }

            if (forceDesignTimeBuild)
            {
                await RunDesignTimeBuildAsync(project);
            }
        }

        private async Task RunDesignTimeBuildAsync(Project project)
        {
            await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

            var solution = await AsyncServiceProvider.GlobalProvider.GetServiceAsync<SVsSolution, IVsSolution>(throwOnFailure: false);

            if (solution != null)
            {
                IVsHierarchy hierarchy;
                if (ErrorHandler.Succeeded(solution.GetProjectOfUniqueName(project.UniqueName, out hierarchy))
                    && hierarchy != null)
                {
                    var solutionBuild = hierarchy as IVsProjectBuildSystem;

                    if (solutionBuild != null)
                    {
                        if (ErrorHandler.Succeeded(solutionBuild.StartBatchEdit()))
                        {
                            solutionBuild.EndBatchEdit();
                        }
                    }
                }
            }
        }

        private void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (runKind != WizardRunKind.AsNewProject
                && runKind != WizardRunKind.AsNewItem)
            {
                ShowErrorMessage(VsResources.TemplateWizard_InvalidWizardRunKind);
                throw new WizardBackoutException();
            }

            _dte = (DTE)automationObject;
            PreinstalledPackageInstaller.InfoHandler = message =>
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                _dte.StatusBar.Text = message;
            };

            if (customParams.Length > 0)
            {
                var vsTemplatePath = (string)customParams[0];
                _configurations = GetConfigurationsFromVsTemplateFile(vsTemplatePath);
            }

            if (replacementsDictionary != null)
            {
                AddTemplateParameters(replacementsDictionary);
            }
        }

        private string GetSolutionDirectoryFromDte(DTE dte)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            // Use .Properties.Item("Path") instead of .FullName because .FullName might not be
            // available if the solution is just being created
            string solutionFilePath;

            var property = dte.Solution.Properties.Item("Path");
            if (property == null)
            {
                return null;
            }
            try
            {
                // When using a temporary solution, (such as by saying File -> New File), querying this value throws.
                // Since we wouldn't be able to do manage any packages at this point, we return null. Consumers of this property typically
                // use a String.IsNullOrEmpty check either way, so it's alright.
                solutionFilePath = (string)property.Value;
            }
            catch (COMException)
            {
                return null;
            }

            return Path.GetDirectoryName(solutionFilePath);
        }

        private void AddTemplateParameters(Dictionary<string, string> replacementsDictionary)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // add the $nugetpackagesfolder$ parameter which returns relative path to the solution's packages folder.
            // this is used by project templates to include assembly references directly inside the template project file
            // without relying on nuget to install the actual packages.
            string targetInstallDir;
            if (replacementsDictionary.TryGetValue("$destinationdirectory$", out targetInstallDir))
            {
                string solutionRepositoryPath = null;
                if (_dte.Solution != null
                    && _dte.Solution.IsOpen)
                {
                    var solutionDirectory = GetSolutionDirectoryFromDte(_dte);
                    solutionRepositoryPath = PackagesFolderPathUtility.GetPackagesFolderPath(solutionDirectory, _settings);
                }
                else
                {
                    string solutionDir = DetermineSolutionDirectory(replacementsDictionary);
                    if (!String.IsNullOrEmpty(solutionDir))
                    {
                        // If the project is a Website that is created on an Http location,
                        // solutionDir may be an Http address, e.g. http://localhost.
                        // In that case, we have to use forward slash instead of backward one.
                        if (Uri.IsWellFormedUriString(solutionDir, UriKind.Absolute))
                        {
                            solutionRepositoryPath = PathUtility.EnsureTrailingForwardSlash(solutionDir) + DefaultRepositoryDirectory;
                        }
                        else
                        {
                            solutionRepositoryPath = Path.Combine(solutionDir, DefaultRepositoryDirectory);
                        }
                    }
                }

                if (solutionRepositoryPath != null)
                {
                    // If the project is a Website that is created on an Http location,
                    // targetInstallDir may be an Http address, e.g. http://localhost.
                    // In that case, we have to use forward slash instead of backward one.
                    if (Uri.IsWellFormedUriString(targetInstallDir, UriKind.Absolute))
                    {
                        targetInstallDir = PathUtility.EnsureTrailingForwardSlash(targetInstallDir);
                    }
                    else
                    {
                        targetInstallDir = PathUtility.EnsureTrailingSlash(targetInstallDir);
                    }

                    replacementsDictionary["$nugetpackagesfolder$"] =
                        PathUtility.EnsureTrailingSlash(PathUtility.GetRelativePath(targetInstallDir, solutionRepositoryPath));
                }
            }

            // provide a current timpestamp (for use by universal provider)
            replacementsDictionary["$timestamp$"] = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.CurrentCulture);
        }

        internal virtual void ThrowWizardBackoutError(string message)
        {
            ShowErrorMessage(message);
            throw new WizardBackoutException();
        }

        internal virtual void ShowErrorMessage(string message)
        {
            MessageHelper.ShowErrorMessage(message, VsResources.TemplateWizard_ErrorDialogTitle);
        }

        internal virtual void ShowWarningMessage(string message)
        {
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async () =>
            {
                var console = await _consoleProvider.CreatePackageManagerConsoleAsync();
                await console.WriteLineAsync(message);
            }
            );
        }

        void IWizard.BeforeOpeningFile(ProjectItem projectItem)
        {
            // do nothing
        }

        void IWizard.ProjectFinishedGenerating(Project project)
        {
            PumpingJTF.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    await ProjectFinishedGeneratingAsync(project);
                });
        }

        void IWizard.ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            PumpingJTF.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    await ProjectItemFinishedGeneratingAsync(projectItem);
                });
        }

        void IWizard.RunFinished()
        {
        }

        void IWizard.RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            // We simply use ThreadHelper.JoinableTaskFactory.Run instead of PumpingJTF.Run, unlike,
            // VsPackageInstaller and VsPackageUninstaller. Because, no powershell scripts get executed
            // as part of the operations performed below. Powershell scripts need to be executed on the
            // pipeline execution thread and they might try to access DTE. Doing that under
            // ThreadHelper.JoinableTaskFactory.Run will consistently make the UI stop responding
            NuGetUIThreadHelper.JoinableTaskFactory.Run(async delegate
                {
                    await NuGetUIThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();

                    // alternatively could get body of WizardData element from replacementsDictionary["$wizarddata$"] instead of parsing vstemplate file.
                    RunStarted(automationObject, replacementsDictionary, runKind, customParams);
                });
        }

        bool IWizard.ShouldAddProjectItem(string filePath)
        {
            // always add all project items
            return true;
        }

        internal static string DetermineSolutionDirectory(Dictionary<string, string> replacementsDictionary)
        {
            // the $solutiondirectory$ parameter is available in VS11 RC and later
            // No $solutiondirectory$? Ok, we're in the case where the solution is in
            // the same directory as the project
            // Is $specifiedsolutionname$ null or empty? We're definitely in the solution
            // in same directory as project case.

            string solutionName;
            string solutionDir;
            bool ignoreSolutionDir = (replacementsDictionary.TryGetValue("$specifiedsolutionname$", out solutionName) && String.IsNullOrEmpty(solutionName));

            // We check $destinationdirectory$ twice because we want the following precedence:
            // 1. If $specifiedsolutionname$ == null, ALWAYS use $destinationdirectory$
            // 2. Otherwise, use $solutiondirectory$ if available
            // 3. If $solutiondirectory$ is not available, use $destinationdirectory$.
            if ((ignoreSolutionDir && replacementsDictionary.TryGetValue("$destinationdirectory$", out solutionDir))
                || replacementsDictionary.TryGetValue("$solutiondirectory$", out solutionDir)
                || replacementsDictionary.TryGetValue("$destinationdirectory$", out solutionDir))
            {
                return solutionDir;
            }
            return null;
        }

        private enum RepositoryType
        {
            /// <summary>
            /// Cache location relative to the template (inside the same folder as the vstemplate file)
            /// </summary>
            Template,

            /// <summary>
            /// Cache location relative to the VSIX that packages the project template
            /// </summary>
            Extension,

            /// <summary>
            /// Cache location stored in the registry
            /// </summary>
            Registry
        }
    }
}
