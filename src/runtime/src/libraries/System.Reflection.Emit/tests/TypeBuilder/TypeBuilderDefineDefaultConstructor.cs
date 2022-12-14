// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Xunit;

namespace System.Reflection.Emit.Tests
{
    public class TypeBuilderDefineDefaultConstructor
    {
        [Theory]
        [ActiveIssue("https://github.com/dotnet/runtime/issues/2389", TestRuntimes.Mono)]
        [InlineData(MethodAttributes.Public)]
        [InlineData(MethodAttributes.Static)]
        [InlineData(MethodAttributes.Family)]
        [InlineData(MethodAttributes.Assembly)]
        [InlineData(MethodAttributes.Private)]
        [InlineData(MethodAttributes.PrivateScope)]
        [InlineData(MethodAttributes.FamORAssem)]
        [InlineData(MethodAttributes.FamANDAssem)]
        [InlineData(MethodAttributes.Final | MethodAttributes.Public)]
        [InlineData(MethodAttributes.Final | MethodAttributes.Family)]
        [InlineData(MethodAttributes.SpecialName | MethodAttributes.Family)]
        [InlineData(MethodAttributes.UnmanagedExport | MethodAttributes.Family)]
        [InlineData(MethodAttributes.RTSpecialName | MethodAttributes.Family)]
        [InlineData(MethodAttributes.HideBySig | MethodAttributes.Family)]
        [InlineData((MethodAttributes)0x8000)]
        public void DefineDefaultConstructor(MethodAttributes attributes)
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.Class | TypeAttributes.Public);
            ConstructorBuilder constructor = type.DefineDefaultConstructor(attributes);
            Helpers.VerifyConstructor(constructor, type, attributes, CallingConventions.Standard, new Type[0]);
        }

        public static bool s_ranConstructor = false;

        [ConditionalFact(typeof(PlatformDetection), nameof(PlatformDetection.IsReflectionEmitSupported))]
        public void DefineDefaultConstructor_GenericParentCreated_Works()
        {
            ModuleBuilder module = Helpers.DynamicModule();
            TypeBuilder genericTypeDefinition = module.DefineType("GenericType", TypeAttributes.Public);
            genericTypeDefinition.DefineGenericParameters("T");

            ConstructorBuilder constructor = genericTypeDefinition.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
            ILGenerator constructorILGenerator = constructor.GetILGenerator();
            constructorILGenerator.Emit(OpCodes.Ldarg_0);
            constructorILGenerator.Emit(OpCodes.Ldc_I4_1);
            constructorILGenerator.Emit(OpCodes.Stfld, typeof(TypeBuilderDefineDefaultConstructor).GetField(nameof(s_ranConstructor)));
            constructorILGenerator.Emit(OpCodes.Ret);

            genericTypeDefinition.CreateTypeInfo();

            Type genericParent = genericTypeDefinition.MakeGenericType(typeof(int));

            TypeBuilder type = module.DefineType("Type");
            type.SetParent(genericParent);
            type.DefineDefaultConstructor(MethodAttributes.Public);

            Type createdType = type.CreateType();
            ConstructorInfo defaultConstructor = createdType.GetConstructor(new Type[0]);
            defaultConstructor.Invoke(null);
            Assert.True(s_ranConstructor);
        }

        [Fact]
        public void DefineDefaultConstructor_CalledMultipleTimes_Works()
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.Public);

            type.DefineDefaultConstructor(MethodAttributes.Public);
            type.DefineDefaultConstructor(MethodAttributes.Public);

            Type createdType = type.CreateType();
            ConstructorInfo[] constructors = createdType.GetConstructors();
            Assert.Equal(2, constructors.Length);
            Assert.Equal(constructors[0].GetParameters(), constructors[1].GetParameters());
        }

        [Fact]
        public void DefineDefaultConstructor_TypeCreated_ThrowsInvalidOperationException()
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.Public);
            type.CreateType();
            Assert.Throws<InvalidOperationException>(() => type.DefineDefaultConstructor(MethodAttributes.Public));
        }

        [Fact]
        public void DefineDefaultConstructor_Interface_ThrowsInvalidOperationException()
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
            Assert.Throws<InvalidOperationException>(() => type.DefineDefaultConstructor(MethodAttributes.Public));
        }

        [Fact]
        public void DefineDefaultConstructor_NoDefaultConstructor_ThrowsNotSupportedException()
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.Public | TypeAttributes.Class);

            FieldBuilder field = type.DefineField("TestField", typeof(int), FieldAttributes.Family);

            ConstructorBuilder constructor = type.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, new Type[] { typeof(int) });
            ILGenerator constructorIlGenerator = constructor.GetILGenerator();

            constructorIlGenerator.Emit(OpCodes.Ldarg_0);
            constructorIlGenerator.Emit(OpCodes.Call, typeof(object).GetConstructor(new Type[0]));

            constructorIlGenerator.Emit(OpCodes.Ldarg_0);
            constructorIlGenerator.Emit(OpCodes.Ldarg_1);
            constructorIlGenerator.Emit(OpCodes.Stfld, field);

            constructorIlGenerator.Emit(OpCodes.Ret);

            Type createdType = type.CreateType();
            TypeBuilder nestedType = Helpers.DynamicType(TypeAttributes.Public | TypeAttributes.Class);
            nestedType.SetParent(createdType);

            Assert.Throws<NotSupportedException>(() => nestedType.DefineDefaultConstructor(MethodAttributes.Public));
        }

        [Theory]
        [InlineData(MethodAttributes.Private)]
        [InlineData(MethodAttributes.PrivateScope)]
        public void DefineDefaultConstructor_PrivateDefaultConstructor_ThrowsNotSupportedException(MethodAttributes attributes)
        {
            TypeBuilder baseType = Helpers.DynamicType(TypeAttributes.Public | TypeAttributes.Class);
            ConstructorBuilder constructor = baseType.DefineConstructor(attributes, CallingConventions.HasThis, new Type[] { typeof(int) });
            constructor.GetILGenerator().Emit(OpCodes.Ret);

            Type createdParentType = baseType.CreateType();

            TypeBuilder type = Helpers.DynamicType(TypeAttributes.Public | TypeAttributes.Class);
            type.SetParent(createdParentType);
            Assert.Throws<NotSupportedException>(() => type.DefineDefaultConstructor(MethodAttributes.Public));
        }

        [Fact]
        public void DefineDefaultConstructor_ParentNotCreated_ThrowsNotSupportedException()
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.Public);
            TypeBuilder parentType = Helpers.DynamicType(TypeAttributes.Public);
            type.SetParent(parentType.AsType());

            Assert.Throws<NotSupportedException>(() => type.DefineDefaultConstructor(MethodAttributes.Public));
        }

        [Fact]
        public void DefineDefaultConstructor_GenericParentNotCreated_ThrowsNotSupportedException()
        {
            ModuleBuilder module = Helpers.DynamicModule();
            TypeBuilder genericTypeDefinition = module.DefineType("GenericType", TypeAttributes.Public);
            genericTypeDefinition.DefineGenericParameters("T");
            Type genericParent = genericTypeDefinition.MakeGenericType(typeof(int));

            TypeBuilder type = module.DefineType("Type");
            type.SetParent(genericParent);
            Assert.Throws<NotSupportedException>(() => type.DefineDefaultConstructor(MethodAttributes.Public));
        }

        [Fact]
        public void DefineDefaultConstructor_StaticVirtual_ThrowsArgumentException()
        {
            TypeBuilder type = Helpers.DynamicType(TypeAttributes.Public);
            AssertExtensions.Throws<ArgumentException>(null, () => type.DefineDefaultConstructor(MethodAttributes.Virtual | MethodAttributes.Static));
        }
    }
}
