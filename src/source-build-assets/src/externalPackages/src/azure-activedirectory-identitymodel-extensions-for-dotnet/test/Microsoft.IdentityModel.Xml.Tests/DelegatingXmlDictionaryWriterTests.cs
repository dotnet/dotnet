// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class DelegatingXmlDictionaryWriterTests
    {
        [Theory, MemberData(nameof(WriteXmlTheoryData), DisableDiscoveryEnumeration = true)]
        public void WriteXml(DelegatingXmlDictionaryWriterTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WriteXml", theoryData);
            try
            {
                theoryData.DelegatingWriter.WriteStartElement("name", "prefix", "namespace");
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DelegatingXmlDictionaryWriterTheoryData> WriteXmlTheoryData
        {
            get
            {
                return new TheoryData<DelegatingXmlDictionaryWriterTheoryData>
                {
                    new DelegatingXmlDictionaryWriterTheoryData
                    {
                        DelegatingWriter = new DelegatingXmlDictionaryWriterPublic(),
                        ExpectedException = ExpectedException.InvalidOperationException("IDX30028:"),
                        First = true,
                        TestId = "InnerWriter-Null"
                    },
                    new DelegatingXmlDictionaryWriterTheoryData
                    {
                        DelegatingWriter = new DelegatingXmlDictionaryWriterPublic
                        {
                            InnerWriterPublic = XmlDictionaryWriter.CreateBinaryWriter(new MemoryStream())
                        },
                        TestId = "InnerWriter-Set"
                    }
                };
            }
        }
    }

    public class DelegatingXmlDictionaryWriterPublic : DelegatingXmlDictionaryWriter
    {
        public XmlDictionaryWriter InnerWriterPublic
        {
            get => InnerWriter;
            set => InnerWriter = value;
        }
    }

    public class DelegatingXmlDictionaryWriterTheoryData : TheoryDataBase
    {
        public DelegatingXmlDictionaryWriterPublic DelegatingWriter { get; set; }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
