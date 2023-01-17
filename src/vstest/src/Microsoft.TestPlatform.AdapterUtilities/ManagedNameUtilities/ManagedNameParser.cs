// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Globalization;

using Microsoft.TestPlatform.AdapterUtilities.Helpers;

namespace Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;

public class ManagedNameParser
{
    /// <summary>
    /// Parses a given fully qualified managed type name into its namespace and type name.
    /// </summary>
    /// <param name="managedTypeName">
    /// The fully qualified managed type name to parse.
    /// The format is defined in <see href="https://github.com/microsoft/vstest/blob/main/RFCs/0017-Managed-TestCase-Properties.md#managedtype-property">the RFC</see>.
    /// </param>
    /// <param name="namespaceName">
    /// When this method returns, contains the parsed namespace name of the <paramref name="managedTypeName"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <param name="typeName">
    /// When this method returns, contains the parsed type name of the <paramref name="managedTypeName"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    public static void ParseManagedTypeName(string managedTypeName, out string namespaceName, out string typeName)
    {
        int pos = managedTypeName.LastIndexOf('.');
        if (pos == -1)
        {
            namespaceName = string.Empty;
            typeName = managedTypeName;
        }
        else
        {
            namespaceName = managedTypeName.Substring(0, pos);
            typeName = managedTypeName.Substring(pos + 1);
        }
    }

    /// <summary>
    /// Parses a given fully qualified managed method name into its name, arity and parameter types.
    /// </summary>
    /// <param name="managedMethodName">
    /// The fully qualified managed method name to parse.
    /// The format is defined in <see href="https://github.com/microsoft/vstest/blob/main/RFCs/0017-Managed-TestCase-Properties.md#managedmethod-property">the RFC</see>.
    /// </param>
    /// <param name="methodName">
    /// When this method returns, contains the parsed method name of the <paramref name="managedMethodName"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <param name="arity">
    /// When this method returns, contains the parsed arity of the <paramref name="managedMethodName"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <param name="parameterTypes">
    /// When this method returns, contains the parsed parameter types of the <paramref name="managedMethodName"/>.
    /// If there are no parameter types in <paramref name="managedMethodName"/>, <paramref name="parameterTypes"/> is set to <see langword="null"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <exception cref="InvalidManagedNameException">
    /// Thrown if <paramref name="managedMethodName"/> contains spaces, incomplete, or the arity isn't numeric.
    /// </exception>
    public static void ParseManagedMethodName(string managedMethodName, out string methodName, out int arity, out string[]? parameterTypes)
    {
        int pos = ParseMethodName(managedMethodName, 0, out var escapedMethodName, out arity);
        methodName = ReflectionHelpers.ParseEscapedString(escapedMethodName);
        pos = ParseParameterTypeList(managedMethodName, pos, out parameterTypes);
        if (pos != managedMethodName.Length)
        {
            string message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorUnexpectedCharactersAtEnd, pos);
            throw new InvalidManagedNameException(message);
        }
    }

    private static string Capture(string managedMethodName, int start, int end)
        => managedMethodName.Substring(start, end - start);

    private static int ParseMethodName(string managedMethodName, int start, out string methodName, out int arity)
    {
        var i = start;
        var quoted = false;
        for (; i < managedMethodName.Length; i++)
        {
            var c = managedMethodName[i];
            if (c == '\'' || quoted)
            {
                quoted = c == '\'' ? !quoted : quoted;
                continue;
            }

            switch (c)
            {
                case var w when char.IsWhiteSpace(w):
                    string message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorWhitespaceNotValid, i);
                    throw new InvalidManagedNameException(message);

                case '`':
                    methodName = Capture(managedMethodName, start, i);
                    return ParseArity(managedMethodName, i, out arity);

                case '(':
                    methodName = Capture(managedMethodName, start, i);
                    arity = 0;
                    return i;
            }
        }
        methodName = Capture(managedMethodName, start, i);
        arity = 0;
        return i;
    }

    // parse arity in the form `nn where nn is an integer value.
    private static int ParseArity(string managedMethodName, int start, out int arity)
    {
        TPDebug.Assert(managedMethodName[start] == '`');

        int i = start + 1; // skip initial '`' char
        for (; i < managedMethodName.Length; i++)
        {
            if (managedMethodName[i] == '(') break;
        }
        if (!int.TryParse(Capture(managedMethodName, start + 1, i), out arity))
        {
            throw new InvalidManagedNameException(Resources.Resources.ErrorMethodArityMustBeNumeric);
        }
        return i;
    }

    private static int ParseParameterTypeList(string managedMethodName, int start, out string[]? parameterTypes)
    {
        parameterTypes = null;
        if (start == managedMethodName.Length)
        {
            return start;
        }
        TPDebug.Assert(managedMethodName[start] == '(');

        var types = new List<string>();

        int i = start + 1; // skip initial '(' char
        for (; i < managedMethodName.Length; i++)
        {
            switch (managedMethodName[i])
            {
                case ')':
                    if (types.Count != 0)
                    {
                        parameterTypes = types.ToArray();
                    }
                    return i + 1; // consume right parens

                case ',':
                    break;

                default:
                    i = ParseParameterType(managedMethodName, i, out var parameterType);
                    types.Add(parameterType);
                    break;
            }
        }

        throw new InvalidManagedNameException(Resources.Resources.ErrorIncompleteManagedName);
    }

    private static int ParseParameterType(string managedMethodName, int start, out string parameterType)
    {
        parameterType = string.Empty;
        var quoted = false;

        int i;
        for (i = start; i < managedMethodName.Length; i++)
        {
            if (managedMethodName[i] == '\'' || quoted)
            {
                quoted = managedMethodName[i] == '\'' ? !quoted : quoted;
                continue;
            }

            switch (managedMethodName[i])
            {
                case '<':
                    i = ParseGenericBrackets(managedMethodName, i + 1);
                    break;

                case '[':
                    i = ParseArrayBrackets(managedMethodName, i + 1);
                    break;

                case ',':
                case ')':
                    parameterType = Capture(managedMethodName, start, i);
                    return i - 1;

                case var w when char.IsWhiteSpace(w):
                    string message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorWhitespaceNotValid, i);
                    throw new InvalidManagedNameException(message);
            }
        }
        return i;
    }

    private static int ParseArrayBrackets(string managedMethodName, int start)
    {
        var quoted = false;

        for (int i = start; i < managedMethodName.Length; i++)
        {
            if (managedMethodName[i] == '\'' || quoted)
            {
                quoted = managedMethodName[i] == '\'' ? !quoted : quoted;
                continue;
            }

            switch (managedMethodName[i])
            {
                case ']':
                    return i;
                case var w when char.IsWhiteSpace(w):
                    string msg = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorWhitespaceNotValid, i);
                    throw new InvalidManagedNameException(msg);
            }
        }

        throw new InvalidManagedNameException(Resources.Resources.ErrorIncompleteManagedName);
    }

    private static int ParseGenericBrackets(string managedMethodName, int start)
    {
        var quoted = false;

        for (int i = start; i < managedMethodName.Length; i++)
        {
            if (managedMethodName[i] == '\'' || quoted)
            {
                quoted = managedMethodName[i] == '\'' ? !quoted : quoted;
                continue;
            }

            switch (managedMethodName[i])
            {
                case '<':
                    i = ParseGenericBrackets(managedMethodName, i + 1);
                    break;

                case '>':
                    return i;

                case var w when char.IsWhiteSpace(w):
                    string msg = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorWhitespaceNotValid, i);
                    throw new InvalidManagedNameException(msg);
            }
        }

        throw new InvalidManagedNameException(Resources.Resources.ErrorIncompleteManagedName);
    }
}
