// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Microsoft.CodeAnalysis.Remote.Razor;

internal interface IInProcServiceFactory
{
    Task<object> CreateInProcAsync(IServiceProvider hostProvidedServices);
}
