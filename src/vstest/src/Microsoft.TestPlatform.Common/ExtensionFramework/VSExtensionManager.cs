// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Common.ExtensionFramework;

/// <summary>
/// Manager for VisualStudio based extensions
/// </summary>
public class VSExtensionManager : IVSExtensionManager
{
    private const string ExtensionManagerService = "Microsoft.VisualStudio.ExtensionManager.ExtensionManagerService";
    private const string ExtensionManagerAssemblyName = @"Microsoft.VisualStudio.ExtensionManager";
    private const string ExtensionManagerImplAssemblyName = @"Microsoft.VisualStudio.ExtensionManager.Implementation";

    private const string SettingsManagerTypeName = "Microsoft.VisualStudio.Settings.ExternalSettingsManager";
    private const string SettingsManagerAssemblyName = @"Microsoft.VisualStudio.Settings.15.0";

    private readonly IFileHelper _fileHelper;

    private Assembly? _extensionManagerAssembly;
    private Assembly? _extensionManagerImplAssembly;
    private Type? _extensionManagerServiceType;

    private Assembly? _settingsManagerAssembly;
    private Type? _settingsManagerType;

    /// <summary>
    /// Default constructor for manager for Visual Studio based extensions
    /// </summary>
    public VSExtensionManager()
        : this(new FileHelper())
    {
    }

    internal VSExtensionManager(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    /// <summary>
    /// Get the available unit test extensions installed.
    /// If no extensions are installed then it returns an empty list.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetUnitTestExtensions()
    {
        try
        {
            return GetTestExtensionsInternal(Constants.UnitTestExtensionType);
        }
        catch (Exception ex)
        {
            string message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.FailedToFindInstalledUnitTestExtensions, ex);
            throw new TestPlatformException(message, ex);
        }
    }

    /// <summary>
    /// Get the unit tests extensions
    /// </summary>
    private IEnumerable<string> GetTestExtensionsInternal(string extensionType)
    {
        IEnumerable<string>? installedExtensions = new List<string>();

        // Navigate up to the IDE folder
        // In case of xcopyable vstest.console, this functionality is not supported.
        var installContext = new InstallationContext(_fileHelper);
        if (!installContext.TryGetVisualStudioDirectory(out string vsInstallPath))
        {
            throw new TestPlatformException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.VSInstallationNotFound));
        }

        // Adding resolution paths for resolving dependencies.
        var resolutionPaths = installContext.GetVisualStudioCommonLocations(vsInstallPath);
        using (var assemblyResolver = new AssemblyResolver(resolutionPaths))
        {
            var settingsManager = SettingsManagerType.GetMethod("CreateForApplication", new Type[] { typeof(string) })?.Invoke(null, new object[] { installContext.GetVisualStudioPath(vsInstallPath) });
            if (settingsManager == null)
            {
                EqtTrace.Warning("VSExtensionManager : Unable to create settings manager");
                return installedExtensions;
            }

            try
            {
                // create extension manager
                var extensionManager = Activator.CreateInstance(ExtensionManagerServiceType, settingsManager);

                if (extensionManager != null)
                {
                    installedExtensions = ExtensionManagerServiceType.GetMethod("GetEnabledExtensionContentLocations", new Type[] { typeof(string) })?.Invoke(
                        extensionManager, new object[] { extensionType }) as IEnumerable<string>;
                }
                else
                {
                    EqtTrace.Warning("VSExtensionManager : Unable to create extension manager");
                }
            }
            finally
            {
                // Dispose the settings manager
                if (settingsManager is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }

        return installedExtensions ?? new List<string>();
    }

    /// <summary>
    /// Used to explicitly load Microsoft.VisualStudio.ExtensionManager.dll
    /// </summary>
    private Assembly ExtensionManagerDefAssembly
    {
        get
        {
            _extensionManagerAssembly ??= Assembly.Load(new AssemblyName(ExtensionManagerAssemblyName));
            return _extensionManagerAssembly;
        }
    }

    /// <summary>
    /// Used to explicitly load Microsoft.VisualStudio.ExtensionManager.Implementation.dll
    /// </summary>
    private Assembly? ExtensionManagerImplAssembly
    {
        get
        {
            if (_extensionManagerImplAssembly == null)
            {
                // Make sure ExtensionManager assembly is already loaded.
                Assembly extensionMgrAssembly = ExtensionManagerDefAssembly;
                if (extensionMgrAssembly != null)
                {
                    _extensionManagerImplAssembly = Assembly.Load(new AssemblyName(ExtensionManagerImplAssemblyName));
                }
            }

            return _extensionManagerImplAssembly;
        }
    }

    /// <summary>
    /// Returns the Type of ExtensionManagerService.
    /// </summary>
    private Type ExtensionManagerServiceType
    {
        get
        {
            if (_extensionManagerServiceType == null)
            {
                TPDebug.Assert(ExtensionManagerImplAssembly is not null, "ExtensionManagerImplAssembly is null");
                _extensionManagerServiceType = ExtensionManagerImplAssembly.GetType(ExtensionManagerService);
                TPDebug.Assert(_extensionManagerServiceType is not null, "_extensionManagerServiceType is null");
            }
            return _extensionManagerServiceType;
        }
    }

    /// <summary>
    /// Used to explicitly load Microsoft.VisualStudio.Settings.15.0.dll
    /// </summary>
    private Assembly SettingsManagerAssembly
    {
        get
        {
            _settingsManagerAssembly ??= Assembly.Load(new AssemblyName(SettingsManagerAssemblyName));

            return _settingsManagerAssembly;
        }
    }

    private Type SettingsManagerType
    {
        get
        {
            if (_settingsManagerType == null)
            {
                _settingsManagerType = SettingsManagerAssembly.GetType(SettingsManagerTypeName);
                TPDebug.Assert(_settingsManagerType is not null, "_settingsManagerType is null");
            }

            return _settingsManagerType;
        }
    }
}
