// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NuGet.ProjectManagement;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Class that manages the binding redirect config section
    /// </summary>
    public class BindingRedirectManager
    {
        private static readonly XName AssemblyBindingName = AssemblyBinding.GetQualifiedName("assemblyBinding");
        private static readonly XName DependentAssemblyName = AssemblyBinding.GetQualifiedName("dependentAssembly");
        private static readonly XName BindingRedirectName = AssemblyBinding.GetQualifiedName("bindingRedirect");

        private string ConfigurationFile { get; set; }
        private IMSBuildProjectSystem MSBuildNuGetProjectSystem { get; set; }

        public BindingRedirectManager(string configurationFile, IMSBuildProjectSystem msBuildNuGetProjectSystem)
        {
            if (String.IsNullOrEmpty(configurationFile))
            {
                throw new ArgumentException(
                    Strings.Argument_Cannot_Be_Null_Or_Empty,
                    nameof(configurationFile));
            }
            if (msBuildNuGetProjectSystem == null)
            {
                throw new ArgumentNullException(nameof(msBuildNuGetProjectSystem));
            }

            ConfigurationFile = configurationFile;
            MSBuildNuGetProjectSystem = msBuildNuGetProjectSystem;
        }

        public void AddBindingRedirects(IEnumerable<AssemblyBinding> bindingRedirects)
        {
            if (bindingRedirects == null)
            {
                throw new ArgumentNullException(nameof(bindingRedirects));
            }

            // Do nothing if there are no binding redirects to add, bail out
            if (!bindingRedirects.Any())
            {
                return;
            }

            // Get the configuration file
            var configFileFullPath = GetConfigurationFileFullPath();
            XDocument document = GetConfiguration(configFileFullPath);

            // Get the runtime element
            XElement runtime = document.Root.Element("runtime");

            if (runtime == null)
            {
                // Add the runtime element to the configuration document
                runtime = new XElement("runtime");
                document.Root.AddIndented(runtime);
            }

            // Get all of the current bindings in config
            ILookup<AssemblyBinding, XElement> currentBindings = GetAssemblyBindings(document);

            XElement assemblyBindingElement = null;
            foreach (var bindingRedirect in bindingRedirects)
            {
                // Look to see if we already have this in the list of bindings already in config.
                if (currentBindings.Contains(bindingRedirect))
                {
                    var existingBindings = currentBindings[bindingRedirect];
                    if (existingBindings.Any())
                    {
                        // Remove all but the first assembly binding elements
                        foreach (var bindingElement in existingBindings.Skip(1))
                        {
                            RemoveElement(bindingElement);
                        }

                        UpdateBindingRedirectElement(existingBindings.First(), bindingRedirect);
                        // Since we have a binding element, the assembly binding node (parent node) must exist.
                        // We don't need to do anything more here.
                        continue;
                    }
                }

                if (assemblyBindingElement == null)
                {
                    // Get an assembly binding element to use
                    assemblyBindingElement = GetAssemblyBindingElement(runtime);
                }
                // Add the binding to that element

                assemblyBindingElement.AddIndented(bindingRedirect.ToXElement());
            }

            // Save the file
            Save(configFileFullPath, document);
        }

        private string GetConfigurationFileFullPath()
        {
            var fullPaths = MSBuildNuGetProjectSystem.GetFullPaths(ConfigurationFile);
            if (fullPaths.Any())
            {
                // if there are multiple configuration files in the project,
                // we need to pick the one that is directly under the project if it exists.
                return fullPaths.First();
            }
            else
            {
                return Path.Combine(MSBuildNuGetProjectSystem.ProjectFullPath, ConfigurationFile);
            }
        }

        public void RemoveBindingRedirects(IEnumerable<AssemblyBinding> bindingRedirects)
        {
            if (bindingRedirects == null)
            {
                throw new ArgumentNullException(nameof(bindingRedirects));
            }

            // Do nothing if there are no binding redirects to remove, bail out
            if (!bindingRedirects.Any())
            {
                return;
            }

            // Get the configuration file
            var configFileFullPath = GetConfigurationFileFullPath();
            XDocument document = GetConfiguration(configFileFullPath);

            // Get all of the current bindings in config
            ILookup<AssemblyBinding, XElement> currentBindings = GetAssemblyBindings(document);

            if (!currentBindings.Any())
            {
                return;
            }

            foreach (var bindingRedirect in bindingRedirects)
            {
                if (currentBindings.Contains(bindingRedirect))
                {
                    foreach (var bindingElement in currentBindings[bindingRedirect])
                    {
                        RemoveElement(bindingElement);
                    }
                }
            }

            // Save the file
            Save(configFileFullPath, document);
        }

        private static void RemoveElement(XElement element)
        {
            // Hold onto the parent element before removing the element
            XElement parentElement = element.Parent;

            // Remove the element from the document if we find a match
            element.RemoveIndented();

            if (!parentElement.HasElements)
            {
                parentElement.RemoveIndented();
            }
        }

        private static XElement GetAssemblyBindingElement(XElement runtime)
        {
            // Pick the first assembly binding element or create one if there aren't any
            XElement assemblyBinding = runtime.Elements(AssemblyBindingName).FirstOrDefault();
            if (assemblyBinding != null)
            {
                return assemblyBinding;
            }

            assemblyBinding = new XElement(AssemblyBindingName);
            runtime.AddIndented(assemblyBinding);

            return assemblyBinding;
        }

        private void Save(string configFileFullPath, XDocument document)
        {
            using (var memoryStream = new MemoryStream())
            {
                document.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // MSBuildNuGetProjectSystem.AddFile() can't handle full path if the app.config
                // file does not exist in the project yet. This happens when NuGet first creates
                // app.config in the project directory. In this case, only the file name is passed.
                var path = configFileFullPath;
                var defaultConfigFile = Path.Combine(
                    MSBuildNuGetProjectSystem.ProjectFullPath,
                    ConfigurationFile);
                if (configFileFullPath.Equals(defaultConfigFile, StringComparison.OrdinalIgnoreCase))
                {
                    path = ConfigurationFile;
                }

                MSBuildNuGetProjectSystem.AddFile(path, memoryStream);
            }
        }

        private static ILookup<AssemblyBinding, XElement> GetAssemblyBindings(XDocument document)
        {
            XElement runtime = document.Root.Element("runtime");

            IEnumerable<XElement> assemblyBindingElements = Enumerable.Empty<XElement>();
            if (runtime != null)
            {
                assemblyBindingElements = GetAssemblyBindingElements(runtime);
            }

            // We're going to need to know which element is associated with what binding for removal
            var assemblyElementPairs = from dependentAssemblyElement in assemblyBindingElements
                                       select new
                                       {
                                           Binding = AssemblyBinding.Parse(dependentAssemblyElement),
                                           Element = dependentAssemblyElement
                                       };

            // Return a mapping from binding to element
            return assemblyElementPairs.ToLookup(p => p.Binding, p => p.Element);
        }

        private static IEnumerable<XElement> GetAssemblyBindingElements(XElement runtime)
        {
            return runtime.Elements(AssemblyBindingName)
                .Elements(DependentAssemblyName);
        }

        private XDocument GetConfiguration(string configFileFullPath)
        {
            try
            {
                return ProjectManagement.XmlUtility.GetOrCreateDocument(
                    "configuration",
                    Path.GetDirectoryName(configFileFullPath),
                    Path.GetFileName(configFileFullPath),
                    MSBuildNuGetProjectSystem.NuGetProjectContext);
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format(CultureInfo.CurrentCulture, Strings.Error_WhileLoadingConfigForBindingRedirects,
                                          configFileFullPath,
                                          ex.Message);

                throw new InvalidOperationException(errorMessage, ex);
            }
        }

        private static void UpdateBindingRedirectElement(
            XElement existingDependentAssemblyElement,
            AssemblyBinding newBindingRedirect)
        {
            var existingBindingRedirectElement = existingDependentAssemblyElement.Element(BindingRedirectName);
            // Since we've successfully parsed this node, it has to be valid and this child must exist.
            if (existingBindingRedirectElement != null)
            {
                existingBindingRedirectElement.SetAttributeValue(XName.Get("oldVersion"), newBindingRedirect.OldVersion);
                existingBindingRedirectElement.SetAttributeValue(XName.Get("newVersion"), newBindingRedirect.NewVersion);
            }
            else
            {
                // At this point, <dependentAssemblyElement> already exists, but <bindingRedirectElement> does not.
                // So, extract the <bindingRedirectElement> from the newDependencyAssemblyElement, and add it
                // to the existingDependentAssemblyElement
                var newDependentAssemblyElement = newBindingRedirect.ToXElement();
                var newBindingRedirectElement = newDependentAssemblyElement.Element(BindingRedirectName);
                existingDependentAssemblyElement.AddIndented(newBindingRedirectElement);
            }
        }
    }
}
