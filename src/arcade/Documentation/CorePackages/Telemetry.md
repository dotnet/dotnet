# Telemetry Scripts

### What is it?

A pair of scripts that can be used to send telemetry information so that you'll be able to see the status of phases of your build on the [Mission Control](http://mc.dot.net/) website. Essentially, you have a [start](../../eng/common/templates/steps/telemetry-start.yml) and an [end](../../eng/common/templates/steps/telemetry-end.yml) script that you can use to track the execution status of particular regions of your build definitions. These scripts are reentrant, so you can use them several times on the same build definition. See example at the end of this document.

One of the **main benefits** of using these scripts to collect telemetry for the build is that you'll be able to see *aggregated information from all your build configurations* on the Mission Control website. 

Note that this component (the telemetry implementation) is not part of the Arcade SDK. Only referencing the SDK isn't sufficient to use the telemetry scripts. See below instructions on how to use it.

### Will an onboarded repository automatically use it?

One of the requirements for onboarding in Arcade is to copy the `eng\common` folder from the root of the Arcade repository to the onboarding repository and reference the  `eng\common\templates\phases\base.yml` file on your YAML build definitions. This will plug in the use of the telemetry scripts on your build. 

These scripts automate the process, but there is some setup that you have to do to enable telemetry collection. Basically, you'll need to:

- Setup parameters to identify the region of code that you are collecting telemetry for and,
- Enable telemetry sending for the build.

For enabling the use of telemetry you will have to set `enableTelemetry` (see file `eng\build.yml` -> `enableTelemetry`) to true. You also have to setup these three parameters `_HelixType`, `_HelixSource` and `_HelixBuildConfig` for each region that you want to collect telemetry. They will basically work as identifiers for the region being monitored. You can find some description of these parameters [here](https://github.com/dotnet/arcade/blob/master/eng/common/templates/phases/base.yml). 

Once that setup is done you can use the telemetry scripts [start](../../eng/common/templates/steps/telemetry-start.yml) and [end](../../eng/common/templates/steps/telemetry-end.yml) to denote regions that you want to collect telemetry for. Note that you can collect telemetry for more than one region of code. Use the parameters mentioned above to identify the region that you're collecting telemetry.

### Usage Example

Below is an example of a `.vsts-ci.yaml` configuration using the telemetry scripts to track the status of the several phases of the repository build. Particularly, note that the `telemetry.yml` file is included in every phase and the Helix parameters (`helixType`, `helixSource` and `buildConfig`) are used to identify which phase the telemetry is being collected for.


```yaml
name: $(Date:yyyMMdd)$(Rev:rr)
variables:
  Build.Repository.Clean: true

phases:

- phase: Build
  steps:
  - template: eng/common/templates/steps/telemetry.yml
    parameters:
      helixType: build/product/
      helixSource: official/helix/$(Build.SourceBranch)/
      buildConfig: $(BuildConfiguration)
      steps:
      - template: /eng/configure.yaml
      - template: /eng/restore.yaml
      - template: /eng/build.yaml

- phase: CredScan
  steps:
  - template: eng/common/templates/steps/telemetry.yml
    parameters:
      helixType: build/staticanalysis/
      helixSource: official/helix/$(Build.SourceBranch)/
      buildConfig: $(BuildConfiguration)
      steps:
      - template: /eng/cred-scan.yaml

- phase: TSLint
  steps:
  - template: eng/common/templates/steps/telemetry.yml
    parameters:
      helixType: build/tslint/
      helixSource: official/helix/$(Build.SourceBranch)/
      buildConfig: $(BuildConfiguration)
      steps:
      - template: /eng/tslint.yaml

- phase: CodeInspection
  steps:
  - template: eng/common/templates/steps/telemetry.yml
    parameters:
      helixType: build/codeinspection/
      helixSource: official/helix/$(Build.SourceBranch)/
      buildConfig: $(BuildConfiguration)
      steps:
      - template: /eng/code-inspection.yaml
```


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CCorePackages%5CTelemetry.md)](https://helix.dot.net/f/p/5?p=Documentation%5CCorePackages%5CTelemetry.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CCorePackages%5CTelemetry.md)</sub>
<!-- End Generated Content-->
