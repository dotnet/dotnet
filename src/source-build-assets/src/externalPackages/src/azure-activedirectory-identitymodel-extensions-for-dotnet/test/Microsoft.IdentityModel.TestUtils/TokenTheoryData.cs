// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.TestUtils
{
    public class TokenTheoryData : TheoryDataBase
    {
        public TokenTheoryData()
        {
        }

        public TokenTheoryData(string testId) : base(testId)
        {
        }

        public TokenTheoryData(TokenTheoryData other)
        {
            Actor = other.Actor;
            ActorTokenValidationParameters = other.ActorTokenValidationParameters;
            Audiences = other.Audiences;
            CanRead = other.CanRead;
            ExpectedException = other.ExpectedException;
            First = other.First;
            Issuer = other.Issuer;
            MemoryStream = other.MemoryStream;
            SecurityToken = other.SecurityToken;
            SigningCredentials = other.SigningCredentials;
            TestId = other.TestId;
            Token = other.Token;
            TokenDescriptor = other.TokenDescriptor;
            ValidationParameters = other.ValidationParameters;
            XmlWriter = other.XmlWriter;
        }

        public string Actor { get; set; }

        public TokenValidationParameters ActorTokenValidationParameters { get; set; }

        public IEnumerable<string> Audiences { get; set; }

        public bool CanRead { get; set; }

        public string Issuer { get; set; }

        public MemoryStream MemoryStream { get; set; }

        public SecurityToken SecurityToken { get; set; }

        public SigningCredentials SigningCredentials { get; set; }

        public string Token { get; set; }

        public SecurityTokenDescriptor TokenDescriptor { get; set; }

        public TokenValidationParameters ValidationParameters { get; set; }

        public XmlReader XmlReader { get; set; }

        public XmlWriter XmlWriter { get; set; }
    }
}
