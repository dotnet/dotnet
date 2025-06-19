// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.TestUtils;
using Newtonsoft.Json;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.Tests
{
    /// <summary>
    /// An implementation of IConfigurationRetriever geared towards Azure AD issuers metadata.
    /// </summary>
    internal class IssuerConfigurationRetriever : IConfigurationRetriever<IssuerMetadata>
    {
        /// <summary>Retrieves a populated configuration given an address and an <see cref="T:Microsoft.IdentityModel.Protocols.IDocumentRetriever"/>.</summary>
        /// <param name="address">Address of the discovery document.</param>
        /// <param name="retriever">The <see cref="T:Microsoft.IdentityModel.Protocols.IDocumentRetriever"/> to use to read the discovery document.</param>
        /// <param name="cancel">A cancellation token that can be used by other objects or threads to receive notice of cancellation. <see cref="T:System.Threading.CancellationToken"/>.</param>
        /// <returns>
        /// A <see cref="Task{IssuerMetadata}"/> that, when completed, returns <see cref="IssuerMetadata"/> from the configuration.
        /// </returns>
        /// <exception cref="ArgumentNullException">address - Azure AD Issuer metadata address URL is required
        /// or retriever - No metadata document retriever is provided.</exception>
        public async Task<IssuerMetadata> GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            string doc = await retriever.GetDocumentAsync(address, cancel).ConfigureAwait(false);
            IssuerMetadata issuerMetadata = JsonConvert.DeserializeObject<IssuerMetadata>(doc);
            return issuerMetadata;
        }
    }

    /// <summary>
    /// Model class to hold information parsed from the Azure AD issuer endpoint.
    /// </summary>
    internal class IssuerMetadata
    {
        /// <summary>
        /// Issuer associated with the OIDC endpoint.
        /// </summary>
        public string Issuer { get; set; }
    }

    public class ExtensibilityTests
    {
        [Theory, MemberData(nameof(GetMetadataTheoryData))]
        public void GetMetadataTest(DocumentRetrieverTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.GetMetadataTest", theoryData);
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
                    theoryData.ExpectedException.ProcessException(x);
                    return true;
                });
            }
        }

        [Fact]
        public void ConfigurationManagerUsingCustomClass()
        {
            var docRetriever = new FileDocumentRetriever();
            var configManager = new ConfigurationManager<IssuerMetadata>("IssuerMetadata.json", new IssuerConfigurationRetriever(), docRetriever);
            var context = new CompareContext($"{this}.GetConfiguration");

            var configuration = configManager.GetConfigurationAsync().Result;
            configManager.MetadataAddress = "IssuerMetadata.json";
            var configuration2 = configManager.GetConfigurationAsync().Result;
            if (!IdentityComparer.AreEqual(configuration.Issuer, configuration2.Issuer))
                context.Diffs.Add("!IdentityComparer.AreEqual(configuration, configuration2)");

            // AutomaticRefreshInterval should pick up new bits.
            configManager = new ConfigurationManager<IssuerMetadata>("IssuerMetadata.json", new IssuerConfigurationRetriever(), docRetriever);
            configManager.RequestRefresh();
            configuration = configManager.GetConfigurationAsync().Result;
            TestUtilities.SetField(configManager, "_lastRefresh", DateTimeOffset.UtcNow - TimeSpan.FromHours(1));
            configManager.MetadataAddress = "IssuerMetadata2.json";
            configManager.RequestRefresh();
            configuration2 = configManager.GetConfigurationAsync().Result;
            if (IdentityComparer.AreEqual(configuration.Issuer, configuration2.Issuer))
                context.Diffs.Add("IdentityComparer.AreEqual(configuration, configuration2)");

            TestUtilities.AssertFailIfErrors(context);
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
