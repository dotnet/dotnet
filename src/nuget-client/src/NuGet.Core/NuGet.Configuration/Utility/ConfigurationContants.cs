// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGet.Configuration
{
    public static class ConfigurationConstants
    {
        public static readonly string ActivePackageSourceSectionName = "activePackageSource";

        public static readonly string Add = "add";

        public static readonly string AllowUntrustedRoot = "allowUntrustedRoot";

        public static readonly string AllowInsecureConnections = "allowInsecureConnections";

        public static readonly string ApiKeys = "apikeys";

        public static readonly string AuditSources = "auditSources";

        public static readonly string Author = "author";

        public static readonly string BeginIgnoreMarker = "NUGET: BEGIN LICENSE TEXT";

        public static readonly string BindingRedirectsSection = "bindingRedirects";

        public static readonly string Certificate = "certificate";

        public static readonly string Package = "package";

        public static readonly string Clear = "clear";

        public static readonly string ClearTextPasswordAttribute = "clearTextPassword";

        public static readonly string ClearTextPasswordToken = "ClearTextPassword";

        public static readonly string ClientCertificates = "clientCertificates";

        public static readonly string Config = "config";

        public static readonly string Configuration = "configuration";

        public static readonly string ConfigurationDefaultsFile = "NuGetDefaults.config";

        public static readonly string CredentialsSectionName = "packageSourceCredentials";

        public static readonly string DefaultPackageManagementFormatKey = "format";

        public static readonly string DefaultPushSource = "DefaultPushSource";

        public static readonly string DependencyVersion = "dependencyversion";

        public static readonly string DisabledPackageSources = "disabledPackageSources";

        public static readonly string DoNotShowPackageManagementSelectionKey = "disabled";

        public static readonly string Enabled = "enabled";

        public static readonly string EndIgnoreMarker = "NUGET: END LICENSE TEXT";

        public static readonly string FailOnBindingRedirects = "successRequired";

        public static readonly string FallbackPackageFolders = "fallbackPackageFolders";

        public static readonly string FileCertificate = "fileCert";

        public static readonly string FindByAttribute = "findBy";

        public static readonly string FindValueAttribute = "findValue";

        public static readonly string Fingerprint = "fingerprint";

        public static readonly string FingerprintAlgorithm = "fingerprintAlgorithm";

        public static readonly string UpdatePackageLastAccessTime = "updatePackageLastAccessTime";

        public static readonly string GlobalPackagesFolder = "globalPackagesFolder";

        public static readonly string HashAlgorithm = "hashAlgorithm";

        public static readonly string HostKey = "http_proxy";

        public static readonly string KeyAttribute = "key";

        public static readonly string MaxHttpRequestsPerSource = "maxHttpRequestsPerSource";

        public static readonly string NameAttribute = "name";

        public static readonly string NoProxy = "no_proxy";

        public static readonly string Owners = "owners";

        public static readonly string PackageManagementSection = "packageManagement";

        public static readonly string PackageRestore = "packageRestore";

        public static readonly string PackageSourceAttribute = "packageSource";

        public static readonly string PackageSources = "packageSources";

        public static readonly string PasswordAttribute = "password";

        public static readonly string PasswordKey = "http_proxy.password";

        public static readonly string PasswordToken = "Password";

        public static readonly string PathAttribute = "path";

        public static readonly string ProtocolVersionAttribute = "protocolVersion";

        public static readonly string Repository = "repository";

        public static readonly string RepositoryPath = "repositoryPath";

        public static readonly string ServiceIndex = "serviceIndex";

        public static readonly string SignatureValidationMode = "signatureValidationMode";

        public static readonly string SkipBindingRedirectsKey = "skip";

        public static readonly string StoreCertificate = "storeCert";

        public static readonly string StoreLocationAttribute = "storeLocation";

        public static readonly string StoreNameAttribute = "storeName";

        public static readonly string TrustedSigners = "trustedSigners";

        public static readonly string PackageSourceMapping = "packageSourceMapping";

        public static readonly string UserKey = "http_proxy.user";

        public static readonly string UsernameToken = "Username";

        public static readonly string ValidAuthenticationTypesToken = "ValidAuthenticationTypes";

        public static readonly string ValueAttribute = "value";

        public static readonly string PatternAttribute = "pattern";

        public static IReadOnlyList<string> GetConfigKeys()
        {
            return new List<string>
            {
                DependencyVersion,
                GlobalPackagesFolder,
                RepositoryPath,
                DefaultPushSource,
                HostKey,
                UserKey,
                PasswordKey,
                NoProxy,
                MaxHttpRequestsPerSource,
                SignatureValidationMode
            };
        }
    }
}
