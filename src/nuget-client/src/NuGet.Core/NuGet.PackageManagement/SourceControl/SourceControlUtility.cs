// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using NuGet.Configuration;

namespace NuGet.ProjectManagement
{
    public static class SourceControlUtility
    {
        private const string SolutionSection = "solution";
        private const string DisableSourceControlIntegerationKey = "disableSourceControlIntegration";

        public static bool IsSourceControlDisabled(ISettings settings)
        {
            var value = SettingsUtility.GetValueForAddItem(settings, SolutionSection, DisableSourceControlIntegerationKey);
            return !string.IsNullOrEmpty(value) && bool.TryParse(value, out var disableSourceControlIntegration) && disableSourceControlIntegration;
        }

        public static void DisableSourceControlMode(ISettings settings)
        {
            settings.AddOrUpdate(SolutionSection, new AddItem(DisableSourceControlIntegerationKey, "true"));
            settings.SaveToDisk();
        }

        public static SourceControlManager GetSourceControlManager(INuGetProjectContext nuGetProjectContext)
        {
            if (nuGetProjectContext != null)
            {
                var sourceControlManagerProvider = nuGetProjectContext.SourceControlManagerProvider;
                if (sourceControlManagerProvider != null)
                {
                    return sourceControlManagerProvider.GetSourceControlManager();
                }
            }

            return null;
        }

        public static bool IsPackagesFolderBoundToSourceControl(INuGetProjectContext nuGetProjectContext)
        {
            var sourceControlManager = GetSourceControlManager(nuGetProjectContext);
            if (sourceControlManager != null)
            {
                return sourceControlManager.IsPackagesFolderBoundToSourceControl();
            }

            return false;
        }
    }
}
