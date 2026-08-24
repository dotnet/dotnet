# Microsoft.IdentityModel Support Policy

_Last updated May 8, 2025_

## Supported versions
The following table lists IdentityModel versions currently supported and receiving security fixes.

| Major Version | Last Release | Patch Release Date  | Support Phase|End of Support |
| --------------|--------------|--------|------------|--------|
| 8.x           | [![Nuget](https://img.shields.io/nuget/v/Microsoft.IdentityModel.JsonWebTokens?label=Latest%20release)](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens/)   |Monthly| Active | Tied to .NET 9 (STS) & 10 (LTS) ~ Nov, 2028.<br/>✅Supported versions: from 8.0.1 to [![Nuget](https://img.shields.io/nuget/v/Microsoft.IdentityModel.JsonWebTokens?label=Latest%20release)](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens/).<br/>⚠️Unsupported versions `< 8.0.1`.|
| 7.x           | 7.7.1        | July 19, 2024 |Active, security fixes only |Supported (LTS) through .NET 8 LTS lifetime Nov 10, 2026.<br/>✅Supported version: `7.7.1`<br/>⚠️Unsupported versions `< 7.7.1`.|
| 5.x           | 5.7.0        |January 9, 2024| Active, security fixes only |Tied to Microsoft.Owin.Security.JWT 4.2.2.<br/>✅Supported version: `5.7.0`<br/>⚠️Unsupported versions `< 5.7.0`. |

## Out of support versions
The following table lists IdentityModel versions no longer supported and no longer receiving security fixes.

| Major Version | Latest Patch Version| Patch Release Date | End of Support Date|
| --------------|--------------|--------|--------|
| 7.x           |    7.7.0      |  July 18, 2024      | July 18, 2024 |
| 6.x           |    6.36.0     | July 18, 2024       | May 2024|
| 5.x           |    5.6.0      | October 18, 2019    | October 18, 2019|
| 1.x           |    1.1.5      | November 17, 2017   | November 18, 2017|

## Overview

Every Microsoft product has a lifecycle. The lifecycle begins when a product is released and ends when it's no longer supported. Knowing key dates in this lifecycle helps you make informed decisions about when to upgrade or make other changes to your software. This product is governed by [Microsoft's Modern Lifecycle Policy](https://learn.microsoft.com/en-us/lifecycle/policies/modern).

The Microsoft suite of auth libraries provides comprehensive tools for identity and security token processing in .NET, and non-.NET, applications, including authentication, authorization, token validation, and integration with Entra ID and other IdPs. To provide clarity and predictability for developers, these libraries follow a Long-Term Support (LTS) policy similar in style to the .NET Core/.NET platform LTS story. This policy defines how long each major version of each library is supported, which versions receive updates (especially security fixes), and when older versions are deprecated. The goal is to ensure developers know which version is safe to use and when to upgrade, in alignment with .NET’s own support cadence.

## Support Policy Guiding Principles
The support policy can be summarized by three key rules:
1. **“Last Major Release” Support Window:** For each major version of the library (v5, v6, v7, v8, etc.), only the latest patch release of that major version is officially supported once a new major version is released. This last release of a major version (for example, 7.7.1 for the 7.x branch) will continue to be supported for a grace period of 180 days after the next major (v8.0) comes out or for the entire lifespan of the .NET LTS release that the library is associated with – whichever is longer. In other words, if a given major version of IdentityModel ships as part of a .NET LTS wave, it inherits that longer support timeline. For example, IdentityModel 7.x is shipped as part of ASP.NET Core in .NET 8 (an LTS release), then IdentityModel 7.x will be supported throughout the supported lifetime of .NET 8. If a major is not tied to an LTS .NET, the default support overlap is 180 days.
2. **Deprecation of Older Versions on New Major Release:** When a new major version of the library is released (e.g., 8.0.0), all previous minor/patch versions of the previous major (e.g., 7.0.0 up to 7.7.0) are immediately considered deprecated, only the last patch release of the previous major (e.g., 7.7.1) remains supported during the 180-day overlap or LTS period as described above. Earlier patches in that branch will no longer receive updates. For example, once 8.0.0 is released, the entire 7.x series before 7.7.1 is deprecated. Developers should move to 7.7.1 (the final 7.x release) or upgrade to 8.x for continued support.
3. **Security Fixes Only in Supported Versions:** Security fixes and critical bug fixes will be provided only for the supported versions – namely, the latest patch of the latest major, and in some cases the latest patch of the previous major during the overlap window. Older majors (and any old patch versions) will not receive security updates once they are out of support. This means if a vulnerability is discovered, the team will issue a fix in the current supported release (and possibly the last release of the previous major if still within 180-day/LTS overlap), but will not back-port fixes to earlier, deprecated patch versions. In practice, organizations must upgrade to the supported version to get the fix. (For example, a security advisory might instruct users to update to 7.7.1 or 8.x to resolve an issue, as older 7.x builds would not be patched.)

## Appendix

Do not use versions 7.0.0 - 7.4.0 due to a critical bug in JSON deserialization - see [the wiki](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/Critical-issues-affecting-version-7.0.0%E2%80%907.4.0) for details. The affected versions were marked as having a severe bug on nuget.org
