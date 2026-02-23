# GitHub Copilot Instructions for ASP.NET Core Razor

This repository contains the **ASP.NET Core Razor** compiler and tooling implementation. It provides the Razor language experience across Visual Studio, Visual Studio Code, and other development environments.

## Repository Overview

This repository implements:

- **Razor Compiler**: The core Razor compilation engine and source generators
- **Language Server**: LSP-based language services for cross-platform editor support
- **Visual Studio Integration**: Rich editing experience and tooling for Visual Studio
- **Visual Studio Code Extension**: Rich editing experience and tooling for Visual Studio
- **IDE Tools**: Debugging, IntelliSense, formatting, and other developer productivity features

## Razor Language Concepts

When working with this codebase, understand these core Razor concepts:

### File Types and Extensions
- `.razor` - Blazor components (client-side and server-side)
- `.cshtml` - Razor views and pages (ASP.NET Core MVC/Pages) also referred to as "Legacy" in the codebase

### Language Kinds
Razor documents contain multiple languages:
- **Razor syntax** - `@` expressions, directives, code blocks
- **C# code** - Server-side logic embedded in Razor
- **HTML markup** - Static HTML and dynamic content
- **JavaScript/CSS** - Client-side code within Razor files

## Development Guidelines

### Coding Patterns

- Always build and test with `build.sh -test` before submitting PRs, without specifying a project or test filter
- Write clear, concise, and maintainable code
- Always place `[WorkItem]` attributes on tests for tracking
- Prefer immutable collection types and pooled collections where possible
- Use `using` statements for disposable resources
- Ensure proper async/await patterns, avoid `Task.Wait()`
- Use GetRequiredAbsoluteIndex for converting positions to absolute indexes

### Testing Patterns

- Add appropriate test coverage for new features
- Prefer `TestCode` over plain strings for before/after test scenarios
- Prefer raw string literals over verbatim strings
- Ideally we test the end user scenario, not implementation details
- Consider cross-platform compatibility by testing path handling and case sensitivity where applicable
- For tooling, "Cohosting" is the new architecture we're moving towards, so always create tests in the src\Razor\test\Microsoft.VisualStudioCode.RazorExtension.Test project

### Architecture Considerations

- **Performance**: Razor compilation happens on every keystroke - optimize for speed
- **Cross-platform**: Code should work on Windows, macOS, and Linux
- **Editor integration**: Consider both Visual Studio and VS Code experiences
- **Backwards compatibility**: Maintain compatibility with existing Razor syntax

## Build and Development

### Prerequisites
- .NET 9.0+ SDK (latest version specified in `global.json`)
- Visual Studio 2026 (Windows) or VS Code with C# extension (Windows, macOS or Linux)
- PowerShell (for Windows build scripts)

### Building
- `./restore.sh` - Restore dependencies
- `./build.sh` - Full build
- DO NOT USE `dotnet build` directly

### Testing
- `./build.sh -test` - Build and run tests
- DO NOT USE `dotnet test` directly — running it at the repo root will include integration tests (e.g., Playwright-based VS Code tests) that require external dependencies and waste significant time. Use `build.cmd -test` (or `build.sh -test`) instead, or target a specific test project with `dotnet test path/to/Project.csproj`.

## VS Code Local Validation

When making changes to Razor tooling for VS Code, you can validate your changes in a real VS Code environment.

For automated validation, run the Playwright-based E2E tests. These tests are an exception to the "DO NOT USE `dotnet test`" rule above, as they require VS Code to be installed and are not run as part of the standard build:
```powershell
cd src\Razor\test\Microsoft.VisualStudioCode.Razor.IntegrationTests
dotnet test
```
