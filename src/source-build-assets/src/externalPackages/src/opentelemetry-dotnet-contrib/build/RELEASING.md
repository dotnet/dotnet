
# Release Process

Releasing of projects from the opentelemetry-dotnet-contrib repo is typically a
fully automated process.

## Requesting a release

Anyone may request the release of a component by using the [Release
request](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/issues/new?assignees=&labels=release&projects=&template=release_request.yml&title=%5Brelease+request%5D+)
issue template.

* Select the component you want released from the dropdown.

  > [!NOTE]
  > Most components only have a single entry which makes this selection simple.
    However some components, for example AspNet instrumentation, are really
    multiple components released together. For the multi-component case, pick
    any component from the group in the dropdown. All components in a group will
    be released together regardless of which is selected.

* Specify the version for the release.

  > [!NOTE]
  > Any version may be specified when creating the release request issue but the
    release will not be initiated until the version complies with the
    `^(\d+\.\d+\.\d+)(?:-((?:alpha)|(?:beta)|(?:rc))\.(\d+))?$` regular
    expression.

* Specify any additional context for why the release is being requested.

* Create the release request issue.

  * If you are an approver or a maintainer of the repo, the release request will
    automatically be approved and the automation will proceed to create the
    "Prepare release" pull request. See [Working with the release
    automation](#working-with-the-release-automation) to continue.

    > [!NOTE]
    > Approvers and maintainers may skip the release request issue and use the
      [GitHub Actions
      UI](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/actions/workflows/prepare-release.yml)
      to run the [prepare-release
      workflow](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/.github/workflows/prepare-release.yml)
      directly.

  * If you are a [component
    owner](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/.github/component_owners.yml)
    for the component requested, the release request will automatically be
    approved and the automation will proceed to create the "Prepare release"
    pull request. See [Working with the release
    automation](#working-with-the-release-automation) to continue.

  * If neither of the above two cases are true for you, then the release request
    issue you created will need to be approved by a component owner, approver,
    or maintainer. They should automatically be tagged/mentioned on your issue
    by the automation.

## Working with the release automation

The release request issue created using the [Requesting a
release](#requesting-a-release) flow above is used to trigger the
[prepare-release
workflow](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/main/.github/workflows/prepare-release.yml)
which will create a pull request to begin the automation flow for all of the
book-keeping/routine tasks needed to be done in the release process:

> [!NOTE]
> Once the "Prepare release" pull request is opened the job of users and
  component owners is complete. Only approvers and maintainers of the repo can
  proceed from this point.

Pre-build:

* `CHANGELOG` updates.
* Public API file (`.publicApi` folder) updates (for stable releases only).

Post-build:

* NuGet push.
* GitHub release creation.
* `PackageValidationBaselineVersion` updates (for stable releases only).

<details>
<summary>Instructions for approvers and maintainers</summary>

1. Review and merge the opened "Prepare release" pull request.

2. Post a comment with "/CreateReleaseTag" in the body. This will tell the
   [Prepare for a
   release](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/actions/workflows/prepare-release.yml)
   workflow to push the tag for the merge commit of the PR which will trigger
   the [Build, pack, and
   publish](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/actions/workflows/publish-packages.yml)
   workflow.

3. Wait for the [Build, pack, and
   publish](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/actions/workflows/publish-packages.yml)
   workflow to complete. When complete a trigger will automatically add a
   comment on the "Prepare Release" PR with a link to the package artifacts and
   GitHub release.

4. If a new stable version of the component was released, a PR should have been
   automatically created by the [Complete
   release](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/actions/workflows/post-release.yml)
   workflow to update the `PackageValidationBaselineVersion` property in project
   files (`.csproj`) to the just released stable version. Merge that PR once the
   build passes (this requires the packages be available on NuGet).

</details>

## Manual process

<details>
<summary>Instructions for preparing for a release manually</summary>

## Pre-requisites

To carry out the release successfully, make sure you fulfill the following:

1. You must be a maintainer on the repo. This will allow you to push tags to
`main` branch and create a GitHub release later on.

2. Have access to the NuGet token for releasing the project to NuGet.org
OpenTelemetry account.

## Pre-steps

1. Decide on the version to use for the release.

2. Update the `CHANGELOG.md` file for your project with relevant details.
Replace any "Unreleased" heading with this version and add the release date.

3. If you are releasing stable version, update public API definition in
`PublicAPI.Shipped.txt` files and cleanup corresponding
`PublicAPI.Unshipped.txt`.

4. Submit a PR to update `CHANGELOG.md` and get it merged to `main` branch.

## Steps

*Note:* Before starting with the following steps, ensure that the latest commit
on the `main` branch is the one which added/updated the `CHANGELOG.md` to the
project being released. *This latest commit will be tagged on the release.*

1. Create and push git tag for the project and the version of the project you
   want to release. The version should be the one used in the **Pre-steps** to
   update the Changelog.

    Note: In the below examples `git push origin` is used. If running in a fork,
    add the main repo as `upstream` and use `git push upstream` instead. Pushing
    a tag to `origin` in a fork pushes the tag to the fork.

    ```powershell
    git tag -a <PROJECT TAG PREFIX>-<VERSION> -m "<PROJECT TAG PREFIX>-<VERSION>"
    ```

    You can find the project tag prefix in the `.csproj` file for the project.
    Look for value of the `<MinVerTagPrefix>` tag.
    See the example [here](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/blob/22f2eb1026162510571241eae1eb6c2952146ace/src/OpenTelemetry.Contrib.Instrumentation.AWS/OpenTelemetry.Contrib.Instrumentation.AWS.csproj#L6).

    ```powershell
    git push origin <PROJECT TAG PREFIX>-<VERSION>
    ```

    **Example:**

    ```powershell
    git tag -a Instrumentation.AWS-1.0.0 -m "Instrumentation.AWS-1.0.0"
    git push origin Instrumentation.AWS-1.0.0
    ```

    **Note:** If you are releasing more than one inter-dependent projects
    that need to go out together, you should create and push all the relevant
    tags together. This will ensure that [MinVer](https://github.com/adamralph/minver#how-it-works)
    will pick up the version for each project is what you specified in the
    tags instead of calculating the version based on the tag depth in the commit
    history.

    This will trigger the building and packaging workflow for the project.

2. Navigate to the
   [**Actions**](https://github.com/open-telemetry/opentelemetry-dotnet-contrib/actions)
   tab on the repo and monitor the "Pack YOUR_PROJECT_NAME" workflow. The last
   step in the workflow is to publish to NuGet.org and prepare corresponding
   GitHub release page.

3. Validate that the new version (as specified in step 1) of the project is
   successfully published to NuGet.org under OpenTelemetry owner.

4. Validate that the new version was published in GitHub.

5. If you released stable package, update `PackageValidationBaselineVersion`
   in corresponding `csproj` file.

</details>
