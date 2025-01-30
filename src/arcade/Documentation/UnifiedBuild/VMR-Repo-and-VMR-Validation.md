# The Unified Build Almanac (TUBA) - Repository and VMR PR/CI validation Strategy

## Purpose

The purpose of this document is to lay out a strategy for:
- Validating code flow into the VMR from consituent repositories.
- Validating changes made directly in the VMR.
- Validating constituent repository changes so that they are less likely to break in the VMR.

## Principles

- Repositories have invested heavily in repo-specific testing over the life of .NET. This validation must not be compromised.
- Validation failures that block PR merge should be actionable.

## Current State

Broadly, the following validation currently takes place:
- Repositories that participate in source build (i.e. this would exclude windowsdesktop, winforms, etc.) have a repo source-build job that runs on PRs and CI. This job performs an *approximation* of the source-only build that would take place in the VMR when the code reaches the SDK. This job **is** valuable at times. It is helpful in catching new prebuilts introduced by repository changes as well as some set of build differences that appear when `DotNetBuildSourceOnly == true`. This is especially important because the time betweeen a repo check-in and VMR/source-build insertion has historically been quite long. It is important to detect issues early. However, this job is also fragile:
  - The same prebuilt detection will catch package restores that would be provided by VMR built package flow (e.g. runtime->roslyn) or previously source built artifacts, requiring costly analysis and baseline updates.
  - The TFM targeted in a full VMR build often ends up different than the repo source build leg.
  - The toolset used may be different between the VMR and the isolated repository, leading to unexpected errors.
- Repositories maintain CI and PR workflows that run repo-level testing (unit tests, scenario tests, etc.) that has been built up over years.
- The sdk repo (currently the source of VMR codeflow) runs a variety of VMR builds on every check-in against main, in source-only and MSFT configurations.
- A set of scenario tests run against the VMR output in the VMR builds in the SDK. These scenario tests are designed to cover a broad swath of functionality against a finished product, rather than a repository build layout.
- Teams run manual validation on staged artifacts.

## Proposed Validation

### Source-build legs are replaced by optional full-VMR validation

- We will introduce the ability to run optional test VMR insertions on each PR for repos that participate in a VMR. This means that the code changes in the PR will be built in context of a full product build. Note: Because the validation may reveal issues that require downstream reaction and are not actionable before merge, these legs must be optional. They are informational and can't be used as a gate
- The existing repo source-build legs will be removed. The short hop between repository and VMR insertion means that detecting source-only build issues as early as possible is not as critical as before. This has the following knock-on effects:
  - Existing offical build source-build legs will be removed.
  - The source build intermediate architecuture can be removed.

**Scheduling: Can be implemented now.**
 
### Optional full-VMR validation is added for MSFT configurations

We will introduce the ability to trigger optional full VMR insertion validation on PRs. These will run a desired subset of the MSFT/source-only VMR builds and validation. We believe this is a net-net win overall for the product. If there are failures on the PR, the developer who submitted the PR or team who is responsible for that PR (e.g. in case of a dependabot PR) would interpret the results and decide what to do. This might mean checking in and dealing with the failure on VMR insertion, changing the PR to avoid the issue, or making a change directly in the VMR.

**Scheduling: Can be implemented now.**

### Repositories retain their existing repo-specific validation but may tweak if necessary

Aside from source-build jobs, existing repository PR/CI validation will not be removed or altered. Existing testing setups can remain as-is. Most repositories build extremely similarly between current official builds and VMR component builds. However, we may identify areas where the PR testing setup is not representative of the VMR build setup and these differences allow product breaks to flow through. This is very similar to PR/Official Build differences today. When such gaps are identified for a repository, we will remedy like any other testing gap. This is the responsibility of the product teams.

**Scheduling: NA, NOP**

### VMR PR/CI builds a selected subset of verticals and scenarios

On insertion into a VMR (either at PR or CI time), we will run a selected representative subset of scenarios. This representative subset may change over time as the product changes, however, a rough sketch may look like:
- Some set of source-only legs w/ scenario testing
- A full-stack matrix that sparsely covers all OSs and architectures w/ scenario testing.
- Some representative set of short stacks (e.g at least one of each OS and at least one of each arch) w/ scenario testing.
- Some set of builds that build repo test projects, some set of builds which exclude repo test projects.
- Change validation to ensure that special files are not edited (e.g. inlined submodules or automation generated files). See [Add VMR change validation](https://github.com/dotnet/arcade-services/issues/2950)
- "Freshness" testing described in https://github.com/dotnet/arcade/issues/15180.

**Scheduling: Ready, Completed (initial set of PR build validation)**

### VMR PR/CI does not run repo-specific validation by default

It will be challenging to mimic the repo-specific testing setups in VMR PR/CI builds. In addition, the quantity of testing is unlikely to be practical for full VMR validation. Running repo unit tests in VMR PRs is not a goal of Unified Build.

**Scheduling: NA, NOP**

### (Potential Future Improvement) VMR PRs have specialized repo specific pipelines

If need arises, e.g. frequent breaks or reduced developer productivity in the VMR, the following system could be implemented to give repo-specific validation:
  - We implement on-demand VMR PR pipelines - 1 per repo in the VMR - to enable additional validation when requested.
  - Each of these would be based on the same YAML file from the VMR and would contain an ID of a target repo pipeline (e.g. dotnet-runtime) which exists and runs in the original repo.
  - This VMR pipeline would trigger the target pipeline, pass to it the VMR SHA.
  - The repo pipeline would have a special checkout step that would run when the pipeline is triggered remotely like this
  - The checkout step replaces the checked out sources with the contents from the VMR and the build proceeds as usual.
  - The pipeline runs some set of validation (e.g. build and unit tests) as defined by the repo owner.

**Scheduling: Long Term Improvement, if needed**

### VMR changes lean heavily on scenario testing for correctness validation

VMR PR/CI will lean heavily on scenario testing for correctness validation. These scenario tests are designed to cover large swaths of product functionality, rather than focusing on a specific component. Where necessary, scenario test quality will be increased. There are certainly gaps today that need filling.

**Scheduling: Already running but need additional tests to be added over time.**

### Backflow + Existing repo CI/PR validation is used for correctness validation

In-depth product validation and signoff will be done via code backflow (packages and code from the VMR to component repositories) + existing PR/CI validation at the repo level. In reality, this is no change from practices today. This is similar to the way official builds (which do not generally run tests) + rolling CI validation support signoff today for repositories that cannot run all of their testing in PRs.

Repo owners will also use VMR artifacts for manual validation and signoff where appropriate. This is no different from validating a staged build.

**Scheduling: NA, NOP**
