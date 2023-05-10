# Mirroring packages from nuget.org to dotnet-public

The dotnet-public feed serves as .NET's OSS mirror of nuget.org, for the packages that our repos depend on. This is an alternate to the use of the 'single-feed with upstreams' approach that many projects use, due to the incompatibility of that approach with many .NET and OSS practices.

Microsoft-owned packages that already exist on dotnet-public are automatically updated every hour with any new versions. New package IDs, or new versions of packages **not** owned by Microsoft on nuget.org must be mirrored using the following process:

1. Navigate to https://dev.azure.com/dnceng/internal/_build?definitionId=931&_a=summary
2. Queue a new build with the following parameters:
    - Under the Mirror type, select "New or non-Microsoft"
    - Enter the package name (ID) of the package you wish to mirror
    - Enter the package version of the package you wish to mirror. Leave as 'latest' to mirror the latest available version.
3. Click Run.

The pipeline will run and mirror the new package, with any transitive dependencies. If the package is owned by Microsoft, new versions (or versions not originally mirrored), will be automatically updated the next time the pipeline runs on its schedule.

Happy Migrating! Share and Enjoy!

