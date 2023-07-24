See the [releases](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/releases) for details on bug fixes and added features.

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
