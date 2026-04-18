// ============================================================================
// THIRD-PARTY NOTICE — Jsonite
// Source: https://github.com/xoofx/jsonite
// License: BSD-2-Clause (reproduced below)
// Embedded for use as a lightweight JSON parser on .NET Framework where
// System.Text.Json is not available as a built-in framework library.
// ============================================================================
//
// Copyright(c) 2016, Alexandre Mutel
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification
// , are permitted provided that the following conditions are met:
// 
// 1. Redistributions of source code must retain the above copyright notice, this
//    list of conditions and the following disclaimer.
// 
// 2. Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#pragma warning disable SA1000 // Spacing around keywords
#pragma warning disable SA1001 // Commas should be followed by space
#pragma warning disable SA1002 // Semicolons should be followed by space
#pragma warning disable SA1003 // Operator should be followed by space
#pragma warning disable SA1005 // Single line comment should begin with a space
#pragma warning disable SA1008 // Opening parenthesis should be followed by space
#pragma warning disable SA1009 // Closing parenthesis should be followed by space
#pragma warning disable SA1011 // Closing square bracket should be followed by space
#pragma warning disable SA1012 // Opening brace should be followed by space
#pragma warning disable SA1013 // Closing brace should be followed by space
#pragma warning disable SA1015 // Closing generic bracket should be followed by space
#pragma warning disable SA1019 // Member access should use 'this.'
#pragma warning disable SA1025 // Code should not contain multiple whitespace in a row
#pragma warning disable SA1026 // Keyword followed by span or tab
#pragma warning disable SA1028 // Code should not contain trailing whitespace
#pragma warning disable SA1100 // DoNotPrefixCallsWithBaseUnlessLocalImplementationExists
#pragma warning disable SA1101 // Prefix local calls with this
#pragma warning disable SA1106 // Code should not contain empty statements
#pragma warning disable SA1107 // Code should not contain multiple statements on one line
#pragma warning disable SA1108 // Block statements should not contain embedded comments
#pragma warning disable SA1110 // Opening parenthesis should be on declaration line
#pragma warning disable SA1111 // Closing parenthesis should be on line of last parameter
#pragma warning disable SA1114 // Parameter list should follow declaration
#pragma warning disable SA1116 // Split parameters should start on line after declaration
#pragma warning disable SA1117 // Parameters should be on same line or separate lines
#pragma warning disable SA1119 // Statement should not use unnecessary parenthesis
#pragma warning disable SA1120 // Comments should contain text
#pragma warning disable SA1122 // Use string.Empty for empty strings
#pragma warning disable SA1124 // DoNotUseRegions
#pragma warning disable SA1128 // Put constructor initializers on their own line
#pragma warning disable SA1131 // Constant values should appear on the right-hand side
#pragma warning disable SA1132 // Do not combine fields
#pragma warning disable SA1134 // Attributes should not share line
#pragma warning disable SA1136 // Enum values should be on separate lines
#pragma warning disable SA1137 // Elements should have the same indentation
#pragma warning disable SA1139 // Use literal suffix notation
#pragma warning disable SA1200 // UsingDirectivesMustBePlacedCorrectly
#pragma warning disable SA1201 // A field should not follow a method
#pragma warning disable SA1202 // 'public' members should come before 'private' members
#pragma warning disable SA1203 // Constant fields should appear before non-constant fields
#pragma warning disable SA1204 // Static members should appear before non-static members
#pragma warning disable SA1206 // Declaration keywords should follow order
#pragma warning disable SA1208 // Using directive ordering
#pragma warning disable SA1209 // Using alias directives should be placed after other using directives
#pragma warning disable SA1210 // Using directives should be ordered alphabetically by namespace
#pragma warning disable SA1211 // Using alias directive ordering
#pragma warning disable SA1300 // Element should begin with an uppercase letter
#pragma warning disable SA1302 // Interface names should begin with I
#pragma warning disable SA1306 // Field names should begin with lower-case letter
#pragma warning disable SA1307 // Field 'Offset' should begin with upper-case letter
#pragma warning disable SA1310 // Field should not contain an underscore
#pragma warning disable SA1311 // Static readonly fields should begin with upper-case letter
#pragma warning disable SA1312 // Variable should begin with lower-case letter
#pragma warning disable SA1313 // Parameter should begin with lower-case letter
#pragma warning disable SA1400 // Member should declare an access modifier
#pragma warning disable SA1401 // Field should be private
#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1403 // File may only contain a single namespace
#pragma warning disable SA1404 // Code analysis suppression should have justification
#pragma warning disable SA1405 // Debug.Assert should provide message text
#pragma warning disable SA1407 // Arithmetic expressions should declare precedence
#pragma warning disable SA1408 // Conditional expressions should declare precedence
#pragma warning disable SA1413 // UseTrailingCommasInMultiLineInitializers
#pragma warning disable SA1500 // Braces for multi-line statements should not share line
#pragma warning disable SA1501 // Statement should not be on a single line
#pragma warning disable SA1502 // Element should not be on a single line
#pragma warning disable SA1503 // Braces should not be omitted
#pragma warning disable SA1505 // An opening brace should not be followed by a blank line
#pragma warning disable SA1507 // Code should not contain multiple blank lines in a row
#pragma warning disable SA1508 // A closing brace should not be preceded by a blank line
#pragma warning disable SA1509 // Opening braces should not be preceded by blank line
#pragma warning disable SA1510 // Chained statement blocks should not be preceded by blank line
#pragma warning disable SA1512 // SingleLineCommentsMustNotBeFollowedByBlankLine
#pragma warning disable SA1513 // Closing brace should be followed by blank line
#pragma warning disable SA1514 // Element documentation header should be preceded by blank line
#pragma warning disable SA1515 // Single-line comment should be preceded by blank line
#pragma warning disable SA1516 // Elements should be separated by blank line
#pragma warning disable SA1519 // Braces should not be omitted from multi-line child statement
#pragma warning disable SA1520 // Use braces consistently
#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable SA1601 // Partial elements should be documented
#pragma warning disable SA1602 // Enumeration items should be documented
#pragma warning disable SA1604 // Element documentation should have summary
#pragma warning disable SA1611 // Element parameters should be documented
#pragma warning disable SA1614 // Element parameter documentation should have text
#pragma warning disable SA1615 // Element return value should be documented
#pragma warning disable SA1616 // Element return value documentation should have text
#pragma warning disable SA1618 // The documentation for type parameter is missing
#pragma warning disable SA1619 // The documentation for type parameter is missing
#pragma warning disable SA1622 // Generic type parameter documentation should have text
#pragma warning disable SA1623 // The property's documentation summary text should begin with
#pragma warning disable SA1625 // Element documentation should not be copied and pasted
#pragma warning disable SA1626 // Single-line comments should not use documentation syntax
#pragma warning disable SA1629 // Documentation text should end with a period
#pragma warning disable SA1633 // The file header is missing or not located at the top
#pragma warning disable SA1642 // Constructor summary documentation should begin with standard text
#pragma warning disable SA1643 // Destructor summary documentation should begin with standard text
#pragma warning disable SA1649 // File name should match first type name
#pragma warning disable IDE0007 // Use implicit type
#pragma warning disable IDE0008 // Use explicit type
#pragma warning disable IDE0011 // Add braces
#pragma warning disable IDE0016 // Use 'throw' expression
#pragma warning disable IDE0017 // Simplify object initialization
#pragma warning disable IDE0018 // Inline variable declaration
#pragma warning disable IDE0019 // Use pattern matching
#pragma warning disable IDE0020 // Use pattern matching
#pragma warning disable IDE0021 // Use expression body for constructors
#pragma warning disable IDE0022 // Use expression body for methods
#pragma warning disable IDE0025 // Use expression body for properties
#pragma warning disable IDE0027 // Use expression body for accessors
#pragma warning disable IDE0028 // Simplify collection initialization
#pragma warning disable IDE0029 // Use coalesce expression
#pragma warning disable IDE0031 // Use null propagation
#pragma warning disable IDE0032 // Use auto property
#pragma warning disable IDE0034 // Simplify 'default' expression
#pragma warning disable IDE0038 // Use pattern matching
#pragma warning disable IDE0040 // Add accessibility modifiers
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0046 // Convert to conditional expression
#pragma warning disable IDE0047 // Remove unnecessary parentheses
#pragma warning disable IDE0048 // Add parentheses for clarity
#pragma warning disable IDE0049 // Simplify names
#pragma warning disable IDE0053 // Use expression body for lambda expressions
#pragma warning disable IDE0054 // Use compound assignment
#pragma warning disable IDE0057 // Use range operator
#pragma warning disable IDE0058 // Expression value is never used
#pragma warning disable IDE0059 // Unnecessary assignment of a value
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0062 // Make local function 'static'
#pragma warning disable IDE0063 // Use simple 'using' statement
#pragma warning disable IDE0065 // Misplaced using directive
#pragma warning disable IDE0066 // Convert switch statement to expression
#pragma warning disable IDE0074 // Use compound assignment
#pragma warning disable IDE0078 // Use pattern matching
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable IDE0161 // Convert to file-scoped namespace declaration
#pragma warning disable IDE0251 // Make member 'readonly'
#pragma warning disable IDE0270 // Use coalesce expression
#pragma warning disable IDE0290 // Use primary constructor
#pragma warning disable IDE0300 // Simplify collection initialization
#pragma warning disable IDE0301 // Simplify collection initialization
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CA1031 // Do not catch general exception types
#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CA1034 // Nested types should not be visible
#pragma warning disable CA1051 // Do not declare visible instance fields
#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1064 // Exceptions should be public
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
#pragma warning disable CA1066 // Implement IEquatable when overriding Object.Equals
#pragma warning disable CA1067 // Override Object.Equals when implementing IEquatable
#pragma warning disable CA1068 // CancellationToken parameters must come last
#pragma warning disable CA1304 // Specify CultureInfo
#pragma warning disable CA1305 // Specify IFormatProvider
#pragma warning disable CA1307 // Specify StringComparison
#pragma warning disable CA1308 // Normalize strings to uppercase
#pragma warning disable CA1309 // Use ordinal string comparison
#pragma warning disable CA1310 // Specify StringComparison for correctness
#pragma warning disable CA1507 // Use nameof to express symbol names
#pragma warning disable CA1510 // Use ArgumentNullException throw helper
#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
#pragma warning disable CA1715 // Identifiers should have correct prefix
#pragma warning disable CA1716 // Identifiers should not match keywords
#pragma warning disable CA1720 // Identifier contains type name
#pragma warning disable CA1721 // Property names should not match get methods
#pragma warning disable CA1724 // Type names should not match namespaces
#pragma warning disable CA1805 // Do not initialize unnecessarily
#pragma warning disable CA1810 // Initialize reference type static fields inline
#pragma warning disable CA1812 // Avoid uninstantiated internal classes
#pragma warning disable CA1819 // Properties should not return arrays
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1825 // Avoid zero-length array allocations
#pragma warning disable CA1829 // Use Length/Count property instead of Count() when available
#pragma warning disable CA1834 // Consider using 'StringBuilder.Append(char)' when applicable
#pragma warning disable CA1841 // Prefer Dictionary.Contains methods
#pragma warning disable CA1845 // Use span-based 'string.Concat'
#pragma warning disable CA1852 // Seal internal types
#pragma warning disable CA1854 // Prefer the 'IDictionary.TryGetValue(TKey, out TValue)' method
#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
#pragma warning disable CA1861 // Avoid constant arrays as arguments
#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable CA2012 // Use ValueTasks correctly
#pragma warning disable CA2201 // Do not raise reserved exception types
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable CA2231 // Overload operator equals on overriding value type Equals
#pragma warning disable CA2241 // Provide correct arguments to formatting methods
#pragma warning disable CA2244 // Do not duplicate indexed element initializations
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type
#pragma warning disable CS8601 // Possible null reference assignment
#pragma warning disable CS8602 // Dereference of a possibly null reference
#pragma warning disable CS8603 // Possible null reference return
#pragma warning disable CS8604 // Possible null reference argument
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability
#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type
#pragma warning disable CS8714 // The type cannot be used as type parameter
#pragma warning disable CS0419 // Ambiguous reference in cref attribute
#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

// ----------------------------------------------------------------------------
// This is a single file version of a fast, simple and accurate JSON serializer 
// and deserializer.
// The serializer should be compatible with the ECMA-404 http://json.org
// And the RFC-4627: https://tools.ietf.org/html/rfc4627
//
// By default, all types are declared internal so that they don't show up in
// the external dependencies of your project when this file is included directly
//
// Latest version of this code is at http://github.com/textamina/jsonite
// ----------------------------------------------------------------------------
//                             Version history
// ----------------------------------------------------------------------------
// Version 1.0                                                xoofx, 2016-02-07
// - Initial version, serializer and deserializer to a simple object 
//   graph. Method for validating a json text.
// ----------------------------------------------------------------------------
namespace Jsonite
{
    /// <summary>
    /// A JSON parser and reflector to Dictionary/List.
    /// </summary>
#if JSONITE_PUBLIC
    public
#else
    internal 
#endif
    static class Json
    {
        private static readonly JsonSettings DefaultSettings = new JsonSettings();
        private static readonly JsonSettings DefaultSettingsForValidate = new JsonSettings();

        /// <summary>
        /// Deserializes the specified json text into an object.
        /// </summary>
        /// <param name="text">A json text.</param>
        /// <param name="settings">The settings used to deserialize.</param>
        /// <returns>An object representing the deserialized json text</returns>
        /// <exception cref="System.ArgumentNullException">if text is null</exception>
        public static object Deserialize(string text, JsonSettings settings = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            return Deserialize(new StringReader(text), settings);
        }

        /// <summary>
        /// Deserializes the specified json text into an object.
        /// </summary>
        /// <param name="reader">The reader providing a json text.</param>
        /// <param name="settings">The settings used to deserialize.</param>
        /// <returns>An object representing the deserialized json text</returns>
        /// <exception cref="System.ArgumentNullException">if reader is null</exception>
        public static object Deserialize(TextReader reader, JsonSettings settings = null)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            var parser = new JsonReader(reader, settings ?? DefaultSettings);
            return parser.Parse(null, typeof(object), false);
        }

        /// <summary>
        /// Validates the specified json text.
        /// </summary>
        /// <param name="text">A json text.</param>
        /// <param name="settings">The settings used to deserialize.</param>
        /// <exception cref="System.ArgumentNullException">if reader is null</exception>
        /// <exception cref="JsonException">if the json text is not valid</exception>
        public static void Validate(string text, JsonSettings settings = null)
        {
            Validate(new StringReader(text), settings);
        }

        /// <summary>
        /// Validates the specified json text.
        /// </summary>
        /// <param name="reader">The reader providing a json text.</param>
        /// <param name="settings">The settings used to deserialize.</param>
        /// <exception cref="System.ArgumentNullException">if reader is null</exception>
        /// <exception cref="JsonException">if the json text is not valid</exception>
        public static void Validate(TextReader reader, JsonSettings settings = null)
        {
            settings = settings ?? DefaultSettingsForValidate;
            settings.Reflector = JsonReflectorForValidate.Default;
            Deserialize(reader, settings);
        }

        /// <summary>
        /// Serializes the specified value to a json text.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="settings">The settings used to serialize.</param>
        /// <returns>A json string representation of the serialized value</returns>
        public static string Serialize(object value, JsonSettings settings = null)
        {
            var stringWriter = new StringWriter(CultureInfo.InvariantCulture);
            Serialize(value, stringWriter, settings);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Serializes the specified value to a json text.
        /// </summary>
        /// <param name="value">The value to serialize.</param>
        /// <param name="writer">The output writer that will contains the serialized json text.</param>
        /// <param name="settings">The settings used to serialize.</param>
        public static void Serialize(object value, TextWriter writer, JsonSettings settings = null)
        {
            var jsonWriter = new JsonWriter(writer, settings ?? DefaultSettings);
            jsonWriter.Write(value);
        }

        /// <summary>
        /// The internal JsonReader used to deserialize a json text into an object graph.
        /// </summary>
        private struct JsonReader
        {
            private int offset;
            private int line;
            private int column;
            private char c;
            private readonly JsonSettings settings;
            private readonly IJsonReflector reflector;
            private readonly StringBuilder builder;
            private const char Eof = '\0';
            private bool isEof;
            private readonly bool isValidate;
            private int level;

            public JsonReader(TextReader reader, JsonSettings settings)
            {
                Reader = reader;
                this.settings = settings;
                this.reflector = settings.Reflector ?? JsonReflectorDefault.Instance;
                reflector.Initialize(settings);
                isValidate = reflector is JsonReflectorForValidate;
                offset = 0;
                line = 0;
                column = 0;
                c = Eof;
                level = 0;
                isEof = false;
                builder = new StringBuilder();
                NextCharSkipWhitespaces();
            }

            private TextReader Reader { get; }

            public object Parse(object existingObject, Type expectedType, bool expectValue)
            {
                switch (c)
                {
                    case '{':
                        return ParseObject(existingObject, expectedType);
                    case '[':
                        return ParseArray(existingObject, expectedType);
                    case '"':
                        return ParseString();
                    case 't':
                        return ParseTrue();
                    case 'f':
                        return ParseFalse();
                    case 'n':
                        return ParseNull();
                    default:
                        if (c == '-' || IsDigit(c))
                        {
                            return ParseNumber();
                        }

                        if (c != Eof)
                        {
                            RaiseUnexpected("");
                        }
                        break;

                }

                if (expectValue)
                {
                    RaiseUnexpected("while parsing a value. Expecting OBJECT, ARRAY, STRING, NUMBER, true, false or null"); // unit-test: 020-test-error-object3.txt
                }

                return null;
            }

            private void IncrementLevel()
            {
                level++;
                if (settings.MaxDepth > 0 && level > settings.MaxDepth)
                {
                    RaiseException("The maximum allowed depth [{settings.MaxDepth}] level has been reached. The object graph is too deep");
                }
            }

            private void DecrementLevel()
            {
                level--;
            }

            private object ParseObject(object obj, Type expectedType)
            {
                IncrementLevel();

                NextCharSkipWhitespaces(); // Skip starting {

                // If we are deserializing to a value that is the same as the target, we can reuse it

                object objectContext;
                obj = reflector.OnDeserializeEnterObject(obj, expectedType, out objectContext);

                bool expectMember = false;

                while (c != Eof)
                {
                    if (c == '"')
                    {
                        // Deserialize the member
                        var memberName = ParseString();

                        if (c != ':')
                        {
                            RaiseUnexpected($"while parsing an object. Expecting a colon ':' after a member");  // unit test: 020-test-error-object2.txt
                        }

                        NextCharSkipWhitespaces();

                        Type memberExpectedType;
                        object memberContext;
                        object memberExistingValue;
                        reflector.OnDeserializePrepareMemberForObject(objectContext, obj, memberName, out memberExpectedType, out memberContext, out memberExistingValue);

                        // Deserialize the value
                        var value = Parse(memberExistingValue, memberExpectedType, true);

                        // Sets the value on the object
                        reflector.OnDeserializeSetObjectMember(objectContext, obj, memberContext, value);
                        expectMember = false;

                        if (c == ',')
                        {
                            NextCharSkipWhitespaces();
                            expectMember = true;
                            continue;
                        }
                    }

                    if (c == '}')
                    {
                        break;
                    }

                    RaiseUnexpected("while parsing an object. Expecting a STRING or '}'"); // unit test: 020-test-error-object1.txt
                }

                if (c == Eof)
                {
                    RaiseUnexpected("while parsing an object"); // unit-test: 020-test-error-object4.txt
                }
                else if (expectMember && !settings.AllowTrailingCommas)
                {
                    RaiseUnexpected("while parsing an object. Expecting a STRING after a comma ','");  // unit-test: 020-test-error-object5.txt
                }

                NextCharSkipWhitespaces(); // Skip closing }

                var result = reflector.OnDeserializeExitObject(objectContext, obj);
                DecrementLevel();
                return result;
            }

            private object ParseArray(object array, Type expectedType)
            {
                IncrementLevel();
                NextCharSkipWhitespaces(); // Skip starting [

                Type expectedArrayItemType;
                object arrayContext;
                array = reflector.OnDeserializeEnterArray(array, expectedType, out expectedArrayItemType, out arrayContext);
                bool expectItem = false;

                int index = 0;
                while (c != Eof)
                {
                    if (c == ']')
                    {
                        break;
                    }

                    var value = Parse(null, expectedArrayItemType, true);
                    expectItem = false;

                    // Add the item to the array
                    reflector.OnDeserializeAddArrayItem(arrayContext, array, index++, value);

                    if (c == ']')
                    {
                        break;
                    }

                    if (c == ',')
                    {
                        NextCharSkipWhitespaces();
                        expectItem = true;
                    }
                    else
                    {
                        RaiseUnexpected("while parsing an array"); // unit-test: 030-test-error-array2.txt
                    }
                }

                if (c == Eof)
                {
                    RaiseUnexpected("while parsing an array"); // unit-test: 030-test-error-array1.txt
                }
                else if (expectItem && !settings.AllowTrailingCommas)
                {
                    RaiseUnexpected("while parsing an array. Expecting a STRING, NUMBER, OBJECT, ARRAY, true, false or null after a comma ','"); // unit-test: 030-test-error-array3.txt
                }

                NextCharSkipWhitespaces(); // Skip closing ]

                var result = reflector.OnDeserializeExitArray(arrayContext, array);
                DecrementLevel();
                return result;
            }

            private string ParseString()
            {
                NextChar(); // Skip " but don't skip whitespaces
                builder.Length = 0;
                while (true)
                {
                    // Handle escape
                    switch (c)
                    {
                        case '\\':
                            NextChar();
                            switch (c)
                            {
                                case '"':
                                    builder.Append('"');
                                    NextChar();
                                    continue;
                                case '\\':
                                    builder.Append('\\');
                                    NextChar();
                                    continue;
                                case '/':
                                    builder.Append('/');
                                    NextChar();
                                    continue;
                                case 'b':
                                    builder.Append('\b');
                                    NextChar();
                                    continue;
                                case 'f':
                                    builder.Append('\f');
                                    NextChar();
                                    continue;
                                case 'n':
                                    builder.Append('\n');
                                    NextChar();
                                    continue;
                                case 'r':
                                    builder.Append('\r');
                                    NextChar();
                                    continue;
                                case 't':
                                    builder.Append('\t');
                                    NextChar();
                                    continue;
                                case 'u':
                                    NextChar();
                                    // Must be followed 4 hex numbers (0000-FFFF)
                                    if (IsHex(c)) // 1
                                    {
                                        var value = HexToInt(c);
                                        NextChar();
                                        if (IsHex(c)) // 2
                                        {
                                            value = (value << 4) | HexToInt(c);
                                            NextChar();
                                            if (IsHex(c)) // 3
                                            {
                                                value = (value << 4) | HexToInt(c);
                                                NextChar();
                                                if (IsHex(c)) // 4
                                                {
                                                    value = (value << 4) | HexToInt(c);
                                                    builder.Append((char)value);
                                                    NextChar();
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    RaiseUnexpected("while parsing a string. Expecting only hexadecimals [0-9a-fA-F] after escape \\u"); // unit-test: 001-test-error-string4.txt
                                    goto end_of_parsing;
                            }
                            RaiseUnexpected("while parsing a string. Only \\ \" b f n r t v u0000-uFFFF are allowed"); // unit-test: 001-test-error-string1.txt
                            goto end_of_parsing;
                        case Eof:
                            RaiseUnexpected("while parsing a string"); // unit-test: 001-test-error-string2.txt
                            goto end_of_parsing;
                        case '"':
                            NextCharSkipWhitespaces();
                            goto end_of_parsing;
                        default:
                            if (c < ' ')
                            {
                                RaiseUnexpected("while parsing a string. Use escape \\ instead"); // unit-test: 001-test-error-string3.txt
                            }
                            builder.Append(c);
                            NextChar();
                            break;
                    }
                }
                end_of_parsing:

                // If we are validating, no need to create a string as we won't use it
                return isValidate ? null : builder.ToString();
            }

            private object ParseNumber()
            {
                bool isFloat = false;
                bool hasExponent = false;
                bool isNegative = false;
                builder.Length = 0;
                if (c == '-')
                {
                    isNegative = true;
                    builder.Append(c);
                    NextChar();
                }

                if (!IsDigit(c))
                {
                    RaiseUnexpected("while parsing a number after a '-'. Expecting a digit 0-9"); // unit-test: 002-test-error-number1.txt
                }

                // If number starts by 0, we don't expect any digit after
                if (c == '0')
                {
                    builder.Append(c);
                    NextChar();

                    // Make sure that we don't have a digit after
                    if (IsDigit(c))
                    {
                        RaiseUnexpected("while parsing a number. The number '0' must followed by '.' or by an exponent or nothing"); // unit-test: 002-test-error-number2.txt
                    }
                }
                else
                {
                    // Else number starts by non-0, so we can advance as much digits as we have
                    do
                    {
                        builder.Append(c);
                        NextChar();
                    } while (IsDigit(c));
                }

                if (c == '.')
                {
                    isFloat = true;
                    builder.Append('.');
                    NextChar();

                    if (!IsDigit(c))
                    {
                        RaiseUnexpected("while parsing the floating part of a number. Expecting a digit 0-9 after a period '.'"); // unit-test: 002-test-error-number3.txt
                    }

                    do
                    {
                        builder.Append(c);
                        NextChar();
                    } while (IsDigit(c));
                }

                if (c == 'e' || c == 'E')
                {
                    hasExponent = true;

                    builder.Append(c);
                    NextChar();
                    if (c == '+' || c == '-')
                    {
                        builder.Append(c);
                        NextChar();
                    }

                    if (!IsDigit(c))
                    {
                        RaiseUnexpected("while parsing the exponent of a number. Expecting a digit 0-9 after an exponent"); // unit-test: 002-test-error-number4.txt
                    }

                    do
                    {
                        builder.Append(c);
                        NextChar();
                    } while (IsDigit(c));
                }

                // Skip any whitespaces after a value
                while (IsWhiteSpace(c))
                {
                    NextChar();
                }

                // If we are expecting to parse only things into strings, early exit here
                if (settings.ParseValuesAsStrings)
                {
                    return builder.ToString();
                }

                if (isFloat || hasExponent)
                {
                    var numberAsText = builder.ToString();
                    if (settings.ParseFloatAsDecimal)
                    {
                        decimal decimalNumber;
                        if (decimal.TryParse(numberAsText, NumberStyles.Float, CultureInfo.InvariantCulture, out decimalNumber))
                        {
                            return decimalNumber;
                        }
                    }
                    else
                    {
                        double doubleNumber;
                        if (double.TryParse(numberAsText, NumberStyles.Float, CultureInfo.InvariantCulture, out doubleNumber))
                        {
                            return doubleNumber;
                        }
                    }
                }
                else
                {
                    // Fast parse for all integers smaller than  -999999999 <= value <= 999999999
                    // 2147483647
                    //  999999999
                    const int maxIntStringEasyParse = 9;
                    int intNumber = 0;
                    if (builder.Length <= (isNegative ? maxIntStringEasyParse + 1 : maxIntStringEasyParse))
                    {
                        for (int i = isNegative ? 1 : 0; i < builder.Length; i++)
                        {
                            intNumber = intNumber * 10 + (builder[i] - '0');
                        }
                        return isNegative ? -intNumber : intNumber;
                    }

                    // Else go the long way
                    
                    // Try first to parse to an int
                    var numberAsText = builder.ToString();
                    if (int.TryParse(numberAsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out intNumber))
                    {
                        return intNumber;
                    }

                    // Then a long
                    long longNumber;
                    if (long.TryParse(numberAsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out longNumber))
                    {
                        return longNumber;
                    }

                    // Or an ulong
                    ulong ulongNumber;
                    if (ulong.TryParse(numberAsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out ulongNumber))
                    {
                        return ulongNumber;
                    }

                    // Or a decimal
                    decimal decimalNumber;
                    if (decimal.TryParse(numberAsText, NumberStyles.Integer, CultureInfo.InvariantCulture, out decimalNumber))
                    {
                        return decimalNumber;
                    }
                }

                RaiseException($"Unable to parse number [{builder}] to a valid C# number ");
                return null;
            }

            private object ParseTrue()
            {
                NextChar();
                if (c == 'r')
                {
                    NextChar();
                    if (c == 'u')
                    {
                        NextChar();
                        if (c == 'e')
                        {
                            NextCharSkipWhitespaces();
                            return settings.ParseValuesAsStrings ? (object)"true" : true;
                        }
                    }
                }
                RaiseUnexpected("while trying to parse a BOOL 'true' value"); // unit-test: 000-test-error-true1.txt and 000-test-error-true2.txt
                return null;
            }

            private object ParseFalse()
            {
                NextChar();
                if (c == 'a')
                {
                    NextChar();
                    if (c == 'l')
                    {
                        NextChar();
                        if (c == 's')
                        {
                            NextChar();
                            if (c == 'e')
                            {
                                NextCharSkipWhitespaces();
                                return settings.ParseValuesAsStrings ? (object)"false" : false;
                            }
                        }
                    }
                }
                RaiseUnexpected("while trying to parse a BOOL 'false' value"); // unit-test: 000-test-error-false1.txt
                return null;
            }

            private object ParseNull()
            {
                NextChar();
                if (c == 'u')
                {
                    NextChar();
                    if (c == 'l')
                    {
                        NextChar();
                        if (c == 'l')
                        {
                            NextCharSkipWhitespaces();
                            return null;
                        }
                    }
                }
                RaiseUnexpected("while trying to parse the NULL 'null' value"); // unit-test: 000-test-error-null1.txt
                return null;
            }

            private void RaiseException(string message)
            {
                reflector.OnDeserializeRaiseParsingError(offset, line, column, message, null);
            }

            private void RaiseUnexpected(string message)
            {
                RaiseException((isEof ? "Unexpected EOF " : $"Unexpected character '{EscapeChar(c)}' ") + message);
            }

            private void NextCharSkipWhitespaces()
            {
                do
                {
                    NextChar();
                } while (IsWhiteSpace(c));
            }

            [MethodImpl((MethodImplOptions)256)]
            private void NextChar()
            {
                var nextChar = Reader.Read();
                if (nextChar < 0)
                {
                    if (c != Eof)
                    {
                        column++;
                        offset++;
                    }
                    isEof = true;
                    c = Eof;
                    return;
                }

                if (c == '\n')
                {
                    offset++;
                    column = 0;
                    line++;
                }
                else if (c != Eof)
                {
                    offset++;
                    column++;
                }
                c = (char)nextChar;
            }

            [MethodImpl((MethodImplOptions)256)]
            private static bool IsWhiteSpace(char c)
            {
                return c == ' ' || c == '\n' || c == '\t' || c == '\r';
            }

            [MethodImpl((MethodImplOptions)256)]
            private static bool IsDigit(char c)
            {
                return c >= '0' && c <= '9';
            }

            [MethodImpl((MethodImplOptions)256)]
            private static int HexToInt(char c)
            {
                if (c >= '0' && c <= '9')
                {
                    return c - '0';
                }
                if (c >= 'a' && c <= 'f')
                {
                    return c - 'a' + 10;
                }
                return c - 'A' + 10;
            }

            [MethodImpl((MethodImplOptions)256)]
            private static bool IsHex(char c)
            {
                return (c >= '0' && c <= '9') ||
                       (c >= 'a' && c <= 'f') ||
                       (c >= 'A' && c <= 'F');
            }
        }

        /// <summary>
        /// The internal class used to serialize an object graph to a json text.
        /// </summary>
        private class JsonWriter
        {
            private readonly TextWriter writer;
            private readonly JsonSettings settings;
            private readonly IJsonReflector reflector;
            private readonly bool indent;
            private readonly char indentChar;
            private readonly int indentCount;
            private int indentLevel;
            private readonly Dictionary<Type, Action<object>> writers;

            public JsonWriter(TextWriter writer, JsonSettings settings)
            {
                this.settings = settings;
                this.reflector = settings.Reflector ?? JsonReflectorDefault.Instance;
                this.writer = writer;
                this.indent = settings.Indent;
                this.indentChar = settings.IndentChar;
                this.indentCount = settings.IndentCount;
                this.indentLevel = 0;
                writers = new Dictionary<Type, Action<object>>()
                {
                    // These converters have to match to the one declared in JsonReflectorDefault.Converters
                    {typeof (string), value => WriteString((string) value)},
                    {typeof (bool), value => { writer.Write((bool) value ? "true" : "false"); }},
                    {typeof (char), value =>  { writer.Write(((char) value).ToString()); }},
                    {typeof (byte), value =>  { writer.Write(((byte) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (sbyte), value => { writer.Write(((sbyte) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (short), value =>  { writer.Write(((short) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (ushort), value => { writer.Write(((ushort) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (int), value =>  { writer.Write(((int) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (uint), value => { writer.Write(((uint) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (long), value =>  { writer.Write(((long) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (ulong), value => { writer.Write(((ulong) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (float), value =>  { writer.Write(((double)(float) value).ToString("R", CultureInfo.InvariantCulture)); }},
                    {typeof (double), value => { writer.Write(((double) value).ToString("R", CultureInfo.InvariantCulture)); }},
                    {typeof (decimal), value =>  { writer.Write(((decimal) value).ToString(CultureInfo.InvariantCulture)); }},
                    {typeof (Type), value => { WriteString(value.ToString()); }},
                    {typeof (Guid), value =>  { WriteString(((Guid)value).ToString("D")); }},
                    {typeof (StringBuilder), value => WriteString(((StringBuilder) value).ToString())}, // TODO: Could be optimized but it is a very uncommon case, so...
                    {typeof (DateTime), value => { WriteString(((DateTime)value).ToString(CultureInfo.InvariantCulture)); }}, // TODO: handle correctly
                    {typeof (TimeSpan), value => { WriteString(((TimeSpan)value).ToString()); }}, // TODO: handle correctly
                };
            }

            public void Write(object value)
            {
                if (value == null)
                {
                    writer.Write("null");
                    return;
                }

                var type = value.GetType();
                Action<object> valueWriter;

                if (writers.TryGetValue(type, out valueWriter))
                {
                    valueWriter(value);
                    return;
                }

                object objectContext;
                var objectType = reflector.OnSerializeGetObjectType(value, type, out objectContext);
                switch (objectType)
                {
                    case JsonObjectType.Object:
                        WriteObject(reflector.OnSerializeGetObjectMembers(objectContext, value));
                        break;
                    case JsonObjectType.Array:
                        WriteArray(reflector.OnSerializeGetArrayItems(objectContext, value));
                        break;
                    default:
                        // Try to serialize as a string
                        WriteString(Convert.ToString(value, CultureInfo.InvariantCulture));
                        break;
                }

                //throw new InvalidOperationException($"Unsupported object type [{value.GetType()}]");
            }

            private void WriteObject(IEnumerable<KeyValuePair<string, object>> members)
            {
                writer.Write('{');
                indentLevel++;
                bool isFirst = true;
                foreach (var keyValue in members)
                {
                    if (!isFirst)
                    {
                        writer.Write(',');
                    }

                    if (indent)
                    {
                        writer.Write('\n');
                        Indent();
                    }

                    WriteString(keyValue.Key);
                    writer.Write(':');
                    if (indent)
                    {
                        writer.Write(' ');
                    }
                    Write(keyValue.Value);
                    isFirst = false;
                }
                indentLevel--;
                if (!isFirst && indent)
                {
                    writer.Write('\n');
                    Indent();
                }
                writer.Write('}');
            }

            private void WriteArray(IEnumerable list)
            {
                // Serialize list
                writer.Write('[');
                indentLevel++;
                bool isFirst = true;
                foreach (var item in list)
                {
                    if (!isFirst)
                    {
                        writer.Write(',');
                    }
                    if (indent)
                    {
                        writer.Write('\n');
                        Indent();
                    }
                    Write(item);
                    isFirst = false;
                }
                indentLevel--;
                if (!isFirst && indent)
                {
                    writer.Write('\n');
                    Indent();
                }
                writer.Write(']');
            }

            [MethodImpl((MethodImplOptions)256)]
            private void Indent()
            {
                for (int i = 0; i < indentCount * indentLevel; i++)
                {
                    writer.Write(indentChar);
                }
            }

            private void WriteString(string text)
            {
                writer.Write('"');
                for (int i = 0; i < text.Length; i++)
                {
                    var c = text[i];

                    switch (c)
                    {
                        case '"':
                            writer.Write('\\');
                            writer.Write('\"');
                            break;
                        case '\\':
                            writer.Write('\\');
                            writer.Write('\\');
                            break;
                        case '\b':
                            writer.Write('\\');
                            writer.Write('b');
                            break;
                        case '\f':
                            writer.Write('\\');
                            writer.Write('f');
                            break;
                        case '\n':
                            writer.Write('\\');
                            writer.Write('n');
                            break;
                        case '\r':
                            writer.Write('\\');
                            writer.Write('r');
                            break;
                        case '\t':
                            writer.Write('\\');
                            writer.Write('t');
                            break;
                        default:
                            if (c < ' ')
                            {
                                writer.Write('\\');
                                writer.Write('u');
                                writer.Write(((int)c).ToString("X4", CultureInfo.InvariantCulture));
                            }
                            else if (IsHighSurrogate(c) || IsLowSurrogate(c))
                            {
                                writer.Write('\\');
                                writer.Write('u');
                                writer.Write(((int)c).ToString("X4", CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                writer.Write(c);
                            }
                            break;
                    }
                }
                writer.Write('"');
            }
        }

        [MethodImpl((MethodImplOptions)256)]
        private static bool IsHighSurrogate(char c)
        {
            if ((int)c >= 55296)
                return (int)c <= 56319;
            return false;
        }

        [MethodImpl((MethodImplOptions)256)]
        private static bool IsLowSurrogate(char c)
        {
            if ((int)c >= 56320)
                return (int)c <= 57343;
            return false;
        }


        private static string EscapeChar(char chr)
        {
            // http://stackoverflow.com/questions/12309104/how-to-print-control-characters-in-console-window
            switch (chr)
            {
                case '\'':
                    return @"\'";
                case '"':
                    return "\\\"";
                case '\\':
                    return @"\\";
                case '\0':
                    return @"\0";
                case '\a':
                    return @"\a";
                case '\b':
                    return @"\b";
                case '\f':
                    return @"\f";
                case '\n':
                    return @"\n";
                case '\r':
                    return @"\r";
                case '\t':
                    return @"\t";
                case '\v':
                    return @"\v";
                default:
                    if (char.IsControl(chr) || IsHighSurrogate(chr) || IsLowSurrogate(chr))
                        return @"\u" + ((int)chr).ToString("X4");
                    else
                        return new string(chr, 1);
            }
        }

        private sealed class JsonReflectorForValidate : IJsonReflector
        {
            public static readonly JsonReflectorForValidate Default = new JsonReflectorForValidate();

            public void Initialize(JsonSettings settings)
            {
            }

            public object OnDeserializeEnterObject(object obj, Type expectedType, out object objectContext)
            {
                objectContext = null;
                return null;
            }

            public void OnDeserializePrepareMemberForObject(object objectContext, object obj, string member, out Type memberType,
                out object memberContext, out object existingMemberValue)
            {
                memberType = typeof (object);
                memberContext = null;
                existingMemberValue = null;
            }

            public void OnDeserializeSetObjectMember(object objectContext, object obj, object memberContext, object value)
            {
            }

            public object OnDeserializeExitObject(object objectContext, object obj)
            {
                return null;
            }

            public object OnDeserializeEnterArray(object obj, Type expectedType, out Type expectedArrayTypeItem, out object arrayContext)
            {
                expectedArrayTypeItem = null;
                arrayContext = null;
                return null;
            }

            public void OnDeserializeAddArrayItem(object arrayContext, object array, int index, object value)
            {
            }

            public object OnDeserializeExitArray(object arrayContext, object obj)
            {
                return null;
            }

            public void OnDeserializeRaiseParsingError(int offset, int line, int column, string message, Exception inner)
            {
                throw new JsonException(offset, line, column, message, inner);
            }


            public JsonObjectType OnSerializeGetObjectType(object obj, Type type, out object objectContext)
            {
                throw new NotImplementedException();
            }

            public bool IsObjectType(Type type)
            {
                throw new NotImplementedException();
            }

            public bool IsArrayType(Type type)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<KeyValuePair<string, object>> OnSerializeGetObjectMembers(object objectContext, object obj)
            {
                throw new NotImplementedException();
            }

            public IEnumerable OnSerializeGetArrayItems(object objectContext, object array)
            {
                throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// The default object used when a deserializing to an object type.
    /// </summary>
#if JSONITE_PUBLIC
    public
#else
    internal 
#endif
    class JsonObject : Dictionary<string, object>
    {
        public override string ToString()
        {
            return Json.Serialize(this);
        }
    }

    /// <summary>
    /// The default array used when deserializing to an array type.
    /// </summary>
#if JSONITE_PUBLIC
    public
#else
    internal 
#endif
    class JsonArray : List<object>
    {
        public override string ToString()
        {
            return Json.Serialize(this);
        }
    }

    /// <summary>
    /// Instance exception used when a parsing exception occured.
    /// </summary>
    /// <remarks>
    /// This exception can be overriden by overriding the method <see cref="IJsonReflector.OnDeserializeRaiseParsingError"/>.
    /// </remarks>
#if JSONITE_PUBLIC
    public
#else
    internal 
#endif
    class JsonException : Exception
    {
        public JsonException(int offset, int line, int column, string message, Exception inner = null) : base(message, inner)
        {
            Offset = offset;
            Line = line;
            Column = column;
        }

        /// <summary>
        /// Character offset from the beginning of the text being parsed.
        /// </summary>
        public readonly int Offset;

        /// <summary>
        /// Line position (zero-based) where the error occured from the beginning of the text being parsed.
        /// </summary>
        public readonly int Line;

        /// <summary>
        /// Column position (zero-based) where the error occured.
        /// </summary>
        public readonly int Column;

        /// <summary>
        /// Prints the line (1-based) and column (1-based).
        /// </summary>
        /// <returns>A string representation of this object</returns>
        public override string ToString()
        {
            var innerMessage = InnerException != null ? " Check inner exception for more details" : string.Empty;
            return $"({Line + 1},{Column + 1}) : error : {Message}{innerMessage}";
        }
    }

    /// <summary>
    /// Defines serialization and deserialization settings used by <see cref="Json.Parse"/> and <see cref="Json.Serialize"/>
    /// </summary>
#if JSONITE_PUBLIC
    public
#else
    internal 
#endif
    class JsonSettings
    {
        public JsonSettings()
        {
            IndentCount = 2;
            IndentChar = ' ';
            Reflector = JsonReflectorDefault.Instance;
        }

        /// <summary>
        /// Gets or sets the maximum depth used when serializing or deserializing.
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating to indent the text when serializing. Default is <c>false</c>.
        /// </summary>
        public bool Indent { get; set; }

        /// <summary>
        /// Gets or sets the number of <see cref="IndentChar"/> used to indent a json output when <see cref="Indent"/> is <c>true</c>.
        /// </summary>
        public int IndentCount { get; set; }

        /// <summary>
        /// Gets or sets the indent character used when <see cref="Indent"/> is <c>true</c>.
        /// </summary>
        public char IndentChar { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether floats should be deserialized to decimal instead of double (default).
        /// </summary>
        public bool ParseFloatAsDecimal { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all values should be deserialized to strings instead of numbers.
        /// </summary>
        public bool ParseValuesAsStrings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow trailing commas in object and array declaration.
        /// </summary>
        public bool AllowTrailingCommas { get; set; }

        /// <summary>
        /// Gets or sets the reflector used for interfacing the json text to an object graph.
        /// </summary>
        public IJsonReflector Reflector { get; set; }
    }

    /// <summary>
    /// A callback interface used during the serialization and deserialization.
    /// </summary>
#if JSONITE_PUBLIC
    public
#else
    internal 
#endif
    interface IJsonReflector
    {
        /// <summary>
        /// Initializes this instance with the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        void Initialize(JsonSettings settings);

        /// <summary>
        /// Called when starting to deserialize an object.
        /// </summary>
        /// <param name="obj">An existing object instance (may be null).</param>
        /// <param name="expectedType">The expected type (not null).</param>
        /// <param name="objectContext">The object context that will be passed to other deserialize methods for objects.</param>
        /// <returns>The object instance to deserialize to. The return value must not be null. This instance can be the input <paramref name="obj"/> if not null, or this method could choose to replace the instance by another during the deserialization.</returns>
        object OnDeserializeEnterObject(object obj, Type expectedType, out object objectContext);

        /// <summary>
        /// Called when deserializing a member, before deserializing its value.
        /// </summary>
        /// <param name="objectContext">The object context that was returned by the <see cref="OnDeserializeEnterObject"/></param>
        /// <param name="obj">The object instance (not null).</param>
        /// <param name="member">The member name being deserialized.</param>
        /// <param name="memberType">Expected type of the member.</param>
        /// <param name="memberContext">The member context that will be passed back to <see cref="OnDeserializeSetObjectMember"/>.</param>
        /// <param name="existingMemberValue">The existing member value if any (may be null).</param>
        void OnDeserializePrepareMemberForObject(object objectContext, object obj, string member, out Type memberType, out object memberContext, out object existingMemberValue);

        /// <summary>
        /// Called when deserializing a member value to effectively set the value for the member on the specified object instance.
        /// </summary>
        /// <param name="objectContext">The object context that was returned by the <see cref="OnDeserializeEnterObject"/></param>
        /// <param name="obj">The object instance (not null).</param>
        /// <param name="memberContext">The member context that was generated by <see cref="OnDeserializePrepareMemberForObject"/>.</param>
        /// <param name="value">The value of the member to set on the object.</param>
        void OnDeserializeSetObjectMember(object objectContext, object obj, object memberContext, object value);

        /// <summary>
        /// Called when deserializing an object is done. This method allows to transform the object to another value.
        /// </summary>
        /// <param name="objectContext">The object context.</param>
        /// <param name="obj">The object instance that has been deserialized.</param>
        /// <returns>The final object deserialized (may be different from <paramref name="obj"/>)</returns>
        object OnDeserializeExitObject(object objectContext, object obj);

        /// <summary>
        /// Called when starting to deserialize an array.
        /// </summary>
        /// <param name="obj">An existing array instance (may be null).</param>
        /// <param name="expectedType">The expected type of the array.</param>
        /// <param name="expectedArrayTypeItem">The expected type of an array item.</param>
        /// <param name="arrayContext">The array context that will be passed to other deserialize methods for arrays.</param>
        /// <returns>The array instance to deserialize to. The return value must not be null.</returns>
        object OnDeserializeEnterArray(object obj, Type expectedType, out Type expectedArrayTypeItem, out object arrayContext);

        /// <summary>
        /// Called when deserializing an array item to add to the specified array instance.
        /// </summary>
        /// <param name="arrayContext">The array context that was returned by the <see cref="OnDeserializeEnterArray"/></param>
        /// <param name="array">The array being deserialized.</param>
        /// <param name="index">The index of the next element (may be used for plain arrays).</param>
        /// <param name="value">The value of the item to add to the array.</param>
        void OnDeserializeAddArrayItem(object arrayContext, object array, int index, object value);

        /// <summary>
        /// Called when deserializing an array is done. This method allows to transform the array to another value (transform a list to a plain .NET array for example)
        /// </summary>
        /// <param name="arrayContext">The array context that was returned by the <see cref="OnDeserializeEnterArray"/></param>
        /// <param name="obj">The array instance that has been deserialized.</param>
        /// <returns>The final array instance deserialized (may be different from <paramref name="obj"/>)</returns>
        object OnDeserializeExitArray(object arrayContext, object obj);

        /// <summary>
        /// Called when an error occured when deserializing. A default implementation should throw a <see cref="JsonException"/>.
        /// </summary>
        /// <param name="offset">The character position from the begining of the buffer being deserialized.</param>
        /// <param name="line">The line position (zero-based)</param>
        /// <param name="column">The column position (zero-based)</param>
        /// <param name="message">The error message.</param>
        /// <param name="inner">An optional inner exception.</param>
        void OnDeserializeRaiseParsingError(int offset, int line, int column, string message, Exception inner);

        /// <summary>
        /// Called when serializing an object, to determine whether the object is an array or a simple object (with members/properties). 
        /// This method is then used to correctly route to <see cref="OnSerializeGetObjectMembers"/> or <see cref="OnSerializeGetArrayItems"/>.
        /// </summary>
        /// <param name="obj">The object instance being serialized</param>
        /// <param name="type">The type of the object being serialized.</param>
        /// <param name="objectContext">An object context that will be passed to other serialize methods.</param>
        /// <returns>The type of the specified object instance (array or object or unknown)</returns>
        JsonObjectType OnSerializeGetObjectType(object obj, Type type, out object objectContext);

        /// <summary>
        /// Called when serializing an object to the members value of this object.
        /// </summary>
        /// <param name="objectContext">The object context that was returned by the <see cref="OnSerializeGetObjectType"/></param>
        /// <param name="obj">The object instance being serialized.</param>
        /// <returns>An enumeration of members [name, value].</returns>
        IEnumerable<KeyValuePair<string, object>> OnSerializeGetObjectMembers(object objectContext, object obj);

        /// <summary>
        /// Called when serializing an array to get the array items.
        /// </summary>
        /// <param name="objectContext">The object context that was returned by the <see cref="OnSerializeGetObjectType"/></param>
        /// <param name="array">The object instance being serialized.</param>
        /// <returns>An enumeration of the array items to serialize.</returns>
        IEnumerable OnSerializeGetArrayItems(object objectContext, object array);
    }

    /// <summary>
    /// Defines the type of object when serializing (returned by method <see cref="IJsonReflector.OnSerializeGetObjectType"/>.
    /// </summary>
#if JSONITE_PUBLIC
    public
#else
    internal 
#endif
    enum JsonObjectType
    {
        /// <summary>
        /// The object type being serialized is unkwown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The object being serialized is an object with members.
        /// </summary>
        Object,

        /// <summary>
        /// The object being serialized is an array (providing <see cref="IEnumerable"/>)
        /// </summary>
        Array,
    }

    /// <summary>
    /// The default implementation of <see cref="IJsonReflector"/> that allows to deserialize a JSON text to a generic <see cref="IDictionary{TKey,TValue}"/> <see cref="JsonObject"/> or <see cref="JsonArray"/>.
    /// </summary>
    /// <seealso cref="IJsonReflector" />
#if JSONITE_PUBLIC
    public
#else
    internal 
#endif
    sealed class JsonReflectorDefault : IJsonReflector
    {
        public static readonly JsonReflectorDefault Instance = new JsonReflectorDefault();

        private JsonReflectorDefault()
        {
        }

        public void Initialize(JsonSettings settings)
        {
        }

        public object OnDeserializeEnterObject(object obj, Type expectedType, out object objectContext)
        {
            if (!typeof(IDictionary<string, object>).GetTypeInfo().IsAssignableFrom(expectedType.GetTypeInfo()) && expectedType != typeof(object))
            {
                throw new ArgumentException($"The default reflector only supports deserializing to a Dictionary<string, object> or a JsonObject instead of [{expectedType}]");
            }

            objectContext = null;
            return expectedType == typeof(object) || expectedType == typeof (JsonObject) || expectedType.GetTypeInfo().IsInterface
                ? new JsonObject()
                : Activator.CreateInstance(expectedType);
        }

        public void OnDeserializePrepareMemberForObject(object objectContext, object obj, string member, out Type expectedMemberType, out object memberContext, out object existingMemberValue)
        {
            memberContext = member;
            expectedMemberType = typeof(object);
            existingMemberValue = null;
        }

        public void OnDeserializeSetObjectMember(object objectContext, object target, object memberContext, object value)
        {
            ((IDictionary<string, object>)target)[(string)memberContext] = value;
        }

        public object OnDeserializeExitObject(object objectContext, object obj)
        {
            return obj;
        }

        public object OnDeserializeEnterArray(object obj, Type expectedType, out Type expectedArrayItemType, out object arrayContext)
        {
            if (!typeof(IList).GetTypeInfo().IsAssignableFrom(expectedType.GetTypeInfo()) && expectedType != typeof(object))
            {
                throw new ArgumentException($"The default reflector only supports deserializing to a IList or a JsonArray instead of [{expectedType}]");
            }

            arrayContext = null;
            expectedArrayItemType = typeof(object);
            return expectedType == typeof(object) || expectedType == typeof(JsonArray) || expectedType.GetTypeInfo().IsInterface
                ? new JsonArray()
                : Activator.CreateInstance(expectedType);
        }

        public void OnDeserializeAddArrayItem(object arrayContext, object array, int index, object value)
        {
            ((IList)array).Add(value);
        }

        public object OnDeserializeExitArray(object arrayContext, object obj)
        {
            return obj;
        }
        public void OnDeserializeRaiseParsingError(int offset, int line, int column, string message, Exception inner)
        {
            throw new JsonException(offset, line, column, message, inner);
        }

        public JsonObjectType OnSerializeGetObjectType(object obj, Type type, out object objectContext)
        {
            objectContext = null;
            var typeInfo = type.GetTypeInfo();
            if (typeof(IDictionary<string, object>).GetTypeInfo().IsAssignableFrom(typeInfo))
            {
                return JsonObjectType.Object;
            }

            if (typeof(IList).GetTypeInfo().IsAssignableFrom(typeInfo) ||
                     typeof(IList<object>).GetTypeInfo().IsAssignableFrom(typeInfo))
            {
                return JsonObjectType.Array;
            }
            return JsonObjectType.Unknown;
        }

        public IEnumerable<KeyValuePair<string, object>> OnSerializeGetObjectMembers(object objectContext, object obj)
        {
            return ((IDictionary<string, object>)obj);
        }

        public IEnumerable OnSerializeGetArrayItems(object objectContext, object array)
        {
            return (IEnumerable)array;
        }
    }
#if NETPRE45
    static class ReflectionHelper
    {
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
    }
#endif
}