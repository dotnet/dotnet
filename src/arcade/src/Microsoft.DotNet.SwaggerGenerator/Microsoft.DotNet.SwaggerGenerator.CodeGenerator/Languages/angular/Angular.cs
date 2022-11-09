using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using HandlebarsDotNet;
using Microsoft.DotNet.SwaggerGenerator.Modeler;

namespace Microsoft.DotNet.SwaggerGenerator.Languages
{
    partial class Language
    {
        private class Angular : Language
        {
            public override string Extension => ".ts";

            protected override string ResolveReference(TypeReference reference, object[] args)
            {
                if (reference is TypeReference.ConstantTypeReference)
                {
                    return "string";
                }

                if (reference is TypeReference.TypeModelReference typeModelRef)
                {
                    var name = Helpers.PascalCase(typeModelRef.Model.Name.AsSpan());
                    if (args.Length > 0)
                    {
                        name = "models." + name;
                    }

                    return name;
                }

                if (reference is TypeReference.ArrayTypeReference arrayTypeRef)
                {
                    var itemType = ResolveReference(arrayTypeRef.BaseType, args);
                    if (itemType.Contains(" "))
                    {
                        return $"({itemType})[]";
                    }
                    return $"{itemType}[]";
                }

                if (reference is TypeReference.DictionaryTypeReference dictTypeRef)
                {
                    return $"Record<string, {ResolveReference(dictTypeRef.ValueType, args)}>";
                }

                if (reference == TypeReference.Boolean)
                {
                    return "boolean";
                }

                if (reference == TypeReference.Int32  ||
                    reference == TypeReference.Int64  || 
                    reference == TypeReference.Float  ||
                    reference == TypeReference.Double)
                {
                    return "number";
                }

                if (reference == TypeReference.String || reference == TypeReference.Uuid)
                {
                    return "string";
                }

                if (reference == TypeReference.Date)
                {
                    return "Date";
                }

                if (reference == TypeReference.DateTime)
                {
                    return "Date";
                }

                if (reference == TypeReference.Void)
                {
                    return "void";
                }

                if (reference == TypeReference.Any)
                {
                    return "any";
                }

                if (reference == TypeReference.File)
                {
                    return "Blob";
                }

                if (reference == TypeReference.Byte)
                {
                    // TODO: implement this
                }

                throw new NotSupportedException(reference.ToString());
            }

            [BlockHelperMethod]
            public static void SerializeToRawObject(
                TextWriter output,
                object context,
                Action<TextWriter, object> template,
                TypeReference reference,
                params object[] args)
            {
                if (reference is TypeReference.TypeModelReference typeModelRef)
                {
                    if (typeModelRef.Model.IsEnum)
                    {
                        template(output, context);
                        return;
                    }

                    var name = Helpers.PascalCase(typeModelRef.Model.Name.AsSpan());
                    if (args.Length > 0)
                    {
                        name = "models." + name;
                    }

                    output.Write($"{name}.toRawObject(");
                    template(output, context);
                    output.Write(")");
                    return;
                }

                if (reference is TypeReference.ArrayTypeReference arrayTypeRef)
                {
                    template(output, context);
                    output.WriteSafeString(".map((e: any) => ");
                    SerializeToRawObject(output, context, (o, c) => o.Write("e"), arrayTypeRef.BaseType, args);
                    output.Write(")");
                    return;
                }

                if (reference is TypeReference.DictionaryTypeReference dictTypeRef)
                {
                    output.Write("Helper.mapValues(");
                    template(output, context);
                    output.WriteSafeString(", (v: any) => ");
                    SerializeToRawObject(output, context, (o, c) => o.Write("v"), dictTypeRef.ValueType, args);
                    output.Write(")");
                    return;
                }

                if (reference == TypeReference.Date ||
                    reference == TypeReference.DateTime)
                {
                    template(output, context);
                    output.Write(".toISOString()");
                    return;
                }

                if (reference == TypeReference.File)
                {
                    template(output, context);
                    return;
                }

                if (
                    reference is TypeReference.ConstantTypeReference ||
                    reference == TypeReference.Boolean  ||
                    reference == TypeReference.Int32    ||
                    reference == TypeReference.Int64    || 
                    reference == TypeReference.Float    ||
                    reference == TypeReference.Double   ||
                    reference == TypeReference.String   ||
                    reference == TypeReference.Uuid     ||
                    reference == TypeReference.Any)
                {
                    template(output, context);
                    return;
                }

                if (reference == TypeReference.Void)
                {
                    return;
                }

                if (reference == TypeReference.Byte)
                {
                    // TODO: implement this
                }

                throw new NotSupportedException(reference.ToString());
            }

            [BlockHelperMethod]
            public static void DeserializeFromRawObject(
                TextWriter output,
                object context,
                Action<TextWriter, object> template,
                TypeReference reference,
                params object[] args)
            {
                if (reference is TypeReference.TypeModelReference typeModelRef)
                {
                    if (typeModelRef.Model.IsEnum)
                    {
                        template(output, context);
                        return;
                    }

                    var name = Helpers.PascalCase(typeModelRef.Model.Name.AsSpan());
                    if (args.Length > 0)
                    {
                        name = "models." + name;
                    }

                    output.Write($"{name}.fromRawObject(");
                    template(output, context);
                    output.Write(")");
                    return;
                }

                if (reference is TypeReference.ArrayTypeReference arrayTypeRef)
                {
                    template(output, context);
                    output.WriteSafeString(".map((e: any) => ");
                    DeserializeFromRawObject(output, context, (o, c) => o.Write("e"), arrayTypeRef.BaseType, args);
                    output.Write(")");
                    return;
                }

                if (reference is TypeReference.DictionaryTypeReference dictTypeRef)
                {
                    output.Write("Helper.mapValues(");
                    template(output, context);
                    output.WriteSafeString(", (v: any) => ");
                    DeserializeFromRawObject(output, context, (o, c) => o.Write("v"), dictTypeRef.ValueType, args);
                    output.Write(")");
                    return;
                }

                if (reference == TypeReference.Date ||
                    reference == TypeReference.DateTime)
                {
                    output.Write("parseISO(");
                    template(output, context);
                    output.Write(")");
                    return;
                }

                if (reference == TypeReference.File)
                {
                    template(output, context);
                    return;
                }

                if (
                    reference == TypeReference.Boolean  ||
                    reference == TypeReference.Int32    ||
                    reference == TypeReference.Int64    || 
                    reference == TypeReference.Float    ||
                    reference == TypeReference.Double   ||
                    reference == TypeReference.String   ||
                    reference == TypeReference.Uuid     ||
                    reference == TypeReference.Any)
                {
                    template(output, context);
                    return;
                }

                if (reference == TypeReference.Void)
                {
                    return;
                }

                if (reference == TypeReference.Byte)
                {
                    // TODO: implement this
                }

                throw new NotSupportedException(reference.ToString());
            }

            [BlockHelperMethod]
            public static void Serialize(TextWriter output, object context, Action<TextWriter, object> template, TypeReference reference, params object[] args)
            {
                if (reference == TypeReference.String ||
                    reference is TypeReference.ConstantTypeReference ||
                    reference == TypeReference.Date ||
                    reference == TypeReference.DateTime)
                {
                    SerializeToRawObject(output, context, template, reference, args);
                    return;
                }

                if (
                    reference == TypeReference.Boolean  ||
                    reference == TypeReference.Int32    ||
                    reference == TypeReference.Int64    || 
                    reference == TypeReference.Float    ||
                    reference == TypeReference.Double)
                {
                    template(output, context);
                    output.WriteSafeString(" + \"\"");
                    return;
                }

                output.Write("JSON.stringify(");
                SerializeToRawObject(output, context, template, reference, args);
                output.Write(")");
            }


            [BlockHelperMethod]
            public static void Deserialize(TextWriter output, object context, Action<TextWriter, object> template, TypeReference reference, params object[] args)
            {
                if (reference == TypeReference.String ||
                    reference is TypeReference.ConstantTypeReference ||
                    reference == TypeReference.Date ||
                    reference == TypeReference.DateTime)
                {
                    DeserializeFromRawObject(output, context, template, reference, args);
                    return;
                }

                if (
                    reference == TypeReference.Boolean  ||
                    reference == TypeReference.Int32    ||
                    reference == TypeReference.Int64    || 
                    reference == TypeReference.Float    ||
                    reference == TypeReference.Double)
                {
                    output.Write("+");
                    template(output, context);
                    return;
                }

                DeserializeFromRawObject(output, context,
                    (o, c) =>
                    {
                        o.Write("JSON.parse(");
                        template(o, c);
                        o.Write(")");
                    }, reference, args);
            }

            [HelperMethod]
            public static string Method(HttpMethod method)
            {
                return method.Method.ToLowerInvariant();
            }

            [HelperMethod]
            public bool IsVerifyable(ClassTypeModel type)
            {
                return type.Properties.Any(p => p.Required);
            }

            public override Templates GetTemplates(IHandlebars hb)
            {
                return Templates.Load("angular", hb);
            }

            public override void GenerateCode(GenerateCodeContext context)
            {
                var model = context.ClientModel;
                context.WriteTemplate("models", context.Templates["Models"], model);
                context.WriteTemplate("helper", context.Templates["Helper"], model);
                context.WriteTemplate("index", context.Templates["index"], model);
                context.WriteTemplate(Helpers.CamelCase(context.Options.ClientName.AsSpan()), context.Templates["ServiceClient"], model);
            }
        }
    }
}
