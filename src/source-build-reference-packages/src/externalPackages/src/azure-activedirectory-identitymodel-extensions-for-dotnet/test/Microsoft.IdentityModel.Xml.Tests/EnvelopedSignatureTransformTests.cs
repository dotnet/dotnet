// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class EnvelopedSignatureTransformTests
    {
        [Fact]
        public void Constructor()
        {
            TestUtilities.WriteHeader($"{this}", "Constructor", true);
            var transform = new EnvelopedSignatureTransform();
        }
    }
}
