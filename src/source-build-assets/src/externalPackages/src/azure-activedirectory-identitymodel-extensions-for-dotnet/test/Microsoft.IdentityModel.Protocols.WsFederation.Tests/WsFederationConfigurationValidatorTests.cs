// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.WsFederation.Tests
{
    /// <summary>
    /// WS Federation Configuration Validator tests.
    /// </summary>
    public class WsFederationConfigurationValidatorTests
    {
        [Theory, MemberData(nameof(ValidateConfigurationTheoryData), DisableDiscoveryEnumeration = true)]
        public void ValidateConfiguration(WsFederationConfigurationTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ValidateConfiguration", theoryData);
            var validator = new WsFederationConfigurationValidator();
            var configToValidate = theoryData.Configuration;

            if (!string.IsNullOrEmpty(theoryData.Metadata))
            {
                var reader = XmlReader.Create(new StringReader(theoryData.Metadata));
                configToValidate = new WsFederationMetadataSerializer().ReadMetadata(reader);
            }

            try
            {
                var result = validator.Validate(configToValidate);
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreConfigurationValidationResultEqual(result, theoryData.ExpectedResult, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<WsFederationConfigurationTheoryData> ValidateConfigurationTheoryData
        {
            get
            {
                return new TheoryData<WsFederationConfigurationTheoryData>
                {
                    new WsFederationConfigurationTheoryData
                    {
                        // Base case for common scenario. All data is present as expected.
                        Metadata = ReferenceMetadata.AADCommonMetadata,
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = true
                        },
                        TestId = nameof(ReferenceMetadata.AADCommonMetadata)
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        // Base case for Active Directory Federation Services V4. All data is present as expected.
                        Metadata = ReferenceMetadata.AdfsV4Metadata,
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = true
                        },
                        TestId = nameof(ReferenceMetadata.AdfsV4Metadata)
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration =null,
                        ExpectedException = new ExpectedException(typeof(ArgumentNullException), "IDX10000:"),
                        TestId = "NullConfiguration"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.Issuer = null;
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22700
                        },
                        TestId = "NullIssuer"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.Signature.KeyInfo = null;
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22702
                        },
                        TestId = "NullSignatureKeyInfo"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.Signature.SignatureValue = "   ";
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22703
                        },
                        TestId = "EmptySignatureValue"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.Signature.SignedInfo.SignatureMethod = "  ";
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22705
                        },
                        TestId = "EmptySignatureMethod"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.Signature.SignedInfo.References.Clear();
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22706
                        },
                        TestId = "NoSignatureReferences"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.ActiveTokenEndpoint = string.Empty;
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22707
                        },
                        TestId = "EmptyActiveTokenEndpoint"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.ActiveTokenEndpoint = "SomeRandomValue@here";
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22708
                        },
                        TestId = "InvalidActiveTokenEndpointUri"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.TokenEndpoint = string.Empty;
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22709
                        },
                        TestId = "EmptyTokenEndpoint"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.TokenEndpoint = "SomeStringThatIsNotAUrl!";
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22710
                        },
                        TestId = "InvalidTokenEndpointUri"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Configuration = ((Func<WsFederationConfiguration>)(() =>{
                            var config = ReferenceMetadata.AdfsV4Endpoint;
                            config.SigningKeys.Clear();
                            return config;
                        }))(),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22711
                        },
                        TestId = "NoSigningKeys"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Metadata = ReferenceMetadata.AdfsV4Metadata.Replace(@"<fed:PassiveRequestorEndpoint><EndpointReference xmlns=""http://www.w3.org/2005/08/addressing""><Address>https://fs.msidlab11.com/adfs/ls/</Address></EndpointReference></fed:PassiveRequestorEndpoint>",
                            @"<fed:PassiveRequestorEndpoint><EndpointReference xmlns=""http://www.w3.org/2005/08/addressing""><Address>https://fs.malicious.com/adfs/ls/</Address></EndpointReference></fed:PassiveRequestorEndpoint>"),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22713
                        },
                        TestId = "TamperedMetadata-TokenEndpoints-PassiveRequestor"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Metadata = ReferenceMetadata.AADCommonMetadata.Replace(@"<fed:SecurityTokenServiceEndpoint><wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing""><wsa:Address>https://login.microsoftonline.com/common/wsfed</wsa:Address></wsa:EndpointReference></fed:SecurityTokenServiceEndpoint>",
                                @"<fed:SecurityTokenServiceEndpoint><wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing""><wsa:Address>https://login.malicious.com/common/wsfed</wsa:Address></wsa:EndpointReference></fed:SecurityTokenServiceEndpoint>"),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22713
                        },
                        TestId = "TamperedMetadata-TokenEndpoints-ActiveRequestor"
                    },
                    new WsFederationConfigurationTheoryData
                    {
                        Metadata = ReferenceMetadata.AADCommonMetadata.Replace(@"</KeyDescriptor><KeyDescriptor use=""signing"">",
                            @"</KeyDescriptor><KeyDescriptor use=""signing""><KeyInfo xmlns=""http://www.w3.org/2000/09/xmldsig#""><X509Data><X509Certificate>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</X509Certificate></X509Data></KeyInfo></KeyDescriptor><KeyDescriptor use=""signing"">"),
                        ExpectedResult = new ConfigurationValidationResult
                        {
                            Succeeded = false,
                            ErrorMessage = LogMessages.IDX22713
                        },
                        TestId = "TamperedMetadata-ExtraMaliciousKey"
                    }
                };
            }
        }

        public class WsFederationConfigurationTheoryData : TheoryDataBase
        {
            public WsFederationConfiguration Configuration { get; set; }

            public string Metadata { get; set; }

            public ConfigurationValidationResult ExpectedResult { get; set; }

            public override string ToString()
            {
                return $"TestId: {TestId}, {ExpectedException}";
            }
        }
    }
}
