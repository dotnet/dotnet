# IdentityModel Extensions for .NET

[![Nuget](https://img.shields.io/nuget/v/Microsoft.IdentityModel.JsonWebTokens?label=Latest%20release)](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens/)

IdentityModel Extensions for .NET provide assemblies that are interesting for web developers that wish to use federated identity providers for establishing the caller's identity.

## Versions

You can find the release notes for each version [here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/releases). Older versions can be found [here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/Release-Notes).

## IdentityModel 7x

We are excited to announce the release of [IdentityModel 7x](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/IdentityModel-7x), a major update to our popular .NET auth validation library. This new version introduces several improvements related to serialization and consistency in the API, which will provide a better user experience for developers, as well as full AOT compatibility on .NET, and huge perf improvements compared to 6x.

## Note about 6.x

We bumped the release from 6.x to 7.x.
We are maintaining two releases from two different branches.
dev - 7.x
dev6x - 6.x

dev6x will be maintained until March 2024, at which point, you will need to move to 7x to continue to get the latest and greatest improvements and security updates.

## Samples and Documentation

The scenarios supported by IdentityModel extensions for .NET are described in [Scenarios](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/scenarios). The libraries are in particular used part of ASP.NET security to validate tokens in ASP.NET Web Apps and Web APIs. To learn more about token validation, and find samples, see:

- [Microsoft Entra ID with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/azure-active-directory/?view=aspnetcore-2.1)
- [Developing ASP.NET Apps with Microsoft Entra ID](https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/developing-aspnet-apps-with-windows-azure-active-directory)
- [Validating tokens](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki/ValidatingTokens)
- more generally, the library's [Wiki](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/wiki)
- the [reference documentation](https://learn.microsoft.com/dotnet/api/microsoft.identitymodel.jsonwebtokens.jsonwebtokenhandler?view=msal-web-dotnet-latest)

## Community Help and Support

We leverage [Stack Overflow](http://stackoverflow.com/) to work with the community on supporting Microsoft Entra and its SDKs, including this one! We highly recommend you ask your questions on Stack Overflow (we're all on there!) Also browse existing issues to see if someone has had your question before.

We recommend you use the "identityModel" tag so we can see it! Here is the latest Q&A on Stack Overflow for IdentityModel: [https://stackoverflow.com/questions/tagged/identityModel](https://stackoverflow.com/questions/tagged/identityModel)

Have a design proposal? Please submit [a design proposal](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/new?assignees=&labels=design-proposal&projects=&template=design_proposal.md) before starting work on a PR to ensure it means the goals/objectives of this library and it's priorities.

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
