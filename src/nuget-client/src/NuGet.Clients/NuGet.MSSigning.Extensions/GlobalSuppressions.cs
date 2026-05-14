// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'X509Certificate2 MSSignAbstract.GetCertificate(X509Certificate2Collection certCollection)', validate parameter 'certCollection' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.MSSigning.Extensions.MSSignAbstract.GetCertificate(System.Security.Cryptography.X509Certificates.X509Certificate2Collection)~System.Security.Cryptography.X509Certificates.X509Certificate2")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'HashAlgorithmName MSSignAbstract.ValidateAndParseHashAlgorithm(string value, string name, SigningSpecifications spec)', validate parameter 'spec' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.MSSigning.Extensions.MSSignAbstract.ValidateAndParseHashAlgorithm(System.String,System.String,NuGet.Packaging.Signing.SigningSpecifications)~NuGet.Common.HashAlgorithmName")]
[assembly: SuppressMessage("Build", "CA1822:Member ValidateAndParseHashAlgorithm does not access instance data and can be marked as static (Shared in VisualBasic)", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.MSSigning.Extensions.MSSignAbstract.ValidateAndParseHashAlgorithm(System.String,System.String,NuGet.Packaging.Signing.SigningSpecifications)~NuGet.Common.HashAlgorithmName")]
[assembly: SuppressMessage("Build", "CA1062:In externally visible method 'void MSSignAbstract.WarnIfNoTimestamper(ILogger logger)', validate parameter 'logger' is non-null before using it. If appropriate, throw an ArgumentNullException when the argument is null or add a Code Contract precondition asserting non-null argument.", Justification = "<Pending>", Scope = "member", Target = "~M:NuGet.MSSigning.Extensions.MSSignAbstract.WarnIfNoTimestamper(NuGet.Common.ILogger)")]
[assembly: SuppressMessage("Build", "CA2227:Change 'PackageOwners' to be read-only by removing the property setter.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.MSSigning.Extensions.RepoSignCommand.PackageOwners")]
[assembly: SuppressMessage("Build", "CA1056:Change the type of property RepoSignCommand.V3ServiceIndexUrl from string to System.Uri.", Justification = "<Pending>", Scope = "member", Target = "~P:NuGet.MSSigning.Extensions.RepoSignCommand.V3ServiceIndexUrl")]
