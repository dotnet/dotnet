// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests.Static
{
    /// <summary>
    /// Tests that we can set statics.
    /// </summary>
    [Collection("Sequential")]
    public class StaticCryptoProviderTests
    {
        /// <summary>
        /// Tests that we can set the default <see cref="CompressionProviderFactory"/>.
        /// </summary>
        /// <param name="theoryData"></param>
        [Theory, MemberData(nameof(CompressionFactoryTestsTheoryData))]
        public void CompressionFactoryTests(CompressionProviderFactoryTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.CompressionFactoryTests", theoryData);
            CompressionProviderFactory originalCompressionProviderFactory = CompressionProviderFactory.Default;

            try
            {
                CompressionProviderFactory.Default = theoryData.CompressionProviderFactory;
                theoryData.ExpectedException.ProcessNoException(context);
                var isSupported = CompressionProviderFactory.Default.IsSupportedAlgorithm(theoryData.Algorithm);
                if (isSupported != theoryData.IsSupported)
                    context.AddDiff($"isSupported: {isSupported}, theoryData.IsSupported: {theoryData.IsSupported}");
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }
            finally
            {
                CompressionProviderFactory.Default = originalCompressionProviderFactory;
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<CompressionProviderFactoryTheoryData> CompressionFactoryTestsTheoryData
        {
            get
            {
                var theoryData = new TheoryData<CompressionProviderFactoryTheoryData>();

                theoryData.Add(new CompressionProviderFactoryTheoryData("FirstDefaultCompressionFactoryTrue")
                {
                    Algorithm = CompressionAlgorithms.Deflate,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    IsSupported = true
                });

                theoryData.Add(new CompressionProviderFactoryTheoryData("CustomCompressionFactoryIsSupportedReturnsFalse")
                {
                    Algorithm = CompressionAlgorithms.Deflate,
                    CompressionProviderFactory = new CustomCompressionProviderFactory(),
                    IsSupported = false
                });

                theoryData.Add(new CompressionProviderFactoryTheoryData("SecondDefaultCompressionFactoryTrue")
                {
                    Algorithm = CompressionAlgorithms.Deflate,
                    CompressionProviderFactory = CompressionProviderFactory.Default,
                    IsSupported = true
                });

                theoryData.Add(new CompressionProviderFactoryTheoryData("ThirdDefaultCompressionFactoryFalse")
                {
                    Algorithm = "NotSupported",
                    CompressionProviderFactory = new CustomCompressionProviderFactory(),
                    IsSupported = false
                });

                return theoryData;
            }
        }
    }

    public class CustomCompressionProviderFactory : CompressionProviderFactory
    {
        public override bool IsSupportedAlgorithm(string algorithm)
        {
            return false;
        }
    }

    public class CompressionProviderFactoryTheoryData : TheoryDataBase
    {
        public CompressionProviderFactoryTheoryData(string testId) : base(testId)
        {
        }

        public CompressionProviderFactory CompressionProviderFactory { get; set; }

        public string Algorithm { get; set; }

        public bool IsSupported { get; set; }
    }
}
