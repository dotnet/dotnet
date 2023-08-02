// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.VisualStudio
{
    public class ExperimentationConstants
    {
        internal ExperimentationConstants(string flightFlag, string flightEnvironmentVariable)
        {
            FlightFlag = flightFlag ?? throw new ArgumentNullException(nameof(flightFlag));
            FlightEnvironmentVariable = flightEnvironmentVariable;
        }

        /// <summary>
        /// The value defined for the VS experimentation service.
        /// </summary>
        internal string FlightFlag { get; }

        /// <summary>
        /// The environment variable means of enabled this feature.
        /// Might be <see cref="null"/>.
        /// </summary>
        internal string FlightEnvironmentVariable { get; }

        public static readonly ExperimentationConstants PackageManagerBackgroundColor = new("nuGetPackageManagerBackgroundColor", "NUGET_PACKAGE_MANAGER_BACKGROUND_COLOR");
    }
}
