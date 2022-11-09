I sat down with the intent of prototyping an orchestrated build of a single repo using the new VSTS "YAML definitions" preview feature.  When I started writing this doc, I intended to just cover my experience, but I realized that it's difficult to understand why some things bothered me without giving some background about where our build orchestration efforts began and where they are now.  Given the early stage of the feature, it was obvious that I would encounter some hiccups, and it wasn't clear just how close to a working prototype I would get.  What follows is a high level overview of my initial reaction to the VSTS Preview Features in their current state and then my notes related to the YAML definitions and how that experience deviates from our current orchestrated build solution (PipeBuild).  For additional context, there's a companion document I wrote which covers a brief [history of PipeBuild](pipebuild-feature-history.md) and the more impactful features we added to it as our needs evolved.

For context, when I say, the "YAML definitions" preview feature, I'm including the VSTS features which permit configuration as code (different than checked in build definitions), and the VSTS implementation of a Pipeline (orchestrated repo build).  From my limited experience, there is no current "Product orchestration" feature which would support orchestrating multiple repository spanning builds.  You could certainly design something within the current feature set to support product orchestration, but it's not a fully supported first-class feature and it would be a bit of an unmanageable mess.

# VSTS Preview Features

Before I dive into my experience, I wanted to briefly cover the involved VSTS Preview Features and my initial take on them. 

## Config as Code

"Config as code" is not the same as a "checked in build definition".  In PipeBuild, we take all of the json that defines a VSTS build definition, download it, and check it in to source.  In "YAML definitions", you take only the "configuration" parts that define a build definition and check them in as code.  I think this abstraction is positive.  The abstraction allows us to store just the data we care about and want to manipulate.  There's a lot of pieces to a "checked in build definition" that we store that we just don't care about (or understand).  On the server side, they have meaning, but to us it's just ignored data which gets in the way when we're trying to understand the data we're looking at and how we want to change it.  It also means, I would hope, that any changes to build definitions would be seamless to "YAML definition" users.  I think that, long-term, if we had decided to continue investing in PipeBuild, this is the direction we would have gone.  It was always on our radar, as a next step, but checked in build definitions filled our immediate need and there was never a desire to further invest in PipeBuild.    

**Concerns:**
- One primary concern is what gaurantees we have that the API is not going to change and force us to make widespread updates to all of our various checked in "config as code" files. I don't see any way to specify a specific api version.  At the moment, the API is in constant flux.  Our previous history with VSTS has shown that this can be problematic if the agent API's change.  Rolling forward should be an intentional /validated process (think Maestro PR's for dependencies but with API versions).  

## YAML

I had no experience with YAML before playing around with "YAML definitions".  It's not terribly difficult to pick up.  There's less than an hour of documentation to read about the language specification, and then it's mostly understanding VSTS' schema implementation (which is fairly well documented though constantly changing).

**Concerns:**
- Why another data format?  We've built up a lot of infrastructure around JSON (mostly deprecated now), then MSBuild, and now YAML.  I understand that MSBuild is not the perfect language format for many reasons, but history has shown it to be resilient and Microsoft (.NET Core) has decided to invest in that direction in spite of convincing arguments for other data languages (JSON).  History has shown that we are stronger when staying within the Microsoft technology ecosystem.  

There are numerous examples where we tried to break away from Microsoft technology (for very valid reasons).  .NET CLI put a large investment in using project.json's for package references then eventually transitioned to MSBuild.  .NET Core moved to Jenkins for CI (and peripherally for official builds), but now is moving towards Microsoft VSTS.  I'm certain that there are some counter-examples to this where moving away from our own products has been a long term win, like moving to GitHub for source control vs TFS. Though, even in that instance we are moving to Git technology but Microsoft hosted source control.   

Using yet another format means more investment, particularly in places where there is an intersection of data sharing between builds and code.  I'd be curious to know what motivated YAML as the format of choice.

## Pipelines

The Pipeline functionality (scripted steps, parallel builds, matrix definitions) seems to be a reasonable implementation which covers much of the orchestration required for a single repo build.  I have some feedback on current implementation details, but overall the model makes sense and its representation in the VSTS UI is fairly clean.  It would be nice if phase dependencies were a bit more clearly defined.  ie, currently, there is no way to visually determine which dependency is blocking a phase from running.

**Concerns:**

- It's not clear to me if product orchestration is intended to be a fully supported feature of VSTS or up to product teams to implement via Pipelines.  If it is left to product teams, then the infrastructure investment on each team could be quite large given that I don't know how the current feature set would support re-entrant orchestration or assist in producing builds reliably (network failure retries).

# PipeBuild development
For additional background and context, please see [pipebuild-feature-history.md](pipebuild-feature-history.md).  Note that the linked doc does not specifically list features which VSTS doesn't support, it's just additional context.  Some of the features mentioned there have already been solved by VSTS, others have not.

# VSTS Prototyping Preview Features
Much of my experience is negatively skewed by the fact that the development work is still in progress. I do think that the experience I had, however, could prove valuable in moving the system from an effective system, to a slightly more user-friendly system.  I don't intend to prescribe implementations here, only to cover my experience with VSTS Preview Features from the perspecitve of someone transitioning from PipeBuild.

## Hello World
My initial step in creating a YAML definition was to create a simple "Hello World" YAML file.  This was a pretty straight forward task and quickly accomplished.  What I immediately realized though; was that, for more complext tasks, the model of coding locally, checking in, pushing to Git, and then scheduling a VSTS build to validate was not efficient.  

## Dev Loop
In considering how to improve my development loop, I stumbled upon the VSTS Agent source and the "--runLocal" option.  This seemed like precisely what I needed to speed up my investigation.  I installed an agent locally from the VSTS page, then I was able to test my YAML files on my dev box.  Here, I made a couple of bad assumptions.  Some of the features in the documentation didn't work, so I thought I assumed that the agent I downloaded from the page wasn't current.  I cloned the agent source, built it, and updated the agent binaries I was using to Latest.

>  Note: I was pleasantly suprised to see that the build cmd file restored an sh.exe which was then used to pass build commands to the build sh file.  The cool result was that the cmd file was just a wrapper around the sh file on Windows instead of duplicating a Windows and a Linux variation of the build script.

Even with the latest code, the agent would complain about the schema of my YAML file and I made the bad assumption that the schema documentation was out of date and that the agent source code was the source of truth.  I was able to attach a debugger to the agent and reason through the schema it expected so that I could produce a reasonably complicated YAML file.  

It turns out that the agent source code was not the source of truth, though neither was the documentation.  Some version of the source code which was running on the server was the source of truth, the documentation was a close second (with a few days lag), and the agent was just a source of confusion.  Regardless, it didn't stop me from making progress locally and I didn't realize this was an issue until the next day when I moved to testing on the server so that I could run jobs across platforms.  Using the VSTS UI to validate schema changes in YAML is a horrific experience.

## VSTS versus PipeBuild gaps
So, where does current VSTS functionality not quite close the gap to what we're accustomed to with PipeBuild?

Here are some items that we must have in order to start using VSTS preview for our builds.

- Azure Key Vault - currently not supported in the YAML schema I previewed though Chris Patterson has told me this is now suported via a task.

- Templates - This feature was deprecated in the prototyping I was doing, but it would allow you to import YAML from another file into your pipeline.  Support for this will definitely make for a cleaner (less error-prone) code-base.

- Agent pools - Currently YAML definitions are only enabled in the Hosted agent pools.  That means, we can't actually build our product because the machines in those pools don't have the necessary pre-requisites installed.

- Build number format.  It doesn't appear that there is (currently) a way to control the build number format via YAML. The documentation hints that this is on the radar very soon, https://github.com/Microsoft/vsts-agent/blob/master/docs/preview/yamlgettingstarted-features.md.  Why does this matter to us?  Currently, our PipeBuild VSTS definition will provide a specifically formatted build number which is then parsed by a library to produce an "OfficialBuildId".  That OfficialBuildId is passed to every build leg so that generated binary and package version numbers are consistent for a build.  An inability to control the build number format via YAML isn't necessarily a regression from what PipeBuild is doing today, but it does mean that we lose some of the benefit of config as code because anybody pointing at a YAML file will need to specifically know to go update their VSTS build definition to produce the proper build number format.

- Telemetry - How do we report status to Mission Control? or elsewhere?

These ae additional items which it would be great to utilize, but we could work around if they're not present

- Docker - Docker is on the radar for support (https://github.com/Microsoft/vsts-agent/blob/master/docs/preview/runtaskindocker.md), but I was unable to get this feature to work using the "Hosted Linux Agent" machine pool which supports YAML definitions.  Having this as a supported task is fantastic, once it works... In the interim, we could implement functionality using similar semantics to how we build for docker today.

- Tasks - At the moment, using any task, or understanding how to implement a task in your YAML schema is extremely difficult.  Chris Patterson says that there is work in progress (shipping m126 [aka next week]) which will allow  you to configure a task or definition in the UI and right-click to copy it to YAML.

- Cleanup - I don't know if this is yet a priority or if we continue to use our infrastructure to clean agents / docker.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CVSTS%5Cvsts-preview-versus-pipebuild.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CVSTS%5Cvsts-preview-versus-pipebuild.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CVSTS%5Cvsts-preview-versus-pipebuild.md)</sub>
<!-- End Generated Content-->
