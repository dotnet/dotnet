// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;

namespace NuGet.PackageManagement
{
    /// <summary>
    /// Provider for the PackageEvents singleton
    /// </summary>
    [Export(typeof(IPackageProjectEventsProvider))]
    public class PackageProjectEventsProvider : IPackageProjectEventsProvider
    {
        private static PackageProjectEvents _instance;

        public PackageProjectEvents GetPackageProjectEvents()
        {
            return Instance;
        }

        internal static PackageProjectEvents Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PackageProjectEvents();
                }

                return _instance;
            }
        }
    }
}
