# Layers

## Product Layers

### Shared

This project is shared between all layers (both compiler and tooling) to provide language support types for modern C# language features.

- Target Framework: `net7.0;netstandard2.0;net472`
- Projects:
  - Microsoft.AspNetCore.Razor.LanguageSupport

### Compiler

- Target Framework: `netstandard2.0`
- Projects:
  - Microsoft.AspNetCore.Mvc.Razor.Extensions
  - Microsoft.AspNetCore.Mvc.Razor.Extensions.Version1_X
  - Microsoft.AspNetCore.Mvc.Razor.Extensions.Version2_X
  - Microsoft.AspNetCore.Razor.Language
  - Microsoft.CodeAnalysis.Razor
  - Microsoft.NET.Sdk.Razor.SourceGenerators

### Tooling Core

These projects are referenced by most Razor tooling projects. Because of this, they
target the broadest set of frameworks.

- Target Framework: `net7.0;netstandard2.0;net472`
- Projects:
  - Microsoft.AspNetCore.Razor.Common
  - Microsoft.CodeAnalysis.Razor.Workspaces

### Razor Language Server

- Target Framework: `net7.0;net472`
- Projects:
  - Microsoft.AspNetCore.Razor.LanguageServer
  - Microsoft.AspNetCore.Razor.LanguageServer.Common
  - Microsoft.AspNetCore.Razor.LanguageServer.Protocol

### Razor Language Server (rzls)

- Target Framework: `net7.0`
- Projects:
  - rzls

### Roslyn OOP (for Visual Studio)

- Target Framework: `netstandard2.0`
- Projects:
  - Microsoft.CodeAnalysis.Remote.Razor
  - Microsoft.CodeAnalysis.Remote.Razor.CoreComponents

### Visual Studio (Windows)

- Target Framework: `net472`
- Projects:
  - Microsoft.VisualStudio.Editor.Razor
  - Microsoft.VisualStudio.LanguageServer.ContainedLanguage
  - Microsoft.VisualStudio.LanguageServerClient.Razor
  - Microsoft.VisualStudio.LiveShare.Razor
  - Microsoft.VisualStudio.RazorExtension
  - Microsoft.VisualStudio.RazorExtension.Dependencies
  - RazorDeployment

### Visual Studio (Mac)

- Target Framework: `net472`
- Projects:
  - Microsoft.VisualStudio.Mac.LanguageServices.Razor
  - Microsoft.VisualStudio.Mac.RazorAddin

### Visual Studio Code (OmniSharp Plug-in)

- Target Framework: `net472`
- Projects:
  - Microsoft.AspNetCore.Razor.OmniSharpPlugin
  - Microsoft.AspNetCore.Razor.OmniSHarpPlugin.StrongNamed

## Testing Layers

### Shared test infra

- Microsoft.AspNetCore.Razor.Test.Common (`net7.0`;`net472`)

### API Shims

- Microsoft.AspNetCore.Razor.Test.ComponentShim (`netstandard2.0`)
- Microsoft.AspNetCore.Razor.Test.MvcShim (`net7.0`;`net472`)
- Microsoft.AspNetCore.Razor.Test.MvcShim.ClassLib (`netstandard2.0`)
- Microsoft.AspNetCore.Razor.Test.MvcShim.Version1_X (`net7.0`;`net472`)
- Microsoft.AspNetCore.Razor.Test.MvcShim.Version2_X (`net7.0`;`net4720`)

### Tooling Core Tests

- Microsoft.CodeAnalysis.Razor.Workspaces.Test (`net7.0`;`net472` - only on Windows)
- Microsoft.CodeAnalysis.Razor.Workspaces.Test.Common (`net7.0`;`net472`)

### Language Server

- Microsoft.AspNetCore.Razor.LanguageServer.Common.Test (`net7.0`)
- Microsoft.AspNetCore.Razor.LanguageServer.Test (`net7.0-windows`)
- Microsoft.AspNetCore.Razor.LanguageServer.Test.Common (`net7.0`;`net472`)

### Roslyn OOP (for Visual Studio) Tests

- Microsoft.CodeAnalysis.Remote.Razor.Test (`net7.0`;`net472` - only on Windows)

### Visual Studio Code (Windows)

- Microsoft.VisualStudio.Editor.Razor.Test (`net472`)
- Microsoft.VisualStudio.Editor.Razor.Test.Common (`net472`)
- Microsoft.VisualStudio.LanguageServer.ContainedLanguage.Test (`net472`)
- Microsoft.VisualStudio.LanguageServer.ContainedLanguage.Test.Common (`net472`)
- Microsoft.VisualStudio.LanguageServerClient.Razor.Test (`net472`)
- Microsoft.VisualStudio.LanguageServices.Razor.Test (`net472`)
- Microsoft.VisualStudio.LiveShare.Razor.Test (`net472`)
- Microsoft.VisualStudio.Razor.IntegrationTests (`net472`)

### Visual Studio (Mac) Tests

- Microsoft.VisualStudio.Mac.LanguageServices.Razor.Test (`net472`)

### Visual Studio Code (OmniSharp Plug-in) Tests

- Microsoft.AspNetCore.Razor.OmniSharpPlugin.Test (`net472`)
