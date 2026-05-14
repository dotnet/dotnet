# Microsoft.DotNet.Build.Tasks.Packaging

Task package which will handle building projects with the extension pkgproj like the ones we currently have on our [corefx](http://github.com/dotnet/corefx/) and [core-setup](http://github.com/dotnet/core-setup/) repos. 

This package also includes the tasks that will do validation to make sure the packages will not break support of any existing framework.

The Packaging.targets file is included to enable the build of nuget packages using the PkgProj content model and targets.  This is enabled by default for projects with the extension .pkgproj and can be disabled by setting ImportPackagingTargets=false.

The PackageLibs.targets is included to report content from any project type to a packaging project.  Content is reported via the GetFilesToPackage target which returns items whose identity is the source file TargetPath is destination inside the package, with metadata to indicate more information about that file for the purposes of dependency harvesting and validation.