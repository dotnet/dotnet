// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace NuGet.PackageManagement.VisualStudio
{
    /// <summary>  
    /// Provides an ICredentialService that should condense all credential providers for VS
    /// </summary>
    public interface ICredentialServiceProvider
    {
        Task<NuGet.Configuration.ICredentialService> GetCredentialServiceAsync();
    }
}
