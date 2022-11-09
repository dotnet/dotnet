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
using System.IO;
using System.Runtime.Serialization;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.SignedHttpRequest.Tests
{
    public class SignedHttpRequestExceptionTests
    {
        [Theory, MemberData(nameof(ExceptionTypes))]
        public void SerializeAndDeserialzeExceptions(SignedHttpExceptionTypeTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.{nameof(SerializeAndDeserialzeExceptions)}", theoryData);

            try
            {
                var exception = Activator.CreateInstance(theoryData.ExceptionType);
                var serializer = new DataContractSerializer(theoryData.ExceptionType);
                var memoryStream = new MemoryStream();

                // Validate that each exception is able to be both serialized and deserialized. If the exception is not able to be,
                // an exception will be raised by the serializer object.
                serializer.WriteObject(memoryStream, exception);
                memoryStream.Seek(0, SeekOrigin.Begin);
                _ = serializer.ReadObject(memoryStream);

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<SignedHttpExceptionTypeTheoryData> ExceptionTypes
        {
            get
            {
                return new TheoryData<SignedHttpExceptionTypeTheoryData>
                {
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId ="SerializeDeserializeSignedHttpRequestCreationException",
                        ExceptionType = typeof(SignedHttpRequestCreationException),
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidAtClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidAtClaimException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidBClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidBClaimException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidCnfClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidCnfClaimException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidHClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidHClaimException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidMClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidMClaimException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidPClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidPClaimException),
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidPopKeyException",
                        ExceptionType = typeof(SignedHttpRequestInvalidPopKeyException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidQClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidQClaimException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidSignatureException",
                        ExceptionType = typeof(SignedHttpRequestInvalidSignatureException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidTsClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidTsClaimException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestInvalidUClaimException",
                        ExceptionType = typeof(SignedHttpRequestInvalidUClaimException) ,
                    },
                    new SignedHttpExceptionTypeTheoryData
                    {
                        TestId = "SerializeDeserializeSignedHttpRequestValidationException",
                        ExceptionType = typeof(SignedHttpRequestValidationException) ,
                    }
                };
            }
        }
    }

    public class SignedHttpExceptionTypeTheoryData : TheoryDataBase
    {
        public Type ExceptionType { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
