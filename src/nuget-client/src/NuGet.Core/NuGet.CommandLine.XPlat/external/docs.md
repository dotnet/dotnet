
***Run scripts\utils\ttGen.ps1 after changes to update docs.md
***Then run scripts\utils\docsPRGen.ps1 to split into several files in dotnet docs fork.
***
***[Dotnet docs repo contrib guidelines](https://github.com/dotnet/docs/blob/master/CONTRIBUTING.md#process-for-contributing)
---file:docs\core\tools\dotnet-nuget-add-source.md
---
title: dotnet nuget add source command
description: The dotnet nuget add source command adds a new package source to your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget add source

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget add source` - Add a NuGet source.

## Synopsis

```dotnetcli
dotnet nuget add source <PACKAGE_SOURCE_PATH> [--name] [--username]
    [--password] [--store-password-in-clear-text] [--valid-authentication-types]
    [--configfile]
dotnet nuget add source [-h|--help]
```

## Description

The `dotnet nuget add source` command adds a new package source to your NuGet configuration files.

## Arguments

- **`PACKAGE_SOURCE_PATH`**

  Path to the package source.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

- **`-n|--name`**

  Name of the source.

- **`-p|--password`**

  Password to be used when connecting to an authenticated source.

- **`--store-password-in-clear-text`**

  Enables storing portable package source credentials by disabling password encryption.

- **`-u|--username`**

  Username to be used when connecting to an authenticated source.

- **`--valid-authentication-types`**

  Comma-separated list of valid authentication types for this source. Set this to `basic` if the server advertises NTLM or Negotiate and your credentials must be sent using the Basic mechanism, for instance when using a PAT with on-premises Azure DevOps Server. Other valid values include `negotiate`, `kerberos`, `ntlm`, and `digest`, but these values are unlikely to be useful.

## Examples

- Add `nuget.org` as a source:

  ```dotnetcli
  dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
  ```

- Add `c:\packages` as a local source:

  ```dotnetcli
  dotnet nuget add source c:\packages
  ```

- Add a source that needs authentication:

  ```dotnetcli
  dotnet nuget add source https://contoso.com/litware -n myTeam -u myUsername -p myPassword --store-password-in-clear-text
  ```

- Add a source that needs authentication (then go install credential provider):

  ```dotnetcli
  dotnet nuget add source https://pkgs.dev.azure.com/contoso/litware/_packaging/litware-deps/nuget/v3/index.json -n myTeam
  ```

## See also

- [Package source sections in NuGet.config files](/nuget/reference/nuget-config-file#package-source-sections)

- [sources command (nuget.exe)](/nuget/reference/cli-reference/cli-ref-sources)
---file:docs\core\tools\dotnet-nuget-add-client-cert.md
---
title: dotnet nuget add client-cert command
description: The dotnet nuget add client-cert command adds a new client certificate to your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget add client-cert

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget add client-cert` - Adds a client certificate configuration that matches the given package source name.

## Synopsis

```dotnetcli
dotnet nuget add client-cert [--package-source] [--path]
    [--password] [--store-password-in-clear-text] [--store-location]
    [--store-name] [--find-by] [--find-value] [--force] [--configfile]
dotnet nuget add client-cert [-h|--help]
```

## Description

The `dotnet nuget add client-cert` command adds a new client certificate to your NuGet configuration files.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

- **`--find-by`**

  Search method to find certificate in certificate store (see docs).

- **`--find-value`**

  Search the certificate store for the supplied value. Used with FindValue (see docs).

- **`-f|--force`**

  Skip certificate validation.

- **`-s|--package-source`**

  Package source name.

- **`--password`**

  Password for the certificate, if needed.

- **`--path`**

  Path to certificate file.

- **`--store-location`**

  Certificate store location (see docs).

- **`--store-name`**

  Certificate store name (see docs).

- **`--store-password-in-clear-text`**

  Enables storing password for the certificate by disabling password encryption.

---file:docs\core\tools\dotnet-nuget-disable-source.md
---
title: dotnet nuget disable source command
description: The dotnet nuget disable source command disables an existing source in your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget disable source

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget disable source` - Disable a NuGet source.

## Synopsis

```dotnetcli
dotnet nuget disable source <NAME> [--configfile]
dotnet nuget disable source [-h|--help]
```

## Description

The `dotnet nuget disable source` command disables an existing source in your NuGet configuration files.

## Arguments

- **`NAME`**

  Name of the source.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

## Examples

- Disable a source with name of `mySource`:

  ```dotnetcli
  dotnet nuget disable source mySource
  ```

## See also

- [Package source sections in NuGet.config files](/nuget/reference/nuget-config-file#package-source-sections)

- [sources command (nuget.exe)](/nuget/reference/cli-reference/cli-ref-sources)
---file:docs\core\tools\dotnet-nuget-enable-source.md
---
title: dotnet nuget enable source command
description: The dotnet nuget enable source command enables an existing source in your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget enable source

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget enable source` - Enable a NuGet source.

## Synopsis

```dotnetcli
dotnet nuget enable source <NAME> [--configfile]
dotnet nuget enable source [-h|--help]
```

## Description

The `dotnet nuget enable source` command enables an existing source in your NuGet configuration files.

## Arguments

- **`NAME`**

  Name of the source.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

## Examples

- Enable a source with name of `mySource`:

  ```dotnetcli
  dotnet nuget enable source mySource
  ```

## See also

- [Package source sections in NuGet.config files](/nuget/reference/nuget-config-file#package-source-sections)

- [sources command (nuget.exe)](/nuget/reference/cli-reference/cli-ref-sources)
---file:docs\core\tools\dotnet-nuget-list-source.md
---
title: dotnet nuget list source command
description: The dotnet nuget list source command lists all existing sources from your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget list source

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget list source` - Lists all configured NuGet sources.

## Synopsis

```dotnetcli
dotnet nuget list source [--format] [--configfile]
dotnet nuget list source [-h|--help]
```

## Description

The `dotnet nuget list source` command lists all existing sources from your NuGet configuration files.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

- **`--format`**

  The format of the list command output: `Detailed` (the default) and `Short`.

## Examples

- List configured sources from the current directory:

  ```dotnetcli
  dotnet nuget list source
  ```

## See also

- [Package source sections in NuGet.config files](/nuget/reference/nuget-config-file#package-source-sections)

- [sources command (nuget.exe)](/nuget/reference/cli-reference/cli-ref-sources)
---file:docs\core\tools\dotnet-nuget-list-client-cert.md
---
title: dotnet nuget list client-cert command
description: The dotnet nuget list client-cert command lists all configured client certificates. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget list client-cert

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget list client-cert` - Lists all the client certificates in the configuration.

## Synopsis

```dotnetcli
dotnet nuget list client-cert [--configfile]
dotnet nuget list client-cert [-h|--help]
```

## Description

The `dotnet nuget list client-cert` command lists all configured client certificates.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

---file:docs\core\tools\dotnet-nuget-remove-source.md
---
title: dotnet nuget remove source command
description: The dotnet nuget remove source command removes an existing source from your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget remove source

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget remove source` - Remove a NuGet source.

## Synopsis

```dotnetcli
dotnet nuget remove source <NAME> [--configfile]
dotnet nuget remove source [-h|--help]
```

## Description

The `dotnet nuget remove source` command removes an existing source from your NuGet configuration files.

## Arguments

- **`NAME`**

  Name of the source.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

## Examples

- Remove a source with name of `mySource`:

  ```dotnetcli
  dotnet nuget remove source mySource
  ```

## See also

- [Package source sections in NuGet.config files](/nuget/reference/nuget-config-file#package-source-sections)

- [sources command (nuget.exe)](/nuget/reference/cli-reference/cli-ref-sources)
---file:docs\core\tools\dotnet-nuget-remove-client-cert.md
---
title: dotnet nuget remove client-cert command
description: The dotnet nuget remove client-cert command removes an existing client certificate configuration from your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget remove client-cert

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget remove client-cert` - Removes the client certificate configuration that matches the given package source name.

## Synopsis

```dotnetcli
dotnet nuget remove client-cert [--package-source] [--configfile]
dotnet nuget remove client-cert [-h|--help]
```

## Description

The `dotnet nuget remove client-cert` command removes an existing client certificate configuration from your NuGet configuration files.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

- **`-s|--package-source`**

  Package source name.

---file:docs\core\tools\dotnet-nuget-update-source.md
---
title: dotnet nuget update source command
description: The dotnet nuget update source command updates an existing source in your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget update source

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget update source` - Update a NuGet source.

## Synopsis

```dotnetcli
dotnet nuget update source <NAME> [--source] [--username]
    [--password] [--store-password-in-clear-text] [--valid-authentication-types]
    [--configfile]
dotnet nuget update source [-h|--help]
```

## Description

The `dotnet nuget update source` command updates an existing source in your NuGet configuration files.

## Arguments

- **`NAME`**

  Name of the source.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

- **`-p|--password`**

  Password to be used when connecting to an authenticated source.

- **`-s|--source`**

  Path to the package source.

- **`--store-password-in-clear-text`**

  Enables storing portable package source credentials by disabling password encryption.

- **`-u|--username`**

  Username to be used when connecting to an authenticated source.

- **`--valid-authentication-types`**

  Comma-separated list of valid authentication types for this source. Set this to `basic` if the server advertises NTLM or Negotiate and your credentials must be sent using the Basic mechanism, for instance when using a PAT with on-premises Azure DevOps Server. Other valid values include `negotiate`, `kerberos`, `ntlm`, and `digest`, but these values are unlikely to be useful.

## Examples

- Update a source with name of `mySource`:

  ```dotnetcli
  dotnet nuget update source mySource --source c:\packages
  ```

## See also

- [Package source sections in NuGet.config files](/nuget/reference/nuget-config-file#package-source-sections)

- [sources command (nuget.exe)](/nuget/reference/cli-reference/cli-ref-sources)
---file:docs\core\tools\dotnet-nuget-update-client-cert.md
---
title: dotnet nuget update client-cert command
description: The dotnet nuget update client-cert command updates an existing client certificate in your NuGet configuration files. 
ms.date: REPLACE_WITH_CURRENT_DATE_IN_PR_CREATION_TOOL
---
# dotnet nuget update client-cert

**This article applies to:** ✔️ .NET Core 3.1.200 SDK and later versions

## Name

`dotnet nuget update client-cert` - Updates the client certificate configuration that matches the given package source name.

## Synopsis

```dotnetcli
dotnet nuget update client-cert [--package-source] [--path]
    [--password] [--store-password-in-clear-text] [--store-location]
    [--store-name] [--find-by] [--find-value] [--force] [--configfile]
dotnet nuget update client-cert [-h|--help]
```

## Description

The `dotnet nuget update client-cert` command updates an existing client certificate in your NuGet configuration files.

## Options

- **`--configfile`**

  The NuGet configuration file. If specified, only the settings from this file will be used. If not specified, the hierarchy of configuration files from the current directory will be used. For more information, see [Common NuGet Configurations](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior).

- **`--find-by`**

  Search method to find certificate in certificate store (see docs).

- **`--find-value`**

  Search the certificate store for the supplied value. Used with FindValue (see docs).

- **`-f|--force`**

  Skip certificate validation.

- **`-s|--package-source`**

  Package source name.

- **`--password`**

  Password for the certificate, if needed.

- **`--path`**

  Path to certificate file.

- **`--store-location`**

  Certificate store location (see docs).

- **`--store-name`**

  Certificate store name (see docs).

- **`--store-password-in-clear-text`**

  Enables storing password for the certificate by disabling password encryption.

