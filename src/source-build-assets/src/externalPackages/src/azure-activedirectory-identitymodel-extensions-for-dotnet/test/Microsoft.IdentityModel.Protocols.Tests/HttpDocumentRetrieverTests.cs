// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpDocumentRetrieverTests
    {
        [Fact]
        public void Constructors()
        {
            HttpDocumentRetriever docRetriever = new HttpDocumentRetriever();
            Assert.Throws<ArgumentNullException>(() => new HttpDocumentRetriever(null));
        }

        [Fact]
        public void Defaults()
        {
        }

        [Fact]
        public void GetSets()
        {
            HttpDocumentRetriever docRetriever = new HttpDocumentRetriever();
            Type type = typeof(HttpDocumentRetriever);
            PropertyInfo[] properties = type.GetProperties();
            if (properties.Length != 1)
                Assert.True(true, "Number of properties has changed from 1 to: " + properties.Length + ", adjust tests");

            GetSetContext context =
                new GetSetContext
                {
                    PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                    {
                        new KeyValuePair<string, List<object>>("RequireHttps", new List<object>{true, false, true}),
                    },
                    Object = docRetriever,
                };

            TestUtilities.GetSet(context);
            TestUtilities.AssertFailIfErrors("HttpDocumentRetrieverTests_GetSets", context.Errors);
        }

        [Theory, MemberData(nameof(GetMetadataTheoryData))]
        public void GetMetadataTest(DocumentRetrieverTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.GetMetadataTest", theoryData);
            try
            {
                string doc = theoryData.DocumentRetriever.GetDocumentAsync(theoryData.Address, CancellationToken.None).Result;
                Assert.NotNull(doc);
                theoryData.ExpectedException.ProcessNoException();
            }
            catch (AggregateException aex)
            {
                aex.Handle((x) =>
                {
                    if (x.Data.Count > 0)
                    {
                        if (!x.Data.Contains(HttpDocumentRetriever.StatusCode))
                            context.AddDiff("!x.Data.Contains(HttpResponseConstants.StatusCode)");
                        if (!x.Data.Contains(HttpDocumentRetriever.ResponseContent))
                            context.AddDiff("!x.Data.Contains(HttpResponseConstants.ResponseContent)");
                        IdentityComparer.AreEqual(x.Data[HttpDocumentRetriever.StatusCode], theoryData.ExpectedStatusCode, context);
                    }
                    theoryData.ExpectedException.ProcessException(x);
                    return true;
                });
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DocumentRetrieverTheoryData> GetMetadataTheoryData
        {
            get
            {
                var theoryData = new TheoryData<DocumentRetrieverTheoryData>();

                var documentRetriever = new HttpDocumentRetriever();
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
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX20108:"),
                    TestId = "Require https, using file: 'OpenIdConnectMetadata.json'"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "httpss://OpenIdConnectMetadata.json",
                    DocumentRetriever = documentRetriever,
                    ExpectedException = new ExpectedException(typeof(ArgumentException), "IDX20108:"),
                    TestId = "Require https, Address: 'httpss://OpenIdConnectMetadata.json'"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "https://login.microsoftonline.com/common/.well-known/openid-configuration",
                    DocumentRetriever = documentRetriever,
                    TestId = "AAD common: https"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "HTTPS://login.microsoftonline.com/common/.well-known/openid-configuration",
                    DocumentRetriever = documentRetriever,
                    TestId = "AAD common: HTTPS"
                });

                documentRetriever = new HttpDocumentRetriever() { RequireHttps = false };
                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "OpenIdConnectMetadata.json",
                    DocumentRetriever = documentRetriever,
                    ExpectedException = new ExpectedException(typeof(IOException), "IDX20804:", typeof(InvalidOperationException)),
                    TestId = "RequireHttps == false, Address: 'OpenIdConnectMetadata.json'"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "https://login.microsoftonline.com/common/FederationMetadata/2007-06/FederationMetadata.xml",
                    DocumentRetriever = documentRetriever,
                    TestId = "AAD common: WsFed"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "https://login.windows.net/f686d426-8d16-42db-81b7-ab578e110ccd/.well-known/openid-configuration",
                    DocumentRetriever = documentRetriever,
                    ExpectedException = new ExpectedException(typeof(IOException), "IDX20807:"),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    TestId = "Client Miss Configuration"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "https://login.windows.net/f686d426-8d16-42db-81b7-ab578e110ccd/.well-known/openid-configuration",
                    DocumentRetriever = new HttpDocumentRetriever(HttpResponseMessageUtils.SetupHttpClientThatReturns("ValidJson.json", HttpStatusCode.RequestTimeout)),
                    TestId = "RequestTimeout_RefreshSucceeds"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "https://login.windows.net/f686d426-8d16-42db-81b7-ab578e110ccd/.well-known/openid-configuration",
                    DocumentRetriever = new HttpDocumentRetriever(HttpResponseMessageUtils.SetupHttpClientThatReturns("ValidJson.json", HttpStatusCode.ServiceUnavailable)),
                    TestId = "ServiceUnavailable_RefreshSucceeds"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "https://login.windows.net/f686d426-8d16-42db-81b7-ab578e110ccd/.well-known/openid-configuration",
                    DocumentRetriever = new HttpDocumentRetriever(HttpResponseMessageUtils.SetupHttpClientThatReturns("ValidJson.json", HttpStatusCode.ServiceUnavailable)),
                    TestId = "ServiceUnavailable_RefreshSucceeds"
                });

                theoryData.Add(new DocumentRetrieverTheoryData
                {
                    Address = "https://login.windows.net/f686d426-8d16-42db-81b7-ab578e110ccd/.well-known/openid-configuration",
                    DocumentRetriever = new HttpDocumentRetriever(HttpResponseMessageUtils.SetupHttpClientThatReturns("ValidJson.json", HttpStatusCode.NotFound)),
                    ExpectedException = new ExpectedException(typeof(IOException), "IDX20807:"),
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    TestId = "NotFound_NoRefresh"
                });

                return theoryData;
            }
        }
    }

    public class DocumentRetrieverTheoryData : TheoryDataBase
    {
        public string Address { get; set; }

        public IDocumentRetriever DocumentRetriever { get; set; }

        public HttpStatusCode ExpectedStatusCode { get; set; }

        public override string ToString()
        {
            return $"{TestId}, {Address}, {ExpectedException}";
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
