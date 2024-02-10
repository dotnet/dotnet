// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.PackageManagement.VisualStudio.Telemetry;
using NuGet.VisualStudio;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>
    /// Contains the information specific to a Visual Basic or C# project or NetCore project.
    /// </summary>
    internal class VsProjectBuildProperties
        : IVsProjectBuildProperties
    {
        private readonly Lazy<Project> _dteProject;
        private Project _project;
        private readonly IVsBuildPropertyStorage _propertyStorage;
        private readonly IVsProjectBuildPropertiesTelemetry _buildPropertiesTelemetry;
        private readonly string[] _projectTypeGuids;

        public VsProjectBuildProperties(
            Project project,
            IVsBuildPropertyStorage propertyStorage,
            IVsProjectBuildPropertiesTelemetry buildPropertiesTelemetry,
            string[] projectTypeGuids)
        {
            Assumes.Present(project);

            _project = project;
            _propertyStorage = propertyStorage;
            _buildPropertiesTelemetry = buildPropertiesTelemetry;
            _projectTypeGuids = projectTypeGuids;
        }

        public VsProjectBuildProperties(
            Lazy<Project> project,
            IVsBuildPropertyStorage propertyStorage,
            IVsProjectBuildPropertiesTelemetry buildPropertiesTelemetry,
            string[] projectTypeGuids)
        {
            Assumes.Present(project);

            _dteProject = project;
            _propertyStorage = propertyStorage;
            _buildPropertiesTelemetry = buildPropertiesTelemetry;
            _projectTypeGuids = projectTypeGuids;
        }

        public string GetPropertyValue(string propertyName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Assumes.NotNullOrEmpty(propertyName);

            if (_propertyStorage == null)
            {
                // This project system does not implement IVsBuildPropertyStorage, meaning
                // this call will never return a value, even when the project file specifies
                // a value for the property.
                Debug.Fail("The project system does not implement IVsBuildPropertyStorage");
                return null;
            }

            var result = _propertyStorage.GetPropertyValue(
                pszPropName: propertyName,
                pszConfigName: null,
                storage: (uint)_PersistStorageType.PST_PROJECT_FILE,
                pbstrPropValue: out string output);

            if (result == VSConstants.S_OK && !string.IsNullOrWhiteSpace(output))
            {
                _buildPropertiesTelemetry.OnPropertyStorageUsed(propertyName, _projectTypeGuids);
                return output;
            }

            return null;
        }

        [Obsolete("New properties should use GetPropertyValue instead. Ideally we should migrate existing properties to stop using DTE as well.")]
        public string GetPropertyValueWithDteFallback(string propertyName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            Assumes.NotNullOrEmpty(propertyName);
            if (_propertyStorage != null)
            {
                var result = _propertyStorage.GetPropertyValue(
                    pszPropName: propertyName,
                    pszConfigName: null,
                    storage: (uint)_PersistStorageType.PST_PROJECT_FILE,
                    pbstrPropValue: out string output);

                if (result == VSConstants.S_OK && !string.IsNullOrWhiteSpace(output))
                {
                    _buildPropertiesTelemetry.OnPropertyStorageUsed(propertyName, _projectTypeGuids);
                    return output;
                }
            }

            try
            {
                if (_project == null)
                {
                    _project = _dteProject.Value;
                }
                var property = _project.Properties.Item(propertyName);
                _buildPropertiesTelemetry.OnDteUsed(propertyName, _projectTypeGuids);
                return property?.Value as string;
            }
            catch (ArgumentException)
            {
                // If the property doesn't exist this will throw an argument exception
            }

            return null;
        }
    }
}
