# Azure Active Directory IdentityModel Extensions for .NET

[![Nuget](https://img.shields.io/nuget/v/Microsoft.IdentityModel.JsonWebTokens?label=Latest%20release)](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens/)

IdentityModel Extensions for .NET provide assemblies that are interesting for web developers that wish to use federated identity providers for establishing the caller's identity.

## Versions

You can find the release notes for each version [here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/releases). Older versions can be found [here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/Release-Notes).

## Note about 6.x

If you noticed, we bumped the release from 5.x to 6.x\
We were maintaining two releases from two different branches.\
dev - 6.x\
dev5x - 5.x

Internally at Microsoft we were quickly required to remove all 3rd party libraries as IdentityModel is all about securing resources.\
Since there were some breaking changes, given the time-line we had to maintain two releases.

Both of these branches were public and moved forward mostly in lock-step.\
Once we finished our SignedHttpRequest functionality in the 6.x branch, we realized the delta between 5.x aqnd 6.x was too large to maintain in both branches.\
We decided now was the time to switch to a single release branch.\
Since internally the versioning was at 6.4.2, we needed to release at 6.5.0.

## There are some small breaking changes

We built and tested ASP.NET core with 6.5.0 without issues.\
We also upgraded in place existing applications to 6.5.0 without issues.\
This of course does not mean you will not hit issues, but we took it seriously.

Any questions or compatibility problems please open issues [here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc).

## Thank you for using our product

The IdentityModel Team.

## Usage

IdentityModel Extensions for .NET 5 has now been released. If you are using IdentityModel Extensions with ASP.NET, the following combinations are supported:

* **IdentityModel Extensions for .NET 5.x**, **ASP.NET Core 1.x**, **ASP.NET Core 2.x**, **Katana 4.x**
* **IdentityModel Extensions for .NET 4.x**, **ASP.NET 4**, **Katana 3.x**
All other combinations aren't supported.

For more details see [Migration notes here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/Migrating-from-Katana-(OWIN)-3.x-to-4.x)

## Samples and Documentation

The scenarios supported by IdentityModel extensions for .NET are described in [Scenarios](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/scenarios). The libraries are in particular used part of ASP.NET security to validate tokens in ASP.NET Web Apps and Web APIs. To learn more about token validation, and find samples, see:

- [Azure Active Directory with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/azure-active-directory/?view=aspnetcore-2.1)
- [Developing ASP.NET Apps with Azure Active Directory](https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/developing-aspnet-apps-with-windows-azure-active-directory)
- [Validating tokens](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/ValidatingTokens)
- more generally, the library's [Wiki](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki)
- the [reference documentation](https://docs.microsoft.com/en-us/dotnet/api/overview/azure/activedirectory/client?view=azure-dotnet)

## Community Help and Support

We leverage [Stack Overflow](http://stackoverflow.com/) to work with the community on supporting Azure Active Directory and its SDKs, including this one! We highly recommend you ask your questions on Stack Overflow (we're all on there!) Also browse existing issues to see if someone has had your question before.

We recommend you use the "adal" tag so we can see it! Here is the latest Q&A on Stack Overflow for ADAL: [http://stackoverflow.com/questions/tagged/adal](http://stackoverflow.com/questions/tagged/adal)

## Security Reporting

See [SECURITY.md](./SECURITY.md)

## Security Vulnerability in Microsoft.IdentityModel.Tokens 5.1.0

IdentityModel Extensions library Microsoft.IdentityModel.Tokens has a known security vulnerability affecting version 5.1.0. Please update to >= 5.1.1 immediately. An updated package is available on NuGet. For more details, see the [security notice](./SECURITY_NOTICE.md).

## Contributing

All code is licensed under the MIT license and we triage actively on GitHub. We enthusiastically welcome contributions and feedback. See [Contributing.md](./Contributing.md) for guidelines, branch information, build instructions, and legalese.

## License

Copyright (c) Microsoft Corporation.  All rights reserved. Licensed under the MIT License (the "License");

## We Value and Adhere to the Microsoft Open Source Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
