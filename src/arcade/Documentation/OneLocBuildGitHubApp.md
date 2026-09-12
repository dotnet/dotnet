# Authenticating OneLocBuild's GitHub check-in with the GitHub App

This document explains how a repository gains access to the default **GitHub App**
authentication path for the OneLocBuild localization check-in PR. It supplements the main
[OneLocBuild in Arcade](OneLocBuild.md) documentation.

## Background

When OneLocBuild is configured for a GitHub-based repo (`RepoType: gitHub`), the task opens or
updates a pull request to check in localized files. The GitHub App authentication path mints a
short-lived installation token (`ghs_…`) at build time, avoiding a stored GitHub credential. The
repositories accessible to the token are determined by the GitHub App installation configuration
(all repositories or the selected repositories).
[GitHub installation tokens expire after one hour](https://docs.github.com/apps/creating-github-apps/authenticating-with-a-github-app/generating-an-installation-access-token-for-a-github-app#generating-an-installation-access-token).

The GitHub App used for this is **`dotnet OneLoc Localization`** (owned by `@dotnet-bot`). Its only
job is to open/update the localization check-in PR on your repository.

## How it works

In [`onelocbuild.yml`](/eng/common/core-templates/job/onelocbuild.yml), the App token is minted
whenever `RepoType` is `gitHub`. OneLocBuild supports the **`dnceng/internal`** and
**`DevDiv/DevDiv`** Azure DevOps projects.

When those hold, the job runs [`get-github-app-token.yml`](/eng/common/core-templates/steps/get-github-app-token.yml),
which signs a JWT with the Secret Manager-managed App private key, exchanges it for an installation
token, and passes that token to the OneLocBuild task via `gitHubPatVariable`.

If App token minting or authentication fails, the job fails; there is no stored-PAT fallback.

## Gaining access

"Access" means two separate things, and **both** are required:

1. **The App must be installed on the GitHub org/account that owns your target repo, and your
   specific repository must be selected in that installation.** The App can only open a PR against a
   repository it is installed on. This is what actually grants the App permission to your repo.
2. **Your pipeline must run in `dnceng/internal` or `DevDiv/DevDiv` and be authorized to use its
   project's GitHub App WIF service connection.** The shared OneLoc job uses that identity to read
   only the two required Secret Manager projections from Key Vault.

The .NET Engineering Services team manages the App credentials, Key Vault permissions, and
service connections.

### Step 1 — Request that your repository be added to the App installation

The App installation and backing Secret Manager values are managed by the .NET Engineering
Services (dnceng) team. To have your repo added:

1. Identify the **GitHub org** and **repository** your OneLoc check-in PR targets. For most repos
   this is the value of the `GitHubOrg` parameter (default `dotnet`) and your repo name. If you use
   a mirrored repository, it's the `GitHubOrg`/`MirrorRepo` the PR is opened against — **not** the
   Azure DevOps mirror.
2. Reach out to the **First Responders**
   [channel](https://teams.microsoft.com/l/channel/19%3Aafba3d1545dd45d7b79f34c1821f6055%40thread.skype/First%20Responders?groupId=4d73664c-9f2f-450d-82a5-c2f02756606d&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47)
   and ask them to add your repository to the **`dotnet OneLoc Localization`** GitHub App
   installation for the appropriate org.
3. The App must have permission to open pull requests (Contents + Pull requests: read & write) on
   the selected repository. dnceng configures this as part of the installation.

> **Note:** The App is installed per GitHub organization. If your repo lives in an org where the App
> is not yet installed, dnceng will need to install and approve it in that org first, which may
> require an org owner's approval.

### Step 2 — Use the default App path

Once your repo is part of the App installation, no authentication parameter is required in the
OneLocBuild template call. For example:

```yaml
- ${{ if eq(variables['Build.SourceBranch'], 'refs/heads/main') }}:
  - template: /eng/common/templates/job/onelocbuild.yml
    parameters:
      LclSource: lclFilesfromPackage
      LclPackageId: 'LCL-JUNO-PROD-YOURREPO'
```

The project-specific WIF service connection reads the App ID and private key directly from
EngKeyVault:

| Azure DevOps project | Service connection |
|---|---|
| `dnceng/internal` | `dnceng-oneloc-githubapp` |
| `DevDiv/DevDiv` | `devdiv-oneloc-githubapp` |

Each identity has `Key Vault Secrets User` access scoped to only
`oneloc-localization-app-app-id` and `oneloc-localization-app-app-private-key`. Each pipeline must
be authorized to use its project's service connection.

### GitHub App parameters

| **Parameter** | **Default** | **Notes** |
|:-:|:-:|-|
| `GitHubAppServiceConnection` | `dnceng-oneloc-githubapp` | WIF service connection used to read the App credentials. DevDiv automatically selects `devdiv-oneloc-githubapp` when this default is unchanged. |
| `GitHubAppKeyVaultName` | `EngKeyVault` | Key Vault containing the Secret Manager projections. |
| `GitHubAppIdSecretName` | `oneloc-localization-app-app-id` | Secret containing the GitHub App ID. |
| `GitHubAppPrivateKeySecretName` | `oneloc-localization-app-app-private-key` | Secret containing the PEM private key. |

The token is minted for the installation on the `GitHubOrg` account (default `dotnet`), so make sure
`GitHubOrg` (and `MirrorRepo`, if mirroring) point at the org/repo where the App is installed.

### Migrating from Key Vault RSA signing

The Key Vault RSA signing path and pipeline-variable credential path have both been removed.

OneLoc job callers using `GitHubAppId` and `GitHubAppPrivateKey` must remove those parameters.
Callers from the older RSA interface must remove `GitHubAppClientId` and `GitHubAppKeyName`;
`GitHubAppServiceConnection` and `GitHubAppKeyVaultName` retain their meanings. Standard callers
need no replacement parameters because the four service-connection and secret-name defaults apply
automatically.

Direct callers of `get-github-app-token.yml` using `appId` and `appPrivateKey` must replace those
parameters with `azureSubscription`, `keyVaultName`, `appIdSecretName`, and
`appPrivateKeySecretName`. Direct callers from the older RSA interface keep `azureSubscription`
and `keyVaultName`, remove `keyName` and `appClientId`, and add the two secret-name parameters.
There is no fallback to the legacy RSA key.

## Verifying it works

Changes to the GitHub App credential retrieval path must pass a protected internal canary before
merge. Public pull-request builds intentionally cannot access the WIF service connection or App
private key, so syntax and unit checks alone do not validate this boundary.

1. Run your pipeline from a branch where the OneLocBuild job runs.
2. In the build, confirm the **`Get GitHub App installation token`** step runs and succeeds before
   the `OneLocBuild` task.
3. Confirm the check-in PR is opened by the **`dotnet OneLoc Localization`** App (the PR author will
   be the App / its bot identity).

## Troubleshooting

- **The App-token step is skipped.** The App path activates when `RepoType` is `gitHub`.
- **The App ID or private key cannot be read.** Confirm the pipeline is authorized to use its
  project's GitHub App service connection and that its identity has `Key Vault Secrets User`
  access to both configured secrets.
- **`404`/`Not Found` when requesting the installation token.** The App is not installed on the
  `GitHubOrg` account, or your repository was not selected in the installation. Complete Step 1.
- **PR fails to open on your repo.** Ensure the App has `Contents` and `Pull requests` (read &
  write) permission on the selected repository, and that your repo is included in the installation.

## Scope and limitations

- Arcade supports OneLocBuild in **`dnceng/internal`** and **`DevDiv/DevDiv`**.
