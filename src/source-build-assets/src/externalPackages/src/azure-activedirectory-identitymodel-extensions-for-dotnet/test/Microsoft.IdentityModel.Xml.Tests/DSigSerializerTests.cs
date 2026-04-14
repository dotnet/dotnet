// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Xml.Tests
{
    public class DSigSerializerTests
    {
        [Fact]
        public void GetSets()
        {
            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("TransformFactory", new List<object>{TransformFactory.Default}),
                    new KeyValuePair<string, List<object>>("MaximumReferenceTransforms", new List<object>{5, 11, 1 })
                },
                Object = new DSigSerializer()
            };

            TestUtilities.GetSet(context);
            TestUtilities.SetGet(new DSigSerializer(), "MaximumReferenceTransforms", 0, ExpectedException.NoExceptionExpected, context);
            TestUtilities.SetGet(new DSigSerializer(), "MaximumReferenceTransforms", -1, ExpectedException.ArgumentOutOfRangeException("IDX30600:"), context);

            TestUtilities.AssertFailIfErrors("DSigSerializerTests_GetSets", context.Errors);
        }

        [Theory, MemberData(nameof(ReadKeyInfoTheoryData))]
        public void ReadKeyInfo(DSigSerializerTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadKeyInfo", theoryData);
            var context = new CompareContext($"{this}.ReadKeyInfo, {theoryData.TestId}");
            try
            {
                var keyInfo = theoryData.Serializer.ReadKeyInfo(XmlUtilities.CreateDictionaryReader(theoryData.Xml));
                theoryData.ExpectedException.ProcessNoException(context.Diffs);
                IdentityComparer.AreKeyInfosEqual(keyInfo, theoryData.KeyInfo, context);

                // make sure we write and then read
                // as we only support partial elements, the KeyInfo's cannot be compared
                var ms = new MemoryStream();
                var writer = XmlDictionaryWriter.CreateTextWriter(ms);
                theoryData.Serializer.WriteKeyInfo(writer, keyInfo);
                writer.Flush();
                var xml = Encoding.UTF8.GetString(ms.ToArray());
                theoryData.Serializer.ReadKeyInfo(XmlUtilities.CreateDictionaryReader(xml));
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context.Diffs);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> ReadKeyInfoTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;
                return new TheoryData<DSigSerializerTheoryData>
                {
                    //KeyInfoTest(KeyInfoTestSet.MalformedCertificate, new ExpectedException(typeof(XmlReadException), "IDX30017:", typeof(FormatException)), true),
                    KeyInfoTest(KeyInfoTestSet.KeyInfoFullyPopulated),
                    KeyInfoTest(KeyInfoTestSet.MultipleCertificates), 
                    KeyInfoTest(KeyInfoTestSet.MultipleIssuerSerial, new ExpectedException(typeof(XmlReadException), "IDX30015:")),
                    KeyInfoTest(KeyInfoTestSet.MultipleSKI, new ExpectedException(typeof(XmlReadException), "IDX30015:")),
                    KeyInfoTest(KeyInfoTestSet.MultipleSubjectName, new ExpectedException(typeof(XmlReadException), "IDX30015:")),
                    KeyInfoTest(KeyInfoTestSet.MultipleX509Data),
                    KeyInfoTest(KeyInfoTestSet.SingleCertificate),
                    KeyInfoTest(KeyInfoTestSet.SingleIssuerSerial),
                    KeyInfoTest(KeyInfoTestSet.SingleSKI),
                    KeyInfoTest(KeyInfoTestSet.SingleSubjectName),
                    KeyInfoTest(KeyInfoTestSet.WithRSAKeyValue),
                    KeyInfoTest(KeyInfoTestSet.WithWhitespace),
                    KeyInfoTest(KeyInfoTestSet.WithUnknownX509DataElements),
                    KeyInfoTest(KeyInfoTestSet.WithAllElements),
                    KeyInfoTest(KeyInfoTestSet.WithUnknownElements),
                    KeyInfoTest(KeyInfoTestSet.WrongNamespace, new ExpectedException(typeof(XmlReadException), "IDX30011:")),
                    KeyInfoTest(KeyInfoTestSet.KeyInfoEmpty),
                    KeyInfoTest(KeyInfoTestSet.X509DataEmpty, new ExpectedException(typeof(XmlReadException), "IDX30108")),
                    KeyInfoTest(KeyInfoTestSet.IssuerSerialEmpty, new ExpectedException(typeof(XmlReadException), "IDX30011:")),
                    KeyInfoTest(KeyInfoTestSet.RSAKeyValueEmpty, new ExpectedException(typeof(XmlReadException), "IDX30011:"))

                };
            }
        }

        [Theory, MemberData(nameof(WriteKeyInfoTheoryData))]
        public void WriteKeyInfo(DSigSerializerTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.WriteKeyInfo", theoryData);
            var context = new CompareContext($"{this}.WriteKeyInfo, {theoryData.TestId}");
            try
            {
                var keyInfo = theoryData.Serializer.ReadKeyInfo(XmlUtilities.CreateDictionaryReader(theoryData.Xml));
                theoryData.ExpectedException.ProcessNoException(context.Diffs);
                IdentityComparer.AreKeyInfosEqual(keyInfo, theoryData.KeyInfo, context);
                var ms = new MemoryStream();
                var writer = XmlDictionaryWriter.CreateTextWriter(ms);
                theoryData.Serializer.WriteKeyInfo(writer, keyInfo);
                writer.Flush();
                var xml = Encoding.UTF8.GetString(ms.ToArray());
                IdentityComparer.AreEqual(theoryData.Xml, xml);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context.Diffs);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> WriteKeyInfoTheoryData
        {
            get
            {
                return new TheoryData<DSigSerializerTheoryData>
                {
                    KeyInfoTest(KeyInfoTestSet.KeyInfoFullyPopulated),
                };
            }
        }

        public static DSigSerializerTheoryData KeyInfoTest(KeyInfoTestSet keyInfo, ExpectedException expectedException = null, bool first = false)
        {
            return new DSigSerializerTheoryData
            {
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                First = first,
                KeyInfo = keyInfo.KeyInfo,
                TestId = keyInfo.TestId ?? nameof(keyInfo),
                Xml = keyInfo.Xml,
            };
        }

        [Theory, MemberData(nameof(ReadSignatureTheoryData))]
        public void ReadSignature(DSigSerializerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadSignature", theoryData);
            try
            {
                var signature = theoryData.Serializer.ReadSignature(XmlUtilities.CreateDictionaryReader(theoryData.Xml));
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(signature, theoryData.Signature, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> ReadSignatureTheoryData
        {
            get
            {
                var signature = Default.Signature;
                signature.SignedInfo.References[0] = Default.ReferenceWithNullTokenStream;

                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;
                var theoryData = new TheoryData<DSigSerializerTheoryData>
                {
                    new DSigSerializerTheoryData
                    {
                        First = true,
                        Signature = signature,
                        TestId = nameof(Default.Signature),
                        Xml =  XmlGenerator.Generate(Default.Signature),
                    }
                };

                signature = Default.SignatureReferenceWithId;
                signature.SignedInfo.References[0] = Default.ReferenceWithNullTokenStreamAndId;
                theoryData.Add(new DSigSerializerTheoryData
                {
                    Signature = signature,
                    TestId = nameof(Default.SignatureReferenceWithId),
                    Xml = XmlGenerator.Generate(Default.SignatureReferenceWithId),
                });

                signature = Default.Signature;
                signature.SignedInfo.References[0] = Default.ReferenceWithNullTokenStream;
                theoryData.Add(new DSigSerializerTheoryData
                {
                    Signature = signature,
                    TestId = nameof(Default.Signature) + "ReferenceWithoutPrefix",
                    Xml = XmlGenerator.Generate(Default.SignatureReferenceWithoutPrefix),
                });

                signature = Default.Signature;
                signature.SignedInfo.References[0] = Default.ReferenceWithNullTokenStream;
                signature.SignedInfo.References[0].DigestMethod = $"_{SecurityAlgorithms.Sha256Digest}";
                theoryData.Add(new DSigSerializerTheoryData
                {
                    Signature = signature,
                    TestId = "UnknownDigestAlgorithm",
                    Xml = XmlGenerator.Generate(Default.Signature).Replace(SecurityAlgorithms.Sha256Digest, $"_{SecurityAlgorithms.Sha256Digest}")
                });

                signature = Default.Signature;
                signature.SignedInfo.References[0] = Default.ReferenceWithNullTokenStream;
                signature.SignedInfo.SignatureMethod = $"_{SecurityAlgorithms.RsaSha256Signature}";
                theoryData.Add(new DSigSerializerTheoryData
                {
                    Signature = signature,
                    TestId = "UnknownSignatureAlgorithm",
                    Xml = XmlGenerator.Generate(Default.Signature).Replace(SecurityAlgorithms.RsaSha256Signature, $"_{SecurityAlgorithms.RsaSha256Signature}")
                });

                theoryData.Add(new DSigSerializerTheoryData
                {
                    ExpectedException = new ExpectedException(typeof(XmlReadException), "IDX30022:"),
                    Signature = new Signature(),
                    TestId = "EmptySignature",
                    Xml = "<Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"></Signature>"
                });

                return theoryData;
            }
        }

        [Theory, MemberData(nameof(WriteSignatureTheoryData))]
        public void WriteSignature(DSigSerializerTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.WriteSignature", theoryData);
            var context = new CompareContext($"{this}.WriteSignature, {theoryData.TestId}");
            try
            {
                var signature = theoryData.Serializer.ReadSignature(XmlUtilities.CreateDictionaryReader(theoryData.Xml));
                theoryData.ExpectedException.ProcessNoException(context.Diffs);
                IdentityComparer.AreEqual(signature, theoryData.Signature, context);
                var ms = new MemoryStream();
                var writer = XmlDictionaryWriter.CreateTextWriter(ms);
                theoryData.Serializer.WriteSignature(writer, signature);
                writer.Flush();
                var xml = Encoding.UTF8.GetString(ms.ToArray());
                IdentityComparer.AreEqual(theoryData.Xml, xml);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> WriteSignatureTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<DSigSerializerTheoryData>
                {
                    SignatureTest(SignatureTestSet.SignatureFullyPopulated)
                };
            }
        }

        public static DSigSerializerTheoryData SignatureTest(SignatureTestSet testSet, ExpectedException expectedException = null, bool first = false)
        {
            return new DSigSerializerTheoryData
            {
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                First = first,                
                Signature = testSet.Signature,
                TestId = testSet.TestId ?? nameof(testSet),
                Xml = testSet.Xml,
            };
        }

        [Theory, MemberData(nameof(ReadSignedInfoTheoryData))]
        public void ReadSignedInfo(DSigSerializerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadSignedInfo", theoryData);
            try
            {
                var signedInfo = theoryData.Serializer.ReadSignedInfo(XmlUtilities.CreateDictionaryReader(theoryData.Xml));
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreSignedInfosEqual(signedInfo, theoryData.SignedInfo, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> ReadSignedInfoTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                ExpectedException.DefaultVerbose = true;

                return new TheoryData<DSigSerializerTheoryData>
                {
                    SignedInfoTest(SignedInfoTestSet.CanonicalizationMethodMissing,
                        new ExpectedException(typeof(XmlReadException), "IDX30011:"), true),
                    SignedInfoTest(SignedInfoTestSet.MissingDigestMethod,
                        new ExpectedException(typeof(XmlReadException), "IDX30011:")),
                    SignedInfoTest(SignedInfoTestSet.MissingDigestValue,
                        new ExpectedException(typeof(XmlReadException), "IDX30011:")),
                    SignedInfoTest(SignedInfoTestSet.NoTransforms),
                    SignedInfoTest(SignedInfoTestSet.StartsWithWhiteSpace),
                    SignedInfoTest(SignedInfoTestSet.TransformsMissing),
                    SignedInfoTest(SignedInfoTestSet.TwoReferences,
                        new ExpectedException(typeof(XmlReadException), "IDX30020:")),
                    SignedInfoTest(SignedInfoTestSet.ReferenceMissing,
                        new ExpectedException(typeof(XmlReadException), "IDX30011:")),
                    SignedInfoTest(SignedInfoTestSet.ReferenceDigestValueNotBase64),
                    SignedInfoTest(SignedInfoTestSet.UnknownReferenceTransform,
                        new ExpectedException(typeof(XmlReadException), "IDX30210:")),
                    SignedInfoTest(SignedInfoTestSet.Valid),
                    SignedInfoTest(SignedInfoTestSet.SignedInfoEmpty,
                        new ExpectedException(typeof(XmlReadException), "IDX30022:"))
                };
            }
        }

        [Theory, MemberData(nameof(WriteSignedInfoTheoryData))]
        public void WriteSignedInfo(DSigSerializerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WriteSignedInfo", theoryData);
            try
            {
                var signedInfo = theoryData.Serializer.ReadSignedInfo(XmlUtilities.CreateDictionaryReader(theoryData.Xml));
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(signedInfo, theoryData.SignedInfo, context);
                var ms = new MemoryStream();
                var writer = XmlDictionaryWriter.CreateTextWriter(ms);
                theoryData.Serializer.WriteSignedInfo(writer, signedInfo);
                writer.Flush();
                var xml = Encoding.UTF8.GetString(ms.ToArray());
                IdentityComparer.AreEqual(theoryData.Xml, xml, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context.Diffs);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> WriteSignedInfoTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<DSigSerializerTheoryData>
                { 
                    SignedInfoTest(SignedInfoTestSet.SignedInfoFullyPopulated)
                };
            }
        }

        public static DSigSerializerTheoryData SignedInfoTest(SignedInfoTestSet testSet, ExpectedException expectedException = null, bool first = false)
        {
            return new DSigSerializerTheoryData
            {
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                First = first,
                SignedInfo = testSet.SignedInfo,
                TestId = testSet.TestId ?? nameof(testSet),
                Xml = testSet.Xml,
            };
        }

        [Theory, MemberData(nameof(ReadReferenceTheoryData))]
        public void ReadReference(DSigSerializerTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadReference", theoryData);
            var context = new CompareContext($"{this}.ReadReference, {theoryData.TestId}");
            try
            {
                var reference = theoryData.Serializer.ReadReference(XmlUtilities.CreateDictionaryReader(theoryData.Xml));
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(reference, theoryData.Reference, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> ReadReferenceTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                //ExpectedException.DefaultVerbose = true;

                return new TheoryData<DSigSerializerTheoryData>
                {
                    ReferenceTest(ReferenceTestSet.ReferenceEmpty,
                    new ExpectedException(typeof(XmlReadException), "IDX30022:"))
                };
            }
        }

        public static DSigSerializerTheoryData ReferenceTest(ReferenceTestSet testSet, ExpectedException expectedException = null, bool first = false)
        {
            return new DSigSerializerTheoryData
            {
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                First = first,
                Reference = testSet.Reference,
                TestId = testSet.TestId ?? nameof(testSet),
                Xml = testSet.Xml,
            };
        }

        [Theory, MemberData(nameof(ReadTransformsTheoryData))]
        public void ReadTransforms(DSigSerializerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.ReadTransform", theoryData);
            try
            {
                var reference = new Reference();
                var reader = XmlUtilities.CreateDictionaryReader(theoryData.Xml);
                theoryData.Serializer.ReadTransforms(reader, reference);
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(reference.Transforms, theoryData.Transforms, context);
                IdentityComparer.AreEqual(reference.CanonicalizingTransfrom, theoryData.CanonicalizingTransfrom, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> ReadTransformsTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                ExpectedException.DefaultVerbose = true;

                var theoryData = new TheoryData<DSigSerializerTheoryData>
                {
                    TransformsTest(TransformTestSet.AlgorithmNull, new ExpectedException(typeof(XmlReadException), "IDX30105:"), true),
                    TransformsTest(TransformTestSet.AlgorithmUnknown, new ExpectedException(typeof(XmlReadException), "IDX30210:")),
                    TransformsTest(TransformTestSet.C14n_WithComments),
                    TransformsTest(TransformTestSet.C14n_WithComments_WithoutPrefix, new ExpectedException(typeof(XmlReadException), "IDX30016:", typeof(System.Xml.XmlException))),
                    TransformsTest(TransformTestSet.C14n_WithInclusivePrefix),
                    TransformsTest(TransformTestSet.C14n_WithComments_WithNS),
                    TransformsTest(TransformTestSet.C14n_WithoutComments),
                    TransformsTest(TransformTestSet.C14n_WithoutNS, new ExpectedException(typeof(XmlReadException), "IDX30016:", typeof(System.Xml.XmlException))),
                    TransformsTest(TransformTestSet.ElementUnknown, new ExpectedException(typeof(XmlReadException), "IDX30016:", typeof(System.Xml.XmlException))),
                    TransformsTest(TransformTestSet.Enveloped),
                    TransformsTest(TransformTestSet.Enveloped_AlgorithmAttributeMissing, new ExpectedException(typeof(XmlReadException), "IDX30105:")),
                    TransformsTest(TransformTestSet.Enveloped_WithNS),
                    TransformsTest(TransformTestSet.Enveloped_WithoutPrefix, new ExpectedException(typeof(XmlReadException), "IDX30016:", typeof(System.Xml.XmlException))),
                    TransformsTest(TransformTestSet.TransformNull, new ExpectedException(typeof(XmlReadException), "IDX30105:")),
                    TransformsTest(TransformTestSet.MultipleTransforms(6, "6-" + SecurityAlgorithms.ExclusiveC14n, SecurityAlgorithms.ExclusiveC14n, null), new ExpectedException(typeof(XmlReadException), "IDX30029:")),
                    TransformsTest(TransformTestSet.MultipleTransforms(5, "5-" + SecurityAlgorithms.ExclusiveC14n, SecurityAlgorithms.ExclusiveC14n, new ExclusiveCanonicalizationTransform())),
                    TransformsTest(TransformTestSet.MultipleTransforms(6, "6-" + SecurityAlgorithms.EnvelopedSignature, SecurityAlgorithms.EnvelopedSignature, null), new ExpectedException(typeof(XmlReadException), "IDX30029:")),
                    TransformsTest(TransformTestSet.MultipleTransforms(5, "5-" + SecurityAlgorithms.EnvelopedSignature, SecurityAlgorithms.EnvelopedSignature, null))
                };

                // check that upping the min works
                var test = TransformsTest(TransformTestSet.MultipleTransforms(16, "16-" + SecurityAlgorithms.EnvelopedSignature, SecurityAlgorithms.EnvelopedSignature, null));
                test.Serializer.MaximumReferenceTransforms = 16;
                theoryData.Add(test);

                return theoryData;
            }
        }

        public static DSigSerializerTheoryData TransformsTest(TransformTestSet testSet, ExpectedException expectedException = null, bool first = false)
        {
            return new DSigSerializerTheoryData
            {
                CanonicalizingTransfrom = testSet.CanonicalizingTransfrom,
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                First = first,
                TestId = testSet.TestId,
                Transform = testSet.Transform,
                Transforms = new List<Transform> { testSet.Transform },
                Xml = testSet.Xml,
            };
        }

        [Theory, MemberData(nameof(ReadTransformTheoryData))]
        public void ReadTransform(DSigSerializerTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.ReadTransforms", theoryData);
            var context = new CompareContext($"{this}.ReadTransforms, {theoryData.TestId}");
            try
            {
                var reference = new Reference();
                theoryData.Serializer.ReadTransforms(XmlUtilities.CreateDictionaryReader(theoryData.Xml), reference);
                theoryData.ExpectedException.ProcessNoException(context);
                IdentityComparer.AreEqual(reference, theoryData.Reference, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context.Diffs);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> ReadTransformTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<DSigSerializerTheoryData>
                {
                    TransformsTest(TransformsTestSet.TransformsEmpty)
                };
            }
        }

        public static DSigSerializerTheoryData TransformsTest(TransformsTestSet testSet, ExpectedException expectedException = null, bool first = false)
        {
            return new DSigSerializerTheoryData
            {
                ExpectedException = expectedException ?? ExpectedException.NoExceptionExpected,
                First = first,
                Reference = testSet.Reference,
                TestId = testSet.TestId ?? nameof(testSet),
                Transforms = testSet.Transforms,
                Xml = testSet.Xml,
            };
        }

        [Theory, MemberData(nameof(WriteReferenceTheoryData))]
        public void WriteReference(DSigSerializerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.WriteReference", theoryData);

            try
            {
                var ms = new MemoryStream();
                var writer = XmlDictionaryWriter.CreateTextWriter(ms);
                theoryData.Serializer.WriteReference(writer, theoryData.Reference);
                writer.Flush();
                var xml = Encoding.UTF8.GetString(ms.ToArray());

                IdentityComparer.AreEqual(theoryData.Xml, xml, context);

                theoryData.ExpectedException.ProcessNoException();
            }
            catch(Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<DSigSerializerTheoryData> WriteReferenceTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                //ExpectedException.DefaultVerbose = true;

                return new TheoryData<DSigSerializerTheoryData>
                {
                    ReferenceTest(ReferenceTestSet.ReferenceWithId),
                    ReferenceTest(ReferenceTestSet.ReferenceWithIdAndUri),
                    ReferenceTest(ReferenceTestSet.ReferenceWithIdAndUriWithoutPrefix)
                };
            }
        }
    }

    public class DSigSerializerTheoryData : TheoryDataBase
    {
        public string Algorithm
        {
            get;
            set;
        }

        public CanonicalizingTransfrom CanonicalizingTransfrom
        {
            get;
            set;
        }

        public string ElementName
        {
            get;
            set;
        }

        public HashAlgorithm HashAlgorithm
        {
            get;
            set;
        }

        public bool IncludeComments
        {
            get;
            set;
        }

        public bool IsCanonicalizationMethod
        {
            get;
            set;
        }

        public KeyInfo KeyInfo
        {
            get;
            set;
        }

        public SecurityKey SecurityKey
        {
            get;
            set;
        }

        public DSigSerializer Serializer
        {
            get;
            set;
        } = new DSigSerializer();

        public Signature Signature
        {
            get;
            set;
        }

        public SignedInfo SignedInfo
        {
            get;
            set;
        }

        public Reference Reference
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"'{TestId}', '{ExpectedException}'";
        }

        public Transform Transform
        {
            get;
            set;
        }

        public List<Transform> Transforms
        {
            get;
            set;
        }

        public string Xml
        {
            get;
            set;
        }

        public XmlTokenStream XmlTokenStream
        {
            get;
            set;
        }

        public XmlWriter XmlWriter
        {
            get;
            set;
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
