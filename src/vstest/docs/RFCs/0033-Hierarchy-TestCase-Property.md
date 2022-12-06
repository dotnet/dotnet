# 0033 Hierarchy TestCase Property

## Summary
This document standardizes an additional `Hierarchy` property on TestCase to support more flexible visual display of tests in the Visual Studio Test Explorer.

## Overview
The `ManagedType` and `ManagedMethod` properties [have been defined](0017-Managed-TestCase-Properties.md) as a way of deterministically identifying the type and method to which a test case belongs (in managed code). However, the legacy `FullyQualifiedName` property is still required to deduce a `Namespace`, `Classname` and `TestGroup` for display in the Test Explorer hierarchy. When the Test Explorer added a hierarchy for easier grouping and navigation of tests, there was no specified way for a TestCase to provide those values so they were heuristically derived from `FullyQualifiedName`. 

This document specifies a way for the TestCase to provide these display values directly to the test explorer, granting flexibility and control back to the test adapter.

## Specification

TestCases for managed code may include a string array (string[]) valued property named `Hierarchy`. The specification below outlines the requirements for the contents of the property.

### `Hierarchy` Property

The `Hierarchy` test case property represents the text strings used to display the test in the Visual Studio Test Explorer window. 

* The property must be an array of strings with 4 elements. If the property is null, missing or doesn't have 4 elements then it is is ignored as invalid.
* The element values are arranged as `[Container, Namespace, Class, TestGroup]`, Each TestCase is coalesced into a tree using the following order from the root. `Container`, `Namespace`, `Class`, `TestGroup`, `DisplayName`. 
* `Container` usually represents the project name or the assembly name. For C# tests, if this property is set as a `null` or empty string ("") then the project name will be used by VS.
* `Namespace` represents the namespace containing the test suite (or a suitable semantic equivalent). If there aren't any equivalent, or the class doesn't belong to a namespace this can be `null` or empty string ("").
* `Class` normally represents the display name of the test suite. For example, in C# this is typically the name of class containing the tests.
* `TestGroup` typically represents the method that implements a particular test or set of tests (if data-driven). If a test is not data-driven, then the TestGroup could be the same as the `DisplayName`. By data-driven, we mean tests that are run repeatedly with different sets of data (i.e. Theory tests in XUnit/NUnit). A test case should be generated for each test/data combination, where the `TestGroup` is set to the name of the test method (or similar) and the `DisplayName` includes a representation of the data used to execute the test in order to distinguish them in the test explorer UI.
   * `TestGroup` can be `null`, or empty string (""). 
   * If `TestGroup` is the same for all tests under a particular `Class` or empty, then the `TestGroup` level is omitted from the hierarchy. 
* `DisplayName` is the name of a particular single test case. For data-driven tests, the `DisplayName` should include a representation of the parameters in order to distinguish the unique test case to the end user. If the test case is not data-driven, then there is no need to include the parameters as the method name would likely be sufficient.
   * If the display name for a test is not unique under a given hierarchy, tests will not be executed or discovered correctly, and depending on the test framework test discovery could fail. For the purposes of uniqueness `Namespace` and `TestGroup` elements with `null` and empty string ("") values are considered equal.
* In some cases `ClassName`, `TestGroup` and `Method` could represent generic methods or types (for instance, tests written in C# or C++). In that case, if possible, the text should reflect the "closed" or instantiated typename. For example when displaying the generic type `List<>`, the closed form (e.g. `List<String>, List<MyType>`) should be used. The generic type `List<>` is referenced in metadata as ``List`1`` and in the code as `List<T>`; both of these are "open" forms of the type because they do not specify the type of `T` and could cause confusion.
