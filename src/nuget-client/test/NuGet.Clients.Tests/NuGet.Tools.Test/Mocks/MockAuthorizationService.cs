// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework.Services;

namespace NuGet.Tools.Test
{
    // This mock is necessary because Moq-ing value type parameters with default value fails.
    // https://github.com/dotnet/runtime/issues/24589
    internal sealed class MockAuthorizationService : IAuthorizationService
    {
        public event EventHandler CredentialsChanged { add { } remove { } }
        public event EventHandler AuthorizationChanged { add { } remove { } }

        public ValueTask<bool> CheckAuthorizationAsync(
            ProtectedOperation operation,
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<IReadOnlyDictionary<string, string>> GetCredentialsAsync(
            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
