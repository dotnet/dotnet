// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities.UnitTests;

[TestClass]
[DeploymentItem("TestClasses.cs")]
public partial class ManagedNameRoundTripTests
{
    private const BindingFlags PrivateBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    private readonly Compilation _compilation;

    public ManagedNameRoundTripTests()
        => _compilation = CSharpCompilation.Create(
            "Test.dll",
            new[] { CSharpSyntaxTree.ParseText(File.ReadAllText("TestClasses.cs")) },
            new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) });

    [TestMethod]
    public void Simple1()
    {
        var outer = _compilation.GetTypeByMetadataName("TestClasses.Outer")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer).GetMethod("Method0")!,
            containingTypeSymbol: outer,
            methodSymbol: outer.FindMethod("Method0")!,
            managedTypeName: "TestClasses.Outer",
            managedMethodName: "Method0");
    }

    [TestMethod]
    public void Simple2()
    {
        var outer = _compilation.GetTypeByMetadataName("TestClasses.Outer")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer).GetMethod("Method1")!,
            containingTypeSymbol: outer,
            methodSymbol: outer.FindMethod("Method1")!,
            managedTypeName: "TestClasses.Outer",
            managedMethodName: "Method1(System.Int32)");
    }

    [TestMethod]
    public void Simple3()
    {
        var outer = _compilation.GetTypeByMetadataName("TestClasses.Outer")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer).GetMethod("Method2")!,
            containingTypeSymbol: outer,
            methodSymbol: outer.FindMethod("Method2")!,
            managedTypeName: "TestClasses.Outer",
            managedMethodName: "Method2(System.Collections.Generic.List`1<System.String>)");
    }

    [TestMethod]
    public void Simple4()
    {
        var outer = _compilation.GetTypeByMetadataName("TestClasses.Outer")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer).GetMethod("Method3")!,
            containingTypeSymbol: outer,
            methodSymbol: outer.FindMethod("Method3")!,
            managedTypeName: "TestClasses.Outer",
            managedMethodName: "Method3(System.String,System.Int32)");
    }

    [TestMethod]
    public void Nested1()
    {
        var outerInner = _compilation.GetTypeByMetadataName("TestClasses.Outer+Inner")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer.Inner).GetMethod("Method0")!,
            containingTypeSymbol: outerInner,
            methodSymbol: outerInner.FindMethod("Method0")!,
            managedTypeName: "TestClasses.Outer+Inner",
            managedMethodName: "Method0");
    }

    [TestMethod]
    public void Nested2()
    {
        var outerInner = _compilation.GetTypeByMetadataName("TestClasses.Outer+Inner")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer.Inner).GetMethod("Method1")!,
            containingTypeSymbol: outerInner,
            methodSymbol: outerInner.FindMethod("Method1")!,
            managedTypeName: "TestClasses.Outer+Inner",
            managedMethodName: "Method1(System.Int32)");
    }

    [TestMethod]
    public void OpenGeneric1()
    {
        var outerT = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>).GetMethod("Method0")!,
            containingTypeSymbol: outerT,
            methodSymbol: outerT.FindMethod("Method0")!,
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method0");
    }

    [TestMethod]
    public void OpenGeneric2()
    {
        var outerT = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>).GetMethod("Method1")!,
            containingTypeSymbol: outerT,
            methodSymbol: outerT.FindMethod("Method1")!,
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method1(!0)");
    }

    [TestMethod]
    public void OpenGeneric3()
    {
        var outerT = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>).GetMethod("Method2")!,
            containingTypeSymbol: outerT,
            methodSymbol: outerT.FindMethod("Method2")!,
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method2`1(!!0[])");
    }

    [TestMethod]
    public void OpenGeneric4()
    {
        var outerT = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>).GetMethod("Method3")!,
            containingTypeSymbol: outerT,
            methodSymbol: outerT.FindMethod("Method3")!,
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method3`1(!0,!!0)");
    }

    [TestMethod]
    public void OpenGenericNested1()
    {
        var outerTInnterV = _compilation.GetTypeByMetadataName("TestClasses.Outer`1+Inner`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>.Inner<>).GetMethod("Method0")!,
            containingTypeSymbol: outerTInnterV,
            methodSymbol: outerTInnterV.FindMethod("Method0")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method0");
    }

    [TestMethod]
    public void OpenGenericNested2()
    {
        var outerTInnterV = _compilation.GetTypeByMetadataName("TestClasses.Outer`1+Inner`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>.Inner<>).GetMethod("Method1")!,
            containingTypeSymbol: outerTInnterV,
            methodSymbol: outerTInnterV.FindMethod("Method1")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method1(!0)");
    }

    [TestMethod]
    public void OpenGenericNested3()
    {
        var outerTInnterV = _compilation.GetTypeByMetadataName("TestClasses.Outer`1+Inner`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>.Inner<>).GetMethod("Method2")!,
            containingTypeSymbol: outerTInnterV,
            methodSymbol: outerTInnterV.FindMethod("Method2")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method2(!1)");
    }

    [TestMethod]
    public void OpenGenericNested4()
    {
        var outerTInnterV = _compilation.GetTypeByMetadataName("TestClasses.Outer`1+Inner`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>.Inner<>).GetMethod("Method3")!,
            containingTypeSymbol: outerTInnterV,
            methodSymbol: outerTInnterV.FindMethod("Method3")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method3`1(!0,!!0,!1)");
    }

    [TestMethod]
    public void OpenGenericNested5()
    {
        var outerTInnterV = _compilation.GetTypeByMetadataName("TestClasses.Outer`1+Inner`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>.Inner<>).GetMethod("Method4")!,
            containingTypeSymbol: outerTInnterV,
            methodSymbol: outerTInnterV.FindMethod("Method4")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method4`2(!!1,!!0)");
    }

    [TestMethod]
    public void OpenGenericNested6()
    {
        var outerTInnerVMoreInnerI = _compilation.GetTypeByMetadataName("TestClasses.Outer`1+Inner`1+MoreInner`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<>.Inner<>.MoreInner<>).GetMethod("Method0")!,
            containingTypeSymbol: outerTInnerVMoreInnerI,
            methodSymbol: outerTInnerVMoreInnerI.FindMethod("Method0")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1+MoreInner`1",
            managedMethodName: "Method0`1(!0,!1,!2,!!0)");
    }

    [TestMethod]
    public void ClosedGeneric1()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>).GetMethod("Method0")!,
            containingTypeSymbol: outerTInt,
            methodSymbol: outerTInt.FindMethod("Method0")!,
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method0");
    }

    [TestMethod]
    public void ClosedGeneric2()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>).GetMethod("Method1")!,
            containingTypeSymbol: outerTInt,
            methodSymbol: outerTInt.FindMethod("Method1")!,
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method1(!0)");
    }

    [TestMethod]
    public void ClosedGeneric3()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>).GetMethod("Method2")!,
            containingTypeSymbol: outerTInt,
            methodSymbol: outerTInt.FindMethod("Method2")!,
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method2`1(!!0[])");
    }

    [TestMethod]
    public void ClosedGeneric4()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>).GetMethod("Method3")!,
            containingTypeSymbol: outerTInt,
            methodSymbol: outerTInt.FindMethod("Method3")!,
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method3`1(!0,!!0)");
    }

    [TestMethod]
    public void ClosedGenericNested1()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var @string = _compilation.GetSpecialType(SpecialType.System_String);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);
        var outerTIntInnerVString = outerTInt.GetTypeMembers().Single().Construct(@string);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>.Inner<string>).GetMethod("Method0")!,
            containingTypeSymbol: outerTIntInnerVString,
            methodSymbol: outerTIntInnerVString.FindMethod("Method0")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method0");
    }

    [TestMethod]
    public void ClosedGenericNested2()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var @string = _compilation.GetSpecialType(SpecialType.System_String);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);
        var outerTIntInnerVString = outerTInt.GetTypeMembers().Single().Construct(@string);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>.Inner<string>).GetMethod("Method1")!,
            containingTypeSymbol: outerTIntInnerVString,
            methodSymbol: outerTIntInnerVString.FindMethod("Method1")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method1(!0)");
    }

    [TestMethod]
    public void ClosedGenericNested3()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var @string = _compilation.GetSpecialType(SpecialType.System_String);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);
        var outerTIntInnerVString = outerTInt.GetTypeMembers().Single().Construct(@string);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>.Inner<string>).GetMethod("Method2")!,
            containingTypeSymbol: outerTIntInnerVString,
            methodSymbol: outerTIntInnerVString.FindMethod("Method2")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method2(!1)");
    }

    [TestMethod]
    public void ClosedGenericNested4()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var @string = _compilation.GetSpecialType(SpecialType.System_String);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);
        var outerTIntInnerVString = outerTInt.GetTypeMembers().Single().Construct(@string);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>.Inner<string>).GetMethod("Method3")!,
            containingTypeSymbol: outerTIntInnerVString,
            methodSymbol: outerTIntInnerVString.FindMethod("Method3")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method3`1(!0,!!0,!1)");
    }

    [TestMethod]
    public void ClosedGenericNested5()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var @string = _compilation.GetSpecialType(SpecialType.System_String);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);
        var outerTIntInnerVString = outerTInt.GetTypeMembers().Single().Construct(@string);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>.Inner<string>).GetMethod("Method4")!,
            containingTypeSymbol: outerTIntInnerVString,
            methodSymbol: outerTIntInnerVString.FindMethod("Method4")!,
            managedTypeName: "TestClasses.Outer`1+Inner`1",
            managedMethodName: "Method4`2(!!1,!!0)");
    }

    [TestMethod]
    public void ClosedGenericMethod1()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var @string = _compilation.GetSpecialType(SpecialType.System_String);
        var outerTInt = _compilation.GetTypeByMetadataName("TestClasses.Outer`1")!.Construct(@int);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer<int>).GetMethod("Method3")!.MakeGenericMethod(typeof(string)),
            containingTypeSymbol: outerTInt,
            methodSymbol: outerTInt.FindMethod("Method3")!.Construct(@string),
            managedTypeName: "TestClasses.Outer`1",
            managedMethodName: "Method3`1(!0,!!0)");
    }

    [TestMethod]
    public void ClosedGenericMethod2()
    {
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var outerInner = _compilation.GetTypeByMetadataName("TestClasses.Outer+Inner")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer.Inner).GetMethod("Method2")!.MakeGenericMethod(typeof(int)),
            containingTypeSymbol: outerInner,
            methodSymbol: outerInner.FindMethod("Method2")!.Construct(@int),
            managedTypeName: "TestClasses.Outer+Inner",
            managedMethodName: "Method2`1(System.Int32)");
    }

    [TestMethod]
    public void ClosedGenericMethod3()
    {
        _ = _compilation.GetSpecialType(SpecialType.System_Int32);
        var @float = _compilation.GetSpecialType(SpecialType.System_Single);
        var @string = _compilation.GetSpecialType(SpecialType.System_String);
        var outerInner = _compilation.GetTypeByMetadataName("TestClasses.Outer+Inner")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Outer.Inner).GetMethod("Method3")!.MakeGenericMethod(typeof(float), typeof(string)),
            containingTypeSymbol: outerInner,
            methodSymbol: outerInner.FindMethod("Method3")!.Construct(@float, @string),
            managedTypeName: "TestClasses.Outer+Inner",
            managedMethodName: "Method3`2(System.Int32)");
    }

    [TestMethod]
    public void ExplicitInterfaceImplementation1()
    {
        var impl = _compilation.GetTypeByMetadataName("TestClasses.Impl")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Impl).GetMethod("TestClasses.IImplementation.ImplMethod0", PrivateBindingFlags)!,
            containingTypeSymbol: impl,
            methodSymbol: impl.FindMethod("TestClasses.IImplementation.ImplMethod0")!,
            managedTypeName: "TestClasses.Impl",
            managedMethodName: "TestClasses.IImplementation.ImplMethod0");
    }

    [TestMethod]
    public void ExplicitInterfaceImplementation2()
    {
        var impl = _compilation.GetTypeByMetadataName("TestClasses.Impl")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Impl).GetMethod("TestClasses.IImplementation.ImplMethod1", PrivateBindingFlags)!,
            containingTypeSymbol: impl,
            methodSymbol: impl.FindMethod("TestClasses.IImplementation.ImplMethod1")!,
            managedTypeName: "TestClasses.Impl",
            managedMethodName: "TestClasses.IImplementation.ImplMethod1(System.Int32)");
    }

    [TestMethod]
    public void GenericExplicitInterfaceImplementation1()
    {
        var implT = _compilation.GetTypeByMetadataName("TestClasses.Impl`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Impl<>).GetMethod("TestClasses.IImplementation<T>.ImplMethod0", PrivateBindingFlags)!,
            containingTypeSymbol: implT,
            methodSymbol: implT.FindMethod("TestClasses.IImplementation<T>.ImplMethod0")!,
            managedTypeName: "TestClasses.Impl`1",
            managedMethodName: "TestClasses.IImplementation<T>.ImplMethod0");
    }

    [TestMethod]
    public void GenericExplicitInterfaceImplementation2()
    {
        var implT = _compilation.GetTypeByMetadataName("TestClasses.Impl`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Impl<>).GetMethod("TestClasses.IImplementation<T>.ImplMethod1", PrivateBindingFlags)!,
            containingTypeSymbol: implT,
            methodSymbol: implT.FindMethod("TestClasses.IImplementation<T>.ImplMethod1")!,
            managedTypeName: "TestClasses.Impl`1",
            managedMethodName: "TestClasses.IImplementation<T>.ImplMethod1(!0)");
    }

    [TestMethod]
    public void GenericExplicitInterfaceImplementation3()
    {
        var implT = _compilation.GetTypeByMetadataName("TestClasses.Impl`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Impl<>).GetMethod("TestClasses.IImplementation<T>.ImplMethod2", PrivateBindingFlags)!,
            containingTypeSymbol: implT,
            methodSymbol: implT.FindMethod("TestClasses.IImplementation<T>.ImplMethod2")!,
            managedTypeName: "TestClasses.Impl`1",
            managedMethodName: "TestClasses.IImplementation<T>.ImplMethod2`1(!0,!!0,System.String)");
    }

    [TestMethod]
    public void Inheritance1()
    {
        var outerPrime = _compilation.GetTypeByMetadataName("TestClasses.OuterPrime")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.OuterPrime).GetMethod("Method3")!,
            containingTypeSymbol: outerPrime,
            methodSymbol: outerPrime.FindMethod("Method3")!,
            managedTypeName: "TestClasses.OuterPrime",
            managedMethodName: "Method3(System.String,System.Int32)");
    }

    [TestMethod]
    public void Inheritance2()
    {
        var outerPrimeZ = _compilation.GetTypeByMetadataName("TestClasses.OuterPrime`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.OuterPrime<>).GetMethod("Method3")!,
            containingTypeSymbol: outerPrimeZ,
            methodSymbol: outerPrimeZ.FindMethod("Method3")!,
            managedTypeName: "TestClasses.OuterPrime`1",
            managedMethodName: "Method3`1(!0,!!0)");
    }

    [TestMethod]
    public void Inheritance3()
    {
        var outerPrimeYz = _compilation.GetTypeByMetadataName("TestClasses.OuterPrime`2")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.OuterPrime<,>).GetMethod("Method3")!,
            containingTypeSymbol: outerPrimeYz,
            methodSymbol: outerPrimeYz.FindMethod("Method3")!,
            managedTypeName: "TestClasses.OuterPrime`2",
            managedMethodName: "Method3`1(!1,!!0)");
    }

    [TestMethod]
    public void Inheritance4()
    {
        var outerString = _compilation.GetTypeByMetadataName("TestClasses.OuterString")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.OuterString).GetMethod("Method3")!,
            containingTypeSymbol: outerString,
            methodSymbol: outerString.FindMethod("Method3")!,
            managedTypeName: "TestClasses.OuterString",
            managedMethodName: "Method3`1(System.String,!!0)");
    }

    [TestMethod]
    public void Overloads1()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0()")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 0),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0");
    }

    [TestMethod]
    public void Overloads2()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0(Int32)")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 0, @int),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0(System.Int32)");
    }

    [TestMethod]
    public void Overloads3()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0(Int32, TestClasses.Overloads)")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 0, @int, overloads),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0(System.Int32,TestClasses.Overloads)");
    }

    [TestMethod]
    public void Overloads4()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var intptr = _compilation.CreatePointerTypeSymbol(@int);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0(Int32*)")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 0, intptr),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0(System.Int32*)");
    }

    [TestMethod]
    public void Overloads5()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var dynamic = _compilation.DynamicType;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0(System.Object)")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 0, dynamic),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0(System.Object)");
    }

    [TestMethod]
    public void Overloads6()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U](U)")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1, 1, m => m.Parameters.Single().Type == m.TypeParameters.Single()),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1(!!0)");
    }

    [TestMethod]
    public void Overloads7()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U]()")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1");
    }

    [TestMethod]
    public void Overloads8()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U,T]()")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 2),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`2");
    }

    [TestMethod]
    public void Overloads9()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U](U[])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1, 1,
                m => m.Parameters.Single().Type is IArrayTypeSymbol arrayType &&
                     arrayType.Rank == 1 &&
                     arrayType.ElementType == m.TypeParameters.Single()),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1(!!0[])");
    }

    [TestMethod]
    public void Overloads10()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U](U[][])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1, 1,
                m => m.Parameters.Single().Type is IArrayTypeSymbol arrayType &&
                     arrayType.Rank == 1 &&
                     arrayType.ElementType is IArrayTypeSymbol innerArrayType &&
                     innerArrayType.Rank == 1 &&
                     innerArrayType.ElementType == m.TypeParameters.Single()),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1(!!0[][])");
    }

    [TestMethod]
    public void Overloads11()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U](U[,])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1, 1,
                m => m.Parameters.Single().Type is IArrayTypeSymbol arrayType &&
                     arrayType.Rank == 2 &&
                     arrayType.ElementType == m.TypeParameters.Single()),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1(!!0[,])");
    }

    [TestMethod]
    public void Overloads12()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U](U[,,])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1, 1,
                m => m.Parameters.Single().Type is IArrayTypeSymbol arrayType &&
                     arrayType.Rank == 3 &&
                     arrayType.ElementType == m.TypeParameters.Single()),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1(!!0[,,])");
    }

    [TestMethod]
    public void Overloads13()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var @int = _compilation.GetSpecialType(SpecialType.System_Int32);
        var listInt = _compilation.GetTypeByMetadataName("System.Collections.Generic.List`1")!.Construct(@int);

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U](System.Collections.Generic.List`1[System.Int32])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1, listInt),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1(System.Collections.Generic.List`1<System.Int32>)");
    }

    [TestMethod]
    public void Overloads14()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var list = _compilation.GetTypeByMetadataName("System.Collections.Generic.List`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U](System.Collections.Generic.List`1[U])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1, 1,
                m =>
                    m.Parameters.Single().Type is INamedTypeSymbol p &&
                    p.OriginalDefinition == list &&
                    p.TypeArguments.Single() == m.TypeParameters.Single()),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1(System.Collections.Generic.List`1<!!0>)");
    }

    [TestMethod]
    public void Overloads15()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var tuple2 = _compilation.GetTypeByMetadataName("System.Tuple`2")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U,V](System.Tuple`2[U,V], System.Tuple`2[V,U])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 2, 2,
                m =>
                    m.Parameters.First() is INamedTypeSymbol p1 &&
                    p1.OriginalDefinition == tuple2 &&
                    p1.TypeArguments.SequenceEqual(m.TypeParameters) &&
                    m.Parameters.Last() is INamedTypeSymbol p2 &&
                    p2.OriginalDefinition == tuple2 &&
                    p2.TypeArguments.SequenceEqual(m.TypeParameters.Reverse())),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`2(System.Tuple`2<!!0,!!1>,System.Tuple`2<!!1,!!0>)");
    }

    [TestMethod]
    public void Overloads16()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var tuple1 = _compilation.GetTypeByMetadataName("System.Tuple`1")!;
        var tuple2 = _compilation.GetTypeByMetadataName("System.Tuple`2")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0(System.Tuple`1[System.Tuple`2[System.String[,],System.Int32]])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 0, 1,
                m =>
                    m.Parameters.Single().Type is INamedTypeSymbol p &&
                    p.OriginalDefinition == tuple1 &&
                    p.TypeArguments.Single() is INamedTypeSymbol t &&
                    t.OriginalDefinition == tuple2),
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0(System.Tuple`1<System.Tuple`2<System.String[,],System.Int32>>)");
    }

    [TestMethod]
    public void Overloads17()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var tuple1 = _compilation.GetTypeByMetadataName("System.Tuple`1")!;
        var tuple2 = _compilation.GetTypeByMetadataName("System.Tuple`2")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0(System.Tuple`2[System.Tuple`1[System.String],System.Tuple`1[System.Int32]])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 0, 1,
                m =>
                    m.Parameters.Single().Type is INamedTypeSymbol p &&
                    p.OriginalDefinition == tuple2 &&
                    p.TypeArguments.All(t => t.OriginalDefinition == tuple1))!,
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0(System.Tuple`2<System.Tuple`1<System.String>,System.Tuple`1<System.Int32>>)");
    }

    [TestMethod]
    public void Overloads18()
    {
        var overloads = _compilation.GetTypeByMetadataName("TestClasses.Overloads")!;
        var tuple1 = _compilation.GetTypeByMetadataName("System.Tuple`1")!;

        VerifyRoundTrip(
            methodInfo: typeof(TestClasses.Overloads).FindMethod("Void Overload0[U](System.Tuple`1[System.Tuple`1[TestClasses.Outer`1+Inner`1[U,U]]])")!,
            containingTypeSymbol: overloads,
            methodSymbol: overloads.FindMethod("Overload0", 1, 1,
                m =>
                    m.Parameters.Single().Type is INamedTypeSymbol p &&
                    p.OriginalDefinition == tuple1 &&
                    p.TypeArguments.Single() is INamedTypeSymbol t &&
                    t.OriginalDefinition == tuple1)!,
            managedTypeName: "TestClasses.Overloads",
            managedMethodName: "Overload0`1(System.Tuple`1<System.Tuple`1<TestClasses.Outer`1+Inner`1<!!0,!!0>>>)");
    }

    #region Helpers
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Code using the parameters is commented out temporarly.")]
    private static void VerifyRoundTrip(
        MethodInfo methodInfo,
        INamedTypeSymbol containingTypeSymbol,
        IMethodSymbol methodSymbol,
        string managedTypeName,
        string managedMethodName)
    {
        VerifyRoundTripFromMethodInfo(methodInfo, managedTypeName, managedMethodName);
        VerifyRoundTripFromName(managedTypeName, managedMethodName, methodInfo);
        // TODO: Enable these checks and remove attributes on method
        // VerifyRoundTripFromMethodSymbol(containingTypeSymbol, methodSymbol, managedTypeName, managedMethodName);
        // VerifyRoundTripFromName(managedTypeName, managedMethodName, containingTypeSymbol, methodSymbol);
    }

    private static void VerifyRoundTripFromMethodInfo(
        MethodInfo methodInfo,
        string expectedManagedTypeName,
        string expectedManagedMethodName)
    {
        // Generate the fqn for the Reflection MethodInfo
        ManagedNameHelper.GetManagedName(methodInfo, out var managedTypeName, out var managedMethodName, out _);

        Assert.AreEqual(expectedManagedTypeName, managedTypeName);
        Assert.AreEqual(expectedManagedMethodName, managedMethodName);

        // Lookup the Reflection MethodInfo using fullTypeName and fullMethodName
        var roundTrippedMethodInfo = ManagedNameHelper.GetMethod(
            Assembly.GetExecutingAssembly(),
            managedTypeName,
            managedMethodName);

        Assert.AreEqual(methodInfo.MetadataToken, roundTrippedMethodInfo.MetadataToken);
    }

    private static void VerifyRoundTripFromName(
        string managedTypeName,
        string managedMethodName,
        MethodInfo expectedMethodInfo)
    {
        // Lookup the Reflection MethodInfo using fullTypeName and fullMethodName
        var methodInfo = ManagedNameHelper.GetMethod(
            Assembly.GetExecutingAssembly(),
            managedTypeName,
            managedMethodName);

        Assert.AreEqual(expectedMethodInfo.MetadataToken, methodInfo.MetadataToken);

        // Generate the fqn for the Reflection MethodInfo
        ManagedNameHelper.GetManagedName(
            methodInfo,
            out var roundTrippedFullTypeName,
            out var roundTrippedFullMethodName);

        Assert.AreEqual(managedTypeName, roundTrippedFullTypeName);
        Assert.AreEqual(managedMethodName, roundTrippedFullMethodName);
    }

    // private void VerifyRoundTripFromMethodSymbol(
    //     INamedTypeSymbol containingTypeSymbol,
    //     IMethodSymbol methodSymbol,
    //     string expectedFullTypeName,
    //     string expectedFullMethodName)
    // {
    //     // Generate the fqn for the Roslyn IMethodSymbol
    //     FullyQualifiedNameHelper.GetFullyQualifiedName(
    //         containingTypeSymbol,
    //         methodSymbol,
    //         out var fullTypeName,
    //         out var fullMethodName);

    //     Assert.AreEqual(expectedFullTypeName, fullTypeName);
    //     Assert.AreEqual(expectedFullMethodName, fullMethodName);

    //     // Lookup the Roslyn ITypeSymbol and IMethodSymbol using fullTypeName and fullMethodName
    //     var roundTrippedContainingTypeSymbol = _compilation.GetTypeByMetadataName(fullTypeName);

    //     Assert.AreEqual(containingTypeSymbol.OriginalDefinition, roundTrippedContainingTypeSymbol.OriginalDefinition);

    //     var roundTrippedMethodSymbol = FullyQualifiedNameHelper.GetMethodFromFullyQualifiedName(
    //         _compilation,
    //         fullTypeName,
    //         fullMethodName);

    //     Assert.AreEqual(methodSymbol.OriginalDefinition, roundTrippedMethodSymbol.OriginalDefinition);
    // }

    // private void VerifyRoundTripFromName(
    //     string fullTypeName,
    //     string fullMethodName,
    //     INamedTypeSymbol expectedContainingTypeSymbol,
    //     IMethodSymbol expectedMethodSymbol)
    // {
    //     // Lookup the Roslyn ITypeSymbol and IMethodSymbol using fullTypeName and fullMethodName
    //     var containingTypeSymbol = _compilation.GetTypeByMetadataName(fullTypeName);
    //
    //     Assert.AreEqual(expectedContainingTypeSymbol.OriginalDefinition, containingTypeSymbol.OriginalDefinition);
    //
    //     var methodSymbol = FullyQualifiedNameHelper.GetMethodFromFullyQualifiedName(
    //         _compilation,
    //         fullTypeName,
    //         fullMethodName);
    //
    //     Assert.AreEqual(expectedMethodSymbol.OriginalDefinition, methodSymbol.OriginalDefinition);
    //
    //     // Generate the fqn for the Roslyn IMethodSymbol
    //     FullyQualifiedNameHelper.GetFullyQualifiedName(
    //         containingTypeSymbol,
    //         methodSymbol,
    //         out var roundTrippedFullTypeName,
    //         out var roundTrippedFullMethodName);
    //
    //     Assert.AreEqual(fullTypeName, roundTrippedFullTypeName);
    //     Assert.AreEqual(fullMethodName, roundTrippedFullMethodName);
    // }
    #endregion
}
