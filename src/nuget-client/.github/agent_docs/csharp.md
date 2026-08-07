# C# conventions

- Never use reflection.
- Deconstruct value-tuple results into named elements instead of assigning the tuple to a single `var`, for example:

  ```csharp
  (var name, var value) = Method();
  ```

- Nullable reference types are enabled at the project level. Do not add `#nullable enable` or `#nullable disable` directives.

For additional repository coding conventions, refer to the [guidelines](../../docs/coding-guidelines.md) when needed.
