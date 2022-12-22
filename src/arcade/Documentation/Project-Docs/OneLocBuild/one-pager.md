# Generating the Localization Index File (`LocProject.json`) for the New Localization System (OneLocBuild)

## Project Summary
For a variety of reasons, the localization workflow is changing and we need to [migrate to the new loc system](https://github.com/dotnet/arcade/issues/6842).
This system is essentially an Azure DevOps task([OneLocBuild](https://ceapex.visualstudio.com/CEINTL/_wiki/wikis/CEINTL.wiki/107/Localization-with-OneLocBuild-Task))
that we run in each repo's build pipeline to gather up our English resource files, send them off to the localization system, and receive
localized resource files back. Because this is common infrastructure that will need to be implemented across all of our customer repos,
it makes sense to implement it in Arcade.

More information on migration can be found [here](https://ceapex.visualstudio.com/CEINTL/_wiki/wikis/CEINTL.wiki/1481/Migrating-out-of-SimpleLoc?anchor=ado-pipeline-creation-for-projects-hosted-in-github).

A major component of the new localization system is an index file called `LocProject.json`. In the linked documentation above, this file is
checked into the repository, which would require every single repo to maintain a list of all of their resource files that need to be localized
manually and make sure it stays in sync with their changes. This is a non-ideal solution. Instead, we hope to generate the `LocProject.json` file
at build time prior to running the OneLocBuild task.

## Goals
The primary goal of the project is to generate the `LocProject.json` file at build time so that we can automate the localization process
as much as possible with minimal to no intervention from customer repos.

## Stakeholders
The primary stakeholders for this project are the .NET Core Engineering team (who will maintain this process on
behalf of our customer repos) and the localization team. The other stakeholders include all of the customer repos who may have
to maintain some new files for localization on their end if we aren't able to completely automate the work within `eng/common`.

## Risk
The most significant risk facing this project is any information in the index file that needs to be manually tweaked. As an example,
[@RussKie](https://github.com/RussKie) created a [first pass attempt](https://github.com/dotnet/arcade/issues/6842#issuecomment-771963490)
at this and found that he still had to manually remove some .resx files from the `LocProject.json` file after generation. We could
create something like an exclusions list or other file that is *more* static than the `LocProject.json` would be to take much
of the burden off of customer repos if we can't find a way to be smart with our file inclusion.

To mitigate this risk, we will work with the localization team to see if we can replicate the logic they were using previously when they were
automatically scanning our repos and loop in the customer repos to make sure the localization pipelines are working properly for them.

A second risk facing us is the need to backport this to servicing branches of Arcade. At this time it is unknown if there will be significant
challenges to this separate from the ones we currently face in master. We will work in tandem with [@mmitche](http://github.com/mmitche)
to make sure that this is mitigated as much as possible.

Finally, the hard deadline of March 31, 2021, which is when the old localization system will be turned off is a risk. While we will likely
be able to accomplish the majority of the work by this point, the unknowns of the servicing branches, in particular, are worrisome. We will work
with the localization team to put in place a temporary manual process they recommended if this date slips for any of our branches.

## Serviceability
Two PATs are required by the OneLocBuild task: a GitHub PAT and an AzDO PAT for the [ceapex organization](https://dev.azure.com/ceapex).
The latter will have to be created and maintained.

There will be tests for the `LocProject.json` generation script and any other scripts that are created to ensure they are generating files
correctly.

### Rollout and Deployment
This project will be tested thoroughly in customer repos before we check it into Arcade. Once we do check it into Arcade, it will simply be rolled out
as a part of Arcade.

## FR Handoff
Most likely, we will only need to write a single document on the results of this project to facilitate FR handoff.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5COneLocBuild%5Cone-pager.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5COneLocBuild%5Cone-pager.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5COneLocBuild%5Cone-pager.md)</sub>
<!-- End Generated Content-->
