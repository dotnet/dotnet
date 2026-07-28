See the [releases](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/releases) for details on bug fixes and added features.

8.15.0
====
## New Features
- **Add ECDsa support in `X509SecurityKey` and `JsonWebKeyConverter.ConvertFromX509SecurityKey`**  
  Extended `X509SecurityKey` and `JsonWebKeyConverter.ConvertFromX509SecurityKey` to support ECDSA keys.  
  See PR [#2377](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2377) for details.

## Bug Fixes
- **Sanitize logs to avoid leaking sensitive data**  
  Updated logging to sanitize sensitive values, reducing the risk of inadvertently exposing secrets or PII in logs.  
  See PR [#3316](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3316) for details.
- **Optimize log sanitization with `SearchValues`**  
  Improved the performance of the log sanitization logic introduced earlier by using `SearchValues`, making sanitization more efficient in high-throughput scenarios.  
  See PR [#3341](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3341) for details.
- **Update test for `IDX10400`**  
  Adjusted the `IDX10400` test to align with the current behavior and error messaging.  
  See PR [#3314](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3314) for details.

## Fundamentals
- **Add supported algorithm tests**  
  Added new tests to validate the set of supported cryptographic algorithms, increasing confidence in algorithm coverage and compatibility.  
  See PR [#3296](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3296) for details.
- **Migrate repository agent rules from `.clinerules` to `agents.md`**  
  Moved repository agent/AI-assist rules into markdown documentation to make them more visible and easier to maintain.  
  See PR [#3313](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3313) for details.
- **Migrate `Microsoft.IdentityModel.TestExtensions` from Newtonsoft.Json to System.Text.Json**  
  Updated `Microsoft.IdentityModel.TestExtensions` to use `System.Text.Json` instead of `Newtonsoft.Json`, aligning tests with the runtime serialization stack.  
  See PR [#3356](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3356) for details.
- **Disable code coverage comments**  
  Turned off automated code coverage comments on PRs to reduce noise while retaining coverage data elsewhere.  
  See PR [#3349](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3349) for details.
- **Fix CodeQL alerts**  
  Addressed CodeQL-reported issues to improve security posture and static analysis cleanliness.  
  See PR [#3364](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3364) for details.

### .NET 10 / SDK and tooling updates
- **Building with .NET 10 preview / RC 1**  
  Updated the repository to build and test against .NET 10.0 preview/RC1, ensuring early compatibility with the upcoming runtime.  
  See PRs [#3287](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3287), [#3357](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3357), and [#3358](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3358) for details.
- **Fix .NET 10 test execution consistency**  
  Ensured consistent use of the `TargetNetNext` parameter across build, test, and pack phases so .NET 10.0 tests execute reliably.  
  See PR [#3337](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3337) for details.
- **Update project files and workflows for .NET 10.0 compatibility**  
  Adjusted project files and CI workflows to correctly target and run on .NET 10.0, including test and pack scenarios.  
  See PR [#3363](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3363) for details.
- **Update .NET version to meet CG compliance**  
  Updated the .NET version references to be compliant with corporate governance (CG) requirements.  
  See PR [#3353](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3353) for details.
- **Update Coverlet collector and test SDK**  
  - Bumped `CoverletCollectorVersion` to 6.0.4.  
    See PR [#3333](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3333) for details.  
  - Upgraded `Microsoft.NET.Test.Sdk` to a newer version for improved test reliability and tooling support.  
    See PR [#3336](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3336) for details.
- **Update `runTests.ps1` to specify dotnet directory**  
  Updated `runTests.ps1` to accept an explicit dotnet directory, improving test execution robustness in environments with multiple SDK installations.  
  See PR [#3368](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3368) for details.
- **Adjust dotnetcore workflow targeting for .NET 10 SDK**  
  Iterated on the CI workflow configuration to correctly target the .NET 10 SDK:  
  - Temporarily removed targeting of the .NET 10 SDK in `dotnetcore.yml`.  
    See PR [#3335](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3335).  
  - Reverted that change to restore .NET 10 SDK targeting.  
    See PR [#3339](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3339) for details.

### Documentation
- **Update support policy documentation**  
  Refreshed `supportPolicy.md` to reflect the latest support policy for IdentityModel.  
  See PR [#3367](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3367) for details.
  
8.14.0
====
## Bug Fixes
- Switch back to use `ValidationResult` instead of `OperationResult` when validating a token in a new experimental validation flow. Additionally removed the dependency on Microsoft.IdentityModel.Abstractions. See [#3299](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3299) for details.

8.13.1
====
## Dependencies
Microsoft.IdentityModel now depends on Microsoft.Identity.Abstractions 9.3.0

## Bug Fixes
- Fixed a decompression failure happening for large JWE payloads. See [#3286](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3286) for details.

### Work related to redesign of IdentityModel's token validation logic [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711)
- Update the validation methods to return Microsoft.Identity.Abstractions.OperationResult. See [#3284](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3284) for details.

8.13.0
====
### Fundamentals
- `CaseSensitiveClaimsIdentity.SecurityToken` setter is now protected internal (was internal). See PR [#3278](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3278) for details.
- Update .NET SDK version to 9.0.108 used when building or running the code. See PR [#3274](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3274) for details.
- Update RsaSecurityKey.cs to replace the Pkcs1 padding by Pss from HasPrivateKey check. See [#3280](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3280) for details.

8.12.1
====
### Fundamentals
- Update .NET SDK version to 9.0.107 used when building or running the code. See [#3385](https://github.com/AzureAD/microsoft-identity-web/pull/3385) for details.
- To keep our experimental code separate from production code, all files associated with experimental features have been moved to the Experimental folders. See PR [#3261](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3261) for details.
- Experimental code leaked into TokenValidationResult from early prototypes. See PR [#3259](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3259) for details.

8.12.0
====
## New Features
- **Enhance ConfigurationManager with event handling**  
  Added event handling capabilities to the `ConfigurationManager`, enabling consumers to subscribe to configuration change events. This enhancement improves extensibility and allows more responsive applications. For details see [#3253](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3253)  

## Bug Fixes
- **Add expected Base64UrlEncoder.Decode overload for NET6 and 8**  
  Introduced the expected overload of `Base64UrlEncoder.Decode` for .NET 6 and 8, ensuring compatibility and preventing missing method issues on these frameworks.  
  For details see [#3249](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3249)  

## Fundamentals
- **Add AI assist rules**  
  Incorporated AI assist rules to enhance AI agents effectiveness.  
  For details see [#3255](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3255)  
- **Update PublicApiAnalyzers and BannedApiAnalyzers to 4.14.0**  
  Upgraded analyzer packages for improved diagnostics and code consistency (in particular delegates are added).  
  For details see [#3256](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3256)  
- **Move suppression of RS006 to csproj**  
  Centralized suppression of RS006 warnings in project files for easier management.  
  For details see [#3230](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3230)  

8.11.0
=====
## New Features:
- Microsoft.IdentityModel now exposes the AadIssuerValidator factory method publicly to enable caching functionality for AadIssuerValidator instances. See issue [#3245](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3245) for details.
- Added a new public async API: `JsonWebTokenHandler.DecryptTokenWithConfigurationAsync`, which decrypts a JWE token using keys from either `TokenValidationParameters` or, if not present, from configuration (such as via a ConfigurationManager). This enhancement improves developer experience by enabling asynchronous, cancellation-aware JWE decryption scenarios, aligning with modern .NET async patterns and making integration with external key/configuration sources more robust and observable. See PR [#3243](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3243) for details.

8.10.0
=====
## Bug Fixes
- Corrected casing of the Type attribute in SubjectConfirmationData. See [#3206](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3206).
- Removed Microsoft.Bcl.Memory dependency for pre-.NET 9.0 targets. See [#3220](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/33784b8ec0ff4300efad788535f90b3adc5bdfd1) to avoid build warnings with the other target frameworks
- Aligned Microsoft.Extensions.Logging.Abstractions version to 8.0.0 for .NET 9 to match other targets. See [#3226](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/793bd34e945eb8ba7528cf87c4a29a29c4704a65).

## Fundamentals
- Introduced Long-Term Support (LTS) policy. See [#3228](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/946bec18cdeb26b8133a04e8c056dd5f17588f89) and [#3232](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/14842aedb3d57c32b9ce784061cd9beccbdd1eb1).

8.9.0
=====
## Bug Fixes
- syncAfter has been updated to preserve UTC information, addressing a bug where GetConfigurationAsync does not refresh configuration in ConfigurationManager. See [#3213](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3213).
- Fixed a null reference issue in KeyInfo. See (#3203)[https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3203].

## New Features
- Introduced a new delegate for reading custom token payload values on JsonWebToken. See [#2981](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2981).
- Added an overload for ReadJsonWebToken to take a ReadOnlyMemory. See [#3205](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/48cdc0ca10e81cf9d6d7071801112d63189a44a7#diff-03e8a19f3b3aeb7f65b778b988b6bf792d118ca00b6dbda337526e8678e0ef7f). 

## Fundamentals
- Utilized IList to avoid enumerator allocation during audience validation. See [#3204](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3204).

8.8.0
=====
## New Features
- Adds the ability for the metadata refresh to be done as a blocking call, as per 8.0.1 behavior. This is done through the `Switch.Microsoft.IdentityModel.UpdateConfigAsBlocking` switch. If set, configuration calls will be blocking when metadata is updated, otherwise, if token arrive with a new signing keys, validation errors will be returned to the caller. See PR [#3193](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3193) for details.
- Identity.Model updates some log and error messages (IDX10214, IDX10215). If the information is needed for debugging purposes, it can be reverted via the `Switch.Microsoft.IdentityModel.DoNotScrubExceptions` AppContextSwitch. See PR [#3195](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3195) and https://aka.ms/identitymodel/app-context-switches for details.
- Change all plain object locks to `System.Thread.Lock` objects for .NET 9 or greater. See PRs [#3185](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3185) and [#3189](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3189) for details.

8.7.0
=====
## Bug Fixes
- Add back internal methods `IsRecoverableException` and `IsRecoverableExceptionType` whose signatures were changed in the previous version. See [#3181](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3181).

## New Features
- Make `Cnf` class public and move it to Microsoft.IdentityModel.Tokens package. See [#3165](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3165).
  
8.6.1
=====
## Bug fix
- Microsoft.IdentityModel now triggers a configuration refresh if token decryption fails. See issue [#3148](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3148) for details.
- Fix a bug in `JsonWebTokenHandler` where `JwtTokenDecryptionParameters`'s `Alg` and `Enc` were not set during token decryption, causing `IDX10611` and `IDX10619` errors to show null values in the messages. See issue [#3003](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3003) for details.

## Fundamentals
- For development, IdentityModel now has a global.json file to specify the .NET SDK version. See issue [#2995](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2995) for details.

8.6.0
=====
## New Features
- TokenValidationParameters has a new boolean property `TryAllDecryptionKeys` that let you choose whether to try all decrypt keys when no key matches the token decrypt key IDs. By default it's set to **true** (legacy behavior) but you can set it to false to avoid tyring all keys which is more performant. See [#3128](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3128)
- Promote KeyInfo.MatchesKey from internal to protected internal virtual to enable SAML extensibility (for CoreWcf). See [#3140](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3140)

## Fundamentals
* Update dependency on Microsoft.Extensions.Logging.Abstractions from 9.0.0 to 8.0.2 to avoid package downgrade in apps on .NET 9 using a netstandard2.0 library referencing logging.abstractions. See [3143](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3143)
* Add more tests for encrypted tokens. See [#3139](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3139)

8.5.0
=====
## Reverting previous breaking change
- The Configuration Manager has been reverted to version 8.3.1. The changes made in 8.4.0 assume the configuration manager is used as a singleton, which is similar to marking the type as disposable. We have since learned that [adding IDisposable is a breaking change](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-idisposable), so we are following [semver guidance](https://semver.org/#what-do-i-do-if-i-accidentally-release-a-backward-incompatible-change-as-a-minor-version) and reverting and releasing a minor version (8.5.0).
- Cherry-picked Changes: Included changes from PR [#3022](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3022) and [#3104](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3104).

8.4.0
=====
## New Features
- App context switch allows blocking or non-blocking calls for configuration. See PR [#3106](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3106) for details and issue [#3082](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3082) for details.
- IdentityModel now enables symmetric and asymmetric keys to be created publicly with JWK. See [#3094](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3094) for details.
- IdentityModel now allows specifying the HTTP protocol version and version policy. See [#2808](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2808) for details.

## Repair items
- Add request count and duration telemetry for configuration requests. See [#3022](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3022) for details.
- `KeyID` should be present in exception messages and is no longer PII. See [#3104](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3104) for details.

## Fundamentals
- Fix spelling issues in xml comments. See [#3117](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3117) for details.
- Fix comment coverage in PR builds. See [#3079](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3079) for details.

### Work related to redesign of IdentityModel's token validation logic [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711)
- See [#3056](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3056). [#3100](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3100), [#3017](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3107), and [#3111](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3111).
- Add internal virtual on TokenHandler. See [#3084](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3084) for details.

8.3.1
=====
## Bug Fixes
* Respect TVP.RequireAudience when set to false. See [#3055](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3055)
* For net4.6.2 select RSACng for PSS support. See [#3097](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3097)
* Fix package downgrade in consuming libraries. See[#3062](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3062)
* Fix integer overflow in `AuthenticationEncryptionProvider.cs`. See [#3063](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3063)

## Fundamentals
* Removed unused property on JsonWebToken ClaimsIdentity. See [#3071](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/3071) for details.
* Upgrade to C# 13. See [#2998](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2998)
* Use new Base64Url API. See [#22817](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2817)
* Add warning quality check. See [#3067](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3067)
* Update dotnet actions. see [#3074](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3074)
* Fix warnings. See [#3081](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3081)
* Test updates in JsonWebToken. See [#3080](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3080).

### Work related to redesign of IdentityModel's token validation logic [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711)
- [#3027](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3027), [#3028](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3028), [#3051](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3051), [#3054](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3054)

8.3.0
=====

## New features

### Work related to redesign of IdentityModel's token validation logic [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711)
* SAML and SAML2 new model validation: Token Replay. See [#2994](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2994)
* Extensibility tests: Token Type - JWT ([#3030](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3030)), Issuer - SAML and SAML2 ([#3026](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3026)), Algorithm and Signature - JWT, SAML and SAML2 ([#3034](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3034)), Token Replay - JWT, SAML and SAML2 ([#3032](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3032)),  Issuer signing key - JWT, SAML and SAML2 ([#3029](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/302))
* Avoid code duplication in extensibility testing. See [#3041](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3041)
* Extensibility Testing: Refactor. See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3011
* Remove duplicate code in extensibility tests. See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3044

## Bug fixes
* Fix bug with AadIssuerValidator. See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3042
* Fixed SignedHttpRequest flaky test. See [#3037](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3037)
 
## Fundamentals
* Install all .NET versions in pipeline to fix run tests task. See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3018
* Changelog for 8.2.1. See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3009
* Remove unnecessary AoT test project. See in https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3045
* Fix powershell script for nuget update. See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3046
* Update to next version. See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3010
* Disable Coverage PR comments. See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3048
* Updates GitHub Action to support long paths, See https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3049
* Stack parameters to improve reading of code. by @brentschmaltz in https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3031

## New Contributors
* @ssmelov made their first contribution in https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/3042

8.2.1
=====
### New features
- Update to use .NET 9 GA. See [2990](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2990).

### Bug fixes
- Remove dependency on Microsoft.Bcl.TimeProvider for .NET 8+ targets. See [2935](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2935).
- Update cgmanifest to align with the JSON schema. See [2969](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2969).

### Fundamentals
- Streamline token creation by using `SecurityTokenDescriptor`. See [2993](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2993).
- Prevent inlining to guarantee stack frames in test. See [2999](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2999).

### Work related to redesign of IdentityModel's token validation logic [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711)
- Simplify stack frame caching. See [2976](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2976).
- Implement new model for reading SAML and SAML2 tokens. See [2980](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2980).
- Implement new model for validating SAML signature. See [2950](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2950).
- Add tests for `IssuerExtensibility`. See [2987](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2987).
- Switch to new validation model for SAML and SAML2 issuer signing key. See [2965](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2965).
- Switch to new validation model for SAML and SAML2 algorithm. See [2984](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2984).

8.2.0
=====
### Fundamentals
- Update System.Text.Json to 8.0.5 CVE-2024-43485. See [2892](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2892).
- Using FixedTimeEquals in NETCore targets. See [2857](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2857).
- Updated .NET 9 to RC 2 [2898](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2898).
- Adds ability to create token without kid [2968](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2968)
- Enables code coverage in PRs [2946](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2946)
- Various test improvements:
- [#2953](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2953)
- [#2955](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2955)
- [#2951](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2951)
- [#2952](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2952)
- [#2947](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2947)

### Work related to redesign of IdentityModel's token validation logic [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711)
- Validates Audience for SAML2TokenHandler with New Model [2863](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2863)
- Improvements to AudienceValidation [2902](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2902)
- Added properties to ValidationResult [2923](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2923)
- Implements Audience and Lifetime validations in SamlSecurityTokenHandler [2925](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2925)
- Implements Issuer validation in SamlSecurityTokenHandler [2948](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2948)

8.1.2
=====
### Bug fixes
- CaseSensitiveClaimsIdentity.Clone() now returns a `CaseSensitiveClaimsIdentity` as expected. See [2879](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2879)
- Multiple unused and unusable (for the moment) public APIs were removed. These were introduced by mistake leaking from the work done on logging and exception handling. See [2888](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2888). No major version changed needed as these APIs were not usable per se.

### Fundamentals
- Enabled PublicApiAnalyzers to better understand and trace changes to the public API. See[2782](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2782)

8.1.1
=====
### Bug fixes
- Fix bug where ConfigurationManager was updating keys too frequently. See [2866](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2866) for details. 

8.1.0
=====
### Performance improvements
- Improves performance during issuer validation by replacing string comparison with span comparison. See PR [#2826](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2826).

### New features
- Add optional check to prevent using keys that are shared across multiple clouds. See issue [#2832](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2832) for details.

### Bug fixes
- JsonWebTokenHandler would only return unwrapped keys if there was no errors. This change is to align with the behavior in JwtSecurityTokenHandler, that is it returns the keys that were able to be unwrapped, and only throw if no keys were able to be unwrapped. See issue [#2695](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2695) for details.

### Fundamentals
- Fix flaky tests. See [#2793](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2793) for details.
- Update XUnit versoin and fix test warnings due to new XUnit analyzers. See PR [#2796](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2796) for details.
- Onhboard to code coverage in ADO. See PR [#2798](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2798).
- Use `IsTargetFrameworkCompatible(*)` so AOT is forward-compatible with .NET 9 and beyond. See PR [#2790](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2790) for details.
- Fix a merge conflict impacting dev. See PR [#2819](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2819).
- Defining the following attribute in multiple assemblies (.Tokens, .Logging) causes an internal error.
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]. See PR [#2820](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2820).
- Remove perl dependency. See PR [#2830](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2830).

### Work related to redesign of IdentityModel's token validation logic [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711)
- [#2794](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2794)
- [#2800](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2800)
- [#2810](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2810)
- [#2811](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2811)
- [#2816](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2816)
- [#2822](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2822)
- [#2815](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2815)
- [#2818](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2818)
- [#2813](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2813)
- [#2827](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2827)
- 
8.0.2
=====

### Security fundamentals
- Add `BannedApiAnalyzers` to prevent use of `ClaimsIdentity` constructors. See PR [#2778](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2778) for details.

### Bug fixes
- IdentityModel now allows the JWT payload to be an empty string. See issue [#2656](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2656) for details.
- Cache `UseRfcDefinitionOfEpkAndKid` switch. See PR [#2747](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2747) for details.
- Method was named `DoNotFailOnMissingTid` in 7x and `DontFailOnMissingTid` in 8x, adding the method for back compat. See issue [#2750](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2750) for details.
- Metadata is now updated on a background thread. See [#2780](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2780) for details.
- `JsonWebKeySet` stores the original string it was created with. See PR [#2755](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2755) for details.
- Restore AOT compatibility. See [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2711).
- Fix OpenIdConnect parsing bug. See [#2772](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2772) for details.
- Remove the lock on creating a `SignatureProvider`. See [#2788](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2788) for details.

### Fundamentals
- Test clean up [#2742](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2742).
- Use only FxCop in .NET framework targets [#2693](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2693).
- Add rule to add file headers automatically [#2748](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2748).
- Code analysis updates [#2746](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2746).
- Include README packages in NuGet [#2752](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2752).
- Update projects inside WilsonUnix solution [#2768](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2768).
- Code style enforced in build [#2603](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2603).
- CodeQL update [#2767](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2767).
- Update build pipeline to new one release build format [#2777](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2777).
- Update GitHub actions to `9.0.100-preview.7.24407.12` and add `<NoWarn>$(NoWarn);SYSLIB0057</NoWarn>` due to breaking changes in preview7. [#2786](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2786).

### Work relating to [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711)
- [#2725](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2725), [#2729](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2729), [#2753](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2753), [#2758](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2758), [#2759](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2759), [#2757](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2758), [#2759](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2757), [#2764](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2758), [#2759](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2764), [#2771](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2758), [#2759](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2759), and [#2779](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2779).

8.0.1
=====
### Bug fixes
- IdentityModel now resolves the public key to EPK. See issue [#1951](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1951) for details.
- Fix a race condition where `SignatureProvider` was disposed but still able to leverage the cache and `SignatureProvider` now disposes when compacting. See PR [#2682](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2682) for details.
- For JWE, `JsonWebTokenHandler.ValidateJWEAsync` now considers the decrypt keys in the configuration. See issue [#2737](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2737) for details.

### Performance improvement
- `AppContext.TryGetSwitch` [statically caches internally](https://source.dot.net/#System.Private.CoreLib/src/libraries/System.Private.CoreLib/src/System/AppContext.cs,0a1f341850c88646) but takes out a lock. 
.NET almost always [caches these values](https://github.com/dotnet/aspnetcore/blob/79f745dfd906db54916bf3da2430720eaeda6254/src/Servers/Kestrel/Core/src/KestrelServerOptions.cs#L34-L38). They're not expected to change while the process is running unlike normal config. IdentityModel now caches the value. See issue [#2722](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2722) for details.

8.0.0
=====
### CVE package updates
[CVE-2024-30105](https://github.com/advisories/GHSA-hh2w-p6rv-4g7w)
- See PR [#2707](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2707) for details.

### Breaking change:
[Full list](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/IdentityModel-8x) of breaking changes.
- A derived `ClaimsIdentity` where claim retrieval is case-sensitive. The current `ClaimsIdentity`, in .NET, retrieves claims in a case-insensitive manner which is different than querying the underlying `SecurityToken`. The new `CaseSensitiveClaimsIdentity` class provides consistent retrieval logic with `SecurityToken`. Fallback to previous behavior via an AppContext switch. See PR [#2700](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2700) for details.
- Make `CollectionUtilities.IsNullOrEmpty` internal. See issues [#2651](**https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2651) and [#1722](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1722) for details. 

### Overall improvements to the validation in IdentityModel:
- See design proposal [#2711](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2711) for details, all work internal for now. Please comment in the GitHub issue and provide feedback there.

### New Features:
- Allow users to provide a `Stream` to `Write` in `OIDCConfigurationSerializer`. See PR [#2698](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2698) for details.

### Bug fixes:
- Remove dependency on `AadIssuerValidator.GetTenantIdFromToken` in `ValidateIssuerSigningKey`, to only consider the `tid`. An AppContext switch enables fallbacking to the previous behavior, which should not be needed. See PR [#2680](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2680) for details.
- Continuation of #2637 and #2646. Add the metadata `authorization_details_types_supported` from [RFC 9396 - OAuth 2.0 Rich Authorization Requests](https://datatracker.ietf.org/doc/html/rfc9396) to `OpenIdConnectConfiguration`.
- The class `OpenIdConnectPrompt` now has the `create` prompt from [Initiating User Registration via OpenID Connect 1.0
](https://openid.net/specs/openid-connect-prompt-create-1_0.html)
-  The following grant types are now included in `OpenIdConnectGrantTypes`:  `urn:ietf:params:oauth:grant-type:saml2-bearer` from [RFC 7522 - Security Assertion Markup Language (SAML) 2.0 Profile for OAuth 2.0 Client Authentication and Authorization Grants](https://datatracker.ietf.org/doc/html/rfc7522), `urn:ietf:params:oauth:grant-type:jwt-bearer` from [RFC 7523 - JSON Web Token (JWT) Profile for OAuth 2.0 Client Authentication and Authorization Grants](https://datatracker.ietf.org/doc/html/rfc7523), `urn:ietf:params:oauth:grant-type:device_code` from [RFC 8628 - OAuth 2.0 Device Authorization Grant](https://datatracker.ietf.org/doc/html/rfc8628), `urn:ietf:params:oauth:grant-type:token-exchange` from [RFC 8693 - OAuth 2.0 Token Exchange](https://www.rfc-editor.org/rfc/rfc8693.html), `urn:openid:params:grant-type:ciba` from [OpenID Connect Client-Initiated Backchannel Authentication Flow - Core 1.0](https://openid.net/specs/openid-client-initiated-backchannel-authentication-core-1_0.html)
-  Serialize byte arrays as base64 strings in Json tokens. This was the behavior in 6.x releases. See issue [#2524](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2524) for details.
- When we added virtuals to abstract methods that threw in the base class, we then called those methods that were implemented in user derived classes. The user code would fault with a `NotImplementedException`. Now a message is returned that the user can act on to fix the issue. See issue [#1970](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1970).

### Fundamentals
- Remove code that was used in target frameworks that got removed. See PR [#2673](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2673) for details.
- Rename local variables for better readability. See PR [#2674](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2674) for details.
- Refactor XML comments for improved clarity. See PR [#2676](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2676), [#2677](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2677), [#2678](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2678), [#2689](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2689) and [#2703](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2703) for details.
- Fix flaky test. See issue [#2683](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2683) for details.
- Made `ConfigurationManager.GetConfigurationAsync` a virtual method. See PR [#2661](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2661)

8.0.0-preview1
=====
### Breaking changes:
- IdentityModel 8x no longer supports .net461, which has reached end of life and is no longer supported. See issue [#2544](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2544) for details.
- Two IdentityModel extension dlls `Microsoft.IdentityModel.KeyVaultExtensions` and `Microsoft.IdentityModel.ManagedKeyVaultSecurityKey` were using ADAL, which is no longer supported . The affected packages have been removed, as the replacement is to use [Microsoft.Identity.Web](https://github.com/AzureAD/microsoft-identity-web/wiki/Certificates). See issue [#2454](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2454) for details.
- `AppContext.SetSwitch` which were included in IdentityModel 7x, have been removed and are the default in IdentityModel 8x. The result is a more performant IdentityModel by default. See issue [#2629](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2629) and https://aka.ms/IdentityModel8x for details.

7.6.1
=====
### New Features:
- Added an Audiences member to the SecurityTokenDescriptor to make it easier to define multiple audiences in JWT and SAML tokens. Addresses issue [#1479](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1479) with PR [#2575](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2575)
- Add  missing metadata parameters to OpenIdConnectConfiguration. See issue [#2498](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2498) for details. 


### Bug Fixes:
- Fix over-reporting of `IDX14100`. See issue [#2058](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2058) and PR [#2618](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2618) for details.
- `JwtRegisteredClaimNames` now contains previously missing Standard OpenIdConnect claims. See issue [#1598](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1598) for details.

### Performance Improvements:
- No longer for every string claim, calling DateTime.TryParse on each value, whether it is expected to be a DateTime or not. See issue [#2615](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2615) for details.

7.6.0
=====
### New Features:
- Update `JsonWebToken` - extract and expose the method that reads the header/payload property values from the reader so it can be overridden in children classes to add any extra own logic. See issues [#2581](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2581), [#2583](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2583), and [#2495](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2495) for details.

### Bug Fixes:
- JWE header algorithm is now compliant to IANA document. See issue [#2089](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2089) for details.

### Performance Improvements:
- Reduce the number of internal array allocations that need to happen for each claim set, see PR [#2596](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2596).

### Fundamentals:
- Add an AOT compatibility check on each PR to ensure only AOT compatible code is checked-in. See PR [#2598](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2598).
- Update perl scrip for OneBranch build. See PR [#2602](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2602).
- Add langversion 12 to benchmark tests. See PR [#2601](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2601).
- Removed unused build.cmd file. See PR [#2605](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2605).
- Create CodeQL exclusions file. See PR [#2609](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2609).
- Fix variable usage in AOT script. See PR [#2610](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2610).
- Move `Microsoft.IdentityModel.Tokens` delegates to a new file. See PR [#2606](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2606)

7.5.2
=====
### Bug Fixes:
- Validate authentication tag length so a JWE with appended characters will not be considered a valid token. See issues [#2201](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2201), [#1641](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1641), PR [#2569](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2569), and [IDX10625 Wiki](IDX10625) for details. 

### Fundamentals:
- App Context Switches in Identity Model 7x are now documented [here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/App-Context-Switches-in-IdentityModel-7x).

### Performance Improvements:
- In .NET 6 or greater, use a temporary buffer to reduce intermediate allocation in `VerifyRsa`/`VerifyECDsa`. See PR [#2589](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2589) for more details.
- Reduce allocations in `ValidateSignature` by using a collection expression instead of `new List<SecurityKey> { key }`, to optimize for the single element case. See PR [#2586](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2586) for more details.
- Remove Task allocation in `AadIssuerValidator`. See PR [#2584](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2584) for more details.

7.5.1
=====
### Performance Improvements:
- Use Base64.DecodeFromUtf8InPlace for base64 decode that saves 12% on token read time. Note that [JsonWebToken](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/a0ffac3dd9d7178430b617bae0e8e24b2188cf6a/src/Microsoft.IdentityModel.JsonWebTokens/JsonWebToken.cs#L4) no longer throws ArgumentOutOfRangeException and ArgumentException exceptions. See PR [#2504](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2504).

### Fundamentals:
- Moved token lifetime validation logic to an [internal static class](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/blob/dev/src/Microsoft.IdentityModel.Tokens/ValidatorUtilities.cs). See PR [#2547](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2547).

### Bug Fix:
- Contribution from @martinb69 to fix correct parsing of `UserInfoEndpoint`. See issue [#2548](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2548) for details.

7.5.0
=====
### New features
- Supports the 1.1 version of the Microsoft Entra ID Endpoint [#2503](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2503)

7.4.1
======
### Bug Fixes:
- `SamlSecurityTokenHandler` and `Saml2SecurityTokenHandler` now can fetch configuration when validating SAML issuer and signature. See PR [#2412](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2412)
- `JsonWebToken.ReadToken` now correctly checks Dot3 index in JWE. See PR [#2501](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2501)

### Engineering Excellence:
- Remove reference to `Microsoft.IdentityModel.Logging` in `Microsoft.IdentityModel.Protocols`, which already depends on it via `Microsoft.IdentityModel.Tokens`. See PR [#2508](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2508)
- Adjust uppercase json serialization tests to fix an unreliable test method, add consistency to naming. See PR [#2512](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2512)
- Disable the 'restore' and 'build' steps of 'build and pack' in `build.sh`, improving speed. See PR [#2521](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2521)

7.4.0
======
### New Features:
- Introduced an injection point for external metadata management and adjusted the issuer Last Known Good (LKG) to maintain the state within the issuer validator. See PR [#2480](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2480).
- Made an internal virtual method public, enabling users to provide signature providers. See PR [#2497](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2497).

### Performance Improvements:
- Added a new JsonWebToken constructor that accepts Memory<char> for improved performance, along with enhancements to existing constructors. More information can be found in issue [#2487](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2487) and in PR [#2458](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2458).

### Fundamentals:
- Resolved the issue of duplicated log messages in the source code and made IDX10506 log message more specific. For more details, refer to PR [#2481](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2481).
- Enhanced Json serialization by ensuring the complete object is always read. This improvement can be found in PR [#2491](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2491).

### Engineering Excellence:
- Streamlined the build and release process by replacing the dependency on updateAssemblyInfo.ps1 with the Version property. Check out the details in PR [#2494](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2494).
- Excluded the packing of Benchmark and TestApp projects for a more efficient process. Details available in PR [#2496](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2496).

7.3.1
======
### Bug Fixes:
- Replace propertyName with `MetadataName` constant. See issue [#2471](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2471) for details.
- Fix 6x to 7x regression where mixed cases OIDC json was not correctly process. See [#2404](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2402) and [#2402](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2402) for details.

### Performance Improvements:
- Update the benchmark configuration. See issue [#2468](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2468).

### Documentation:
- Update comment for `azp` in `JsonWebToken`. See [#2475](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2475) for details.
- Link to breaking change announcement. See [#2478].
- Fix typo in log message. See [#2479].

7.3.0
======
### New Features:
Addition of the ClientCertificates property to the HttpRequestData class enables exposure of certificate collection involved in authenticating the client against the server and unlock support of new scenarios within the SDK. See PR [#2462](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2462) for details.

### Bug Fixes:
Fixed bug where x5c property is empty in JwtHeader after reading a JWT containing x5c in its header, issue [#2447](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2447), see PR [#2460](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2460) for details.
Fixed bug where JwtPayload.Claim.Value was not culture invariant [#2409](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2409). Fixed by PRs [#2453](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2453) and [#2461](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2461).
Fixed bug where Guid values in JwtPayload caused an exception, issue [#2439](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2439). Fixed by PR [#2440](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2440).

### Performance Improvements:
Remove linq from BaseConfigurationComparer, improvement [#2464](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2464), for additional details see PR [#2465](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2465).

### Engineering Excellence:
New benchmark tests for AsymmetricAdapter signatures. For details see PR [#2449](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2449).

7.2.0
======
### Performance Improvements:
Reduce allocations and transformations when creating a token [#2395](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2428).
Update Esrp Code Signing version to speed up release build [#2429](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2429).

### Engineering Excellence:
Improve benchmark consistency [#2428](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2428).
Adding P50, P90 and P100 percentiles to benchmarks [#2411](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2411).
Decouple benchmark tests from test projects [#2413](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2413).
Include pack step in PR builds [#2442](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2442).

### Fundamentals:
Improve logging in Wilson for failed token validation when key not found [#2436](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2436).
Remove conditional Net8.0 compilation [#2424](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2424).

7.1.2
======
### Security fixes:
See https://aka.ms/IdentityModel/Jan2024/zip and https://aka.ms/IdentityModel/Jan2024/jku for details.

7.0.3
======
### Bug Fixes:
- Fix errors like the following reported by multiple customers at dotnet/aspnetcore#51005 when they tried to upgrade their app using `AddMicrosoftIdentityWebApp` to .NET 8. See [PR](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2361) for details.
- Fix compatibility issue with 6x when claims are a bool. See issue [#2354](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2354) for details.

7.0.2
======
### Bug Fixes:
- Resolved an issue where JsonWebToken properties would throw exceptions when the input string was 'null'. See PR[#2335](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2335) for details.

7.0.1
======
### Bug Fixes:
- GetPayloadClaim("aud") returns a string when a single audience is specified, aligning with the behavior in 6.x. See PR[#2331](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2331) for details.

7.0.0
======
See [IdentityModel7x](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/IdentityModel-7x) for the updates on this much anticipated release.

7.0.0-preview5
=======
### Bug fixes:
- Improve log messages. See PR [#2289](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2289) for details.
- In `AadIssuerValidator` return a `ValueTask<string>` instead of a `Task<string>`. See Issue [#2286](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2286) and PR [https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2287] for details.
- Deprecate `int? JwtPayload.Exp`, `.Iat`, and `.Nbf`. See issue [#2266](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2266) for details, [#92](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/92), and [#1525](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/1525).
- General clean-up. See PR [#2285](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2285).

7.0.0-preview4
=======
### Bug fixes:
- Add nullables to the properties in `WsFederationMessage`. See issue [#2240](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2240) for details.
- Fix regression where `JsonWebToken.TryGetPayloadValue()` was not compatible with dictionary types. See issue [#2246](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2246) for details.
- Fix regression where dictionary claims added to `SecurityTokenDescriptor.Claims` are no longer correctly serialized. See issue [#2245](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2245) for details.
- Fix regression with a Y2038 bug. See issue [#2261](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2261) for details.
- Fix a regression where claims with multiple values are incorrectly serialized. See [#2244](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2244) for details.

## Performance improvements:
- Remove sync-over-async pattern with `JsonWebTokens.ValidateToken`, which when in the hot path can lead to threadpool starvation. See issue [#2253](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2253) for details.
- Perf testing using benchmark dotnet and crank, similar to aspnetcore, to better gauge requests per second perf impacts. See issue [#2232](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2232) for details.
- Use optimistic synchronization in `JsonWebToken.Audiences`. See [PR](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2243) for details.
- Reduce allocations when enumerating over collections. See [PR](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2242) for details.

## Documentation:
- Fix description for [JWT X5tS256 field](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2252).

## Fundamentals:
- Improvements to the build script to accommodate .NET's source-build requirements. See [PR](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2211) for details.

7.0.0-preview3
=======
## Performance improvements:
- Replace Newtonsoft.Json with System.Text.Json, see [#2233](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2233), and as a result, ASP.NET's JwtBearer auth handler will now be fully AOT compatible.

7.0.0-preview2
=======
## Performance improvements:
- Series of perf improvements in collaboration with ASP .NET Core DevDiv team, results in improvements from 280K Request per second (RPS) in `7.0.0-preview` to 370K RPS in `7.0.0-preview2`, with more improvements to come in later versions: [#2195](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2195), [#2194](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2194), [#2193](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2193), [#2192](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2192), [#2190](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2190), [#2188](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2188), [#2184](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2184), [#2181](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2181), [#2180](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2180), [#2178](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2178), [#2175](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2175), [#2172](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2172), [#2171](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2171), [#2170](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2170), [#2169](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2169), [#2168](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2168), [#2167](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2167), [#2166](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2166), [#2164](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2164), [#2162](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2162), [#2161](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2161), [#2160](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2160), [#2159](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2159), [#2158](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2158), [#2221](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2221)

- First increment in replacing newtonsoft with System.Text.Json, see [#2174](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2174)

- Reading and writing JsonWebKey and JsonWebKeySet types now use System.Text.Json.Utf8JsonReaders/Writers for serialization. See PR [@2208](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2208) for details.

- Remove the use of Newtonsoft from OpenIdConnectConfiguration and OpenIdConnectMessage. See PR [@2214](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2214) for details.


## Engineering excellence:

- Fix casing Properties directory in `updateAssemblyInfo.ps1` script see,
[#2189](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2189)

- Add code coverage in ADO, see [#2176](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2176)

- Add codeQL scanning for compliance, see [#2151](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2151)

- Start adding support for Nullables, see [#2139](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2139) and [#2203](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2203).

7.1.0-preview
=======
_Delisted from NuGet due to versioning inconsistency_
Include IdentityModel 6.32.0 release updates, including AAD specific signing key issuer validator and fix perf regression.

7.0.0-preview
=======
Join the 7x [discussion](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/discussions/2092) and provide your feedback!

Relevant PRs for supporting .NET 8:
[#2108](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2108)
[#2121](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2121)
[#2122](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2122)

Remove net45, see [#2123](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2123)

JwtSecurityTokenConverter, see [#2117](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2117)

6.32.3
=======
## Bug fixes:
- Fix logging messages. See [#2288](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2288) for details.

6.32.2
=======
## Bug fixes:
- Underlying JsonDocument is never disposed, causing high latency in large scale services. See [#2258](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2258) for details.

6.32.1
=======
## Bug fixes:
- Fix thread safety for `JsonClaimSet` Claims and `JsonWebToken` Audiences. See [#2185](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2185) for details.

6.32.0
=======
## New features:
- Adding an AAD specific signing key issuer validator. See issue [#2134](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2134) for details.
- Better support for WsFederation. See [PR](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2100) for details.

## Bug fixes
- Address perf regression introduced in 6.31.0. See [PR](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2131) for details.

6.31.0
========
This release contains work from the following PRs and commits:

- Introduce ConfigurationValidationException(#2076)
- Disarm security artifacts(#2064)
- Throw SecurityTokenMalformedTokenException on malformed tokens(#2080)
- Add ClaimsMapping to [JsonWebTokenHandler](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/8e7f07e859629a850e375518fcce2b6057380721)

6.30.1
=========
This release contains work from the following PRs:
- Modified token validation to be async throughout the call graph #2075
- Enforce key sizes when creating HMAC #2072
- Fix AotCompatibilityTests #2066
- Use up-to-date "now", in case take long time to get Metadata #2063

This release addresses #1743 and, as such, going forward if the SymmetricKey is smaller than the required size for HMAC IdentityModel will throw an ArgumentOutOfRangeException which is the same exception when the SymmetricKey is smaller than the minimum key size for encryption.

6.30.0
=========
Beginning in release [6.28.0](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/releases/tag/6.28.0) the library stopped throwing SecurityTokenUnableToValidateException. This version (6.30.0) marks the exception type as obsolete to make this change more discoverable. Not including it in the release notes explicitly for 6.28.0 was a mistake. This exception type will be removed completely in the next few months as the team moves towards a major version bump. More information on how to replace the usage going forward can be found here: https://aka.ms/SecurityTokenUnableToValidateException

Indicate that a SecurityTokenDescriptor can create JWS or JWE
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2055
Specify 'UTC' in log messages
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/ceb10b10ad2edb97217e263915d407da1d957e03
Fix order of log messages
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/05eeeb513e66a4236ae519ef9304bf2b6f26766f

Fixed issues with matching Jwt.Kid with a X509SecurityKey.x5t
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2057
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2061

Marked Exception that is no longer used as obsolete
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2060

Added support for AesGcm on .NET 6.0 or higher
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/85fa86af743e2b1a0078a9ecd956f34ee703acfc

First round of trimming analysis preparation for AOT
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2042

Added new API on TokenHandler.ValidateTokenAsync(SecurityToken ...) implemented only on JsonWebTokenHandler.
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2056

6.29.0
=========
- Add BootstrapRefreshInterval (#2052)
- Added net462 target (#2049)
- Create the configuration cache in the BaseConfigurationManager class (#2048)

6.28.1
=========
- Add BootstrapRefreshInterval (#2052)
- Added net462 target (#2049)
- Create the configuration cache in the BaseConfigurationManager class (#2048)

6.28.0
========
* Update Wilson logs with aka.ms pointers to known wikis in https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2027
* Fix typo in documentation https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2034
* Introduce a LKG configuration cache to store each valid base configuration instead of a single entry of configuration https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2007
* Add encryption keys to base configuration https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2023
* Updated CHANGELOG link https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2026

6.27.0
========
Servicing release
Set maximum depth for Newtonsoft parsing.
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2024
Improve metadata failure message.
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2010
Validate size of symmetric signatures.
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2008
Added property TokenEndpoint to BaseConfiguration.
https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/1998

6.26.1
=========
### Bug Fixes:

Releasing a Hotfix for Wilson 6.26.0 that reverts async/await changes made in #1996 to address a performance reduction issue.
- Changes are in #2015
- Root cause analysis and fix will be tracked in #2017


Next release (6.22.1 or 6.23.0)
=========
### New Features:
Microsoft.IdentityModel has two assemblies to manipulate JWT tokens:

System.IdentityModel.Tokens.Jwt, which is the legacy assembly. It defines JwtSecurityTokenHandler class to manipulate JWT tokens.
Microsoft.IdentityModel.JsonWebTokens, which defines the JsonWebToken class and JsonWebTokenHandler, more modern, and more efficient.
When using JwtSecurityTokenHandler, the short named claims (oid, tid), used to be transformed into the long named claims (with a namespace). With JsonWebTokenHandler this is no longer the case, but when you migrate your application from using JwtSecurityTokenHandler to JsonWebTokenHandler (or use a framework that does), you will only get original claims sent by the IdP. This is more efficient, and occupies less space, but might trigger a lot of changes in your application. In order to make it easier for people to migrate without changing their app too much, this PR offers extensibility to re-add the claims mapping.

### Bug Fixes:


6.22.0
=========

### New Features:

**Unmasked non-PII properties in log messages -**
In Microsoft.IdentityModel logs, previously only system metadata (DateTime, class name, httpmethod etc.) was displayed in clear text. For all other log arguments, the type was being logged to prevent Personally Identifiable Information (PII) from being displayed when ShowPII flag is turned OFF. To improve troubleshooting experience non-PII properties - Issuer, Audience, Key location, Key Id (kid) and some SAML constants will now be displayed in clear text. See issue #1903 for more details.

**Prefix Wilson header message to the first log message -**
To always log the Wilson header (Version, DateTime, PII ON/OFF message), EventLogLevel.LogAlways was mapped to LogLevel.Critical in Microsoft.IdentityModel.LoggingExtensions.IdentityLoggerAdapter class which caused confusion on why header was being displayed as a fatal log.
To address this, header is now prefixed to the first message logged by Wilson and separated with a newline. EventLogLevel.LogAlways has been remapped to LogLevel.Trace. See issue #1907 for more details.

### Bug Fixes:

**[Copy the IssuerSigningKeyResolverUsingConfiguration delegate in Clone()](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/commit/652d5d8d7371c2882306d3e95fc6e0de21ac7411)** #1909
