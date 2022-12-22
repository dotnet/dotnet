// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Cci.Extensions;
using Microsoft.Cci.Extensions.CSharp;
using Microsoft.Cci.Filters;
using Microsoft.Cci.Writers.Syntax;

namespace Microsoft.Cci.Writers.CSharp
{
    public partial class CSDeclarationWriter
    {
        private void WriteMethodDefinition(IMethodDefinition method)
        {
            if (method.IsPropertyOrEventAccessor())
                return;

            WriteMethodPseudoCustomAttributes(method);

            WriteAttributes(method.Attributes);
            WriteAttributes(method.SecurityAttributes);
            WriteAttributes(method.ReturnValueAttributes, prefix: "return");

            if (method.IsDestructor())
            {
                // If platformNotSupportedExceptionMessage is != null we're generating a dummy assembly which means we don't need a destructor at all.
                if (_platformNotSupportedExceptionMessage == null)
                    WriteDestructor(method);

                return;
            }

            var writeVisibility = true;

            if (method.ContainingTypeDefinition.IsInterface)
            {
                writeVisibility = false;
            }
            
            if (method.IsExplicitInterfaceMethod() || method.IsStaticConstructor)
            {
                writeVisibility = false;
            }

            if (writeVisibility)
            {
                WriteVisibility(method.Visibility);
            }

            WriteMethodModifiers(method);

            WriteInterfaceMethodModifiers(method);
            WriteMethodDefinitionSignature(method);
            WriteMethodBody(method);
        }

        private void WriteDestructor(IMethodDefinition method)
        {
            WriteSymbol("~");
            WriteIdentifier(((INamedEntity)method.ContainingTypeDefinition).Name);
            WriteSymbol("(");
            WriteSymbol(")", false);
            WriteEmptyBody();
        }


        private void WriteTypeName(ITypeReference type, ITypeReference containingType, IEnumerable<ICustomAttribute> attributes = null, byte? methodNullableContextValue = null)
        {
            WriteTypeName(type, attributes: attributes, methodNullableContextValue: methodNullableContextValue);
        }

        private string GetNormalizedMethodName(IName name)
        {
            switch (name.Value)
            {
                case "op_Decrement": return "operator --";
                case "op_Increment": return "operator ++";
                case "op_UnaryNegation": return "operator -";
                case "op_UnaryPlus": return "operator +";
                case "op_LogicalNot": return "operator !";
                case "op_True": return "operator true";
                case "op_False": return "operator false";
                case "op_OnesComplement": return "operator ~";
                case "op_Addition": return "operator +";
                case "op_Subtraction": return "operator -";
                case "op_Multiply": return "operator *";
                case "op_Division": return "operator /";
                case "op_Modulus": return "operator %";
                case "op_ExclusiveOr": return "operator ^";
                case "op_BitwiseAnd": return "operator &";
                case "op_BitwiseOr": return "operator |";
                case "op_LeftShift": return "operator <<";
                case "op_RightShift": return "operator >>";
                case "op_UnsignedRightShift": return "operator >>>";
                case "op_Equality": return "operator ==";
                case "op_GreaterThan": return "operator >";
                case "op_LessThan": return "operator <";
                case "op_Inequality": return "operator !=";
                case "op_GreaterThanOrEqual": return "operator >=";
                case "op_LessThanOrEqual": return "operator <=";
                case "op_Explicit": return "explicit operator";
                case "op_Implicit": return "implicit operator";
                case "op_CheckedDecrement": return "operator checked --";
                case "op_CheckedIncrement": return "operator checked ++";
                case "op_CheckedUnaryNegation": return "operator checked -";
                case "op_CheckedAddition": return "operator checked +";
                case "op_CheckedSubtraction": return "operator checked -";
                case "op_CheckedMultiply": return "operator checked *";
                case "op_CheckedDivision": return "operator checked /";
                case "op_CheckedExplicit": return "explicit operator checked";
                default: return name.Value; // return just the name
            }
        }

        private void WriteMethodName(IMethodDefinition method)
        {
            if (method.IsConstructor || method.IsStaticConstructor)
            {
                INamedEntity named = method.ContainingTypeDefinition.UnWrap() as INamedEntity;
                if (named != null)
                {
                    WriteIdentifier(named.Name.Value);
                    return;
                }
            }

            if (method.IsExplicitInterfaceMethod())
            {
                IMethodImplementation methodImplementation = method.GetMethodImplementation();
                object nullableAttributeArgument = methodImplementation.GetExplicitInterfaceMethodNullableAttributeArgument(_metadataReaderCache);

                WriteTypeName(methodImplementation.ImplementedMethod.ContainingType, noSpace: true, nullableAttributeArgument: nullableAttributeArgument);
                WriteSymbol(".");
                WriteIdentifier(GetNormalizedMethodName(methodImplementation.ImplementedMethod.Name));
            }
            else
            {
                WriteIdentifier(GetNormalizedMethodName(method.Name));
            }
        }

        private void WriteMethodDefinitionSignature(IMethodDefinition method)
        {
            byte? nullableContextValue = method.Attributes.GetCustomAttributeArgumentValue<byte?>(CSharpCciExtensions.NullableContextAttributeFullName);
            bool isOperator = method.IsConversionOperator();

            if (!isOperator && !method.IsConstructor && !method.IsStaticConstructor)
            {
                if (method.Attributes.HasIsReadOnlyAttribute() && (LangVersion >= LangVersion8_0))
                {
                    WriteKeyword("readonly");
                }

                if (method.ReturnValueIsByRef)
                {
                    WriteKeyword("ref");

                    if (method.ReturnValueAttributes.HasIsReadOnlyAttribute())
                        WriteKeyword("readonly");
                }

                // We are ignoring custom modifiers right now, we might need to add them later.
                WriteTypeName(method.Type, method.ContainingType, method.ReturnValueAttributes, nullableContextValue);
            }

            if (method.IsExplicitInterfaceMethod() && _forCompilationIncludeGlobalprefix)
                Write("global::");

            WriteMethodName(method);

            if (isOperator)
            {
                WriteSpace();

                WriteTypeName(method.Type, method.ContainingType, methodNullableContextValue: nullableContextValue);
            }

            Contract.Assert(method is not IGenericMethodInstance, "Currently don't support generic method instances");
            if (method.IsGeneric)
                WriteGenericParameters(method.GenericParameters);

            WriteParameters(method.Parameters, method.ContainingType, nullableContextValue, extensionMethod: method.IsExtensionMethod(), acceptsExtraArguments: method.AcceptsExtraArguments);
            if (method.IsGeneric && !method.IsOverride() && !method.IsExplicitInterfaceMethod())
                WriteGenericContraints(method.GenericParameters, nullableContextValue);
        }

        private void WriteParameters(IEnumerable<IParameterDefinition> parameters, ITypeReference containingType, byte? methodNullableContextValue, bool property = false, bool extensionMethod = false, bool acceptsExtraArguments = false)
        {
            string start = property ? "[" : "(";
            string end = property ? "]" : ")";

            WriteSymbol(start);
            _writer.WriteList(parameters, p =>
            {
                WriteParameter(p, containingType, extensionMethod, methodNullableContextValue);
                extensionMethod = false;
            });

            if (acceptsExtraArguments)
            {
                if (parameters.Any())
                    _writer.WriteSymbol(",");
                _writer.WriteSpace();
                _writer.Write("__arglist");
            }

            WriteSymbol(end);
        }

        private void WriteParameter(IParameterDefinition parameter, ITypeReference containingType, bool extensionMethod, byte? methodNullableContextValue)
        {
            WriteAttributes(parameter.Attributes, true);

            if (extensionMethod)
                WriteKeyword("this");

            if (parameter.IsParameterArray)
                WriteKeyword("params");

            if (parameter.IsOut && !parameter.IsIn && parameter.IsByReference)
            {
                WriteKeyword("out");
            }
            else
            {
                // For In/Out we should not emit them until we find a scenario that is needs them.
                //if (parameter.IsIn)
                //   WriteFakeAttribute("System.Runtime.InteropServices.In", writeInline: true);
                //if (parameter.IsOut)
                //    WriteFakeAttribute("System.Runtime.InteropServices.Out", writeInline: true);
                if (parameter.IsByReference)
                {
                    if (parameter.Attributes.HasIsReadOnlyAttribute())
                    {
                        WriteKeyword("in");
                    }
                    else
                    {
                        WriteKeyword("ref");
                    }
                }
            }

            WriteTypeName(parameter.Type, containingType, parameter.Attributes, methodNullableContextValue);
            WriteIdentifier(parameter.Name);
            if (parameter.IsOptional && parameter.HasDefaultValue)
            {
                WriteSymbol(" = ");
                WriteMetadataConstant(parameter.DefaultValue, parameter.Type);
            }
        }

        private void WriteInterfaceMethodModifiers(IMethodDefinition method)
        {
            if (method.GetHiddenBaseMethod(_filter) != Dummy.Method)
                WriteKeyword("new");
        }

        private void WriteMethodModifiers(IMethodDefinition method)
        {
            if (method.IsMethodUnsafe() ||
                (method.IsConstructor && IsBaseConstructorCallUnsafe(method.ContainingTypeDefinition)))
            {
                WriteKeyword("unsafe");
            }

            if (method.IsStatic)
                WriteKeyword("static");

            if (method.IsPlatformInvoke)
                WriteKeyword("extern");

            if (method.IsVirtual)
            {
                if (method.ContainingTypeDefinition.IsInterface)
                {
                    if (method.IsStatic && method.IsAbstract)
                    {
                        WriteKeyword("abstract");
                    }
                }
                else if (method.IsNewSlot)
                {
                    if (method.IsAbstract)
                        WriteKeyword("abstract");
                    else if (!method.IsSealed) // non-virtual interfaces implementations are sealed virtual newslots
                        WriteKeyword("virtual");
                }
                else
                {
                    if (method.IsAbstract)
                        WriteKeyword("abstract");
                    else if (method.IsSealed)
                        WriteKeyword("sealed");
                    WriteKeyword("override");
                }
            }
        }

        private void WriteMethodBody(IMethodDefinition method)
        {
            if (method.IsAbstract || !_forCompilation || method.IsPlatformInvoke)
            {
                WriteSymbol(";");
                return;
            }

            if (method.IsConstructor)
                WriteBaseConstructorCall(method.ContainingTypeDefinition);

            // Write Dummy Body
            WriteSpace();
            WriteSymbol("{", true);

            if (_platformNotSupportedExceptionMessage != null && !method.IsDispose())
            {
                Write("throw new ");
                if (_forCompilationIncludeGlobalprefix)
                    Write("global::");

                Write("System.PlatformNotSupportedException(");

                if (_platformNotSupportedExceptionMessage.StartsWith("SR."))
                {
                    if (_forCompilationIncludeGlobalprefix)
                        Write("global::");
                    Write($"System.{ _platformNotSupportedExceptionMessage}");
                }
                else if (_platformNotSupportedExceptionMessage.Length > 0)
                    Write($"\"{_platformNotSupportedExceptionMessage}\"");

                Write("); ");
            }
            else if (NeedsMethodBodyForCompilation(method))
            {
                Write("throw null; ");
            }

            WriteSymbol("}");
        }

        private bool NeedsMethodBodyForCompilation(IMethodDefinition method)
        {
            // Structs cannot have empty constructors so we need a body
            if (method.ContainingTypeDefinition.IsValueType && method.IsConstructor)
                return true;

            // Compiler requires out parameters to be initialized
            if (method.Parameters.Any(p => p.IsOut))
                return true;

            // For non-void returning methods we need a body.
            if (!TypeHelper.TypesAreEquivalent(method.Type, method.ContainingTypeDefinition.PlatformType.SystemVoid))
                return true;

            return false;
        }

        private void WritePrivateConstructor(ITypeDefinition type)
        {
            if (!_forCompilation ||
                type.IsInterface ||
                type.IsEnum ||
                type.IsDelegate ||
                type.IsValueType ||
                type.IsStatic)
                return;

            var visibility = Filter switch
            {
                IncludeAllFilter _ => TypeMemberVisibility.Private,
                InternalsAndPublicCciFilter _ => TypeMemberVisibility.Private,
                IntersectionFilter intersection => intersection.Filters.Any(
                        f => f is IncludeAllFilter || f is InternalsAndPublicCciFilter) ?
                    TypeMemberVisibility.Private :
                    TypeMemberVisibility.Assembly,
                _ => TypeMemberVisibility.Assembly
            };

            WriteVisibility(visibility);
            if (IsBaseConstructorCallUnsafe(type))
            {
                WriteKeyword("unsafe");
            }

            WriteIdentifier(((INamedEntity)type).Name);
            WriteSymbol("(");
            WriteSymbol(")");
            WriteBaseConstructorCall(type);
            WriteEmptyBody();
        }

        private void WriteBaseConstructorCall(ITypeDefinition type)
        {
            var ctor = GetBaseConstructorForCall(type);
            if (ctor == null)
                return;

            WriteSpace();
            WriteSymbol(":", true);
            WriteKeyword("base");
            WriteSymbol("(");
            _writer.WriteList(ctor.Parameters, p => WriteDefaultOf(p.Type, ShouldSuppressNullCheck()));
            WriteSymbol(")");
        }

        private bool IsBaseConstructorCallUnsafe(ITypeDefinition type)
        {
            var constructor = GetBaseConstructorForCall(type);
            if (constructor == null)
            {
                return false;
            }

            foreach (var parameter in constructor.Parameters)
            {
                if (parameter.Type.IsUnsafeType())
                {
                    return true;
                }
            }

            return false;
        }

        private IMethodDefinition GetBaseConstructorForCall(ITypeDefinition type)
        {
            if (!_forCompilation)
            {
                // No need to generate a call to a base constructor.
                return null;
            }

            var baseType = type.BaseClasses.FirstOrDefault().GetDefinitionOrNull();
            if (baseType == null)
            {
                // No base type to worry about.
                return null;
            }

            var constructors = baseType.Methods.Where(
                m => m.IsConstructor && _filter.Include(m) && !m.Attributes.Any(a => a.IsObsoleteWithUsageTreatedAsCompilationError()));

            if (constructors.Any(c => c.ParameterCount == 0))
            {
                // Don't need a base call if base class has a default constructor.
                return null;
            }

            return constructors.FirstOrDefault();
        }

        /// <summary>
        /// When generated .notsupported.cs files, we need to generate calls to the base constructor.
        /// However, if the base constructor doesn't accept null, passing default(T) will cause a compile
        /// error. In this case, suppress the null check.
        /// NOTE: It was deemed too much work to dynamically check if the base constructor accepts null
        /// or not, until we update GenAPI to be based on Roslyn instead of CCI. For now, just always
        /// suppress the null check.
        /// </summary>
        private bool ShouldSuppressNullCheck() =>
            LangVersion >= LangVersion8_0 &&
            _platformNotSupportedExceptionMessage != null;

        private void WriteEmptyBody()
        {
            if (!_forCompilation)
            {
                WriteSymbol(";");
            }
            else
            {
                WriteSpace();
                WriteSymbol("{", true);
                WriteSymbol("}");
            }
        }

        private void WriteDefaultOf(ITypeReference type, bool suppressNullCheck = false)
        {
            WriteKeyword("default", true);
            WriteSymbol("(");
            WriteTypeName(type, noSpace: true);
            WriteSymbol(")");

            if (suppressNullCheck && !type.IsValueType)
            {
                WriteSymbol("!");
            }
        }

        public static IDefinition GetDummyConstructor(ITypeDefinition type)
        {
            return new DummyInternalConstructor() { ContainingType = type };
        }

        private class DummyInternalConstructor : IDefinition
        {
            public ITypeDefinition ContainingType { get; set; }

            public IEnumerable<ICustomAttribute> Attributes
            {
                get { throw new System.NotImplementedException(); }
            }

            public void Dispatch(IMetadataVisitor visitor)
            {
                throw new System.NotImplementedException();
            }

            public IEnumerable<ILocation> Locations
            {
                get { throw new System.NotImplementedException(); }
            }

            public void DispatchAsReference(IMetadataVisitor visitor)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
