// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

internal sealed class Program
{
    // The code in this program is expected to be trim and AOT compatible
    private static int Main()
    {
        const string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJjbGFpbV9hc19kYXRldGltZSI6IjIwMTktMTEtMTVUMTQ6MzE6MjEuNjEwMTMyNloifQ.yYcHSl-rNT2nHe8Nb0aWe6Qu3E0ZOn2_OUidpxuw0wk";

        JsonWebToken t = new JsonWebToken(token);
        if (t.Claims.Count() != 1)
        {
            return -1;
        }

        Claim dateClaim = t.GetClaim("claim_as_datetime");
        if (dateClaim.Value != "2019-11-15T14:31:21.6101326Z")
        {
            return -2;
        }

        return 100;
    }
}
