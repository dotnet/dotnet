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

                if (!theoryData.First)
                {
                    Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.Depth, theoryData.DelegatingReader.Depth);
                    Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.Value, theoryData.DelegatingReader.Value);
                    Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.Name, theoryData.DelegatingReader.Name);
                    Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.NamespaceURI, theoryData.DelegatingReader.NamespaceURI);
                    Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.AttributeCount, theoryData.DelegatingReader.AttributeCount);
                }

                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        [Theory, MemberData(nameof(ReadPartialXmlWithUniqueIdTheoryData))]
        public void ReadPartialXml(DelegatingXmlDictionaryReaderTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadXml", theoryData);
            try
            {
                Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.Depth, theoryData.DelegatingReader.Depth);
                Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.Value, theoryData.DelegatingReader.Value);
                Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.Name, theoryData.DelegatingReader.Name);
                Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.NamespaceURI, theoryData.DelegatingReader.NamespaceURI);
                Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.AttributeCount, theoryData.DelegatingReader.AttributeCount);

                theoryData.DelegatingReader.InnerReaderPublic.MoveToContent();
                theoryData.DelegatingReader.MoveToContent();

                theoryData.DelegatingReader.InnerReaderPublic.MoveToAttribute("URI");
                theoryData.DelegatingReader.MoveToAttribute("URI");

                Assert.Equal(theoryData.DelegatingReader.InnerReaderPublic.ReadContentAsUniqueId(),
                    theoryData.DelegatingReader.ReadContentAsUniqueId());

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

        public static TheoryData<DelegatingXmlDictionaryReaderTheoryData> ReadPartialXmlWithUniqueIdTheoryData
        {
            get
            {
                return new TheoryData<DelegatingXmlDictionaryReaderTheoryData>
                {
                    new DelegatingXmlDictionaryReaderTheoryData
                    {
                        DelegatingReader = new DelegatingXmlDictionaryReaderPublic
                        {
                            InnerReaderPublic = XmlUtilities.CreateDictionaryReader(Default.OuterXml)
                        },
                        First = true,
                        ExpectedException = ExpectedException.XmlException(inner: typeof(FormatException)),
                        TestId = "InnerReader-FullXml"
                    },
                    new DelegatingXmlDictionaryReaderTheoryData
                    {
                        DelegatingReader = new DelegatingXmlDictionaryReaderPublic
                        {
                            InnerReaderPublic = XmlUtilities.CreateDictionaryReader("<Elemnt URI=\"uuid-88d1a312-e27e-4bb8-a69f-e4fd295daf04\" />")
                        },
                        TestId = "InnerReader-PartialXmlWithUri"
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
