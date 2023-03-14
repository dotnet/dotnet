# Not maintained anymore

:warning: Microsoft.DotNet.ApiCompat which is CCI based isn't maintained anymore and was never intended to be publicly used. Please switch to the Roslyn based ApiCompat & PackageValidation functionality that is part of the .NET SDK: https://learn.microsoft.com/en-us/dotnet/fundamentals/package-validation/overview. This code base will be deleted in the future.

## Microsoft.DotNet.ApiCompat

APICompat is a tool which may be used to test API compatibility between a two .NET assemblies.

When testing, the tool will compare a *contract* to an *implementation*.  

The *contract* represents the API that's expected : for example a reference assembly or a previous version of an assembly.

The *implementation* represents the API that's provided : for example the current version of an assembly.

### Usage

API Compat can be used by referencing this Microsoft.DotNet.ApiCompat package from the *implementation* project, and providing the path to the *contract* via a single `@(ResolvedMatchingContract)` item.  Dependencies of `@(ResolvedMatchingContract)` must be specified in either `DependencyPaths` metadata on the items themselves or via the `$(ContractDependencyPaths)` property.

When API Compat identifies an error it will log the error and fail the build.  If you wish to ignore the error you can copy the error text to a baseline file (see below).  Take care when doing this as these errors represent compatibility problems between the *contract* and *implementation*.

## Required setting

- `@(ResolvedMatchingContract)` - should point to a single file that represents the contract to validate
- `%(DependencyPaths)` - optional, specifies a semi-colon delimited set of paths that contain the assembly dependencies of this contract
- `$(ContractDependencyPaths)` - optional, specifies a semi-colon delimited set of paths that contain the assembly dependencies of this contract

## Additional settings

- `$(RunApiCompat)` - true to run APICompat, defaults to true
- `$(RunApiCompatForSrc)` - true to run APICompat treating project output as *implementation* and `@(ResolvedMatchingContract)` as *contract*, defaults to true.
- `$(RunMatchingRefApiCompat)` - true to run APICompat treating project output as *contract* and  `@(ResolvedMatchingContract)` as *implementation*, defaults to false.  This is also known as reverse API compat and can help ensure that every public API defined in a project is exposed in `@(ResolvedMatchingContract)`.
- `$(ApiCompatExcludeAttributeList)` - Attributes to exclude from APICompat checks.  This is a text file containing types in DocID format, EG: T:Namespace.TypeName.
- `$(ApiCompatEnforceOptionalRules)` - true to enforce optional rules, default is false.  An example of an optional rule is parameter naming which can break source compatibility but not binary compatibility.
- `$(ApiCompatBaseline)` - path to baseline file used to suppress errors, defaults to a file in the project directory.
- `$(BaselineAllAPICompatError)` - true to indicate that the baseline file should be rewritten suppressing all API compat errors.  You may set this when building the project to conveniently update the baseline when you wish to suppress them, eg: `dotnet msbuild /p:BaselineAllAPICompatError=true`
- `$(MatchingRefApiCompatBaseline)` - same as `$(ApiCompatBaseline)` but for reverse API compat.
- `$(BaselineAllMatchingRefApiCompatError)` - same as `$(BaselineAllAPICompatError)` but for reverse API compat.
- `$(AdditionalApiCompatOptions)` - allows you to pass additional parameters as command line parameters, for example `--exclude-non-browsable`.
