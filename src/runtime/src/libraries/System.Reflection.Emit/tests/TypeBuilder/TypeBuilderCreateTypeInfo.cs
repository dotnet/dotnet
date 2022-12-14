// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit;

namespace System.Reflection.Emit.Tests
{
    public class TypeBuilderCreateTypeInfo
    {
        [Theory]
        [InlineData(TypeAttributes.Abstract)]
        [InlineData(TypeAttributes.AnsiClass)]
        [InlineData(TypeAttributes.AutoClass)]
        [InlineData(TypeAttributes.BeforeFieldInit)]
        [InlineData(TypeAttributes.ClassSemanticsMask | TypeAttributes.Abstract)]
        [InlineData(TypeAttributes.Public)]
        [InlineData(TypeAttributes.Sealed)]
        [InlineData(TypeAttributes.SequentialLayout)]
        [InlineData(TypeAttributes.Serializable)]
        [InlineData(TypeAttributes.SpecialName)]
        [InlineData(TypeAttributes.StringFormatMask)]
        [InlineData(TypeAttributes.UnicodeClass)]
        public void CreateType(TypeAttributes attributes)
        {
            TypeBuilder type = Helpers.DynamicType(attributes);
            Type createdType = type.CreateType();
            Assert.Equal(type.Name, createdType.Name);

            Assert.Equal(type.CreateTypeInfo(), createdType.GetTypeInfo());
        }

        [Fact]
        public void CreateType_NestedType()
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.NotPublic);
            type.DefineNestedType("NestedType");

            Type createdType = type.CreateType();
            Assert.Equal(type.Name, createdType.Name);
        }

        [Fact]
        public void CreateType_GenericType()
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.NotPublic);
            type.DefineGenericParameters("T");

            Type createdType = type.CreateType();
            Assert.Equal(type.Name, createdType.Name);
        }

        [Theory]
        [ActiveIssue("https://github.com/dotnet/runtime/issues/2389", TestRuntimes.Mono)]
        [InlineData(TypeAttributes.ClassSemanticsMask, typeof(InvalidOperationException))]
        [InlineData(TypeAttributes.HasSecurity, typeof(ArgumentException))]
        [InlineData(TypeAttributes.LayoutMask, typeof(ArgumentException))]
        [InlineData(TypeAttributes.NestedAssembly, typeof(ArgumentException))]
        [InlineData(TypeAttributes.NestedFamANDAssem, typeof(ArgumentException))]
        [InlineData(TypeAttributes.NestedFamily, typeof(ArgumentException))]
        [InlineData(TypeAttributes.NestedFamORAssem, typeof(ArgumentException))]
        [InlineData(TypeAttributes.NestedPrivate, typeof(ArgumentException))]
        [InlineData(TypeAttributes.NestedPublic, typeof(ArgumentException))]
        [InlineData(TypeAttributes.RTSpecialName, typeof(ArgumentException))]
        public void CreateType_InvalidTypeAttributes_Throws(TypeAttributes attributes, Type exceptionType)
        {
            Assert.Throws(exceptionType, () => Helpers.DynamicType(attributes));
        }
    }
}
