// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
