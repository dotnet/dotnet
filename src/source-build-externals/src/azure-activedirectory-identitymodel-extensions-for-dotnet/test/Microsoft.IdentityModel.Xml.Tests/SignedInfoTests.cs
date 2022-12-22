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

        [Theory, MemberData(nameof(SignedInfoConstructorTheoryData))]
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
