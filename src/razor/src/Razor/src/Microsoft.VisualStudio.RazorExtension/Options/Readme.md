# How To Add A New Option

Options shown in VS are controlled by Razor. This is currently due to a limitation in how options for LSP have to be introduced. We want to exist in a Tools > Options style for VS, so we need to provide our own page and respond to the [workspace/configuration](https://microsoft.github.io/language-server-protocol/specifications/lsp/3.17/specification/#workspace_configuration) request.

These are the steps needed to add a new option:

1. Choose the correct page. As of writing this, there is an `AdvancedOptionPage`, so we'll use that for the example.
2. Add the setting type to OptionsStorage so that it is written to disk when the user changes. If needed, add a new Get/Set method for the type.
3. Add a property with `LocCategory`, `LocDescription`, and `LocDisplayName`, with the argument being the `nameof` operator on the resource accessor.
4. Add the property to the `ClientAdvancedSettings` record and update `OptionsStorage.GetAdvancedSettings` to appropriately construct the type.
5. Add the appropriate setting to `RazorLSPOptions` if it needs to be used on the LSP side. This represents the LSP Server understanding of any client side settings that need to be enabled. There is a constructor that takes `ClientSettings` and applies to properties as needed.
6. In whatever place you need the option, import an `IOptionsMonitor<RazorLSPOptions>` to get the current value as needed.