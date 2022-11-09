// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Common
{
    /// <summary>
    /// This enum is used to quantify NuGet error and warning codes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Format - NUxyzw where NU is the prefix indicating NuGet and xyzw is a 4 digit code
    /// </para>
    ///
    /// Numbers - xyzw
    ///     x - 'x' is the largest digit and should be used to quantify a set of errors.
    ///         For example 1yzw are set of restore related errors and no other code path should use the range 1000 to 1999 for errors or warnings.
    ///
    ///     y - 'y' is the second largest digit and should be used for sub sections withing a broad category.
    ///
    ///         For example 12zw cvould be http related errors.
    ///         Further 'y' = 0-4 should be used for errors and 'y' = 5-9 should be warnings.
    ///
    ///     zw - 'zw' are the least two digit.
    ///         These could be used for different errors or warnings within the broad categories set by digits 'xy'.
    ///
    /// <para>
    /// Groups:
    /// 1000-1999 - Restore
    /// 3000-3999 - Signing
    /// 5000-5999 - Packaging
    /// </para>
    ///
    /// <para>
    /// Sub groups for Restore:
    /// error/warning - Reason
    /// 1000/1500     - Input
    /// 1100/1600     - Resolver
    /// 1200/1700     - Compat
    /// 1300/1800     - Feed
    /// 1400/1900     - Package
    /// </para>
    ///
    /// <para>
    /// All new codes need a corresponding MarkDown file under https://github.com/NuGet/docs.microsoft.com-nuget/tree/master/docs/reference/errors-and-warnings.
    /// </para>
    /// </remarks>
    public enum NuGetLogCode
    {
        /// <summary>
        /// Do not display the code.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Undefined error
        /// </summary>
        NU1000 = 1000,

        /// <summary>
        /// Project has zero target frameworks.
        /// </summary>
        NU1001 = 1001,

        /// <summary>
        /// Invalid combination with CLEAR
        /// </summary>
        NU1002 = 1002,

        /// <summary>
        /// Invalid combination of PTF and ATF
        /// </summary>
        NU1003 = 1003,

        /// <summary>
        /// Locked mode, but restore needs to update the lock file.
        /// </summary>
        NU1004 = 1004,

        /// <summary>
        /// Invalid combination of RestorePackagesWithLockFile and packages.lock.json file.
        /// </summary>
        NU1005 = 1005,

        /// <summary>
        /// NuGet configuration file has an invalid package source value.
        /// </summary>
        NU1006 = 1006,

        /// <summary>
        /// Project provided runtime graph is invalid. Either does not exist or cannot be parsed.
        /// </summary>
        NU1007 = 1007,

        /// <summary>
        /// Projects that use central package version management should not define the version on the PackageReference items but on the PackageVersion items.
        /// </summary>
        NU1008 = 1008,

        /// <summary>
        /// Projects that use central package version management should not define SDK implicit referenced packages. For more information, see https://aka.ms/sdkimplicitrefs
        /// </summary>
        NU1009 = 1009,

        /// <summary>
        /// The PackageReference items {0} do not have corresponding PackageVersions.
        /// </summary>
        NU1010 = 1010,

        /// <summary>
        /// Central floating versions are not allowed.
        /// </summary>
        NU1011 = 1011,

        /// <summary>
        /// Platform version not found.
        /// </summary>
        NU1012 = 1012,

        /// <summary>
        /// Projects that use central package version management are not configured to allow package version overrides.
        /// </summary>
        NU1013 = 1013,

        /// <summary>
        /// Unable to resolve package, generic message for unknown type constraints.
        /// </summary>
        NU1100 = 1100,

        /// <summary>
        /// No versions of the package exist on any of the sources.
        /// </summary>
        NU1101 = 1101,

        /// <summary>
        /// Versions of the package exist, but none are in the range.
        /// </summary>
        NU1102 = 1102,

        /// <summary>
        /// Range does not allow prerelease packages and only prerelease versions were found
        /// within the range.
        /// </summary>
        NU1103 = 1103,

        /// <summary>
        /// Project path does not exist on disk.
        /// </summary>
        NU1104 = 1104,

        /// <summary>
        /// Unable to read project information for 'ProjectFile'. The project file may be invalid or missing targets required for restore.
        /// </summary>
        NU1105 = 1105,

        /// <summary>
        /// Resolver conflict
        /// </summary>
        NU1106 = 1106,

        /// <summary>
        /// Version conflict.
        /// </summary>
        NU1107 = 1107,

        /// <summary>
        /// Circular dependency.
        /// </summary>
        NU1108 = 1108,

        /// <summary>
        /// Package dependency downgraded because of centrally defined package version.
        /// </summary>
        NU1109 = 1109,

        /// <summary>
        /// Dependency project has an incompatible framework.
        /// </summary>
        NU1201 = 1201,

        /// <summary>
        /// Dependency package does not contain assets for the current framework.
        /// </summary>
        NU1202 = 1202,

        /// <summary>
        /// un-matched reference assemblies
        /// </summary>
        NU1203 = 1203,

        /// <summary>
        /// Invalid package types
        /// </summary>
        NU1204 = 1204,

        /// <summary>
        /// Project has an invalid dependency count
        /// </summary>
        NU1211 = 1211,

        /// <summary>
        /// Incompatible tools package/project combination
        /// </summary>
        NU1212 = 1212,

        /// <summary>
        /// Incompatible package type
        /// </summary>
        NU1213 = 1213,

        /// <summary>
        /// Package Source is unreachable.
        /// </summary>
        NU1301 = 1301,

        /// <summary>
        /// Package MinClientVersion did not match.
        /// </summary>
        NU1401 = 1401,

        /// <summary>
        /// Package contains unsafe zip entry.
        /// </summary>
        NU1402 = 1402,

        /// <summary>
        /// Package sha512 validation failed.
        /// </summary>
        NU1403 = 1403,

        /// <summary>
        /// Package Signature is invalid
        /// </summary>
        NU1410 = 1410,

        /// <summary>
        /// Undefined warning
        /// </summary>
        NU1500 = 1500,

        /// <summary>
        /// Missing restore target.
        /// </summary>
        NU1501 = 1501,

        /// <summary>
        /// Unknown compatibility profile
        /// </summary>
        NU1502 = 1502,

        /// <summary>
        /// Skipping project that does not support restore.
        /// </summary>
        NU1503 = 1503,

        /// <summary>
        /// Duplicate PackageReference found
        /// </summary>
        NU1504 = 1504,

        /// <summary>
        /// Duplicate PackageDownload found
        /// </summary>
        NU1505 = 1505,

        /// <summary>
        /// Duplicate PackageVersion found
        /// </summary>
        NU1506 = 1506,

        /// <summary>
        /// Central package management is in use but there are multiple feeds configured without using package source mapping.
        /// </summary>
        NU1507 = 1507,

        /// <summary>
        /// Dependency bumped up
        /// </summary>
        NU1601 = 1601,

        /// <summary>
        /// Non-exact match on dependency range due to non inclusive minimum bound.
        /// </summary>
        NU1602 = 1602,

        /// <summary>
        /// Non-exact match on dependency range due to missing package version.
        /// </summary>
        NU1603 = 1603,

        /// <summary>
        /// Project dependency does not include a lower bound.
        /// </summary>
        NU1604 = 1604,

        /// <summary>
        /// Package dependency downgraded.
        /// </summary>
        NU1605 = 1605,

        // These codes have been moved and should not be reused.
        // NU1606 -> NU1108
        // NU1607 -> NU1107

        /// <summary>
        /// Version is higher than upper bound.
        /// </summary>
        NU1608 = 1608,

        /// <summary>
        /// Fallback framework used for a package reference.
        /// </summary>
        NU1701 = 1701,

        /// <summary>
        /// Fallback framework used for a project reference.
        /// </summary>
        NU1702 = 1702,

        /// <summary>
        /// MacCatalyst platform fell back to xamarin.ios - Added in 6.0, removed in 6.1.
        /// </summary>
        NU1703 = 1703,

        /// <summary>
        /// Feed error converted to a warning when ignoreFailedSources is true.
        /// </summary>
        NU1801 = 1801,

        /// <summary>
        /// Could not update package .nupkg.metadata timestamp
        /// </summary>
        NU1802 = 1802,

        /// <summary>
        /// HTTP Source specified, but HTTP sources will be deprecated.
        /// </summary>
        NU1803 = 1803,

        /// <summary>
        /// Undefined signature error
        /// </summary>
        NU3000 = 3000,

        /// <summary>
        /// Invalid input error
        /// </summary>
        NU3001 = 3001,

        /// <summary>
        /// The '-Timestamper' option was not provided. The signed package will not be timestamped.
        /// </summary>
        NU3002 = 3002,

        /// <summary>
        /// The package signature is invalid.
        /// </summary>
        NU3003 = 3003,

        /// <summary>
        /// The package is not signed.
        /// </summary>
        NU3004 = 3004,

        /// <summary>
        /// The package signature file entry is invalid.
        /// </summary>
        /// <remarks>
        /// Examples which would trigger this include:
        ///     * the entry has incorrect external file attributes
        ///     * the entry is compressed not stored
        ///     * the entry's compressed and uncompressed sizes differ
        /// </remarks>
        NU3005 = 3005,

        /// <summary>
        /// Signed Zip64 packages are not supported.
        /// </summary>
        NU3006 = 3006,

        /// <summary>
        /// The package signature format version is not supported.
        /// </summary>
        NU3007 = 3007,

        /// <summary>
        /// The package integrity check failed.
        /// </summary>
        NU3008 = 3008,

        /// <summary>
        /// The package signature file does not contain exactly one primary signature.
        /// </summary>
        NU3009 = 3009,

        /// <summary>
        /// The primary signature does not have a signing certificate.
        /// </summary>
        NU3010 = 3010,

        /// <summary>
        /// The primary signature is invalid.
        /// </summary>
        NU3011 = 3011,

        /// <summary>
        /// Primary signature validation failed.
        /// </summary>
        NU3012 = 3012,

        /// <summary>
        /// The signing certificate has an unsupported signature algorithm.
        /// </summary>
        NU3013 = 3013,

        /// <summary>
        /// The signing certificate does not meet a minimum public key length requirement.
        /// </summary>
        NU3014 = 3014,

        /// <summary>
        /// Certificates with lifetime signer EKU are not supported.
        /// </summary>
        NU3015 = 3015,

        /// <summary>
        /// The package hash uses an unsupported hash algorithm.
        /// </summary>
        NU3016 = 3016,

        /// <summary>
        /// The signing certificate is not yet valid.
        /// </summary>
        NU3017 = 3017,

        /// <summary>
        /// Chain building failed for primary signature
        /// </summary>
        NU3018 = 3018,

        /// <summary>
        /// The timestamp integrity check failed.
        /// </summary>
        NU3019 = 3019,

        /// <summary>
        /// The timestamp signature does not have a signing certificate.
        /// </summary>
        NU3020 = 3020,

        /// <summary>
        /// Timestamp signature validation failed.
        /// </summary>
        NU3021 = 3021,

        /// <summary>
        /// The timestamp certificate has an unsupported signature algorithm.
        /// </summary>
        NU3022 = 3022,

        /// <summary>
        /// The timestamp's certificate does not meet a minimum public key length requirement.
        /// </summary>
        NU3023 = 3023,

        /// <summary>
        /// The timestamp signature has an unsupported digest algorithm.
        /// </summary>
        NU3024 = 3024,

        /// <summary>
        /// The timestamp signing certificate is not yet valid.
        /// </summary>
        NU3025 = 3025,

        /// <summary>
        /// The timestamp response is invalid.  Nonces did not match.
        /// </summary>
        NU3026 = 3026,

        /// <summary>
        /// The primary signature should be timestamped to enable long-term signature validity after the certificate has expired.
        /// </summary>
        NU3027 = 3027,

        /// <summary>
        /// Chain building failed for timestamp
        /// </summary>
        NU3028 = 3028,

        /// <summary>
        /// The timestamp signature is invalid.
        /// </summary>
        NU3029 = 3029,

        /// <summary>
        /// The timestamp's message imprint uses an unsupported hash algorithm.
        /// </summary>
        NU3030 = 3030,

        /// <summary>
        /// The repository countersignature is invalid.
        /// </summary>
        NU3031 = 3031,

        /// <summary>
        /// The package signature contains multiple repository countersignatures.
        /// </summary>
        NU3032 = 3032,

        /// <summary>
        /// A repository primary signature must not have a repository countersignature.
        /// </summary>
        NU3033 = 3033,

        /// <summary>
        /// The package signature certificate does not match the trusted certificate list.
        /// </summary>
        NU3034 = 3034,

        /// <summary>
        /// Chain building failed for the repository countersignature.
        /// </summary>
        NU3035 = 3035,

        /// <summary>
        /// Timestamp Generalized time is outside certificate's valdity period
        /// </summary>
        NU3036 = 3036,

        /// <summary>
        /// The signature has expired.
        /// </summary>
        NU3037 = 3037,

        /// <summary>
        /// Verification settings require a repository countersignature, but the package does not have a repository countersignature.
        /// </summary>
        NU3038 = 3038,

        /// <summary>
        /// The package cannot be signed as it would require the Zip64 format.
        /// </summary>
        NU3039 = 3039,

        /// <summary>
        /// There where two equal certificate entries with conflicting attributes in the nuget.config
        /// </summary>
        NU3040 = 3040,

        /// <summary>
        /// Downloading a package from a plugin is not supported since unsigned packages are not allowed and package download plugins do not support signed package verification.
        /// </summary>
        NU3041 = 3041,

        /// <summary>
        /// Undefined Package Error.
        /// </summary>
        NU5000 = 5000,

        /// <summary>
        /// Error_WriteResolvedNuSpecOverwriteOriginal
        /// </summary>
        NU5001 = 5001,

        /// <summary>
        /// Error_InputFileNotSpecified
        /// </summary>
        NU5002 = 5002,

        /// <summary>
        /// Error_InvalidTargetFramework
        /// </summary>
        NU5003 = 5003,

        /// <summary>
        /// Error_PackageCommandNoFilesForLibPackage
        /// </summary>
        NU5004 = 5004,

        /// <summary>
        /// Error_PackageCommandNoFilesForSymbolsPackage
        /// </summary>
        NU5005 = 5005,

        /// <summary>
        /// Error_UnableToLocateBuildOutput
        /// </summary>
        NU5007 = 5007,

        /// <summary>
        /// ErrorManifestFileNotFound
        /// </summary>
        NU5008 = 5008,

        /// <summary>
        /// Error_CannotFindMsbuild
        /// </summary>
        NU5009 = 5009,

        /// <summary>
        /// Error_InvalidVersionInPackage
        /// </summary>
        NU5010 = 5010,

        /// <summary>
        /// Error_UnableToExtractAssemblyMetadata
        /// </summary>
        NU5011 = 5011,

        /// <summary>
        /// Error_UnableToFindBuildOutput
        /// </summary>
        NU5012 = 5012,

        /// <summary>
        /// Error_FailedToBuildProject
        /// </summary>
        NU5013 = 5013,

        /// <summary>
        /// Error_ProcessingNuspecFile
        /// </summary>
        NU5014 = 5014,

        /// <summary>
        /// Error_MultipleTargetFrameworks
        /// </summary>
        NU5015 = 5015,

        /// <summary>
        /// Error_InvalidDependencyVersionConstraints
        /// </summary>
        NU5016 = 5016,

        /// <summary>
        /// Error_CannotCreateEmptyPackage
        /// </summary>
        NU5017 = 5017,

        /// <summary>
        /// Error_Manifest_InvalidReference
        /// </summary>
        NU5018 = 5018,

        /// <summary>
        /// Error_PackageAuthoring_FileNotFound
        /// </summary>
        NU5019 = 5019,

        /// <summary>
        /// Error_EmptySourceFilePath
        /// </summary>
        NU5020 = 5020,

        /// <summary>
        /// Error_EmptySourceFileProjectDirectory
        /// </summary>
        NU5021 = 5021,

        /// <summary>
        /// Error_InvalidMinClientVersion
        /// </summary>
        NU5022 = 5022,

        /// <summary>
        /// Error_AssetsFileNotFound
        /// </summary>
        NU5023 = 5023,

        /// <summary>
        /// Error_InvalidPackageVersion
        /// </summary>
        NU5024 = 5024,

        /// <summary>
        /// Error_AssetsFileDoesNotHaveValidPackageSpec
        /// </summary>
        NU5025 = 5025,

        /// <summary>
        /// Error_FileNotFound
        /// </summary>
        NU5026 = 5026,

        /// <summary>
        /// Error_InvalidTargetFramework
        /// </summary>
        NU5027 = 5027,

        /// <summary>
        /// Error_NoPackItemProvided
        /// </summary>
        NU5028 = 5028,

        /// <summary>
        /// Error_InvalidNuspecProperties
        /// </summary>
        NU5029 = 5029,

        /// <summary>
        /// Error_Manifest_LicenseFileNotInNupkg
        /// </summary>
        NU5030 = 5030,

        /// <summary>
        /// Error_Manifest_LicenseFileExtensionIsInvalid
        /// </summary>
        NU5031 = 5031,

        /// <summary>
        /// Error_Manifest_LicenseExpressionIsUnparseable
        /// </summary>
        NU5032 = 5032,

        /// <summary>
        /// Error_Manifest_InvalidLicenseMetadata
        /// </summary>
        NU5033 = 5033,

        /// <summary>
        /// Error_Manifest_InvalidLicenseExpressionVersion
        /// </summary>
        NU5034 = 5034,

        /// <summary>
        /// Error_Manifest_LicenseUrlCannotBeUsedWithLicenses
        /// </summary>
        NU5035 = 5035,

        /// <summary>
        /// https://aka.ms/malformedNuGetLicenseUrl
        /// </summary>
        NU5036 = 5036,

        /// <summary>
        /// Error_MissingNuspecFile
        /// </summary>
        NU5037 = 5037,

        /// <summary>
        /// Error_ReadmeFileExtensionIsInvalid
        /// </summary>
        NU5038 = 5038,

        /// <summary>
        /// Error_ReadmeNoFileElement
        /// </summary>
        NU5039 = 5039,

        /// <summary>
        /// Error_ReadmeErrorEmpty
        /// </summary>
        NU5040 = 5040,

        /// <summary>
        /// Error_ReadmeCannotOpenFile
        /// </summary>
        NU5041 = 5041,

        /// <summary>
        /// Error_ProjectJsonPack_Deprecated_And_Disabled
        /// </summary>
        NU5042 = 5042,

        /// <summary>
        /// Invalid icon extension error
        /// </summary>
        NU5045 = 5045,

        /// <summary>
        /// Error_Manifest_IconCannotOpenFile
        /// </summary>
        NU5046 = 5046,

        /// <summary>
        /// Error_Icon_InvalidSize
        /// </summary>
        NU5047 = 5047,

        /// <summary>
        /// IconUrlDeprecationWarning
        /// </summary>
        NU5048 = 5048,

        /// <summary>
        /// Packing an SDK-based project with NuGet.exe error
        /// </summary>
        NU5049 = 5049,

        /// <summary>
        /// Attempted to write files from multiple sources into the same location
        /// </summary>
        NU5050 = 5050,

        /// <summary>
        /// AssemblyOutsideLibWarning
        /// </summary>
        NU5100 = 5100,

        /// <summary>
        /// AssemblyDirectlyUnderLibWarning
        /// </summary>
        NU5101 = 5101,

        /// <summary>
        /// DefaultSpecValueWarning
        /// </summary>
        NU5102 = 5102,

        /// <summary>
        /// InvalidFrameworkWarning
        /// </summary>
        NU5103 = 5103,

        /// <summary>
        /// InvalidPrereleaseDependencyWarning
        /// </summary>
        NU5104 = 5104,

        /// <summary>
        /// LegacyVersionWarning
        /// </summary>
        NU5105 = 5105,

        /// <summary>
        /// WinRTObsoleteWarning
        /// </summary>
        NU5106 = 5106,

        /// <summary>
        /// MisplacedInitScriptWarning
        /// </summary>
        NU5107 = 5107,

        /// <summary>
        /// MisplacedTransformFileWarning
        /// </summary>
        NU5108 = 5108,

        /// <summary>
        /// PlaceholderFileInPackageWarning
        /// </summary>
        NU5109 = 5109,

        /// <summary>
        /// ScriptOutsideToolsWarning
        /// </summary>
        NU5110 = 5110,

        /// <summary>
        /// UnrecognizedScriptWarning
        /// </summary>
        NU5111 = 5111,

        /// <summary>
        /// UnspecifiedDependencyVersionWarning
        /// </summary>
        NU5112 = 5112,

        /// <summary>
        /// Warning_DuplicatePropertyKey
        /// </summary>
        NU5114 = 5114,

        /// <summary>
        /// Warning_UnspecifiedField
        /// </summary>
        NU5115 = 5115,

        /// <summary>
        /// Warning_FileDoesNotExist
        /// </summary>
        NU5116 = 5116,

        /// <summary>
        /// Warning_UnresolvedFilePath
        /// </summary>
        NU5117 = 5117,

        /// <summary>
        /// Warning_FileNotAddedToPackage
        /// </summary>
        NU5118 = 5118,

        /// <summary>
        /// Warning_FileExcludedByDefault
        /// </summary>
        NU5119 = 5119,

        /// <summary>
        /// Migrator_PackageHasInstallScript
        /// </summary>
        NU5120 = 5120,

        /// <summary>
        /// Migrator_PackageHasContentFolder
        /// </summary>
        NU5121 = 5121,

        /// <summary>
        /// Migrator_XdtTransformInPackage
        /// </summary>
        NU5122 = 5122,

        /// <summary>
        /// Warning_FilePathTooLong
        /// </summary>
        NU5123 = 5123,

        /// <summary>
        /// Warning_UnrecognizedLicenseIdentifier
        /// </summary>
        NU5124 = 5124,

        /// <summary>
        /// LicenseUrlDeprecated
        /// </summary>
        NU5125 = 5125,

        ///<summary>
        /// ProjectJsonPack_Deprecated
        /// </summary>
        NU5126 = 5126,

        /// <summary>
        /// No ref or lib folder in the package
        /// </summary>
        NU5127 = 5127,

        /// <summary>
        /// TFM dependencies in the lib or ref folder don't have exact matches in the nuspec
        /// </summary>
        NU5128 = 5128,

        /// <summary>
        /// No build files that follow the build convention ("package_id".props)
        /// </summary>
        NU5129 = 5129,

        /// <summary>
        /// TFM dependencies in the lib or ref folder has a compatible match, but not an exact match
        /// </summary>
        NU5130 = 5130,

        /// <summary>
        /// References in the nuspec don't match up with the ref folder in the package
        /// </summary>
        NU5131 = 5131,

        /// <summary>
        /// File last write timestamp is out of range of what the zip format supports warning
        /// </summary>
        NU5132 = 5132,

        /// <summary>
        /// NuGet.exe needs to be unblocked after downloading
        /// </summary>
        NU5133 = 5133,

        /// <summary>
        /// Undefined package warning
        /// </summary>
        NU5500 = 5500,

        /// <summary>
        /// InvalidUndottedFrameworkWarning
        /// </summary>
        NU5501 = 5501,
    }
}
