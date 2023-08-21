// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Protocols.WsFederation.Tests
{
    /// <summary>
    /// WsFed metadata reading tests.
    /// </summary>
    public class WsFederationConfigurationRetrieverTests
    {
        [Theory, MemberData(nameof(ReadMetadataTheoryData))]
        public void ReadMetadata(WsFederationMetadataTheoryData theoryData)
        {
            var context  = TestUtilities.WriteHeader($"{this}.ReadMetadata", theoryData);
            var configuration = new WsFederationConfiguration();

            try
            {
                if (!string.IsNullOrEmpty(theoryData.Metadata))
                {
                    var reader = XmlReader.Create(new StringReader(theoryData.Metadata));
                    configuration = theoryData.Serializer.ReadMetadata(reader);
                }
                else
                {
                    var reader = XmlReader.Create(theoryData.MetadataPath);
                    configuration = theoryData.Serializer.ReadMetadata(reader);
                }
               
                if (theoryData.SigningKey != null)
                    configuration.Signature.Verify(theoryData.SigningKey, theoryData.SigningKey.CryptoProviderFactory);

                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreWsFederationConfigurationsEqual(configuration, theoryData.Configuration, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<WsFederationMetadataTheoryData> ReadMetadataTheoryData
        {
            get
            {
                // uncomment to see exception displayed to user.
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<WsFederationMetadataTheoryData>
                {
                    new WsFederationMetadataTheoryData
                    {
                        // Base case for common scenario (not tenant specific).
                        // All data is present as expected.
                        Configuration = ReferenceMetadata.AADCommonEndpoint,
                        First = true,
                        Metadata = ReferenceMetadata.AADCommonMetadata,
                        SigningKey = ReferenceMetadata.MetadataSigningKey,
                        TestId = nameof(ReferenceMetadata.AADCommonMetadata)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Only EntityDescriptor tag, empty XML. 
                        Configuration = ReferenceMetadata.EmptyEntityDescriptor,
                        Metadata = ReferenceMetadata.MetadataEmptyEntityDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataEmptyEntityDescriptor)
                    },
                    // ---------------------------------------------------------------------------------------------------------------------
                    // Passive Requestor variations (EntityDescriptor\RoleDescriptor\PassiveRequestorEndpoint)
                    // ---------------------------------------------------------------------------------------------------------------------
                    new WsFederationMetadataTheoryData
                    {
                        // Empty EntityDescriptor\RoleDescriptor\PassiveRequestorEndpoint tag <fed:PassiveRequestorEndpoint /> 
                        // Error Message: "IDX22812: Element: '{0}' was an empty element. 'TokenEndpoint' value is missing in wsfederationconfiguration.";
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22812:"),
                        Metadata = ReferenceMetadata.MetadataEmptyPassiveRequestorEndpoint,
                        TestId = nameof(ReferenceMetadata.MetadataEmptyPassiveRequestorEndpoint)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Empty EntityDescriptor\RoleDescriptor\PassiveRequestorEndpoint\EndpointReference\Address tag  <wsa:Address/>
                        // Error Message:  "IDX22803: Token reference address is missing in PassiveRequestorEndpoint in metadata file."
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22803:"),
                        Metadata = ReferenceMetadata.MetadataEmptyPassiveRequestorEndpointAddress,
                        TestId = nameof(ReferenceMetadata.MetadataEmptyPassiveRequestorEndpointAddress)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Empty EntityDescriptor\RoleDescriptor\PassiveRequestorEndpoint\EndpointReference <wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing"" />
                        // Error Message: Element: '{0}' was an empty element. 'TokenEndpoint' value is missing in wsfederationconfiguration.";
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22812:"),
                        Metadata = ReferenceMetadata.MetadataEmptyPassiveRequestorEndpointReference,
                        TestId = nameof(ReferenceMetadata.MetadataEmptyPassiveRequestorEndpointReference)
                    },
                    // ---------------------------------------------------------------------------------------------------------------------
                    // SecurityTokenServiceEndpoint variations (EntityDescriptor\RoleDescriptor\SecurityTokenServiceEndpoint)
                    // ---------------------------------------------------------------------------------------------------------------------
                    new WsFederationMetadataTheoryData
                    {
                        // Empty EntityDescriptor\RoleDescriptor\SecurityTokenServiceEndpoint tag <fed:SecurityTokenServiceEndpoint /> 
                        // Error Message: "IDX22812: Element: '{0}' was an empty element. 'TokenEndpoint' value is missing in wsfederationconfiguration.";
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22812:"),
                        Metadata = ReferenceMetadata.MetadataEmptySecurityTokenServiceEndpoint,
                        TestId = nameof(ReferenceMetadata.MetadataEmptySecurityTokenServiceEndpoint)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Empty EntityDescriptor\RoleDescriptor\SecurityTokenServiceEndpoint\EndpointReference\Address tag  <wsa:Address/>
                        // Error Message: "IDX22814: Token reference address is missing in SecurityTokenServiceEndpoint in metadata file."
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22814:"),
                        Metadata = ReferenceMetadata.MetadataEmptySecurityTokenServiceEndpointAddress,
                        TestId = nameof(ReferenceMetadata.MetadataEmptySecurityTokenServiceEndpointAddress)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Empty EntityDescriptor\RoleDescriptor\SecurityTokenServiceEndpoint\EndpointReference <wsa:EndpointReference xmlns:wsa=""http://www.w3.org/2005/08/addressing"" />
                        // Error Message: "IDX22812: Element: '{0}' was an empty element. 'TokenEndpoint' value is missing in wsfederationconfiguration.";
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22812:"),
                        Metadata = ReferenceMetadata.MetadataEmptySecurityTokenServiceEndpointReference,
                        TestId = nameof(ReferenceMetadata.MetadataEmptySecurityTokenServiceEndpointReference)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Base case for tenant specific scenario (tenant 268da1a1-9db4-48b9-b1fe-683250ba90cc).
                        // All data is present as expected.
                        Configuration = ReferenceMetadata.AADCommonFormated,
                        Metadata = ReferenceMetadata.AADCommonMetadataFormated,
                        TestId = nameof(ReferenceMetadata.AADCommonMetadataFormated)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30200:"),
                        Configuration = ReferenceMetadata.AADCommonFormated,
                        Metadata = ReferenceMetadata.AADCommonMetadataFormated,
                        SigningKey = ReferenceMetadata.MetadataSigningKey,
                        TestId = nameof(ReferenceMetadata.AADCommonMetadataFormated) + " Signature Failure"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Validate that the presence of spaces or new lines does not affect the parsing of the XML content.
                        Configuration = ReferenceMetadata.AADCommonFormated,
                        Metadata = ReferenceMetadata.MetadataWithBlanks,
                        TestId = nameof(ReferenceMetadata.MetadataWithBlanks)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\RoleDescriptor\KeyDescriptor is missing. Validate the resulting Configuration object only includes the data present.
                        Configuration = new WsFederationConfiguration
                        {
                            Issuer = ReferenceMetadata.Issuer,
                            TokenEndpoint = ReferenceMetadata.TokenEndpoint
                        },
                        Metadata = ReferenceMetadata.MetadataNoKeyDescriptorForSigningInRoleDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoKeyDescriptorForSigningInRoleDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\@entityID attribute (issuer) is missing from EntityDescriptor tag.
                        // Error Message: IDX22801: entityID attribute is not found in EntityDescriptor element in metadata file.
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22801:"),
                        Metadata = ReferenceMetadata.MetadataNoIssuer,
                        TestId = nameof(ReferenceMetadata.MetadataNoIssuer)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\RoleDescriptor\PassiveRequestorEndpoint\EndpointReference\Address Empty Address value (white space and new line only)
                        // Error Message: "IDX22803: Token reference address is missing in PassiveRequestorEndpoint in metadata file.";
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22803:"),
                        Metadata = ReferenceMetadata.MetadataNoPassiveRequestorEndpointUri,
                        TestId = nameof(ReferenceMetadata.MetadataNoPassiveRequestorEndpointUri)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\RoleDescriptor\SecurityTokenServiceEndpoint\EndpointReference\Address Empty Address value (white space and new line only)
                        // Error Message: "IDX22814: Token reference address is missing in SecurityTokenServiceEndpoint in metadata.";
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22814:"),
                        Metadata = ReferenceMetadata.MetadataNoSecurityTokenServiceEndpointUri,
                        TestId = nameof(ReferenceMetadata.MetadataNoSecurityTokenServiceEndpointUri)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // KeyDescriptor\KeyInfo\X509Data tag holds invalid certificate data.
                        // Error Message: "IDX22800: Exception thrown while reading WsFedereationMetadata. Element '{0}'. Caught exception: '{1}'.";
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22800:", typeof(FormatException)),
                        Metadata = ReferenceMetadata.MetadataMalformedCertificate,
                        TestId = nameof(ReferenceMetadata.MetadataMalformedCertificate)
                    },
                    // ---------------------------------------------------------------------------------------------------------------------
                    // XML Signature validation (EntityDescriptor\Signature)
                    // ---------------------------------------------------------------------------------------------------------------------
                    new WsFederationMetadataTheoryData
                    {
                        // Invalid XML signature. Unknown element before </Signature>
                        // Error Message: "IDX30025: Unable to read XML. Expecting XmlReader to be at EndElement: '{0}'. Found XmlNode 'type.name': '{1}.{2}'.";
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30025:"),
                        Metadata = ReferenceMetadata.MetadataUnknownElementBeforeSignatureEndElement,
                        TestId = nameof(ReferenceMetadata.MetadataUnknownElementBeforeSignatureEndElement)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Invalid XML signature. SignedInfo tag is missing.
                        // Error Message: "IDX30011: Unable to read XML. Expecting XmlReader to be at ns.element: '{0}.{1}', found: '{2}.{3}'."
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30011:"),
                        Metadata = ReferenceMetadata.MetadataNoSignedInfoInSignature,
                        TestId = nameof(ReferenceMetadata.MetadataNoSignedInfoInSignature)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor tag missing.
                        // Error Message: "IDX30011: Unable to read XML. Expecting XmlReader to be at ns.element: '{0}.{1}', found: '{2}.{3}'."
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30011:"),
                        Metadata = ReferenceMetadata.MetadataNoEntityDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoEntityDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\RoleDescriptor tag missing.
                        Configuration = ReferenceMetadata.NoRoleDescriptor,
                        Metadata = ReferenceMetadata.MetadataNoRoleDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoRoleDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\RoleDescriptor\KeyDescriptor\KeyInfo tag missing.
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22802:"),
                        Metadata = ReferenceMetadata.MetadataNoKeyInfoInKeyDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoKeyInfoInKeyDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\RoleDescriptor\PassiveRequestorEndpoint tag is missing.
                        Configuration = new WsFederationConfiguration
                        {
                            Issuer = ReferenceMetadata.Issuer
                        },
                        Metadata = ReferenceMetadata.MetadataNoPassiveRequestorEndpointInRoleDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoPassiveRequestorEndpointInRoleDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\RoleDescriptor\PassiveRequestorEndpoint\EndpointReference tag is missing.
                        // Error Message: "IDX30011: Unable to read XML. Expecting XmlReader to be at ns.element: '{0}.{1}', found: '{2}.{3}'."
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30011:"),
                        Metadata = ReferenceMetadata.MetadataNoEndpointReference,
                        TestId = nameof(ReferenceMetadata.MetadataNoEndpointReference)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // EntityDescriptor\RoleDescriptor\PassiveRequestorEndpoint\EndpointReference\Address tag is missing.
                        // Error Message: "IDX30011: Unable to read XML. Expecting XmlReader to be at ns.element: '{0}.{1}', found: '{2}.{3}'."
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30011:"),
                        Metadata = ReferenceMetadata.MetadataNoAddressInEndpointReference,
                        TestId = nameof(ReferenceMetadata.MetadataNoAddressInEndpointReference)
                    },
                    // ---------------------------------------------------------------------------------------------------------------------
                    // Active Directory Federation Services
                    // ---------------------------------------------------------------------------------------------------------------------
                    new WsFederationMetadataTheoryData
                    {
                        // Base case for Active Directory Federation Services V2.
                        // All data present and valid.
                        Metadata = ReferenceMetadata.AdfsV2Metadata,
                        SigningKey = ReferenceMetadata.AdfsV2MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV2Endpoint,
                        TestId = nameof(ReferenceMetadata.AdfsV2Metadata)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Base case for Active Directory Federation Services V3.
                        // All data present and valid.
                        Metadata = ReferenceMetadata.AdfsV3Metadata,
                        SigningKey = ReferenceMetadata.AdfsV3MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV3Endpoint,
                        TestId = nameof(ReferenceMetadata.AdfsV3Metadata)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Base case for Active Directory Federation Services V4.
                        // All data present and valid.
                        Metadata = ReferenceMetadata.AdfsV4Metadata,
                        SigningKey = ReferenceMetadata.AdfsV4MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV4Endpoint,
                        TestId = nameof(ReferenceMetadata.AdfsV4Metadata)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Base case for Active Directory Federation Services V2.
                        // All data present and valid.
                        MetadataPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../adfs-v2-metadata.xml"),
                        SigningKey = ReferenceMetadata.AdfsV2MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV2Endpoint,
                        TestId = "AdfsV2Metadata from xml file"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Base case for Active Directory Federation Services V3.
                        // All data present and valid.
                        MetadataPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../adfs-v3-metadata.xml"),
                        SigningKey = ReferenceMetadata.AdfsV3MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV3Endpoint,
                        TestId = "AdfsV3Metadata from xml file"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // Base case for Active Directory Federation Services V4.
                        // All data present and valid.
                        MetadataPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../adfs-v4-metadata.xml"),
                        SigningKey = ReferenceMetadata.AdfsV4MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV4Endpoint,
                        TestId = "AdfsV4Metadata from xml file"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ReadEntityDescriptorTheoryData))]
        public void ReadEntityDescriptor(WsFederationMetadataTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadEntityDescriptor", theoryData);
            var serializer = new WsFederationMetadataSerializerPublic();
            try
            {
                XmlReader reader = null;
                if (theoryData.Metadata != null)
                    reader = XmlReader.Create(new StringReader(theoryData.Metadata));
                serializer.ReadEntityDescriptorPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<WsFederationMetadataTheoryData> ReadEntityDescriptorTheoryData
        {
            get
            {
                return new TheoryData<WsFederationMetadataTheoryData>
                {
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TestId = "ReadEntityDescriptor"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Metadata = ReferenceMetadata.EmptyEntityDescriptorMetadata,
                        TestId = "ReadEmptyEntityDescriptor"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ReadKeyDescriptorForSigningTheoryData))]
        public void ReadKeyDescriptorForSigning(WsFederationMetadataTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadKeyDescriptorForSigning", theoryData);
            var serializer = new WsFederationMetadataSerializerPublic();
            try
            {
                XmlReader reader = null;
                if (theoryData.Metadata != null)
                    reader = XmlReader.Create(new StringReader(theoryData.Metadata));
                serializer.ReadKeyDescriptorForSigningPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<WsFederationMetadataTheoryData> ReadKeyDescriptorForSigningTheoryData
        {
            get
            {
                return new TheoryData<WsFederationMetadataTheoryData>
                {
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TestId = "ReadKeyDescriptorForSigning"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ReadKeyDescriptorForSigningKeyUseTheoryData))]
        public void ReadKeyDescriptorForSigningKeyUse(WsFederationMetadataTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadKeyDescriptorForSigningKeyUse", theoryData);
            var serializer = new WsFederationMetadataSerializerPublic();
            try
            {
                serializer.ReadKeyDescriptorForSigningPublic(XmlReader.Create(new StringReader(theoryData.Metadata)));
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<WsFederationMetadataTheoryData> ReadKeyDescriptorForSigningKeyUseTheoryData
        {
            get
            {
                return new TheoryData<WsFederationMetadataTheoryData>
                {
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        Metadata = ReferenceMetadata.KeyDescriptorNoKeyUse,
                        TestId = "ReadKeyDescriptorForSigning: 'use' is null"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = ExpectedException.NoExceptionExpected,
                        Metadata = ReferenceMetadata.KeyDescriptorKeyUseNotForSigning,
                        TestId = "ReadKeyDescriptorForSigning: 'use' is not 'signing'"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22802"),
                        Metadata = ReferenceMetadata.EmptyKeyDescriptor,
                        TestId = "ReadKeyDescriptorForSigning: KeyDescriptor is empty"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ReadSecurityTokenServiceTypeRoleDescriptorTheoryData))]
        public void ReadSecurityTokenServiceTypeRoleDescriptor(WsFederationMetadataTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadSecurityTokenServiceTypeRoleDescriptor", theoryData);
            var serializer = new WsFederationMetadataSerializerPublic();
            try
            {
                XmlReader reader = null;
                if (theoryData.Metadata != null)
                    reader = XmlReader.Create(new StringReader(theoryData.Metadata));
                serializer.ReadSecurityTokenServiceTypeRoleDescriptorPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<WsFederationMetadataTheoryData> ReadSecurityTokenServiceTypeRoleDescriptorTheoryData
        {
            get
            {
                return new TheoryData<WsFederationMetadataTheoryData>
                {
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TestId = "ReadSecurityTokenServiceTypeRoleDescriptor"
                    },
                     new WsFederationMetadataTheoryData
                    {
                        Metadata = ReferenceMetadata.EmptyRoleDescriptor,
                        TestId = "ReadSecurityTokenServiceTypeRoleDescriptor : Empty"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ReadSecurityTokenEndpointTheoryData))]
        public void ReadSecurityTokenEndpoint(WsFederationMetadataTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadSecurityTokenEndpoint", theoryData);
            var serializer = new WsFederationMetadataSerializerPublic();
            try
            {
                XmlReader reader = null;
                if (theoryData.Metadata != null)
                    reader = XmlReader.Create(new StringReader(theoryData.Metadata));
                serializer.ReadSecurityTokenEndpointPublic(reader);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<WsFederationMetadataTheoryData> ReadSecurityTokenEndpointTheoryData
        {
            get
            {
                return new TheoryData<WsFederationMetadataTheoryData>
                {
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TestId = "ReadSecurityTokenEndpoint"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22812:"),
                        Metadata = ReferenceMetadata.EmptyPassiveRequestorEndpoint,
                        TestId = "ReadSecurityTokenEndpoint : Empty"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22812:"),
                        Metadata = ReferenceMetadata.EndpointWithEmptyEndpointReference,
                        TestId = "ReadSecurityTokenEndpoint : EmptyEndpointReference"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22803:"),
                        Metadata = ReferenceMetadata.EndpointWithEmptyAddress,
                        TestId = "ReadSecurityTokenEndpoint : EmptyAddress"
                    },
                };
            }
        }

        [Theory, MemberData(nameof(WriteMetadataTheoryData))]
        public void WriteMetadata(WsFederationMetadataTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.WriteMetadata", theoryData);
            var context = new CompareContext($"{this}.WriteMetadata, {theoryData.TestId}");
            try
            {
                var settings = new XmlWriterSettings();
                var builder = new StringBuilder();

                if (theoryData.UseNullWriter)
                {
                    theoryData.Serializer.WriteMetadata(null, theoryData.Configuration);
                    theoryData.ExpectedException.ProcessNoException(context);
                }
                else
                {
                    using (var writer = XmlWriter.Create(builder, settings))
                    {
                        // add signingCredentials so we can created signed metadata.
                        if (theoryData.Configuration != null)
                            theoryData.Configuration.SigningCredentials = KeyingMaterial.DefaultX509SigningCreds_2048_RsaSha2_Sha2;

                        // write configuration content into metadata and sign the metadata
                        var serializer = new WsFederationMetadataSerializer();
                        serializer.WriteMetadata(writer, theoryData.Configuration);
                        writer.Flush();
                        var metadata = builder.ToString();

                        // read the created metadata into a new configuration
                        var reader = XmlReader.Create(new StringReader(metadata));
                        var configuration = theoryData.Serializer.ReadMetadata(reader);

                        // assign signingcredentials and verify the signature of created metadata
                        configuration.SigningCredentials = theoryData.Configuration.SigningCredentials;
                        if (configuration.SigningCredentials != null)
                            configuration.Signature.Verify(configuration.SigningCredentials.Key, configuration.SigningCredentials.Key.CryptoProviderFactory);

                        // remove the signature and do the comparison
                        configuration.Signature = null;
                        theoryData.Configuration.Signature = null;

                        theoryData.ExpectedException.ProcessNoException(context);
                        IdentityComparer.AreWsFederationConfigurationsEqual(configuration, theoryData.Configuration, context);
                    }
                }
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<WsFederationMetadataTheoryData> WriteMetadataTheoryData
        {
            get
            {
                return new TheoryData<WsFederationMetadataTheoryData>
                {
                    new WsFederationMetadataTheoryData
                    {
                        First = true,
                        Configuration = ReferenceMetadata.AADCommonFormatedNoSignature,
                        TestId = nameof(ReferenceMetadata.AADCommonFormatedNoSignature)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        UseNullWriter = true,
                        Configuration = ReferenceMetadata.AADCommonFormatedNoSignature,
                        TestId = "Use null writer"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("IDX10000:"),
                        TestId = "Use null configuration"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlWriteException), "IDX22810:"),
                        Configuration = ReferenceMetadata.AADCommonFormatedNoIssuer,
                        TestId = nameof(ReferenceMetadata.AADCommonFormatedNoIssuer)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlWriteException), "IDX22811:"),
                        Configuration = ReferenceMetadata.AADCommonFormatedNoTokenEndpoint,
                        TestId = nameof(ReferenceMetadata.AADCommonFormatedNoTokenEndpoint)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // The active token endpoint is optional at this point and should not throw an exception if missing.
                        Configuration = ReferenceMetadata.AADCommonFormatedNoActiveTokenEndpoint,
                        TestId = nameof(ReferenceMetadata.AADCommonFormatedNoActiveTokenEndpoint)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        // All data is present including signature and SigningCredentials (required for signature validation)
                        Configuration = ReferenceMetadata.AADCommonFormated,
                        TestId = nameof(ReferenceMetadata.AADCommonFormated)
                    },
                };
            }
        }

        public class WsFederationMetadataTheoryData : TheoryDataBase
        {
            public WsFederationConfiguration Configuration { get; set; }

            public string Metadata { get; set; }

            public string MetadataPath { get; set; }

            public WsFederationMetadataSerializer Serializer { get; set; } = new WsFederationMetadataSerializer();

            public SecurityKey SigningKey { get; set; }

            public override string ToString()
            {
                return $"TestId: {TestId}, {ExpectedException}";
            }

            public bool UseNullWriter { get; set; } = false;
        }

        private class WsFederationMetadataSerializerPublic : WsFederationMetadataSerializer
        {
            public WsFederationConfiguration ReadEntityDescriptorPublic(XmlReader reader)
            {
                return base.ReadEntityDescriptor(reader);
            }

            public KeyInfo ReadKeyDescriptorForSigningPublic(XmlReader reader)
            {
                return base.ReadKeyDescriptorForSigning(reader);
            }

            public SecurityTokenServiceTypeRoleDescriptor ReadSecurityTokenServiceTypeRoleDescriptorPublic(XmlReader reader)
            {
                return base.ReadSecurityTokenServiceTypeRoleDescriptor(reader);
            }

            public string ReadSecurityTokenEndpointPublic(XmlReader reader)
            {
                return base.ReadPassiveRequestorEndpoint(reader);
            }
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
