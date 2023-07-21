# Coding guidelines

Let's face it. No matter what coding guidelines we choose, we're not going to make everyone happy.
In fact, some people out there might be downright angry at the choices we make.
But the fact of the matter is that there is no "one true bracing style," despite [attempts to name a bracing style as such](http://en.wikipedia.org/wiki/Indent_style#Variant:_1TBS).

While we would like to embrace everyone's individual style, working together on the same codebase would be utter chaos if we don't enforce some consistency. When it comes to coding guidelines, consistency can be even more important than being "right".

## Copyright header and license notice

All source code files (mostly `src/**/*.cs` and `test/**/*.cs`) require this exact header at the beginning of the file, without any modifications:

```cs
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
```

It is not mandatory to add the header to generated files, such as `*.designer.cs`.

Every repo also needs the Apache 2.0 License in a file called LICENSE.txt in the root of the repo.

## C# Coding Style

The NuGet.Client coding style is similar to the [.NET Framework Coding style](https://github.com/dotnet/runtime/blob/master/docs/coding-guidelines/coding-style.md) with some minor alterations that consider the history of the client repository.

For non code files (xml, etc), our current best guidance is consistency. When editing files, keep new code and changes consistent with the style in the files. For new files, it should conform to the style for that component. If there is a completely new component, anything that is reasonably broadly accepted is fine.

The general rule we follow is "use Visual Studio defaults".

1. We use [Allman style](http://en.wikipedia.org/wiki/Indent_style#Allman_style) braces, where each brace begins on a new line. One exception is that a `using` statement is permitted to be nested within another `using` statement by starting on the following line at the same indentation level, even if the nested `using` contains a controlled block.

1. We use four spaces of indentation (no tabs).

1. We use `_camelCase` for internal and private fields and use `readonly` where possible. Prefix internal and private instance fields with `_`. Static fields are all PascalCase regardless of visibility. When used on static fields, `readonly` should come after `static` (e.g. `static readonly` not `readonly static`).  Public fields should be used sparingly and should use PascalCasing with no prefix when used.

1. We avoid `this.` unless absolutely necessary.

1. We always specify the visibility, even if it's the default (e.g.
   `private string _foo` not `string _foo`). Visibility should be the first modifier (e.g.
   `public abstract` not `abstract public`).

1. Namespace imports should be specified at the top of the file, *outside* of
   `namespace` declarations, and should be sorted alphabetically, with the exception of `System.*` namespaces, which are to be placed on top of all others. Always remove unnecessary imports.

1. Avoid more than one empty line at any time. For example, do not have two blank lines between members of a type.

1. Avoid spurious free spaces. For example avoid `if (someVar == 0)...`, where the dots mark the spurious free spaces. Consider enabling "View White Space (Ctrl+R, Ctrl+W)" or "Edit -> Advanced -> View White Space" if using Visual Studio to aid detection.

1. If a file happens to differ in style from these guidelines (e.g. private members are named `m_member` rather than `_member`), the existing style in that file takes precedence. Changes/refactorings are possible, but depending on the complexity, change frequency of the file, might need to be considered on their own merits in a separate pull request.

1. We only use `var` when it's obvious what the variable type is.
For example the following are correct:

    ```cs
    var fruit = "Apple";
    var fruits = new List<Fruit>();
    string fruit = null; // can't use "var" because the type isn't known (though you could do (string)null, don't!)
    const string expectedName = "name"; // can't use "var" with const
    FruitFlavor flavor = fruit.GetFlavor();
    var nugetVersion = NuGetVersion.Parse("1.0.0");
    ```

    The following are incorrect:

    ```cs
    var flavor = fruit.GetFlavor();
    string fruit = "Apple";
    List<Fruit> fruits = new List<Fruit>();
    ```

1. We use language keywords instead of BCL types (e.g. `int, string, float` instead of `Int32, String, Single`, etc) for both type references as well as method calls (e.g. `int.Parse` instead of `Int32.Parse`).
    The following are correct:

    ```cs
    public string TrimString(string s)
    {
        return string.IsNullOrEmpty(s)
            ? null
            : s.Trim();
    }

    var intTypeName = nameof(Int32); // can't use C# type keywords with nameof
    ```

    The following are incorrect:

    ```cs
    public String TrimString(String s)
    {
        return String.IsNullOrEmpty(s)
            ? null
            : s.Trim();
    }
    ```

1. We use PascalCasing to name all our constant local variables and fields. The only exception is for interop code where the constant value should exactly match the name and value of the code you are calling via interop.

1. We use ```nameof(...)``` instead of ```"..."``` whenever possible and relevant.

1. When including non-ASCII characters in the source code use Unicode escape sequences (\uXXXX) instead of literal characters. Literal non-ASCII characters occasionally get garbled by a tool or editor.

1. When using a single-statement if, we follow these conventions:
    - Never use single-line form (for example: `if (source == null) throw new ArgumentNullException("source");`)
    - Using braces is always accepted, and required if any block of an `if`/`else if`/.../`else` compound statement uses braces or if a single statement body spans multiple lines.
    - Braces may be omitted only if the body of *every* block associated with an `if`/`else if`/.../`else` compound statement is placed on a single line.

1. Do *not* use regions.

1. Do sort the members in classes in the following order: static members, fields, constructors, events, properties then methods.

1. Do add a new line at the end of all files.

1. Do prefer modern language features when available, such as object initializers, collection initializers, coalescing expressions, etc.

    These are incorrect:

    ```cs
    var packages = new List<PackageIdentity>;

    var packageIdentity = new PackageIdentity();
    packageIdentity.Id = "NuGet.Commands";
    packageIdentity.Version = "5.4.0";

    var idList = new List<string>();
    var id1 = "Id1"
    var id2 = "Id2";
    idList.Add(id1);
    idList.Add(id2);
    ```

    These are correct:

    ```cs
    var packageIdentity = new PackageIdentity();
    {
        Id = "NuGet.Commands";
        Version = "5.4.0";
    }

    var idList = new List<string>
    {
        "Id1",
        "Id2",
    };
    ```

1. All async methods should have a name ending with Async.

1. All interfaces must be pascal cased prefixed with I.

1. Adding TODOs is not recommended and should be avoided whenever possible. In cases in which they do get added each TODO needs to be linked to an issue. Use the full url to the GitHub issue.

1. Do not having trailing whitespace at the end of lines.

1. When using `this` or `base` to call other constructors, put the colon and call to the constructor on a new line.

    This is correct:
    ```cs
    public MyClass(string arg)
      : this(arg, arg2: false)
    {
       ...
    }
    ```

    This is incorrect:
    ```cs
    public MyClass(string arg) : this(arg, arg2: false)
    {
      ...
    }
    ```

1. Do not use `Func<>` or `Action<>` as return types. Instead, let the compiler convert the method into a func or action as necessary.

    This is correct:
    ```cs
    Func<string, int> factory = DoSomething;

    int DoSomething(string input)
    {
        // method body
    }
    ```

    This is incorrect:
    ```cs
    Func<string, int> factory = GetFactory();

    Func<string, int> GetFactory()
    {
        return (string input) => // method body
    }
    ```

1. Prefer a using directive over fully qualifying a type.

    This is correct:
    ```cs
    using System.Threading.Tasks;

    Task DoSomethingAsync()
    ```

    This is incorrect:
    ```cs
    using System;

    System.Threading.Tasks.Task DoSomethingAsync()
    ```

1. All the parameter names in methods and constructors should be `camelCase`.

1. The namespace of a new class would normally match the assembly name, or a more qualfied version of it. Exceptions may apply. For example if the namespace is preserved for type forwarding purposes. For a class in `NuGet.Packaging`:

    These are correct:

    ```cs
    namespace NuGet.Packaging {
        public class SpecialSigningUtility
        {
        }
    }
    ...
    namespace NuGet.Packaging.Signing {
        public class SpecialSigningUtility
        {
        }
    }
    ```

    This is incorrect:

    ```cs
    namespace NuGet.Common {
        public class SpecialSigningUtility
        {
        }
    }
    ```

    Exceptions are allowed when multiple type names coming from different namespaces are available.

1. Do not use `as` to cast types

   These are correct:

   ```cs
   ListItem item = (ListItem)selectedItem; // InvalidCastException if wrong type
   string name = item.Name;
   ```

   ```cs
   if (obj is Type1 t1)
   {
       return t1.Value;
   }
   else if (obj is Type2 t2)
   {
       return t2.Value;
   }
   else
   {
        throw new InvalidOperationException($"Unexpected type {sender.GetType().Name}");
   }
   ```

   This is incorrect:

   ```cs
   ListItem item = selectedItem as ListItem;
   string name = item.Name; // NullReferenceException if selectedItem was not ListItem
   ```

Many of the guidelines, wherever possible, and potentially some not listed here, are enforced by an [EditorConfig](https://editorconfig.org "EditorConfig homepage") file (`.editorconfig`) at the root of the repository.

### When to use internals vs. public and when to use InternalsVisibleTo

Usage of internal types and members is allowed. Do consider whether external customers could benefit from having said internal class public.

In .NET public types are a big commitment so be aware that any time you add a public method/class that is an API we are willing to maintain.

We should keep our public API surface area reasonably small.

`InternalsVisibleTo` is used only to allow a unit test to test internal types and members of its runtime assembly. We do not use `InternalsVisibleTo` between two runtime assemblies.

If two runtime assemblies need to share common helpers then we use shared compilation.

If two runtime assemblies need to call each other's APIs, the APIs should be public. If we need it, it is likely that our customers need it.
Do consider the scope of the assembly when adding new types, ask yourself whether that assembly should be doing *the work* in question.

### Argument null checking

Null checking is required for parameters that cannot be null (big surprise!). All of our null checks are within the method body.
In constructors where a field or property is initialized coalescing checks are allowed. Otherwise use the regular bracing approach.

```cs

    Path = path ?? throw new ArgumentNullException(nameof(path));
    ....
    if (item == null)
    {
        throw new ArgumentNullException(nameof(item));
    }

```

### Optional parameters

Optional parameters are allowed in certain situations, but discouraged.
Optional parameters are at the root for many versioning issues.
The optional parameter default values are a compile time decision, that's why adding a new optional parameter to an API is a breaking change!
Furthermore, if you have a method with optional parameters, adding an overload with additional optional parameters might cause a compile-time breaking change.
Optional parameters in internal/private methods are acceptable.

### Async method patterns

By default all async methods must have the `Async` suffix. There are some exceptional circumstances where a method name from a previous framework will be grandfathered in.

Passing cancellation tokens can be passed an optional parameter with a value of `default(CancellationToken)`, which is equivalent to `CancellationToken.None`.
Prefer not having optional parameters.

Sample async method:

```cs
public Task GetDataAsync(
    QueryParams query,
    int maxData,
    CancellationToken cancellationToken)
{
    ...
}
```

### Extension method patterns

The general rule is: if a regular static method would suffice, avoid extension methods.

Extension methods are often useful to create chainable method calls, for example, when constructing complex objects, or creating queries.
Internal extension methods are allowed, but bear in mind the previous guideline: ask yourself if an extension method is truly the most appropriate pattern.
The namespace of the extension method class should generally be the namespace that represents the functionality of the extension method, as opposed to the namespace of the target type.
The class name of an extension method container (also known as a "sponsor type") should generally follow the pattern of `<Feature>Extensions`, `<Target><Feature>Extensions`, or `<Feature><Target>Extensions`. For example:

```cs
namespace Food
{
    class Fruit { ... }
}
namespace Fruit.Eating
{
    class FruitExtensions { public static void Eat(this Fruit fruit); }
  OR
    class FruitEatingExtensions { public static void Eat(this Fruit fruit); }
  OR
    class EatingFruitExtensions { public static void Eat(this Fruit fruit); }
}
```

When writing extension methods for an interface the sponsor type name must not start with an `I`.

### Code comments

The person writing the code will write the doc comments. Public APIs only. No need for doc comments on non-public types.

Note: Public means callable by a customer, so it includes protected APIs. However, some public APIs might still be "for internal use only" but need to be public for technical reasons. We will still have doc comments for these APIs but they will be documented as appropriate.

- Do not include empty XML comments.
  - summary element must not be empty.
  - param element must not be empty.

Correct:

```cs
        /// <summary>
        /// Calculates all fruit types
        /// </summary>
        /// <returns>A set of fruit types contained in this basket.</returns>
        public ISet<FruitType> GetAllFruitTypes()

        /// <summary>
        /// Determines whether the fruit has seeds.
        /// </summary>
        /// <param name="fruitType">A fruit type</param>
        /// <returns> Whether the fruit has seeds. </returns>
        public bool HasSeeds(FruitType fruitType)
```

Incorrect:

```cs
        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public ISet<FruitType> GetAllFruitTypes()

        /// <summary>
        /// Determines whether the fruit has seeds.
        /// </summary>
        /// <param name="fruitType"></param>
        /// <returns> </returns>
        public bool HasSeeds(FruitType fruitType)
```

### Assertions

Do not use `Debug.Assert()`. That's what unit tests are for.
Consider using `Assumes.Present()`, specifically in Visual Studio code when interacting with the service providers.

### Unit tests and functional tests

#### Assembly naming

The unit tests for the `NuGet.Fruit` assembly live in the `NuGet.Fruit.Tests` assembly.

The functional tests for the `NuGet.Fruit` assembly live in the `NuGet.Fruit.FunctionalTests` assembly.

In general there should be exactly one unit test assembly for each product runtime assembly. In general there should be one functional test assembly per product (NuGet.exe/MSBuild.exe/dotnet.exe). Exceptions can be made for both. Some are already grandfathered in.

#### Unit test class naming

Test class names end with `Test` and live in a similar namespace as the class being tested. For example, the unit tests for the `NuGet.Fruit.Banana` class would be in a `NuGet.Fruit.Test.BananaTest` class in the test assembly.

#### Unit test method naming

Unit test method names must be descriptive about *what is being tested*, *under what conditions*, and *what the expectations are*.

All *new* test cases should follow this convention:
Pascal casing and underscores should be used to improve readability.

> Format: `<what is being tested>_<under what conditions>_<with what expectations>`

The following test names are correct:

```cs
PublicApi_Arguments_ShouldNotBeNull
MsbuildRestore_WithRelativeSource_ResolvesAgainstCurrentWorkingDirectory
```

The following test names are incorrect:

```cs
PublicApiArgumentsShouldNotBeNull
Test1
Constructor
FormatString
GetData
```

#### Unit test structure

The contents of every unit test should be split into three distinct stages, optionally separated by these comments:

```cs
// Arrange
// Act
// Assert
```

The crucial thing here is that the `Act` stage is exactly one statement. That one statement is nothing more than a call to the one method that you are trying to test. Keeping that one statement as simple as possible is also very important. For example, this is not ideal:

```cs
int result = myObj.CallSomeMethod(GetComplexParam1(), GetComplexParam2(), GetComplexParam3());
```

This style is not recommended because way too many things can go wrong in this one statement. All the `GetComplexParamN()` calls can throw for a variety of reasons unrelated to the test itself. It is thus unclear to someone running into a problem why the failure occurred.

The ideal pattern is to move the complex parameter building into the `Arrange` section:

```cs
// Arrange
P1 p1 = GetComplexParam1();
P2 p2 = GetComplexParam2();
P3 p3 = GetComplexParam3();

// Act
int result = myObj.CallSomeMethod(p1, p2, p3);

// Assert
Assert.AreEqual(1234, result);
```

Now the only reason the line with `CallSomeMethod()` can fail is if the method itself blew up. This is especially important when you're using helpers such as `ExceptionHelper`, where the delegate you pass into it must fail for exactly one reason.

### Testing exception messages

In general testing the specific exception message in a unit test is important. This ensures that the exact desired exception is what is being tested rather than a different exception of the same type. In order to verify the exact exception it is important to verify the message.

To make writing unit tests easier it is recommended to compare the error message to the resx resource. However, comparing against a string literal is also permitted.

```cs
var ex = Assert.Throws<InvalidOperationException>(
    () => fruitBasket.GetBananaById(1234));
Assert.Equal(
    Strings.FormatInvalidBananaID(1234),
    ex.Message);
```

#### Test assertions

Both xunit.net and FluentAssertions are allowed. FluentAssertions do not truncate the equality messages, thus sometimes making it easier to diagnose the failure.
Both of these will make the tests a lot more readable and also allow the test runner report the best possible errors. For example, these are bad:

```cs
Assert.Equal(true, someBool);

Assert.True("abc123" == someString);

Assert.True(list1.Length == list2.Length);

for (int i = 0; i < list1.Length; i++) {
    Assert.True(
        String.Equals
            list1[i],
            list2[i],
            StringComparison.OrdinalIgnoreCase));
}
```

These are good:

```cs
Assert.True(someBool);

Assert.Equal("abc123", someString);

// built-in collection assertions!
Assert.Equal(list1, list2, StringComparer.OrdinalIgnoreCase);
```

Some places where FluentAssetion shine are:

```cs

RestoreResult restoreResult = await RestoreRunner.RestoreAsync(restoreRequest);

// xunit assertions
Assert.True(restoreResult.Success);

// fluent assertions

restoreResult.Success.Should().BeTrue(because: restoreResult.AllOutput);
```

#### Parallel tests

By default all unit test assemblies should run in parallel mode, which is the default. Unit tests shouldn't depend on any shared state, and so should generally be runnable in parallel. If the tests fail in parallel, the first thing to do is to figure out *why*; do not just disable parallel tests!

For functional tests it is reasonable to disable parallel tests.

### Use only complete words or common/standard abbreviations in public APIs

Public namespaces, type names, member names, and parameter names must use complete words or common/standard abbreviations.

These are correct:

```cs
public void AddReference(AssemblyReference reference);
public EcmaScriptObject SomeObject { get; }
```

These are incorrect:

```cs
public void AddRef(AssemblyReference ref);
public EcmaScriptObject SomeObj { get; }
```

### GitHub Flavored Markdown

GitHub supports Markdown in many places throughout the system (issues, comments, etc.). However, there are a few differences from regular Markdown that are described [here](https://help.github.com/articles/github-flavored-markdown).
