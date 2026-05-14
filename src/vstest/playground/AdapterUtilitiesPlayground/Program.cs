// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;
using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities.UnitTests;

namespace AdapterUtilitiesPlayground;

internal class Program
{
    private const BindingFlags PrivateBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    private static readonly Compilation _compilation = CSharpCompilation.Create(
            "Test.dll",
            new[] { CSharpSyntaxTree.ParseText(File.ReadAllText("TestClasses.cs")) },
            new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) });

    static void Main(string[] args)
    {
        var derivedClass = typeof(TestClasses.DerivedClass);
        var baseClass = typeof(TestClasses.BaseClass);

        var derivedMethods = derivedClass.GetMethods(PrivateBindingFlags).ToArray();
        var baseMethods = baseClass.GetMethods(PrivateBindingFlags).ToArray();
        var derivedMethod0 = derivedMethods.Single(i => i.Name == "Method0" && i.DeclaringType == derivedClass);
        var derivedbaseMethod0 = derivedMethods.Single(i => i.Name == "Method0" && i.DeclaringType == baseClass);
        var baseMethod0 = baseMethods.Single(i => i.Name == "Method0" && i.DeclaringType == baseClass);

        // {
        //     ManagedNameHelper.GetManagedName(derivedMethod0, out var managedType, out var managedMethod, out var hierarchies);
        //     var methodBase = ManagedNameHelper.GetMethod(derivedClass.Assembly, managedType, managedMethod);
        // }
        // 
        // {
        //     ManagedNameHelper.GetManagedName(derivedbaseMethod0, out var managedType, out var managedMethod, out var hierarchies);
        //     var methodBase = ManagedNameHelper.GetMethod(derivedClass.Assembly, managedType, managedMethod);
        // }

        //{
        //    ManagedNameHelper.GetManagedName(baseMethod0, out var managedType, out var managedMethod, out var hierarchies);
        //    var methodBase = ManagedNameHelper.GetMethod(derivedClass.Assembly, managedType, managedMethod);
        //}

        {
            var method = typeof(TestClasses.IImplementation<string>).GetMethods(PrivateBindingFlags).SingleOrDefault(i => i.Name == "ImplMethod2")!;
            method = method.MakeGenericMethod(typeof(int));

            ManagedNameHelper.GetManagedName(method, out var managedType, out var managedMethod, out var hierarchies);
            var methodBase = ManagedNameHelper.GetMethod(derivedClass.Assembly, managedType, managedMethod);
        }
    }
}
