// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;

namespace System.IdentityModel.Tokens.Jwt
{
    /// <summary>
    /// Delegate that can be set on <see cref="JsonExtensions.Serializer"/> to control serialization of objects into JSON.
    /// </summary>
    /// <param name="obj">Object to serialize</param>
    /// <returns>The serialized object.</returns>
    public delegate string Serializer(object obj);

    /// <summary>
    /// Delegate that can be set on <see cref="JsonExtensions.Deserializer"/> to control deserialization JSON into objects.
    /// </summary>
    /// <param name="obj">JSON to deserialize.</param>
    /// <param name="targetType">Type expected.</param>
    /// <returns>The deserialized object.</returns>
    public delegate object Deserializer(string obj, Type targetType);

    /// <summary>
    /// Dictionary extensions for serializations
    /// </summary>
    public static class JsonExtensions
    {
        private static Serializer _serializer = JsonConvert.SerializeObject;
        private static Deserializer _deserializer = JsonConvert.DeserializeObject;

        /// <summary>
        /// Gets or sets a <see cref="Serializer"/> to use when serializing objects to JSON.
        /// </summary>
        /// <exception cref="ArgumentNullException">If 'value' is null.</exception>
        public static Serializer Serializer
        {
            get
            {
                return _serializer;
            }
            set
            {
                _serializer = value ?? throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(value)));
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="Deserializer"/> to use when deserializing objects from JSON.
        /// </summary>
        /// <exception cref="ArgumentNullException">If 'value' is null.</exception>
        public static Deserializer Deserializer
        {
            get
            {
                return _deserializer;
            }
            set
            {
                _deserializer = value ?? throw LogHelper.LogExceptionMessage(new ArgumentNullException(nameof(value)));
            }
        }

        /// <summary>
        /// Serializes an object to JSON.
        /// </summary>
        /// <param name="value">The object to serialize</param>
        /// <returns>The object as JSON.</returns>
        public static string SerializeToJson(object value)
        {
            return Serializer(value);
        }

        /// <summary>
        /// Deserialzes JSON into an instance of type T.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="jsonString">The JSON to deserialze.</param>
        /// <returns>A new instance of type T.</returns>
        public static T DeserializeFromJson<T>(string jsonString) where T : class
        {
            return Deserializer(jsonString, typeof(T)) as T;
        }

        /// <summary>
        /// Deserialzes JSON into an instance of <see cref="JwtHeader"/>.
        /// </summary>
        /// <param name="jsonString">The JSON to deserialze.</param>
        /// <returns>A new instance <see cref="JwtHeader"/>.</returns>
        public static JwtHeader DeserializeJwtHeader(string jsonString)
        {
            return Deserializer(jsonString, typeof(JwtHeader)) as JwtHeader;
        }

        /// <summary>
        /// Deserialzes JSON into an instance of <see cref="JwtPayload"/>.
        /// </summary>
        /// <param name="jsonString">The JSON to deserialze.</param>
        /// <returns>A new instance <see cref="JwtPayload"/>.</returns>
        public static JwtPayload DeserializeJwtPayload(string jsonString)
        {
            return Deserializer(jsonString, typeof(JwtPayload)) as JwtPayload;
        }
    }
}
