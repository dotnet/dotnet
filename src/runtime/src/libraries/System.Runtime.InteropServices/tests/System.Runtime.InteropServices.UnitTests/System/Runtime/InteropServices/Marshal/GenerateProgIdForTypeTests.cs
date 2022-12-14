// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.Tests.Common;
using Xunit;

namespace System.Runtime.InteropServices.Tests
{
    public partial class GenerateProgIdForTypeTests
    {
        public static IEnumerable<object[]> GenerateProgIdForType_Valid_TestData()
        {
            yield return new object[] { typeof(int), typeof(int).FullName };
            yield return new object[] { typeof(NonGenericClass), typeof(NonGenericClass).FullName };
            yield return new object[] { typeof(AbstractClass), typeof(AbstractClass).FullName };
            yield return new object[] { typeof(NonGenericStruct), typeof(NonGenericStruct).FullName };
            yield return new object[] { typeof(ClassWithProgID), "TestProgID" };
            yield return new object[] { typeof(ClassWithNullProgID), "" };
        }

        [ConditionalTheory(typeof(PlatformDetection), nameof(PlatformDetection.IsBuiltInComEnabled))]
        [MemberData(nameof(GenerateProgIdForType_Valid_TestData))]
        public void GenerateProgIdForType_ValidType_ReturnsExpected(Type type, string expected)
        {
            Assert.Equal(expected, Marshal.GenerateProgIdForType(type));
        }

        [ComVisible(true)]
        [ProgId("TestProgID")]
        public class ClassWithProgID
        {
        }

        [ComVisible(true)]
        [ProgId(null)]
        public class ClassWithNullProgID
        {
        }

        [ConditionalFact(typeof(PlatformDetection), nameof(PlatformDetection.IsBuiltInComEnabled))]
        public void GenerateProgIdForType_NullType_ThrowsArgumentNullException()
        {
            AssertExtensions.Throws<ArgumentNullException>("type", () => Marshal.GenerateProgIdForType(null));
        }

        public static IEnumerable<object[]> GenerateProgIdForType_Invalid_TestData()
        {
            yield return new object[] { typeof(int).MakePointerType() };
            yield return new object[] { typeof(int).MakeByRefType() };
            yield return new object[] { typeof(string[]) };

            yield return new object[] { typeof(GenericClass<string>) };
            yield return new object[] { typeof(GenericStruct<string>) };
            yield return new object[] { typeof(INonGenericInterface) };
            yield return new object[] { typeof(IGenericInterface<string>) };

            yield return new object[] { typeof(GenericClass<>) };
            yield return new object[] { typeof(GenericClass<>).GetTypeInfo().GenericTypeParameters[0] };

            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("Assembly"), AssemblyBuilderAccess.RunAndCollect);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("Module");
            TypeBuilder typeBuilder = moduleBuilder.DefineType("Type");
            Type collectibleType = typeBuilder.CreateType();
            yield return new object[] { collectibleType };
        }
    }
}
