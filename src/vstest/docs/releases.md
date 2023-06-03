# Release Notes

## 17.6.1 and newer

Please see release notes directly in the release page: https://github.com/microsoft/vstest/releases

## 17.6.0

### Issues Fixed

* Add legacy feeds
* [rel/17.6] Fix Newtonsoft versions in testhost.deps.json [#4372](https://github.com/microsoft/vstest/pull/4372)
* Revert "Revert "Fix signature verification" (#4333" [#4345](https://github.com/microsoft/vstest/pull/4345)
* Revert "Fix signature verification" [#4333](https://github.com/microsoft/vstest/pull/4333)
* Fix signature verification [#4331](https://github.com/microsoft/vstest/pull/4331)
* Pre-start testhosts [#3666](https://github.com/microsoft/vstest/pull/3666)
* Add `dotnet vstest` deprecation message [#4297](https://github.com/microsoft/vstest/pull/4297)
* Catch unhandled exception and avoid crash on test host exit [#4291](https://github.com/microsoft/vstest/pull/4291)
* Remove chutzpah [#4249](https://github.com/microsoft/vstest/pull/4249)
* Fix string conversion of `Microsoft.TestPlatform.Extensions.TrxLogger.ObjectMode.TestOutcome` [#4243](https://github.com/microsoft/vstest/pull/4243)
* Fix potential trx logger NRE [#4240](https://github.com/microsoft/vstest/pull/4240)
* handle object disposed exception [#4221](https://github.com/microsoft/vstest/pull/4221)
* Added support for checking testhost compatibility with test sessions [#4199](https://github.com/microsoft/vstest/pull/4199)

See full log [here](https://github.com/microsoft/vstest/compare/v17.5.0...v17.6.0)

### Artifacts

* TestPlatform vsix: [17.6.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.6/20230515-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.6.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.6.0)

## 17.3.3

### Issues Fixed
* [rel/17.3] Update Newtonsoft.Json to 13.0.1 [#4299](https://github.com/microsoft/vstest/pull/4299)

See full log [here](https://github.com/microsoft/vstest/compare/v17.3.2...v17.3.3)

### Drops

* TestPlatform vsix: [17.3.3](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.3/20230324-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.3.3](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.3.3)

## 17.2.1

### Issues Fixed

* [rel/17.2] Update Newtonsoft.Json to 13.0.1 [#4310](https://github.com/microsoft/vstest/pull/4310)

See full log [here](https://github.com/microsoft/vstest/compare/v17.2.0...v17.2.1)

### Drops

* TestPlatform vsix: [17.2.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.2/20230324-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.2.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.2.1)

## 17.0.2

### Issues Fixed

* [rel/17.0] Update Newtonsoft.Json to 13.0.1 [#4309](https://github.com/microsoft/vstest/pull/4309)
* [rel/17.0] Update dependencies from devdiv/DevDiv/vs-code-coverage [#3159](https://github.com/microsoft/vstest/pull/3159)

See full log [here](https://github.com/microsoft/vstest/compare/v17.0.0...v17.0.2)

### Drops

* TestPlatform vsix: [17.0.2](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.0/20230324-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.0.2](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.0.2)

## 17.6.0-preview-20230223-05

### Issues Fixed

* Pre-start testhosts [#3666](https://github.com/microsoft/vstest/pull/3666)
* Add `dotnet vstest` deprecation message [#4297](https://github.com/microsoft/vstest/pull/4297)
* Catch unhandled exception and avoid crash on test host exit [#4291](https://github.com/microsoft/vstest/pull/4291)
* Remove chutzpah [#4249](https://github.com/microsoft/vstest/pull/4249)
* Fix string conversion of `Microsoft.TestPlatform.Extensions.TrxLogger.ObjectMode.TestOutcome` [#4243](https://github.com/microsoft/vstest/pull/4243)
* Fix potential trx logger NRE [#4240](https://github.com/microsoft/vstest/pull/4240)
* handle object disposed exception [#4221](https://github.com/microsoft/vstest/pull/4221)
* Added support for checking testhost compatibility with test sessions [#4199](https://github.com/microsoft/vstest/pull/4199)

See full log [here](https://github.com/microsoft/vstest/compare/v17.5.0-preview-20221221-03...v17.6.0-preview-20230223-05)

### Artifacts

* TestPlatform vsix: [17.6.0-preview-20230223-05](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20230223-05;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.6.0-preview-20230223-05](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.6.0-preview-20230223-05)

## 17.5.0

### Issues Fixed

* Fix SDK issue [#4278](https://github.com/microsoft/vstest/pull/4278)
* Add test run serialization feature [#4126](https://github.com/microsoft/vstest/pull/4126)
* Ensure that the OnAbort message is sent if the testhost aborts early [#3993](https://github.com/microsoft/vstest/pull/3993)
* Add custom satellite assemblies resolution [#4133](https://github.com/microsoft/vstest/pull/4133)
* Fixed muxer resolution strategy enum [#4134](https://github.com/microsoft/vstest/pull/4134)
* Fixed dotnet resolution for in-process vstest.console scenarios [#4122](https://github.com/microsoft/vstest/pull/4122)
* Ensure to not produce and ship exe for netcore [#4124](https://github.com/microsoft/vstest/pull/4124)
* Fixed testhost crash for net7 [#4112](https://github.com/microsoft/vstest/pull/4112)
* Revert "Run tests with Server GC enabled & concurrent GC disabled. (#3661)" [#4108](https://github.com/microsoft/vstest/pull/4108)
* Revert making Microsoft.NET.Test.Sdk package transitive [#4104](https://github.com/microsoft/vstest/pull/4104)
* Fix recursive resource lookup [#4095](https://github.com/microsoft/vstest/pull/4095)
* Fixed CC for in-process console scenarios [#4084](https://github.com/microsoft/vstest/pull/4084)
* Fixed test session issues [#4075](https://github.com/microsoft/vstest/pull/4075)
* Fix Invalid target architecture 'S390x' error [#4066](https://github.com/microsoft/vstest/pull/4066)
* Add Mono.Cecil.Rocks [#4071](https://github.com/microsoft/vstest/pull/4071)
* Revert "Revert "Re-enable arm64 ngen (#3931)" (#3948)" [#3951](https://github.com/microsoft/vstest/pull/3951)
* Update docker to the latest tagging schema [#4041](https://github.com/microsoft/vstest/pull/4041)
* Update resources [#4063](https://github.com/microsoft/vstest/pull/4063)
* Use environment variables for AeDebugger mode [#4049](https://github.com/microsoft/vstest/pull/4049)
* Add postmortem blame mode [#4032](https://github.com/microsoft/vstest/pull/4032)
* Add VSTEST_DISABLE_THREADPOOL_SIZE_INCREASE feature flag [#4046](https://github.com/microsoft/vstest/pull/4046)

See full log [here](https://github.com/microsoft/vstest/compare/v17.4.1...v17.5.0)

### Artifacts

* TestPlatform vsix: [17.5.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.5/20230221-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [17.5.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.5.0)

## 17.5.0-preview-20221221-03

### Issues Fixed

* Add test run serialization feature [#4126](https://github.com/microsoft/vstest/pull/4126)
* Ensure that the OnAbort message is sent if the testhost aborts early [#3993](https://github.com/microsoft/vstest/pull/3993)
* Add custom satellite assemblies resolution [#4133](https://github.com/microsoft/vstest/pull/4133)
* Fixed muxer resolution strategy enum [#4134](https://github.com/microsoft/vstest/pull/4134)
* Fixed dotnet resolution for in-process vstest.console scenarios [#4122](https://github.com/microsoft/vstest/pull/4122)
* Ensure to not produce and ship exe for netcore [#4124](https://github.com/microsoft/vstest/pull/4124)
* Fixed testhost crash for net7 [#4112](https://github.com/microsoft/vstest/pull/4112)
* Revert "Run tests with Server GC enabled & concurrent GC disabled. (#3661)" [#4108](https://github.com/microsoft/vstest/pull/4108)
* Revert making Microsoft.NET.Test.Sdk package transitive [#4104](https://github.com/microsoft/vstest/pull/4104)
* Fix recursive resource lookup [#4095](https://github.com/microsoft/vstest/pull/4095)
* Fixed CC for in-process console scenarios [#4084](https://github.com/microsoft/vstest/pull/4084)
* Fixed test session issues [#4075](https://github.com/microsoft/vstest/pull/4075)
* Fix Invalid target architecture 'S390x' error [#4066](https://github.com/microsoft/vstest/pull/4066)
* Add Mono.Cecil.Rocks [#4071](https://github.com/microsoft/vstest/pull/4071)
* Revert "Revert "Re-enable arm64 ngen (#3931)" (#3948)" [#3951](https://github.com/microsoft/vstest/pull/3951)
* Update docker to the latest tagging schema [#4041](https://github.com/microsoft/vstest/pull/4041)
* Update resources [#4063](https://github.com/microsoft/vstest/pull/4063)
* Use environment variables for AeDebugger mode [#4049](https://github.com/microsoft/vstest/pull/4049)
* Add postmortem blame mode [#4032](https://github.com/microsoft/vstest/pull/4032)
* Add VSTEST_DISABLE_THREADPOOL_SIZE_INCREASE feature flag [#4046](https://github.com/microsoft/vstest/pull/4046)

See full log [here](https://github.com/microsoft/vstest/compare/v17.5.0-preview-20221003-04...v17.5.0-preview-20221221-03)

### Artifacts

* TestPlatform vsix: [17.5.0-preview-20221221-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.5/20221221-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.5.0-preview-20221221-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.5.0-preview-20221221-03)

## 17.4.1

### Issues Fixed

* Fix satellite resolution for Microsoft.TestPlatform.Common [#4147](https://github.com/microsoft/vstest/pull/4147)

See full log [here](https://github.com/microsoft/vstest/compare/v17.4.0...v17.4.1)

### Drops

* TestPlatform vsix: [17.4.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.4/20221215-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.4.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.4.1)

## 17.4.0

### Issues Fixed

* Revert "Run tests with Server GC enabled & concurrent GC disabled. (#3661)" (#4108) [#4109](https://github.com/microsoft/vstest/pull/4109)
* Revert making Microsoft.NET.Test.Sdk package transitive [#4105](https://github.com/microsoft/vstest/pull/4105)
* [rel/17.4] Fix Invalid target architecture 'S390x' error [#4079](https://github.com/microsoft/vstest/pull/4079)
* Fix release note and prebuild scripts
* [rel/17.4] Remove portable CPP adapter and dbghelp [#4020](https://github.com/microsoft/vstest/pull/4020)
* Mirror test commit.
* Fix ManagedNameHelper to support namespaceless methods. [#4003](https://github.com/microsoft/vstest/pull/4003)
* Remove netstandard telemetry dependencies [#4007](https://github.com/microsoft/vstest/pull/4007)
* Playground project file refactored. [#4002](https://github.com/microsoft/vstest/pull/4002)
* Fixed wording for Github issue template [#3998](https://github.com/microsoft/vstest/pull/3998)
* Add more // to the console mode comment in playground to make it easier to uncomment. [#3999](https://github.com/microsoft/vstest/pull/3999)
* Fixed Selenium test run hang after stopping the debugger [#3995](https://github.com/microsoft/vstest/pull/3995)
* Enable usage of datacollectors in playground. [#3981](https://github.com/microsoft/vstest/pull/3981)
* Update azure-pipelines.yml
* Fixed common.lib.ps1 use correct dotnet on ARM64 devices. [#3986](https://github.com/microsoft/vstest/pull/3986)
* Fix pipeline build triggers. [#3988](https://github.com/microsoft/vstest/pull/3988)
* Fix variable name in common.lib.ps1 [#3985](https://github.com/microsoft/vstest/pull/3985)
* Add some polyfill to simplify code across compilations [#3974](https://github.com/microsoft/vstest/pull/3974)
* Update versions of diagnostics dependencies [#3976](https://github.com/microsoft/vstest/pull/3976)
* Refactor supported TFMs names [#3973](https://github.com/microsoft/vstest/pull/3973)
* Updated deprecated build VMs. [#3972](https://github.com/microsoft/vstest/pull/3972)
* Signing fixed. [#3971](https://github.com/microsoft/vstest/pull/3971)
* Fix signing verification script
* Updated build scripts to always install the latest dotnet patch. [#3968](https://github.com/microsoft/vstest/pull/3968)
* Localized file check-in by OneLocBuild Task: Build definition ID 2923: Build ID 6606255 [#3970](https://github.com/microsoft/vstest/pull/3970)
* Remove TargetLatestRuntimePatch properties [#3969](https://github.com/microsoft/vstest/pull/3969)
* Updated dotnet runtime versions.
* Add missing signing [#3964](https://github.com/microsoft/vstest/pull/3964)
* Added net7 support. [#3944](https://github.com/microsoft/vstest/pull/3944)
* Declare Newtonsoft.Json dependency for netstandard2.0 in Microsoft.Te… [#3962](https://github.com/microsoft/vstest/pull/3962)
* Make TraitCollection serializable in all supported TFMs [#3963](https://github.com/microsoft/vstest/pull/3963)
* Replace netstandard1.0 and netstandard1.3 with netstandard2.0 [#3921](https://github.com/microsoft/vstest/pull/3921)
* Fix stack overflow in FilterExpression.ValidForProperties [#3946](https://github.com/microsoft/vstest/pull/3946)
* Add process id to  VSTEST_DUMP_PROCDUMPARGUMENTS usage for for BlameDataCollector [#3957](https://github.com/microsoft/vstest/pull/3957)
* Console logger splits path using directory and alt directory separators [#3923](https://github.com/microsoft/vstest/pull/3923)
* Do not match .NET Standard to Dotnet testhost runner [#3949](https://github.com/microsoft/vstest/pull/3949)
* Remove AllowMultiple on DirectoryBasedTestDiscovererAttribute. [#3953](https://github.com/microsoft/vstest/pull/3953)
* Revert "Re-enable arm64 ngen (#3931)" [#3948](https://github.com/microsoft/vstest/pull/3948)
* Re-enable arm64 ngen [#3931](https://github.com/microsoft/vstest/pull/3931)
* Support test discovery in sources that are directories [#3932](https://github.com/microsoft/vstest/pull/3932)
* Allow DotNetHostPath to contain env vars [#3858](https://github.com/microsoft/vstest/pull/3858)
* update fakes package version to include fix that prevents testhost from crashing [#3928](https://github.com/microsoft/vstest/pull/3928)
* Run tests with Server GC enabled & concurrent GC disabled. [#3661](https://github.com/microsoft/vstest/pull/3661)
* VS: move Solution Items folder out of scripts [#3919](https://github.com/microsoft/vstest/pull/3919)
* Set discovery batch size to 1000 [#3896](https://github.com/microsoft/vstest/pull/3896)
* Update Fakes packages [#3912](https://github.com/microsoft/vstest/pull/3912)
* Fix warnings on main (IDE only) [#3914](https://github.com/microsoft/vstest/pull/3914)
* Fix name of testhost folder in playground [#3917](https://github.com/microsoft/vstest/pull/3917)
* fixed paths duplicates [#3907](https://github.com/microsoft/vstest/pull/3907)
* Add some tests for AssemblyHelper [#3911](https://github.com/microsoft/vstest/pull/3911)
* Fix broken behaviour for AssemblyHelper [#3909](https://github.com/microsoft/vstest/pull/3909)
* Fix verification of signing [#3904](https://github.com/microsoft/vstest/pull/3904)
* Migrate FabricBot Tasks to Config-as-Code [#3823](https://github.com/microsoft/vstest/pull/3823)
* Removed unnecessary signing instruction. [#3903](https://github.com/microsoft/vstest/pull/3903)
* Fix some vulnerability [#3897](https://github.com/microsoft/vstest/pull/3897)
* Signing: Fix path to TestHost folders [#3902](https://github.com/microsoft/vstest/pull/3902)
* Move codebase to netcoreapp3.1 [#3861](https://github.com/microsoft/vstest/pull/3861)
* Update dotnet runtimes [#3901](https://github.com/microsoft/vstest/pull/3901)
* CC package update [#3881](https://github.com/microsoft/vstest/pull/3881)
* Use runtime 3.1.27 [#3900](https://github.com/microsoft/vstest/pull/3900)
* Added inproc wrapper friend [#3898](https://github.com/microsoft/vstest/pull/3898)
* Remove un-needed entries from sln [#3887](https://github.com/microsoft/vstest/pull/3887)
* Move MSTest1 back to Playground folder [#3885](https://github.com/microsoft/vstest/pull/3885)
* Build compatibility matrix tests faster [#3884](https://github.com/microsoft/vstest/pull/3884)
* Fixed review comments for PR #3728 [#3882](https://github.com/microsoft/vstest/pull/3882)
* Make Microsoft.NET.Test.Sdk package transitive [#3879](https://github.com/microsoft/vstest/pull/3879)
* Enable some design rules that could be interesting [#3875](https://github.com/microsoft/vstest/pull/3875)
* In-process vstest.console [#3728](https://github.com/microsoft/vstest/pull/3728)
* Globally exclude IDE1006 for vstest.ProgrammerTests namespace [#3872](https://github.com/microsoft/vstest/pull/3872)
* Fix warnings on main [#3870](https://github.com/microsoft/vstest/pull/3870)
* CA1051: Do not declare visible instance fields [#3859](https://github.com/microsoft/vstest/pull/3859)
* Fixed bugs in ManagedMethod parsing, and updated hierarchies. [#3704](https://github.com/microsoft/vstest/pull/3704)
* CA1001: Types that own disposable fields should be disposable [#3860](https://github.com/microsoft/vstest/pull/3860)
* CA1018: Mark attributes with AttributeUsageAttribute [#3865](https://github.com/microsoft/vstest/pull/3865)
* Disable IDE1006 on vstest namespace [#3866](https://github.com/microsoft/vstest/pull/3866)
* Fix concurrency access causing flakyness in the test [#3867](https://github.com/microsoft/vstest/pull/3867)
* Don't parallelize default platform tests [#3849](https://github.com/microsoft/vstest/pull/3849)
* Remove default architecture env variable [#3863](https://github.com/microsoft/vstest/pull/3863)
* Fix signing [#3864](https://github.com/microsoft/vstest/pull/3864)
* Cleanups post move to net462 [#3856](https://github.com/microsoft/vstest/pull/3856)
* IDE0060: Remove unused parameter [#3854](https://github.com/microsoft/vstest/pull/3854)
* Enforce use of correct dispose pattern [#3852](https://github.com/microsoft/vstest/pull/3852)
* Follow .NET lifecycle: update codebase to net462 [#3646](https://github.com/microsoft/vstest/pull/3646)
* CA1822: Mark members as static [#3853](https://github.com/microsoft/vstest/pull/3853)
* Fixed some security vulnerabilities. [#3851](https://github.com/microsoft/vstest/pull/3851)
* Increase ThreadPool.MinThreads limit [#3845](https://github.com/microsoft/vstest/pull/3845)
* Enable culture analyzer and fix issues [#3678](https://github.com/microsoft/vstest/pull/3678)
* Revert #3715 [#3843](https://github.com/microsoft/vstest/pull/3843)
* Enable more rules on test projects [#3832](https://github.com/microsoft/vstest/pull/3832)
* Ignore CancelTestDiscovery test as it is flaky [#3839](https://github.com/microsoft/vstest/pull/3839)
* Fix Newtonsoft.Json.dll 13.0.1 signature verification [#3835](https://github.com/microsoft/vstest/pull/3835)
* Bump to 17.4.0 [#3831](https://github.com/microsoft/vstest/pull/3831)
* Upgrade to Newtonsoft.Json 13.0.1 [#3815](https://github.com/microsoft/vstest/pull/3815)
* Enable CA1824 - Mark assemblies with NeutralResourcesLanguageAttribute [#3833](https://github.com/microsoft/vstest/pull/3833)
* Make test functions static when possible [#3830](https://github.com/microsoft/vstest/pull/3830)
* Fix unused using warning [#3828](https://github.com/microsoft/vstest/pull/3828)
* Remove unused msdia140typelib_clr0200.dll [#3822](https://github.com/microsoft/vstest/pull/3822)
* Enable nullables on all public API files [#3808](https://github.com/microsoft/vstest/pull/3808)
* Fix missing signature [#3796](https://github.com/microsoft/vstest/pull/3796)
* Version bumped to 17.4 [#3818](https://github.com/microsoft/vstest/pull/3818)

See full log [here](https://github.com/microsoft/vstest/compare/v17.3.2...v17.4.0)

### Drops

* TestPlatform vsix: [17.4.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20221107-04;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.4.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.4.0)

## 17.5.0-preview-20221003-04

### Issues Fixed

* Fix release note and prebuild scripts [#4039](https://github.com/microsoft/vstest/pull/4039)
* Update Fakes binaries [#4033](https://github.com/microsoft/vstest/pull/4033)
* Add support for ppc64le processor architecture [#4028](https://github.com/microsoft/vstest/pull/4028)

See full log [here](https://github.com/microsoft/vstest/compare/v17.4.0-preview-20220726-02...v17.5.0-preview-20221003-04)

### Drops

* TestPlatform vsix: [17.5.0-preview-20221003-04](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20221003-04;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.5.0-preview-20221003-04](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.5.0-preview-20221003-04)

## 17.4.0-preview-20221003-03

### Issues Fixed

* Fix release note and prebuild scripts
* [rel/17.4] Remove portable CPP adapter and dbghelp [#4020](https://github.com/microsoft/vstest/pull/4020)
* Fix ManagedNameHelper to support namespaceless methods. [#4003](https://github.com/microsoft/vstest/pull/4003)
* Remove netstandard telemetry dependencies [#4007](https://github.com/microsoft/vstest/pull/4007)
* Playground project file refactored. [#4002](https://github.com/microsoft/vstest/pull/4002)
* Fixed wording for Github issue template [#3998](https://github.com/microsoft/vstest/pull/3998)
* Add more // to the console mode comment in playground to make it easier to uncomment. [#3999](https://github.com/microsoft/vstest/pull/3999)
* Fixed Selenium test run hang after stopping the debugger [#3995](https://github.com/microsoft/vstest/pull/3995)
* Enable usage of datacollectors in playground. [#3981](https://github.com/microsoft/vstest/pull/3981)
* Update azure-pipelines.yml
* Fixed common.lib.ps1 use correct dotnet on ARM64 devices. [#3986](https://github.com/microsoft/vstest/pull/3986)
* Fix pipeline build triggers. [#3988](https://github.com/microsoft/vstest/pull/3988)
* Fix variable name in common.lib.ps1 [#3985](https://github.com/microsoft/vstest/pull/3985)
* Add some polyfill to simplify code across compilations [#3974](https://github.com/microsoft/vstest/pull/3974)
* Update versions of diagnostics dependencies [#3976](https://github.com/microsoft/vstest/pull/3976)
* Refactor supported TFMs names [#3973](https://github.com/microsoft/vstest/pull/3973)
* Updated deprecated build VMs. [#3972](https://github.com/microsoft/vstest/pull/3972)
* Signing fixed. [#3971](https://github.com/microsoft/vstest/pull/3971)
* Fix signing verification script
* Updated build scripts to always install the latest dotnet patch. [#3968](https://github.com/microsoft/vstest/pull/3968)
* Remove TargetLatestRuntimePatch properties [#3969](https://github.com/microsoft/vstest/pull/3969)
* Updated dotnet runtime versions.
* Add missing signing [#3964](https://github.com/microsoft/vstest/pull/3964)
* Added net7 support. [#3944](https://github.com/microsoft/vstest/pull/3944)
* Declare Newtonsoft.Json dependency for netstandard2.0 in Microsoft.Te… [#3962](https://github.com/microsoft/vstest/pull/3962)
* Make TraitCollection serializable in all supported TFMs [#3963](https://github.com/microsoft/vstest/pull/3963)
* Replace netstandard1.0 and netstandard1.3 with netstandard2.0 [#3921](https://github.com/microsoft/vstest/pull/3921)
* Fix stack overflow in FilterExpression.ValidForProperties [#3946](https://github.com/microsoft/vstest/pull/3946)
* Add process id to  VSTEST_DUMP_PROCDUMPARGUMENTS usage for for BlameDataCollector [#3957](https://github.com/microsoft/vstest/pull/3957)
* Console logger splits path using directory and alt directory separators [#3923](https://github.com/microsoft/vstest/pull/3923)
* Localized file check-in by OneLocBuild Task: Build definition ID 2923: Build ID 6589224 [#3954](https://github.com/microsoft/vstest/pull/3954)
* Do not match .NET Standard to Dotnet testhost runner [#3949](https://github.com/microsoft/vstest/pull/3949)
* Remove AllowMultiple on DirectoryBasedTestDiscovererAttribute. [#3953](https://github.com/microsoft/vstest/pull/3953)
* Revert "Re-enable arm64 ngen (#3931)" [#3948](https://github.com/microsoft/vstest/pull/3948)
* Re-enable arm64 ngen [#3931](https://github.com/microsoft/vstest/pull/3931)
* Support test discovery in sources that are directories [#3932](https://github.com/microsoft/vstest/pull/3932)
* Allow DotNetHostPath to contain env vars [#3858](https://github.com/microsoft/vstest/pull/3858)
* update fakes package version to include fix that prevents testhost from crashing [#3928](https://github.com/microsoft/vstest/pull/3928)
* Run tests with Server GC enabled & concurrent GC disabled. [#3661](https://github.com/microsoft/vstest/pull/3661)
* VS: move Solution Items folder out of scripts [#3919](https://github.com/microsoft/vstest/pull/3919)
* Set discovery batch size to 1000 [#3896](https://github.com/microsoft/vstest/pull/3896)
* Update Fakes packages [#3912](https://github.com/microsoft/vstest/pull/3912)
* Fix warnings on main (IDE only) [#3914](https://github.com/microsoft/vstest/pull/3914)
* Fix name of testhost folder in playground [#3917](https://github.com/microsoft/vstest/pull/3917)
* fixed paths duplicates [#3907](https://github.com/microsoft/vstest/pull/3907)
* Add some tests for AssemblyHelper [#3911](https://github.com/microsoft/vstest/pull/3911)
* Fix broken behaviour for AssemblyHelper [#3909](https://github.com/microsoft/vstest/pull/3909)
* Fix verification of signing [#3904](https://github.com/microsoft/vstest/pull/3904)
* Migrate FabricBot Tasks to Config-as-Code [#3823](https://github.com/microsoft/vstest/pull/3823)
* Removed unnecessary signing instruction. [#3903](https://github.com/microsoft/vstest/pull/3903)
* Fix some vulnerability [#3897](https://github.com/microsoft/vstest/pull/3897)
* Signing: Fix path to TestHost folders [#3902](https://github.com/microsoft/vstest/pull/3902)
* Move codebase to netcoreapp3.1 [#3861](https://github.com/microsoft/vstest/pull/3861)
* Update dotnet runtimes [#3901](https://github.com/microsoft/vstest/pull/3901)
* CC package update [#3881](https://github.com/microsoft/vstest/pull/3881)

See full log [here](https://github.com/microsoft/vstest/compare/v17.4.0-preview-20220726-02...v17.4.0-preview-20221003-03)

### Drops

* TestPlatform vsix: [17.4.0-preview-20221003-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.4/20221003-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.4.0-preview-20221003-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.4.0-preview-20221003-03)

## 17.3.2

### Issue Fixed

* Fixed Selenium test run hang after stopping the debugger [#4013](https://github.com/microsoft/vstest/pull/4013)

See full log [here](https://github.com/microsoft/vstest/compare/v17.3.1...v17.3.2)

### Drops

* TestPlatform vsix: [17.3.2](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.3/20220919-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.3.2](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.3.2)

## 17.3.1

### Issues Fixed

* Do not match .NET Standard to Dotnet testhost runner [#3958](https://github.com/microsoft/vstest/pull/3958)

See full log [here](https://github.com/microsoft/vstest/compare/v17.3.0...v17.3.1)

### Drops

* TestPlatform vsix: [17.3.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.3/20220829-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.3.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.3.1)

## 17.3.0

### Issue Fixed

* Reverts change that breaks DOTNET_ROOT [#3844](https://github.com/microsoft/vstest/pull/3844)
* Add runtime location to host log [#3806](https://github.com/microsoft/vstest/pull/3806)
* Fix reading/setting culture env variables [#3802](https://github.com/microsoft/vstest/pull/3802)
* Enable nullable on missed files [#3801](https://github.com/microsoft/vstest/pull/3801)
* Enable nullable on ObjectModel [#3793](https://github.com/microsoft/vstest/pull/3793)
* Add missing nullable [#3795](https://github.com/microsoft/vstest/pull/3795)
* Improve how to retrieve process ID [#3794](https://github.com/microsoft/vstest/pull/3794)
* Enable nullables on common [#3790](https://github.com/microsoft/vstest/pull/3790)
* Fix race condition issues on stream access on LengthPrefixCommunicatiΓÇª [#3789](https://github.com/microsoft/vstest/pull/3789)
* Enable nullables on CrossPlatEngine [#3779](https://github.com/microsoft/vstest/pull/3779)
* Enable nullable on VS Translation layer [#3781](https://github.com/microsoft/vstest/pull/3781)
* Enable missed nullables on already handled projects [#3773](https://github.com/microsoft/vstest/pull/3773)
* Add background option for testhosts [#3772](https://github.com/microsoft/vstest/pull/3772)
* Pass sources, to fix native debug [#3777](https://github.com/microsoft/vstest/pull/3777)
* Temporary disable ngen for arm64 binaries [#3765](https://github.com/microsoft/vstest/pull/3765)
* Add default platform option to runsettings [#3770](https://github.com/microsoft/vstest/pull/3770)
* Support arm64 native CppUnitTestFramework with `dotnet test` [#3768](https://github.com/microsoft/vstest/pull/3768)
* Reduce usage of bang + reduce usage of throw/catch [#3771](https://github.com/microsoft/vstest/pull/3771)
* Fix warnings and failed assertions [#3767](https://github.com/microsoft/vstest/pull/3767)
* Enable nullable on Communication utilities [#3758](https://github.com/microsoft/vstest/pull/3758)
* Add default VS settings to playground [#3756](https://github.com/microsoft/vstest/pull/3756)
* Skip sources when runtime provider is not found [#3760](https://github.com/microsoft/vstest/pull/3760)
* Fix loop on TPDebug.Assert [#3764](https://github.com/microsoft/vstest/pull/3764)
* Remove backup project that should not have been checked in [#3763](https://github.com/microsoft/vstest/pull/3763)
* Enable nullables on acceptance tests [#3757](https://github.com/microsoft/vstest/pull/3757)
* Enable nullables on TRX logger [#3754](https://github.com/microsoft/vstest/pull/3754)
* Enable nullables on CoreUtilities [#3751](https://github.com/microsoft/vstest/pull/3751)
* Fix failing assertions on tests [#3761](https://github.com/microsoft/vstest/pull/3761)
* Get PlatformAbstractions from ObjectModel [#3722](https://github.com/microsoft/vstest/pull/3722)
* Fix nullable conflict [#3753](https://github.com/microsoft/vstest/pull/3753)
* Remove missed #nullable disable [#3741](https://github.com/microsoft/vstest/pull/3741)
* Fix which value is used in platform warning [#3752](https://github.com/microsoft/vstest/pull/3752)
* Experimental feature: enable negative values of MaxCpuCount to match a percentage of number of cores [#3748](https://github.com/microsoft/vstest/pull/3748)
* Enable nullables on SettingsMigrator [#3744](https://github.com/microsoft/vstest/pull/3744)
* Enable nullables on TestHostProvider [#3738](https://github.com/microsoft/vstest/pull/3738)
* Update MSTest and VSTest versions [#3663](https://github.com/microsoft/vstest/pull/3663)
* Fix DOTNET_ROOT env var for .NET 6.0+ [#3715](https://github.com/microsoft/vstest/pull/3715)
* Enable nullables on TestPlatform.Client [#3745](https://github.com/microsoft/vstest/pull/3745)
* Add env var to control host priority [#3740](https://github.com/microsoft/vstest/pull/3740)

See full log [here](https://github.com/microsoft/vstest/compare/v17.3.0-preview-20220612-01...v17.3.0)

### Drops

* TestPlatform vsix: [17.3.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.3/20220809-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.3.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.3.0)

## 17.4.0-preview-20220726-02

### Issue Fixed

* Use runtime 3.1.27 [#3900](https://github.com/microsoft/vstest/pull/3900)
* Remove un-needed entries from sln [#3887](https://github.com/microsoft/vstest/pull/3887)
* Build compatibility matrix tests faster [#3884](https://github.com/microsoft/vstest/pull/3884)
* Make Microsoft.NET.Test.Sdk package transitive [#3879](https://github.com/microsoft/vstest/pull/3879)
* In-process vstest.console [#3728](https://github.com/microsoft/vstest/pull/3728)
* Fix warnings on main [#3870](https://github.com/microsoft/vstest/pull/3870)
* Fixed bugs in ManagedMethod parsing, and updated hierarchies. [#3704](https://github.com/microsoft/vstest/pull/3704)
* Remove default architecture env variable [#3863](https://github.com/microsoft/vstest/pull/3863)
* Enforce use of correct dispose pattern [#3852](https://github.com/microsoft/vstest/pull/3852)
* Follow .NET lifecycle: update codebase to net462 [#3646](https://github.com/microsoft/vstest/pull/3646)
* Fixed some security vulnerabilities. [#3851](https://github.com/microsoft/vstest/pull/3851)
* Increase ThreadPool.MinThreads limit [#3845](https://github.com/microsoft/vstest/pull/3845)

See full log [here](https://github.com/microsoft/vstest/compare/v17.4.0-preview-20220707-01...v17.4.0-preview-20220726-02)

### Drops

* TestPlatform vsix: [17.4.0-preview-20220726-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220726-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.4.0-preview-20220726-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.4.0-preview-20220726-02)

## 17.4.0-preview-20220707-01

### Issue Fixed

* Revert #3715 [#3843](https://github.com/microsoft/vstest/pull/3843)
* Enable more rules on test projects [#3832](https://github.com/microsoft/vstest/pull/3832)
* Ignore CancelTestDiscovery test as it is flaky [#3839](https://github.com/microsoft/vstest/pull/3839)
* Fix Newtonsoft.Json.dll 13.0.1 signature verification [#3835](https://github.com/microsoft/vstest/pull/3835)
* Bump to 17.4.0 [#3831](https://github.com/microsoft/vstest/pull/3831)
* Upgrade to Newtonsoft.Json 13.0.1 [#3815](https://github.com/microsoft/vstest/pull/3815)
* Enable CA1824 - Mark assemblies with NeutralResourcesLanguageAttribute [#3833](https://github.com/microsoft/vstest/pull/3833)
* Make test functions static when possible [#3830](https://github.com/microsoft/vstest/pull/3830)
* Fix unused using warning [#3828](https://github.com/microsoft/vstest/pull/3828)
* Remove unused msdia140typelib_clr0200.dll [#3822](https://github.com/microsoft/vstest/pull/3822)
* Enable nullables on all public API files [#3808](https://github.com/microsoft/vstest/pull/3808)
* Fix missing signature [#3796](https://github.com/microsoft/vstest/pull/3796)
* Version bumped to 17.4 [#3818](https://github.com/microsoft/vstest/pull/3818)
* Add runtime location to host log [#3806](https://github.com/microsoft/vstest/pull/3806)
* Fix reading/setting culture env variables [#3802](https://github.com/microsoft/vstest/pull/3802)
* Enable nullable on missed files [#3801](https://github.com/microsoft/vstest/pull/3801)
* Enable nullable on ObjectModel [#3793](https://github.com/microsoft/vstest/pull/3793)
* Add missing nullable [#3795](https://github.com/microsoft/vstest/pull/3795)
* Improve how to retrieve process ID [#3794](https://github.com/microsoft/vstest/pull/3794)
* Enable nullables on common [#3790](https://github.com/microsoft/vstest/pull/3790)
* Fix race condition issues on stream access on LengthPrefixCommunicati??? [#3789](https://github.com/microsoft/vstest/pull/3789)
* Enable nullables on CrossPlatEngine [#3779](https://github.com/microsoft/vstest/pull/3779)
* Enable nullable on VS Translation layer [#3781](https://github.com/microsoft/vstest/pull/3781)
* Enable missed nullables on already handled projects [#3773](https://github.com/microsoft/vstest/pull/3773)
* Add background option for testhosts [#3772](https://github.com/microsoft/vstest/pull/3772)
* Pass sources, to fix native debug [#3777](https://github.com/microsoft/vstest/pull/3777)
* Temporary disable ngen for arm64 binaries [#3765](https://github.com/microsoft/vstest/pull/3765)
* Add default platform option to runsettings [#3770](https://github.com/microsoft/vstest/pull/3770)
* Support arm64 native CppUnitTestFramework with `dotnet test` [#3768](https://github.com/microsoft/vstest/pull/3768)
* Reduce usage of bang + reduce usage of throw/catch [#3771](https://github.com/microsoft/vstest/pull/3771)
* Fix warnings and failed assertions [#3767](https://github.com/microsoft/vstest/pull/3767)
* Enable nullable on Communication utilities [#3758](https://github.com/microsoft/vstest/pull/3758)
* Add default VS settings to playground [#3756](https://github.com/microsoft/vstest/pull/3756)
* Skip sources when runtime provider is not found [#3760](https://github.com/microsoft/vstest/pull/3760)
* Fix loop on TPDebug.Assert [#3764](https://github.com/microsoft/vstest/pull/3764)
* Remove backup project that should not have been checked in [#3763](https://github.com/microsoft/vstest/pull/3763)
* Enable nullables on acceptance tests [#3757](https://github.com/microsoft/vstest/pull/3757)
* Enable nullables on TRX logger [#3754](https://github.com/microsoft/vstest/pull/3754)
* Enable nullables on CoreUtilities [#3751](https://github.com/microsoft/vstest/pull/3751)
* Fix failing assertions on tests [#3761](https://github.com/microsoft/vstest/pull/3761)
* Get PlatformAbstractions from ObjectModel [#3722](https://github.com/microsoft/vstest/pull/3722)
* Fix nullable conflict [#3753](https://github.com/microsoft/vstest/pull/3753)
* Remove missed #nullable disable [#3741](https://github.com/microsoft/vstest/pull/3741)
* Fix which value is used in platform warning [#3752](https://github.com/microsoft/vstest/pull/3752)
* Experimental feature: enable negative values of MaxCpuCount to match a percentage of number of cores [#3748](https://github.com/microsoft/vstest/pull/3748)
* Enable nullables on SettingsMigrator [#3744](https://github.com/microsoft/vstest/pull/3744)
* Enable nullables on TestHostProvider [#3738](https://github.com/microsoft/vstest/pull/3738)
* Update MSTest and VSTest versions [#3663](https://github.com/microsoft/vstest/pull/3663)
* Fix DOTNET_ROOT env var for .NET 6.0+ [#3715](https://github.com/microsoft/vstest/pull/3715)
* Enable nullables on TestPlatform.Client [#3745](https://github.com/microsoft/vstest/pull/3745)
* Add env var to control host priority [#3740](https://github.com/microsoft/vstest/pull/3740)

See full log [here](https://github.com/microsoft/vstest/compare/f6b89cfcace13f8ac0e994af9dbe7f9a7438e958...v17.4.0-preview-20220707-01)

### Drops

* TestPlatform vsix: [17.4.0-preview-20220707-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220707-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.4.0-preview-20220707-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.4.0-preview-20220707-01)

## 17.3.0-preview-20220612-01

### Issue Fixed

* Fix logging setup when running in remote [#3643](https://github.com/microsoft/vstest/pull/3643)
* Fix get process architecture [#3726](https://github.com/microsoft/vstest/pull/3726)
* Enable nullables on Abstraction project [#3723](https://github.com/microsoft/vstest/pull/3723)
* Enable nullables on TP Build project [#3719](https://github.com/microsoft/vstest/pull/3719)
* Add target framework information to the AttachDebugger callback [#3701](https://github.com/microsoft/vstest/pull/3701)
* Enable nullable on BlameDataCollector [#3713](https://github.com/microsoft/vstest/pull/3713)
* Enable nullable on HtmlLogger [#3712](https://github.com/microsoft/vstest/pull/3712)
* Update framework detection logic to not rely on throwing/catching NRE [#3714](https://github.com/microsoft/vstest/pull/3714)
* Enable nullable on TP utilities [#3700](https://github.com/microsoft/vstest/pull/3700)
* Enable nullable on vstest.console [#3694](https://github.com/microsoft/vstest/pull/3694)
* Minor optimizations [#3687](https://github.com/microsoft/vstest/pull/3687)
* Using "\." instead of "." as it is not a valid regex [#3708](https://github.com/microsoft/vstest/pull/3708)

See full log [here](https://github.com/microsoft/vstest/compare/v17.3.0-preview-20220530-08...v17.3.0-preview-20220612-01)

### Drops

* TestPlatform vsix: [17.3.0-preview-20220612-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220612-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.3.0-preview-20220612-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.3.0-preview-20220612-01)

## 17.3.0-preview-20220530-08

### Issue Fixed

* Fix CppUnitTestFramework package layout [#3695](https://github.com/microsoft/vstest/pull/3695)
* Share files instead of duplicating them [#3692](https://github.com/microsoft/vstest/pull/3692)
* Enable nullable on adapter utilities [#3688](https://github.com/microsoft/vstest/pull/3688)
* Allow sharing testhosts for non-paralell runs on a Shared testhost [#3682](https://github.com/microsoft/vstest/pull/3682)
* Fix same file access exception in htmlLogger [#3373](https://github.com/microsoft/vstest/pull/3373)
* Perform file roll only occasionally when file is locked [#3684](https://github.com/microsoft/vstest/pull/3684)
* Enable nullable for EventLogCollector [#3674](https://github.com/microsoft/vstest/pull/3674)
* Fixed telemetry data sharing exception [#3676](https://github.com/microsoft/vstest/pull/3676)
* Enable nullable on AttachVS [#3671](https://github.com/microsoft/vstest/pull/3671)
* Enable nullable for datacollector [#3670](https://github.com/microsoft/vstest/pull/3670)
* Fix CUIT test [#3673](https://github.com/microsoft/vstest/pull/3673)
* Fix NRE when coverage merge tool is not found [#3665](https://github.com/microsoft/vstest/pull/3665)
* Run multiple target frameworks and architectures in single vstest.console [#3412](https://github.com/microsoft/vstest/pull/3412)
* Fix NgenArchitecture [#3651](https://github.com/microsoft/vstest/pull/3651)
* Fix missing attachement processor [#3644](https://github.com/microsoft/vstest/pull/3644)
* Added null checks for GetCachedExtensions call [#3639](https://github.com/microsoft/vstest/pull/3639)
* Fix more issues with parallel discovery and cancellation [#3605](https://github.com/microsoft/vstest/pull/3605)
* Better deserialization and serialization performance [#3608](https://github.com/microsoft/vstest/pull/3608)
* Refined runsettings matching criteria for test sessions [#3610](https://github.com/microsoft/vstest/pull/3610)

See full log [here](https://github.com/microsoft/vstest/compare/v17.3.0-preview-20220426-02...v17.3.0-preview-20220530-08)

### Drops

* TestPlatform vsix: [17.3.0-preview-20220530-08](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220530-08;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.3.0-preview-20220530-08](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.3.0-preview-20220530-08)

## 17.3.0-preview-20220426-02

### Issue Fixed

* Collect artifacts in case of test abort/cancelled [#3606](https://github.com/microsoft/vstest/pull/3606)
* Use most recent stable version of Microsoft.CodeCoverage for unit tests [#3601](https://github.com/microsoft/vstest/pull/3601)
* Patched CVE-2017-11770 and CVE-2019-0981 [#3578](https://github.com/microsoft/vstest/pull/3578)
* Fixed assembly loading in Explicit mode. [#3570](https://github.com/microsoft/vstest/pull/3570)
* Make vstest.console, and datacollector upgrade across major .NET version [#3561](https://github.com/microsoft/vstest/pull/3561)
* Fix parallel discovery [#3437](https://github.com/microsoft/vstest/pull/3437)
* Remove CC from vsix [#3546](https://github.com/microsoft/vstest/pull/3546)
* Bundle arm64 managed code coverage support [#3547](https://github.com/microsoft/vstest/pull/3547)

See full log [here](https://github.com/microsoft/vstest/compare/v17.2.0-preview-20220401-08...v17.3.0-preview-20220426-02)

### Drops

* TestPlatform vsix: [17.3.0-preview-20220426-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220426-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.3.0-preview-20220426-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.3.0-preview-20220426-02)

## 17.2.0

See full log [here](https://github.com/microsoft/vstest/compare/v17.2.0-preview-20220401-07...v17.2.0)

### Drops

* TestPlatform vsix: [17.2.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.2/20220510-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.2.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.2.0)

## 17.2.0-preview-20220401-08

### Issue Fixed

* Add packing to PR build [#3540](https://github.com/microsoft/vstest/pull/3540)

See full log [here](https://github.com/microsoft/vstest/compare/v17.2.0-preview-20220401-08...v17.2.0-preview-20220401-07)

### Drops

* TestPlatform vsix: [17.2.0-preview-20220401-08](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220401-08;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.2.0-preview-20220401-08](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.2.0-preview-20220401-08)

## v17.2.0-preview-20220401-07

### Issue Fixed

* Added telemetry data point for extensions loaded during test discovery/run [#3511](https://github.com/microsoft/vstest/pull/3511)
* Catch all exceptions when exiting process [#3530](https://github.com/microsoft/vstest/pull/3530)
* Test matrix [#3459](https://github.com/microsoft/vstest/pull/3459)
* Make Cancel Discovery faster and more reliable [#3527](https://github.com/microsoft/vstest/pull/3527)
* Fix remote testhost [#3492](https://github.com/microsoft/vstest/pull/3492)
* Fix feature flag name [#3483](https://github.com/microsoft/vstest/pull/3483)
* Add parity for AnyCPU containers between .NET Core and .NET FX, add banner with architecture and warning if running emulated(all on win arm64) [#3481](https://github.com/microsoft/vstest/pull/3481)
* Update feature flag logic to disable only semantics [#3479](https://github.com/microsoft/vstest/pull/3479)
* Allows to override shutdown timeout [#3466](https://github.com/microsoft/vstest/pull/3466)
* Support reading embedded pdbs [#3454](https://github.com/microsoft/vstest/pull/3454)
* Update nuspec project target frameworks for source-build [#3285](https://github.com/microsoft/vstest/pull/3285)
* Run DataCollectorAttachmentsProcessors inside custom AppDomain [#3434](https://github.com/microsoft/vstest/pull/3434)
* Newer approach to reference assemblies on MacOS and Linux [#3448](https://github.com/microsoft/vstest/pull/3448)
* CA1840: Use 'Environment.CurrentManagedThreadId' [#3440](https://github.com/microsoft/vstest/pull/3440)
* Patched CVE-2019-0657. [#3436](https://github.com/microsoft/vstest/pull/3436)
* Made telemetry data constants true constants [#3416](https://github.com/microsoft/vstest/pull/3416)
* Add a zero-width space after test name in HTML results [#3423](https://github.com/microsoft/vstest/pull/3423)
* Temporary disable full post processing in design mode [#3429](https://github.com/microsoft/vstest/pull/3429)
* Build vstest.console.arm64.exe and fix process architecture retrieval [#3422](https://github.com/microsoft/vstest/pull/3422)
* Fix multi tfm project tests [#3425](https://github.com/microsoft/vstest/pull/3425)

See full log [here](https://github.com/microsoft/vstest/compare/v17.2.0-preview-20220401-07...v17.2.0-preview-20220301-01)

### Drops

* TestPlatform vsix: [17.2.0-preview-20220401-07](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220401-07;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.2.0-preview-20220401-07](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.2.0-preview-20220401-07)

## 17.2.0-preview-20220301-01

### Issue Fixed

* Handle correctly waiting for process exit on Unix systems [#3410](https://github.com/microsoft/vstest/pull/3410)
* Move usings outside namespace [#3398](https://github.com/microsoft/vstest/pull/3398)
* Prefer string alias over String type [#3408](https://github.com/microsoft/vstest/pull/3408)
* Use null-coalesce assignment [#3405](https://github.com/microsoft/vstest/pull/3405)
* Added telemetry codeowners [#3403](https://github.com/microsoft/vstest/pull/3403)
* Map incoming and outgoing requests [#3314](https://github.com/microsoft/vstest/pull/3314)
* Use fakes to surround vstest core and test it [#3347](https://github.com/microsoft/vstest/pull/3347)
* Fix more IDE warnings [#3388](https://github.com/microsoft/vstest/pull/3388)
* Remember to keep in sync SDK feature flags [#3393](https://github.com/microsoft/vstest/pull/3393)
* Fix FeatureFlag singleton implementation [#3389](https://github.com/microsoft/vstest/pull/3389)
* Localized file check-in by OneLocBuild Task: Build definition ID 2923: Build ID 5773266 [#3386](https://github.com/microsoft/vstest/pull/3386)
* Test adapter loading strategy [#3380](https://github.com/microsoft/vstest/pull/3380)
* Enable parallel discovery [#3349](https://github.com/microsoft/vstest/pull/3349)
* Enable post processing [#3384](https://github.com/microsoft/vstest/pull/3384)
* add post processing intergration test [#3377](https://github.com/microsoft/vstest/pull/3377)
* Add ARM64 .NET Framework testhost [#3370](https://github.com/microsoft/vstest/pull/3370)
* Remove architecture validation [#3371](https://github.com/microsoft/vstest/pull/3371)
* Add ci switch for Windows and Windows-Acceptance [#3372](https://github.com/microsoft/vstest/pull/3372)
* Revert part of "Remove regions (#3366)" [#3369](https://github.com/microsoft/vstest/pull/3369)
* Remove regions [#3366](https://github.com/microsoft/vstest/pull/3366)
* Add unit tests for the artifact post processing [#3352](https://github.com/microsoft/vstest/pull/3352)
* Update version of VS sdk build tools [#3344](https://github.com/microsoft/vstest/pull/3344)
* Use !! null check [#3341](https://github.com/microsoft/vstest/pull/3341)
* Enable nullable on new files for all projects [#3359](https://github.com/microsoft/vstest/pull/3359)
* Enable more rules [#3345](https://github.com/microsoft/vstest/pull/3345)
* Enable pattern code style [#3358](https://github.com/microsoft/vstest/pull/3358)
* Enable even more rules [#3356](https://github.com/microsoft/vstest/pull/3356)
* Fixed conditional expression [#3355](https://github.com/microsoft/vstest/pull/3355)
* Telemetry improvements [#3340](https://github.com/microsoft/vstest/pull/3340)
* Simplify calls to EqtTrace logger [#3351](https://github.com/microsoft/vstest/pull/3351)
* Implement the post processing extension feature [#3324](https://github.com/microsoft/vstest/pull/3324)
* Add ionide exclusions to gitignore [#3336](https://github.com/microsoft/vstest/pull/3336)
* Enable TreatWarningsAsErrors only on CI [#3335](https://github.com/microsoft/vstest/pull/3335)
* Fix failfast [#3327](https://github.com/microsoft/vstest/pull/3327)
* Fix red [#3325](https://github.com/microsoft/vstest/pull/3325)
* Update git blame [#3326](https://github.com/microsoft/vstest/pull/3326)
* Run dotnet format whitespace [#3307](https://github.com/microsoft/vstest/pull/3307)
* Suppress assembly architecture, assembly conflict and restore warnings [#3323](https://github.com/microsoft/vstest/pull/3323)
* Make property readonly when possible [#3320](https://github.com/microsoft/vstest/pull/3320)
* Log callbacks to delegates better [#3283](https://github.com/microsoft/vstest/pull/3283)
* Fix OperationCanceledException handling for the TestRunAttachmentsProcessingManager [#3319](https://github.com/microsoft/vstest/pull/3319)
* Fix serialization issue with TestRunSettings [#3317](https://github.com/microsoft/vstest/pull/3317)
* Fallback to loaded assembly if load file fails during the extension discovery v2 [#3315](https://github.com/microsoft/vstest/pull/3315)
* Stabilize unit test  [#3311](https://github.com/microsoft/vstest/pull/3311)

See full log [here](https://github.com/microsoft/vstest/compare/v17.2.0-preview-20220301-01...v17.2.0-preview-20220131-20)

### Drops

* TestPlatform vsix: [17.2.0-preview-20220301-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220301-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.2.0-preview-20220301-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.2.0-preview-20220301-01)

## 17.1.0

### Issues Fixed

* Added support for TestAdapterLoadingStrategy. [#3374](https://github.com/microsoft/vstest/pull/3374)
* Fix architecture retrival [#3251](https://github.com/microsoft/vstest/pull/3251)
* External dependencies updated [#3204](https://github.com/microsoft/vstest/pull/3204)
* Fixing .net frmw parallel issue [#3192](https://github.com/microsoft/vstest/pull/3192)
* Testhost sharing between discovery & execution [#2687](https://github.com/microsoft/vstest/pull/2687)
* Cleanup tmp code for the --arch feature [#3174](https://github.com/microsoft/vstest/pull/3174)
* Make older uwp work [#3166](https://github.com/microsoft/vstest/pull/3166)
* Enable code coverage when "Code Coverage;arg1=val1;arg2=val2" is provided in cli [#3172](https://github.com/microsoft/vstest/pull/3172)
* Remove TargetPlatform before start test host [#3170](https://github.com/microsoft/vstest/pull/3170)
* Adding elements to code coverage config passed via commandline, [#3162](https://github.com/microsoft/vstest/pull/3162)* Aggregate api files [#3165](https://github.com/microsoft/vstest/pull/3165)
* Fix GenerateProgramFile [#3163](https://github.com/microsoft/vstest/pull/3163)
* Move new enum into public api [#3157](https://github.com/microsoft/vstest/pull/3157)
* Included UAP10.0 version of Microsoft.VisualStudio.TestPlatform.ObjectModel.dll for signing. [#3160](https://github.com/microsoft/vstest/pull/3160)
* Add package with UWP dependencies for UWP runner [#3133](https://github.com/microsoft/vstest/pull/3133)
* Honor `--arch` switch for arm64 on Windows and Mac [#3100](https://github.com/microsoft/vstest/pull/3100)
* Don't publish for win runtime identifier in source-build [#3096](https://github.com/microsoft/vstest/pull/3096)
* CPP runner under .NET (Core) [#3003](https://github.com/microsoft/vstest/pull/3003)
* Updating SDK versions [#3083](https://github.com/microsoft/vstest/pull/3083)
* Upgrade nuget packages to fix security issues [#3072](https://github.com/microsoft/vstest/pull/3072)

See full log [here](https://github.com/microsoft/vstest/compare/v17.0.0...v17.1.0)

### Drops

* TestPlatform vsix: [17.1.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.1/20220216-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.1.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.1.0)

## 17.2.0-preview-20220131-20

### Issues Fixed

* Removed system-wide PDB purge to make build faster [#3310](https://github.com/microsoft/vstest/pull/3310)
* Test stabilization, increase waiting time for report events [#3309](https://github.com/microsoft/vstest/pull/3309)
* Use longer timeout for playground project [#3301](https://github.com/microsoft/vstest/pull/3301)
* Remove stylecop files [#3308](https://github.com/microsoft/vstest/pull/3308)
* Enable some rules with no impact on public API [#3299](https://github.com/microsoft/vstest/pull/3299)
* Fix race condition inside DataCollectionAttachmentManager [#3296](https://github.com/microsoft/vstest/pull/3296)
* Remove unused properties and solution files [#3295](https://github.com/microsoft/vstest/pull/3295)
* Ignore project and build script re-format in blame [#3297](https://github.com/microsoft/vstest/pull/3297)
* Reformat projects and script files [#3290](https://github.com/microsoft/vstest/pull/3290)
* Recognize AnyCPU in case of fallback to PEReader [#3287](https://github.com/microsoft/vstest/pull/3287)
* Add Architecture.S390x [#3289](https://github.com/microsoft/vstest/pull/3289)
* Migrating to 1ES Hosted Pools [#3278](https://github.com/microsoft/vstest/pull/3278)
* Remove formatting changes from git blame [#3288](https://github.com/microsoft/vstest/pull/3288)
* Medium and low level vulnerabilities patched [#3286](https://github.com/microsoft/vstest/pull/3286)
* Apply modern code styles [#3264](https://github.com/microsoft/vstest/pull/3264)
* Add diag log env variable [#3275](https://github.com/microsoft/vstest/pull/3275)
* Fix encoding when writing updated dependencies in build [#3277](https://github.com/microsoft/vstest/pull/3277)
* Enable parallelization of acceptance tests [#3268](https://github.com/microsoft/vstest/pull/3268)
* Fix some typos in codebase [#3262](https://github.com/microsoft/vstest/pull/3262)
* Logs improvement [#3271](https://github.com/microsoft/vstest/pull/3271)
* Skip attachment processor if doesn't support incremental processing [#3270](https://github.com/microsoft/vstest/pull/3270)
* Fix change done in commit a223146b8c3d5dbbf7bae49149aed762560434c4 [#3258](https://github.com/microsoft/vstest/pull/3258)
* Improve logging for the architecture switch feature [#3265](https://github.com/microsoft/vstest/pull/3265)
* Improve the cache of the extension framework [#3261](https://github.com/microsoft/vstest/pull/3261)
* Improve error logging for VS output [#3260](https://github.com/microsoft/vstest/pull/3260)
* Ensure the Public API analyzer gets installed only for src projects [#3256](https://github.com/microsoft/vstest/pull/3256)
* Architecture test improvement [#3254](https://github.com/microsoft/vstest/pull/3254)
* Update system.net.http to 4.3.2 for uap10 [#3249](https://github.com/microsoft/vstest/pull/3249)
* Add tests for architecture switch feature [#3253](https://github.com/microsoft/vstest/pull/3253)
* Fixed manifest publishing [#3246](https://github.com/microsoft/vstest/pull/3246)
* Fix architecture retrival [#3250](https://github.com/microsoft/vstest/pull/3250)
* Complete the attachment processors extension [#3161](https://github.com/microsoft/vstest/pull/3161)
* Use the ToString format specifier rather than the Replace method [#3242](https://github.com/microsoft/vstest/pull/3242)
* Fixed manifest publishing
* Use stable channel for dotnet 6 installation [#3243](https://github.com/microsoft/vstest/pull/3243)
* Check exitcodes in build script [#3236](https://github.com/microsoft/vstest/pull/3236)
* improve attachvs output [#3230](https://github.com/microsoft/vstest/pull/3230)
* Add playground project [#3200](https://github.com/microsoft/vstest/pull/3200)
* Bump private dotnet version [#3228](https://github.com/microsoft/vstest/pull/3228)
* Bumped TP version to 17.2 [#3214](https://github.com/microsoft/vstest/pull/3214)
* Add marco as code owner for public api [#3217](https://github.com/microsoft/vstest/pull/3217)
* Add PublicAPI analyzer to all public projects [#3205](https://github.com/microsoft/vstest/pull/3205)

See full log [here](https://github.com/microsoft/vstest/compare/v17.1.0-preview-20211130-02...v17.2.0-preview-20220131-20)

### Drops

* TestPlatform vsix: [17.2.0-preview-20220131-20](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220131-20;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.2.0-preview-20220131-20](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.2.0-preview-20220131-20)

## 17.1.0-release-20220113-05

### Issue Fixed

* Fix architecture retrival [#3251](https://github.com/microsoft/vstest/pull/3251)

See full log [here](https://github.com/microsoft/vstest/compare/v17.1.0-preview-20211130-02...v17.1.0-release-20220113-05)

### Drops

* TestPlatform vsix: [17.1.0-release-20220113-05](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20220113-05;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.1.0-release-20220113-05](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.1.0-release-20220113-05)

## 17.1.0-preview-20211130-02

### Issue Fixed

* AttachVS PR comments fixed [#3201](https://github.com/microsoft/vstest/pull/3201)
* Attach to VS automatically [#3197](https://github.com/microsoft/vstest/pull/3197)
* Fixing .net frmw parallel issue [#3192](https://github.com/microsoft/vstest/pull/3192)
* Testhost sharing between discovery & execution [#2687](https://github.com/microsoft/vstest/pull/2687)
* Make older uwp work [#3166](https://github.com/microsoft/vstest/pull/3166)
* Enable code coverage when "Code Coverage;arg1=val1;arg2=val2" is provided in cli [#3172](https://github.com/microsoft/vstest/pull/3172)
* Remove TargetPlatform before start test host [#3170](https://github.com/microsoft/vstest/pull/3170)
* Adding elements to code coverage config passed via commandline, [#3162](https://github.com/microsoft/vstest/pull/3162)
* Aggregate api files [#3165](https://github.com/microsoft/vstest/pull/3165)
* Fix GenerateProgramFile [#3163](https://github.com/microsoft/vstest/pull/3163)
* Move new enum into public api [#3157](https://github.com/microsoft/vstest/pull/3157)

See full log [here](https://github.com/microsoft/vstest/compare/v17.1.0-preview-20211109-03...v17.1.0-preview-20211130-02)

### Drops

* TestPlatform vsix: [17.1.0-preview-20211130-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20211130-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.1.0-preview-20211130-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.1.0-preview-20211130-02)

## 17.1.0-preview-20211109-03

### Issues Fixed

* Honor `--arch` switch for arm64 on Windows and Mac [#3100](https://github.com/microsoft/vstest/pull/3100)
* CPP runner under .NET (Core) [#3003](https://github.com/microsoft/vstest/pull/3003)
* Log messages from RequestSender [#3057](https://github.com/microsoft/vstest/pull/3057)
* Fixed CVE-2018-8292 & CVE-2021-26701 [#3054](https://github.com/microsoft/vstest/pull/3054)
* --diag should take files with no extension [#3048](https://github.com/microsoft/vstest/pull/3048)
* Blame fix 32 bit hang dump [#3043](https://github.com/microsoft/vstest/pull/3043)
* Add public api analyzers for ObjectModel and dependencies [#3042](https://github.com/microsoft/vstest/pull/3042)

See full log [here](https://github.com/microsoft/vstest/compare/v17.0.0...v17.1.0-preview-20211109-03)

### Drops

* TestPlatform vsix: [17.1.0-preview-20211109-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/main/20211109-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.1.0-preview-20211109-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.1.0-preview-20211109-03)

## 17.0.0

See full log [here](https://github.com/microsoft/vstest/compare/v16.11.0...v17.0.0)

### Drops

* TestPlatform vsix: [17.0.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/17.0/20211022-05;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [17.0.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/17.0.0)

## 16.11.0

### Issues Fixed

* 16.11 External Packages Insertion [#2996](https://github.com/microsoft/vstest/pull/2996)
* Update externals to 16.11 [#2932](https://github.com/microsoft/vstest/pull/2932)

See full log [here](https://github.com/microsoft/vstest/compare/v16.10.0...v16.11.0)

### Drops

* TestPlatform vsix: [16.11.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.11/20210812-04;Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI.vsman)
* Microsoft.TestPlatform.ObjectModel : [16.11.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.11.0)

## 16.10.0-release-20210429-01

### Issues Fixed

* Adding mono.cecil libs to packages, (#2878) [#2878](https://github.com/microsoft/vstest/pull/2878)
* Upgrade CC components to 16.10.0-beta.21227.2 (#2877) [#2877](https://github.com/microsoft/vstest/pull/2877)

See full log [here](https://github.com/microsoft/vstest/compare/v16.10.0-release-20210330-02...v16.10.0-release-20210422-02)

### Drops

* TestPlatform vsix: [16.10.0-release-20210429-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.10/20210429-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [16.10.0-release-20210429-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.10.0-release-20210429-01)

## 16.10.0-release-20210422-02

### Issues Fixed

* Mark early testhost startup APIs as internal for TP 16.10 (#2768) [#2864](https://github.com/microsoft/vstest/pull/2864)
* Added some capabilities to package utilities (#2854) [#2862](https://github.com/microsoft/vstest/pull/2862)
* Fixed #2814 and #2853.
* Added support for WinUI3 appxrecipe. [#2849](https://github.com/microsoft/vstest/pull/2849)

See full log [here](https://github.com/microsoft/vstest/compare/v16.10.0-release-20210330-02...v16.10.0-release-20210422-02)

### Drops

* TestPlatform vsix: [16.10.0-release-20210422-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.10/20210422-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [16.10.0-release-20210422-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.10.0-release-20210422-02)

## 16.9.4

### Issue Fixed

* Loadind corelib.net dynamically (<https://github.com/microsoft/vstest/pull/2762>)

See full log [here](https://github.com/microsoft/vstest/compare/v16.9.1...v16.9.4)

### Drops

* TestPlatform vsix: [16.9.4](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.9/20210401-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [16.9.4](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.9.4)

## 16.10.0-release-20210330-02

### Issue Fixed

* Updating code coverage version [#2836](https://github.com/microsoft/vstest/pull/2836)

See full log [here](https://github.com/microsoft/vstest/compare/v16.10.0-release-20210329-03...v16.10.0-release-20210330-02)

### Drops

* TestPlatform vsix: [16.10.0-release-20210330-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.10/20210330-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel : [16.10.0-release-20210330-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.10.0-release-20210330-02)

## 16.10.0-release-20210329-03

### Issues Fixed

* Add basic mstestv1 telemetry [#2781](https://github.com/microsoft/vstest/pull/2781)
* Update TP externals [#2809](https://github.com/microsoft/vstest/pull/2809)

See full log [here](https://github.com/microsoft/vstest/compare/v16.10.0-preview-20210219-03...v16.10.0-release-20210329-03)

### Drops

* TestPlatform vsix: [16.10.0-release-20210329-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.10/20210329-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.10.0-release-20210329-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.10.0-release-20210329-03)

## 16.10.0-preview-20210219-03

### Issues Fixed

* Dynamic corelib.net loading [#2762](https://github.com/microsoft/vstest/pull/2762)
* Add failed logic for trx logger when TreatNoTestAsError is set to true [#2758](https://github.com/microsoft/vstest/pull/2758)
* Adding resources for corelib.net in portable package, [#2759](https://github.com/microsoft/vstest/pull/2759)
* Prefer agent temp directory if available. [#2752](https://github.com/microsoft/vstest/pull/2752)
* Generating cc attachments with correct uri [#2750](https://github.com/microsoft/vstest/pull/2750)
* Added support for spaces and other special characters in method names into the ManagedNameUtilities [#2738](https://github.com/microsoft/vstest/pull/2738)
* Don't swallow stacktrace from adapter exception when running in thread [#2746](https://github.com/microsoft/vstest/pull/2746)
* Fix duration in console logger for parallel tests [#2739](https://github.com/microsoft/vstest/pull/2739)
* Marked `InvalidManagedNameException` as serializable [#2732](https://github.com/microsoft/vstest/pull/2732)
* Print stack trace from executor [#2730](https://github.com/microsoft/vstest/pull/2730)
* Added constants for hierarchical naming. [#2724](https://github.com/microsoft/vstest/pull/2724)
* Fix divide by zero in HTML logger [#2723](https://github.com/microsoft/vstest/pull/2723)

See full log [here](https://github.com/microsoft/vstest/compare/v16.9.0-preview-20210127-04...v16.10.0-preview-20210219-03)

### Drops

* TestPlatform vsix: [16.10.0-preview-20210219-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.10/20210219-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.10.0-preview-20210219-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.10.0-preview-20210219-03)

## 16.9.1

### Issues Fixed

* Implemented Workitem support in TRX logger (#2666)
* Stopped merging code coverage logs (#2671)
* Early testhost startup performance improved (#2584)
* Removed TypesToLoadAttribute from ObjectModel, and moved the functionallity into adapters (#2674)
* Fixed assembly names of TestHost executables (#2682)
* Add metrics for datacollector.exe - provides information about profilers (#2705)
* Added `Microsoft.TestPlatform.AdapterUtilities`. (#2714)

See full log [here](https://github.com/microsoft/vstest/compare/v16.9.0-preview-20210127-04...v16.9.1)

### Drops

* TestPlatform vsix: [16.9.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/rel/16.9/20210223-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.9.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.9.1)

## 16.9.0-preview-20210127-04

### Issue Fixed

* Move FQN related code into a separate NuGet package [#2714](https://github.com/microsoft/vstest/pull/2714)
* vstest.console: CommandLineOptions: preserve stacktrace on re-throw (CA2200) [#2606](https://github.com/microsoft/vstest/pull/2606)
* Add metrics for datacollector.exe - provides information about profilers [#2705](https://github.com/microsoft/vstest/pull/2705)
* Loc Update [#2685](https://github.com/microsoft/vstest/pull/2685)

See full log [here](https://github.com/microsoft/vstest/compare/v16.9.0-preview-20210106-01...v16.9.0-preview-20210127-04)

### Drops

* TestPlatform vsix: [16.9.0-preview-20210127-04](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20210127-04;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.9.0-preview-20210127-04](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.9.0-preview-20210127-04)

## 16.9.0-preview-20210106-01

### Issues Fixed

* Upgrade CC to 16.9.0-beta.20630.1 [#2684](https://github.com/microsoft/vstest/pull/2684)
* Upgrade fakes version [#2683](https://github.com/microsoft/vstest/pull/2683)
* Fixed assembly names of TestHost executables [#2682](https://github.com/microsoft/vstest/pull/2682)
* Upgrade CC and CLR IE versions [#2681](https://github.com/microsoft/vstest/pull/2681)
* Update dependencies from <https://github.com/dotnet/arcade> build 20201221.2 [#2680](https://github.com/microsoft/vstest/pull/2680)
* Adding environment variable used during build process, [#2679](https://github.com/microsoft/vstest/pull/2679)
* Getting TraceDataCollector from nuget [#2678](https://github.com/microsoft/vstest/pull/2678)
* Attribute refactoring [#2676](https://github.com/microsoft/vstest/pull/2676)
* Removed TypesToLoadAttribute from ObjectModel. [#2674](https://github.com/microsoft/vstest/pull/2674)
* Early testhost startup performance work [#2584](https://github.com/microsoft/vstest/pull/2584)
* Do not merge logs from code coverage [#2671](https://github.com/microsoft/vstest/pull/2671)
* Implement Workitem support in TRX logger [#2666](https://github.com/microsoft/vstest/pull/2666)
* Bumping Fakes TestRunnerHarness version [#2661](https://github.com/microsoft/vstest/pull/2661)
* Fixed "issue" pluralization in write-release-notes.ps1 [#2665](https://github.com/microsoft/vstest/pull/2665)
* Updating Microsoft.VisualStudio.TraceDataCollector source [#2663](https://github.com/microsoft/vstest/pull/2663)
* Update dependencies from <https://github.com/dotnet/arcade> build 20201130.3 [#2659](https://github.com/microsoft/vstest/pull/2659)
* Cross platform acceptance tests [#2653](https://github.com/microsoft/vstest/pull/2653)
* Upgrade externals - remove interop [#2650](https://github.com/microsoft/vstest/pull/2650)

See full log [here](https://github.com/microsoft/vstest/compare/v16.9.0-preview-20201123-03...v16.9.0-preview-20210106-01)

### Drops

* TestPlatform vsix: [16.9.0-preview-20210106-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20210106-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.9.0-preview-20210106-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.9.0-preview-20210106-01)

## 16.9.0-preview-20201123-03

### Issues Fixed

* Added support for new CC merging library for netstandard2.0 [#2598](https://github.com/microsoft/vstest/pull/2598)
* [master] Fix collect dump always  [#2645](https://github.com/microsoft/vstest/pull/2645)
* Managed TestCase Properties implemented [#2611](https://github.com/microsoft/vstest/pull/2611)
* Use jsonSerializer2 for protocol version 3 [#2630](https://github.com/microsoft/vstest/pull/2630)
* Fixed assembly loading issue for netcoreapp on linux [#2636](https://github.com/microsoft/vstest/pull/2636)
* Fixed assembly loading issue for netcoreapp. [#2631](https://github.com/microsoft/vstest/pull/2631)
* Generation of CodeCoverage.deps.json file [#2627](https://github.com/microsoft/vstest/pull/2627)
* TP trace data collector changes to support CLR IE [#2618](https://github.com/microsoft/vstest/pull/2618)
* Enable linux build [#2617](https://github.com/microsoft/vstest/pull/2617)
* Implemented functionality to return non-zero value when no tests available. [#2610](https://github.com/microsoft/vstest/pull/2610)
* Ensure that a supplied vstest.console path is escape sequenced [#2600](https://github.com/microsoft/vstest/pull/2600)
* Temporary code to enable correct Fakes and Code Coverage integration [#2604](https://github.com/microsoft/vstest/pull/2604)
* netstandard1.0 and uap10.0 support [#2596](https://github.com/microsoft/vstest/pull/2596)

See full log [here](https://github.com/microsoft/vstest/compare/v16.9.0-preview-20201020-06...v16.9.0-preview-20201123-03)

### Drops

* TestPlatform vsix: [16.9.0-preview-20201123-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20201123-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.9.0-preview-20201123-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.9.0-preview-20201123-03)

## 16.8.3

### Issues Fixed

* Fixed PackageReferences on ObjectModel nupkg [#2660](https://github.com/microsoft/vstest/pull/2660)
* [16.8] Fix collect dump always [#2641](https://github.com/microsoft/vstest/pull/2641)
* Assembly load fixes [#2644](https://github.com/microsoft/vstest/pull/2644)

See full log [here](https://github.com/microsoft/vstest/compare/v16.8.0...v16.8.3)

### Drops

* TestPlatform vsix: [16.8.3](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.8/20201202-04;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.8.3](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.8.3)

## 16.8.0

### Issues Fixed

* Cherry-picked signing fixes from `master` [#2619](https://github.com/microsoft/vstest/pull/2619)
* Signing instructions for Newtonsoft.Json.dll added (#2601) [#2603](https://github.com/microsoft/vstest/pull/2603)
* Fix the initial assets location of VSTest assets [#2589](https://github.com/microsoft/vstest/pull/2589)
* Generate release notes in pipeline
* Forward merge fixes from master to rc2 [#2581](https://github.com/microsoft/vstest/pull/2581)
* Fix blame parameter, warning, and add all testhosts to be ngend [#2579](https://github.com/microsoft/vstest/pull/2579)
* Merge branch 'master' of <https://github.com/microsoft/vstest> into rel/16.8
* Blame upload on crash even if hang dump started [#2553](https://github.com/microsoft/vstest/pull/2553)
* Fix errors reported by StyleCop Analyzers [#2555](https://github.com/microsoft/vstest/pull/2555)
* Improve blame [#2552](https://github.com/microsoft/vstest/pull/2552)
* Remove sleeps and extra process dumps from blame
* Changes to allow special characters in parameter names [#2481](https://github.com/microsoft/vstest/pull/2481)
* Update dependencies from <https://github.com/dotnet/arcade> build 20200827.6 [#2547](https://github.com/microsoft/vstest/pull/2547)
* Update dependencies from <https://github.com/dotnet/arcade> build 20200827.2 [#2546](https://github.com/microsoft/vstest/pull/2546)
* Merge branch 'rel/16.8' of <https://github.com/microsoft/vstest> into rel/16.8
* Remove env variables
* Add binaries to enable running Fakes in Net Core [#2529](https://github.com/microsoft/vstest/pull/2529)
* Trigger dumps asynchronously [#2542](https://github.com/microsoft/vstest/pull/2542)
* Revert "Trigger dumps asynchronously (#2533)" [#2541](https://github.com/microsoft/vstest/pull/2541)
* Trigger dumps asynchronously [#2533](https://github.com/microsoft/vstest/pull/2533)
* Print version of the product in log [#2535](https://github.com/microsoft/vstest/pull/2535)
* Merge rel16.7 into master [#2534](https://github.com/microsoft/vstest/pull/2534)
* Print version of the product in log [#2536](https://github.com/microsoft/vstest/pull/2536)
* Print only whole version on release branch.
* Optionally force procdump [#2531](https://github.com/microsoft/vstest/pull/2531)
* Forward merge master
* Crash dumps via net client [#2532](https://github.com/microsoft/vstest/pull/2532)
* Added a command-line argument to vstest.console.exe for setting environment variables [#2528](https://github.com/microsoft/vstest/pull/2528)
* Fixed code coverage compatibility issue [#2527](https://github.com/microsoft/vstest/pull/2527)
* Custom dump path for helix [#2525](https://github.com/microsoft/vstest/pull/2525)
* Take non-completed tests instead of last [#2526](https://github.com/microsoft/vstest/pull/2526)
* Forgot to regenerate resources before commit, as always [#2524](https://github.com/microsoft/vstest/pull/2524)
* Fixing the reminder of crash dumps [#2520](https://github.com/microsoft/vstest/pull/2520)
* Dumping child processes [#2518](https://github.com/microsoft/vstest/pull/2518)
* Replace NET451 compiler directives with NETFRAMEWORK [#2516](https://github.com/microsoft/vstest/pull/2516)
* [master] Update dependencies from dotnet/arcade [#2509](https://github.com/microsoft/vstest/pull/2509)
* Updated TP external dependencies [#2515](https://github.com/microsoft/vstest/pull/2515)
* Add environment variables to enable MacOS dump
* Fixed code coverage compatibility issue [#2514](https://github.com/microsoft/vstest/pull/2514)
* Fixed TRX file overwrite in certain circumstances [#2508](https://github.com/microsoft/vstest/pull/2508)
* Use OS bitness to figure out .NETCore runner architecture [#2507](https://github.com/microsoft/vstest/pull/2507)
* Updated TP external dependencies [#2503](https://github.com/microsoft/vstest/pull/2503)
* Add missing space before parens in message [#2504](https://github.com/microsoft/vstest/pull/2504)
* CI failure fix [#2500](https://github.com/microsoft/vstest/pull/2500)
* Fix signing [#2497](https://github.com/microsoft/vstest/pull/2497)
* Add the new MacOs env variable to allow dumps to be created. [#2496](https://github.com/microsoft/vstest/pull/2496)
* Macos dumps [#2495](https://github.com/microsoft/vstest/pull/2495)
* Multitarget testhost [#2493](https://github.com/microsoft/vstest/pull/2493)
* Revert detecting default architecture, to allow dotnet to default to 64-bit [#2492](https://github.com/microsoft/vstest/pull/2492)
* Nuget.Frameworks renamed netcoreapp5.0 to net5.0 [#2491](https://github.com/microsoft/vstest/pull/2491)
* Console output for minimal and quiet [#2191](https://github.com/microsoft/vstest/pull/2191)
* Update License
* Linux build [#2477](https://github.com/microsoft/vstest/pull/2477)
* Windows 32 bit issue [#2482](https://github.com/microsoft/vstest/pull/2482)
* Update dependencies from <https://github.com/dotnet/arcade> build 20200715.6 [#2485](https://github.com/microsoft/vstest/pull/2485)
* Create test results directory [#2483](https://github.com/microsoft/vstest/pull/2483)
* Use testhost.exe only on Windows x86 and x64, and enable hang dumps on ARM and ARM64 [#2479](https://github.com/microsoft/vstest/pull/2479)
* Localization HB. [#2478](https://github.com/microsoft/vstest/pull/2478)
* Introduced acceptance tests for default exclusion merging [#2454](https://github.com/microsoft/vstest/pull/2454)
* Change indicators to words [#2475](https://github.com/microsoft/vstest/pull/2475)
* Adding test run attachments processing [#2463](https://github.com/microsoft/vstest/pull/2463)
* Adding test run attachments processing [#2463](https://github.com/microsoft/vstest/pull/2463)
* Localization check-in 07-01-2020 [#2471](https://github.com/microsoft/vstest/pull/2471)
* Update dependencies from <https://github.com/dotnet/arcade> build 20200626.2 [#2470](https://github.com/microsoft/vstest/pull/2470)
* Added new exception handling [#2461](https://github.com/microsoft/vstest/pull/2461)
* Update branding to 16.8.0 [#2460](https://github.com/microsoft/vstest/pull/2460)

See full log [here](https://github.com/microsoft/vstest/compare/v16.7.0...v16.8.0)

### Drops

* TestPlatform vsix: [16.8.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/v16.8.0;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.8.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.8.0)

## 16.9.0-preview-20201020-06

### Issues Fixed

* Enable Fakes Datacollector settings to be added in design mode [#2586](https://github.com/microsoft/vstest/pull/2586)
* Fix blame parameter, warning, and add all testhosts to be ngend [#2579](https://github.com/microsoft/vstest/pull/2579)
* Add netcoreapp1.0 support to `Microsoft.TestPlatform.TestHost` NuGet [#2569](https://github.com/microsoft/vstest/pull/2569)
* Use bitness from process or OS [#2571](https://github.com/microsoft/vstest/pull/2571)
* Restore netcoreapp1.0 support for testhost [#2554](https://github.com/microsoft/vstest/pull/2554)
* Get symbols of DiaSymReader from externals [#2560](https://github.com/microsoft/vstest/pull/2560)
* Do not force .NET4.5 in case legacy test settings are provided [#2545](https://github.com/microsoft/vstest/pull/2545)
* Simplify package references [#2559](https://github.com/microsoft/vstest/pull/2559)
* Enable default compile items for Microsoft.TestPlatform.PlatformAbstractions [#2556](https://github.com/microsoft/vstest/pull/2556)
* Avoid logging >Task returned false but did not log an error.< [#2557](https://github.com/microsoft/vstest/pull/2557)
* Fixed code coverage compatibility issue [#2527](https://github.com/microsoft/vstest/pull/2527)
* Add environment variables to enable MacOS dump
* Adding test run attachments processing [#2463](https://github.com/microsoft/vstest/pull/2463)

See full log [here](https://github.com/microsoft/vstest/compare/v16.8.0-release-20200921-02...v16.9.0-preview-20201020-06)

### Drops

* TestPlatform vsix: [16.9.0-preview-20201020-06](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20201020-06;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.9.0-preview-20201020-06](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.9.0-preview-20201020-06)

## 16.8.0-preview-20200921-01

### Issues Fixed

* Fix blame parameter, warning, and add all testhosts to be ngend [#2579](https://github.com/microsoft/vstest/pull/2579)
* Trigger dumps asynchronously [#2533](https://github.com/microsoft/vstest/pull/2533)
* Print version of the product in log [#2535](https://github.com/microsoft/vstest/pull/2535)

See full log [here](https://github.com/microsoft/vstest/compare/v16.8.0-preview-20200812-03...v16.8.0-preview-20200921-01)

### Drops

* TestPlatform vsix: [16.8.0-preview-20200921-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200921-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.8.0-preview-20200921-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.8.0-preview-20200921-01)

## 16.7.1

### Issues Fixed

* Fixed code coverage compatibility issue [#2527](https://github.com/microsoft/vstest/pull/2527)
* Adding test run attachments processing [#2463](https://github.com/microsoft/vstest/pull/2463)

See full log [here](https://github.com/microsoft/vstest/compare/v16.7.0...v16.7.1)

### Drops

* TestPlatform vsix: [16.7.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.7/20200819-04;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.7.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.7.1)

## 16.8.0-preview-20200812-03

### Issues Fixed

* Replace NET451 compiler directives with NETFRAMEWORK [#2516](https://github.com/Microsoft/vstest/pull/2516)
* [master] Update dependencies from dotnet/arcade [#2509](https://github.com/Microsoft/vstest/pull/2509)
* Updated TP external dependencies [#2515](https://github.com/Microsoft/vstest/pull/2515)
* Fixed code coverage compatibility issue [#2514](https://github.com/Microsoft/vstest/pull/2514)
* Fixed TRX file overwrite in certain circumstances [#2508](https://github.com/Microsoft/vstest/pull/2508)

See full log [here](https://github.com/Microsoft/vstest/compare/v16.8.0-preview-20200806-02...v16.8.0-preview-20200812-03)

### Drops

* TestPlatform vsix: [16.8.0-preview-20200812-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200812-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.8.0-preview-20200812-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.8.0-preview-20200812-03)

## 16.8.0-preview-20200806-02

### Issues Fixed

* Use OS bitness to figure out .NETCore runner architecture [#2507](https://github.com/microsoft/vstest/pull/2507)
* Updated TP external dependencies [#2503](https://github.com/microsoft/vstest/pull/2503)
* Add missing space before parens in message [#2504](https://github.com/microsoft/vstest/pull/2504)
* CI failure fix [#2500](https://github.com/microsoft/vstest/pull/2500)
* Fix signing [#2497](https://github.com/microsoft/vstest/pull/2497)
* Add the new MacOs env variable to allow dumps to be created. [#2496](https://github.com/microsoft/vstest/pull/2496)
* Macos dumps [#2495](https://github.com/microsoft/vstest/pull/2495)
* Multitarget testhost [#2493](https://github.com/microsoft/vstest/pull/2493)
* Revert detecting default architecture, to allow dotnet to default to 64-bit [#2492](https://github.com/microsoft/vstest/pull/2492)
* Nuget.Frameworks renamed netcoreapp5.0 to net5.0 [#2491](https://github.com/microsoft/vstest/pull/2491)
* Console output for minimal and quiet [#2191](https://github.com/microsoft/vstest/pull/2191)
* Update License
* Linux build [#2477](https://github.com/microsoft/vstest/pull/2477)
* Windows 32 bit issue [#2482](https://github.com/microsoft/vstest/pull/2482)
* Update dependencies from <https://github.com/dotnet/arcade> build 20200715.6 [#2485](https://github.com/microsoft/vstest/pull/2485)
* Create test results directory [#2483](https://github.com/microsoft/vstest/pull/2483)
* Use testhost.exe only on Windows x86 and x64, and enable hang dumps on ARM and ARM64 [#2479](https://github.com/microsoft/vstest/pull/2479)
* Localization HB. [#2478](https://github.com/microsoft/vstest/pull/2478)
* Introduced acceptance tests for default exclusion merging [#2454](https://github.com/microsoft/vstest/pull/2454)
* Change indicators to words [#2475](https://github.com/microsoft/vstest/pull/2475)
* Adding test run attachments processing [#2463](https://github.com/microsoft/vstest/pull/2463)
* Localization check-in 07-01-2020 [#2471](https://github.com/microsoft/vstest/pull/2471)
* Update dependencies from <https://github.com/dotnet/arcade> build 20200626.2 [#2470](https://github.com/microsoft/vstest/pull/2470)
* Added new exception handling [#2461](https://github.com/microsoft/vstest/pull/2461)
* Update branding to 16.8.0 [#2460](https://github.com/microsoft/vstest/pull/2460)
* Update dependencies from <https://github.com/dotnet/arcade> build 20200602.3 [#2455](https://github.com/microsoft/vstest/pull/2455)
* Added exception handling while creating "TestResults" folder [#2450](https://github.com/microsoft/vstest/pull/2450)
* Added support for default exclusion merging for code coverage [#2431](https://github.com/microsoft/vstest/pull/2431)
* LOC CHECKIN | microsoft/vstest master | 20200526 [#2445](https://github.com/microsoft/vstest/pull/2445)
* Generate xlf for blame [#2442](https://github.com/microsoft/vstest/pull/2442)

See full log [here](https://github.com/microsoft/vstest/compare/v16.7.0-preview-20200519-01...v16.8.0-preview-20200806-02)

### Drops

* TestPlatform vsix: [16.8.0-preview-20200806-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200806-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.8.0-preview-20200806-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.8.0-preview-20200806-02)

## 16.7.0

### Issues Fixed

* Added new exception handling [#2461](https://github.com/microsoft/vstest/pull/2461)
* Added exception handling while creating "TestResults" folder [#2450](https://github.com/microsoft/vstest/pull/2450)
* Added support for default exclusion merging for code coverage [#2431](https://github.com/microsoft/vstest/pull/2431)
* LOC CHECKIN | microsoft/vstest master | 20200526 [#2445](https://github.com/microsoft/vstest/pull/2445)
* Generate xlf for blame [#2442](https://github.com/microsoft/vstest/pull/2442)
* Upgrade TestPlatform external dependencies [#2440](https://github.com/microsoft/vstest/pull/2440)
* Added support for debugging external test processes [#2325](https://github.com/microsoft/vstest/pull/2325)
* Add the parameters to VSTestTask to allow dotnet test to work [#2438](https://github.com/microsoft/vstest/pull/2438)
* Add hangdump and crash dump capabilities and options [#2434](https://github.com/microsoft/vstest/pull/2434)
* Update arcade tooling to latest [#2436](https://github.com/microsoft/vstest/pull/2436)
* More verbose info in datacollector log [#2430](https://github.com/microsoft/vstest/pull/2430)
* Support /TestCaseFilter and /Tests arguments at the same time [#2371](https://github.com/microsoft/vstest/pull/2371)
* Wrap error message/stack trace content in `<pre>` [#2419](https://github.com/microsoft/vstest/pull/2419)
* Update telemetry to latest [#2421](https://github.com/microsoft/vstest/pull/2421)
* Merge test run parameters that have spaces [#2409](https://github.com/microsoft/vstest/pull/2409)
* update externals [#2406](https://github.com/microsoft/vstest/pull/2406)
* VS Depencencies from 16.7.0 signed build [#2382](https://github.com/microsoft/vstest/pull/2382)
* Changed new configurator method name (#2397) [#2403](https://github.com/microsoft/vstest/pull/2403)
* Fix null reference [#2401](https://github.com/microsoft/vstest/pull/2401)
* Changed new configurator method name [#2398](https://github.com/microsoft/vstest/pull/2398)

See full log [here](https://github.com/microsoft/vstest/compare/v16.6.0...v16.7.0)

### Drops

* TestPlatform vsix: [16.7.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/v16.7.0;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.7.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.7.0)

## 16.7.0-preview-20200519-01

### Issues Fixed

* Upgrade TestPlatform external dependencies [#2440](https://github.com/microsoft/vstest/pull/2440)
* Added support for debugging external test processes [#2325](https://github.com/microsoft/vstest/pull/2325)
* Add the parameters to VSTestTask to allow dotnet test to work [#2438](https://github.com/microsoft/vstest/pull/2438)
* Add hangdump and crash dump capabilities and options [#2434](https://github.com/microsoft/vstest/pull/2434)
* Update arcade tooling to latest [#2436](https://github.com/microsoft/vstest/pull/2436)
* More verbose info in datacollector log [#2430](https://github.com/microsoft/vstest/pull/2430)
* Support /TestCaseFilter and /Tests arguments at the same time [#2371](https://github.com/microsoft/vstest/pull/2371)
* Wrap error message/stack trace content in `<pre>` [#2419](https://github.com/microsoft/vstest/pull/2419)

See full log [here](https://github.com/microsoft/vstest/compare/v16.7.0-preview-20200428-01...v16.7.0-preview-20200519-01)

### Drops

* TestPlatform vsix: [16.7.0-preview-20200519-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200519-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.7.0-preview-20200519-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.7.0-preview-20200519-01)

## 16.7.0-preview-20200428-01

### Issues Fixed

* Update telemetry to latest [#2421](https://github.com/microsoft/vstest/pull/2421)
* Merge test run parameters that have spaces [#2409](https://github.com/microsoft/vstest/pull/2409)
* updated package version [#2412](https://github.com/microsoft/vstest/pull/2412)
* update externals [#2406](https://github.com/microsoft/vstest/pull/2406)
* VS Depencencies from 16.7.0 signed build [#2382](https://github.com/microsoft/vstest/pull/2382)
* Changed new configurator method name (#2397) [#2403](https://github.com/microsoft/vstest/pull/2403)
* Fix null reference [#2401](https://github.com/microsoft/vstest/pull/2401)
* Changed new configurator method name [#2398](https://github.com/microsoft/vstest/pull/2398)

See full log [here](https://github.com/microsoft/vstest/compare/v16.6.1...v16.7.0-preview-20200428-01)

### Drops

* TestPlatform vsix: [16.7.0-preview-20200428-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200428-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.7.0-preview-20200428-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.7.0-preview-20200428-01)

## 16.6.1

### Issues Fixed

* Fix fakes version [#2412](https://github.com/microsoft/vstest/pull/2412)

See full log [here](https://github.com/microsoft/vstest/compare/v16.6.0...v16.6.1)

### Drops

* TestPlatform vsix: [16.6.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.6/20200423-06;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.6.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.6.1)

## 16.6.0

### Issues Fixed

* Fix null reference in Fakes [#2400](https://github.com/microsoft/vstest/pull/2400)
* Changed new configurator method name [#2397](https://github.com/microsoft/vstest/pull/2397)
* Fixes Test Platform. [#2393](https://github.com/microsoft/vstest/pull/2393)
* Fixing a typo for the method arguments for the Fakes utility method. [#2385](https://github.com/microsoft/vstest/pull/2385)
* Ignore flaky test [#2386](https://github.com/microsoft/vstest/pull/2386)
* LOC CHECKIN | Microsoft/vstest master | 20200403 [#2383](https://github.com/microsoft/vstest/pull/2383)
* Upgrade CppUnitTestFramework to newest version [#2381](https://github.com/microsoft/vstest/pull/2381)
* Added method to look for new api in fakes datacollector [#2339](https://github.com/microsoft/vstest/pull/2339)
* Take TestCaseFilter from runsettings [#2356](https://github.com/microsoft/vstest/pull/2356)
* Pin dotnet [#2373](https://github.com/microsoft/vstest/pull/2373)
* Fix writing to trx when error has no message [#2364](https://github.com/microsoft/vstest/pull/2364)
* Fix symbols [#2363](https://github.com/microsoft/vstest/pull/2363)
* Report informational messages when platform logs are enabled [#2361](https://github.com/microsoft/vstest/pull/2361)
* Add option to specify custom test host [#2359](https://github.com/microsoft/vstest/pull/2359)
* Fix running self-contained apps on Windows [#2358](https://github.com/microsoft/vstest/pull/2358)
* Remove unused usings. [#2350](https://github.com/microsoft/vstest/pull/2350)
* Better error when discoverer defaultExecutorUri is not set. [#2354](https://github.com/microsoft/vstest/pull/2354)
* Add coverlet smoke test [#2348](https://github.com/microsoft/vstest/pull/2348)
* Fix splitting of test name from fully qualified name [#2355](https://github.com/microsoft/vstest/pull/2355)
* Spelling / conventions and grammar fixes [#2338](https://github.com/microsoft/vstest/pull/2338)
* Small build fixes [#2345](https://github.com/microsoft/vstest/pull/2345)
* Fix race condition on testhost exit before we connect [#2344](https://github.com/microsoft/vstest/pull/2344)
* Move test publish to the bottom [#2342](https://github.com/microsoft/vstest/pull/2342)
* Do not crash on Debug.Assert [#2335](https://github.com/microsoft/vstest/pull/2335)
* Switch arguments for expected and actual in Assert.AreEquals in multiple tests [#2329](https://github.com/microsoft/vstest/pull/2329)
* Run acceptance tests against the locally built sources [#2340](https://github.com/microsoft/vstest/pull/2340)

See full log [here](https://github.com/microsoft/vstest/compare/v16.5.0...v16.6.0)

### Drops

* TestPlatform vsix: [16.6.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/16.6/20200414-04;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.6.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.6.0)

## 16.6.0-preview-20200318-01

### Issues Fixed

* Fix writing to trx when error has no message [#2364](https://github.com/microsoft/vstest/pull/2364)
* Fix symbols [#2363](https://github.com/microsoft/vstest/pull/2363)
* Report informational messages when platform logs are enabled [#2361](https://github.com/microsoft/vstest/pull/2361)
* Add option to specify custom test host [#2359](https://github.com/microsoft/vstest/pull/2359)

See full log [here](https://github.com/microsoft/vstest/compare/v16.6.0-preview-20200310-03...v16.6.0-preview-20200318-01)

### Drops

* TestPlatform vsix: [16.6.0-preview-20200318-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200318-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.6.0-preview-20200318-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.6.0-preview-20200318-01)

## 16.6.0-preview-20200310-03

### Issues Fixed

* Fix running self-contained apps on Windows [#2358](https://github.com/microsoft/vstest/pull/2358)
* Remove unused usings. [#2350](https://github.com/microsoft/vstest/pull/2350)
* Better error when discoverer defaultExecutorUri is not set. [#2354](https://github.com/microsoft/vstest/pull/2354)

See full log [here](https://github.com/microsoft/vstest/compare/v16.6.0-preview-20200309-01...v16.6.0-preview-20200310-03)

### Drops

* TestPlatform vsix: [16.6.0-preview-20200310-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200310-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.6.0-preview-20200310-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.6.0-preview-20200310-03)

## 16.6.0-preview-20200309-01

### Issues Fixed

* Add coverlet smoke test [#2348](https://github.com/microsoft/vstest/pull/2348)
* Fix splitting of test name from fully qualified name [#2355](https://github.com/microsoft/vstest/pull/2355)

See full log [here](https://github.com/microsoft/vstest/compare/v16.6.0-preview-20200226-03...v16.6.0-preview-20200309-01)

### Drops

* TestPlatform vsix: [16.6.0-preview-20200309-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200309-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.6.0-preview-20200309-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.6.0-preview-20200309-01)

## 16.6.0-preview-20200226-03

### Issues Fixed

* Spelling / conventions and grammar fixes [#2338](https://github.com/microsoft/vstest/pull/2338)
* Small build fixes [#2345](https://github.com/microsoft/vstest/pull/2345)
* Fix race condition on testhost exit before we connect [#2344](https://github.com/microsoft/vstest/pull/2344)
* Move test publish to the bottom [#2342](https://github.com/microsoft/vstest/pull/2342)
* Do not crash on Debug.Assert [#2335](https://github.com/microsoft/vstest/pull/2335)
* Switch arguments for expected and actual in Assert.AreEquals in multiple tests [#2329](https://github.com/microsoft/vstest/pull/2329)
* Run acceptance tests against the locally built sources [#2340](https://github.com/microsoft/vstest/pull/2340)

See full log [here](https://github.com/microsoft/vstest/compare/v16.5.0-preview-20200203-01...v16.6.0-preview-20200226-03)

### Drops

* TestPlatform vsix: [16.6.0-preview-20200226-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200226-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.6.0-preview-20200226-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.6.0-preview-20200226-03)

## 16.5.0

### Issues fixed (since 16.4.0)

* Use version of external package with fixes [#2315](https://github.com/microsoft/vstest/pull/2315)
* Use latest version of VS that is available [#2314](https://github.com/microsoft/vstest/pull/2314)
* Pass coverlet codebase in runsettings for inproc data collector initialization [#2288](https://github.com/microsoft/vstest/pull/2288)
* Make --verbosity case insensitive [#2300](https://github.com/microsoft/vstest/pull/2300)
* Revert "Use patched version of TestPlatform.Extensions (#2283)" [#2307](https://github.com/microsoft/vstest/pull/2307)
* Update arcade [#2302](https://github.com/microsoft/vstest/pull/2302)
* Fix SocketCommunicationManager [#2290](https://github.com/microsoft/vstest/pull/2290)
* Use patched version of TestPlatform.Extensions [#2283](https://github.com/microsoft/vstest/pull/2283)
* Cap version of VS to <16.0 [#2285](https://github.com/microsoft/vstest/pull/2285)
* Remove duplicate counting of test results in Consolelogger [#2267](https://github.com/microsoft/vstest/pull/2267)
* Test run parameter added as part of CLI runsettings args [#2251](https://github.com/microsoft/vstest/pull/2251)
* Initialize only coverlet data collector [#2274](https://github.com/microsoft/vstest/pull/2274)
* Use RunSettingsFilePath from when using dotnet test [#2272](https://github.com/microsoft/vstest/pull/2272)
* Eqt trace error was thrown if extension uri is not given [#2264](https://github.com/microsoft/vstest/pull/2264)
* Fix for discovery not working on Mac machines [#2266](https://github.com/microsoft/vstest/pull/2266)
* Disable reusing nodes when building localization [#2268](https://github.com/microsoft/vstest/pull/2268)
* Trx changes for fqdn mapping in test method name [#2259](https://github.com/microsoft/vstest/pull/2259)
* Expand environment variables in codeBase before loading extension [#1871](https://github.com/microsoft/vstest/pull/1871)
* Coverlet in-process collector is not loaded for version > 1.0.0 [#2221](https://github.com/microsoft/vstest/pull/2221)
* fix path in ngen [#2246](https://github.com/microsoft/vstest/pull/2246)
* LOC CHECKIN | Microsoft/vstest master | 20191104 [#2241](https://github.com/microsoft/vstest/pull/2241)
* Move Tp version to 16.5 [#2243](https://github.com/microsoft/vstest/pull/2243)
* Add support for an arg to enable progress indicator, disabled by default. [#2234](https://github.com/microsoft/vstest/pull/2234)
* Correct name and link for RFC 17 [#2232](https://github.com/microsoft/vstest/pull/2232)

See full log [here](https://github.com/microsoft/vstest/compare/v16.4.0...v16.5.0)
See changes since the last preview [here](https://github.com/microsoft/vstest/compare/16.5.0-preview-20200203-01...v16.5.0)

### Drops

* TestPlatform vsix: [16.5.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200205-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.5.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.5.0)

## 16.5.0-preview-20200203-01

### Issues Fixed

* Use version of external package with fixes [#2315](https://github.com/microsoft/vstest/pull/2315)
* Use latest version of VS that is available [#2314](https://github.com/microsoft/vstest/pull/2314)
* Pass coverlet codebase in runsettings for inproc data collector initialization [#2288](https://github.com/microsoft/vstest/pull/2288)
* Make --verbosity case insensitive [#2300](https://github.com/microsoft/vstest/pull/2300)

See full log [here](https://github.com/microsoft/vstest/compare/v16.5.0-preview-20200116-01...v16.5.0-preview-20200203-01)

### Drops

* TestPlatform vsix: [16.5.0-preview-20200203-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200203-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.5.0-preview-20200203-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.5.0-preview-20200203-01)  

## 16.5.0-preview-20200116-01

### Issues Fixed

* Revert "Use patched version of TestPlatform.Extensions (#2283)" [#2307](https://github.com/microsoft/vstest/pull/2307)
* Update arcade [#2302](https://github.com/microsoft/vstest/pull/2302)
* Fix SocketCommunicationManager [#2290](https://github.com/microsoft/vstest/pull/2290)

See full log [here](https://github.com/Microsoft/vstest/compare/v16.5.0-preview-20200110-02...v16.5.0-preview-20200116-01)

### Drops

* TestPlatform vsix: [16.5.0-preview-20200116-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200116-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.5.0-preview-20200116-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.5.0-preview-20200116-01)

## 16.5.0-preview-20200110-02

### Issues Fixed

* Remove duplicate counting of test results in Consolelogger [#2267](https://github.com/microsoft/vstest/pull/2267)
* Cap version of VS to <16.0 [#2285](https://github.com/microsoft/vstest/pull/2285)
* Use patched version of TestPlatform.Extensions [#2283](https://github.com/microsoft/vstest/pull/2283)

See full log [here](https://github.com/Microsoft/vstest/compare/v16.5.0-preview-20200102-01...v16.5.0-preview-20200110-02)

### Drops

* TestPlatform vsix: [16.5.0-preview-20200110-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200110-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.5.0-preview-20200110-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.5.0-preview-20200110-02)

## 16.5.0-preview-20200102-01

### Issues Fixed

* Test run parameter added as part of CLI runsettings args [#2251](https://github.com/microsoft/vstest/pull/2251)

### Drops

* TestPlatform vsix: [16.5.0-preview-20200102-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20200102-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.5.0-preview-20200102-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.5.0-preview-20200102-01)

## 16.5.0-preview-20191216-02

### Issues Fixed

* Trx changes for fqdn mapping in test method name [#2259](https://github.com/microsoft/vstest/pull/2259)
* Fix for test discovery not working on mac machines [#2266](https://github.com/microsoft/vstest/pull/2266)
* Use RunSettingsFilePath from project file when using dotnet test [#2272](https://github.com/microsoft/vstest/pull/2272)

### Drops

* TestPlatform vsix: [16.5.0-preview-20191216-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20191216-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.5.0-preview-20191216-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.5.0-preview-20191216-02)

## 16.5.0-preview-20191115-01

### Issues Fixed

* Fixed Coverlet in-process collector not loaded for version > 1.0.0 [#2204](https://github.com/microsoft/vstest/pull/2221)

### Drops

* TestPlatform vsix: [16.5.0-preview-20191115-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20191115-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.5.0-preview-20191115-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.5.0-preview-20191115-01)

## 16.4.0

### Issues Fixed

* Adding log prefixkey to html logger [#2204](https://github.com/microsoft/vstest/pull/2204)
* AnyCPU tests to choose default architecture based on process [#2206](https://github.com/microsoft/vstest/pull/2206)
* Only send Coverlet in proc datacollector dll to testhost [#2226](https://github.com/microsoft/vstest/pull/2226)
* Missing Cancel Implementation [#2227](https://github.com/microsoft/vstest/pull/2227)

### Drops

* TestPlatform vsix: [16.4.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20191025-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.4.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.4.0)

## 16.4.0-preview-20191007-01

### Issues Fixed / Features Added

* Redirect procdump process output to diag files [#2181](https://github.com/microsoft/vstest/pull/2181)
* Implemented cancellation of individual source files discovery [#2134](https://github.com/microsoft/vstest/pull/2134)
* Enabling native code debugging of test host [#2190](https://github.com/microsoft/vstest/pull/2190)
* Logging Adapter Load issues to console [#2156](https://github.com/microsoft/vstest/pull/2156)
* Fixed DataCollector to load with only uri (and not friendly name) specified in Runsettings [#2177](https://github.com/microsoft/vstest/pull/2177)
* Added env var support to blame test results directory path and fixed blame aborting without killing the test host process on hang timeout when there is an error with dump collection/attachment [#2216](https://github.com/microsoft/vstest/pull/2216)

## 16.3.0

### Issues Fixed

* Html logger [#2103](https://github.com/microsoft/vstest/pull/2103)
* Add LogFilePrefix Parameter for supporting trx for multi-targetted projects [#2140](https://github.com/microsoft/vstest/pull/2140)
* Support x86 platform targeting for .NET core tests [#2161](https://github.com/microsoft/vstest/pull/2161)
* Add logging for Translation layer [#2166](https://github.com/microsoft/vstest/pull/2166)

### Drops

* TestPlatform vsix: [16.3.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20190919-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.3.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.3.0)

## 16.3.0-preview-20190828-03

### Issues Fixed

* Add noprogress parameter to disable progress indicator [#2117](https://github.com/microsoft/vstest/pull/2117)
* Accept short names for framewwork [#2116](https://github.com/microsoft/vstest/pull/2116)
* Specifying environment variables in RunSettings file [#2128](https://github.com/microsoft/vstest/pull/2128)
* VsTestConsoleWrapper endsession should shut down vstest console process [#2145](https://github.com/microsoft/vstest/pull/2145)

### Drops

* TestPlatform vsix: [16.3.0-preview-20190828-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20190828-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.3.0-preview-20190828-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.3.0-preview-20190828-03)

## 16.3.0-preview-20190715-02

### Issues Fixed

* TestPlatform targeting netstandard2.0. [#2076](https://github.com/microsoft/vstest/pull/2076)
* Implemented the cancellation of discovery request [#2076](https://github.com/microsoft/vstest/pull/2076)
* Generating manifest for publishing to BAR. [#2069](https://github.com/microsoft/vstest/pull/2069)

### Drops

* TestPlatform vsix: [16.3.0-preview-20190715-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20190715-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.3.0-preview-20190715-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.3.0-preview-20190715-02)

## 16.2.0

### Issues Fixed

* Updated TestPlatform.ObjectModel.nuspec. [#2055](https://github.com/microsoft/vstest/pull/2055)
* Fixed incorrect timeout message when test host crashes [#2056](https://github.com/microsoft/vstest/pull/2056)
* Incompatible framework message fix. [#2044](https://github.com/microsoft/vstest/pull/2044)
* Cleaned up remaining set of dependencies for source build. [#2058](https://github.com/microsoft/vstest/pull/2058)

### Drops

* TestPlatform vsix: [16.2.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/microsoft/vstest/master/20190626-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.2.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.2.0)

## 16.2.0-preview-20190606-02

### Issues Fixed

* Spurious vstest.console process spin up fixed. [#2035](https://github.com/microsoft/vstest/pull/2035)
* Test host locking pdb fixed [#2029](https://github.com/microsoft/vstest/pull/2029)
* Encoding change from UCS-2 to UTF-8. [#2044](https://github.com/microsoft/vstest/pull/2044)
* Unable to find Microsoft.VisualStudio.ArchitectureTools.PEReader fixed. [#2008](https://github.com/microsoft/vstest/pull/2008)

### Drops

* TestPlatform vsix: [16.2.0-preview-20190606-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20190606-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.2.0-preview-20190606-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.2.0-preview-20190606-02)

## 16.1.1

### Issues Fixed

* Prevent unnecessary progress indicator refresh to improve test run time. [#2024](https://github.com/microsoft/vstest/pull/2024)
* Changes to allow clients to provide environment variable while initializing VsTestConsoleWrapper [#2023](https://github.com/microsoft/vstest/pull/2023)
* Fix for the trx classname being wrongly stamped when testname and fullyqualifiedname are same. [#2014](https://github.com/microsoft/vstest/pull/2014)
* Search datacollectors in output directory as well. [#2015](https://github.com/microsoft/vstest/pull/2015)
* Changes to avoid restoring of packages that are not required for the BuildFromSource scenario. [#2017](https://github.com/microsoft/vstest/pull/2017)

### Drops

* TestPlatform vsix: [16.1.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20190529-04;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.1.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.1.1)

## 16.0.2-Preview-20190502-01

### Issues Fixed

* Improve the cli experience for dotnet test. [#1964](https://github.com/Microsoft/vstest/pull/1964)
* Improve readability of dotnet test [#1960](https://github.com/Microsoft/vstest/pull/1960)
* Make testhost.x86 large address aware [#1986](https://github.com/Microsoft/vstest/pull/1986)
* Vstest.console Should not message to Testhost process if it has exited [#1994](https://github.com/Microsoft/vstest/pull/1994)
* [Revert] Fix for dotnet test on a multi-target projects logs only the last target [#1996](https://github.com/Microsoft/vstest/pull/1996)
* [Trxlogger] Fixing the code to preserve newline for adapter logs to stdout [#1999](https://github.com/Microsoft/vstest/pull/1999)

### Drops

* TestPlatform vsix: [16.0.2-preview-20190502-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20190502-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.0.2-preview-20190502-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.0.2-preview-20190502-01)

## 16.0.1

### Issues Fixed

* Reverted aborting test run when source and target frameworks/architectures are incompatible. [#1935](https://github.com/Microsoft/vstest/pull/1935)

### Drops

* TestPlatform vsix: [16.0.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20190304-05;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.0.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.0.1)

## 16.0.0

### Issues Fixed

* Added missing Utilities dependency to netstandard1.5 [#1913](https://github.com/Microsoft/vstest/pull/1913)
* Add support for xplat vstest console in translationlayer [#1893](https://github.com/Microsoft/vstest/pull/1893)
* Aborting test run when source and target frameworks/architectures are incompatible. [#1789](https://github.com/Microsoft/vstest/pull/1789)

### Drops

* TestPlatform vsix: [16.0.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20190228-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.0.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.0.0)

## 16.0.0-preview-20190201-03

### Issues Fixed

* Running NETFramework 3.5 tests in compat mode [#1906](https://github.com/Microsoft/vstest/pull/1906)
* Make timeouts for translation layer timeout configurable. [#1909](https://github.com/Microsoft/vstest/pull/1909)

### Drops

* TestPlatform vsix: [16.0.0-preview-20190201-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20190201-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.0.0-preview-20190201-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.0.0-preview-20190201-03)

## 16.0.0-preview-20190124-02

### Issues Fixed

* Downgrade Test.Sdk to net40 [#1860](https://github.com/Microsoft/vstest/pull/1860)
* Fix xml exception when we are dealing with special chars [#1872](https://github.com/Microsoft/vstest/pull/1872)
* Fix - dotnet test on a multi-target projects logs only the last target [#1877](https://github.com/Microsoft/vstest/pull/1877)
* Avoid usage of JsonConvert in test host process [#1881](https://github.com/Microsoft/vstest/pull/1881)
* Fixing logging error in event sources [#1897](https://github.com/Microsoft/vstest/pull/1897)

### Drops

* TestPlatform vsix: [16.0.0-preview-20190124-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20190124-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.0.0-preview-20190124-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.0.0-preview-20190124-02)

## 16.0.0-preview-20181205-02

### Issues Fixed

* Stop trying to connect if the test host exits unexpectedly [#1853](https://github.com/Microsoft/vstest/pull/1853)
* Move warning into a target to fix msbuild error [#1856](https://github.com/Microsoft/vstest/pull/1856)
* Adding the missing assemblyInfo files and updating the copyrights [#1859](https://github.com/Microsoft/vstest/pull/1859)

### Drops

* TestPlatform vsix: [16.0.0-preview-20181205-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20181205-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.0.0-preview-20181205-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.0.0-preview-20181205-02)

## 16.0.0-preview-20181128-01

### Issues Fixed

* Allow external use of the TRX Logger [#1792](https://github.com/Microsoft/vstest/pull/1792)
* Add "!~" operator to test filter [#1803](https://github.com/Microsoft/vstest/pull/1803)
* Simplify SDK languages support [#1804](https://github.com/Microsoft/vstest/pull/1804)
* Make Translation Layer connection timeout configurable [#1843](https://github.com/Microsoft/vstest/pull/1843)
* Fixed issue where proc dump was not getting terminated on no crash [#1849](https://github.com/Microsoft/vstest/pull/1849)

### Drops

* TestPlatform vsix: [16.0.0-preview-20181128-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20181128-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [16.0.0-preview-20181128-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/16.0.0-preview-20181128-01)

## 15.9.0

### Issues Fixed

* Unstable testId for nunit tests [#1785](https://github.com/Microsoft/vstest/pull/1785)
* Run tests only for test projects [#1745](https://github.com/Microsoft/vstest/pull/1745)
* Add info log if try to run tests with no IsTestProject prop [#1778](https://github.com/Microsoft/vstest/pull/1778)

### Drops

* TestPlatform vsix: [15.9.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.9/20181008-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.9.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.9.0)

## 15.9.0-preview-20180924-03

### Issues Fixed

* Fix Video Datacollector errors [#1719](https://github.com/Microsoft/vstest/pull/1719)
* Show error message on Framework35 [#1723](https://github.com/Microsoft/vstest/pull/1723)
* Suggest publish for running on an isolated machine[#1726](https://github.com/Microsoft/vstest/pull/1726)
* Fix UWP tests app socket exception [#1728](https://github.com/Microsoft/vstest/pull/1728)
* Run tests only for test projects in "dotnet test my.sln" scenario [#1745](https://github.com/Microsoft/vstest/pull/1745)

### Drops

* TestPlatform vsix: [15.9.0-preview-20180924-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180924-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.9.0-preview-20180924-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.9.0-preview-20180924-03)

## 15.9.0-preview-20180807-05
 
### Issues Fixed

* Fix for VSTest to honor /nologo user input from dotnet cli [#1717](https://github.com/Microsoft/vstest/pull/1717)
* Fixed ISettingsProvider in TestAdapter assembly [#1704](https://github.com/Microsoft/vstest/pull/1704)
* Added `.NETCoreApp,Version=v2.0` example in error message [#1714](https://github.com/Microsoft/vstest/pull/1714)
* Print start of testhost standard error stacktrace [#1708](https://github.com/Microsoft/vstest/pull/1708)
* Use of culture specified by user in case it differs with that of OS [#1712](https://github.com/Microsoft/vstest/pull/1712)
* Added attributes for sequence file generated by blame [#1716](https://github.com/Microsoft/vstest/pull/1716)
* Trx Logger class name fix for Nunit Data Driven tests [#1677](https://github.com/Microsoft/vstest/pull/1677)
* Trx Logger Fixed to generate trx file when test run aborts [#1710](https://github.com/Microsoft/vstest/pull/1710)
* Added trace level for diag argument [#1681](https://github.com/Microsoft/vstest/pull/1681)
 
### New Features introduced

* Enhancing Blame data collector options to include DumpType and AlwaysCollectDump [#1682](https://github.com/Microsoft/vstest/pull/1682)
* Procdump arguments enhanced to handle crash scenarios [#1700](https://github.com/Microsoft/vstest/pull/1700)
 
### Drops

* TestPlatform vsix: [15.9.0-preview-20180807-05](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180807-05;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.9.0-preview-20180807-05](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.9.0-preview-20180807-05)

## 15.8.0

### Issues Fixed

* Fix vstest.console.exe grabs exclusive read access to its test container [#1660](https://github.com/Microsoft/vstest/pull/1660)
* Registring correct property attributes during deserialization [#1644](https://github.com/Microsoft/vstest/pull/1644)
* Fixed test platform messages on cancellation request [#1667](https://github.com/Microsoft/vstest/pull/1667)
* Fixed warning messages for scenario when no tests are found matching TestCaseFilter [#1656](https://github.com/Microsoft/vstest/pull/1656)
* Fixed UWP VC++ unit tests not executing [#1649](https://github.com/Microsoft/vstest/pull/1649)
* Handling null value deserialization in TestCategory [#1640](https://github.com/Microsoft/vstest/pull/1640)

### New Features introduced

* Auto-generate F# program file. [#1664](https://github.com/Microsoft/vstest/pull/1664)
* Added support for dotnet test --collect:"Code Coverage" (windows only) [#981](https://github.com/Microsoft/vstest/issues/981)

### Drops

* TestPlatform vsix: [15.8.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180710-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.8.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.8.0)

## 15.8.0-preview-20180610-02

### New Features introduced

* Collect Code coverage with dotnet core sdk on windows. [#981](https://github.com/Microsoft/vstest/issues/981)

### Drops

* TestPlatform vsix: [15.8.0-preview-20180610-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180610-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.8.0-preview-20180610-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.8.0-preview-20180610-02)

## 15.8.0-preview-20180605-02

### Issues Fixed

* Fix Exception thrown while creating framework based on default enums. [#1598](https://github.com/Microsoft/vstest/pull/1598)
* Deprecate Testplatform\Extensions path for Adapters [#1602](https://github.com/Microsoft/vstest/pull/1602)
* Update Test Source with Package for Inprogress Tests [#1605](https://github.com/Microsoft/vstest/pull/1605)
* Use DateTime.UtcNow instead of DateTime.Now for consistency across test reporting data [#1612](https://github.com/Microsoft/vstest/pull/1612)
* Fixed RecordResult to SendTestCaseEnd if not already for datacollectors to end and get attachments correctly [#1615](https://github.com/Microsoft/vstest/pull/1615)
* Fix attachment uri in trx if same attachment filename is same [#1625](https://github.com/Microsoft/vstest/pull/1625)
* Add support to escape/unescape testcase filter strings [#1627](https://github.com/Microsoft/vstest/pull/1627)

### New Features introduced

* Add a tool to migrate testsettings to runsettings [#1600](https://github.com/Microsoft/vstest/pull/1600)

### Drops

* TestPlatform vsix: [15.8.0-preview-20180605-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180605-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.8.0-preview-20180605-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.8.0-preview-20180605-02)

## 15.8.0-preview-20180510-03

### Issues Fixed

* Extend FastFilter to support multiple properties. [#1523](https://github.com/Microsoft/vstest/pull/1523)
* Make all communication timeouts configurable. [#1538](https://github.com/Microsoft/vstest/pull/1538)
* Honoring cancel and abort request in test platform. [#1543](https://github.com/Microsoft/vstest/pull/1543)
* FilterOptions serialization issue when running .NET core tests. [#1551](https://github.com/Microsoft/vstest/pull/1551)
* Telemetry points for legacy settings. [#1564](https://github.com/Microsoft/vstest/pull/1564)
* Flushing test results even if RecordEnd is not called. [#1573](https://github.com/Microsoft/vstest/pull/1573)
* Searching adapters in Test Source directory in all scenarios. [#1574](https://github.com/Microsoft/vstest/pull/1574)
* Filtering non existent adapter paths. [#1578](https://github.com/Microsoft/vstest/pull/1578)

### New Features introduced

* Introduced category attribtue for adapter to specify supported assembly type.[#1528](https://github.com/Microsoft/vstest/pull/1528), [#1529](https://github.com/Microsoft/vstest/pull/1529), [#1537](https://github.com/Microsoft/vstest/pull/1537)

### Drops

* TestPlatform vsix: [15.8.0-preview-20180510-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180510-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.8.0-preview-20180510-03](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.8.0-preview-20180510-03)

## 15.7.2

### Issues Fixed

* Code coverage fix for async functions. [242314](https://developercommunity.visualstudio.com/content/problem/242314/code-coverage-doesnt-show-async-methods.html)

### Drops

* TestPlatform vsix: [15.7.2]( https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.7/20180514-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.7.2](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.7.2)

## 15.7.0

### Issues Fixed

* Code coverage fix for runsettings. [#1510](https://github.com/Microsoft/vstest/pull/1510)
* Logging fix for UWP.[#1508](https://github.com/Microsoft/vstest/pull/1508)
* Perf improvements for LUT [#1517](https://github.com/Microsoft/vstest/pull/1517)
* Fix for preserving CR LF line endings in TRX file. [#1521](https://github.com/Microsoft/vstest/pull/1521)
* Fix socket exception on datacollection in parallel. [#1514](https://github.com/Microsoft/vstest/pull/1514)

### New Features introduced

* Introduced running UWP test using ".appx" file as input, for CLI.

### Drops

* TestPlatform vsix: [15.7.0]( https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.7/20180403-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.7.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.7.0)

## 15.7.0-preview-20180320-02

### Issues Fixed

* Fixing the tests for string comparison issue. [#1462](https://github.com/Microsoft/vstest/pull/1462)
* Sync for binarywriter writes.[#1470](https://github.com/Microsoft/vstest/pull/1470)
* Usability Fixes [#1478](https://github.com/Microsoft/vstest/pull/1478)
* Fix for Design mode clients hang for errors [1451](https://github.com/Microsoft/vstest/pull/1451)
* Fix datacollectors temporary files cleanup [1483](https://github.com/Microsoft/vstest/pull/1483)

### Drops

* TestPlatform vsix: [15.7.0-preview-20180320-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180320-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.7.0-preview-20180320-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.7.0-preview-20180320-02)

## 15.7.0-preview-20180307-01

### Issues Fixed

* Fix CUIT tests fail to run on no VS installed machine.  [#1450](https://github.com/Microsoft/vstest/pull/1450)

### Drops

* TestPlatform vsix: [15.7.0-preview-20180307-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180307-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.7.0-preview-20180307-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.7.0-preview-20180307-01)

## 15.7.0-preview-20180221-13

### Issues Fixed

* Adding Category to Test Category mapping for ListFullyQualifiedTests. [#1369](https://github.com/Microsoft/vstest/pull/1369)
* Support escaping "," in Test filter. [#1374](https://github.com/Microsoft/vstest/pull/1374)
* Generate proper default settings for EnableCodeCoverage. [#1390](https://github.com/Microsoft/vstest/pull/1390)
* Test run directory fix for loggers. [#1399](https://github.com/Microsoft/vstest/pull/1399)
* Fixed the normal verbosity level to not log the full information for non-failed tests. [#1396](https://github.com/Microsoft/vstest/pull/1396)
* Ignore case for targetframework input. [#1420](https://github.com/Microsoft/vstest/pull/1420)
* Fixed logger to have additonal lines after std output. [#1421](https://github.com/Microsoft/vstest/pull/1421)
* Fixed the error message. [#1422](https://github.com/Microsoft/vstest/pull/1422)
* Fix: Logger attachments not coming in vsts test run. [#1431](https://github.com/Microsoft/vstest/pull/1431)
* Fixed help test to mention default value of verbosity in console logger. [#1433](https://github.com/Microsoft/vstest/pull/1433)
* Exceptions flow to Translation layer [#1434](https://github.com/Microsoft/vstest/pull/1434)

### New Features introduced

* Logger support in run settings.[#1382](https://github.com/Microsoft/vstest/pull/1382)
* Added CUIT package in vstest xcopy package. [#1394](https://github.com/Microsoft/vstest/pull/1394)
* Making Trx Logger Hierarchical for ordered test and data driven tests. [#1330](https://github.com/Microsoft/vstest/pull/1330)

### Drops

* TestPlatform vsix: [15.7.0-preview-20180221-13](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180221-13;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.7.0-preview-20180221-13](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.7.0-preview-20180221-13)

## 15.6.2

### Issues Fixed

* Fix socket exception on datacollection in parallel  [#1505](https://github.com/Microsoft/vstest/pull/1505)
* Fix datacollectors temporary files cleanup [#1506](https://github.com/Microsoft/vstest/pull/1506)

### Drops

* TestPlatform vsix: [15.6.2](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.6/20180326-08;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [v15.6.2](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.6.2)

## 15.6.1

### Issues Fixed

* Synchronize concurrent writes to communication channel  [#1457](https://github.com/Microsoft/vstest/pull/1457)

### Drops

* TestPlatform vsix: [15.6.1](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.6/20180307-08;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [v15.6.1](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.6.1)

## 15.6.0

### Issues Fixed

* Fix for Communication b/w testhost, & datacollector fails causing tests processes to hang. [#1406](https://github.com/Microsoft/vstest/pull/1406)
* Fix for Cancellation hanging TestExplorer with the unclickable cancelling. [#1398](https://github.com/Microsoft/vstest/pull/1398)

### Drops

* TestPlatform vsix: [15.6.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.6/20180215-04;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [v15.6.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.6.0)

## 15.6.0-preview-20180207-05

### Issues Fixed

* Enabling diagnostics for UWP causes app to hang. [#1387](https://github.com/Microsoft/vstest/pull/1387)

### Drops

* Microsoft.TestPlatform.ObjectModel: [v15.6.0-preview-20180207-05](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.6.0-preview-20180207-05)

## 15.6.0-preview-20180109-01

### Issues Fixed

* Make latest ObjectModel API compatible with ObjectModel 11.0.0. [#1251](https://github.com/Microsoft/vstest/pull/1251/)
* Fix no error message in case of invalid runsettings. [#1344](https://github.com/Microsoft/vstest/pull/1344)
* Fix CodedUI debug broken. [#1352](https://github.com/Microsoft/vstest/pull/1352)
* Fix debug stop causing 10s or indefinite wait in test explorer.  [#1358](https://github.com/Microsoft/vstest/pull/1358)
* Fix video datacollector assemblies first changes exception while running tests .  [#1362](https://github.com/Microsoft/vstest/pull/1362)
* Fix datacollector initialization failure on slow machines. [#1355](https://github.com/Microsoft/vstest/pull/1355)
* Fix running fakes and code coverage with embedded testsettings in runsettings. [#1364](https://github.com/Microsoft/vstest/pull/1364)

### New Features introduced

* Support reflection based discovery for UWP C++ Unit tests projects.[#1336](https://github.com/Microsoft/vstest/pull/1336)
* Add testhost external dependencies for UWP to Microsoft.NET.Test.Sdk. [#1351](https://github.com/Microsoft/vstest/pull/1351)

### Drops

* TestPlatform vsix: [15.6.0-preview-20180109-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20180109-01;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.6.0-preview-20180109-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.6.0-preview-20180109-01)

## 15.6.0-preview-20171211-02

### Issues Fixed

* Removed warning for AnyCPU assemblies
* Fix updating runsettings in dotnet core.
* Fix exception in Event Log DataCollector. [#1288](https://github.com/Microsoft/vstest/pull/1288)
* Fix support for multiple paths is TestAdapterPath Argument. [#1320](https://github.com/Microsoft/vstest/pull/1320)
* Perf: Using Event based communication over sockets using LengthPrefix communication channel. [1294](https://github.com/Microsoft/vstest/pull/1294)

### Drops

* TestPlatform vsix: [15.6.0-preview-20171211-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20171211-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.6.0-preview-20171211-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.6.0-preview-20171211-02)

## 15.5.0

### Issues Fixed

* Removed compile time dependency on castle.core.dll. [#1246](https://github.com/Microsoft/vstest/pull/1246)
* Fix test run for x64 c++ tests. [#1269](https://github.com/Microsoft/vstest/pull/1269)
* Localization fixes for error scenarios. [#1266](https://github.com/Microsoft/vstest/pull/1266)
* Fix for FastFilter issue with TestCaseFilter. [#1252](https://github.com/Microsoft/vstest/pull/1252)
* Updating codecoverage analysis dll's in external package. [#1282](https://github.com/Microsoft/vstest/pull/1282)

### Drops

* TestPlatform vsix: [15.5.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.5/20171108-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.5.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.5.0)

## 15.5.0-preview-20171031-01

### Issues Fixed

* Add LocalExtensionData property to TestCase Class.
* Do not crash data collector if extension fails to initialize or set environment variables.
* Use TPv2 as default for .NET 3.5 test projects.
* Loading native dll's correctly for UWP release mode.
* Insertion PR: <https://github.com/Microsoft/vstest/pull/1250>

### Drops

* TestPlatform vsix: [15.5.0-preview-20171031-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.5/20171031-01;/TestPlatform.vsix)

* Microsoft.TestPlatform.ObjectModel: [15.5.0-preview-20171031-01](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.5.0-preview-20171031-01)

## 15.5.0-preview-20171012-09

### Issues Fixed

* Fixed Data Collector Attachment issues for legacy TMI test execution workflow.
* Added error message and help when vstest.console is invoked without arguments.
* Fixed failure in loading extensions without Identifier Data.
* Handled Test Host close.
* TestCase Display Name is displayed instead of FullyQualifiedName.
* Fixed issues with Static Cover Coverage, Ordered tests through TMI.

### New Features introduced

* Added Telemetry Infra for Design Mode.
* Supported running .Net Framework v35 in compat mode.
* Localization changes.
* Automatically find Platform and Framework if not specified explicitly.
* Adding object model changes and Telemetry optin status.

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.5.0-preview-20170923-02...v15.5.0-preview-20171012-09).

### Drops

* TestPlatform vsix: [15.5.0-preview-20171012-09](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20171012-09;/TestPlatform.vsix)

* Microsoft.TestPlatform.ObjectModel: [15.5.0-preview-20171012-09](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.5.0-preview-20171012-09)

## 15.5.0-preview-20170923-02

### Issues Fixed

* Feature flag for executing net35 tests through TPv2 in compat mode.
* Removed unnecessary binding redirects from app.configs. [More info here](https://github.com/Microsoft/vstest/pull/1117)
* Put quotes around TestHost path so in case of spaces in name it starts correctly. [More info here](https://github.com/Microsoft/vstest/pull/1108)
* Performance Automation Infra.

### New Features introduced

* Added filter support on test case discovery.
* Added Telemetry Collection Infrastructure.
* Added support for listing fully qualified test cases.
* Exposed discovery events to loggers.

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/f8020e56e418f3a14637d401928fd154a061c9c4...v15.5.0-preview-20170923-02).

### Drops

* TestPlatform vsix: [15.5.0-preview-20170923-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20170923-02;/TestPlatform.vsix)

* Microsoft.TestPlatform.ObjectModel: [15.5.0-preview-20170923-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.5.0-preview-20170923-02)

## 15.5.0-preview-20170914-09

### Issues Fixed

* <https://github.com/Microsoft/vstest/issues/979>
* <https://github.com/Microsoft/vstest/pull/992>
* Made TestPlatform.ObjectModel CLS-compliant
* Made Microsoft.CodeCoverage as nuget package dependency for Microsoft.NET.Test.Sdk nuget package. [More info here](https://github.com/Microsoft/vstest/issues/852)
* Perf improvements. [More info here](https://github.com/Microsoft/vstest/pull/1041)
* Fixed issue related to /EnableCodeCoverage. [More info here](https://github.com/Microsoft/vstest/pull/1072)
* <https://github.com/Microsoft/vstest/issues/902>
* Highest version filtering for extensions. [More info here](https://github.com/Microsoft/vstest/pull/1051)
* <https://github.com/Microsoft/vstest/pull/1060>

### New Features introduced

* InProc execution of tests inside vstest.console process. [More info here](https://github.com/Microsoft/vstest/pull/1009)
* Added Verbosity Level as prefix for loggers. [More info here](https://github.com/Microsoft/vstest/pull/967)
* Event Log Data Collector. [More info here](https://github.com/Microsoft/vstest/blob/master/docs/analyze.md#event-log-data-collector)
* Introduced /UseVsixExtensions argument in CLI.

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.5.0-preview-20170810-02...f8020e56e418f3a14637d401928fd154a061c9c4).

### Drops

* TestPlatform vsix: [15.5.0-preview-20170914-09](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20170914-09;/TestPlatform.vsix)

* Microsoft.TestPlatform.ObjectModel: [15.5.0-preview-20170914-09](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.5.0-preview-20170914-09)

## 15.5.0-preview-20170810-02

### Issues Fixed

* <https://github.com/Microsoft/vstest/issues/861>
* <https://github.com/Microsoft/vstest/issues/916>
* Made latest testhost compat with older vstest.console and vice versa.
* Some performance improvement

### New Features introduced

* Added blame data collector support in `dotnet test`.
* Add ExecutionThreadApartmentState property in runsettings. [More info here](https://github.com/Microsoft/vstest/blob/master/docs/configure.md#execution-thread-apartment-state)
* Added async APIs support in translationLayer.

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/73f4a07adfa802257e3ebe11c197016010f2e080...v15.5.0-preview-20170727-01).

### Drops

* TestPlatform vsix: [15.5.0-preview-20170810-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20170810-02;/TestPlatform.vsix)

* Microsoft.TestPlatform.ObjectModel: [15.5.0-preview-20170810-02](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.5.0-preview-20170810-02)

## 15.5.0-preview-20170727-01

### Issues Fixed

* Support for devices: build up the TestHostRuntime APIs
* Console test runs will not collect File/LineNumber information
* Several performance improvements
* Reliability improvements to parallel runs
* Engineering fixes to build/test

### New Features introduced

* Blame for vstest. Reports the test which crashes a run
* Response file support for vstest
* `TestSessionTimeout` cancels a test run if it exceeds a timeout
* Mono support for vstest
* VSTest now runs on .NET 4.5.1 runtime

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/73f4a07adfa802257e3ebe11c197016010f2e080...v15.5.0-preview-20170727-01).

### Drops

* TestPlatform vsix: [15.5.0-preview-20170727-01](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20170727-01;/TestPlatform.vsix)

* Microsoft.TestPlatform.ObjectModel: [15.5.0-preview-20170727-01](http://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.5.0-preview-20170727-01)

## 15.3.0

### Issues Fixed

* Clean testhost before sending discoveryComplete/ExecutionCompltete.
* Closing VS should also close vstest.console process.

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.3.0-preview-20170618-03...rel/15.3-rtm).

### Drops

* TestPlatform vsix: [15.3.0](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.3-rtm/20170807-05;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.3.0](https://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.3.0)

## 15.3.0-preview-20170618-03

### Issues Fixed

* <https://github.com/Microsoft/vstest/issues/632>
* <https://github.com/Microsoft/vstest/issues/844>
* <https://github.com/Microsoft/vstest/issues/847>
* <https://github.com/Microsoft/vstest/issues/840>
* <https://github.com/Microsoft/vstest/issues/843>

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.3.0-preview-20170601-03...v15.3.0-preview-20170618-03).

### Drops

* TestPlatform vsix: [15.3.0-preview-20170618-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/15.3-rtm/20170618-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.3.0-preview-20170618-03](http://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.3.0-preview-20170618-03)

## 15.3.0-preview-20170601-03

* Monitor data Collector Launch and Exit events, log error in case data collector crashes.
* Fixed for issue where using environment variables in test results directory path in run settings throws error.
* Added support to handle `CollectSourceInformation` flag in runsettings
* Fixed scenario where testhost crash info is not coming to Testwindow
* In case of parallel if test host is aborted, add a new one in place of that

### Issues Fixed

* <https://github.com/Microsoft/vstest/issues/823>

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.3.0-preview-20170517-02...v15.3.0-preview-20170601-03).

### Drops

* TestPlatform vsix: [15.3.0-preview-20170601-03](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20170601-03;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.3.0-preview-20170601-03](http://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.3.0-preview-20170601-03)

## 15.3.0-preview-20170517-02

* Fakes support.
* Wait for testhost stderr to be available if connection is broken between vstest.console and testhost.
* Data collector log message improvements.
* Extracedt socket implementation to allow experimentation with multiple data interchange formats and ipc. Added concept of framing for messages passed between various processes. TestRequestSender2 uses these concepts and is a replacement for the earlier TestRequestSender.
* Localized new added string.
* Code cleanup

### Issues Fixed

* <https://github.com/Microsoft/vstest/issues/646>
* <https://github.com/Microsoft/vstest/issues/706>
* <https://github.com/Microsoft/vstest/issues/618>
* <https://github.com/Microsoft/vstest/issues/801>
* <https://github.com/Microsoft/vstest/issues/799>

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.3.0-preview-20170427-09...v15.3.0-preview-20170517-02).

### Drops

* TestPlatform vsix: [15.3.0-preview-20170517-02](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20170517-02;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.3.0-preview-20170517-02](http://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.3.0-preview-20170517-02)

## 15.3.0-preview-20170425-07

* Data collector support enabled
* Test Host Extensibility enabled
* ResultsDirectory argument and Runsettings priority order [#322](https://github.com/Microsoft/vstest/pull/322)
* Supporting Multiple TestProperty with the same key value [#328](https://github.com/Microsoft/vstest/pull/328)
* Allow VSTestConsole path to be specified [#325](https://github.com/Microsoft/vstest/pull/325)
* Adding /InIsolation flag for backward compatibility [#414](https://github.com/Microsoft/vstest/pull/414)
* Fixed reading test adapter paths from runsettings [#455](https://github.com/Microsoft/vstest/pull/455)
* Honor cache timeout for discovery. [#470](https://github.com/Microsoft/vstest/pull/470)
* Read asynchronously from test host process [#529](https://github.com/Microsoft/vstest/pull/529)
* Fixing nunit inconclusive tests reported as failure [#533](https://github.com/Microsoft/vstest/pull/533)
* BatchSize Runsettings [#550](https://github.com/Microsoft/vstest/pull/550)
* Make default testcase filter property name FullyQualifiedName [#555](https://github.com/Microsoft/vstest/pull/555)
* Logger extensibility [#557](https://github.com/Microsoft/vstest/pull/557)
* Update Microsoft.VisualStudio.TestTools.TestPlatform.V2.CLI.vsmanproj [#581](https://github.com/Microsoft/vstest/pull/581)
* Add Microsoft.NET.Test.Sdk.props to buildMultiTargeting [#580](https://github.com/Microsoft/vstest/pull/580)
* Moving to Netcoreapp 2.0 [#603](https://github.com/Microsoft/vstest/pull/603)
* Create config file for test project targeting .NET Framework [#642](https://github.com/Microsoft/vstest/pull/642)
* Create new RuntimeProvider to be associated with each ProxyOperationManager [#653](https://github.com/Microsoft/vstest/pull/653)
* Inject entry point only for project targeting netcoreapp [#665](https://github.com/Microsoft/vstest/pull/665)
* Dotnet test output coloring [#641](https://github.com/Microsoft/vstest/pull/641)
* Remove binding redirect of Newtonsoft.Json from testhost config file [#663](https://github.com/Microsoft/vstest/pull/663)
* Resolve testhost from source directory if we couldnt resolve it via nuget cache [#690](https://github.com/Microsoft/vstest/pull/690)
* Improve testplatform message [#691](https://github.com/Microsoft/vstest/pull/691)
* Protocol v2 improvements [#672](https://github.com/Microsoft/vstest/pull/672), [#698](https://github.com/Microsoft/vstest/pull/698)
* Use "dotnet test --verbosity" arg for console verbosity [#735](https://github.com/Microsoft/vstest/pull/735)

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.0.0...v15.3.0-preview-20170425-07).

### Drops

* TestPlatform vsix: [15.3.0-preview-20170425-07](https://vsdrop.corp.microsoft.com/file/v1/Products/DevDiv/Microsoft/vstest/master/20170425-07;/TestPlatform.vsix)
* Microsoft.TestPlatform.ObjectModel: [15.3.0-preview-20170425-07](http://www.nuget.org/packages/Microsoft.TestPlatform.ObjectModel/15.3.0-preview-20170425-07)

## 15.0.0-preview-20170125-04

* Localization for following nuget packages:

 1) Microsoft.TestPlatform.CLI
 2) Microsoft.TestPlatform
 3) Microsoft.TestPlatform.ObjectModel
 4) Microsoft.TestPlatform.TestHost

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.0.0-preview-20170123-02...15.0.0-preview-20170125-04).

### Drops

* Microsoft.TestPlatform.ObjectModel: [15.0.0-preview-20170125-04](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.ObjectModel/15.0.0-preview-20170125-04)
* Microsoft.TestPlatform.TranslationLayer: [15.0.0-preview-20170125-04](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.TranslationLayer/15.0.0-preview-20170125-04)

## 15.0.0-preview-20170123.02

* Allow multiple test properties with same key [#239](https://github.com/Microsoft/vstest/issues/239), [#358](https://github.com/Microsoft/vstest/issues/358)
* Localization for testplatform vsix package [#146](https://github.com/Microsoft/vstest/issues/146)
* Working directory should be set to test source parent directory [#311](https://github.com/Microsoft/vstest/issues/311)
* Allow relative source paths to vstest.console [#331](https://github.com/Microsoft/vstest/issues/331)
* Stacktrace and error message should be in context of failed test [#285](https://github.com/Microsoft/vstest/issues/285)

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/RC.3...15.0-rtm).

### Drops

* TestPlatform vsix: [TestPlatform.CI.Real-20170123-02](https://devdiv.visualstudio.com/DevDiv/_build/index?buildId=533598&_a=summary)
* Microsoft.TestPlatform.ObjectModel: [15.0.0-preview-20170123-02](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.ObjectModel/15.0.0-preview-20170123-02)
* Microsoft.TestPlatform.TranslationLayer: [15.0.0-preview-20170123-02](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.TranslationLayer/15.0.0-preview-20170123-02)

## 15.0.0-preview-20170106.08

* First Draft for the Protocol tool. [#306](https://github.com/Microsoft/vstest/pull/306)
* Fixed DiaSession issue which showed async methods to be `external` [#307](https://github.com/Microsoft/vstest/pull/307)
* Localized vstest [#308](https://github.com/Microsoft/vstest/pull/308)
* Added OutputType to Microsoft.NET.Test.Sdk.target [#310](https://github.com/Microsoft/vstest/pull/310)
* Enclosed run settings arguments to handle whitespace [#312](https://github.com/Microsoft/vstest/pull/312)
* Converted TestPlatform.vsix from V2 to V3 format [#316](https://github.com/Microsoft/vstest/pull/316)

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.0.0-preview-20161227-02...v15.0.0-preview-20170106-08).

### Drops

* TestPlatform vsix: [TestPlatform.CI.Real-20170106-08](https://devdiv.visualstudio.com/DevDiv/VS.in%20Agile%20Testing%20IDE/_build/index?buildId=505945&_a=summary&tab=artifacts)
* Microsoft.TestPlatform.ObjectModel: [15.0.0-preview-20170106-08](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.ObjectModel/15.0.0-preview-20170106-08)
* Microsoft.TestPlatform.TranslationLayer: [15.0.0-preview-20170106-08](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.TranslationLayer/15.0.0-preview-20170106-08)

## 15.0.0-preview-20161227.02

* Add enhancement: trx logger can take logfile parameter [#282](https://github.com/Microsoft/vstest/pull/282).
* Allow TranslationLayer to specify Diag parameters [#296](https://github.com/Microsoft/vstest/pull/296).
* Passing runsettings as command line arguments [#297](https://github.com/Microsoft/vstest/pull/297).
* Localization work [#302](https://github.com/Microsoft/vstest/pull/302).
* Testhost Diag log file name format change [#303](https://github.com/Microsoft/vstest/pull/303).
* Fix for issue where xlftool.exe is not able to update neutral xlf file if we update any existing resource string [#305](https://github.com/Microsoft/vstest/pull/305).

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.0.0-preview-20161216-01...v15.0.0-preview-20161227-02).

### Drops

* TestPlatform vsix: [TestPlatform.CI.Real-20161227-02](https://devdiv.visualstudio.com/DevDiv/VS.in%20Agile%20Testing%20IDE/_build/index?buildId=490545&_a=summary&tab=artifacts)
* Microsoft.TestPlatform.ObjectModel: [15.0.0-preview-20161227-02](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.ObjectModel/15.0.0-preview-20161227-02)
* Microsoft.TestPlatform.TranslationLayer: [15.0.0-preview-20161227-02](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.TranslationLayer/15.0.0-preview-20161227-02)

## 15.0.0-preview-20161216.01

* Migrate to csproj from xproj [#217](https://github.com/Microsoft/vstest/pull/217).
* Translationlayer timeout for CustomHost Launch changed to indefinate [#265](https://github.com/Microsoft/vstest/pull/265)
* Added net46 folder in lib of Microsoft.TestPlatform Nuget Package [#247](https://github.com/Microsoft/vstest/pull/247).
* Added license link.
* Added third party notice to nuget packages [#249](https://github.com/Microsoft/vstest/pull/249).
* Change assembly signing to public [#256](https://github.com/Microsoft/vstest/pull/256).
* Make testhost a project dependency instead of content [#264](https://github.com/Microsoft/vstest/pull/264).
* Several changes to build infrastructure for csproj migration [#262](https://github.com/Microsoft/vstest/pull/262) [#268](https://github.com/Microsoft/vstest/pull/268/files).
* Include microbuild.core as a dependency for signing [#267](https://github.com/Microsoft/vstest/pull/267).
* Make External packages are restored with a separate csproj [#273](https://github.com/Microsoft/vstest/pull/273).
* Add Acceptance tests for netcore1.0 target [#259](https://github.com/Microsoft/vstest/pull/259).
* Add Acceptance tests for netcore1.1 target [#270](https://github.com/Microsoft/vstest/pull/270).
* Added E2E test for RunTestsWithCustomTestHostLauncher.
* Change testcase gereration id algorithm to SHA1 to be in compat with Associate-WorkItem scenarios [#279](https://github.com/Microsoft/vstest/pull/279).
* Bug fix: Default logger output path should be cmd-line friendly [#244](https://github.com/Microsoft/vstest/issues/244).
* Bug fix: TRX logger Started Time incorrect [#253](https://github.com/Microsoft/vstest/pull/253).
* Update README.md [#263](https://github.com/Microsoft/vstest/pull/263).

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.0.0-preview-20161123-03...v15.0.0-preview-20161216-01).

### Drops

* TestPlatform vsix: [TestPlatform.CI.Real-20161216-01](https://devdiv.visualstudio.com/DevDiv/VS.in%20Agile%20Testing%20IDE/_build/index?buildId=474910&_a=summary&tab=summary)
* Microsoft.TestPlatform.ObjectModel: [15.0.0-preview-20161216-01](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.ObjectModel/15.0.0-preview-20161216-01)
* Microsoft.TestPlatform.TranslationLayer: [15.0.0-preview-20161216-01](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.TranslationLayer/15.0.0-preview-20161216-01)

## 15.0.0-preview-20161123.03

* Support for debugging .net core project.
* Support for parallel discovery and execution.
* Support to discover and run test from a solution having .net core and desktop project.
* Support of arguments(output, configuration, framework and noBuild) in dotnet test.
* Support to run test from project targeting multiple TargetFrameworkMoniker using dotnet test.
* Support for trx logger in non-windows machine.
* Added diag argument to enable logging in vstest.console.
* Acceptance test for test platform.
* Bug fix: display a message on console when dotnet is not installed on the machine.
* Bug fix: dotnet test should return a non-0 exit code when any test fails [here](https://github.com/Microsoft/vstest/issues/241).
* Bug fix: dotnet test fails due to missing quotes in the path of vstest.console [here](https://github.com/Microsoft/vstest/issues/231).
* Bug fix: terminate vstest.console if no testhost found [here](https://github.com/Microsoft/vstest/issues/144).
* Bug fix: testCaseFilter argument doesn't filter tests in .net core [here](https://github.com/Microsoft/vstest/issues/201).
* Bug fix: cannot add Microsoft.Net.Test.Sdk as a dependency of net451 projects [here](https://github.com/Microsoft/vstest/issues/190).

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.0.0-preview-20160923-03...v15.0.0-preview-20161123-03).

### Drops

* TestPlatform vsix: [TestPlatform.CI.Real-20161123-03](https://devdiv.visualstudio.com/DevDiv/VS.in%20Agile%20Testing%20IDE/_build/index?buildId=442970&_a=summary)
* Microsoft.TestPlatform.ObjectModel: [15.0.0-preview-20161123-03](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.ObjectModel/15.0.0-preview-20161123-03)
* Microsoft.TestPlatform.TranslationLayer: [15.0.0-preview-20161123-03](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.TranslationLayer/15.0.0-preview-20161123-03)

## 15.0.0-preview-20160923-03

* New configuration `DisableParallelization` in runsettings. This setting may be used by adapters to disable parallel run in certain scenarios, e.g. test profiling or instrumented runs.
* Support for non-shared test hosts. A non shared test host is exclusive per test source. E.g. .net core tests use a non-shared host.
* New nuget package: Microsoft.TestPlaform.TestHost. All .net core test apps will refer to this package.
* Sample performance tests for test platform
* Bug fix: support for reg free COM in Dia
* Bug fix: display a message in VS on test host crash
* Bug fix: in .net core, user may provide relative path to run tests

A list of all changes since last release are available [here](https://github.com/Microsoft/vstest/compare/v15.0.0-preview-20160914-02...v15.0.0-preview-20160923-03).

### Drops

* TestPlatform vsix: [TestPlatform.CI.Real-20160923-03](https://devdiv.visualstudio.com/DevDiv/VS.in%20Agile%20Testing%20IDE/_build/index?buildId=343725&_a=summary)
* Microsoft.TestPlatform.ObjectModel: [15.0.0-preview-20160923-03](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.ObjectModel/15.0.0-preview-20160923-03)
* Microsoft.TestPlatform.TranslationLayer: [15.0.0-preview-20160923-03](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.TranslationLayer/15.0.0-preview-20160923-03)

## 15.0.0-preview-20160914-02

* Support for .net core framework
* New nuget package `Microsoft.Testplatform.CLI` for dotnet-cli
* Performance instrumentation of runner, discovery and execution
* Bug fix: Handle crash of test host
* Bug fix: Handle paths and arguments on Unix
* Bug fix: Sign core binaries

### Drops

* TestPlatform vsix: [TestPlatform.CI.Real-20160914-02](https://devdiv.visualstudio.com/DevDiv/VS.in%20Agile%20Testing%20IDE/_build/index?buildId=329464&_a=summary)
* Microsoft.TestPlatform.ObjectModel: [15.0.0-preview-20160914-02](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.ObjectModel/15.0.0-preview-20160914-02)
* Microsoft.TestPlatform.TranslationLayer: [15.0.0-preview-20160914-02](https://dotnet.myget.org/feed/vstest/package/nuget/Microsoft.TestPlatform.TranslationLayer/15.0.0-preview-20160914-02)
