// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Utilities.UnifiedSettings;

namespace NuGet.PackageManagement.VisualStudio.Options
{
    internal static class ExternalSettingsUtility
    {
        public static ExternalSettingOperationResult<T> CreateSettingErrorResult<T>(string errorMessage)
        {
            var failure = new ExternalSettingOperationResult<T>.Failure(
                errorMessage,
                scope: ExternalSettingsErrorScope.SingleSettingOnly,
                isTransient: true);

            return failure;
        }
    }
}
