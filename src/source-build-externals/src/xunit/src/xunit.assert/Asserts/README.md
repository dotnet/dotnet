# About This Project

This project contains the xUnit.net assertion library source code, intended to be used as a Git submodule.

To open an issue for this project, please visit the [core xUnit.net project issue tracker](https://github.com/xunit/xunit/issues).

## Annotations

The following pre-processor directives can be used to influence the resulting code contained in this repository:

### `XUNIT_NULLABLE`

Projects that consume this repository as source, which are compiled using C# 8 and wish to use nullable reference type annotations should define the `XUNIT_NULLABLE` compilation symbol to opt-in to the relevant nullability analysis annotations on method signatures.

### `XUNIT_SKIP`

The Skip family of assertions (like `Assert.Skip`) require xUnit.net v3. Define this to enable the Skip assertions.

### `XUNIT_SPAN`

There are optimized versions of `Assert.Equal` for arrays which use `Span<T>`- and/or `Memory<T>`-based comparison options. If you are using a target framework that supports `Span<T>` and `Memory<T>`, you should define `XUNIT_SPAN` to enable these new assertions.

### `XUNIT_VALUETASK`

Any asynchronous assertion API (like `Assert.ThrowsAsync`) is available with versions that consume `Task` or `Task<T>`. If you are using a target framework and compiler that support `ValueTask<T>`, you should define `XUNIT_VALUETASK` to enable additional versions of those assertions that will consume `ValueTask` and/or `ValueTask<T>`.

### `XUNIT_VISIBILITY_INTERNAL`

By default, the `Assert` class has `public` visibility. This is appropriate for the default usage (as a shipped library). If your consumption of `Assert` via source is intended to be local to a single library, you should define `XUNIT_VISIBILITY_INTERNAL` to move the visibility of the `Assert` class to `internal`.

## About xUnit.net

[<img align="right" width="100px" src="https://dotnetfoundation.org/img/logo_v4.svg" />](https://dotnetfoundation.org/projects/xunit)

xUnit.net is a free, open source, community-focused unit testing tool for the .NET Framework. Written by the original inventor of NUnit v2, xUnit.net is the latest technology for unit testing C#, F#, VB.NET and other .NET languages. xUnit.net works with ReSharper, CodeRush, TestDriven.NET and Xamarin. It is part of the [.NET Foundation](https://www.dotnetfoundation.org/), and operates under their [code of conduct](http://www.dotnetfoundation.org/code-of-conduct). It is licensed under [Apache 2](https://opensource.org/licenses/Apache-2.0) (an OSI approved license).

For project documentation, please visit the [xUnit.net project home](https://xunit.net/).
