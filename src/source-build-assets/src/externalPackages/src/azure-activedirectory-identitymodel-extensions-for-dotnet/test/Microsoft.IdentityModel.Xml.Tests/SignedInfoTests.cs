// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Xml.Tests
{
    public class SignedInfoTests
    {

        [Fact]
        public void GetSets()
        {
            var type = typeof(SignedInfo);
            var properties = type.GetProperties();
            Assert.True(properties.Length == 5, $"Number of properties has changed from 5 to: {properties.Length}, adjust tests");
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("CanonicalizationMethod", new List<object>{SecurityAlgorithms.ExclusiveC14n, SecurityAlgorithms.ExclusiveC14nWithComments}),
                    new KeyValuePair<string, List<object>>("Id", new List<object>{(string)null, Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("Prefix", new List<object>{"", Guid.NewGuid().ToString()}),
                    new KeyValuePair<string, List<object>>("SignatureMethod", new List<object>{SecurityAlgorithms.RsaSha256Signature, Guid.NewGuid().ToString()})
                },
                Object = new SignedInfo(),
            };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors($"{this}.GetSets", context.Errors);
        }

        [Theory, MemberData(nameof(SignedInfoConstructorTheoryData), DisableDiscoveryEnumeration = true)]
        public void SignedInfoConstructor(SignedInfoTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.SignedInfoConstructor", theoryData);
            var context = new CompareContext($"{this}.SignedInfoConstructor : {theoryData}");
            try
            {
                var signedInfo = new SignedInfo();
                if (signedInfo.References == null)
                    context.Diffs.Add("signedInfo.References == null");

                if (signedInfo.References != null && signedInfo.References.Count != 0)
                    context.Diffs.Add("(signedInfo.References != null && signedInfo.References.Count != 0)");

                if (!string.Equals(signedInfo.SignatureMethod, SecurityAlgorithms.RsaSha256Signature))
                    context.Diffs.Add($"!string.Equals(signedInfo.SignatureMethod, SecurityAlgorithms.RsaSha256Signature) was: {signedInfo.SignatureMethod}");

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignedInfoTheoryData> SignedInfoConstructorTheoryData
        {
            get
            {
                return new TheoryData<SignedInfoTheoryData>
                {
                    new SignedInfoTheoryData
                    {
                        First = true,
                        TestId = "Constructor"
                    }
                };
            }
        }
    }

    public class SignedInfoTheoryData : TheoryDataBase
    {
        public DSigSerializer Serializer { get; set; } = new DSigSerializer();

        public SignedInfo SignedInfo { get; set; }

        public string Xml { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
