# Localization with OneLocBuild in Arcade

As of April 1, 2021, all .NET repositories will be using OneLocBuild for localization. Documentation on this system can
be found [here](https://ceapex.visualstudio.com/CEINTL/_wiki/wikis/CEINTL.wiki/107/Localization-with-OneLocBuild-Task).
This system is **not a replacement for Xliff-Tasks**; rather, it is replacing the localization team's old system called
Simple Loc. OneLocBuild coordinates getting translations for new and updated strings and merging them back into the
repo. Xliff-Tasks will continue to be used in addition to OneLocBuild.

To make OneLocBuild easier to use, we have integrated the task into Arcade. This integration is a job template
([here](/eng/common/templates/job/onelocbuild.yml)) that is described in this document.

To see your repo's current loc configuration, please refer to https://aka.ms/locstats.

## Onboarding to OneLocBuild Using Arcade

Onboarding to OneLocBuild is a simple process:

1. Ensure that your repository is on the latest version of Arcade.
2. Create a test branch (e.g. `LocalizationTests`) in your repository and add the following job template to your YAML:
```yaml
- template: /eng/common/templates/job/onelocbuild.yml
  parameters:
    CreatePr: false
```
Note: If you are running your PR builds and official builds off of the same definition and are on dnceng,
you will want to conditionalize this step with the following:
```yaml
- ${{ if and(ne(variables['System.TeamProject'], 'public'), notin(variables['Build.Reason'], 'PullRequest')) }}:
```
To prevent OneLocBuild from running in the public project where it will fail.

3. Run the pipeline you want to use OneLocBuild on your test branch.
4. Open a ticket with the localization team using
   [this template](https://aka.ms/ceChangeLocConfig).
   Include the link to the test build you've done.
5. The loc team will generate an LCL package for you and send you its ID. It will be something like
   `LCL-JUNO-PROD-YOURREPO`.
6. Change your YAML (subbing `'LCL-JUNO-PROD-YOURREPO'` for the package ID given to you) to:
```yaml
- ${{ if eq(variables['Build.SourceBranch'], 'refs/heads/main') }}:
  - template: /eng/common/templates/job/onelocbuild.yml
    parameters:
      LclSource: lclFilesfromPackage
      LclPackageId: 'LCL-JUNO-PROD-YOURREPO'
```
Make sure to remove the `CreatePr: false` line from step 2. Additionally, if you added the YAML condition from step
2, make sure that your new YAML condition now looks like:
```yaml
- ${{ if and(ne(variables['System.TeamProject'], 'public'), notin(variables['Build.Reason'], 'PullRequest'), eq(variables['Build.SourceBranch'], 'refs/heads/main')) }}:
```

7. If using a mirrored repository (your code is mirrored to a trusted repository which your official build uses),
   add the following parameter to your YAML (subbing e.g. `sdk` for the value):
```yaml
    MirrorRepo: name of your GitHub repository, e.g. 'sdk'
```
This naming might be confusing for repositories using code mirroring through Maestro, as typically the
"mirror repository" refers to the trusted Azure DevOps repository our GitHub repositories mirror to.
In this case, however, it refers to the repository that _mirrors to_ the Azure DevOps repository
that the official build is based on.

As a further note, the template by default assumes that your mirror repository is located in the dotnet GitHub
organization. If that is not the case, you will need to specify `GitHubOrg` as well.

8. Merge the changes to your main branch and then open a
   [repo modification ticket](https://aka.ms/ceChangeLocConfig)
   with the loc team to let them know to retarget the branch.

## Releasing with OneLocBuild Using Arcade

**Note: The SLA for translations is one week. Please allow at least two weeks from the release for this process.**

### If You're Releasing from `main`
If you're releasing from the main branch of your repository, all that you need to do is ensure that you're merging
PRs from OneLocBuild as they are made and that you allow the translator SLA for any new strings prior to the release.

### If You're Releasing from a Branch Other Than `main` (Including Servicing Branches)
Depending on how often you want to release from the servicing branch you could:
* Change the loc task, that parses your default branch, to temporarily parse resources from the servicing branch. Here is what you would need to do for that:
  1. Add the OneLocBuild job template to the pipeline YAML of the release branch. When you do this, you have to change the YAML of both the main branch and the target branch to include a conditional specifying 
   the target branch rather than main (as above). Additionally, your YAML should include the following line (substituting your target branch for `target-branch`):
   ```yaml
      MirrorBranch: target-branch
   ```
  2. Open a [repo modification ticket](https://aka.ms/ceChangeLocConfig) with the 
   loc team at least two weeks before the release and request that they re-target your repository to the release branch.
  3. Merge the OneLocBuild PRs to your release branch.
  4. After the release, open another repo modification ticket to re-target your repository to the `main` branch again.

* Register a servicing branch with the loc team using [this ticket](https://aka.ms/ceNewLoc) ([Here](https://ceapex.visualstudio.com/CEINTL/_workitems/edit/523494)'s an example). This allows the loc team to parse your servicing branch(es) while your default branch continues to be parsed. If you already have an old servicing branch parsed by the loc team, you could update it to point to a newer servicing branch. Here's what you would need to do to update the branch:
   1. Open a [repo modification ticket](https://aka.ms/ceChangeLocConfig) with the 
   loc team at least two weeks before the release and request that they re-target your servicing branch to new branch.
   2. Ask the loc team for a package Id for this servicing branch on the ticket. The value of the Package ID would then be substituted in the YAML of the OneLocBuild task.
   ```yaml
      LclPackageId: 'LCL-JUNO-PROD-YOURREPOSVC'
   ```
   3. Update the conditional on the OneLocBuild task to point to the right branch and change/add in a MirrorBranch field to the task as well. So if the servicing branch is `dev17.0.x` the task would look like:
   ```yaml
   - ${{ if eq(variables['Build.SourceBranch'], 'refs/heads/dev17.0.x') }}:
     - template: /eng/common/templates/job/onelocbuild.yml
       parameters:
         LclSource: lclFilesfromPackage
         LclPackageId: 'LCL-JUNO-PROD-YOURREPOSVC'
         MirrorBranch: dev17.0.x
   ```

# Common Issues

## Filing Issues for Translation Issues

File a translation issue ticket with the localization team [here](https://aka.ms/ceLocBug).

## Leaving Comments for Translators

Sometimes the proper solution to translation issues is to give the translators context for their work. This can be done easily in RESX files (which will be carried over to the XLF files by xliff-tasks) or in JSON files directly. For more information on how to leave translation comments, see the documentation [here](https://aka.ms/commenting).

# Technical Documentation

## LocProject.json Index File

The core component of OneLocBuild is the LocProject.json file. This file is an index file containing references to
all of the files to localize in your repository. In order to reduce overhead for repo owners, we are auto-generating
the LocProject.json file (PowerShell script [here](/eng/common/generate-locproject.ps1)). This script and the
OneLocBuild Azure DevOps task are both wrapped in the onelocbuild.yml job template.

The script searches your checked-in code for all localized XLF files and template JSON files. Files can be excluded
using a checked in file (/Localize/LocExclusions.json). The LocExclusions file excludes files based on simple matching.
For example, the LocExclusions.json file below will exclude everything in a directory called `tests` and any file
which include `test.xlf` in its name.

```json
{
    "Exclusions": [
        "\\tests\\",
        "test.xlf"
    ]
}
```

The selected files are then added to a generated LocProject.json file. At this point, template currently provides two
options for how to proceed.

### Build-Time Generation

**The recommended path** is to have the script pass the generated LocProject.json directly to the OneLocBuild task.
This is the simpler of the two methods and removes the overhead of needing to maintain a checked in
LocProject.json file. The LocProject.json file is emitted in build logs and as a build artifact for examination.

### Build-Time Validation

While it is **not the recommended path**, repos can instead opt to check in a static LocProject.json and have the
script compare it against the generated one. If they differ, the script will break the build so that a dev can
update either the LocProject.json or the LocExclusions.json file accordingly. The LocProject.json should be checked
placed in the `eng/Localize` directory.

Because the script can be run locally, devs can also do this validation prior to pushing their changes.

### Custom LocProject.json Files

Currently, the LocProject.json generation script only creates fairly uniform LocProject.json files. If your repository
requires the use of any of the more complex LocProject.json features as described in the OneLocBuild docs linked above,
you will need to check in and maintain the LocProject.json file manually. To still use this template for running the task,
set the `SkipLocProjectJsonGeneration` parameter to `true`.

## OneLocBuild Template Parameters

The most basic structure for calling the OneLocBuild template is:

```yaml
jobs:
- ${{ if and(ne(variables['System.TeamProject'], 'public'), notin(variables['Build.Reason'], 'PullRequest'), eq(variables['Build.SourceBranch'], 'refs/heads/main')) }}:
  - template: /eng/common/templates/job/onelocbuild.yml
    parameters:
      LclSource: lclFilesfromPackage
      LclPackageId: 'LCL-PACKAGE-ID'
```

The parameters that can be passed to the template are as follows:

| **Parameter** | **Default Value** | **Notes** |
|:-:|:-:|-|
| `RepoType` | `'gitHub'` | Should be set to `'gitHub'` for GitHub-based repositories and `'azureDevOps'` for Azure DevOps-based ones. |
| `SourcesDirectory` | `$(Build.SourcesDirectory)` | This is the root directory for your repository source code. |
| `CreatePr` | `true` | When set to `true`, instructs the OneLocBuild task to make a PR back to the source repository containing the localized files. |
| `AutoCompletePr` | `false` | When set to `true`, instructs the OneLocBuild task to autocomplete the created PR. Requires permissions to bypass any checks on the main branch. |
| `ReusePr` | `true` | When set to `true`, instructs the OneLocBuild task to update an existing PR (if one exists) rather than open a new one to reduce PR noise. |
| `UseLfLineEndings` | `true` | When set to `true`, instructs the OneLocBuild task to use LF line endings during check-in rather than CRLF. |
| `GitHubOrg` | `'dotnet'` | The GitHub organization to be used when making a PR (only used when using a mirrored repository). |
| `MirrorRepo` | `''` | The name of the GitHub repository to make a PR to (only used when using a mirrored repository). |
| `MirrorBranch` | `'main'` | The branch on GitHub to make a PR to (only used when using a mirrored repository). |
| `UseCheckedInLocProjectJson` | `false` | When set to `true`, instructs the LocProject.json generation script to use build-time validation rather than build-time generation, as described above. |
| `SkipLocProjectJsonGeneration` | `false` | When set to `true`, skips the LocProject.json generation in favor of using a checked-in LocProject.json.
| `LanguageSet` | `VS_Main_Languages` | This defines the `LanguageSet` of the LocProject.json as described in the [OneLocBuild task documentation](https://ceapex.visualstudio.com/CEINTL/_wiki/wikis/CEINTL.wiki/107/Localization-with-OneLocBuild-Task?anchor=languageset%2C-languages-(required)). |
| `LclSource` | `LclFilesInRepo` | This passes the `LclSource` input to the OneLocBuild task as described in [its documentation](https://ceapex.visualstudio.com/CEINTL/_wiki/wikis/CEINTL.wiki/107/Localization-with-OneLocBuild-Task?anchor=languageset%2C-languages-(required)). For most repos, this should be set to `LclFilesfromPackage`. |
| `LclPackageId` | `''` | When `LclSource` is set to `LclFilesfromPackage`, this passes in the package ID as described in the [OneLocBuild task documentation](https://ceapex.visualstudio.com/CEINTL/_wiki/wikis/CEINTL.wiki/107/Localization-with-OneLocBuild-Task?anchor=scenario-2%3A-lcl-files-from-a-package). |
| `condition` | `''` | Allows for conditionalizing the template's steps on build-time variables. |
| `JobNameSuffix` | `''` | Allows for custom job name suffix. This is helpful for disambiguation in case of need for more then one OneLocBuild job run - e.g. as a way to set multiple package IDs. |

It is recommended that you set `LclSource` and `LclPackageId` as shown in the example above.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5COneLocBuild.md)](https://helix.dot.net/f/p/5?p=Documentation%5COneLocBuild.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5COneLocBuild.md)</sub>
<!-- End Generated Content-->
