// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using NuGet.Common;
using NuGet.Frameworks;

namespace NuGet.Commands.PackCommand
{
    /// <summary>
    /// Contains Package specific properties for Warnings.
    /// </summary>
    public class PackageSpecificWarningProperties
    {
        /// <summary>
        /// Contains Package specific No warn properties.
        /// NuGetLogCode -> LibraryId -> Set of Frameworks.
        /// </summary>
        internal IDictionary<NuGetLogCode, IDictionary<string, ISet<NuGetFramework>>> Properties { get; private set; }

        /// <summary>
        /// Extracts PackageSpecific WarningProperties from a PackageSpec
        /// </summary>
        /// <param name="noWarnProperties">noWarnProperties containing the Dependencies with WarningProperties</param>
        /// <returns>PackageSpecific WarningProperties extracted from a noWarnProperties</returns>
        public static PackageSpecificWarningProperties CreatePackageSpecificWarningProperties(IDictionary<string, HashSet<(NuGetLogCode, NuGetFramework)>> noWarnProperties)
        {
            if (noWarnProperties == null)
            {
                return null;
            }

            // NuGetLogCode -> LibraryId -> Set of Frameworks.
            var warningProperties = new PackageSpecificWarningProperties();

            foreach (KeyValuePair<string, HashSet<(NuGetLogCode, NuGetFramework)>> packageNoWarnProperty in noWarnProperties)
            {
                string packageId = packageNoWarnProperty.Key;

                foreach ((NuGetLogCode nuGetLogCode, NuGetFramework nuGetFramework) propertyPair in packageNoWarnProperty.Value)
                {
                    warningProperties.Add(propertyPair.nuGetLogCode, packageId, propertyPair.nuGetFramework);
                }
            }

            return warningProperties;
        }

        /// <summary>
        /// Adds a NuGetLogCode into the NoWarn Set for the specified library Id and target graph.
        /// </summary>
        /// <param name="code">NuGetLogCode for which no warning should be thrown.</param>
        /// <param name="libraryId">Library for which no warning should be thrown.</param>
        /// <param name="framework">Target graph for which no warning should be thrown.</param>
        internal void Add(NuGetLogCode code, string libraryId, NuGetFramework framework)
        {
            Properties ??= new Dictionary<NuGetLogCode, IDictionary<string, ISet<NuGetFramework>>>();

            if (!Properties.TryGetValue(code, out IDictionary<string, ISet<NuGetFramework>> frameworksByLibraryId))
            {
                frameworksByLibraryId = new Dictionary<string, ISet<NuGetFramework>>(StringComparer.OrdinalIgnoreCase);
                Properties.Add(code, frameworksByLibraryId);
            }

            if (!frameworksByLibraryId.TryGetValue(libraryId, out ISet<NuGetFramework> frameworks))
            {
                frameworks = new HashSet<NuGetFramework>(NuGetFrameworkFullComparer.Instance);
                frameworksByLibraryId.Add(libraryId, frameworks);
            }

            frameworks.Add(framework);
        }

        /// <summary>
        /// Checks if a NuGetLogCode is part of the NoWarn list for the specified library Id and target graph.
        /// </summary>
        /// <param name="code">NuGetLogCode to be checked.</param>
        /// <param name="libraryId">library Id to be checked.</param>
        /// <param name="framework">target graph to be checked.</param>
        /// <returns>True if the NuGetLogCode is part of the NoWarn list for the specified libraryId and Target Graph.</returns>
        internal bool Contains(NuGetLogCode code, string libraryId, NuGetFramework framework)
        {
            return Properties != null &&
                Properties.TryGetValue(code, out var libraryIdsAndFrameworks) &&
                libraryIdsAndFrameworks.TryGetValue(libraryId, out var frameworkSet) &&
                frameworkSet.Contains(framework);
        }

        /// <summary>
        /// Attempts to suppress a warning log message.
        /// The decision is made based on the Package Specific no warn properties.
        /// </summary>
        /// <param name="message">Message that should be suppressed.</param>
        /// <returns>Bool indicating is the warning should be suppressed or not.</returns>
        internal bool ApplyNoWarnProperties(IPackLogMessage message)
        {
            return ApplyPackageSpecificNoWarnProperties(message);
        }

        /// <summary>
        /// Method is used to check is a warning should be suppressed due to package specific no warn properties.
        /// </summary>
        /// <param name="message">Message to be checked for no warn.</param>
        /// <returns>bool indicating if the IRestoreLogMessage should be suppressed or not.</returns>
        private bool ApplyPackageSpecificNoWarnProperties(IPackLogMessage message)
        {
            if (message.Level == LogLevel.Warning &&
                !string.IsNullOrEmpty(message.LibraryId) &&
                message.Framework != null)
            {
                // Suppress the warning if the code + libraryId combination is suppressed for given framework.
                if (Contains(message.Code, message.LibraryId, message.Framework))
                {
                    return true;
                }
            }

            // The message is not a warning or it does not contain a LibraryId or it is not suppressed in package specific settings.
            return false;
        }
    }
}
