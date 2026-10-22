### Note

This (and code in Rules) was pulled from the [sdk](https://github.com/dotnet/sdk/blob/8f983a2c12162e49af7a7f2eb7744d220859f42b/src/RazorSdk/Targets/Microsoft.NET.Sdk.Razor.DesignTime.targets). These files are for tooling to define specific design time behavior and thus should be owned by tooling. As of writing these are only used in VS Code.

Visual Studio uses a separate copy in `src/Shared/files/RazorDesignTime`. Those six files preserve the exact bytes from `Microsoft.NET.Sdk.Razor` version `6.0.0-alpha.1.21072.5`, including the visible fields in `RazorExtension.xaml`. Their source is [dotnet/aspnetcore at commit 20b6779cf3af2fed6a8fe64a0865cfab50b776bb](https://github.com/dotnet/aspnetcore/tree/20b6779cf3af2fed6a8fe64a0865cfab50b776bb/src/Razor/Microsoft.NET.Sdk.Razor/src/build/netstandard2.0); the package uses CRLF where the source uses LF.

Generated-rule fallback names hash the XAML bytes, so the scoped `.gitattributes` disables line-ending conversion for these files. Preserve their original bytes when updating them.

Keeping only the design-time target and five rules avoids restoring the SDK's unused task assemblies. The scoped `cgmanifest.json` records their provenance, and `NOTICE.txt` includes the Apache-2.0 attribution. Keep the VS and VS Code copies separate unless their behaviour has been checked together.