# Running the Staging Pipeline

Running DotNet-Stage may seem intimidating at first, but fear not! 
It's only a little bit complicated.

## Parameters

The actual part you need to understand before running is the pipeline parameters. Here's a full description of them:

1. **Bar Build Ids** &ndash; a comma-separated list of BAR IDs to use as the basis for the release. These should be installer BAR IDs.
2. **Treat build as...** &ndash; optionally override project as public or internal. If you know you want it to publish to public feeds/containers, you choose public; if you want to publish to internal feeds/containers, you choose internal. Normally, "default" should be selected.
3. **Security Release** &ndash; should be checked if this is a security release.
4. **Release Date** &ndash; corresponds to date of actual release (not today's date).
5. **CVE List** &ndash; if Security Release is checked, this should be filled out with the relevant CVEs fixed by the release. CVEs should be listed in the format `CVE-<YEAR>-<ID>`.
6. **Certificate Substitutions** &ndash; not necessary in most cases. If you want to sign Windows files with a different certificate, you specify "{old certificate}={new certificate}" in this box to replace {old certificate} with {new certificate}.
7. **Always Download Asset List** &ndash; used for partial releases. Makes sure that we download that file which is required for releases. Leave as default for most cases.
8. **Skip Publish to vsufile** &ndash; only used for testing purposes.

If you want to only run up to a certain stage, you can disable all later stages using the **Stages to run** feature of Azure Pipelines.

## Failures

There are several manual overrides for stages which are allowed to have failures &ndash; these stages require approval if the stage they're related to had any failures. For information on common causes of failure in these stages, check our [validation documentation](https://github.com/dotnet/arcade/blob/main/Documentation/Validation.md#what-do-i-do-if-an-issue-is-opened-in-my-repository).

## Branching for Arcade Release

Dotnet-Release should be branched on the same cadence as Arcade. Check the [Arcade Servicing doc](https://github.com/dotnet/arcade/blob/main/Documentation/Policy/ArcadeServicing.md) for more information on when branching occurs.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CStaging-Pipeline%5Crunning-the-pipeline.md)](https://helix.dot.net/f/p/5?p=Documentation%5CStaging-Pipeline%5Crunning-the-pipeline.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CStaging-Pipeline%5Crunning-the-pipeline.md)</sub>
<!-- End Generated Content-->
