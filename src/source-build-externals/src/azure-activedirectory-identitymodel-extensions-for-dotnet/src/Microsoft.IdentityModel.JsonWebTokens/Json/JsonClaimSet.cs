// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Json;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// This class provides an abstraction over the json parser for net461+ using System.Text.Json.
    /// <see cref="JsonWebToken"/> will delegate to this class to get values.
    /// </summary>
    internal class JsonClaimSet
    {
        internal const string ClassName = "Microsoft.IdentityModel.JsonWebTokens.JsonClaimSet";

        internal object _claimsLock = new();
        internal readonly Dictionary<string, object> _jsonClaims;
        private List<Claim> _claims;

        internal JsonClaimSet() { _jsonClaims = new Dictionary<string, object>(); }

        internal JsonClaimSet(Dictionary<string, object> jsonClaims)
        {
            _jsonClaims = jsonClaims;
        }

        internal List<Claim> Claims(string issuer)
        {
            if (_claims == null)
                lock (_claimsLock)
                    _claims ??= CreateClaims(issuer);

            return _claims;
        }

        internal List<Claim> CreateClaims(string issuer)
        {
            var claims = new List<Claim>(_jsonClaims.Count);
            foreach (KeyValuePair<string, object> kvp in _jsonClaims)
                CreateClaimFromObject(claims, kvp.Key, kvp.Value, issuer);

            return claims;
        }

        internal static void CreateClaimFromObject(List<Claim> claims, string claimType, object value, string issuer)
        {
            // Json.net recognized DateTime by default.
            if (value is string str)
                claims.Add(new Claim(claimType, str, JwtTokenUtilities.GetStringClaimValueType(str, claimType), issuer, issuer));
            else if (value is int i)
                claims.Add(new Claim(claimType, i.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer32, issuer, issuer));
            else if (value is long l)
                claims.Add(new Claim(claimType, l.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64, issuer, issuer));
            else if (value is bool b)
            {
                // Can't just use ToString or bools will get encoded as True/False instead of true/false.
                if (b)
                    claims.Add(new Claim(claimType, "true", ClaimValueTypes.Boolean, issuer, issuer));
                else
                    claims.Add(new Claim(claimType, "false", ClaimValueTypes.Boolean, issuer, issuer));
            }
            else if (value is double d)
                claims.Add(new Claim(claimType, d.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double, issuer, issuer));
            else if (value is DateTime dt)
                claims.Add(new Claim(claimType, dt.ToString("o",CultureInfo.InvariantCulture), ClaimValueTypes.DateTime, issuer, issuer));
            else if (value is float f)
                claims.Add(new Claim(claimType, f.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double, issuer, issuer));
            else if (value is decimal m)
                claims.Add(new Claim(claimType, m.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double, issuer, issuer));
            else if (value is null)
                claims.Add(new Claim(claimType, string.Empty, JsonClaimValueTypes.JsonNull, issuer, issuer));
            else if (value is IList ilist)
            {
                foreach (var item in ilist)
                    CreateClaimFromObject(claims, claimType, item, issuer);
            }
            else if (value is JsonElement j)
                if (j.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement jsonElement in j.EnumerateArray())
                    {
                        Claim claim = CreateClaimFromJsonElement(claimType, issuer, jsonElement);
                        if (claim != null)
                            claims.Add(claim);
                    }
                }
                else
                {
                    Claim claim = CreateClaimFromJsonElement(claimType, issuer, j);
                    if (claim != null)
                        claims.Add(claim);
                }
        }

        internal static Claim CreateClaimFromJsonElement(string claimType, string issuer, JsonElement jsonElement)
        {
            // Json.net recognized DateTime by default.
            if (jsonElement.ValueKind == JsonValueKind.String)
            {
                string claimValue = jsonElement.ToString();
                return new Claim(claimType, claimValue, JwtTokenUtilities.GetStringClaimValueType(claimValue, claimType), issuer, issuer);
            }
            else if (jsonElement.ValueKind == JsonValueKind.Null)
                return new Claim(claimType, string.Empty, JsonClaimValueTypes.JsonNull, issuer, issuer);
            else if (jsonElement.ValueKind == JsonValueKind.Object)
                return new Claim(claimType, jsonElement.ToString(), JsonClaimValueTypes.Json, issuer, issuer);
            else if (jsonElement.ValueKind == JsonValueKind.False)
                return new Claim(claimType, "false", ClaimValueTypes.Boolean, issuer, issuer);
            else if (jsonElement.ValueKind == JsonValueKind.True)
                return new Claim(claimType, "true", ClaimValueTypes.Boolean, issuer, issuer);
            else if (jsonElement.ValueKind == JsonValueKind.Number)
            {
                if (jsonElement.TryGetInt32(out int i))
                    return new Claim(claimType, i.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer32, issuer, issuer);
                else if (jsonElement.TryGetInt64(out long l))
                    return new Claim(claimType, l.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64, issuer, issuer);
                else if (jsonElement.TryGetDouble(out double d))
                    return new Claim(claimType, d.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double, issuer, issuer);
                else if (jsonElement.TryGetUInt32(out uint u))
                    return new Claim(claimType, u.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.UInteger32, issuer, issuer);
                else if (jsonElement.TryGetUInt64(out ulong ul))
                    return new Claim(claimType, ul.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.UInteger64, issuer, issuer);
                else if (jsonElement.TryGetSingle(out float f))
                    return new Claim(claimType, f.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double, issuer, issuer);
                else if (jsonElement.TryGetDecimal(out decimal m))
                    return new Claim(claimType, m.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double, issuer, issuer);
            }
            else if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                return new Claim(claimType, jsonElement.ToString(), JsonClaimValueTypes.JsonArray, issuer, issuer);
            }

            return null;
        }

        internal Claim GetClaim(string key, string issuer)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (_jsonClaims.TryGetValue(key, out object _))
            {
                foreach (var claim in Claims(issuer))
                    if (claim.Type == key)
                        return claim;
            }

            throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14304, key)));
        }

        internal string GetStringValue(string key)
        {
            if (_jsonClaims.TryGetValue(key, out object obj))
            {
                if (obj == null)
                    return null;

                return obj.ToString();
            }

            return string.Empty;
        }

        internal DateTime GetDateTime(string key)
        {
            long l = GetValue<long>(key, false, out bool found);
            if (found)
                return EpochTime.DateTime(l);

            return DateTime.MinValue;
        }

        internal T GetValue<T>(string key)
        {
            return GetValue<T>(key, true, out bool _);
        }

        /// <summary>
        /// The goal here is return types that are expected in a JWT token.
        /// The 5 basic types: number, string, true / false, nil, array (of basic types).
        /// This is not a general purpose translation layer for complex types.
        /// For that we would need to provide a way to hook a JsonConverter to for complex types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="throwEx">if this is called from TryGetValue then we don't want to throw.</param>
        /// <param name="found"></param>
        /// <returns></returns>
        internal T GetValue<T>(string key, bool throwEx, out bool found)
        {
            found = _jsonClaims.TryGetValue(key, out object obj);

            if (!found)
                return throwEx ? throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14304, key))) : default;

            if (obj == null)
                if (typeof(T) == typeof(object) || typeof(T).IsClass || Nullable.GetUnderlyingType(typeof(T)) != null)
                    return (T)(object)null;
                else
                    return default;

            Type objType = obj.GetType();
            if (typeof(T) == objType)
                return (T)(obj);

            if (typeof(T) == typeof(object))
                return (T)obj;

            // When the JsonClaimSet is created JsonArray and JsonObject are stored as JsonElement's
            if (obj is JsonElement jsonElement)
            {
                if (JsonSerializerPrimitives.TryCreateTypeFromJsonElement<T>(jsonElement, out T t))
                    return t;
            }
            // the below here should only be simple types, string, int, ...
            else if (typeof(T) == typeof(string))
            {
                if (objType == typeof(DateTime))
                    return (T)((object)((DateTime)obj).ToString("o", CultureInfo.InvariantCulture));

                if (obj is List<string> list)
                {
                    if (list.Count == 1)
                        return (T)((object)(list[0]));
                }
                else
                    return (T)((object)obj.ToString());
            }
            else if (typeof(T) == typeof(bool))
            {
                if (bool.TryParse(obj.ToString(), out bool value))
                    return (T)(object)value;
            }
            else if (typeof(T) == typeof(int))
            {
                if (int.TryParse(obj.ToString(), out int value))
                    return (T)(object)value;
            }
            else if (typeof(T) == typeof(long))
            {
                if (objType == typeof(int))
                    return (T)(object)(long)(int)obj;

                if (long.TryParse(obj.ToString(), out long value))
                    return (T)(object)value;
            }
            else if (typeof(T) == typeof(string[]))
            {
                if (objType == typeof(string))
                    return (T)(object)new string[] { (string)obj };

                if (objType == typeof(DateTime))
                    return (T)(object)new string[] { ((DateTime)obj).ToString("o", CultureInfo.InvariantCulture) };

                return (T)(object)new string[] { obj.ToString() };
            }
            else if (typeof(T) == typeof(List<string>))
            {
                if (objType == typeof(string))
                    return (T)(object)new List<string> { (string)obj };

                if (objType == typeof(DateTime))
                    return (T)(object)new List<string> { ((DateTime)obj).ToString("o", CultureInfo.InvariantCulture) };

                return (T)(object)new List<string> { obj.ToString() };
            }
            else if (typeof(T) == typeof(Collection<string>))
            {
                if (objType == typeof(string))
                    return (T)(object)new Collection<string> { (string)obj };

                if (objType == typeof(DateTime))
                    return (T)(object)new Collection<string> { ((DateTime)obj).ToString("o", CultureInfo.InvariantCulture) };

                return (T)(object)new Collection<string> { obj.ToString() };
            }
            // we could have added an OR condition to List<string>
            // but we have set an order of preference for the return types: Collection<string> is preferred over IList<string>
            else if (typeof(T) == typeof(IList<string>))
            {
                if (objType == typeof(string))
                    return (T)(object)new List<string> { (string)obj };

                if (objType == typeof(DateTime))
                    return (T)(object)new List<string> { ((DateTime)obj).ToString("o", CultureInfo.InvariantCulture) };

                return (T)(object)new List<string> { obj.ToString() };
            }
            // we could have added an OR condition to Collection<string>
            // but we have set an order of preference for the return types:
            // string[], List<string>, Collection<string>, IList<string>, ICollection<string>
            else if (typeof(T) == typeof(ICollection<string>))
            {
                if (objType == typeof(string))
                    return (T)(object)new Collection<string> { (string)obj };

                if (objType == typeof(DateTime))
                    return (T)(object)new Collection<string> { ((DateTime)obj).ToString("o", CultureInfo.InvariantCulture) };

                return (T)(object)new Collection<string> { obj.ToString() };
            }
            else if (typeof(T) == typeof(object[]))
                return (T)(object)new object[] { obj };

            else if (typeof(T) == typeof(List<object>))
                return (T)(object)new List<object> { obj };

            else if (typeof(T) == typeof(Collection<object>))
                return (T)(object)new Collection<object> { obj };

            else if(typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), obj.ToString(), ignoreCase: true);
            }
            else if (typeof(T) == typeof(DateTime))
            {
                if (DateTime.TryParse(obj.ToString(), out DateTime value))
                    return (T)(object)value;
            }
            else if (typeof(T) == typeof(int[]))
            {
                if (objType == typeof(int))
                    return (T)(object)new int[] { (int)obj };

                if (int.TryParse(obj.ToString(), out int value))
                    return (T)(object)new int[] { value };
            }
            else if (typeof(T) == typeof(long[]))
            {
                if (objType == typeof(long))
                    return (T)(object)new long[] { (long)obj };

                if(objType == typeof(int))
                    return (T)(object)new long[] { (int)obj };

                if (long.TryParse(obj.ToString(), out long value))
                    return (T)(object)new long[] { value };
            }
            else if (typeof(T) == typeof(double))
            {
                if(double.TryParse(obj.ToString(), out double value))
                    return (T)(object)value;
            }
            else if (typeof(T) == typeof(uint))
            {
                if (uint.TryParse(obj.ToString(), out uint value))
                    return (T)(object)value;
            }
            else if (typeof(T) == typeof(ulong))
            {
                if (ulong.TryParse(obj.ToString(), out ulong value))
                    return (T)(object)value;
            }
            else if (typeof(T) == typeof(float))
            {
                if (float.TryParse(obj.ToString(), out float value))
                    return (T)(object)value;
            }
            else if (typeof(T) == typeof(decimal))
            {
                if (decimal.TryParse(obj.ToString(), out decimal value))
                    return (T)(object)value;
            }

            found = false;
            if (throwEx)
                throw LogHelper.LogExceptionMessage(
                    new ArgumentException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX14305,
                            LogHelper.MarkAsNonPII(key),
                            LogHelper.MarkAsNonPII(typeof(T)),
                            LogHelper.MarkAsNonPII(objType),
                            obj.ToString())));
            else
                LogHelper.LogExceptionMessage(
                    new ArgumentException(
                        LogHelper.FormatInvariant(
                            LogMessages.IDX14305,
                            LogHelper.MarkAsNonPII(key),
                            LogHelper.MarkAsNonPII(typeof(T)),
                            LogHelper.MarkAsNonPII(objType),
                            obj.ToString())));

            return default;
        }

        internal bool TryGetClaim(string key, string issuer, out Claim claim)
        {
            claim = null;
            if (!_jsonClaims.TryGetValue(key, out object value))
                return false;

            foreach (Claim c in Claims(issuer))
                if (c.Type == key)
                {
                    claim = c;
                    return true;
                }

            return false;
        }

        /// <summary>
        /// The return types that are expected in a JWT token.
        /// The 5 basic types: number, string, true / false, nil, array (of basic types).
        /// This is not a general purpose translation layer for complex types.
        /// For that we would need to provide a way to hook a JsonConverter to for complex types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        internal bool TryGetValue<T>(string key, out T value)
        {
            value = GetValue<T>(key, false, out bool found);
            return found;
        }

        internal bool HasClaim(string claimName)
        {
            return _jsonClaims.TryGetValue(claimName, out _);
        }
    }
}
