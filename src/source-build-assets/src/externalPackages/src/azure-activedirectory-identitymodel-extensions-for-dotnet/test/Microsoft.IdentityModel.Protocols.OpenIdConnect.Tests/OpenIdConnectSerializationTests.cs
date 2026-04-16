// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens.Json.Tests;
using Xunit;

namespace Microsoft.IdentityModel.Protocols.OpenIdConnect.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenIdConnectSerializationTests
    {
        [Theory, MemberData(nameof(DesrializeTheoryData), DisableDiscoveryEnumeration = true)]
        public void Deserialize(OpenIdConnectTheoryData theoryData)
        {
            var context = TestUtilities.WriteHeader($"{this}.Deserialize", theoryData);

            try
            {
                OpenIdConnectConfiguration configuration = new OpenIdConnectConfiguration(theoryData.Json);
                OpenIdConnectConfiguration configurationUpperCase = new OpenIdConnectConfiguration(JsonUtilities.SetPropertiesToUpperCase(theoryData.Json));
                theoryData.ExpectedException.ProcessNoException(context);

                IdentityComparer.AreEqual(configuration, theoryData.CompareTo, context);
                IdentityComparer.AreEqual(configurationUpperCase, theoryData.CompareTo, context);
            }
            catch (Exception ex)
            {
                theoryData.ExpectedException.ProcessException(ex, context);
            }

            TestUtilities.AssertFailIfErrors(context);
        }

        public static TheoryData<OpenIdConnectTheoryData> DesrializeTheoryData
        {
            get
            {
                TheoryData<OpenIdConnectTheoryData> theoryData = new TheoryData<OpenIdConnectTheoryData>();

                // the reason to replace AdditionalData with upper case is because the test deserializes uppercase and lowercase.
                // we wanted to leave the data sets in original form from discovery to be used in other tests.
                theoryData.Add(new OpenIdConnectTheoryData("AADCommonV1")
                {
                    CompareTo = JsonUtilities.SetAdditionalDataKeysToUpperCase(OpenIdConfigData.AADCommonV1Config),
                    Json = JsonUtilities.SetAdditionalDataKeysToUpperCase(OpenIdConfigData.AADCommonV1Json, OpenIdConfigData.AADCommonV1Config)
                });

                theoryData.Add(new OpenIdConnectTheoryData("AADCommonV2")
                {
                    CompareTo = JsonUtilities.SetAdditionalDataKeysToUpperCase(OpenIdConfigData.AADCommonV2Config),
                    Json = JsonUtilities.SetAdditionalDataKeysToUpperCase(OpenIdConfigData.AADCommonV2Json, OpenIdConfigData.AADCommonV2Config)
                });

                theoryData.Add(new OpenIdConnectTheoryData("AccountsGoogleCom")
                {
                    CompareTo = OpenIdConfigData.AccountsGoogleComConfig,
                    Json = OpenIdConfigData.AccountsGoogleComJson
                });

                theoryData.Add(new OpenIdConnectTheoryData("FrontChannelFalse")
                {
                    CompareTo = OpenIdConfigData.FrontChannelFalseConfig,
                    Json = OpenIdConfigData.FrontChannelFalse
                });

                theoryData.Add(new OpenIdConnectTheoryData("FrontChannelTrue")
                {
                    CompareTo = OpenIdConfigData.FrontChannelTrueConfig,
                    Json = OpenIdConfigData.FrontChannelTrue
                });

                theoryData.Add(new OpenIdConnectTheoryData("ArrayFirst")
                {
                    CompareTo = OpenIdConfigData.ArraysConfig,
                    Json = OpenIdConfigData.ArrayFirstObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("ArrayMiddle")
                {
                    CompareTo = OpenIdConfigData.ArraysConfig,
                    Json = OpenIdConfigData.ArrayMiddleObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("ArrayLast")
                {
                    CompareTo = OpenIdConfigData.ArraysConfig,
                    Json = OpenIdConfigData.ArrayLastObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("ObjectFirst")
                {
                    CompareTo = OpenIdConfigData.ObjectConfig,
                    Json = OpenIdConfigData.ObjectFirstObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("ObjectMiddle")
                {
                    CompareTo = OpenIdConfigData.ObjectConfig,
                    Json = OpenIdConfigData.ObjectMiddleObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("ObjectLast")
                {
                    CompareTo = OpenIdConfigData.ObjectConfig,
                    Json = OpenIdConfigData.ObjectLastObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("Duplicates")
                {
                    CompareTo = OpenIdConfigData.DuplicatesConfig,
                    Json = OpenIdConfigData.Duplicates
                });

                theoryData.Add(new OpenIdConnectTheoryData("String")
                {
                    CompareTo = OpenIdConfigData.StringConfig,
                    Json = JsonData.StringObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("BoolFalse")
                {
                    CompareTo = OpenIdConfigData.BoolFalseConfig,
                    Json = JsonData.FalseObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("BoolTrue")
                {
                    CompareTo = OpenIdConfigData.BoolTrueConfig,
                    Json = JsonData.TrueObject
                });

                theoryData.Add(new OpenIdConnectTheoryData("Null")
                {
                    CompareTo = OpenIdConfigData.NullConfig,
                    Json = JsonData.NullObject
                });

                return theoryData;
            }
        }
    }
}
