// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.IdentityModel.Tokens.Json.Tests
{
    /// <summary>
    /// Properties that will show up in AdditionalData
    /// </summary>
    public static class JsonData
    {
        // Create a unique string for each property, to avoid collisions
        // Moved these to UpperInvariant so test that perform uppercase to lowercase will work.
        public static string ArrayProperty = Guid.NewGuid().ToString().ToUpperInvariant();
        public static string ObjectProperty = Guid.NewGuid().ToString().ToUpperInvariant();
        public static string FalseProperty = Guid.NewGuid().ToString().ToUpperInvariant();
        public static string TrueProperty = Guid.NewGuid().ToString().ToUpperInvariant();
        public static string StringProperty = Guid.NewGuid().ToString().ToUpperInvariant();
        public static string StringValue = Guid.NewGuid().ToString().ToUpperInvariant();
        public static string NullProperty = Guid.NewGuid().ToString().ToUpperInvariant();

        // Json strings are name:value (claim) pairs inside an object
        // The naming here is:
        // Value - the value of the claim, this is used to create JsonUtilities.CreateJsonElement, to create the expected value in AdditionalData.
        // Claim - the name:value pair, that can be inserted into a Json object: string jsonboject =  $$"""{{{ArrayClaim}}}""";
        // Object - the Claim is wrapped inside an object, simplifies wrting tests that read the json,
        // otherwise each test would have to write: string jsonString =  $$"""{{{ArrayClaim}}}""";
        public static string ArrayStrings=
            """
            "arrayValue", "arrayValue"
            """;

        public static string ArrayValue =
            $$"""
            [{{ArrayStrings}}]
            """;

        public static string ArrayClaim =
            $"""
            "{ArrayProperty}":{ArrayValue}
            """;

        public static string ArrayObject =
            $$"""
            {{{ArrayClaim}}}
            """;

        public static string ObjectValue =
            $$"""
            {"OBJECT":["ObjectValue1","ObjectValue2"]}
            """;

        public static string ObjectClaim =
            $$"""
            "{{ObjectProperty}}":{{ObjectValue}}
            """;

        public static string ObjectObject =
            $$"""
            {{{ObjectClaim}}}
            """;

        public static string ObjectValue2 =
            $$"""
            {"object2":["ObjectValue21","ObjectValue22"]}
            """;

        public static string ArrayOfObjectsValue =
            $"""
            [{ObjectValue}, {ObjectValue2}]
            """;

        public static string ArrayOfObjectsClaim =
            $$"""
            "{{ArrayProperty}}":{{ArrayOfObjectsValue}}
            """;

        public static string ArrayOfObjectsObject =
            $$"""
            {{{ArrayOfObjectsClaim}}}
            """;

        public static string StringClaim =
            $$"""
            "{{StringProperty}}":"{{StringValue}}"
            """;

        public static string StringObject =
            $$"""
            {{{StringClaim}}}
            """;

        public static string TrueClaim =
            $$"""
            "{{TrueProperty}}":true
            """;

        public static string TrueObject =
            $$"""
            {{{TrueClaim}}}
            """;

        public static string FalseClaim =
            $$"""
            "{{FalseProperty}}" : false
            """;

        public static string FalseObject =
            $$"""
            {{{FalseClaim}}}
            """;

        public static string NullClaim =
            $$"""
            "{{NullProperty}}" : null
            """;

        public static string NullObject =
            $$"""
            {{{NullClaim}}}
            """;
    }
}
