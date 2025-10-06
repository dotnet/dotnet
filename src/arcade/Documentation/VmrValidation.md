# VMR Validation in repo-level PRs

It is often important to verify repo-level changes in the context of the VMR. This is possible using
a default YML template `eng\common\templates\vmr-build-pr.yml`.

Work with "First Responders" on creation of the new public Azure pipeline, naming it like `<repo>-unified-build`
Pipeline should be non-production. Ask for enabling of manual trigger using a GitHub comment.

## Customizing build jobs

The pipeline will run a full set of validation jobs defined in:
https://github.com/dotnet/dotnet/blob/10060d128e3f470e77265f8490f5e4f72dae738e/eng/pipelines/templates/stages/vmr-build.yml#L27-L38

For repos that do not need to run the full set, you would do the following:

1. Copy this YML file to a repo-specific location, i.e. outside of eng/common.

2. Add `verifications` parameter to VMR template reference

   Examples:
   - For source-build stage 1 verification, add the following:
       verifications: [ "source-build-stage1" ]

   - For Windows only verifications, add the following:
       verifications: [ "unified-build-windows-x64", "unified-build-windows-x86" ]

## Manual run

To run the pipeline from a PR, add the following PR comment: `/azp run <pipeline-name>`

## Further customization

To get pipeline to run automatically, update the `pr` trigger in the YML.

To make this pipeline a required PR check, update the GitHub PR settings for your repo.
