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

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.TestUtils;
using Microsoft.IdentityModel.Tokens;

namespace System.IdentityModel.Tokens.Jwt.Tests
{
    /// <summary>
    /// This class is used to define the construction of a JWT for testing purposes.
    /// Use it by writing a method to populate a set for a specific purpose, such as SignatureValidationVariations, below.
    /// Then run through a loop that checks expected results.
    /// 
    /// </summary>
    public class JwtSecurityTokenTestVariation
    {
        public JwtSecurityTokenTestVariation() 
        {
        }

        // ===========================
        // token setup params - different variations will set different items
        public string Audience { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public JwtHeader Header { get; set; }
        public ExpectedException ExpectedException { get; set; } = ExpectedException.NoExceptionExpected;
        public JwtSecurityToken ExpectedJwtSecurityToken { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow + TimeSpan.FromHours(1);
        public string Issuer { get; set; }
        public string Name { get; set; }
        public DateTime NotBefore { get; set; } = DateTime.UtcNow;
        public string RawAuthenticationTag { get; set; }
        public string RawCiphertext { get; set; }
        public string RawEncryptedKey { get; set; }
        public string RawHeader { get; set; }
        public string RawInitializationVector { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }

    public class JwtTestUtilities
    {
        public static string GetJwtParts( string jwt, string whichParts )
        {
            string[] parts = jwt.Split( '.' );
            if ( string.Equals( whichParts, "AllParts", StringComparison.OrdinalIgnoreCase ) )
            {
                return string.Format( "{0}.{1}.{2}", parts[0], parts[1], parts[2] );
            }
            
            if ( string.Equals( whichParts, "Parts-0-1", StringComparison.OrdinalIgnoreCase ) )
            {
                return string.Format( "{0}.{1}.", parts[0], parts[1] );
            }

            Console.WriteLine( string.Format("Hey, the 'whichParts' parameter wasn't recognized: '{0}'.  Returning'string.Empty' hope that is what you wanted", whichParts ) );
            return string.Empty;
        }
    }
}
