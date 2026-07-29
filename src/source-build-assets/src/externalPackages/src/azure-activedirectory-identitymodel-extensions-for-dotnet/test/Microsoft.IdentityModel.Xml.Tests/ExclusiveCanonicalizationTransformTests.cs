// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Xml;
using Xunit;

#pragma warning disable CS3016 // Arrays as attribute arguments is not CLS-compliant

namespace Microsoft.IdentityModel.Tokens.Xml.Tests
{
    public class ExclusiveCanonicalizationTransformTests
    {
        [Fact]
        public void GetSets()
        {
            var type = typeof(ExclusiveCanonicalizationTransform);
            var properties = type.GetProperties();
            Assert.True(properties.Length == 3, $"Number of properties has changed from 3 to: {properties.Length}, adjust tests");

            var context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("Algorithm", new List<object>{SecurityAlgorithms.ExclusiveC14n, SecurityAlgorithms.ExclusiveC14nWithComments, SecurityAlgorithms.ExclusiveC14n }),
                    new KeyValuePair<string, List<object>>("IncludeComments", new List<object>{false, true}),
                    new KeyValuePair<string, List<object>>("InclusiveNamespacesPrefixList", new List<object>{(string)null, "saml dsig wsfed", "saml2 dsig2 wsfed2" })
                },
                Object = new ExclusiveCanonicalizationTransform(false),
            };

            TestUtilities.GetSet(context);

            context = new GetSetContext
            {
                PropertyNamesAndSetGetValue = new List<KeyValuePair<string, List<object>>>
                {
                    new KeyValuePair<string, List<object>>("Algorithm", new List<object>{SecurityAlgorithms.ExclusiveC14nWithComments, SecurityAlgorithms.ExclusiveC14n, SecurityAlgorithms.ExclusiveC14nWithComments }),
                    new KeyValuePair<string, List<object>>("IncludeComments", new List<object>{true, false, true}),
                    new KeyValuePair<string, List<object>>("InclusiveNamespacesPrefixList", new List<object>{(string)null, "saml dsig wsfed", "saml2 dsig2 wsfed2" })
                },
                Object = new ExclusiveCanonicalizationTransform(true),
            };

            TestUtilities.GetSet(context);

            TestUtilities.AssertFailIfErrors($"{this}.GetSets", context.Errors);
        }

        [Theory, MemberData(nameof(ConstructorTheoryData), DisableDiscoveryEnumeration = true)]
        public void Constructor(ExclusiveCanonicalizationTransformTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}.Constructor", theoryData);
            var transform = new ExclusiveCanonicalizationTransform(theoryData.IncludeComments);

            Assert.Equal(transform.IncludeComments, theoryData.IncludeComments);
        }

        public static TheoryData<ExclusiveCanonicalizationTransformTheoryData> ConstructorTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<ExclusiveCanonicalizationTransformTheoryData>
                {
                    new ExclusiveCanonicalizationTransformTheoryData
                    {
                        First = true,
                        IncludeComments = false,
                        TestId = "ExclusiveC14n, , IncludeComments : true"
                    },
                    new ExclusiveCanonicalizationTransformTheoryData
                    {
                        IncludeComments = true,
                        TestId = "ExclusiveC14n, IncludeComments : true"
                    }
                };
            }
        }

        [Theory, MemberData(nameof(ProcessAndDigestTheoryData), DisableDiscoveryEnumeration = true)]
        public void ProcessAndDigest(ExclusiveCanonicalizationTransformTheoryData theoryData)
        {
            TestUtilities.WriteHeader($"{this}", "ProcessAndDigest", true);
            var context = new CompareContext($"{this}.ProcessAndDigest, {theoryData.TestId}");
            try
            {
                theoryData.Transform.ProcessAndDigest(theoryData.XmlTokenStream, theoryData.HashAlgorithm);
                theoryData.ExpectedException.ProcessNoException(context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<ExclusiveCanonicalizationTransformTheoryData> ProcessAndDigestTheoryData
        {
            get
            {
                // uncomment to view exception displayed to user
                // ExpectedException.DefaultVerbose = true;

                return new TheoryData<ExclusiveCanonicalizationTransformTheoryData>
                {
                    new ExclusiveCanonicalizationTransformTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("tokenStream"),
                        First = true,
                        HashAlgorithm = SHA256.Create(),
                        TestId = "TokenStream null",
                        Transform = new ExclusiveCanonicalizationTransform(false)
                    },
                    new ExclusiveCanonicalizationTransformTheoryData
                    {
                        ExpectedException = ExpectedException.ArgumentNullException("hash"),
                        Transform = new ExclusiveCanonicalizationTransform(false),
                        TestId = "hash null",
                        XmlTokenStream = XmlUtilities.CreateXmlTokenStream(Default.OuterXml)
                    },
                    new ExclusiveCanonicalizationTransformTheoryData
                    {
                        HashAlgorithm = SHA256.Create(),
                        Transform = new ExclusiveCanonicalizationTransform(false),
                        TestId = "reader, hash set",
                        XmlTokenStream = XmlUtilities.CreateXmlTokenStream(Default.OuterXml)
                    }
                };
            }
        }
    }

    public class ExclusiveCanonicalizationTransformTheoryData : TheoryDataBase
    {
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

        public override string ToString()
        {
            return $"'{TestId}', '{ExpectedException}'";
        }

        public ExclusiveCanonicalizationTransform Transform
        {
            get;
            set;
        }

        public XmlTokenStream XmlTokenStream
        {
            get;
            set;
        }
    }
}

#pragma warning restore CS3016 // Arrays as attribute arguments is not CLS-compliant
