# 0023 TestSettings deprecation for automated unit and functional testing scenarios.

## Summary

This note outlines the proposed changes to deprecate support for using .testsettings in automated unit and functional testing scenarios.

## Motivation

Today there are two types of files to configure test runs: *.testsettings and *.runsettings. To simplify the experience, we are planning to deprecate support for *.testsettings in automated unit and functional testing scenarios.  
Please note that *.testsettings will continue to be supported for load test scenarios.

## RoadMap

The recommendation for those who are using testsettings in automated unit and functional testing scenarios, is to move to runsettings. [Here](http://aka.ms/runsettings) is a documentation of how to use runsettings to configure test runs.

If you are not able to achieve all your existing configurations in testsettings via runsettings, you may try to achieve the same using the below legacy nodes that will be newly introduced in runsettings.  
RunSettings will start supporting these features previously supported only via TestSettings. The following table specifies the new nodes and attributes, that will be introduced and how they are mapped from the existing nodes in testsettings.

| TestSettings Node                                                   | RunSettings Node                                                           |
|---------------------------------------------------------------------|----------------------------------------------------------------------------|
|/TestSettings/Deployment                                             |/RunSettings/LegacySettings/Deployment                                      |
|/TestSettings/Scripts                                                |/RunSettings/LegacySettings/Scripts                                         |
|/TestSettings/Execution/TestTypeSpecific/UnitTestRunConfig           |/RunSettings/LegacySettings/Execution/TestTypeSpecific/UnitTestRunConfig    |
|/TestSettings/Execution/Timeouts: testTimeout                        |/RunSettings/LegacySettings/Execution/Timeouts: testTimeout                 |
|/TestSettings/Execution/Timeouts: runTimeout                         |/RunSettings/RunConfiguration/TestSessionTimeout                            |
|/TestSettings/Execution: parallelTestCount                           |/RunSettings/LegacySettings/Execution: parallelTestCount                    |
|/TestSettings/Execution: hostProcessPlatform                         |/RunSettings/LegacySettings/Execution: hostProcessPlatform                  |
|/TestSettings/Execution/Hosts                                        |/RunSettings/LegacySettings/Execution/Hosts                                 |
|/TestSettings/Execution/TestTypeSpecific/WebTestRunConfiguration     |/RunSettings/WebTestRunConfiguration                                        |

## Migration

A tool named SettingsMigrator will be introduced with test platform, which can be used to migrate your existing testsettings files to runsettings as follows:

SettingsMigrator.exe {Full path to testsettings file or runsettings file to be migrated}  
SettingsMigrator.exe {Full path to testsettings file or runsettings file to be migrated} {Full path to runsettings file to be created}  

Examples:  
SettingsMigrator.exe  E:\MyTest\MyTestSettings.testsettings  
SettingsMigrator.exe  E:\MyTest\MyOldRunSettings.runsettings  
SettingsMigrator.exe  E:\MyTest\MyTestSettings.testsettings E:\MyTest\MyNewRunSettings.runsettings  
SettingsMigrator.exe  E:\MyTest\MyOldRunSettings.runsettings E:\MyTest\MyNewRunSettings.runsettings  

The exe can usually be found in C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\Extensions\TestPlatform\SettingsMigrator.exe depending on your Visual Studio install location.

## Expected Ship Date

Support for the new legacy settings in runsettings is expected to come in the next release. The deprecation of testsettings for automated unit and functional testing scenarios is expected to be in effect from the next major VS release.
