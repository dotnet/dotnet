### How to See What's the Latest Version of an Arcade Package

1. Follow steps 1-6 outlined [here](https://github.com/dotnet/arcade/blob/master/Documentation/DependencyFlowOnboarding.md#32-set-up-your-darc-client) to create a Maestro++ authorization token for you.
2. Go to [swagger API](https://maestro.dot.net/swagger/ui/index.html) and click on "Authorize" button on the top-right corner.
3. In the "Value" input box add "Bearer" + the token generated in the previous step. i.e "Bearer m1T0ken6tab5" and click "Authorize".
4. On the Swagger API go to [Builds -> Latest](https://maestro.dot.net/swagger/ui/index.html#/Builds/Builds_GetLatest) and click on the "***Try it out***" button on the right hand side.
5. Fill in the following fields:
   1. On the "repository" field inform the URL of the repository (e.g., https://github.com/dotnet/arcade)
   2. On the "loadCollections" field select "true"
   3. Click on the "Execute" button
6. The "Response Body" field will contain information about the latest assets published.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CSeePackagesLatestVersion.md)](https://helix.dot.net/f/p/5?p=Documentation%5CSeePackagesLatestVersion.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CSeePackagesLatestVersion.md)</sub>
<!-- End Generated Content-->
