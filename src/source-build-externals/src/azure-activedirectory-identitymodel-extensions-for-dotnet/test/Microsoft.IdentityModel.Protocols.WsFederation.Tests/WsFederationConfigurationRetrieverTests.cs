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
            try
            {
                var config = ReferenceMetadata.AADCommonEndpoint;
                var configuration = new WsFederationConfiguration();

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

                if (theoryData.SigingKey != null)
                    configuration.Signature.Verify(theoryData.SigingKey, theoryData.SigingKey.CryptoProviderFactory);

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
                        Configuration = ReferenceMetadata.AADCommonEndpoint,
                        First = true,
                        Metadata = ReferenceMetadata.AADCommonMetadata,
                        SigingKey = ReferenceMetadata.MetadataSigningKey,
                        TestId = nameof(ReferenceMetadata.AADCommonMetadata)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Configuration = ReferenceMetadata.EmptyEntityDescriptor,
                        Metadata = ReferenceMetadata.MetadataEmptyEntityDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataEmptyEntityDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22812:"),
                        Metadata = ReferenceMetadata.MetadataEmptyPassiveRequestorEndpoint,
                        TestId = nameof(ReferenceMetadata.MetadataEmptyPassiveRequestorEndpoint)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22803:"),
                        Metadata = ReferenceMetadata.MetadataEmptyEndpointAddress,
                        TestId = nameof(ReferenceMetadata.MetadataEmptyEndpointAddress)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22812:"),
                        Metadata = ReferenceMetadata.MetadataEmptyEndpointReference,
                        TestId = nameof(ReferenceMetadata.MetadataEmptyEndpointReference)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Configuration = ReferenceMetadata.AADCommonFormated,
                        Metadata = ReferenceMetadata.AADCommonMetadataFormated,
                        TestId = nameof(ReferenceMetadata.AADCommonMetadataFormated)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlValidationException), "IDX30200:"),
                        Configuration = ReferenceMetadata.AADCommonFormated,
                        Metadata = ReferenceMetadata.AADCommonMetadataFormated,
                        SigingKey = ReferenceMetadata.MetadataSigningKey,
                        TestId = nameof(ReferenceMetadata.AADCommonMetadataFormated) + " Signature Failure"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Configuration = ReferenceMetadata.AADCommonFormated,
                        Metadata = ReferenceMetadata.MetadataWithBlanks,
                        TestId = nameof(ReferenceMetadata.MetadataWithBlanks)
                    },
                    new WsFederationMetadataTheoryData
                    {
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
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22801:"),
                        Metadata = ReferenceMetadata.MetadataNoIssuer,
                        TestId = nameof(ReferenceMetadata.MetadataNoIssuer)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22803:"),
                        Metadata = ReferenceMetadata.MetadataNoTokenUri,
                        TestId = nameof(ReferenceMetadata.MetadataNoTokenUri)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22800:", typeof(FormatException)),
                        Metadata = ReferenceMetadata.MetadataMalformedCertificate,
                        TestId = nameof(ReferenceMetadata.MetadataMalformedCertificate)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30025:"),
                        Metadata = ReferenceMetadata.MetadataUnknownElementBeforeSignatureEndElement,
                        TestId = nameof(ReferenceMetadata.MetadataUnknownElementBeforeSignatureEndElement)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30011:"),
                        Metadata = ReferenceMetadata.MetadataNoSignedInfoInSignature,
                        TestId = nameof(ReferenceMetadata.MetadataNoSignedInfoInSignature)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30011:"),
                        Metadata = ReferenceMetadata.MetadataNoEntityDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoEntityDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Configuration = ReferenceMetadata.NoRoleDescriptor,
                        Metadata = ReferenceMetadata.MetadataNoRoleDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoRoleDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX22802:"),
                        Metadata = ReferenceMetadata.MetadataNoKeyInfoInKeyDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoKeyInfoInKeyDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Configuration = new WsFederationConfiguration
                        {
                            Issuer = ReferenceMetadata.Issuer
                        },
                        Metadata = ReferenceMetadata.MetadataNoPassiveRequestorEndpointInRoleDescriptor,
                        TestId = nameof(ReferenceMetadata.MetadataNoPassiveRequestorEndpointInRoleDescriptor)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30011:"),
                        Metadata = ReferenceMetadata.MetadataNoEndpointReference,
                        TestId = nameof(ReferenceMetadata.MetadataNoEndpointReference)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30011:"),
                        Metadata = ReferenceMetadata.MetadataNoAddressInEndpointReference,
                        TestId = nameof(ReferenceMetadata.MetadataNoAddressInEndpointReference)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Metadata = ReferenceMetadata.AdfsV2Metadata,
                        SigingKey = ReferenceMetadata.AdfsV2MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV2Endpoint,
                        TestId = nameof(ReferenceMetadata.AdfsV2Metadata)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Metadata = ReferenceMetadata.AdfsV3Metadata,
                        SigingKey = ReferenceMetadata.AdfsV3MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV3Endpoint,
                        TestId = nameof(ReferenceMetadata.AdfsV3Metadata)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        Metadata = ReferenceMetadata.AdfsV4Metadata,
                        SigingKey = ReferenceMetadata.AdfsV4MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV4Endpoint,
                        TestId = nameof(ReferenceMetadata.AdfsV4Metadata)
                    },
                    new WsFederationMetadataTheoryData
                    {
                        MetadataPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../adfs-v2-metadata.xml"),
                        SigingKey = ReferenceMetadata.AdfsV2MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV2Endpoint,
                        TestId = "AdfsV2Metadata from xml file"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        MetadataPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../adfs-v3-metadata.xml"),
                        SigingKey = ReferenceMetadata.AdfsV3MetadataSigningKey,
                        Configuration = ReferenceMetadata.AdfsV3Endpoint,
                        TestId = "AdfsV3Metadata from xml file"
                    },
                    new WsFederationMetadataTheoryData
                    {
                        MetadataPath = Path.Combine(Directory.GetCurrentDirectory(), "../../../adfs-v4-metadata.xml"),
                        SigingKey = ReferenceMetadata.AdfsV4MetadataSigningKey,
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
                    }
                };
            }
        }

        public class WsFederationMetadataTheoryData : TheoryDataBase
        {
            public WsFederationConfiguration Configuration { get; set; }

            public string Metadata { get; set; }

            public string MetadataPath { get; set; }

            public WsFederationMetadataSerializer Serializer { get; set; } = new WsFederationMetadataSerializer();

            public SecurityKey SigingKey { get; set; }

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
