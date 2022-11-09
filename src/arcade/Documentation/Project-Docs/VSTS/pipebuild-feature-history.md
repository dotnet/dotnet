# PipeBuild History

## Background

PipeBuild was created out of the need to produce a single unified build (set of binaries) which spanned multiple OS's and architectures.  In some instances, architecture was less of a motivating factor, and configuration certainly wasn't a factor; because those aspects could have been done on a single machine or independently in the case of configuration.  The ability to parallelize all of these aspects, however, proved tremendously valuable in terms of throughput and certainly leads to more desirable architectural patterns.

Here's the "evolution" of PipeBuild broken into loosely chronologically implemented features.

### Feature 1 - Creating an Orchestrator
Initially, PipeBuild was very simple.  There was a json file which defined build definitions, and one that combined build definitions into pipelines.  Being "aware" of a build definition meant associating a name with a VSTS Build Definition ID.  

Definitions.json

```
  "Definitions": [
    {
      "DefinitionId": 893,
      "Name": "CoreFx-Windows-Trusted",
      "ProjectName": "DevDiv",
      "BaseUrl": "https://devdiv.visualstudio.com/DefaultCollection"
    },
    {
      "DefinitionId": 1054,
      "Name": "CoreFx-Linux-Native-Trusted",
      "ProjectName": "DevDiv",
      "BaseUrl": "https://devdiv.visualstudio.com/DefaultCollection"
    }
  ]
```

Pipelines.json
```
  "Pipelines": [
  {
      "Name": "Trusted-All-Release",
      "Parameters": {
        "TreatWarningsAsErrors": "false"
      },
      "Definitions": [
        {
          "Name": "CoreFx-Linux-Native-Trusted",
          "Parameters": {
            "DockerTag": "debian82_prereqs_2",
            "ConfigurationGroup": "Release"
          }
        },
        {
          "Name": "CoreFx-Linux-Native-Trusted",
          "Parameters": {
            "DockerTag": "rhel7_prereqs_2",
            "ConfigurationGroup": "Release"
          }
        },
    ...
```

We could then define a named definition, and provide values for variables defined in that definition (via the defined pipeline).  When PipeBuild was launched, it would update replace any named variables in the definition with the values in the pipeline.json file.  Side note: the pipeline.json and definitions.json file all lived in source code next to PipeBuild itself (this very quickly proved to be an unwieldy model).  I'll outline some of the immediate pitfalls, but it was a very quickly thrown together solution to a major problem we had so problems were expected.  

Cons:

- pipeline definitions next to PipeBuild source code and not in product repos meant a multi-step process for making changes to the build and a synchronization nightmare.  It also meant that we started seeing a proliferation of checked in files named "pipelines.json", "pipelines.corefx.json", "pipelines.corefx.1.0.0.json", "pipelines.corefx.1.1.0.json", etc...  Additionally, any change to the pipelines ended up being handled by the same person (not a product repo dev but a removed engineering dev) because the process was so disjoint from the source code.
- We ended up duplicating a lot of data because you would have one pipeline for Release, and one for Debug, and you defined them in the same file but separately.  We later made a change to support definition groups so that you could re-use the same set of definitions for a pipeline but provide different variable values to it (Debug vs Release).  This cut down the duplication and the (somewhat common) instance where someone would make a change to the Release build but not scroll down through the massive file and make the exact same change to the Debug pipeline.
- Reliability wasn't great, but it's actually gotten worse since then as our requirements have grown
- Growing pains.  As PipeBuild became adopted across more and more teams, support and maintenance costs skyrocketed

Pros:

- Note the reference to "DockerTag" in the pipeline.  This reliance on Docker ended up being a huge win in terms of machine maintenance since we could define one VM image but run our builds / tests on many Linux distros.
- At the time DotNet Core was moving towards JSON, so the format was familiar and somewhat reasonable by devs.
- It did the basic job we wanted; ran pipeline jobs in parallel or sequentially, across platforms

### Feature 2 - Definition groups

As mentioned in the cons section of "Phase 1", our primitive json model meant for much duplication when defining a pipeline.  The more places a dev has to change code, the more chance of failure.  Making pipeline changes generally required a few cycles of: update, run an official build, investigate failures, rinse, and repeat.  That cycle could be about 1 - 2 hours depending on the repo.  The result was that making any change to the pipeline requied at least a day to get right.  

We introduced the concept of "Definition groups" which allowed us to define a set of build definitions with default variable values.  The definition group could be explicitly referenced with a different variable value which applied to the entire grouping.  This was our solution which reduced duplicated code and reduced dev errors considerably.  It should be noted that this (like many of the "solutions") was not necessarily the best solution, but it addressed a certain need, and was the kind of thingyou do when there is limited investment towards a tool.  The major take-away, is eliminating the places where code needs to be duplicated is a HUGE win.

### Feature 3 - Reporting Parameters

Reporting parameters had some positive characteristics and some negative ones.  The issue we encountered was that VSTS is keenly adapted towards builds and less (or not at all) tailored towards tests.  Some subset of our tests (public unit tests) were a part of our product builds, but to fully test our product across all of the required architectures, we had to create a different thing called Helix.  Helix allowed us to take our product build pieces along with our test binaries, and massively parallelize running tests across architectures / platforms /etc.  VSTS didn't have a great way to view both our builds and all of our tests in one place, so a new system (Mission Control) was developed that could collate all of the data into one viewable place.

The way data was exposed from VSTS to Mission Control was via Reporting Parameters.  Reporting Parameters were variables that our Orchestrator exposed from VSTS to Mission Control so that Mission Control could better identify builds (things like build number, platform, configuration, architecture, url, etc...).

Positives:
- The data allowed us to provide direct links from Mission Control to VSTS builds with differentiated labels.
- The data allowed us to group product builds together and also link test results (from Helix) with those builds.  Ok, to be fair, some of this was the Reporting Parameters, and some of it was other telemetry that we wired into our orchestrator (like providing a unique guid identifier unifying every product build, test build, and test run to a single orchestrated build instance).

Negatives:
- I hated Reporting Parameters.  It was always an as-needed thing.  As our supported build variations increased, we would have to go back and plumb through an update to support the new requirements. ie, all of a sudden we would need to differentiate what was displayed in Mission Control based on configuration or architecture or some other factor.  I hated this because it meant that the Product team would expose the requirement to the Mission Control team. The Mission Control team would make the update to their source to support the new parameter, then they would talk to the PipeBuild team (not really a team), and that person would have to go change the pipeline json to support the new parameter.  It was terribly inefficient and annoying, and the requirements varied across repos.

### Feature 4 - SkipBranchAndVersionOverrides

When developing a pipeline, we found that there were some build legs that weren't necessarily associated with a repo.  This was an intentional thing for our "Publish" build definition which we shared across repos.  We didn't want to arbitrarily force that build leg to clone / sync the repo which the build legs were using because every clone is another possible point of failure and we, also, were beginning to focus on performance (builds were slow).  Cutting out an additional 20 minute clone / sync step (at the time our repos were not ideally performant) was a huge win.  The simple fix was to add a switch ("SkipBranchAndVersionOverrides") that told the build definition to ignore any repo / branch information the orchstrator was providing.

Take-aways:
- Sharing build definitions is a thing and provides many benefits.  These are the same benefits which are widely discussed by any code-reuse proponents
- Performance is important
- Reliability is important
- Most of these take-aways are pretty obvious, but the less obvious one (sharing build definition), shouldn't be overlooked.

### Feature 5 - Cleanup

As we continued to grow in the number of repos that we built and docker based linux variants we supported, we started to see build failures on a regular (~ every 3 weeks builds would start to fail across the board) because machine disk space would fill up and VSTS didn't provide the level of cleanup that we required when dealing with hundreds of builds a day in a fixed machine pool shared across products.  

To address disk space issues we invested in infrastructure that we could run on every build to cleanup the VSTS agent working directories which contained builds older than a day, and also to cleanup docker images / containers which began accumulating on machines and taking up precious space.  The compromise of only deleting day old builds arose because we couldn't cleanup every build (though we wanted to) due to repos failing but holding locked processes that would cause cleanup to fail.  The workaround of ignoring those cases wasn't acceptable because it meant that every build we ran reported the same warning of a failed cleanup task and eternally "yellow" builds is not as pleasant as seeing "green" builds.

Cleanup is actually a major thing as every time a machines disk drive fills, it requires someone to investigate the issue then manually clean it up or enlist the help of DDFUN.  It's even worse because machines (obviously) fill up disk space faster under heavy usage which tends to happen when products are preparing to ship and you really don't want to see random infrastructure failures. 

### Feature 6 - Checked in definitions

The number of repos and branches (release branches, servicing branches, dev branches) that we started to support for official builds was continually growing.  It quickly became clear that supporting a branching code base is an increasingly difficult task.  For example: If we had to make a change to the Linux build definitions for a release, we had to go manually update about 15 build definitions. If there was a breaking change to our orchestrator, the number of build definitions that had to be updated was closer to 40 (at the time...).  VSTS was slow and this was an extremely time consuming and error prone process.  

The more difficult task with definitions, was that, when we would branch for a release, we had to clone all of our build definitions, rename them, update our definitions.json and pipeline.json files, and validate.  This was a terrible ordeal.

At some point, we discovered that, via REST API's, we could download the build definition json and put it in our product source repos as code.  Moving to this method was a HUGE win when we would branch for a release.  It should be noted that we didn't branch our orchestrator along with servicing branches, or the VSTS build definition which ran the PipeBuild orchestrator.  We always thought we were going to move away from PipeBuild, so we've lived with this state, but if I had to push for one more feature to add to our current PipeBuild tool, it would certainly be to add both of those pieces as code.

One other minor win we had with checked in definitions, was associating the build definition ID of the orchestrating PipeBuild VSTS definition with the orchestrated build legs VSTS build.  Prior to this minor change, it was an engineering feat to determine which of the various PipeBuild definitions representing various servicing releases had scheduled a particular build leg.

Checked in definitions are clearly superior for DotNet teams because of their branching nature, but there are certainly some downsides to our current iteration
- We never invested in a clean flow for modifying checked in definitions.  The JSON dump style works, but it includes a bunch of extra data which devs don't care about, or understand.  I wrote a tool that launches a web browser and loads the local JSON into a new build definition so that it can be iterated on via the standard VSTS definition editing process.  After making changes via the web UI, you can use the same tool to download the code again locally.  The tool was never truly invested in though, and often the code you downloaded from VSTS would look vastly different because of back-end formatting changes which would add additional metadata or change GUIDs (devs never understood Task GUIDs).
- The JSON build definitions contained too much extra metadata and it was nearly impossible for any dev to manually make a change to the definition with a text editor unless it was changing a variable name / value.
- Secrets became confusing because they couldn't be defined by the JSON, they were provided by the PipeBuild orchestrator.

### Feature 7 - Conditional build legs

I'll be brief on this topic because VSTS now provides custom conditions.  Prior to custom conditions, we had to implement a feature that would skip an entire build leg (we didn't have step / task level access) for some publishing scenarios.  Yay custom conditions!

### Feature 8 - Azure Key Vault

Secret management started to become more and more of an issue.  Whether it was a PAT expiring or (and I'm guilty of this on one occassion) an unintended secret getting leaked, when you had to update a secret in VSTS, it became a nightmare.  

- Multiple product teams owning their own builds meant that the same secret was defined by differently named variables in different repos
- Multiple devs working on builds meant that sometimes secrets were unintentionally duplicated with different variables
- You can't read the secrets (duh), so, without careful monitoring, it became nearly impossible to know which variable applied to which secret.

Secrets were defined all over the place!  Updating them was a nightmare.  Thankfully, we were following the practice of keeping our secrets in Azure Key Vault so that we could retrieve them if necessary; for example, when bringing up a new orchestrated build.  There was still no connection between key vault and the VSTS build definition, so if we had to update a secret it fell on one of two or three devs that were familiar with the system to go through and manually update all of the numerous build definitions.

Our solution was to build in azure key vault access to our build orchestrator.  Rather than keeping secrets in VSTS build definitions, we kept plain text values like this...

```
[AzureKeyVault=EngKeyVault,SecretName=dn-bot-devdiv-build-rw-code-rw]
```

When PipeBuild saw a variable with a text value looking similar to the above, it would connect to Azure Key Vault and retrieve the specified secret from the specified vault.  After this change, there was only one secret (the access Azure Key Vault secret) in every PipeBuild VSTS definition, and all of the rest of the secrets were plain-text encoded values.  Devs could reason about what a secret was, and where to find its value.  Cycling a secret now meant updating the value in Azure Key Vault and every build definition would just continue to work.  

On the down side, individual build legs still have no connection to the azure secret values, they just have null values in the JSON marked as secret.  So, you still have to know to go look at the PipeBuild VSTS definition to figure what the secret value is.

### Things we never solved

As a reminder, the .NET Core Engineering team never wanted to own a build orchestrator.  It was a product which was created to fill a need which VSTS (and other solutions) weren't providing.  The tool worked (with some pain along the way), but since it was always viewed as a temporary solution, it never was fully funded or treated with the intent of being a top notch piece of infrastructure.  That mindset meant that many features which would have made our lives much easier, were added late, or never implemented.  Here is an additional list of some of the features that we always wanted to implement but never got funded.

- PipeBuild definition not checked in.  The PipeBuild orchestrator is a tool that is launched via a VSTS build definition.  While individual build legs are defined by checked in JSON code, the orchestrating definition is not. PipeBuild does not fork with the code and neither does the launching VSTS build definition.

- PipeBuild was never versioned.  Every official build in every branch of every product, uses the same code base (HEAD).  That meant breaking changes were not possible though they happened on occassion at great expense.  Some times a breaking change wouldn't be discovered to have occurred until a dormant servicing branch was spun up to produce a servicing fix.  The cost at that point is prohibitive both because spinning builds in a servicing branch can be difficult, and because knowledge of how that branch worked could be lost.

- Clean PipeBuild output. Our PipeBuild output just periodically queries VSTS for build status and dumps the output to the UI console. Tracking down a failing build leg while the build was in progress was difficult as its status would just scroll off the screen if you weren't diligent.  If you waited until the build completed, you would get a dump of the failing build legs, but you had to scroll to the bottom of a lengthy output and doing this on a mobile device was an exercisein extreme patience.

- Dev workflow for modifying definitions.  Checked in definitions were great, but never fully supported after implementation.  It was difficult to reason about the JSON defintions, and editing them required every dev to ping me for access to the hacky tool I had written and mentioned above.  Merging two JSON blobs representing different VSTS api versions was very arduous.

- Test changes to PipeBuild code path.  To this day, there is no great way to test changes to our official builds without actually merging those changes and scheduling an official build.  The current "best" work around, is to clone a definition, disable official build logging, change the title, and figure out some way to disable publishing (remove the publishing leg, change the publishing endpoint, or change the build number format to ensure there is no package version conflict when publishing to MyGet).

- Shared Libraries for build orchestrator.  For good and bad reasons, shared infrastructure libraries are currently available via DotNet BuildTools. BuildTools provides some benefits, but there are a lot of current concerns over the tooling.  Discussing the concerns with BuildTools is an entirely different effort.

- Replayability.  Reliability has been (and continues to be) one of the most outstanding (not in a good way) issues that we have with respect to builds.  There has been a tremendous effort to invest in our infrastructure so that it is resilient to intermittent network issues.  In fact, we have been so busy fighting intermittent network issues, machine issues, disk space issues, etc... that we have not had time to invest in recovering from failures.  We create a lot of builds for a single orchestrated build, and failures occur regularly.  Today, we're forced to hotfix the failure (if possible) and then respin an entire build.  This means that any failure requires at least a two hour reset to attempt to get a clean build.  Multiplying that single repo concept across an entire orchestrated product (requiring many repos to successfully build) can lead to a nearly impossible task of getting 6 repos to build cleanly and understanding that any repo failing will likely lead to a day delayed trying to get another build.  Re-entrant orchestration where one failing repo could be replayed without resetting an entire orchestrated build would be monumental.  Barring that, even having replayability so that a single repo is not forced to entirely reset would provide benefit.  


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CVSTS%5Cpipebuild-feature-history.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CVSTS%5Cpipebuild-feature-history.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CVSTS%5Cpipebuild-feature-history.md)</sub>
<!-- End Generated Content-->
