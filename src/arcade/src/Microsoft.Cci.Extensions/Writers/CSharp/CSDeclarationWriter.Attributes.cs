// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Cci.Comparers;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Writers.Syntax;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter
    {
        // writeInline => [ , , ] vs []\n[]\n
        public void WriteAttributes(IEnumerable<ISecurityAttribute> securityAttributes, bool writeInline = false, string prefix = "")
        {
            if (!securityAttributes.SelectMany(s => s.Attributes).Any(IncludeAttribute))
                return;

            securityAttributes = securityAttributes.OrderBy(s => s.Action.ToString(), StringComparer.OrdinalIgnoreCase);

            bool first = true;
            WriteSymbol("[");
            foreach (ISecurityAttribute securityAttribute in securityAttributes)
            {
                foreach (ICustomAttribute attribute in securityAttribute.Attributes)
                {
                    if (!first)
                    {
                        if (writeInline)
                        {
                            WriteSymbol(",", addSpace: true);
                        }
                        else
                        {
                            WriteSymbol("]");
                            _writer.WriteLine();
                            WriteSymbol("[");
                        }
                    }

                    WriteAttribute(attribute, prefix, securityAttribute.Action);

                    first = false;
                }
            }
            WriteSymbol("]");
            if (!writeInline)
                _writer.WriteLine();
        }
        private static FakeCustomAttribute s_methodImpl = new FakeCustomAttribute("System.Runtime.CompilerServices", "MethodImpl");
        private static FakeCustomAttribute s_dllImport = new FakeCustomAttribute("System.Runtime.InteropServices", "DllImport");

        private void WriteMethodPseudoCustomAttributes(IMethodDefinition method)
        {
            // Decided not to put more information (parameters) here as that would have introduced a lot of noise.
            if (method.IsPlatformInvoke)
            {
                if (IncludeAttribute(s_dllImport))
                {
                    string typeName = _forCompilation ? s_dllImport.FullTypeName : s_dllImport.TypeName;
                    WriteFakeAttribute(typeName, writeInline: true, parameters: "\"" + method.PlatformInvokeData.ImportModule.Name.Value + "\"");
                }
            }

            var ops = CreateMethodImplOptions(method);
            if (ops != default(System.Runtime.CompilerServices.MethodImplOptions))
            {
                if (IncludeAttribute(s_methodImpl))
                {
                    string typeName = _forCompilation ? s_methodImpl.FullTypeName : s_methodImpl.TypeName;
                    string enumValue = _forCompilation ?
                        string.Join("|", ops.ToString().Split(',').Select(x => "System.Runtime.CompilerServices.MethodImplOptions." + x.TrimStart())) :
                        ops.ToString();
                    WriteFakeAttribute(typeName, writeInline: true, parameters: enumValue);
                }
            }
        }

        private System.Runtime.CompilerServices.MethodImplOptions CreateMethodImplOptions(IMethodDefinition method)
        {
            // Some options are not exposed in portable contracts. PortingHelpers.cs exposes the missing constants.
            System.Runtime.CompilerServices.MethodImplOptions options = default(System.Runtime.CompilerServices.MethodImplOptions);
            if (method.IsUnmanaged)
                options |= System.Runtime.CompilerServices.MethodImplOptionsEx.Unmanaged;
            if (method.IsForwardReference)
                options |= System.Runtime.CompilerServices.MethodImplOptionsEx.ForwardRef;
            if (method.PreserveSignature)
                options |= System.Runtime.CompilerServices.MethodImplOptions.PreserveSig;
            if (method.IsRuntimeInternal)
                options |= System.Runtime.CompilerServices.MethodImplOptionsEx.InternalCall;
            if (method.IsSynchronized)
                options |= System.Runtime.CompilerServices.MethodImplOptionsEx.Synchronized;
            if (method.IsNeverInlined)
                options |= System.Runtime.CompilerServices.MethodImplOptions.NoInlining;
            if (method.IsAggressivelyInlined)
                options |= System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining;
            if (method.IsNeverOptimized)
                options |= System.Runtime.CompilerServices.MethodImplOptions.NoOptimization;

            return options;
        }

        public void WriteAttributes(IEnumerable<ICustomAttribute> attributes, bool writeInline = false, string prefix = null)
        {
            attributes = attributes.Where(IncludeAttribute);
            if (!attributes.Any())
                return;

            attributes = attributes.OrderBy(a => a, new AttributeComparer(_filter, _forCompilation));

            bool first = true;
            WriteSymbol("[");

            foreach (ICustomAttribute attribute in attributes)
            {
                if (!first)
                {
                    if (writeInline)
                    {
                        WriteSymbol(",", addSpace: true);
                    }
                    else
                    {
                        WriteSymbol("]");
                        _writer.WriteLine();
                        WriteSymbol("[");
                    }
                }

                WriteAttribute(attribute, prefix);
                first = false;
            }
            WriteSymbol("]", addSpace: writeInline);
            if (!writeInline)
                _writer.WriteLine();

        }

        public void WriteAttribute(ICustomAttribute attribute, string prefix = null, SecurityAction action = SecurityAction.ActionNil)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                WriteKeyword(prefix, noSpace: true);
                WriteSymbol(":", addSpace: true);
            }
            WriteTypeName(attribute.Constructor.ContainingType, noSpace: true); // Should we strip Attribute from name?

            if (attribute.NumberOfNamedArguments > 0 || attribute.Arguments.Any() || action != SecurityAction.ActionNil)
            {
                WriteSymbol("(");
                bool first = true;

                if (action != SecurityAction.ActionNil)
                {
                    Write("System.Security.Permissions.SecurityAction." + action.ToString());
                    first = false;
                }

                foreach (IMetadataExpression arg in attribute.Arguments)
                {
                    if (!first) WriteSymbol(",", true);
                    WriteMetadataExpression(arg);
                    first = false;
                }

                foreach (IMetadataNamedArgument namedArg in attribute.NamedArguments)
                {
                    if (!first) WriteSymbol(",", true);
                    WriteIdentifier(namedArg.ArgumentName);
                    WriteSymbol("=");
                    WriteMetadataExpression(namedArg.ArgumentValue);
                    first = false;
                }
                WriteSymbol(")");
            }
        }

        private void WriteFakeAttribute(string typeName, params string[] parameters)
        {
            WriteFakeAttribute(typeName, false, parameters);
        }

        private void WriteFakeAttribute(string typeName, bool writeInline, params string[] parameters)
        {
            // These fake attributes are really only useful for the compilers
            if (!_forCompilation && !_includeFakeAttributes)
                return;

            if (_forCompilationIncludeGlobalprefix)
                typeName = "global::" + typeName;

            WriteSymbol("[");

            _writer.WriteTypeName(typeName);

            if (parameters.Length > 0)
            {
                WriteSymbol("(");
                _writer.WriteList(parameters, p => Write(p));
                WriteSymbol(")");
            }

            WriteSymbol("]");
            if (!writeInline)
                _writer.WriteLine();
        }

        private void WriteMetadataExpression(IMetadataExpression expression)
        {
            IMetadataConstant constant = expression as IMetadataConstant;
            if (constant != null)
            {
                WriteMetadataConstant(constant);
                return;
            }

            IMetadataCreateArray array = expression as IMetadataCreateArray;
            if (array != null)
            {
                WriteMetadataArray(array);
                return;
            }

            IMetadataTypeOf type = expression as IMetadataTypeOf;
            if (type != null)
            {
                WriteKeyword("typeof", noSpace: true);
                WriteSymbol("(");
                WriteTypeName(type.TypeToGet, noSpace: true, omitGenericTypeList: true);
                WriteSymbol(")");
                return;
            }

            throw new NotSupportedException("IMetadataExpression type not supported");
        }

        private void WriteMetadataConstant(IMetadataConstant constant, ITypeReference constantType = null)
        {
            object value = constant.Value;
            ITypeReference type = (constantType == null ? constant.Type : constantType);

            if (value == null)
            {
                if (type.IsValueType)
                {
                    // Write default(T) for value types
                    WriteDefaultOf(type);
                }
                else
                {
                    WriteKeyword("null", noSpace: true);
                }
            }
            else if (type.ResolvedType.IsEnum)
            {
                WriteEnumValue(constant, constantType);
            }
            else if (value is string)
            {
                Write(QuoteString((string)value));
            }
            else if (value is char)
            {
                Write(String.Format("'{0}'", EscapeChar((char)value, false)));
            }
            else if (value is double)
            {
                double val = (double)value;
                if (double.IsNegativeInfinity(val))
                    Write("-1.0 / 0.0");
                else if (double.IsPositiveInfinity(val))
                    Write("1.0 / 0.0");
                else if (double.IsNaN(val))
                    Write("0.0 / 0.0");
                else
                    Write(((double)value).ToString("R", CultureInfo.InvariantCulture));
            }
            else if (value is float)
            {
                float val = (float)value;
                if (float.IsNegativeInfinity(val))
                    Write("-1.0f / 0.0f");
                else if (float.IsPositiveInfinity(val))
                    Write("1.0f / 0.0f");
                else if (float.IsNaN(val))
                    Write("0.0f / 0.0f");
                else
                    Write(((float)value).ToString("R", CultureInfo.InvariantCulture) + "f");
            }
            else if (value is bool)
            {
                if ((bool)value)
                    WriteKeyword("true", noSpace: true);
                else
                    WriteKeyword("false", noSpace: true);
            }
            else if (value is int)
            {
                // int is the default and most used constant value so lets
                // special case int to avoid a bunch of useless casts.
                Write(value.ToString());
            }
            else
            {
                // Explicitly cast the value so that we avoid any signed/unsigned resolution issues
                WriteSymbol("(");
                WriteTypeName(type, noSpace: true);
                WriteSymbol(")");
                Write(value.ToString());
            }

            // Might need to add support for other types...
        }

        private void WriteMetadataArray(IMetadataCreateArray array)
        {
            bool first = true;

            WriteKeyword("new");
            WriteTypeName(array.Type, noSpace: true);
            WriteSymbol("{", addSpace: true);
            foreach (IMetadataExpression expr in array.Initializers)
            {
                if (first) { first = false; } else { WriteSymbol(",", true); }
                WriteMetadataExpression(expr);
            }
            WriteSymbol("}");
        }

        private static string QuoteString(string str)
        {
            StringBuilder sb = new StringBuilder(str.Length + 4);
            sb.Append("\"");
            foreach (char ch in str)
            {
                sb.Append(EscapeChar(ch, true));
            }
            sb.Append("\"");
            return sb.ToString();
        }

        private static string EscapeChar(char c, bool inString)
        {
            switch (c)
            {
                case '\r': return @"\r";
                case '\n': return @"\n";
                case '\f': return @"\f";
                case '\t': return @"\t";
                case '\v': return @"\v";
                case '\0': return @"\0";
                case '\a': return @"\a";
                case '\b': return @"\b";
                case '\\': return @"\\";
                case '\'': return inString ? "'" : @"\'";
                case '"': return inString ? "\\\"" : "\"";
            }
            var cat = CharUnicodeInfo.GetUnicodeCategory(c);
            if (cat == UnicodeCategory.Control ||
              cat == UnicodeCategory.LineSeparator ||
              cat == UnicodeCategory.Format ||
              cat == UnicodeCategory.Surrogate ||
              cat == UnicodeCategory.PrivateUse ||
              cat == UnicodeCategory.OtherNotAssigned)
                return String.Format("\\u{0:X4}", (int)c);
            return c.ToString();
        }

        private static bool ExcludeSpecialAttribute(ICustomAttribute c)
        {
            string typeName = c.FullName();

            switch (typeName)
            {
                case "System.ParamArrayAttribute": return true;
                case "System.Reflection.AssemblyDelaySignAttribute": return true;
                case "System.Reflection.AssemblyKeyFileAttribute": return true;
                case "System.Reflection.DefaultMemberAttribute": return true;
                case "System.Runtime.CompilerServices.CompilerFeatureRequiredAttribute": return true;
                case "System.Runtime.CompilerServices.DynamicAttribute": return true;
                case "System.Runtime.CompilerServices.ExtensionAttribute": return true;
                case "System.Runtime.CompilerServices.FixedBufferAttribute": return true;
                case "System.Runtime.CompilerServices.IsByRefLikeAttribute": return true;
                case "System.Runtime.CompilerServices.IsReadOnlyAttribute": return true;
                case "System.Runtime.CompilerServices.RequiredMemberAttribute": return true;
                case "System.Runtime.CompilerServices.TupleElementNamesAttribute": return true;
                case "System.ObsoleteAttribute":
                    {
                        var arg = c.Arguments.OfType<IMetadataConstant>().FirstOrDefault();

                        if (arg?.Value is string)
                        {
                            string argValue = (string)arg.Value;
                            if (argValue is "Types with embedded references are not supported in this version of your compiler."
                                         or "Constructors of types with required members are not supported in this version of your compiler.")
                            {
                                return true;
                            }
                        }
                        break;
                    }
            }
            return false;
        }

        private bool IncludeAttribute(ICustomAttribute attribute)
        {
            if (ExcludeSpecialAttribute(attribute))
                return false;

            return _alwaysIncludeBase || _filter.Include(attribute);
        }
    }
}
