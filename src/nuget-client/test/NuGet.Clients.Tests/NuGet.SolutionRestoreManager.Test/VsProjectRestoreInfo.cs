// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.SolutionRestoreManager.Test
{
    /// <summary>
    /// Root object containing project restore info.
    /// Implementation of <see cref="IVsProjectRestoreInfo"/> for internal consumption.
    /// Used solely by <see cref="ProjectRestoreInfoBuilder"/>.
    /// </summary>
    internal class VsProjectRestoreInfo : IVsProjectRestoreInfo
    {
        public string BaseIntermediatePath { get; }

        public string MSBuildProjectExtensionsPath { get; }

        public string OriginalTargetFrameworks { get; set; }

        public IVsTargetFrameworks TargetFrameworks { get; }

        public IVsReferenceItems ToolReferences { get; set; }

        public VsProjectRestoreInfo(
            string baseIntermediatePath,
            IVsTargetFrameworks targetFrameworks)
        {
            if (string.IsNullOrEmpty(baseIntermediatePath))
            {
                throw new ArgumentException("Argument cannot be null or empty", nameof(baseIntermediatePath));
            }

            if (targetFrameworks == null)
            {
                throw new ArgumentNullException(nameof(targetFrameworks));
            }

            BaseIntermediatePath = baseIntermediatePath;
            TargetFrameworks = targetFrameworks;
        }
    }
}
