// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Text.Json
{
    /// <summary>
    /// Determines the naming policy used to convert a string-based name to another format, such as a camel-casing format.
    /// </summary>
#if BUILDING_SOURCE_GENERATOR
    internal
#else
    public
#endif
    abstract class JsonNamingPolicy
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JsonNamingPolicy"/>.
        /// </summary>
        protected JsonNamingPolicy() { }

        /// <summary>
        /// Returns the naming policy for camel-casing.
        /// </summary>
        public static JsonNamingPolicy CamelCase { get; } = new JsonCamelCaseNamingPolicy();

        /// <summary>
        /// Returns the naming policy for lower snake-casing.
        /// </summary>
        public static JsonNamingPolicy SnakeCaseLower { get; } = new JsonSnakeCaseLowerNamingPolicy();

        /// <summary>
        /// Returns the naming policy for upper snake-casing.
        /// </summary>
        public static JsonNamingPolicy SnakeCaseUpper { get; } = new JsonSnakeCaseUpperNamingPolicy();

        /// <summary>
        /// Returns the naming policy for lower kebab-casing.
        /// </summary>
        public static JsonNamingPolicy KebabCaseLower { get; } = new JsonKebabCaseLowerNamingPolicy();

        /// <summary>
        /// Returns the naming policy for upper kebab-casing.
        /// </summary>
        public static JsonNamingPolicy KebabCaseUpper { get; } = new JsonKebabCaseUpperNamingPolicy();

        /// <summary>
        /// When overridden in a derived class, converts the specified name according to the policy.
        /// </summary>
        /// <param name="name">The name to convert.</param>
        /// <returns>The converted name.</returns>
        public abstract string ConvertName(string name);
    }
}
