See the [releases](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/releases) for details on bug fixes and added features.

7.0.0
======
See [IdentityModel7x](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/IdentityModel-7x-breaking-changes) for the updates on this much anticipated release.

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
- Perf testing using brenchmark dotnet and crank, similar to aspnetcore, to better gauge requests per second perf impacts. See issue [#2232](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/2232) for details.
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

- Reading and writing JsonWebKey and JsonWebKeySet types now use System.Text.Json.Utf8JsonReaders/Writers for serialization. Seee PR [@2208](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/pull/2208) for details.

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

First round of triming analysis preperation for AOT
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
