# Contributing

## Branch Structure

* **dev**: The dev working branch of master, which is 6.x
* **dev5x**: The dev working branch of master for 5.x
* **rel/version**: Release tracking branch for each new release

If you want to track a branch for a specific release, use the [tag branches](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/tags).

## Filing Bugs

Please file issues you see in the [issue tracker](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues). Include:

* The version you're using.
* The behavior you're seeing. If at all possible, please submit a reduced repro or test that demonstrates the issue.
* What you expect to see.

## Instructions for Contributing Code

### Contributing bug fixes

We accept bug fixes. A bug must have an issue tracking it in the issue tracker. Please link the issue and PR.

### Contributing features

Features (things that add new or improved functionality) may be accepted, but will need to first be approved (tagged with "Enhancement") in the issue.

## Legal

You will need to complete a Contributor License Agreement (CLA). Briefly, this agreement testifies that you are granting us permission to use the submitted change according to the terms of the project's license, and that the work being submitted is under appropriate copyright.

Please submit a Contributor License Agreement (CLA) before submitting a pull request. You may visit https://cla.microsoft.com to sign digitally. Once we have received the signed CLA, we'll review the request.

## Housekeeping

Your pull request should:

* Include a description of what your change intends to do
* Be based on a reasonably recent pull in the correct branch
  * Please rebase and squash all commits into a single one
* Pass all tests
* Have clear commit messages
* Include new tests for new features
* To avoid line ending issues, set `autocrlf = input` and `whitespace = cr-at-eol` in your git configuration

## Building and running tests

To build and run tests, use 'build'

```Shell
>build
```

Build and test failures will appear in red in the console window.
