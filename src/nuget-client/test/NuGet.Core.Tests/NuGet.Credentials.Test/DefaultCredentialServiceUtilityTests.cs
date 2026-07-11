// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using Xunit;

namespace NuGet.Credentials.Test
{
    public class DefaultCredentialServiceUtilityTests
    {
        [Fact]
        public void ResetCredentialService_ClearsPinnedCredentialService()
        {
            Lazy<ICredentialService>? original = HttpHandlerResourceV3.CredentialService;
            try
            {
                HttpHandlerResourceV3.CredentialService = null;
                DefaultCredentialServiceUtility.SetupDefaultCredentialService(NullLogger.Instance, nonInteractive: true);
                Assert.NotNull(HttpHandlerResourceV3.CredentialService);

                DefaultCredentialServiceUtility.ResetCredentialService();

                // A reused process must rebuild the credential service for the next restore (with the current
                // interactivity, settings and providers) instead of pinning the first build's instance.
                Assert.Null(HttpHandlerResourceV3.CredentialService);
            }
            finally
            {
                HttpHandlerResourceV3.CredentialService = original;
            }
        }
    }
}
