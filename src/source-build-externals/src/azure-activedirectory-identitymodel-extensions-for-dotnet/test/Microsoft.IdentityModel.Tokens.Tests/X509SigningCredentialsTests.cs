//------------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class X509SigningCredentialsTests
    {
        [Fact]
        public void Constructors()
        {
            var context = TestUtilities.WriteHeader($"{this}", "Constructors", true);

            // public X509SigningCredentials(X509Certificate2 certificate)
            try
            {
                new X509SigningCredentials(null);
                TestUtilities.CheckForArgumentNull(context, "certificate", null);
            }
            catch (Exception ex)
            {
                TestUtilities.CheckForArgumentNull(context, "certificate", ex);
            }

            // public X509SigningCredentials(X509Certificate2 certificate, string algorithm)
            try
            {
                new X509SigningCredentials(Default.Certificate, null);
                TestUtilities.CheckForArgumentNull(context, "algorithm", null);
            }
            catch (Exception ex)
            {
                TestUtilities.CheckForArgumentNull(context, "algorithm", ex);
            }

            // public X509SigningCredentials(X509Certificate2 certificate, string algorithm)
            try
            {
                new X509SigningCredentials(Default.Certificate, string.Empty);
                TestUtilities.CheckForArgumentNull(context, "algorithm", null);
            }
            catch (Exception ex)
            {
                TestUtilities.CheckForArgumentNull(context, "algorithm", ex);
            }

            var cert = Default.Certificate;
            var signingCredentials = new X509SigningCredentials(cert);
            if (!object.ReferenceEquals(signingCredentials.Certificate, cert))
                context.Diffs.Add("!object.ReferenceEquals(signingCredentials.Certificate, cert)");

            if (!SecurityAlgorithms.RsaSha256.Equals(signingCredentials.Algorithm))
                context.Diffs.Add("!SecurityAlgorithms.RsaSha256.Equals(signingCredentials.Algorithm)");

            signingCredentials = new X509SigningCredentials(cert, SecurityAlgorithms.RsaSha384);
            if (!object.ReferenceEquals(signingCredentials.Certificate, cert))
                context.Diffs.Add("!object.ReferenceEquals(signingCredentials.Certificate, cert)");

            if (!SecurityAlgorithms.RsaSha384.Equals(signingCredentials.Algorithm))
                context.Diffs.Add("!SecurityAlgorithms.RsaSha256.Equals(signingCredentials.Algorithm)");

            if (signingCredentials.Digest != null)
                context.Diffs.Add("signingCredentials.Digest != null");

            TestUtilities.AssertFailIfErrors(context);
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
