// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HandlebarsDotNet;

namespace Microsoft.DotNet.SwaggerGenerator
{
    [AttributeUsage(AttributeTargets.Method)]
    public class HelperMethodAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class BlockHelperMethodAttribute : Attribute
    {
    }

    internal static class HelperFactory
    {
        internal static void RegisterAllForType(IHandlebars hb, Type type, object instance)
        {
            var helpers = CreateHelpersForType(type, instance);

            foreach (var (name, helper) in helpers)
            {
                hb.RegisterHelper(name, helper);
            }

            var blockHelpers = CreateBlockHelpersForType(type, instance);

            foreach (var (name, helper) in blockHelpers)
            {
                hb.RegisterHelper(name, helper);
            }
        }

        private static List<(string name, HandlebarsBlockHelper helper)> CreateBlockHelpersForType(Type type, object instance)
        {
            var helpers = new List<(string name, HandlebarsBlockHelper helper)>();
            foreach (var method in GetAllMethods(type))
            {
                if (method.GetCustomAttribute<BlockHelperMethodAttribute>() == null)
                {
                    continue;
                }

                if (!method.IsPublic)
                {
                    continue;
                }

                if (instance == null && !method.IsStatic)
                {
                    continue;
                }

                helpers.Add(CreateBlockHelperForMethod(method, instance));
            }

            return helpers;
        }

        private static List<(string name, HandlebarsHelper helper)> CreateHelpersForType(Type type, object instance)
        {
            var helpers = new List<(string name, HandlebarsHelper helper)>();
            foreach (var method in GetAllMethods(type))
            {
                if (method.GetCustomAttribute<HelperMethodAttribute>() == null)
                {
                    continue;
                }

                if (!method.IsPublic)
                {
                    continue;
                }

                if (instance == null && !method.IsStatic)
                {
                    continue;
                }

                helpers.Add(CreateHelperForMethod(method, instance));
            }

            return helpers;
        }

        private static IEnumerable<MethodInfo> GetAllMethods(Type type)
        {
            while (type != null)
            {
                foreach (var method in type.GetRuntimeMethods())
                {
                    yield return method;
                }
                type = type.BaseType;
            }
        }

        private static (string name, HandlebarsBlockHelper helper) CreateBlockHelperForMethod(MethodInfo method, object instance)
        {
            var name = Helpers.CamelCase(method.Name.AsSpan());
            var fn = CreateBlockHelperFunctionForMethod(method, instance);
            return (name, fn);
        }

        private static (string name, HandlebarsHelper helper) CreateHelperForMethod(MethodInfo method, object instance)
        {
            var name = Helpers.CamelCase(method.Name.AsSpan());
            var fn = CreateHelperFunctionForMethod(method, instance);
            return (name, fn);
        }

        private static HandlebarsBlockHelper CreateBlockHelperFunctionForMethod(MethodInfo method, object instance)
        {
            return (output, options, context, arguments) =>
            {
                var templateOutput = output;
                var writer = new SafeTextWriter(output);
                Action<TextWriter, object> template = (_, value) => options.Template(in templateOutput, value);
                Action<TextWriter, object> inverse = (_, value) => options.Inverse(in templateOutput, value);

                object result = method.Invoke(
                    instance,
                    GetParameterValues(method, context.Value, writer, template, inverse, arguments));

                WriteResult(output, method.ReturnType, result);
            };
        }

        private static HandlebarsHelper CreateHelperFunctionForMethod(MethodInfo method, object instance)
        {
            return (output, context, arguments) =>
            {
                object result = method.Invoke(
                    instance,
                    GetParameterValues(method, context.Value, new SafeTextWriter(output), null, null, arguments));

                WriteResult(output, method.ReturnType, result);
            };
        }

        private static object[] GetParameterValues(
            MethodInfo method,
            object context,
            TextWriter output,
            Action<TextWriter, object> template,
            Action<TextWriter, object> inverse,
            Arguments arguments)
        {
            var values = new List<object>();
            int argumentIndex = 0;

            foreach (ParameterInfo parameter in method.GetParameters())
            {
                if (parameter.ParameterType == typeof(object) && parameter.Name == "context")
                {
                    values.Add(context);
                }
                else if (parameter.ParameterType == typeof(TextWriter) && parameter.Name == "output")
                {
                    values.Add(output);
                }
                else if (parameter.ParameterType == typeof(Action<TextWriter, object>) && parameter.Name == "template")
                {
                    values.Add(template);
                }
                else if (parameter.ParameterType == typeof(Action<TextWriter, object>) && parameter.Name == "inverse")
                {
                    values.Add(inverse);
                }
                else if (parameter.ParameterType.IsArray && parameter.GetCustomAttribute<ParamArrayAttribute>() != null)
                {
                    Type elementType = parameter.ParameterType.GetElementType();
                    Array remainingArguments = Array.CreateInstance(elementType, arguments.Length - argumentIndex);
                    for (int i = argumentIndex; i < arguments.Length; i++)
                    {
                        remainingArguments.SetValue(CoerceObject(elementType, arguments[i]), i - argumentIndex);
                    }

                    values.Add(remainingArguments);
                    argumentIndex = arguments.Length;
                }
                else
                {
                    values.Add(CoerceObject(parameter.ParameterType, arguments[argumentIndex]));
                    argumentIndex++;
                }
            }

            return values.ToArray();
        }

        private static readonly Type UndefinedBindingResultType =
            typeof(Handlebars).Assembly.GetType("HandlebarsDotNet.UndefinedBindingResult");

        private static object CoerceObject(Type output, object input)
        {
            if (output == typeof(bool))
            {
                return HandlebarsUtils.IsTruthyOrNonEmpty(input, includeZero: false);
            }

            if (input?.GetType() == UndefinedBindingResultType)
            {
                input = null;
            }

            Type nullableType = Nullable.GetUnderlyingType(output);
            if (nullableType != null)
            {
                return input == null ? null : Convert.ChangeType(input, nullableType);
            }

            if (output.IsPrimitive)
            {
                return Convert.ChangeType(input, output);
            }

            return input;
        }

        private static void WriteResult(EncodedTextWriter output, Type type, object result)
        {
            if (type == typeof(void))
            {
                return;
            }

            if (type == typeof(bool))
            {
                output.Write((bool)result ? "true" : "", encode: false);
                return;
            }

            output.Write(result.ToString(), encode: false);
        }

        private sealed class SafeTextWriter : TextWriter
        {
            private EncodedTextWriter _writer;

            public SafeTextWriter(EncodedTextWriter writer)
            {
                _writer = writer;
            }

            public override Encoding Encoding => _writer.Encoding;

            public override void Write(char value)
            {
                _writer.Write(value.ToString(), encode: false);
            }

            public override void Write(string value)
            {
                _writer.Write(value, encode: false);
            }

            public override void Write(object value)
            {
                _writer.Write(value?.ToString(), encode: false);
            }
        }
    }
}
