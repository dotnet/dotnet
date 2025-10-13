# GitHub Copilot Instructions for ASP.NET Core Razor

This repository contains the **ASP.NET Core Razor** compiler and tooling implementation. It provides the Razor language experience across Visual Studio, Visual Studio Code, and other development environments.

## Repository Overview

This repository implements:

- **Razor Compiler**: The core Razor compilation engine and source generators
- **Language Server**: LSP-based language services for cross-platform editor support
- **Visual Studio Integration**: Rich editing experience and tooling for Visual Studio 
- **IDE Tools**: Debugging, IntelliSense, formatting, and other developer productivity features

### Key Components

| Component | Purpose | Key Projects |
|-----------|---------|--------------|
| **Compiler** | Core Razor compilation and code generation | `Microsoft.AspNetCore.Razor.Language`, `Microsoft.CodeAnalysis.Razor.Compiler` |
| **Language Server** | Cross-platform language services via LSP | `Microsoft.AspNetCore.Razor.LanguageServer` |
| **Visual Studio** | VS-specific tooling and integration | `Microsoft.VisualStudio.RazorExtension`, `Microsoft.VisualStudio.LanguageServices.Razor` |
| **Workspaces** | Project system and document management | `Microsoft.CodeAnalysis.Razor.Workspaces` |

## Razor Language Concepts

When working with this codebase, understand these core Razor concepts:

### File Types and Extensions
- `.razor` - Blazor components (client-side and server-side)
- `.cshtml` - Razor views and pages (ASP.NET Core MVC/Pages)

### Language Kinds
Razor documents contain multiple languages:
- **Razor syntax** - `@` expressions, directives, code blocks
- **C# code** - Server-side logic embedded in Razor
- **HTML markup** - Static HTML and dynamic content
- **JavaScript/CSS** - Client-side code within Razor files

### Key Architecture Patterns
- **RazorCodeDocument** - Central document model containing parsed syntax tree and generated outputs
- **Language Server Protocol (LSP)** - Cross-platform editor integration
- **Virtual Documents** - Separate C# and HTML projections for tooling
- **Document Mapping** - Translation between Razor and generated C# positions

## Development Guidelines

### Code Style and Patterns

Follow these patterns when contributing:

```csharp
// ✅ Prefer readonly fields and properties
private readonly ILogger _logger;

// ✅ Use explicit interface implementations for internal contracts
internal sealed class MyService : IMyService
{
    public void DoWork() { }
}

// ✅ Use cancellation tokens in async methods
public async Task<Result> ProcessAsync(CancellationToken cancellationToken)
{
    // Implementation
}

// ✅ Follow null-checking patterns
public void Method(string? value)
{
    if (value is null)
    {
        return;
    }
    
    // Use value here
}
```

### Testing Patterns

- Prefer `TestCode` over plain strings for before/after test scenarios
- Prefer raw string literals over verbatim strings
- Ideally we test the end user scenario, not implementation details

### Architecture Considerations

- **Performance**: Razor compilation happens on every keystroke - optimize for speed
- **Cross-platform**: Code should work on Windows, macOS, and Linux
- **Editor integration**: Consider both Visual Studio and VS Code experiences
- **Backwards compatibility**: Maintain compatibility with existing Razor syntax

## Build and Development

### Prerequisites
- .NET 8.0+ SDK (latest version specified in `global.json`)
- Visual Studio 2022 17.8+ or VS Code with C# extension
- PowerShell (for Windows build scripts)

### Building
- `./restore.sh` - Restore dependencies
- `./build.sh` - Full build
- `./build.sh -test` - Build and run tests

### Visual Studio Development
- `./startvs.ps1` - Open Visual Studio with correct environment
- `./build.cmd -deploy` - Build and deploy VS extension for testing

## Project Structure Navigation

```
src/
├── Compiler/           # Core Razor compilation engine
│   ├── Microsoft.AspNetCore.Razor.Language/     # Core language APIs
│   └── Microsoft.CodeAnalysis.Razor.Compiler/   # Roslyn integration
├── Razor/             # Language server and tooling
│   ├── src/Microsoft.AspNetCore.Razor.LanguageServer/  # LSP implementation
│   ├── src/Microsoft.VisualStudio.RazorExtension/      # VS package
│   └── test/          # Language server tests
└── Shared/            # Common utilities and test helpers
```

## Debugging and Diagnostics

### Language Server Debugging
- Use F5 debugging in VS with `Microsoft.VisualStudio.RazorExtension` as startup project
- Check language server logs in VS Output window (Razor Logger Output)

### Common Issues
- **Performance**: Check document mapping efficiency and async/await usage
- **Cross-platform**: Test path handling and case sensitivity
- **Memory**: Use `using` statements for disposable resources
- **Threading**: Ensure proper async/await patterns, avoid `Task.Wait()`

## Contributing

- Follow the [Contributing Guidelines](CONTRIBUTING.md)
- Build and test locally before submitting PRs
- Add appropriate test coverage for new features
- Update documentation for public API changes
- Consider cross-platform compatibility in all changes

## Useful Resources

- [ASP.NET Core Razor documentation](https://docs.microsoft.com/aspnet/core/razor-pages)
- [Blazor documentation](https://learn.microsoft.com/en-gb/aspnet/core/blazor/?)
- [Language Server Protocol specification](https://microsoft.github.io/language-server-protocol/)
- [Build from source instructions](docs/contributing/BuildFromSource.md)