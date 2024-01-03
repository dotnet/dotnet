// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using HandlebarsDotNet;
using Microsoft.DotNet.SwaggerGenerator.Modeler;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using TypeReference = Microsoft.DotNet.SwaggerGenerator.Modeler.TypeReference;

namespace Microsoft.DotNet.SwaggerGenerator.Languages
{
    public partial class Language
    {
        private class CSharp : Language
        {
            [HelperMethod]
            public static string PascalCaseNs(string value)
            {
                string[] parts = value.Split('.');
                string ns = string.Join(".", parts.Select(p => Helpers.PascalCase(p.AsSpan())));
                return ns;
            }

            [HelperMethod]
            public static bool IsNullableType(TypeReference reference)
            {
                if (reference == TypeReference.Boolean)
                {
                    return false;
                }

                if (reference == TypeReference.Int32)
                {
                    return false;
                }

                if (reference == TypeReference.Int64)
                {
                    return false;
                }

                if (reference == TypeReference.Float)
                {
                    return false;
                }

                if (reference == TypeReference.Double)
                {
                    return false;
                }

                if (reference == TypeReference.Date)
                {
                    return false;
                }

                if (reference == TypeReference.DateTime)
                {
                    return false;
                }

                if (reference == TypeReference.Uuid)
                {
                    return false;
                }

                return true;
            }

            public override string Extension => ".cs";

            protected override string ResolveReference(TypeReference reference, object[] args)
            {
                if (reference is TypeReference.ConstantTypeReference)
                {
                    return "string";
                }

                if (reference is TypeReference.TypeModelReference typeModelRef)
                {
                    return "Models." + Helpers.PascalCase(typeModelRef.Model.Name.AsSpan());
                }

                if (reference is TypeReference.ArrayTypeReference arrayTypeRef)
                {
                    return $"IImmutableList<{ResolveReference(arrayTypeRef.BaseType, args)}>";
                }

                if (reference is TypeReference.DictionaryTypeReference dictTypeRef)
                {
                    return $"IImmutableDictionary<string, {ResolveReference(dictTypeRef.ValueType, args)}>";
                }

                if (reference == TypeReference.Boolean)
                {
                    return "bool";
                }

                if (reference == TypeReference.Int32)
                {
                    return "int";
                }

                if (reference == TypeReference.Int64)
                {
                    return "long";
                }

                if (reference == TypeReference.Float)
                {
                    return "float";
                }

                if (reference == TypeReference.Double)
                {
                    return "double";
                }

                if (reference == TypeReference.String)
                {
                    return "string";
                }

                if (reference == TypeReference.Date)
                {
                    return "DateTimeOffset";
                }

                if (reference == TypeReference.DateTime)
                {
                    return "DateTimeOffset";
                }

                if (reference == TypeReference.Uuid)
                {
                    return "Guid";
                }

                if (reference == TypeReference.Void)
                {
                    return "void";
                }

                if (reference == TypeReference.Any)
                {
                    return "Newtonsoft.Json.Linq.JToken";
                }

                if (reference == TypeReference.File)
                {
                    return "System.IO.Stream";
                }

                if (reference == TypeReference.Byte)
                {
                    // TODO: implement this
                }

                throw new NotSupportedException(reference.ToString());
            }

            [BlockHelperMethod]
            public void NullCheck(TextWriter output, object context, Action<TextWriter, object> template, TypeReference reference, bool required)
            {
                if (reference == TypeReference.String)
                {
                    output.Write("string.IsNullOrEmpty(");
                    template(output, context);
                    output.Write(")");
                }
                else
                {
                    template(output, context);
                    output.WriteSafeString($" == {GetDefaultExpression(reference, required)}");
                }
            }

            [BlockHelperMethod]
            public void NotNullCheck(TextWriter output, object context, Action<TextWriter, object> template, TypeReference reference, bool required)
            {
                if (reference == TypeReference.String)
                {
                    output.Write("!string.IsNullOrEmpty(");
                    template(output, context);
                    output.Write(")");
                }
                else
                {
                    template(output, context);
                    output.WriteSafeString($" != {GetDefaultExpression(reference, required)}");
                }
            }

            public string GetDefaultExpression(TypeReference reference, bool required)
            {
                string typeElement = ResolveReference(reference, null);
                string nullableElement = "";
                if (!required && !IsNullable(reference))
                {
                    nullableElement = "?";
                }
                return $"default({typeElement}{nullableElement})";
            }

            [HelperMethod]
            public static string Method(HttpMethod method)
            {

                if (method == HttpMethod.Delete || method == HttpMethod.Get || method == HttpMethod.Head ||
                    method.Method.ToLower() == "patch" || method == HttpMethod.Post || method == HttpMethod.Put)
                {
                    return $"RequestMethod.{Helpers.PascalCase(method.Method.ToLower().AsSpan())}";
                }

                return $"RequestMethod.Parse(\"{method.Method}\")";
            }

            public override Templates GetTemplates(IHandlebars hb)
            {
                return Templates.Load("csharp", hb);
            }

            public override void GenerateCode(GenerateCodeContext context)
            {
                var model = context.ClientModel;
                context.WriteTemplate(context.Options.ClientName, context.Templates["ServiceClient"], model);
                context.WriteTemplate("PagedResponse", context.Templates["PagedResponse"], model);

                foreach (TypeModel type in model.Types)
                {
                    context.WriteTemplate($"Models/{Helpers.PascalCase(type.Name.AsSpan())}", context.Templates["Model"], type);
                }

                foreach (MethodGroupModel group in model.MethodGroups)
                {
                    context.WriteTemplate(group.Name, context.Templates["MethodGroup"], group);
                }
            }

            [HelperMethod]
            public bool IsVerifyable(object type)
            {
                if (type is TypeReference.TypeModelReference modelRef)
                {
                    type = modelRef.Model;
                }
                if (type is ClassTypeModel classType)
                {
                    return classType.Properties.Any(p => p.Required && IsNullable(p.Type));
                }

                return false;
            }

            [HelperMethod]
            public bool IsNullable(TypeReference type)
            {
                if (
                    type is TypeReference.ArrayTypeReference ||
                    type is TypeReference.DictionaryTypeReference ||
                    type is TypeReference.TypeModelReference ||
                    type == TypeReference.Any ||
                    type == TypeReference.File ||
                    type == TypeReference.String)
                {
                    return true;
                }


                return false;
            }
        }
    }
}
