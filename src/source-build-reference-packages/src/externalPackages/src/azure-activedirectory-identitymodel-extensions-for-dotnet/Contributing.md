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

### Contributor License agreement

Please visit [https://cla.microsoft.com/](https://cla.microsoft.com/) and sign the Contributor License
Agreement.  You only need to do that once. We can not look at your code until you've submitted this request.

### Finding an issue to work on

Over the years we've seen many PRs targeting areas of the code which are not urgent or critical for us to address, or areas which we didn't plan to expand further at the time. In all these cases we had to say no to those PRs and close them. That, obviously, is not a great outcome for us. And it's especially bad for the contributor, as they've spent a lot of effort preparing the change. To resolve this problem, we've decided to separate a bucket of issues, which would be great candidates for community members to contribute to. We mark these issues with the help wanted label. You can find all these issues [here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues?q=is%3Aopen+is%3Aissue+label%3A%22help+wanted%22+label%3A%22good+first+issue%22+).

With that said, we have additionally marked issues that are good candidates for first-time contributors. Those do not require too much familiarity with the authN/authZ and are more novice-friendly. Those are marked with the good first issue label.

If you would like to make a contribution to an area not captured [here](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues?q=is%3Aopen+is%3Aissue+label%3A%22help+wanted%22+label%3A%22good+first+issue%22+), first open an issue with a description of the change you would like to make and the problem it solves so it can be discussed before a pull request is submitted.

If you are working on an involved feature, please file a design proposal, more instructions can be found below, under [Before writing code](#before-writing-code).

### Before writing code

We've seen PRs, where customers would solve an issue in a way which either wouldn't fit into the E2E design because of how it's implemented, or it would change the E2E in a way, which is not something we'd like to do. To avoid these situations and potentially save you a lot of time, we encourage customers to discuss the preferred design with the team first. To do so, file a new design proposal issue, link to the issue you'd like to address, and provide detailed information about how you'd like to solve a specific problem.

To file a design proposal, look for the relevant issue in the `New issue` page or simply click [proposal for Identity Model](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet/issues/new?assignees=&labels=design-proposal&projects=&template=design_proposal.md).

### Identifying scale

If you would like to contribute to one of our repositories, first identify the scale of what you would like to contribute. If it is small (grammar/spelling or a bug fix) feel free to start working on a fix. If you are submitting a feature or substantial code contribution, please discuss it with the team and ensure it follows the product roadmap. You might also read these two blogs posts on contributing code: [Open Source Contribution Etiquette by Miguel de Icaza](http://tirania.org/blog/archive/2010/Dec-31.html) and [Don't "Push" Your Pull Requests by Ilya Grigorik](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/). All code submissions will be rigorously reviewed and tested further by the team, and only those that meet an extremely high bar for both quality and design/roadmap appropriateness will be merged into the source.

### Before submitting the pull request

Before submitting a pull request, make sure that it checks the following requirements:

- You find an existing issue with the "help-wanted" label or discuss with the team to agree on adding a new issue with that label.
- You post a high-level description of how it will be implemented and receive a positive acknowledgement from the team before getting too committed to the approach or investing too much effort in implementing it.
- You add test coverage following existing patterns within the codebase
- Your code matches the existing syntax conventions within the codebase
- Your PR is small, focused, and avoids making unrelated changes

If your pull request contains any of the below, it's less likely to be merged.

- Changes that break backward compatibility
- Changes that are only wanted by one person/company. Changes need to benefit a large enough proportion of developers using our auth libraries.
- Changes that add entirely new feature areas without prior agreement
- Changes that are mostly about refactoring existing code or code style

Very large PRs that would take hours to review (remember, we're trying to help lots of people at once). For larger work areas, please discuss with us to find ways of breaking it down into smaller, incremental pieces that can go into separate PRs.

### During pull request review

A core contributor will review your pull request and provide feedback. To ensure that there is not a large backlog of inactive PRs, the pull request will be marked as stale after two weeks of no activity. After another four days, it will be closed.

### Submitting a pull request

If you're not sure how to create a pull request, read this article: https://help.github.com/articles/using-pull-requests. Make sure the repository can build and all tests pass. Familiarize yourself with the project workflow and our coding conventions. The coding, style, and general engineering guidelines are published on the Engineering guidelines page.

Your pull request should:

* Include a description of what your change intends to do
* Be based on a reasonably recent pull in the correct branch
  * Please rebase and squash all commits into a single one
* Pass all tests
* Have clear commit messages
* Include new tests for new features
* To avoid line ending issues, set `autocrlf = input` and `whitespace = cr-at-eol` in your git configuration

### Tests

- Tests need to be provided for every bug/feature that is completed.
  - Unit tests cover all new aspects of the code.
- Before and after performance and stress tests results are evaluated (no regressions allowed).
- Performance and stress tests are extended as relevant.

### Feedback

Your pull request will now go through extensive checks by the subject matter experts on our team. Please be patient; we have hundreds of pull requests across all of our repositories. Update your pull request according to feedback until it is approved by one of the team members.

### Merging pull requests

When your pull request has had all feedback addressed, it has been signed off by one or more reviewers with commit access, and all checks are green, we will commit it.
We commit pull requests as a single Squash commit unless there are special circumstances. This creates a simpler history than a Merge or Rebase commit. "Special circumstances" are rare, and typically mean that there are a series of cleanly separated changes that will be too hard to understand if squashed together, or for some reason we want to preserve the ability to dissect them.

## Building and running tests

To build and run tests, use 'build'

```Shell
>build
```

Build and test failures will appear in red in the console window.
