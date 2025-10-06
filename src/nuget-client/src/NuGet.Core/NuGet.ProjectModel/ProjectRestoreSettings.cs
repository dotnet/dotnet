// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Shared;
using NuGet.Versioning;

namespace NuGet.ProjectModel
{
    /// <summary>
    /// This class is used to hold restore related, project specific settings.
    /// </summary>
    public class ProjectRestoreSettings
    {
        /// <summary>
        /// Bool property is used inr estore command to not log errors and warning.
        /// Currently this is only being used for net core based projects on nomination.
        /// </summary>
        public bool HideWarningsAndErrors { get; set; } = false;

        /// <summary>
        /// Indicates the .NET SDK Version if any.
        /// In combination with SdkAnalysisLevel, it allows us to determine whether the analysis level is the default one or manually specified.
        /// </summary>
        public NuGetVersion SdkVersion { get; set; }

        public ProjectRestoreSettings Clone()
        {
            var clonedObject = new ProjectRestoreSettings();
            clonedObject.HideWarningsAndErrors = HideWarningsAndErrors;
            clonedObject.SdkVersion = SdkVersion;
            return clonedObject;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ProjectRestoreSettings);
        }

        public bool Equals(ProjectRestoreSettings other)
        {
            if (other == null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return HideWarningsAndErrors == other.HideWarningsAndErrors &&
                   EqualityUtility.EqualsWithNullCheck(SdkVersion, other.SdkVersion);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCodeCombiner();
            hashCode.AddObject(HideWarningsAndErrors);
            hashCode.AddObject(SdkVersion);
            return hashCode.CombinedHash;
        }
    }
}
