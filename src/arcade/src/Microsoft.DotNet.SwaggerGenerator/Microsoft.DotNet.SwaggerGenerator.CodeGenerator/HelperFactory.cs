using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using HandlebarsDotNet;
using JetBrains.Annotations;

namespace Microsoft.DotNet.SwaggerGenerator
{
    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
    public class HelperMethodAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    [MeansImplicitUse(ImplicitUseKindFlags.Access)]
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
            var outputParameter = Expression.Parameter(typeof(TextWriter), "output");
            var optionsParameter = Expression.Parameter(typeof(HelperOptions), "options");
            var contextParameter = Expression.Parameter(typeof(object), "context");
            var argumentsParameter = Expression.Parameter(typeof(object[]), "arguments");

            var parameterExpressions = GetParameterExpressions(method, argumentsParameter, new List<ProvidedParameter>
            {
                new ProvidedParameter("context", typeof(object), contextParameter),
                new ProvidedParameter("output", typeof(TextWriter), outputParameter),
                new ProvidedParameter("template", typeof(Action<TextWriter, object>), Expression.MakeMemberAccess(optionsParameter, typeof(HelperOptions).GetProperty("Template"))),
                new ProvidedParameter("inverse", typeof(Action<TextWriter, object>), Expression.MakeMemberAccess(optionsParameter, typeof(HelperOptions).GetProperty("Inverse"))),
            });

            Expression invokeExpression;
            if (!method.IsStatic)
            {
                invokeExpression = Expression.Call(Expression.Constant(instance), method, parameterExpressions);
            }
            else
            {
                invokeExpression = Expression.Call(method, parameterExpressions);
            }

            Expression body;
            if (method.ReturnType == typeof(void))
            {
                body = invokeExpression;
            }
            else
            {
                var writerOutput = ConvertResultExpression(method.ReturnType, invokeExpression);
                body = Expression.Call(HandlebarsExtensionsWriteSafeString, outputParameter, writerOutput);
            }

            var result = Expression.Lambda<HandlebarsBlockHelper>(
                body,
                outputParameter,
                optionsParameter,
                contextParameter,
                argumentsParameter);
            Debug.WriteLine("Compiling Expression: " + result);
            var function = result.Compile();
            return (output, options, context, parameters) => { function(output, options, context, parameters); };
        }

        private static HandlebarsHelper CreateHelperFunctionForMethod(MethodInfo method, object instance)
        {
            var outputParameter = Expression.Parameter(typeof(TextWriter), "output");
            var contextParameter = Expression.Parameter(typeof(object), "context");
            var argumentsParameter = Expression.Parameter(typeof(object[]), "arguments");

            var parameterExpressions = GetParameterExpressions(method, argumentsParameter, new List<ProvidedParameter>
            {
                new ProvidedParameter("context", typeof(object), contextParameter),
                new ProvidedParameter("output", typeof(TextWriter), outputParameter),
            });

            Expression invokeExpression;
            if (!method.IsStatic)
            {
                invokeExpression = Expression.Call(Expression.Constant(instance), method, parameterExpressions);
            }
            else
            {
                invokeExpression = Expression.Call(method, parameterExpressions);
            }
            var writerOutput = ConvertResultExpression(method.ReturnType, invokeExpression);
            var body = Expression.Call(HandlebarsExtensionsWriteSafeString, outputParameter, writerOutput);

            var result = Expression.Lambda<HandlebarsHelper>(
                body,
                outputParameter,
                contextParameter,
                argumentsParameter);
            Debug.WriteLine("Compiling Expression: " + result.ToString());
            var function = result.Compile();
            return (output, context, parameters) => { function(output, context, parameters); };
        }

        private static MethodInfo ObjectToString = typeof(object).GetMethod("ToString");

        private static MethodInfo HandlebarsExtensionsWriteSafeString = typeof(HandlebarsExtensions).GetMethod(
            "WriteSafeString",
            new[] {typeof(TextWriter), typeof(string)});

        private static MethodInfo EnumerableSkip(Type member) =>
            typeof(Enumerable).GetMethod("Skip").MakeGenericMethod(member);

        private static MethodInfo EnumerableToArray(Type member) =>
            typeof(Enumerable).GetMethod("ToArray").MakeGenericMethod(member);

        private static MethodInfo EnumerableSelect(Type input, Type output) =>
            typeof(Enumerable).GetMethods()
                .Single(
                    m => m.Name == "Select" &&
                         m.GetParameters().Length == 2 &&
                         m.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
                .MakeGenericMethod(input, output);

        private static MethodInfo ConvertChangeType = typeof(Convert).GetMethod(
            "ChangeType",
            new[] {typeof(object), typeof(Type)});

        private static MethodInfo HandlebarsUtilsIsTruthyOrNonEmpty = typeof(HandlebarsUtils).GetMethod("IsTruthyOrNonEmpty");

        private class ProvidedParameter
        {
            public ProvidedParameter(string name, Type type, Expression value)
            {
                Name = name;
                Type = type;
                Value = value;
            }

            public string Name { get; }
            public Type Type { get; }
            public Expression Value { get; }
        }

        private static IEnumerable<Expression> GetParameterExpressions(MethodInfo method, ParameterExpression argumentsParameter, List<ProvidedParameter> providedParameters)
        {
            var parameters = method.GetParameters();
            var consumedInputCount = 0;

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var providedParameter = providedParameters.FirstOrDefault(
                    p => p.Type == parameter.ParameterType && p.Name == parameter.Name);
                if (providedParameter != null)
                {
                    yield return providedParameter.Value;
                    continue;
                }

                yield return GetExpressionForParameter(parameter, consumedInputCount, argumentsParameter);
                consumedInputCount++;
            }
        }

        private static readonly Type UndefinedBindingResultType =
            typeof(Handlebars).Assembly.GetType("HandlebarsDotNet.Compiler.UndefinedBindingResult");

        private static Expression CoerceObjectExpression(Type output, Expression input)
        {
            if (output == typeof(bool))
            {
                return Expression.Call(HandlebarsUtilsIsTruthyOrNonEmpty, input);
            }

            if (output.IsPrimitive)
            {
                return Expression.Convert(Expression.Call(ConvertChangeType, input, Expression.Constant(output)), output);
            }

            if (!output.IsValueType ||
                (output.IsConstructedGenericType && output.GetGenericTypeDefinition() == typeof(Nullable<>)))
            {
                input = Expression.Condition(
                    Expression.TypeIs(input, UndefinedBindingResultType),
                    Expression.Constant(null, typeof(object)),
                    input);
            }

            return Expression.Convert(input, output);
        }

        private static Expression ConvertResultExpression(Type type, Expression input)
        {
            if (type == typeof(bool))
            {
                return Expression.Condition(input, Expression.Constant("true"), Expression.Constant(""));
            }

            return Expression.Call(input, ObjectToString);
        }

        private static Expression GetExpressionForParameter(
            ParameterInfo parameter,
            int index,
            ParameterExpression argumentsParameter)
        {
            var parameterType = parameter.ParameterType;
            if (parameter.ParameterType.IsArray && parameter.GetCustomAttribute<ParamArrayAttribute>() != null)
            {
                return GetExpressionForParamArrayParameter(parameterType, index, argumentsParameter);
            }

            var element = Expression.ArrayIndex(argumentsParameter, Expression.Constant(index));
            return CoerceObjectExpression(parameterType, element);
        }

        private static Expression GetExpressionForParamArrayParameter(Type parameterType, int index, ParameterExpression argumentsParameter)
        {
            Expression result = argumentsParameter;
            if (index != 0)
            {
                result = Expression.Call(EnumerableSkip(typeof(object)), result, Expression.Constant(index));
            }

            var elementType = parameterType.GetElementType();
            var selectParam = Expression.Parameter(typeof(object), "o");

            result = Expression.Call(
                EnumerableToArray(elementType),
                Expression.Call(
                    EnumerableSelect(typeof(object), elementType),
                    result,
                    Expression.Lambda(CoerceObjectExpression(elementType, selectParam), selectParam)));

            return result;
        }
    }
}
