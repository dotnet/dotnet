// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace NuGet.Commands
{
    public interface IClientCertArgsWithPackageSource
    {
        string PackageSource { get; set; }
    }

    public interface IClientCertArgsWithConfigFile
    {
        string Configfile { get; set; }
    }

    public interface IClientCertArgsWithForce
    {
        bool Force { get; set; }
    }

    public interface IClientCertArgsWithStoreData
    {
        string FindBy { get; set; }
        string FindValue { get; set; }
        string StoreLocation { get; set; }
        string StoreName { get; set; }
    }

    public interface IClientCertArgsWithFileData
    {
        string Password { get; set; }
        string Path { get; set; }
        bool StorePasswordInClearText { get; set; }
    }
}
