// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Xunit;

namespace Microsoft.IdentityModel.Tokens.Tests
{
    public class BaseConfigurationComparerTests
    {
        [Theory, MemberData(nameof(ComparerTheoryData))]
        public void Compare(BaseConfigurationComparerTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Compare", theoryData);
            var comparer = new BaseConfigurationComparer();
            var result = comparer.Equals(theoryData.ConfigurationA, theoryData.ConfigurationB);
            Assert.Equal(theoryData.ShouldBeEqual, result);
        }

        public static TheoryData<BaseConfigurationComparerTheoryData> ComparerTheoryData()
        {
            var jsonWebKey1 = new TestConfiguration
            {
                Issuer = "http://example.com/issuer/1",
            };

            jsonWebKey1.SigningKeys.Add(DataSets.JsonWebKey1);

            var jsonWebKey2 = new TestConfiguration
            {
                Issuer = "http://example.com/issuer/1",
            };

            jsonWebKey2.SigningKeys.Add(DataSets.JsonWebKey2);

            var bothJsonWebKeys = new TestConfiguration
            {
                Issuer = "http://example.com/issuer/1",
            };

            bothJsonWebKeys.SigningKeys.Add(DataSets.JsonWebKey1);
            bothJsonWebKeys.SigningKeys.Add(DataSets.JsonWebKey2);

            return new TheoryData<BaseConfigurationComparerTheoryData>()
            {
                new()
                {
                    TestId = "Both null",
                    ConfigurationA = null,
                    ConfigurationB = null,
                    ShouldBeEqual = true,
                },
                new()
                {
                    TestId = "B null",
                    ConfigurationA = new TestConfiguration(),
                    ConfigurationB = null,
                    ShouldBeEqual = false,
                },
                new()
                {
                    TestId = "A null",
                    ConfigurationA = null,
                    ConfigurationB = new TestConfiguration(),
                    ShouldBeEqual = false,
                },
                new()
                {
                    TestId = "Issuer mismatched",
                    ConfigurationA = new TestConfiguration
                    {
                        Issuer = "http://example.com/issuer/1"
                    },
                    ConfigurationB = new TestConfiguration
                    {
                        Issuer = "http://example.com/issuer/2"
                    },
                    ShouldBeEqual = false,
                },
                new()
                {
                    TestId = "No Keys",
                    ConfigurationA = new TestConfiguration
                    {
                        Issuer = "http://example.com/issuer/1",
                    },
                    ConfigurationB = new TestConfiguration
                    {
                        Issuer = "http://example.com/issuer/1"
                    },
                    ShouldBeEqual = true,
                },
                new()
                {
                    TestId = "different keys",
                    ConfigurationA = jsonWebKey1,
                    ConfigurationB = jsonWebKey2,
                    ShouldBeEqual = false,
                },
                new()
                {
                    TestId = "same keys",
                    ConfigurationA = jsonWebKey1,
                    ConfigurationB = jsonWebKey1,
                    ShouldBeEqual = true,
                },
                new ()
                {
                    TestId = "different number of keys",
                    ConfigurationA = jsonWebKey1,
                    ConfigurationB = bothJsonWebKeys,
                    ShouldBeEqual = false,
                },
            };
        }
    }

    public class BaseConfigurationComparerTheoryData : TheoryDataBase
    {
        public BaseConfiguration ConfigurationA { get; set; }

        public BaseConfiguration ConfigurationB { get; set; }

        public bool ShouldBeEqual { get; set; }
    }

    public class TestConfiguration : BaseConfiguration
    {
    }
}
