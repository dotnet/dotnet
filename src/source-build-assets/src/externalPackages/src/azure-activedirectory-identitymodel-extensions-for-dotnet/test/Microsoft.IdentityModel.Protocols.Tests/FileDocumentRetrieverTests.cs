// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.Tests
{
    public class FileDocumentRetrieverTests
    {

        [Theory, MemberData(nameof(GetMetadataTheoryData), DisableDiscoveryEnumeration = true)]
        public async Task GetMetadataTest(DocumentRetrieverTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.GetMetadataTest", theoryData);
            try
            {
                string doc = await theoryData.DocumentRetriever.GetDocumentAsync(theoryData.Address, CancellationToken.None);
                Assert.NotNull(doc);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }
        }

        public static TheoryData<DocumentRetrieverTheoryData> GetMetadataTheoryData
        {
            get
            {
                var theoryData = new TheoryData<DocumentRetrieverTheoryData>();

                var documentRetriever = new FileDocumentRetriever();
                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = null,
                    DocumentRetriever = documentRetriever,
                    ExpectedException = ExpectedException.ArgumentNullException(),
                    First = true,
                    TestId = "Address NULL"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "OpenIdConnectMetadata.json",
                    DocumentRetriever = documentRetriever,
                    ExpectedException = ExpectedException.IOException("IDX20804:", typeof(FileNotFoundException), "IDX20814:"),
                    TestId = "File not found: OpenIdConnectMetadata.json"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "ValidJson.json",
                    DocumentRetriever = documentRetriever,
                    TestId = "ValidJson.json - JsonWebKeySet"
                });

                return theoryData;
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
