// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.VisualStudio.Implementation.Extensibility
{
    internal class VsProjectJsonToPackageReferenceMigrateResult : IVsProjectJsonToPackageReferenceMigrateResult
    {
        public bool IsSuccess { get; }

        public string ErrorMessage { get; }

        public VsProjectJsonToPackageReferenceMigrateResult(bool success, string errorMessage)
        {
            IsSuccess = success;
            ErrorMessage = errorMessage;
        }
    }
}
