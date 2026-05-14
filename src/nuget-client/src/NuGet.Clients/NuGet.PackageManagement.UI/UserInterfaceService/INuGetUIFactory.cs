// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using NuGet.VisualStudio.Internal.Contracts;

namespace NuGet.PackageManagement.UI
{
    public interface INuGetUIFactory
    {
        ValueTask<INuGetUI> CreateAsync(IServiceBroker serviceBroker, params IProjectContextInfo[] projects);
    }
}
