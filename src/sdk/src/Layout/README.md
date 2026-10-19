# macOS Roslyn apphost signing

`Directory.Build.props` assigns stable `com.microsoft.dotnet.<tool-name>` signing
identifiers to the `csc`, `vbc`, and `VBCSCompiler` apphosts created by
`PublishRoslyn` in `redist/targets/GenerateLayout.targets`. Identifiers are
lowercase, including `com.microsoft.dotnet.vbcscompiler`.

The signing step retains `roslyn-entitlements.plist`. Reusable apphost templates
remain application-neutral.
