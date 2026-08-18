For SDK-style .NET test projects, the supported path is to reference
`Microsoft.NET.Test.Sdk` and run tests with `dotnet test`.

`dotnet test` builds the test project, restores NuGet packages, and runs the test
assembly with the test platform components provided by `Microsoft.NET.Test.Sdk`.
The VSTest testhost provider still uses the test assembly's `.deps.json` and, when
present, `.runtimeconfig.dev.json` to locate `testhost.dll`; if it cannot resolve
the package-provided testhost for a managed test assembly, it fails with guidance
to add `Microsoft.NET.Test.Sdk`.

In normal SDK-style projects there is no separate VSTest-specific probing-path setup
to document or configure. If you need to run tests from a copied directory or on a
machine that does not have the restored NuGet packages, publish the test project and
run from the publish output so the test assembly and its runtime dependencies are
available together.

Current `dotnet test` usage is documented in the .NET testing guide:
<https://learn.microsoft.com/dotnet/core/testing/>.
