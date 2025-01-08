// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.TestPlatform.AdapterUtilities.Helpers;

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;

public static partial class ManagedNameHelper
{
    /// <summary>
    /// Gets fully qualified managed type and method name from given <see href="MethodBase" /> instance.
    /// </summary>
    /// <param name="method">
    /// A <see cref="MethodBase" /> instance to get fully qualified managed type and method name.
    /// </param>
    /// <param name="managedTypeName">
    /// When this method returns, contains the fully qualified managed type name of the <paramref name="method"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// The format is defined in <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md#managedtype-property">the RFC</see>.
    /// </param>
    /// <param name="managedMethodName">
    /// When this method returns, contains the fully qualified managed method name of the <paramref name="method"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// The format is defined in <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md#managedmethod-property">the RFC</see>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> is null.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// <paramref name="method"/> must describe a method.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Required functionality on <paramref name="method"/> is missing on the current platform.
    /// </exception>
    /// <remarks>
    /// More information about <paramref name="managedTypeName"/> and <paramref name="managedMethodName"/> can be found in
    /// <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md">the RFC</see>.
    /// </remarks>
    public static void GetManagedName(MethodBase method, out string managedTypeName, out string managedMethodName)
     => GetManagedNameAndHierarchy(method, false, out managedTypeName, out managedMethodName, out _);

    /// <summary>
    /// Gets fully qualified managed type and method name from given <see href="MethodBase" /> instance.
    /// </summary>
    /// <param name="method">
    /// A <see cref="MethodBase" /> instance to get fully qualified managed type and method name.
    /// </param>
    /// <param name="managedTypeName">
    /// When this method returns, contains the fully qualified managed type name of the <paramref name="method"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// The format is defined in <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md#managedtype-property">the RFC</see>.
    /// </param>
    /// <param name="managedMethodName">
    /// When this method returns, contains the fully qualified managed method name of the <paramref name="method"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// The format is defined in <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md#managedmethod-property">the RFC</see>.
    /// </param>
    /// <param name="hierarchyValues">
    /// When this method returns, contains the default test hierarchy values of the <paramref name="method"/>.
    /// This parameter is passed uninitialized; any value originally supplied in result will be overwritten.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> is null.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// <paramref name="method"/> must describe a method.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Required functionality on <paramref name="method"/> is missing on the current platform.
    /// </exception>
    /// <remarks>
    /// More information about <paramref name="managedTypeName"/> and <paramref name="managedMethodName"/> can be found in
    /// <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md">the RFC</see>.
    /// </remarks>
    public static void GetManagedName(MethodBase method, out string managedTypeName, out string managedMethodName, out string?[] hierarchyValues)
    {
        GetManagedName(method, out managedTypeName, out managedMethodName);
        GetManagedNameAndHierarchy(method, true, out _, out _, out hierarchyValues);
    }

    /// <summary>
    /// Gets default hierarchy values for a given <paramref name="method"/>.
    /// </summary>
    /// <param name="method">
    /// A <see cref="MethodBase" /> instance to get default hierarchy values.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> is null.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// <paramref name="method"/> must describe a method.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// Required functionality on <paramref name="method"/> is missing on the current platform.
    /// </exception>
    /// <returns>
    /// The hierarchy values.
    /// </returns>
    public static string?[] GetManagedHierarchy(MethodBase method)
    {
        GetManagedNameAndHierarchy(method, true, out _, out _, out var hierarchyValues);

        return hierarchyValues;
    }

    private static void GetManagedNameAndHierarchy(MethodBase method, bool useClosedTypes, out string managedTypeName, out string managedMethodName, out string?[] hierarchyValues)
    {
        _ = method ?? throw new ArgumentNullException(nameof(method));

        if (!ReflectionHelpers.IsMethod(method))
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorMethodExpectedAsAnArgument, nameof(method)));
        }

        var semanticType = ReflectionHelpers.GetReflectedType(method)
            // TODO: @Haplois, exception expects a message and not a param name.
            ?? throw new NotSupportedException(nameof(method));

        if (ReflectionHelpers.IsGenericType(semanticType) && !useClosedTypes)
        {
            // The type might have some of its generic parameters specified, so make
            // sure we are working with the open form of the generic type.
            semanticType = semanticType.GetGenericTypeDefinition();

            // The method might have some of its parameters specified by the original closed type
            // declaration. Here we use the method handle (basically metadata token) to create
            // a new method reference using the open form of the reflected type. The intent is
            // to strip all generic type parameters.
            var methodHandle = ReflectionHelpers.GetMethodHandle(method);
            method = MethodBase.GetMethodFromHandle(methodHandle, semanticType.TypeHandle)!;
        }

        if (method.IsGenericMethod && !useClosedTypes)
        {
            // If this method is generic, then convert to the generic method definition
            // so that we get the open generic type definitions for parameters.
            method = ((MethodInfo)method).GetGenericMethodDefinition();
        }

        var typeBuilder = new StringBuilder();
        var methodBuilder = new StringBuilder();

        // Namespace and Type Name (with arity designation)
        // hierarchyPos contains [startIndexOfNamespace, endIndexOfNameSpace, endIndexOfTypeName]
        var hierarchyPos = AppendTypeString(typeBuilder, semanticType, closedType: useClosedTypes);
        if (hierarchyPos is null || hierarchyPos.Length != 3)
        {
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorMethodExpectedAsAnArgument, nameof(method)));
        }

        // Method Name with method arity
        var arity = method.GetGenericArguments().Length;
        AppendMethodString(methodBuilder, method.Name, arity);
        if (arity > 0)
        {
            if (useClosedTypes)
            {
                AppendGenericMethodParameters(methodBuilder, method);
            }
            else
            {
                methodBuilder.Append('`');
                methodBuilder.Append(arity);
            }
        }

        // Type Parameters
        var paramList = method.GetParameters();
        if (paramList.Length != 0)
        {
            methodBuilder.Append('(');
            foreach (var p in paramList)
            {
                // closedType is always true here by RFC
                AppendTypeString(methodBuilder, p.ParameterType, closedType: true);
                methodBuilder.Append(',');
            }
            // Replace the last ',' with ')'
            methodBuilder[methodBuilder.Length - 1] = ')';
        }
        var methodNameEndIndex = methodBuilder.Length;

        managedTypeName = typeBuilder.ToString();
        managedMethodName = methodBuilder.ToString();

        hierarchyValues = new string[HierarchyConstants.Levels.TotalLevelCount];
        hierarchyValues[HierarchyConstants.Levels.TestGroupIndex] = managedMethodName.Substring(0, methodNameEndIndex);
        if (hierarchyPos[1] == hierarchyPos[0]) // No namespace
        {
            hierarchyValues[HierarchyConstants.Levels.ClassIndex] = managedTypeName.Substring(0, hierarchyPos[2]);
            hierarchyValues[HierarchyConstants.Levels.NamespaceIndex] = null;
        }
        else
        {
            hierarchyValues[HierarchyConstants.Levels.ClassIndex] = managedTypeName.Substring(hierarchyPos[1] + 1, hierarchyPos[2] - hierarchyPos[1] - 1);
            hierarchyValues[HierarchyConstants.Levels.NamespaceIndex] = managedTypeName.Substring(hierarchyPos[0], hierarchyPos[1] - hierarchyPos[0]);
        }
        hierarchyValues[HierarchyConstants.Levels.ContainerIndex] = method.DeclaringType?.Assembly?.GetName()?.Name ?? string.Empty;
    }

    /// <summary>
    /// Gets the <see cref="MethodBase"/> object with the specified <paramref name="managedTypeName"/>
    /// and <paramref name="managedMethodName"/> in the <paramref name="assembly"/> instance.
    /// </summary>
    /// <param name="assembly">
    /// An <see cref="Assembly" /> instance to search in.
    /// </param>
    /// <param name="managedTypeName">
    /// The fully qualified managed name of the type.
    /// The format is defined in <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md#managedtype-property">the RFC</see>.
    /// </param>
    /// <param name="managedMethodName">
    /// The fully qualified managed name of the method.
    /// The format is defined in <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md#managedmethod-property">the RFC</see>.
    /// </param>
    /// <returns>
    /// A <see cref="MethodBase" /> object that represents specified parameters, throws if null.
    /// </returns>
    /// <exception cref="InvalidManagedNameException">
    /// Values specified with <paramref name="managedTypeName"/> and <paramref name="managedMethodName"/>
    /// does not correspond to a method in the <paramref name="assembly"/> instance, or malformed.
    /// </exception>
    /// <remarks>
    /// More information about <paramref name="managedTypeName"/> and <paramref name="managedMethodName"/> can be found in
    /// <see href="https://github.com/microsoft/vstest/blob/main/docs/RFCs/0017-Managed-TestCase-Properties.md">the RFC</see>.
    /// </remarks>
    public static MethodBase GetMethod(Assembly assembly, string managedTypeName, string managedMethodName)
    {
        var parsedManagedTypeName = ReflectionHelpers.ParseEscapedString(managedTypeName);
        var type = assembly.GetType(parsedManagedTypeName, throwOnError: false, ignoreCase: false);

        if (type == null)
        {
            string message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorTypeNotFound, parsedManagedTypeName);
            throw new InvalidManagedNameException(message);
        }

        MethodInfo? method = null;
        ManagedNameParser.ParseManagedMethodName(managedMethodName, out var methodName, out var methodArity, out var parameterTypes);

        if (!string.IsNullOrWhiteSpace(methodName))
        {
            method = FindMethod(type, methodName, methodArity, parameterTypes);
        }

        if (method == null)
        {
            string message = string.Format(CultureInfo.CurrentCulture, Resources.Resources.ErrorMethodNotFound, methodName, managedTypeName);
            throw new InvalidManagedNameException(message);
        }

        return method;
    }

    private static MethodInfo? FindMethod(Type type, string methodName, int methodArity, string[]? parameterTypes)
    {
        bool Filter(MemberInfo mbr, object? param)
        {
            if (mbr is not MethodInfo method || method.Name != methodName || method.GetGenericArguments().Length != methodArity)
            {
                return false;
            }

            var paramList = method.GetParameters();
            if (paramList.Length == 0 && parameterTypes == null)
            {
                return true;
            }
            else if (parameterTypes == null || paramList.Length != parameterTypes.Length)
            {
                return false;
            }

            for (int i = 0; i < paramList.Length; i++)
            {
                var parameterType = GetTypeString(paramList[i].ParameterType, closedType: true);
                if (parameterType != parameterTypes[i])
                {
                    return false;
                }
            }

            return true;
        }

        var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        var methods = type.FindMembers(MemberTypes.Method, bindingFlags, Filter, null);

        return (MethodInfo?)(methods.Length switch
        {
            1 => methods[0],
            > 1 => methods.SingleOrDefault(i => i.DeclaringType == type),
            _ => null
        });
    }

    private static int[]? AppendTypeString(StringBuilder b, Type? type, bool closedType)
    {
        if (type is null)
        {
            return null;
        }

        int[]? hierarchies = null;
        if (type.IsArray)
        {
            hierarchies = AppendTypeString(b, type.GetElementType(), closedType);
            b.Append('[');
            for (int i = 0; i < type.GetArrayRank() - 1; i++)
            {
                b.Append(',');
            }
            b.Append(']');
        }
        else if (type.IsGenericParameter)
        {
            if (ReflectionHelpers.GetDeclaringMethod(type) != null)
            {
                b.Append('!');
            }
            b.Append('!');
            b.Append(type.GenericParameterPosition);
        }
        else
        {
            hierarchies = new int[3];
            hierarchies[0] = b.Length;

            if (type.Namespace != null)
            {
                AppendNamespace(b, type.Namespace);
                hierarchies[1] = b.Length;

                b.Append('.');
            }
            else
            {
                hierarchies[1] = hierarchies[0];
            }

            AppendNestedTypeName(b, type, closedType);
            if (closedType)
            {
                AppendGenericTypeParameters(b, type);
            }

            hierarchies[2] = b.Length;
        }

        return hierarchies;
    }

    private static void AppendNamespace(StringBuilder b, string? namespaceString)
    {
        if (namespaceString is null)
        {
            return;
        }

        int start = 0;
        bool shouldEscape = false;

        for (int i = 0; i <= namespaceString.Length; i++)
        {
            if (i == namespaceString.Length || namespaceString[i] == '.')
            {
                if (start != 0)
                {
                    b.Append('.');
                }

                var part = namespaceString.Substring(start, i - start);
                if (shouldEscape)
                {
                    NormalizeAndAppendString(b, part);
                    shouldEscape = false;
                }
                else
                {
                    b.Append(part);
                }

                start = i + 1;
                continue;
            }

            shouldEscape = shouldEscape || NeedsEscaping(namespaceString[i], i - start);
        }
    }

    private static void AppendMethodString(StringBuilder methodBuilder, string name, int methodArity)
    {
        var arityStart = name.LastIndexOf('`');
        var arity = 0;
        if (arityStart > 0)
        {
            arityStart++;
            var arityString = name.Substring(arityStart, name.Length - arityStart);
            if (int.TryParse(arityString, out arity))
            {
                if (arity == methodArity)
                {
                    name = name.Substring(0, arityStart - 1);
                }
            }
        }

        if (IsNormalized(name))
        {
            methodBuilder.Append(name);
        }
        else
        {
            NormalizeAndAppendString(methodBuilder, name);
        }

        if (arity > 0 && methodArity == arity)
        {
            methodBuilder.Append(
#if NET6_0_OR_GREATER
                System.Globalization.CultureInfo.InvariantCulture,
#endif
                $"`{arity}");
        }
    }

    private static void NormalizeAndAppendString(StringBuilder b, string name)
    {
        b.Append('\'');
        for (int i = 0; i < name.Length; i++)
        {
            char c = name[i];
            if (NeedsEscaping(c, i))
            {
                if (c is '\\' or '\'')
                {
                    // var encoded = Convert.ToString(((uint)c), 16);
                    // b.Append("\\u");
                    // b.Append('0', 4 - encoded.Length);
                    // b.Append(encoded);

                    b.Append('\\');
                    b.Append(c);
                    continue;
                }
            }

            b.Append(c);
        }
        b.Append('\'');
    }

    private static int AppendNestedTypeName(StringBuilder b, Type? type, bool closedType)
    {
        if (type is null)
        {
            return 0;
        }

        var outerArity = 0;
        if (type.IsNested)
        {
            outerArity = AppendNestedTypeName(b, type.DeclaringType, closedType);
            b.Append('+');
        }

        var typeName = type.Name;
        var stars = 0;
        if (type.IsPointer)
        {
            for (int i = typeName.Length - 1; i > 0; i--)
            {
                if (typeName[i] != '*')
                {
                    stars = typeName.Length - i - 1;
                    typeName = typeName.Substring(0, i + 1);
                    break;
                }
            }
        }

        var info = type.GetTypeInfo();
        var arity = !info.IsGenericType
                  ? 0
                  : info.GenericTypeParameters.Length > 0
                    ? info.GenericTypeParameters.Length
                    : info.GenericTypeArguments.Length;

        AppendMethodString(b, typeName, arity - outerArity);
        b.Append('*', stars);
        return arity;
    }

    private static void AppendGenericMethodParameters(StringBuilder methodBuilder, MethodBase method)
    {
        Type[] genericArguments = method.GetGenericArguments();
        AppendGenericArguments(methodBuilder, genericArguments);
    }

    private static void AppendGenericTypeParameters(StringBuilder b, Type type)
    {
        Type[] genericArguments = type.GetGenericArguments();
        AppendGenericArguments(b, genericArguments);
    }

    private static void AppendGenericArguments(StringBuilder b, Type[] genericArguments)
    {
        if (genericArguments.Length != 0)
        {
            b.Append('<');
            foreach (var argType in genericArguments)
            {
                AppendTypeString(b, argType, closedType: true);
                b.Append(',');
            }
            // Replace the last ',' with '>'
            b[b.Length - 1] = '>';
        }
    }

    private static bool IsNormalized(string s)
    {
        var brackets = 0;

        for (int i = 0; i < s.Length; i++)
        {
            var c = s[i];
            if (NeedsEscaping(c, i) && c != '.')
            {
                if (i != 0)
                {
                    if (c == '<')
                    {
                        brackets++;
                        continue;
                    }

                    if (c == '>' && s[i - 1] != '<' && brackets > 0)
                    {
                        brackets--;
                        continue;
                    }
                }

                return false;
            }
        }

        return brackets == 0;
    }

    private static bool NeedsEscaping(char c, int pos)
    {
        if (pos == 0 && char.IsDigit(c))
        {
            return true;
        }

        if (c == '_'
            // 'Digit' does not include letter numbers, which are valid identifiers as per docs https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/identifier-names'.
            || char.IsLetterOrDigit(c) // Lu, Ll, Lt, Lm, Lo, or Nd
            )
        {
            return false;
        }

        var category = CharUnicodeInfo.GetUnicodeCategory(c);
        return category
            is not UnicodeCategory.LetterNumber           // Nl
            and not UnicodeCategory.NonSpacingMark        // Mn
            and not UnicodeCategory.SpacingCombiningMark  // Mc
            and not UnicodeCategory.ConnectorPunctuation  // Pc
            and not UnicodeCategory.Format;               // Cf
    }

    private static string GetTypeString(Type type, bool closedType)
    {
        var builder = new StringBuilder();
        AppendTypeString(builder, type, closedType);
        return builder.ToString();
    }
}
