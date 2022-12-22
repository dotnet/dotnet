# Overview of VS2019 Policy 
Provide a reasonable mechanism to scout new minor versions of VS 2019 (both preview and release) that will minimize disruption to existing production queues

## Definition used to determine release 
The versioning format used below is <Major.Minor.Patch>. As an example for version 16.5.1, 16 is the major version, 5 is the minor and 1 is the patch version. 

## Policy for supporting VS 2019 – Released version
DNCEng will create a temporary image (build.windows.vs2019.scouting.open) with 5 machines (20 cores) for each minor release of Visual Studio that can use to validate any changes made to the product. 

This queue will be activated approximately one week after the [release](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes) of a minor update and will be available for two weeks. After approximately 2 weeks, we will in-place update the existing images (any helix image containing *.vs2019) will be updated as part of a regularly scheduled Helix Machines release.

Patch updates will be made directly to the queues as required by corporate security or when requested by customers

## Policy for supporting VS 2019 – Public Preview Versions
DNCEng will create a temporary build image (e.g. build.windows.vs2019.pre.scouting.open) for each public preview of Visual Studio that can use to validate changes. This queue will be activated approximately one week after the release of [public preview](https://docs.microsoft.com/en-us/visualstudio/releases/2019/release-notes-preview) and will be available for two weeks. 

After approximagely two weeks, we will in-place update the existing images as part of a regularly scheduled Helix Machines release (typically Wednesdays)

## Policy for supporting VS 2019 – Private Preview Versions
There is currently no Engineering Services support for providing VS private preview queues.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CPolicy%5CVS2019SupportPolicy.md)](https://helix.dot.net/f/p/5?p=Documentation%5CPolicy%5CVS2019SupportPolicy.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CPolicy%5CVS2019SupportPolicy.md)</sub>
<!-- End Generated Content-->
