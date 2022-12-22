## V3 Publishing 
We need to retire V1 and V2 publishing.

Why do we need to retire V1 and V2? 
Both V1 and V2 use multi stage(s) publishing infrastructure. V3 on the other hand uses single stage publishing, thereby reducing UI clutter. V3 reduces the number of machines used during publishing, which speeds up the whole process. In both V1 and V2, when new channels are added it requires an arcade update to the customer repository, but in V3 it will only require arcade getting an arcade update.

Currently arcade release/5.0, main and all the repos getting updates from these branches are already using V3 publishing. In this epic we are planning to move arcade release/3.0 branch to use V3 publishing. We need all the repos currently which takes updates from arcade release/3.0 to use the latest V3 publishing. Also removing all the legacy publishing code that includes V1 and V2 publishing from arcade main and release/3.0 branches.

Also will be working on ways to improve the performance of publishing artifacts and symbols, and add more tests during this process. This will include better way of downloading artifacts to improve publishing performance.

## Stakeholders
- .NET Core Engineering
- .NET Core Engingeering Partners

## Risk
What are the unknowns?
- How arcade-services would react to this publishing arcade update, because right now we have special stages in arcade-services compared to other repos which consumes update from arcade/release-3.0.
- While on-boarding repos to V3, there might be some risk because of some unknown dependency of the repo on V1/V2 publishing.

## Rollout and Deployment
V1/V2 to V3
a) We are deprecating legacy publshing code. This functionality will be first tested in arcade main and then in arcade-validation. Upon successful test, since all the repos getting update from arcade main are currently using V3 publishing. This rollout is going to be seamless. This is just going to be an arcade update and repo owners do not have to do anything here.
b) Then V3 publishing infrastructure has to be added in arcade/release-3.0 and this will be tested against some repos that takes update from arcade/release-3.0. Upon successful testing, an arcade update will be rolled out which customer repos have to consume.
c) Make a list of all the repos that will require to update like we did for arcade/release-5.0 eg:(https://github.com/dotnet/arcade/blob/main/Documentation/V3StatusUpdate.md)
d) Will send out an email to partners to upgrade from V1/V2 to V3 and help them upgrade to V3. Documentation on how to upgrade can be found here (https://github.com/dotnet/arcade/blob/main/Documentation/CorePackages/Publishing.md#how-to-upgrade-from-v2-to-v3)
e) After all the repos are onboarded successfully, V1 and V2 publishing infrastructure will be deprecated from arcade/release-3.0. This is going to be an arcade rollout which customers repos have to consume.

Performance improvements 
a) All the performance related improvements are going to be an arcade update which customer repos have to consume. This will be tested against runtime, installer before roll out.

## Serviceability
Testing 
a) While improving the performance of publishing artifacts and symbols, tests will be added to cover downloading artifacts.
b) While deprecating legacy publishing, some V2 publishing tests will replaced by V3 publishing tests.
c) Some tests related to PublishArtifactsInManifest, SettingUpV3Config and Symbol publishing are already in place and can be found here (https://github.com/dotnet/arcade/tree/main/src/Microsoft.DotNet.Build.Tasks.Feed.Tests)

PATs
a) No new PATs are added as part of this epic.

SDL 
No change to the SDL threat model.

Confidence in deployments/shipping
a) Before on-boarding repos using arcade/release-3.0 on V3 publishing, a subset of repos will be tested with the latest update, only upon successful test the repos will be on-boarded. 
b) Adding more tests to the publishing infrastructure will increase the confidence. 

## Monitoring
Customers are responsible for keeping their build green once the changes are rolled out.

## FR Hand off
Publishing FAQs are already in place here (https://github.com/dotnet/arcade/blob/main/Documentation/CorePackages/Publishing.md#frequently-asked-questions), this document can be updated incase of new errors. 

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CV3%20Publishing%5Cone-pager.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CV3%20Publishing%5Cone-pager.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CV3%20Publishing%5Cone-pager.md)</sub>
<!-- End Generated Content-->
