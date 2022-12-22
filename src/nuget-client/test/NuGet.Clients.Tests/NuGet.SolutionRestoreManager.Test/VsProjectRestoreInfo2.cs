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
    internal class VsProjectRestoreInfo2 : IVsProjectRestoreInfo2
    {
        public string BaseIntermediatePath { get; }

        public string MSBuildProjectExtensionsPath { get; }

        public string OriginalTargetFrameworks { get; set; }

        public IVsTargetFrameworks2 TargetFrameworks { get; }

        public IVsReferenceItems ToolReferences { get; set; }

        public VsProjectRestoreInfo2(
            string baseIntermediatePath,
            IVsTargetFrameworks2 targetFrameworks)
        {
            if (string.IsNullOrEmpty(baseIntermediatePath))
            {
                throw new ArgumentException("Argument cannot be null or empty", nameof(baseIntermediatePath));
            }

            BaseIntermediatePath = baseIntermediatePath;
            TargetFrameworks = targetFrameworks ?? throw new ArgumentNullException(nameof(targetFrameworks));
        }
    }
}
