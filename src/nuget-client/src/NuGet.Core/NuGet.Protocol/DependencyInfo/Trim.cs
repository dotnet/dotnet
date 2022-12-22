// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;

namespace NuGet.Protocol
{
    internal static class Trim
    {
        //  This code trims the metadata tree to just include packages that fit within an allowed-version specification.
        //  Packages outside of the range are removed from the tree. That removal should filter up the tree which is what the second
        //  pass is intended to achieve. This notion of allowed versions can trivially be extended for example to include
        //  for example multiple version ranges. (In fact this code could probably be parameterized with an actual trim lambda.)

        public static void TrimByAllowedVersions(RegistrationInfo registrationInfo, IDictionary<string, VersionRange> allowedVersions)
        {
            foreach (var allowedVersion in allowedVersions)
            {
                Execute(registrationInfo, allowedVersion);
            }
        }

        private static void Execute(RegistrationInfo registrationInfo, KeyValuePair<string, VersionRange> allowedVersion)
        {
            Pass1(registrationInfo, allowedVersion);

            var updated = true;
            while (updated)
            {
                updated = false;
                Pass2(registrationInfo, ref updated);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Pass1

        private static void Pass1(PackageInfo packageInfo, KeyValuePair<string, VersionRange> allowedVersion)
        {
            foreach (var dependencyInfo in packageInfo.Dependencies)
            {
                Pass1(dependencyInfo.RegistrationInfo, allowedVersion);
            }
        }

        private static void Pass1(RegistrationInfo registrationInfo, KeyValuePair<string, VersionRange> allowedVersion)
        {
            if (registrationInfo.Id == allowedVersion.Key)
            {
                IList<PackageInfo> packagesToRemove = registrationInfo.Packages.Where(p => !allowedVersion.Value.Satisfies(p.Version)).ToList();

                foreach (var packageToRemove in packagesToRemove)
                {
                    registrationInfo.Packages.Remove(packageToRemove);
                }
            }

            foreach (var child in registrationInfo.Packages)
            {
                Pass1(child, allowedVersion);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Pass2

        private static void Pass2(PackageInfo packageInfo, ref bool updated)
        {
            foreach (var dependencyInfo in packageInfo.Dependencies)
            {
                Pass2(dependencyInfo.RegistrationInfo, ref updated);
            }
        }

        private static void Pass2(RegistrationInfo registrationInfo, ref bool updated)
        {
            IList<PackageInfo> packagesToRemove = new List<PackageInfo>();

            foreach (var package in registrationInfo.Packages)
            {
                if (!CheckDependenciesExists(package))
                {
                    packagesToRemove.Add(package);
                }
            }

            foreach (var packageToRemove in packagesToRemove)
            {
                registrationInfo.Packages.Remove(packageToRemove);
                updated = true;
            }

            foreach (var child in registrationInfo.Packages)
            {
                Pass2(child, ref updated);
            }
        }

        private static bool CheckDependenciesExists(PackageInfo packageInfo)
        {
            foreach (var dependencyInfo in packageInfo.Dependencies)
            {
                if (dependencyInfo.RegistrationInfo.Packages.Count == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
