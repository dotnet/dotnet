// Copyright(c) .NET Foundation.All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Configuration
{
    public enum SettingElementType
    {
        Unknown,

        Configuration,

        /** ---- Known sections --- **/

        ActivePackageSource,

        BindingRedirects,

        Config,

        PackageManagement,

        PackageRestore,

        PackageSourceCredentials,

        PackageSources,

        /** ---- Known items --- **/

        Add,

        Author,

        Certificate,

        Clear,

        Owners,

        Repository,

        FileCert,

        StoreCert,

        PackageSourceMapping,

        PackageSource,

        Package,

        AuditSources,
    }
}
