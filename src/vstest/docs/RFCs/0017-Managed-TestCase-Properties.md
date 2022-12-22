

# 0017 Properties for TestCases in Managed Code

## Summary
This document standardizes additional properties on TestCase to support test cases written in managed languages.

## Overview
There are a broad variety of choices that current test adapter implementers have made when setting the `FullyQualifiedName` on TestCase objects created during test discovery. `FullyQualifiedName` typically identifies the type and method that implements a test case in managed code, but because there has been no standardization, VS cannot rely on the format of the `FullyQualifiedName`. We've considered standardizing the format of `FullyQualifiedName`, but this presents migration & compatibility costs that outweigh the benefits. Instead, we are adding two new properties to TestCase that represent the type (`ManagedType`) and method (`ManagedMethod`) for each test case. These additional properties are only applicable to unit tests written in managed languages (e.g. C# & VB, et al.).

## Specification

TestCases for managed code must include a string-valued property named `ManagedType` property and a string-valued property named `ManagedMethod`. The specification below outlines the requirements for the contents of these two properties.

### `ManagedType` Property

The `ManagedType` test case property represents the fully specified type name in metadata format:

    NamespaceA.NamespaceB.ClassName`1+InnerClass`2

* The type name must be fully qualified (in the CLR sense), including its namespace. Any generic classes must also include an arity value using backtick notation (`# where # is the number of type arguments that the class requires).
* Nested classes are appended with a '+' and must also include an arity if generic.
* `ManagedType` name must be escaped if it doesn't conform to identifier naming rules.

### `ManagedMethod` Property

The `ManagedMethod` test case property is the fully specified method including the method name and a list of its parameter types inside parentheses separated by commas.

    MethodName`2(ParamTypeA,ParamTypeB,…)

* If the method accepts no parameters, then the parentheses should be omitted.
* If the method is generic, then an arity must be specified (in the same way as the type property).
* The list of parameter types must be encoded using the type specification below.
* Return types are not encoded.
* `ManagedMethod` must be escaped if it doesn't conform to identifier naming rules.

### Parameter Type Encoding

Parameters are encoded as a comma-separated list of strings in parentheses at the end of the `ManagedMethod` property. Each parameter is encoded using the rules below.

* Basic Types - Types should be written using their namespace and type name with no extra whitespace. For example (`NamespaceA.NamespaceB.Class`). Native types should be written using their CLR type names (`System.Int32`,`System.String`).
* Array Types - Arrays should be encoded as the element type followed by square brackets (`[]`). Multidimensional arrays are indicated with commas inside the square brackets. There should be one less comma than the number of dimensions (i.e. 2 dimensional array is encoded as `[,]`, 3 dimensional array as `[,,]` ).
* Generic Types - Generic types should be encoded as the type name, followed by comma-separated type arguments in angle brackets (`<>`).
* Generic Parameters - Parameters that are typed by a generic argument on the containing type are encoded with an exclamation point (`!`) followed by the ordinal index of the parameter in the generic argument list.
* Generic Method Parameters - Parameters that are typed by a generic argument on the method are encoded with a double exclamation point (`!!`) followed by the ordinal index of the parameter in the method's generic argument list.
* Pointer Types - Pointer types should be encoded as the type name, followed by an asterisk (`*`).
* Dynamic Types - Dynamic types should be represented as System.Object.

#### Examples

```csharp
// Custom Types
Method(NamespaceA.NamespaceB.Class)
'𝐌𝐲 𝗮𝘄𝗲𝘀𝗼𝗺𝗲 method w\\𝘢𝘯 𝒊𝒏𝒂𝒄𝒄𝒆𝒔𝒔𝒊𝒃𝒍𝒆 𝙣𝙖𝙢𝙚 🤦‍♂️'(NamespaceA.NamespaceB.Class)

// Native Types
Method(System.String,System.Int32)

// Array Types
Method(System.String[])

// Generic Types
Method(System.Collections.Generic.List`1<System.String>)
'Method Name'(System.Collections.Generic.List`1<System.String>) // Method name contains a space

// Generic Type Parameters
Method(!0)

// Generic Method Parameters
Method(!!0)

// Generic Type with a Generic Type Parameter
Method(System.Collections.Generic.List`1<!0>)
```

### Special Methods

The CLR has some features that are implemented by way of special methods. While these methods are unlikely to be used to represent tests, the list below indicates how they should be encoded, if necessary.

* Explicit Interface Implementation - methods that explicitly implement an interface should be prefixed with the interface typename. For example the method name for the explicit implementation of IEnumerable<T>.GetEnumerator would look like this; `System.Collections.Generic.IEnumerable<T>.GetEnumerator`. Note that this is only the `ManagedMethod` property. The `ManagedType` property would still include the namespace of the class on which this method is declared.
* Constructors - constructors should be referenced using the method name `.ctor`
* Operators - operators are a language specific feature, and are translated into methods using compiler-specific rules. Use the underlying compiler-generated method name to reference the operator. For instance, in C#, `operator+` would be represented by a method named `op_Addition`.
* Finalizers - finalizers should be referenced using the name `Finalize`

### Nested Generic Type Parameter Numbering

If a generic type is nested in another generic type, then the generic arguments of the containing type are also reflected in the nested type, even if it's not reflected in the language syntax. This means that the numbering of generic parameters take into account the generic parameters of the containing type as if they are declared on the local type.

```csharp
// type A has arity 1, and one generic argument T
// ManagedType = "A`1"
public class A<T>
{
    // type B has arity 1, but two generic arguments T and X
    // ManagedType = "A`1+B`1"
    public class B<X> {

        // ManagedMethod = "Method(!0, !1)"
        public void Method(T t, X x) {}

        // ManagedMethod = "Method(!1)"
        public void Method(X x) {}

        // ManagedMethod = "Method(!0, !1, !!0)"
        public void Method<U>(T t, X x, U u) {}
    }
}
```

## Syntactic vs Semantic Test Location

We are defining syntactic location as the location in the code/syntax where the method is defined that implements the test. On the other hand, semantic location is the location in the type hierarchy at which point the method becomes recognized as a valid test.

There can be a situation where a test is declared on an abstract base type (syntactic), but the test is only discovered in a derived class (semantic). The question is which type should the `ManagedType` property point to? The base type, where the test code is located? Or the derived type, where the test is discovered? We will use the semantic location for the type because that is where the test adapter will find the test and it is where the test class would be instantiated. Also, there is a one to many relationship between the two. There can be many semantic test locations that each are implemented by the same base class method. If we were to standardize on using the syntactic location, we would risk having name collisions or fail to find the correct test case since there are multiple possible candidates.

## Managed Tests that are Not Type/Method Based

It is possible to create unit tests in managed code where tests are not necessarily based on types & methods. While this is somewhat uncommon, we need to be able to handle this situation gracefully. If a test adapter cannot provide a mapping between a testcase and a managed type and method, then it should not provide those properties. When `ManagedType` and `ManagedMethod` properties are not provided, the end-user may lose access to some features in the Test Explorer such as fast test execution by name, or source linking.

## Tests Returning Multiple Results

There are some situations where the number of test cases cannot be determined at discovery time. In these cases, when the tests are executed multiple results are returned for a single discovered test case.

## Uniqueness

The combination of `ManagedType` and `ManagedMethod` will be unique to a particular method within an assembly, but are not unique to a test case. This is due to the fact that when tests are data-driven, there can be many test cases that are executed by the same method. The test case ID is expected to be unique for every test case within an assembly. The test case ID should also be deterministic with respect to the method and its arguments. In other words, the test ID should not change given a particular method and a set of argument values (if any).

## Identifier Naming Rules
1. The first character cannot be a number (A number is a `char` belong  to Unicode category `Nd`). 
2. It can contain letters or digits. (Unicode categories: `Lu`, `Ll`, `Lt`, `Lm`, `Lo`, `Nl` and `Nd`.)
3. It can contain formatting characters. (Unicode categories: `Mn`, `Mc`, `Pc`, and `Cf`.)

## Escaping
If an identifier does not conform to identifier naming rules, it gets escaped. An escaped identifier always start and end with a `'`. 
```console
         Type: CleanNamespaceName.SecondLevel.𝐌𝐲 𝘤𝘭𝘢𝘴𝘴 with 𝘢𝘯 𝒊𝒏𝒂𝒄𝒄𝒆𝒔𝒔𝒊𝒃𝒍𝒆 𝙣𝙖𝙢𝙚 🤷‍♀️
       Method: int Sum(int x, int y)
  Parsed Type: CleanNamespaceName.SecondLevel.'𝐌𝐲 𝘤𝘭𝘢𝘴𝘴 with 𝘢𝘯 𝒊𝒏𝒂𝒄𝒄𝒆𝒔𝒔𝒊𝒃𝒍𝒆 𝙣𝙖𝙢𝙚 🤷‍♀️'
Parsed Method: Sum(System.Int32,System.Int32)

         Type: CleanNamespaceName.SecondLevel.Deeply wrong .namespace name.NamespaceA.Class1
       Method: int Method with . in it(int x, int y)
  Parsed Type: CleanNamespaceName.SecondLevel.'Deeply wrong '.'namespace name'.NamespaceA.Class1
Parsed Method: 'Method with . in it'(System.Int32,System.Int32)
```

If a character is `'` or `\` it gets escaped to `\'` or `\\` consecutively.
```console
         Type: CleanNamespaceName.ClassName\Continues
       Method: void MethodName(int x, int y)
  Parsed Type: CleanNamespaceName.'ClassName\\Continues'
Parsed Method: MethodName(System.Int32,System.Int32)
```

If an identifier is ending with arity, but that arity is not correct it will be escaped.
```console
         Type: CleanNamespaceName.ClassName
       Method: void MethodName`1(int x, int y)
  Parsed Type: CleanNamespaceName.ClassName
Parsed Method: 'MethodName`1'(System.Int32,System.Int32)

         Type: CleanNamespaceName.ClassName
       Method: void MethodName<T>(int x, int y)
  Parsed Type: CleanNamespaceName.ClassName
Parsed Method: MethodName`1(System.Int32,System.Int32)
```
