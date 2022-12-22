Migration status:
1. Using Azure DevOps for CI
   - [Already in use](https://dev.azure.com/dnceng/public/_build?definitionId=106) except for `base.yml` work, which is a low priority.
2. Using shared toolset (Arcade SDK)
   - We first have to migrate off of `project.json` to .NET 2 SDK.  This is our current top priority.
3. Engineering dependency flow
   - In place in a hacked manner, once we're on .NET 2 SDK we'll immediately start the Arcade integration.
4. Internal builds from dnceng
   - Lowest priority, no current plans.

Expected delivery dates:
1. 23 November 2018.
2. 23 November 2018.
3. 30 November 2018.
4. mid December 2018.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CMigrationPlan%5CVisualFSharp.md)](https://helix.dot.net/f/p/5?p=Documentation%5CMigrationPlan%5CVisualFSharp.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CMigrationPlan%5CVisualFSharp.md)</sub>
<!-- End Generated Content-->
