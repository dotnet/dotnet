# Razor Legacy ASP.NET Core Editor

This project contains the implementation for the Razor "legacy" ASP.NET Core
editor. This is a version of the Razor editor targeting ASP.NET Core projects
that pre-dates the LSP-based Razor editor, which is the default. Users can
enable the legacy editor by enabling the "Use legacy Razor editor for ASP.NET
Core" option in Tools->Options->Text Editor->HTML->Advanced. Unless this option
is checked, the legacy editor assembly should _not_ load in Visual Studio.

This project is intended to be isolated. Only
`Microsoft.AspNetCore.Razor.ExternalAccess.LegacyEditor` (and tests) may take a
dependency on it. `ExternalAccess.LegacyEditor` exposes an internal API shim
that is used directly by Web Tools.
