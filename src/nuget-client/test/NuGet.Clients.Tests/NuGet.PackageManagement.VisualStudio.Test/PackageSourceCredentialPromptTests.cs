// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Xunit;

namespace NuGet.PackageManagement.VisualStudio.Test
{
    public class PackageSourceCredentialPromptTests
    {
        [Fact]
        public void GetDisplayName_WithCredentialsAndQuery_DoesNotExposeSecrets()
        {
            var uri = new Uri("https://user:secret@unit.test/v3/index.json?token=secret");

            string source = PackageSourceCredentialPrompt.GetDisplayName(uri);

            Assert.Equal("https://unit.test/v3/index.json", source);
        }
    }
}
