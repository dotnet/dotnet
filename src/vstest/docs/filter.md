# TestCase filter

This document will help you to selectively execute tests based on filtering conditions through `--filter` for `dotnet test` and `--testcasefilter` for `vstest.console.exe`.

### Syntax

   `dotnet test --filter <Expression>` or
   `vstest.console.exe --testcasefilter:<Expression>`

**Expression** is in the format __\<property>\<operator>\<value>[|&\<Expression>]__. Expressions can be
enclosed in paranthesis. e.g. `(Name~MyClass) | (Name~MyClass2)`.

> `vstest 15.1+`: An expression without any **operator** is automatically considered as a `contains` on `FullyQualifiedName` property.
E.g. `dotnet test --filter xyz` is same as `dotnet test --filter FullyQualifiedName~xyz`.

**Property** is an attribute of the `Test Case`. For example, the following are the properties
supported by popular unit test frameworks.

| Test Framework | Supported properties |
| -------------- | -------------------- |
| MSTest         | <ul><li>FullyQualifiedName</li><li>Name</li><li>ClassName</li><li>Priority</li><li>TestCategory</li></ul> |
| Xunit          | <ul><li>FullyQualifiedName</li><li>DisplayName</li><li>Traits</li></ul> |
| NUnit          | <ul><li>FullyQualifiedName</li><li>Name</li><li>Priority</li><li>TestCategory</li><li>Category</li><li>Property</li></ul>|

Allowed **operators**:

* `=` implies an exact match
* `!=` implies an exact not match
* `~` implies a contains lookup
* `!~` implies a not contains lookup

**Value** is a string. All the lookups are case insensitive.

**Escape Sequences** must be used to represent characters in the value that have special meanings in the filter, i.e. filter operators.

| Escape Sequence | Represents |
| -------------- | -------------------- |
| \\\\            |  \\                   |
| \\(             |  (                   |
| \\)             |  )                   |
| \\&             |  &                   |
| \\\|             |  \|                   |
| \\=             |  =                   |
| \\!             |  !                   |
| \\~             |  ~                   |

A helper method `Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities.FilterHelper.Escape`
is also available by referencing the `Microsoft.VisualStudio.TestPlatform.ObjectModel` NuGet package, which can be used to escape strings programatically.

Expressions can be joined with boolean operators. The following boolean operators are supported:

* `|` implies a boolean `OR`
* `&` implies a boolean `AND`

## Examples

The following examples use `dotnet test`, if you're using `vstest.console.exe` replace `--filter` with `--testcasefilter:`.

### MSTest

```CSharp
namespace MSTestNamespace
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UnitTestClass1
    {
        [Priority(2)]
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestCategory("CategoryA")]
        [Priority(3)]
        [TestMethod]
        public void TestMethod2()
        {
        }
    }
}
```

| Expression | What it does? |
| ---------- | ------------- |
| `dotnet test --filter Method` | Runs tests whose `FullyQualifiedName` contains `Method`. Available in `vstest 15.1+`. |
| `dotnet test --filter Name~TestMethod1` | Runs tests whose name contains `TestMethod1`. |
| `dotnet test --filter ClassName=MSTestNamespace.UnitTestClass1` | Runs tests which are in the class  `MSTestNamespace.UnitTestClass1`. <br/>**Note:** The ClassName value should have a namespace, ClassName=UnitTestClass1 won't work. |
| `dotnet test --filter FullyQualifiedName!=MSTestNamespace.UnitTestClass1.TestMethod1` | Runs all tests except `MSTestNamespace.UnitTestClass1.TestMethod1`. |
| `dotnet test --filter TestCategory=CategoryA` | Runs tests which are annotated with `[TestCategory("CategoryA")]` |
| `dotnet test --filter Priority=3` | Runs tests which are annotated with `[Priority(3)]`.**Note:** `Priority~3` is invalid as Priority is an int not a string. |

#### Using Logical operators `| and &`

| Expression | What it does? |
| ---------- | ------------- |
| `dotnet test --filter "FullyQualifiedName~UnitTestClass1\|TestCategory=CategoryA"` | Runs tests which have `UnitTestClass1` in FullyQualifiedName **or** TestCategory is CategoryA. |
| `dotnet test --filter "FullyQualifiedName~UnitTestClass1&TestCategory=CategoryA"` | Runs tests which have `UnitTestClass1` in FullyQualifiedName **and** TestCategory is CategoryA. |
| `dotnet test --filter "(FullyQualifiedName~UnitTestClass1&TestCategory=CategoryA)\|Priority=1"` | Runs tests which have either FullyQualifiedName contains `UnitTestClass1` and TestCategory is CategoryA or Priority is 1. |

### xUnit

| Expression | What it does? |
| ---------- | ------------- |
| `dotnet test --filter DisplayName=XUnitNamespace.TestClass1.Test1` | Runs only one test `XUnitNamespace.TestClass1.Test1`. |
| `dotnet test --filter FullyQualifiedName!=XUnitNamespace.TestClass1.Test1` | Runs all tests except `XUnitNamespace.TestClass1.Test1` |
| `dotnet test --filter DisplayName~TestClass1` | Runs tests whose display name contains `TestClass1`. |

#### Using traits for filter

```CSharp
namespace XUnitNamespace
{
    public class TestClass1
    {
        [Trait("Category", "bvt")]
        [Trait("Priority", "1")]
        [Fact]
        public void foo()
        {
        }

        [Trait("Category", "Nightly")]
        [Trait("Priority", "2")]
        [Fact]
        public void bar()
        {
        }
    }
}

```

In above code we defined traits with keys `Category` and `Priority` which can be used for filtering.

| Expression | What it does? |
| ---------- | ------------- |
| `dotnet test --filter XUnit` | Runs tests whose `FullyQualifiedName` contains `XUnit`.  Available in `vstest 15.1+`. |
| `dotnet test --filter Category=bvt` | Runs tests which have `[Trait("Category", "bvt")]`. |

#### Using Logical operators `| and &`

| Expression | What it does? |
| ---------- | ------------- |
| `dotnet test --filter "FullyQualifiedName~TestClass1\|Category=Nightly"` | Runs tests which have `TestClass1` in FullyQualifiedName **or** Category is Nightly. |
| `dotnet test --filter "FullyQualifiedName~TestClass1&Category=Nightly"` | Runs tests which have `TestClass1` in FullyQualifiedName **and** Category is Nightly. |
| `dotnet test --filter "(FullyQualifiedName~TestClass1&Category=Nightly)\|Priority=1"` | Runs tests which have either FullyQualifiedName contains `TestClass1` and Category is CategoryA or Priority is 1. |

### NUnit

```csharp
namespace NUnitTestNamespace;

public class TestClass
{
    [Property("Priority","1")]
    [Test]
    public void Test1()
    {
        Assert.Pass();
    }

    [Property("Whatever", "SomeValue")]
    [Test]
    public void Test2()
    {
        Assert.Pass();
    }

    [Category("SomeCategory")]
    [Test]
    public void Test3()
    {
        Assert.Pass();
    }
}
```

#### Usage of the filters

| Expression | What it does? |
| ---------- | ------------- |
| `dotnet test --filter FullyQualifiedName=NUnitTestNamespace.TestClass.Test1` | Runs only the given test |
| `dotnet test --filter Name=Test1` | Runs all tests whose test name (method) equals `Test1`. |
| `dotnet test --filter Name=TestClass` | Runs tests within all classes named `TestClass`. |
| `dotnet test --filter Name=NUnitTestNamespace` | Runs all tests within the namespace `NUnitTestNamespace`. |
| `dotnet test --filter Priority=1` | Runs tests with property named Priority and value = 1`. |
| `dotnet test --filter Whatever=TestClass` | Runs tests with property named `Whatever` and value = `SomeValue`. |
| `dotnet test --filter Category=SomeCategory` | Runs tests with category set to `SomeCategory`.  Note: You can also use TestCategory in the filter. |

Logical operators works the same as for the other frameworks.


