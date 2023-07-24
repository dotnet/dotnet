// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
#if !CrossVersionTokenValidation
using System.IdentityModel.Tokens.Jwt;
#endif
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.TestUtils
{
    public enum DefaultRoles
    {
        Developer,
        Sales,
        role1,
        roles1
    }

    /// <summary>
    /// Contains a number of different claims sets used to test round tripping claims sets.
    /// </summary>
    public static class ClaimSets
    {
        static ClaimSets()
        {
#if !CrossVersionTokenValidation
            AllReserved = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Actort, "TOKEN"),
                new Claim(JwtRegisteredClaimNames.Aud, "audClaimSets.Value"),
                new Claim(JwtHeaderParameterNames.Typ, "BADDTYPE"),
                new Claim(JwtRegisteredClaimNames.Exp, "BADDATEFORMAT"),
                new Claim(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.UtcNow).ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, "issuerClaimSets.Value"),
                new Claim(JwtRegisteredClaimNames.Jti, "jwtIdClaimSets.Value"),
                new Claim(JwtRegisteredClaimNames.Nbf, "BADDATEFORMAT"),
                new Claim(JwtRegisteredClaimNames.Exp, EpochTime.GetIntDate(DateTime.UtcNow + TimeSpan.FromHours(1)).ToString()),
                new Claim(JwtRegisteredClaimNames.Nbf, EpochTime.GetIntDate(DateTime.UtcNow).ToString()),
                new Claim(JwtRegisteredClaimNames.Prn, "princlipalClaimSets.Value"),
                new Claim(JwtRegisteredClaimNames.Sub, "Subject.Value"),
                new Claim(JwtRegisteredClaimNames.Typ, "Type.Value"),
            };
#endif

            AadClaims = new List<Claim>
            {
                new Claim("tid", "tenantId", ClaimValueTypes.String, Default.Issuer),
            };

            DefaultClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Country, "USA", ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.NameIdentifier, "Bob", ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.Email, "Bob@contoso.com", ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.GivenName, "Bob", ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.Role, "Developer", ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimTypes.Role, "Sales", ClaimValueTypes.String, Default.Issuer),
                new Claim(ClaimsIdentity.DefaultNameClaimType, "Jean-Sébastien", ClaimValueTypes.String, Default.Issuer),
                new Claim("role", "role1", ClaimValueTypes.String, Default.Issuer),
                new Claim("roles", "roles1", ClaimValueTypes.String, Default.Issuer),
            };

            DerivedGlobalClaims = new List<Claim>()
            {
                new Claim("Arabic", @"?????", ClaimValueTypes.String, Default.Issuer),
                new Claim("Turkish1", @"??I?i???çöÇÖ", ClaimValueTypes.String, Default.Issuer),
                new Claim("Turkish2", @"???Ö", ClaimValueTypes.String, Default.Issuer),
                new Claim("Chinese1", @"???", ClaimValueTypes.String, Default.Issuer),
                new Claim("Chinese2", @"??", ClaimValueTypes.String, Default.Issuer),
                new Claim("Japanese1", @"???", ClaimValueTypes.String, Default.Issuer),
                new Claim("Japanese2", @"????<", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtA1", @"????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtA2", @"???????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtA3", @"????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtA4", @"?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtA4", @"????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtB1", @"????????????????????????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtB2", @"??????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtB3", @"????????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtB4", @"????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????<", ClaimValueTypes.String, Default.Issuer),
                new Claim("ExtB5", @"??????????????????????????????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("EnteringIntlChars1", @"????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("EnteringIntlChars2", @"????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("EnteringIntlChars3", @"????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("EnteringIntlChars4", @"??a??z??4??M??f??N??g??S??l??T??m??Y??r??Y??E??K??7??P??i??P??i??U??n??)??B??G??3??L??e??M??9??R??k??S??l??X??q??X??D??]??v??1??J??c??K??7??P??i??Q??i", ClaimValueTypes.String, Default.Issuer),
                new Claim("EnteringIntlChars5", @"??9??R??k??S??l??X??q??Y??E??J??gtOyYeqMY9E6??O??h??P??i??U??n??)??A??Z??s??y??e??L??8??Q??j??R??k??????????????????????W??p??X??D??]??v??1??I??b??J??6??O??h??P??i??U??n??)??B??Z??s", ClaimValueTypes.String, Default.Issuer),
                new Claim("CommonSurrogates1", @"??????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("CommonSurrogates2", @"??????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("CommonSurrogates3", @"??????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("CommonSurrogates4", @"??????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample1", @"!#)6=@Aa}~<", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample2", @"????????????????????????????€????????????????????????????????????????€", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample3", @"??????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample4", @"????????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample5", @"??????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample6", @"???????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample7", @"??????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample8", @"???????????????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample9", @"??????????", ClaimValueTypes.String, Default.Issuer),
                new Claim("STBSample10", @"??????????", ClaimValueTypes.String, Default.Issuer),
            };

            var claims = new List<Claim>();
            claims.AddRange(DefaultClaims);
            foreach (var claim in DefaultClaims)
            {
                claims.Add(new Claim(claim.Type, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer));
            }
            DerivedClaims = claims;

            claims = new List<Claim>();
            foreach (var claim in DerivedGlobalClaims)
            {
                claims.Add(new Claim(claim.Type, claim.Value, claim.ValueType, claim.Issuer, claim.OriginalIssuer));
            }
            DerivedGlobalClaims = claims;

            claims = new List<Claim>();
            claims.AddRange(DefaultClaims);
            claims.AddRange(DefaultClaims);
            DefaultDuplicatedClaims = claims;

            DefaultClaimsIdentity = new ClaimsIdentity(DefaultClaims, Default.AuthenticationType);
            DefaultClaimsIdentity.Label = Default.ClaimsIdentityLabel;
            DefaultClaimsIdentityClaimsDuplicated = new ClaimsIdentity(DefaultDuplicatedClaims, Default.AuthenticationType);
            DefaultClaimsIdentityClaimsDuplicated.Label = Default.ClaimsIdentityLabelDup;
            ClaimsIdentityDerivedClaims = new ClaimsIdentity(DerivedClaims, Default.AuthenticationType);
            DerivedClaimsIdentityDefaultClaims = new ClaimsIdentity(DefaultClaims);
            DerivedClaimsIdentityDerivedClaims = new ClaimsIdentity(DerivedClaims);
            DefaultClaimsPrincipal = new ClaimsPrincipal(DefaultClaimsIdentity);
        }

        public static List<Claim> AadClaims
        {
            get;
            private set;
        }

        public static List<Claim> DefaultClaims
        {
            get;
            private set;
        }

        public static List<Claim> DerivedClaims
        {
            get;
            private set;
        }

        public static List<Claim> DerivedGlobalClaims
        {
            get;
            private set;
        }

        public static List<Claim> DefaultDuplicatedClaims
        {
            get;
            private set;
        }

        public static ClaimsIdentity DefaultClaimsIdentity
        {
            get;
            private set;
        }

        public static ClaimsIdentity DefaultClaimsIdentityClaimsDuplicated
        {
            get;
            private set;
        }

        public static ClaimsIdentity ClaimsIdentityDerivedClaims
        {
            get;
            private set;
        }

        public static ClaimsIdentity DerivedClaimsIdentityDefaultClaims
        {
            get;
            private set;
        }

        public static ClaimsIdentity DerivedClaimsIdentityDerivedClaims
        {
            get;
            private set;
        }

        public static ClaimsPrincipal DefaultClaimsPrincipal
        {
            get;
            private set;
        }

        /// <summary>
        /// Claims containing global Unicode chars. Gleamed from a number of sources.
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="originalIssuer"></param>
        /// <returns></returns>
        public static List<Claim> GlobalClaims
        {
            get;
            private set;
        }


        public static List<Claim> AllReserved
        {
            get;
            private set;
        }

        public static List<Claim> Empty
        {
            get { return new List<Claim>(); }
        }

#if !CrossVersionTokenValidation
        public static List<Claim> MultipleAudiences()
        {
            return MultipleAudiences(Default.Issuer, Default.Issuer);
        }
        public static List<Claim> MultipleAudiences(string issuer, string orignalIssuer)
        {
            var claims = new List<Claim>();
            foreach(var aud in Default.Audiences)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, aud, ClaimValueTypes.String, issuer ?? Default.Issuer, orignalIssuer ?? Default.Issuer));
            }

            return claims;
        }

        public static List<Claim> SingleAudience()
        {
            return SingleAudience(Default.Issuer, Default.Issuer);
        }

        public static List<Claim> SingleAudience(string issuer, string orignalIssuer)
        {
            return new List<Claim> { new Claim(JwtRegisteredClaimNames.Aud, Default.Audience, ClaimValueTypes.String, issuer ?? Default.Issuer, orignalIssuer ?? Default.Issuer) };
        }

        public static List<string> GetDefaultRoles()
        {
            return new List<string>{"role1", "roles1"};
        }

        public static Dictionary<string, string> GetDefaultRolePairs()
        {
            return new Dictionary<string, string> {{"role","role1"},{"roles","roles1"}};
        }

        public static List<Claim> GetDefaultRoleClaims(JwtSecurityTokenHandler handler)
        {
            var claims = new List<Claim>();
            foreach(var kv in GetDefaultRolePairs())
                AddMappedClaim(kv.Key, kv.Value, handler, claims);

            return claims;
        }

        private static void AddMappedClaim(string claimTypeIn, string claimValue, JwtSecurityTokenHandler handler, List<Claim> claims)
        {
            string claimType;
            if (handler == null || !handler.InboundClaimTypeMap.TryGetValue(claimTypeIn, out claimType))
            {
                claims.Add(new Claim(claimTypeIn, claimValue, ClaimValueTypes.String, Default.Issuer, Default.Issuer));
            }
            else
            {
                var claim = new Claim(claimType, claimValue, ClaimValueTypes.String, Default.Issuer, Default.Issuer);
                claim.Properties[JwtSecurityTokenHandler.ShortClaimTypeProperty] = claimTypeIn;
                claims.Add(claim);
            }
        }

        public static List<Claim> Simple(string issuer, string originalIssuer)
        {
            return new List<Claim>()
            {
                new Claim( ClaimTypes.Country, "USA", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.Email, "user@contoso.com", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer),
                new Claim( ClaimTypes.GivenName, "Tony", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer),
                new Claim( ClaimTypes.Role, "Sales", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimsIdentity.DefaultNameClaimType, "Jean-Sébastien", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
            };
        }

        public static IEnumerable<Claim> SimpleShortClaimtypes(string issuer, string originalIssuer)
        {
            return new List<Claim>()
            {                
                NewClaimWithShortType(ClaimTypes.Country, "USA", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer),
                NewClaimWithShortType(ClaimTypes.Email, "user@contoso.com", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer),
                NewClaimWithShortType(ClaimTypes.GivenName, "Tony", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                NewClaimWithShortType(ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer),
                NewClaimWithShortType(ClaimTypes.Role, "Sales", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                NewClaimWithShortType(ClaimsIdentity.DefaultNameClaimType, "Jean-Sébastien", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
            };
        }

        public static Claim NewClaimWithShortType(string claimType, string claimValue, string claimValueType, string issuer, string originalIssuer)
        {
            return new Claim(JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.ContainsKey(ClaimTypes.Country) ? JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap[ClaimTypes.Country] : ClaimTypes.Country, claimValue, claimValueType, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer);
        }

        public static IEnumerable<Claim> ActorClaimNotJwt(string issuer, string originalIssuer )
        {
            return new List<Claim>()
            {
                new Claim( ClaimTypes.Actor, "USA", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.Email, "user@contoso.com", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer),
                new Claim( ClaimTypes.GivenName, "Tony", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer),
                new Claim( ClaimTypes.Role, "Sales", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
            };
        }

        public static IEnumerable<Claim> DefaultClaimsAsCreatedInPayload()
        {
            return new List<Claim>()
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country", "USA"),
                new Claim("email", "Bob@contoso.com"),
                new Claim("given_name", "Bob"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/homephone", "555.1212"),
                new Claim("unique_name", "Jean-Sébastien"),
                new Claim("iss", Default.Issuer),
                new Claim("aud", Default.Audience)
            };
        }

        public static IEnumerable<Claim> AllInboundShortClaimTypes(string issuer, string originalIssuer, IEnumerable<Claim> extraClaims = null)
        {
            foreach ( KeyValuePair<string, string> pair in JwtSecurityTokenHandler.DefaultInboundClaimTypeMap )
            {
                yield return new Claim( pair.Key, pair.Value, ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer );
            }

            if ( extraClaims != null )
            {
                foreach ( Claim c in extraClaims )
                {
                    yield return c;
                }
            }
        }

        public static IEnumerable<Claim> ExpectedInClaimsIdentityUsingAllInboundShortClaimTypes(string issuer, string originalIssuer, IEnumerable<Claim> extraClaims = null)
        {
            foreach (KeyValuePair<string, string> pair in JwtSecurityTokenHandler.DefaultInboundClaimTypeMap)
            {
                Claim claim = new Claim(pair.Value, pair.Value, ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer);
                claim.Properties.Add(new KeyValuePair<string, string>(JwtSecurityTokenHandler.ShortClaimTypeProperty, pair.Key));
                yield return claim;
            }

            if (extraClaims != null)
            {
                foreach (Claim c in extraClaims)
                {
                    yield return c;
                }
            }
        }

        public static List<Claim> DuplicateTypes()
        {
            return DuplicateTypes(Default.Issuer, Default.Issuer);
        }
        
        /// <summary>
        /// Returns an enumeration containing duplicate claims. Used to test duplicates.
        /// </summary>
        /// <param name="issuer"></param>
        /// <param name="originalIssuer"></param>
        /// <returns></returns>
        public static List<Claim> DuplicateTypes(string issuer, string originalIssuer)
        {
            return new List<Claim>
            {
                new Claim( ClaimTypes.Country, "USA", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.Email, "user@contoso.com", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.GivenName, "Tony", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.HomePhone, "555.1212", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.Role, "Sales", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.Country, "USA_2", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.Email, "user@contoso.com_2", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.GivenName, "Tony_2", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.HomePhone, "555.1212_2", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer ),
                new Claim( ClaimTypes.Role, "Sales_2", ClaimValueTypes.String, issuer ?? Default.Issuer, originalIssuer ?? Default.OriginalIssuer )
            };
        }

        public static List<Claim> EntityAsJsonClaim( string issuer, string orginalIssuer )
        {
            return new List<Claim> { new Claim(typeof(Entity).ToString(), JsonExtensions.SerializeToJson(Entity.Default), JsonClaimValueTypes.Json, issuer ?? Default.Issuer, orginalIssuer) };
        }
#endif
    }

    /// <summary>
    /// Complex type. Used for testing round tripping using complex claims.
    /// </summary>
    public class Entity
    {
        public static Entity Default
        {
            get
            {
                Entity entity = new Entity
                {
                    Address = new Address
                    {
                        Country = "Country",
                        Locality = "Locality",
                        Region = "Region"
                    },
                    Email = "email@email.com",
                    Email_Verified = false,
                    Exp = 1234567891,
                    FavoriteColors = new string[] { "blue", "red", "orange" },
                    Nothing = null,
                    pi = 3.14159,
                    Request = new Request
                    {
                        Acr = new Acr()
                        {
                            Values = new string[] { "urn:mace:incommon:iap:silver", "urn:mace:incommon:iap:bronze" }
                        },
                        AuthTime = new AuthTime()
                        {
                            Essential = false
                        },
                        NicKName = null
                    },
                    Urn = "urn:example:attributes"
                };

                return entity;
            }
        }

        public string Email { get; set; }

        public bool Email_Verified { get; set; }

        public string Urn { get; set; }

        public long   Exp { get; set; }

        public double pi  { get; set; }

        public string Nothing { get; set; }

        public string[] FavoriteColors { get; set; }

        public Address Address { get; set; }

        public Request Request { get; set; }
    }

    /// <summary>
    /// Contained in Entity class to test complex claims
    /// </summary>
    public class AuthTime
    {
        public bool Essential { get; set;}
    }

    /// <summary>
    /// Contained in Entity class to test complex claims
    /// </summary>
    public class Acr
    {
        public string[] Values{ get; set;}
    }

    /// <summary>
    /// Contained in Entity class to test complex claims
    /// </summary>
    public class Request
    {
        public string NicKName { get; set; }

        public AuthTime AuthTime { get; set; }

        public Acr Acr { get; set; }
    }

    /// <summary>
    /// Contained in Entity class to test complex claims
    /// </summary>
    public class Address
    {
        public string Locality { get; set; }

        public string Region   { get; set; }

        public string Country  { get; set; }
    }
}
