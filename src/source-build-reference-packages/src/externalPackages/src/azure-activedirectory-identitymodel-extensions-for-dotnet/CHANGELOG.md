See the [releases](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/releases) for details on bug fixes and added features.

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
