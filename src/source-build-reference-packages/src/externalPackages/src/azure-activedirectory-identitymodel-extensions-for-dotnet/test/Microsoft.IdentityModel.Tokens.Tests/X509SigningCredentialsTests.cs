// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
