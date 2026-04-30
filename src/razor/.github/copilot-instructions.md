# GitHub Copilot Instructions for ASP.NET Core Razor

## Critical Rules

- **Build**: Use `build.cmd` (Windows) or `./build.sh` (Linux/Mac). NEVER use `dotnet build` directly.
- **Test**: Use `build.cmd -test` or target a specific project with `dotnet test path/to/Project.csproj`. NEVER run `dotnet test` at the repo root — it includes Playwright integration tests that require VS Code and waste significant time.
- **Build wrappers**: Be careful passing `-projects` through `build.cmd`/PowerShell wrappers. Do not pass a semicolon-delimited project list through a nested PowerShell command invocation, because PowerShell can treat `;` as a statement separator and open `.csproj` files in Visual Studio. Prefer a single project at a time, or invoke the underlying script in a way that preserves the full `-projects` value as one argument.
- **Processes**: NEVER kill dotnet processes by name (`Stop-Process -Name`, `taskkill /IM`). Other work may be running on the machine.
- **Bug fixes**: Look for existing code that already handles the scenario before adding new code. The bug is more likely in existing logic than a missing feature.
- **Helpers**: Review existing helpers (`UsingDirectiveHelper`, `AddUsingsHelper`, etc.) before writing new utility methods. Don't duplicate.

## File Types

- `.razor` — Blazor components. `.cshtml` — Razor views/pages (referred to as "Legacy" in the codebase).

## Code Patterns

- **Collections**: Use `ListPool<T>.GetPooledObject(out var list)` and `PooledArrayBuilder<T>` instead of allocating new collections. Prefer immutable collection types.
- **Positions**: Use `GetRequiredAbsoluteIndex` for converting positions to absolute indexes.
- **LSP conversions**: `sourceText.GetTextChange(textEdit)` converts LSP `TextEdit` → Roslyn `TextChange`. Reverse: `sourceText.GetTextEdit(change)`. Both in `LspExtensions_SourceText.cs`.
- **RazorCodeDocument**: Immutable — every `With*` method creates a new instance passing ALL fields through the constructor. When adding a new field, thread it through every existing `With*` method. Prefer computing derived data via extension methods (e.g., `GetUnusedDirectives()`) rather than storing computed results as fields.
- **Razor documents in Roslyn**: Stored as additional documents. Resolve via `solution.GetDocumentIdsWithFilePath(filePath)` → `solution.GetAdditionalDocument(documentId)`.
- **Remote services**: Place the public stub method (calling `RunServiceAsync`) directly above its private implementation method.

## Testing

- Place `[WorkItem("url")]` on tests that track a specific issue (GitHub or DevOps URL).
- Use `TestCode` with `[|...|]` span markers for before/after test scenarios. Access `input.Text` (cleaned) and `input.Span` (marked range).
- Prefer raw string literals (`"""..."""`) over verbatim strings (`@"..."`).
- Test end-user scenarios, not implementation details.
- Verify/helper methods go at the bottom of test files. New test methods go above them.
- New tooling tests go in `src\Razor\test\Microsoft.VisualStudioCode.RazorExtension.Test` (Cohosting architecture).
- Integration tests using `AdditionalSyntaxTrees` for tag helper discovery must set `UseTwoPhaseCompilation => true` (see `ComponentDiscoveryIntegrationTest`).

## Adding OOP Remote Services

When adding a new `IRemote*Service` and `Remote*Service`:

1. Interface → `src\Razor\src\Microsoft.CodeAnalysis.Razor.Workspaces\Remote\`
2. Implementation → `src\Razor\src\Microsoft.CodeAnalysis.Remote.Razor\`
3. Register in `RazorServices.cs` (add to `MessagePackServices` or `JsonServices`)
4. **Add entry to `eng\targets\Services.props`**: `Include="Microsoft.VisualStudio.Razor.{ShortName}"` with `ClassName="{FullTypeName}+Factory"`
5. Validate: `dotnet test src\Razor\test\Microsoft.CodeAnalysis.Remote.Razor.Test --filter "FullyQualifiedName~RazorServicesTest"`

## VS Code Validation

Run Playwright E2E tests (exception to the "no `dotnet test` at root" rule):
```powershell
cd src\Razor\test\Microsoft.VisualStudioCode.Razor.IntegrationTests
dotnet test
```
