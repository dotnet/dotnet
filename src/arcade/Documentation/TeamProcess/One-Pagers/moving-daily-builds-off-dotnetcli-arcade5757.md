# Moving daily builds off the dotnetcli storage account (Related to [Arcade epic #5757](https://github.com/dotnet/arcade/issues/5757))

## Summary 

Prior to .NET Core 6.0, all internally-produced daily builds of .NET Core were Authenticode "real" signed and published to the same storage locations used for official releases of .NET Core. In latest builds, the same publishing locations continue to be used but non-final builds are not real-signed.

While publishing to a single location is convenient for the infrastructure, it poses multiple risks.  Every build that publishes ends up handling secrets that allow writing to the official storage account, making these builds sensitive security-wise. These builds' publishing infrastructure could also simply contain bugs and accidentally overwrite blobs they should not, or delete them simply by accident. To prevent this we should move daily builds onto a storage account (possibly in a different subscription entirely, but this is not necessary), and secrets that relate to publishing to the production account can be removed from almost all locations and be made available to a select few Azure DevOps build pipelines, and only when publishing real assets.

As this will be a disruptive change, something to do at the same time that can improve safety would be to start using [the Azure Storage "Legal Hold" flag](https://docs.microsoft.com/en-us/rest/api/storagerp/blobcontainers/setlegalhold) for all containers with .NET Core builds in them. In experiments, this easily-reversed container-level change allows adding new blobs but blocks deleting or modifying existing ones. This means an extra layer of protection both from engineering errors (e.g. accidental overwriting, accidentally duplicating content) as well as meaning malicious users obtaining storage account credentials could not modify existing assets to contain their payloads.

## Stakeholders

- .NET Core Engineering Services team (contact: @dnceng)
- .NET Core SDK team (contact: @marcpopMSFT)
- .NET Core Docker team (contact: @MichaelSimons / @mthalman): The .NET Docker team produces Docker images on mcr.microsoft.com that preinstall .NET Core SDKs and may need to adjust this.
- .NET Install Scripts team (contact: @vlada-shubina / @BOzturkMSFT): If a new location for .NET Core SDKs/ Runtimes is introduced, the install scripts should support this directly for non-official build
- .NET Core PM Team (contact: @richlander) : Any change in the source of our runtimes / SDKs will need to have a public annoucement letting users know this is coming, as there are some number of external customers using daily builds in their existing development / CI process.
- .NET Core Runtime teams (various; most other than SDK will uptake any change in behavior from Arcade.)
- .NET Core Release team (contact: @leecow https://github.com/dotnet/release)
- Anyone using dotnet-install to download daily bits

## Risk

- What are the unknowns? 
  - We don't know outside common Arcade functionality where all dependencies on these storage account URIs are located.  There will almost certainly be a multi-week tail of folks finding their build or dev workflow broken and pinging us from inside and outside the .NET org.
  - We actually don't know how many users actually use dailies (following up with @RichLander)
  - We don't know all the places where usage of dotnetcli URLs is hard-coded in calculating download urls, but there are definitely plenty.

- Are there any POCs (proof of concepts) required to be built for this epic? _(note I used the "epic" one-pager; this might not be an epic)_

  There is nothing super groundbreaking about having multiple locations that one might find a build in, with a preference hierarchy for when the same content is found in more than one location. That said, here are some things I imagine we might want to try rigging up versions of the dotnet-install.sh / ps1 scripts that start looking in dotnetcli, then fall back automatically to the new build storage account (named something like "dotnetbuilds") to get feedback on whether this is satisfactory.

- What dependencies will this epic have? Are the dependencies currently in a state that the functionality in the epic can consume them now, or will they need to be updated? 

  None - it's new work to improve how and when we access secrets that expose our sensitive assets are, along with making it clear via simple base URI that a build is not official.

- Will the new implementation of any existing functionality cause breaking changes for existing consumers? 

  This change guarantees there will be some breaking changes for users who miss public communication about the changes to backing storage for .NET Core SDKs/Runtimes. I would alse expect to see some problems just arising from bugs and needing to run the "machinery" of our build/release project to catch all the places that this occurs. This can be mitigated in the short term by continuing to do the old behaviors (e.g. sign all binaries and publish to both locations) until we feel confident in the content being uploaded to the new storage account.

- Is there a goal to have this work completed by, and what is the risk of not hitting that date? (e.g. missed OKRs, increased pain-points for consumers, functionality is required for the next product release, et cetera)

  Aside from the challenges listed, there is no specific date or milestone this work must be completed within. It should be done as quickly as safely possible for its benefits but is not tied to any particular milestone or product release date.

## Open questions 

- Is this actually in the Unified build access epic? (dotnet/arcade#5757)

  I don't know, but I'm at least writing this one-pager as part of that epic.

- Do we actually want external users to have access to unsigned .NET Core SDKs? 

  Per discussion with @mmitche: "Yes; we're (implicitly) saying that MS has not made a statement about the quality or validity of it, and if the user (say, Bing.com) wants to ship it they have to sign it"

- Same question, but do we want unsigned SDKs building official builds?  

  Per discussion with @mmitche, it's unavoidable in certain cases, and while ideally we'd only use offically released previews,  Barry says this is not a problem"

- How can we actually determine how many external / internal users are using dailies? 

- Who owns the SDL validation and the new CDN that would likely be needed here (note: a sufficiently geo-redundant storage account can likely handle this performance-wise)

- Is there an SDL Threat Model for dotnetcli? 

  Nothing since 2016.  It is likely something we must address. 

- Where should these resources live, and who should own them?

## Components to change, with order/estimates of work to do

### Component: dotnet-install scripts 

  As these scripts are the most common entry point for installing .NET Core SDKs and runtimes, including for most DncEng infrastructure, updating these scripts would be the first step such that as soon as builds start publishing bits there, these scripts would continue to work as expected. Estimate: ~1 week.

  #### High-level activities:
  - Secure and create the storage account to use (note "dotnetbuild" is taken, perhaps "dotnetbuilds" or one of our pre-existing accounts like "dotnetbuildoutput")
  - Insert secrets for the new storage account into EngKeyVault for usage by builds.
  - Copy over (and munge versions to prove things worked) some builds from the DotNetCLI storage account into this account
  - Modify scripts in https://github.com/dotnet/install-scripts until they can correctly install from the new location (with preference "DotNetCLI -> New Storage account --> Version specified on command line).

### Component: dotnet/arcade repo

  Shared functionality for fetching the dotnet-install scripts, as well as the "where to publish" logic lives in https://github.com/dotnet/arcade. This makes an obvious next place to address.  Additionally, we could bring back logic created earlier to have dotnet-install.sh/ps1 in the eng/common folder of Arcade, allowing Arcade-ified repositories to use the latest script changes before any chance of impacting all other users publicly, or have a secondary location supported for the dotnet-install scripts.

  #### High-level activities:
  - Add variable group(s) containing secrets for publishing to the new storage accounts
  - Introduce the ability to publish daily builds to the new storage account consuming values from these variable groups.
  - Update mentions of storage accounts in documentation to reflect changes.
  - Remove existing referneces to dotnetcli-storage-key variables

### Component: Partner teams (dotnet/sdk, docker, and other repos

  The .NET Core SDK team (and possibly others; we would search through all existing variable usage in their main builds and get a list) would need to make any related changes related to acquiring bits.

  #### High-level activities:
  - Work with SDK team to ensure relevant components know about possible new locations for bits.
  - Scan for all uses of dotnetcli in the main dev branches and start triaging them (understand and convert or update usage)
  - Work with Docker team to understand how they acquire .NET core installations for Docker image creation; this would likely just be a change to the infrastructure that finds URLs and calculates hashes of the bits to be installed in docker images, and ensuring it knows about the new account.

### .NET Engineering Services tasks

  Aside from being responsible for driving this overall effort and all dotnet/arcade changes, the .NET Core Engineering Services team would be responsible for cleaning up after daily builds were migrated:

  #### High-level activities:
  - Remove entirely (or limit the pipelines allowed to use it) the DotNet-DotNetCli-Storage and DotNet-MSRC-Storage variable groups from dnceng.vs and devdiv.vs.
  - Remove all dotnetcli/dotnetclimsrc secrets from EngKeyVault and cycle storage keys (after dailies are working for some time)  
  - Rig up the ability for official release pipelines to continue to publish to the "official" storage account.
  - Set Legal Hold flag on all real containers used for builds and make sure documentation reflects this (along with "how to break out of this temporarily if needed" docs)
  - Updates for publish destinations (i.e.) (PublishingConstants.cs). Change the target storage accounts to the dotnetbuildoutput/dotnetbuilds account.

## Serviceability

- How will the components that make up this epic be tested? 

  Where functionality is changed in a repository, unit / scenario testing will be added. Most of the work is high-level configuration of Azure DevOps and builds so the testing will running the machinery in question. For the end-to-end "full .NET Core SDK is produced and uploaded to the new account" scenario, this will require a manual test plan to ensure that at least the happy-path works before turning functionality on.

- How will we have confidence in the deployments/shipping of the components of this epic? 

  We'll know things have successfully moved over when we disable / delete the secrets related to the dotnetcli storage accounts from shared key vaults, cycle these values, resolve outstanding issues, and continue to function.

- Identifying secrets (e.g. PATs, certificates, et cetera) that will be used (new ones to be created; existing ones to be used).
  - Storage Account Keys (used for publishing), rotated via Azure Portal
  - Storage account container-specific SAS tokens, generated programmatically or via Azure Portal

- Does this change any existing SDL threat model?
- Does this require a new SDL threat model?

  The most recent threat model I am aware of dates back to 2016 and was written before Azure DevOps public support existed. While this change is largely meant to improve security, it's clear this area is due for some SDL review regardless of this change.


### Rollout and Deployment
- How will we roll this out safely into production?

  This may be difficult; it's impossible to know everything you don't know. However, making the storage-account choice conditional on build type and enabling it a little bit at a time make it relatively simple to undo if major problems are hit.  Additionally, publishing to both locations in the beginning may represent the simplest way to proceed. This would be temporary, to allow us to rapidly switch back in case of problems.

    - Are we deprecating something else? 

    No, all previously defined storage accounts for SDKs/Runtimes continue to exist, they only get fewer and more meaningful insertions, along with making blobs inherently immutable via the "Legal Hold" feature.

- How often and with what means we will deploy this?

Once adopted these changes are enshrined in the DotNet Arcade publishing workflow, so changes are deployed to where they're used via regular Arcade dependency flow pull requests.

- What needs to be deployed and where?

  New storage accounts will be added to a subscription (TBD; this subscription will likely need to be treated "special").  Everything else comes from changes to Azure DevOps pipelines and the dotnet/arcade repo.

- What are the risks when doing it?

  While change is ongoing, access to daily builds, the ability to produce new release builds, and .NET Core repositories' official pipelines may be broken for some time.


## Usage Telemetry
- How are we tracking the “usefulness” to our customers of the goals? 

  As this is an improvement for both security and reliability, we don't care how useful these changes are for users, just that they can still perform their builds through some means.

- How are we tracking the usage of the changes of the goals? 

  Usage metrics would be based off the existing dotnet install telemetry (and likely this is where we'd have to go to know who is using non-release builds).


## FR Hand off
- What documentation/information needs to be provided to FR so the team as a whole is successful in maintaining these changes? 

Changes to the publishing workflow, specifically the components inside dotnet/arcade, will need to be detailed and stored in the wiki or documentation folders of dotnet/arcade.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cmoving-daily-builds-off-dotnetcli-arcade5757.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cmoving-daily-builds-off-dotnetcli-arcade5757.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cmoving-daily-builds-off-dotnetcli-arcade5757.md)</sub>
<!-- End Generated Content-->
