// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.JsonWebTokens
{
    /// <summary>
    /// This class provides an abstraction over the json parser for net461+ using System.Text.Json.
    /// <see cref="JsonWebToken"/> will delegate to this class to get values.
    /// </summary>
    internal class JsonClaimSet
    {
        internal const string ClassName = "Microsoft.IdentityModel.JsonWebTokens.JsonClaimSet";

        internal static JsonClaimSet Empty { get; } = new JsonClaimSet("{}"u8.ToArray());
        internal object _claimsLock = new();
        internal IDictionary<string, object> _jsonClaims;
        private IList<Claim> _claims;

        internal JsonClaimSet(IDictionary<string, object> jsonClaims)
        {
            _jsonClaims = jsonClaims;
        }
        internal JsonClaimSet(byte[] jsonUtf8Bytes)
        {
            _jsonClaims = JwtTokenUtilities.CreateClaimsDictionary(jsonUtf8Bytes, jsonUtf8Bytes.Length);
        }

        internal IList<Claim> Claims(string issuer)
        {
            if (_claims == null)
                lock (_claimsLock)
                    _claims ??= CreateClaims(issuer);

            return _claims;
        }

        internal IList<Claim> CreateClaims(string issuer)
        {
            IList<Claim> claims = new List<Claim>();
            foreach (KeyValuePair<string, object> kvp in _jsonClaims)
                CreateClaimFromObject(claims, kvp.Key, kvp.Value, issuer);

            return claims;
        }

        internal static void CreateClaimFromObject(IList<Claim> claims, string claimType, object value, string issuer)
        {
            // Json.net recognized DateTime by default.
            if (value is string str)
                claims.Add(new Claim(claimType, str, JwtTokenUtilities.GetStringClaimValueType(str), issuer, issuer));
            else if (value is int i)
                claims.Add(new Claim(claimType, i.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer32, issuer, issuer));
            else if (value is long l)
                claims.Add(new Claim(claimType, l.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64, issuer, issuer));
            else if (value is bool b)
                claims.Add(new Claim(claimType, b.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Boolean, issuer, issuer));
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

        internal static Claim CreateClaimFromJsonElement(string claimType, string issuer, JsonElement value)
        {
            // Json.net recognized DateTime by default.
            if (value.ValueKind == JsonValueKind.String)
            {
                string claimValue = value.ToString();
                return new Claim(claimType, claimValue, JwtTokenUtilities.GetStringClaimValueType(claimValue), issuer, issuer);
            }
            else if (value.ValueKind == JsonValueKind.Null)
                return new Claim(claimType, string.Empty, JsonClaimValueTypes.JsonNull, issuer, issuer);
            else if (value.ValueKind == JsonValueKind.Object)
                return new Claim(claimType, value.ToString(), JsonClaimValueTypes.Json, issuer, issuer);
            else if (value.ValueKind == JsonValueKind.False)
                return new Claim(claimType, "False", ClaimValueTypes.Boolean, issuer, issuer);
            else if (value.ValueKind == JsonValueKind.True)
                return new Claim(claimType, "True", ClaimValueTypes.Boolean, issuer, issuer);
            else if (value.ValueKind == JsonValueKind.Number)
            {
                if (value.TryGetInt32(out int i))
                    return new Claim(claimType, i.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer, issuer, issuer);
                else if (value.TryGetInt64(out long l))
                    return new Claim(claimType, l.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Integer64, issuer, issuer);
                else if (value.TryGetUInt32(out uint u))
                    return new Claim(claimType, u.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.UInteger32, issuer, issuer);
                else if (value.TryGetDouble(out double d))
                    return new Claim(claimType, d.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double, issuer, issuer);
                else if (value.TryGetDecimal(out decimal m))
                    return new Claim(claimType, m.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.Double, issuer, issuer);
                else if (value.TryGetUInt64(out ulong ul))
                    return new Claim(claimType, ul.ToString(CultureInfo.InvariantCulture), ClaimValueTypes.UInteger64, issuer, issuer);
            }
            else if (value.ValueKind == JsonValueKind.Array)
            {
                return new Claim(claimType, value.ToString(), JsonClaimValueTypes.JsonArray, issuer, issuer);
            }

            return null;
        }

        internal static object CreateObjectFromJsonElement(JsonElement jsonElement)
        {
            if (jsonElement.ValueKind == JsonValueKind.Array)
            {
                int numberOfElements = 0;
                // is this an array of properties
                foreach (JsonElement element in jsonElement.EnumerateArray())
                    numberOfElements++;

                object[] objects = new object[numberOfElements];

                int index = 0;
                foreach (JsonElement element in jsonElement.EnumerateArray())
                    objects[index++] = CreateObjectFromJsonElement(element);

                return (object)objects;
            }
            else if (jsonElement.ValueKind == JsonValueKind.String)
            {
                if (DateTime.TryParse(jsonElement.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime dateTime))
                    return (object)dateTime;

                return jsonElement.GetString();
            }
            else if (jsonElement.ValueKind == JsonValueKind.Null)
                return (object)null;
            else if (jsonElement.ValueKind == JsonValueKind.Object)
                return jsonElement.ToString();
            else if (jsonElement.ValueKind == JsonValueKind.False)
                return (object)false;
            else if (jsonElement.ValueKind == JsonValueKind.True)
                return (object)true;
            else if (jsonElement.ValueKind == JsonValueKind.Number)
            {
                if (jsonElement.TryGetInt64(out long longValue))
                    return longValue;
                else if (jsonElement.TryGetInt32(out int intValue))
                    return intValue;
                else if (jsonElement.TryGetDecimal(out decimal decimalValue))
                    return decimalValue;
                else if (jsonElement.TryGetDouble(out double doubleValue))
                    return doubleValue;
                else if (jsonElement.TryGetUInt32(out uint uintValue))
                    return uintValue;
                else if (jsonElement.TryGetUInt64(out ulong ulongValue))
                    return ulongValue;
            }

            return jsonElement.GetString();
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
                return obj.ToString();

            return string.Empty;
        }

        internal DateTime GetDateTime(string key)
        {
            if (!_jsonClaims.TryGetValue(key, out object value))
                return DateTime.MinValue;

            return EpochTime.DateTime(Convert.ToInt64(Math.Truncate((double)GetValueAsLong(key, value))));
        }

        internal T GetValue<T>(string key)
        {
            T retval = GetValue<T>(key, true, out bool _);
            return retval;
        }

        /// <summary>
        /// The goal here is return types that are expected in a JWT token.
        /// The 5 basic types: number, string, true / false, nil, array (of basic types).
        /// This is not a general purpose translation layer for complex types.
        /// For that we would need to provide a way to hook a JsonConverter to for complex types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="throwEx"></param>
        /// <param name="found"></param>
        /// <returns></returns>
        internal T GetValue<T>(string key, bool throwEx, out bool found)
        {
            found = _jsonClaims.TryGetValue(key, out object obj);

            if (!found)
            {
                if (throwEx)
                    throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14304, key)));
                else
                    return default;
            }

            if (obj == null)
                if (typeof(T) == typeof(object) || typeof(T).IsClass || Nullable.GetUnderlyingType(typeof(T)) != null)
                {
                    return (T)(object)null;
                }
                else
                {
                    found = false;
                    return default;
                }

            Type objType = obj.GetType();

            if (typeof(T) == objType)
                return (T)(obj);

            if (typeof(T) == typeof(object))
                return (T)obj;

            if (typeof(T) == typeof(string))
                return (T)((object)obj.ToString());

            if (typeof(T) == typeof(IList<string>))
            {
                List<string> list = new();
                if (obj is IList iList)
                {
                    foreach (object item in iList)
                        list.Add(item?.ToString());

                    return (T)((object)list);
                }
                else
                {
                    list.Add(obj.ToString());
                }
                return (T)(object)list;
            }

            if (typeof(T) == typeof(int) && int.TryParse(obj.ToString(), out int i))
                return (T)(object)i;

            if (typeof(T) == typeof(long) && long.TryParse(obj.ToString(), out long l))
                return (T)(object)l;

            if (typeof(T) == typeof(double) && double.TryParse(obj.ToString(), out double d))
                return (T)(object)d;

            if (typeof(T) == typeof(DateTime) && DateTime.TryParse(obj.ToString(), out DateTime dt))
                return (T)(object)dt;

            if (typeof(T) == typeof(uint) && uint.TryParse(obj.ToString(), out uint u))
                return (T)(object)u;

            if (typeof(T) == typeof(float) && float.TryParse(obj.ToString(), out float f))
                return (T)(object)f;

            if (typeof(T) == typeof(decimal) && decimal.TryParse(obj.ToString(), out decimal m))
                return (T)(object)m;

            if (typeof(T) == typeof(IList<object>))
            {
                List<object> list = new();
                if (obj is IList items)
                {
                    foreach (object item in items)
                        list.Add(item);
                }
                else
                {
                    list.Add(obj);
                }
                return (T)((object)list);
            }

            if (typeof(T) == typeof(int[]))
            {
                int[] ints;
                if (obj is IList ilist)
                {
                    ints = new int[ilist.Count];
                    int index = 0;
                    foreach (object item in ilist)
                    {
                        if (typeof(int) == item.GetType())
                            ints[index++] = (int)item;
                    }

                    // all items must be int
                    if (index == ilist.Count)
                        return (T)(object)(int[])ints;
                }
                else if (objType == typeof(int))
                {
                    ints = new int[]{(int)obj};
                    return (T)(object)(int[])ints;
                }
            }

            if (typeof(T) == typeof(object[]))
            {
                object[] objects;
                if (obj is IList ilist)
                {
                    objects = new object[ilist.Count];
                    int index = 0;
                    foreach (object item in ilist)
                        objects[index++] = item;
                }
                else
                {
                    objects = new object[] { obj };
                }

                return (T)(object)(object[])objects;
            }


            found = false;
            if (throwEx)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX14305, key, typeof(T), objType, obj.ToString())));

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

        private static long GetValueAsLong(string claimName, object obj)
        {
            if (obj is int i)
                return (long)i;

            if (obj is long)
                return (long)(obj);

            if (obj is double d)
                return (long)d;

            if (obj is uint u)
                return (long)u;

            if (obj is float f)
                return (long)f;

            if (obj is decimal m)
                return (long) m;

            if (obj is string str)
            {
                if (int.TryParse(str, out int ii))
                    return (long)ii;

                if (long.TryParse(str, out long l))
                    return l;

                if (double.TryParse(str, out double dd))
                    return (long)dd;

                if (uint.TryParse(str, out uint uu))
                    return (long)uu;

                if (float.TryParse(str, out float ff))
                    return (long)ff;

                if (decimal.TryParse(str, out decimal mm))
                    return (long)mm;
            }

            throw LogHelper.LogExceptionMessage(new FormatException(LogHelper.FormatInvariant(LogMessages.IDX14300, claimName, obj?.ToString(), typeof(long))));
        }
    }
}
