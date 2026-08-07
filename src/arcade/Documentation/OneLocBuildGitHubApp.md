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

The App path is enabled by default. In
[`onelocbuild.yml`](/eng/common/core-templates/job/onelocbuild.yml), the App token is minted only
when **all** of the following are true:

- `UseGitHubAppAuthentication` is `true` (the default), **and**
- `RepoType` is `gitHub`, **and**
- the build is running in the **`internal`** Azure DevOps project (the App service connection and
  Key Vault key are scoped to `dnceng/internal`).

When those hold, the job runs [`get-github-app-token.yml`](/eng/common/core-templates/steps/get-github-app-token.yml),
which signs a JWT with the App's RSA key in Key Vault, exchanges it for an installation token, and
passes that token to the OneLocBuild task via `gitHubPatVariable`.

If `UseGitHubAppAuthentication` is explicitly set to `false` — or the build runs in any project
other than `internal` (e.g. `DevDiv`, `public`) — the job uses the existing `GithubPat` parameter.
This is a template-selection fallback only: if App token minting or authentication fails after the
App path is selected, the job fails and does not retry with the PAT.

## Gaining access

"Access" means two separate things, and **both** are required:

1. **The App must be installed on the GitHub org/account that owns your target repo, and your
   specific repository must be selected in that installation.** The App can only open a PR against a
   repository it is installed on. This is what actually grants the App permission to your repo.
2. **Your pipeline must use the default App path** by leaving `UseGitHubAppAuthentication` set to
   `true` in the `onelocbuild.yml` template call.

### Step 1 — Request that your repository be added to the App installation

The App installation and the backing `dnceng/internal` service connection / Key Vault key are
managed by the .NET Engineering Services (dnceng) team. To have your repo added:

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

The dnceng service connection, App client ID, Key Vault, and key name are centralized as defaults in
the Arcade template. They can be overridden for separately provisioned infrastructure. A pipeline
can temporarily set `UseGitHubAppAuthentication: false` to select the PAT path instead.

### GitHub App parameters

| **Parameter** | **Default** | **Notes** |
|:-:|:-:|-|
| `UseGitHubAppAuthentication` | `true` | Activates the App path for GitHub repos in `dnceng/internal`. Set to `false` to select the PAT path. |
| `GitHubAppServiceConnection` | `'dnceng-oneloc-githubapp'` | The Azure DevOps **WIF service connection** whose identity has `Sign` permission on the App's Key Vault key. |
| `GitHubAppClientId` | `'Iv23lijBU8x3gc9lDOc9'` | The GitHub App's **Client ID** (used as the JWT `iss` claim). |
| `GitHubAppKeyVaultName` | `'EngKeyVault'` | The Key Vault holding the App's RSA signing key. |
| `GitHubAppKeyName` | `'oneloc-localization-app-key'` | The name of the RSA key inside that Key Vault (the App's private key). |

The token is minted for the installation on the `GitHubOrg` account (default `dotnet`), so make sure
`GitHubOrg` (and `MirrorRepo`, if mirroring) point at the org/repo where the App is installed.

## Verifying it works

1. Run your pipeline (on the `internal` project) from a branch where the OneLocBuild job runs.
2. In the build, confirm the **`Get GitHub App installation token`** step runs and succeeds before
   the `OneLocBuild` task.
3. Confirm the check-in PR is opened by the **`dotnet OneLoc Localization`** App (the PR author will
   be the App / its bot identity).

## Troubleshooting

- **The App-token step is skipped.** The App path only activates when
  `UseGitHubAppAuthentication` is `true`, `RepoType` is `gitHub`, and the build runs in the
  `internal` project. Verify all three.
- **Token minting fails with a Key Vault authorization error.** The service connection identity
  needs the `Key Vault Crypto User` role (or at least the `Sign` action) on the App's key. Contact
  First Responders.
- **`404`/`Not Found` when requesting the installation token.** The App is not installed on the
  `GitHubOrg` account, or your repository was not selected in the installation. Complete Step 1.
- **PR fails to open on your repo.** Ensure the App has `Contents` and `Pull requests` (read &
  write) permission on the selected repository, and that your repo is included in the installation.

## Scope and limitations

- The App path is only available in the **`dnceng/internal`** Azure DevOps project. Pipelines in
  other projects use `GithubPat` and are not covered by the `dnceng-oneloc-githubapp` service
  connection. DevDiv can use the same template path after a DevDiv-scoped service connection and
  signing-key access are provisioned.
