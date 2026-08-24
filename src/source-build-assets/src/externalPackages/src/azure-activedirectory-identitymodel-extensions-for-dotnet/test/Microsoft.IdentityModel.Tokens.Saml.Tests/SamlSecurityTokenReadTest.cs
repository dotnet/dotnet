// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Saml.Tests
{
    public class SamlSecurityTokenReadTest
    {
        [Theory, MemberData(nameof(SamlReadFromTheoryData), DisableDiscoveryEnumeration = true)]
        public void SamlSecurityTokenReadFrom(SamlTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.SamlSecurityTokenReadFrom", theoryData);
            context.PropertiesToIgnoreWhenComparing.Add(typeof(SamlAssertion), new List<string> { "CanonicalString" });
            try
            {
                var sr = new StringReader(theoryData.SamlTokenTestSet.Xml);
                var reader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(sr));
                var assertion = theoryData.SamlSerializer.ReadAssertion(reader);
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(assertion, (theoryData.SamlTokenTestSet.SecurityToken as SamlSecurityToken).Assertion, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SamlTheoryData> SamlReadFromTheoryData
        {
            get
            {
                return new TheoryData<SamlTheoryData>
                {
                    new SamlTheoryData
                    {
                        First = true,
                        SamlTokenTestSet = ReferenceSaml.SamlSecurityTokenValid,
                        SamlSerializer = new SamlSerializer(),
                        TestId = nameof(ReferenceSaml.SamlSecurityTokenValid)
                    }
                };
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
