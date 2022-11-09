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

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// Constants for Json Web tokens.
    /// </summary>
    public static class JsonClaimValueTypes
    {
        /// <summary>
        /// A URI that represents the JSON XML data type.
        /// </summary>
        /// <remarks>When mapping json to .Net Claim(s), if the value was not a string (or an enumeration of strings), the ClaimValue will serialized using the current JSON serializer, a property will be added with the .Net type and the ClaimTypeValue will be set to 'JsonClaimValueType'.</remarks>
        public const string Json = Microsoft.IdentityModel.JsonWebTokens.JsonClaimValueTypes.Json;

        /// <summary>
        /// A URI that represents the JSON array XML data type.
        /// </summary>
        /// <remarks>When mapping json to .Net Claim(s), if the value was not a string (or an enumeration of strings), the ClaimValue will serialized using the current JSON serializer, a property will be added with the .Net type and the ClaimTypeValue will be set to 'JsonClaimValueType'.</remarks>
        public const string JsonArray = Microsoft.IdentityModel.JsonWebTokens.JsonClaimValueTypes.JsonArray;

        /// <summary>
        /// A URI that represents the JSON null data type
        /// </summary>
        /// <remarks>When mapping json to .Net Claim(s), we use empty string to represent the claim value and set the ClaimValueType to JsonNull</remarks>
        public const string JsonNull = Microsoft.IdentityModel.JsonWebTokens.JsonClaimValueTypes.JsonNull;
    }
}
