# Localization

When adding or changing a localized string:

1. Edit only the corresponding `.resx` file.
2. Build the project to regenerate the `.Designer.cs` and `.xlf` files.
3. Include the `.resx`, `.Designer.cs`, and generated `.xlf` changes in the pull request.

Never edit `.xlf` files manually. For corrections to an existing translation, follow the translation-error process in the [localizability guidance](../../docs/localizability.md) instead of creating an `.xlf`-only change.
