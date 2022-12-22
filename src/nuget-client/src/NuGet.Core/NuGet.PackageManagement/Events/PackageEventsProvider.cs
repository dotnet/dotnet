// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel.Composition;

namespace NuGet.ProjectManagement
{
    /// <summary>
    /// Provider for the PackageEvents singleton
    /// </summary>
    [Export(typeof(IPackageEventsProvider))]
    public class PackageEventsProvider : IPackageEventsProvider
    {
        private static PackageEvents _instance;

        public PackageEvents GetPackageEvents()
        {
            return Instance;
        }

        internal static PackageEvents Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PackageEvents();
                }

                return _instance;
            }
        }
    }
}
