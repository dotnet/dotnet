// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json.Linq;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    public class JsonUtilities
    {
        static string EmptyHeader = Base64UrlEncoder.Encode("{}");

        static IList<object> _arrayDataAsObjectList = new List<object> { "value1", "value2" };
        static string _arrayData = @"[""value1"",""value2""]";
        static string _objectData = @"{""Object"":""string""}";
        static string DP = "ErP3OpudePAY3uGFSoF16Sde69PnOra62jDEZGnPx_v3nPNpA5sr-tNc8bQP074yQl5kzSFRjRlstyW0TpBVMP0ocbD8RsN4EKsgJ1jvaSIEoP87OxduGkim49wFA0Qxf_NyrcYUnz6XSidY3lC_pF4JDJXg5bP_x0MUkQCTtQE";
        static string DQ = "YbBsthPt15Pshb8rN8omyfy9D7-m4AGcKzqPERWuX8bORNyhQ5M8JtdXcu8UmTez0j188cNMJgkiN07nYLIzNT3Wg822nhtJaoKVwZWnS2ipoFlgrBgmQiKcGU43lfB5e3qVVYUebYY0zRGBM1Fzetd6Yertl5Ae2g2CakQAcPs";
        static string Exponent = "AQAB";
        static string InverseQ = "lbljWyVY-DD_Zuii2ifAz0jrHTMvN-YS9l_zyYyA_Scnalw23fQf5WIcZibxJJll5H0kNTIk8SCxyPzNShKGKjgpyZHsJBKgL3iAgmnwk6k8zrb_lqa0sd1QWSB-Rqiw7AqVqvNUdnIqhm-v3R8tYrxzAqkUsGcFbQYj4M5_F_4";
        static string Modulus = "6-FrFkt_TByQ_L5d7or-9PVAowpswxUe3dJeYFTY0Lgq7zKI5OQ5RnSrI0T9yrfnRzE9oOdd4zmVj9txVLI-yySvinAu3yQDQou2Ga42ML_-K4Jrd5clMUPRGMbXdV5Rl9zzB0s2JoZJedua5dwoQw0GkS5Z8YAXBEzULrup06fnB5n6x5r2y1C_8Ebp5cyE4Bjs7W68rUlyIlx1lzYvakxSnhUxSsjx7u_mIdywyGfgiT3tw0FsWvki_KYurAPR1BSMXhCzzZTkMWKE8IaLkhauw5MdxojxyBVuNY-J_elq-HgJ_dZK6g7vMNvXz2_vT-SykIkzwiD9eSI9UWfsjw";
        static string P = "_avCCyuo7hHlqu9Ec6R47ub_Ul_zNiS-xvkkuYwW-4lNnI66A5zMm_BOQVMnaCkBua1OmOgx7e63-jHFvG5lyrhyYEmkA2CS3kMCrI-dx0fvNMLEXInPxd4np_7GUd1_XzPZEkPxBhqf09kqryHMj_uf7UtPcrJNvFY-GNrzlJk";
        static string Q = "7gvYRkpqM-SC883KImmy66eLiUrGE6G6_7Y8BS9oD4HhXcZ4rW6JJKuBzm7FlnsVhVGro9M-QQ_GSLaDoxOPQfHQq62ERt-y_lCzSsMeWHbqOMci_pbtvJknpMv4ifsQXKJ4Lnk_AlGr-5r5JR5rUHgPFzCk9dJt69ff3QhzG2c";
        static string P256_D = "OOX7PnYlSTE41BSclDj5Gi_sx_SPgEqStjY3doku4TQ";
        static string P256_X = "luR290c8sXxbOGhNquQ3J3rh763Os4D609cHK-L_5fA";
        static string P256_Y = "tUqUwtaVHwc7_CXnuBrCpMQTF5BJKdFnw9_JkSIXWpQ";
        static string X5C = "MIIDPjCCAiqgAwIBAgIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAMC0xKzApBgNVBAMTImFjY291bnRzLmFjY2Vzc2NvbnRyb2wud2luZG93cy5uZXQwHhcNMTQwMTAxMDcwMDAwWhcNMTYwMTAxMDcwMDAwWjAtMSswKQYDVQQDEyJhY2NvdW50cy5hY2Nlc3Njb250cm9sLndpbmRvd3MubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAkSCWg6q9iYxvJE2NIhSyOiKvqoWCO2GFipgH0sTSAs5FalHQosk9ZNTztX0ywS/AHsBeQPqYygfYVJL6/EgzVuwRk5txr9e3n1uml94fLyq/AXbwo9yAduf4dCHTP8CWR1dnDR+Qnz/4PYlWVEuuHHONOw/blbfdMjhY+C/BYM2E3pRxbohBb3x//CfueV7ddz2LYiH3wjz0QS/7kjPiNCsXcNyKQEOTkbHFi3mu0u13SQwNddhcynd/GTgWN8A+6SN1r4hzpjFKFLbZnBt77ACSiYx+IHK4Mp+NaVEi5wQtSsjQtI++XsokxRDqYLwus1I1SihgbV/STTg5enufuwIDAQABo2IwYDBeBgNVHQEEVzBVgBDLebM6bK3BjWGqIBrBNFeNoS8wLTErMCkGA1UEAxMiYWNjb3VudHMuYWNjZXNzY29udHJvbC53aW5kb3dzLm5ldIIQsRiM0jheFZhKk49YD0SK1TAJBgUrDgMCHQUAA4IBAQCJ4JApryF77EKC4zF5bUaBLQHQ1PNtA1uMDbdNVGKCmSf8M65b8h0NwlIjGGGy/unK8P6jWFdm5IlZ0YPTOgzcRZguXDPj7ajyvlVEQ2K2ICvTYiRQqrOhEhZMSSZsTKXFVwNfW6ADDkN3bvVOVbtpty+nBY5UqnI7xbcoHLZ4wYD251uj5+lo13YLnsVrmQ16NCBYq2nQFNPuNJw6t3XUbwBHXpF46aLT1/eGf/7Xx6iy8yPJX4DyrpFTutDz882RWofGEO5t4Cw+zZg70dJ/hH/ODYRMorfXEW+8uKmXMKmX2wyxMKvfiPbTy5LmAU8Jvjs2tLg4rOBcXWLAIarZ";

        public static JsonElement? CreateJsonElement(string json)
        {
            Utf8JsonReader reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json).AsSpan());
            JsonElement? jsonElement;

#if NET6_0_OR_GREATER
            bool ret = JsonElement.TryParseValue(ref reader, out jsonElement);
#else
            using (JsonDocument jsonDocument = JsonDocument.ParseValue(ref reader))
                jsonElement = jsonDocument.RootElement.Clone();
#endif
            return jsonElement;
        }

        public static JsonWebKey FullyPopulatedJsonWebKey()
        {
            JsonWebKey jsonWebKey = new JsonWebKey
            {
                Alg = SecurityAlgorithms.Sha256,
                Crv = "CRV",
                D = P256_D,
                DP = DP,
                DQ = DQ,
                E = Exponent,
                K = "K",
                KeyId = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                Kid = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                Kty = "RSA",
                N = Modulus,
                P = P,
                Q = Q,
                QI = InverseQ,
                Use = "sig",
                X = P256_X,
                X5t = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                X5tS256 = "x5t256",
                X5u = "https://jsonkeyurl",
                Y = P256_Y
            };

            jsonWebKey.X5c.Add(X5C);
            jsonWebKey.KeyOps.Add("keyOps");
            jsonWebKey.Oth.Add("oth1");
            jsonWebKey.Oth.Add("oth2");
            SetAdditionalData(jsonWebKey.AdditionalData);

            return jsonWebKey;
        }

        public static JsonWebKey6x FullyPopulatedJsonWebKey6x()
        {
            JsonWebKey6x jsonWebKey = new JsonWebKey6x
            {
                Alg = SecurityAlgorithms.Sha256,
                Crv = "CRV",
                D = P256_D,
                DP = DP,
                DQ = DQ,
                E = Exponent,
                K = "K",
                KeyId = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                Kid = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                Kty = "RSA",
                N = Modulus,
                P = P,
                Q = Q,
                QI = InverseQ,
                Use = "sig",
                X = P256_X,
                X5t = "NGTFvdK-fythEuLwjpwAJOM9n-A",
                X5tS256 = "x5t256",
                X5u = "https://jsonkeyurl",
                Y = P256_Y
            };

            jsonWebKey.X5c.Add(X5C);
            jsonWebKey.KeyOps.Add("keyOps");
            jsonWebKey.Oth = new List<string>
            {
                "oth1",
                "oth2"
            };

            SetAdditionalData6x(jsonWebKey.AdditionalData);

            return jsonWebKey;
        }

        public static JsonWebKeySet FullyPopulatedJsonWebKeySet()
        {
            JsonWebKeySet jsonWebKeySet = new JsonWebKeySet();
            jsonWebKeySet.Keys.Add(FullyPopulatedJsonWebKey());
            SetAdditionalData(jsonWebKeySet.AdditionalData);

            return jsonWebKeySet;
        }

        public static JsonWebKeySet6x FullyPopulatedJsonWebKeySet6x()
        {
            JsonWebKeySet6x jsonWebKeySet = new JsonWebKeySet6x();
            jsonWebKeySet.Keys.Add(FullyPopulatedJsonWebKey6x());
            SetAdditionalData6x(jsonWebKeySet.AdditionalData);

            return jsonWebKeySet;
        }

        public static void SetAdditionalData(IDictionary<string, object> dictionary, string key = null, object obj = null)
        {
            SetAdditionalDataNumbers(dictionary);
            SetAdditionalDataValues(dictionary);
            dictionary["Object"] = CreateJsonElement(_objectData);
            dictionary["Array"] = CreateJsonElement(_arrayData);
            if (key != null)
                dictionary[key] = obj;
        }

        public static void SetAdditionalData6x(IDictionary<string, object> dictionary, string key = null, object obj = null)
        {
            SetAdditionalDataNumbers(dictionary);
            SetAdditionalDataValues(dictionary);
            dictionary["Object"] = JObject.Parse(_objectData);
            dictionary["Array"] = JArray.Parse(_arrayData);
            if (key != null)
                dictionary[key] = obj;
        }

        public static void SetAdditionalDataNumbers(IDictionary<string, object> dictionary)
        {
            dictionary["int"] = (int)1;
            dictionary["long"] = (long)1234567890123456;
        }

        public static void SetAdditionalDataValues(IDictionary<string, object> dictionary)
        {
            dictionary["string"] = "string";
            dictionary["false"] = false;
            dictionary["true"] = true;
        }

        public static string CreateUnsignedToken(string key, object value)
        {
            return EmptyHeader + "." + CreateEncodedJson(key, value) + ".";
        }

        public static string CreateUnsignedToken(string headerKey, object headerValue, string payloadKey, object payloadValue)
        {
            return CreateEncodedJson(headerKey, headerValue) + "." + CreateEncodedJson(payloadKey, payloadValue) + ".";
        }

        public static string CreateEncodedJson(string key, object value)
        {
            Utf8JsonWriter writer = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                try
                {
                    writer = new Utf8JsonWriter(memoryStream);
                    writer.WriteStartObject();

                    JsonSerializerPrimitives.WriteObject(ref writer, key, value);

                    writer.WriteEndObject();
                    writer.Flush();

                    return Base64UrlEncoder.Encode(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                }
                finally
                {
                    writer?.Dispose();
                }
            }
        }

        public static string SetPropertiesToUpperCase(string json)
        {
            Utf8JsonReader reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    if (propertyName != null)
                    {
                        json = json.Replace("\"" + propertyName + "\":", "\"" + propertyName.ToUpperInvariant() + "\":");
                    }
                }
            }

            return json;
        }

        /// <summary>
        /// json is used in a test to create the OpenIdConnectConfiguration, all values found in the json that match keys in AdditionalData are set to upper case.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static string SetAdditionalDataKeysToUpperCase(string json, OpenIdConnectConfiguration configuration)
        {
            foreach (string key in configuration.AdditionalData.Keys)
                json = json.Replace("\"" + key + "\":", "\"" + key.ToUpperInvariant() + "\":");

            return json;
        }

        public static OpenIdConnectConfiguration SetAdditionalDataKeysToUpperCase(OpenIdConnectConfiguration configuration)
        {
            SetAdditionalDataKeysToUpperCase(configuration.AdditionalData);
            return configuration;
        }

        public static JsonWebKeySet SetAdditionalDataKeysToUpperCase(JsonWebKeySet jsonWebKeySet)
        {
            SetAdditionalDataKeysToUpperCase(jsonWebKeySet.AdditionalData);
            foreach (JsonWebKey jsonWebKey in jsonWebKeySet.Keys)
                SetAdditionalDataKeysToUpperCase(jsonWebKey);

            return jsonWebKeySet;
        }

        public static JsonWebKey SetAdditionalDataKeysToUpperCase(JsonWebKey jsonWebKey)
        {
            SetAdditionalDataKeysToUpperCase(jsonWebKey.AdditionalData);
            return jsonWebKey;
        }

        /// <summary>
        /// json is used to create the JsonWebKeySet, all values found in the json that match keys in AdditionalData are set to upper case.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="jsonWebKeySet"></param>
        /// <returns></returns>
        public static string SetAdditionalDataKeysToUpperCase(string json, JsonWebKeySet jsonWebKeySet)
        {
            foreach (string key in jsonWebKeySet.AdditionalData.Keys)
                json = json.Replace("\"" + key + "\":", "\"" + key.ToUpperInvariant() + "\":");

            foreach (JsonWebKey jsonWebKey in jsonWebKeySet.Keys)
                json = SetAdditionalDataKeysToUpperCase(json, jsonWebKey);

            return json;
        }

        /// <summary>
        /// json is used to create the JsonWebKey, all values found in the json that match keys in AdditionalData are set to upper case.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="jsonWebKey"></param>
        /// <returns></returns>
        public static string SetAdditionalDataKeysToUpperCase(string json, JsonWebKey jsonWebKey)
        {
            foreach (string key in jsonWebKey.AdditionalData.Keys)
                json = json.Replace("\"" + key + "\":", "\"" + key.ToUpperInvariant() + "\":");

            return json;
        }

        public static void SetAdditionalDataKeysToUpperCase(IDictionary<string,object> additionalData)
        {
            List<string> keys = [.. additionalData.Keys];

            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                additionalData[key.ToUpperInvariant()] = additionalData[key];
                additionalData.Remove(key);
            }
        }
    }
}
