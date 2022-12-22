// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.ExtensionManager;
using Microsoft.VisualStudio.Shell;
using NuGet.VisualStudio.Implementation.Resources;

namespace NuGet.VisualStudio
{
    internal class ExtensionManagerShim
    {
        private static Type _iVsExtensionManagerType;
        private static Type _sVsExtensionManagerType;
        private static MethodInfo _tryGetInstalledExtensionMethod;
        private static bool _typesInitialized;

        private readonly object _extensionManager;

        public ExtensionManagerShim(object extensionManager, Action<string> errorHandler)
        {
            InitializeTypes(errorHandler);
            _extensionManager = extensionManager ?? AsyncPackage.GetGlobalService(_sVsExtensionManagerType);
        }

        private static void InitializeTypes(Action<string> errorHandler)
        {
            if (_typesInitialized)
            {
                return;
            }

            try
            {
                Assembly extensionManagerAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .First(a => a.FullName.StartsWith("Microsoft.VisualStudio.ExtensionManager,", StringComparison.InvariantCultureIgnoreCase));
                _sVsExtensionManagerType =
                    extensionManagerAssembly.GetType("Microsoft.VisualStudio.ExtensionManager.SVsExtensionManager");
                _iVsExtensionManagerType =
                    extensionManagerAssembly.GetType("Microsoft.VisualStudio.ExtensionManager.IVsExtensionManager");
                _tryGetInstalledExtensionMethod = _iVsExtensionManagerType.GetMethod("TryGetInstalledExtension",
                    new[] { typeof(string), typeof(IInstalledExtension).MakeByRefType() });
                if (_tryGetInstalledExtensionMethod == null || _sVsExtensionManagerType == null)
                {
                    throw new Exception();
                }

                _typesInitialized = true;
            }
            catch
            {
                // if any of the types or methods cannot be loaded throw an error. this indicates that some API in
                // Microsoft.VisualStudio.ExtensionManager got changed.
                errorHandler(VsResources.PreinstalledPackages_ExtensionManagerError);
            }
        }

        public bool TryGetExtensionInstallPath(string extensionId, out string installPath)
        {
            installPath = null;
            object[] parameters = { extensionId, null };
            bool result = (bool)_tryGetInstalledExtensionMethod.Invoke(_extensionManager, parameters);
            if (!result)
            {
                return false;
            }
            var extension = parameters[1] as IInstalledExtension;
            installPath = extension?.InstallPath;
            return true;
        }
    }
}
