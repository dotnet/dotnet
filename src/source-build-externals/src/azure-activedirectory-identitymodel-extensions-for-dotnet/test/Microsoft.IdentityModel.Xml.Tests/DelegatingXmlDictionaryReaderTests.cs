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
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class DelegatingXmlDictionaryReaderTests
    {
        [Theory, MemberData(nameof(ReadXmlTheoryData))]
        public void ReadXml(DelegatingXmlDictionaryReaderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadXml", theoryData);
            try
            {
                var depth = theoryData.DelegatingReader.Depth;
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DelegatingXmlDictionaryReaderTheoryData> ReadXmlTheoryData
        {
            get
            {
                return new TheoryData<DelegatingXmlDictionaryReaderTheoryData>
                {
                    new DelegatingXmlDictionaryReaderTheoryData
                    {
                        DelegatingReader = new DelegatingXmlDictionaryReaderPublic(),
                        ExpectedException = ExpectedException.InvalidOperationException("IDX30027:"),
                        First = true,
                        TestId = "InnerReader-Null"
                    },
                    new DelegatingXmlDictionaryReaderTheoryData
                    {
                        DelegatingReader = new DelegatingXmlDictionaryReaderPublic
                        {
                            InnerReaderPublic = XmlUtilities.CreateDictionaryReader(Default.OuterXml)
                        },
                        TestId = "InnerReader-Set"
                    }
                };
            }
        }
    }

    public class DelegatingXmlDictionaryReaderPublic : DelegatingXmlDictionaryReader
    {
        public XmlDictionaryReader InnerReaderPublic
        {
            get => InnerReader;
            set => InnerReader = value;
        }
    }

    public class DelegatingXmlDictionaryReaderTheoryData : TheoryDataBase
    {
        public DelegatingXmlDictionaryReaderPublic DelegatingReader { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
