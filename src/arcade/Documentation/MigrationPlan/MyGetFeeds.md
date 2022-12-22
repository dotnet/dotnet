# Plan-Of-Record for discontinuing our use of MyGet

## Summary
Now that our primary dependency is on Azure DevOps feeds for .NET Core 3.x, it's time to finish the work so that we can completely discontinue our use of MyGet.  The main business driver is senior management's strong desire to take our mission critical dependencies on end points we own, which in this case is AzDO over MyGet.

## Scope and Impact
- We will be shutting down dotnet.myget.org.
- Based on a quick audit, the following are the primary folks still pushing.  (e.g. impacted)
  - Fsharp, Roslyn, Tomas, UWP, Corefxlab, NuGet, MSBuild
- See Appendix A for the audit results.  (NOTE: there are likely some feeds which we don't have visibility to, but these won't be public of course)

## Plan Overview
1. Investigate and broadly socialize this plan with product teams to both set expectations, and catch any gaps.  (end of Jan 2020)
2. Make all MyGet feeds read-only (first part of Feb)
3. Bulk load a new ‘legacy’ feed in Azure DevOps from the existing MyGet feeds (end of Feb)
4. Turn off MyGet (first week of March)
5. Finish cleaning up (April)

## Details

### Investigation and Socialization
- The scope includes all feeds on dotnet.myget.org which includes servicing.
- We will need to communicate to our community about this change.
- This migration will be disruptive, so setting clear expectations, and giving us plenty of time to adjust will be key to our success.
- There are a variety of partner teams which depend on MyGet today to get updates to .NET Core.  These consumers need to be identified as part of the investigation/socialization effort so that they can move their dependencies.
- Further investigation is needed to determine the best approach for other types of packages, e.g. VSIX.  Universal packages might work...


### Make MyGet Feeds Read-Only
- This prevents the publishing of new packages which gives a stable "snapshot" to migrate.
- Before this happens, it's very important that partner teams are aware that new packages can only be published to the Azure DevOps feeds.  
- There will likely be some fallout from this step.  Once it's done however, our confidence will be higher that we have functional understanding of who the consumers of our feeds are.

### Bulk Load a "Legacy" Feed
- MyGet is backed by our own Azure storage location.  The idea is to bulk load a new "legacy" feed in Azure DevOps with all of our existing packages in MyGet.
- This should allow our servicing builds to simply add a new feed to their nuget.config.
- It's very important however, that no new packages be published to this legacy feed.  The eventual idea is that we can simply "turn off" the legacy feed once the usage is either gone or minimal.

### Turn off MyGet (and clean up)
- The two main factors which should make this transition reasonable are:
  - We don't have that many dependencies on MyGet left in our 3.x builds (relatively speaking)
  - There will be a "legacy" feed for existing packages.
  - The migration is being staged over time
- It is understood that this transition will be noisy and the engineering servicing team is committed to doing whatever is necessary to help out where and as needed. 

### Appendix A - donet.myget feed audit (from 12/18/19 courtesy of Matt Mitchell)

|	Feed	|	Team	|	Last Push	|	Teams who recently pushed?	|	Notes
|	 ----------------------------	|	 ----------------------------	|	 ----------------------------	|	 ----------------------------	|	 ----------------------------
|	dotnet-core-svc	|	runtime	|	4 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-1-0-4-patch	|	aspnet	|	3 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-1-0-5-may2017-patch	|	aspnet	|	3 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-1-0-5-may2017-patch-public	|	aspnet	|	3 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-1-0-6-september2017-patch	|	aspnet	|	2 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-1-1-0-rtm	|	aspnet	|	3 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-1-1-1-patch	|	aspnet	|	3 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-1-1-2-may2017-patch	|	aspnet	|	3 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-1-1-2-may2017-patch-public	|	aspnet	|	3 years ago	|		|	
|	aspnet-1-1-3-september2017-patch	|	aspnet	|	2 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnet-2-0-2-october2017-patch	|	aspnet	|	2 years ago	|		|	
|	aspnet-2-0-2-october2017-patch-public	|	aspnet	|	2 years ago	|		|	
|	aspnet-2018-feb-patch-public	|	aspnet	|	2 years ago	|		|	
|	aspnet-feb2017-patch	|	aspnet	|	3 years ago	|		|	
|	aspnet-signalr	|	aspnet	|	8 months ago	|		|	
|	aspnetcore-2-0-0-preview1-no-timestamp	|	aspnet	|	3 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnetcore-tools	|	aspnet	|	10 months ago	|		|	
|	aspnetcore-dev	|	aspnet	|	1 week ago	|	aspnet	|	Some 5.0 npm packages, 2.2 servicing
|	aspnetcore-feature-work	|	aspnet	|	2 years ago	|		|	
|	aspnetcore-master	|	aspnet	|	2 years ago	|		|	
|	aspnetcore-patch	|	aspnet	|	2 years ago	|		|	
|	aspnetcore-rel-1-0-3	|	aspnet	|	3 years ago	|		|	Looks like some packages were replaced in-place about a year ago
|	aspnetcore-release	|	aspnet	|	2 years ago	|		|	
|	aspnetwebhooks	|	aspnet	|	2 years ago	|		|	
|	aspnetwebstack-dev	|	aspnet	|	1 year ago	|		|	
|	blazor-dev	|	aspnet	|	7 months ago	|		|	
|	dotnet-1-0-3-rtm	|	runtime	|	3 years ago	|		|	
|	dotnet-2-0-0-preview2-final	|	runtime	|	2 years ago	|		|	
|	dotnet-2-0-0-rtm	|	runtime	|	2 years ago	|		|	
|	katana-dev	|	aspnet	|	1 month ago	|	builds of aspnet/AspNetKatana. Chris Ross	|	
|	javascriptservices-dev	|	aspnet	|	3 years ago	|		|	
|	msbuildtools	|	aspnet	|	3 years ago	|		|	
|	open-xml-sdk	|	office?	|	1 day ago	|	Office	|	
|	roslyn-for-vs-for-mac	|	roslyn	|	2 years ago	|		|	
|	roslyn	|	roslyn	|	1 day ago	|	Roslyn, Fsharp, VS Project system	|	
|	buildxl-selfhost	|	buildxl	|	3 month ago	|		|	
|	dotnet-2017-09-servicing	|	runtime	|	2 years ago	|		|	
|	dotnet-buildtools	|	runtime	|	5 months ago	|		|	
|	dotnet-cli	|	sdk	|	1 year ago	|		|	
|	dotnet-core	|	various	|	1 day ago	|	NuGet (only Nuget.Build.Tasks.Pack), UWP6.0, Framework?	|	Some System.ServiceModel.Duplex 4.5.4 packges)
|	dotnet-core-dev-api	|	runtime	|	3 years ago	|		|	
|	dotnet-core-dev-defaultintf 	|	runtime	|	3 years ago	|		|	
|	dotnet-core-dev-eng	|	runtime	|	2 years ago	|		|	
|	dotnet-core-rel	|	runtime	|	4 years ago	|		|	
|	dotnet-core-test	|	runtime	|	10 months ago	|		|	
|	dotnet-coreclr	|	runtime	|	5 years ago	|		|	
|	dotnet-corefxlab	|	corefxlab	|	5 days ago	|	Corefxlab folks	|	
|	dotnet-stage	|	aspnet	|	3 years ago	|		|	
|	msbuild	|	msbuild	|	5 days ago	|	msbuild	|	Disabled publishing by default in August
|	windows-sdk-beta	|	uwp	|	10 months ago	|	uwp	|	
|	rx	|	aspnet	|	5 months ago	|		|	
|	nuget-beta	|	nuget	|	3 years ago	|		|	
|	nuget-build	|	nuget	|	5 months ago	|	Looks like custom dev branches	|	
|	nuget-volatile	|	nuget	|	8 months ago	|		|	
|	aspnetcoremodule	|	aspnet	|	2 years ago	|		|	
|	format	|	runtime	|	1 day ago	|	Builds of dotnet/format.  Almost every update just an arcade update	|	
|	metadata-tools	|	roslyn (tmat)	|	1 year ago	|		|	
|	sourcelink	|	tmat	|	1 month ago	|	tmat	|	
|	symstore	|	tmat	|	4 months ago	|	tmat	|	
|	symreader	|	tmat	|	1 year ago	|		|	
|	symreader-native	|	tmat	|	1 year ago	|		|	
|	symreader-converter	|	tmat	|	2 months ago	|	tmat	|	
|	symreader-portable	|	tmat	|	1 year ago	|		|	
|	orleans-ci	|	orleans	|	1 week ago	|	orleans	|	
|	dotnet-apiport	|	dotnet fundamentals	|	1 month ago	|	dotnet fundamentals	|	
|	dotnet-web	|	mono	|	10 months ago	|		|	
|	temp-projfiletools	|	roslyn?	|	3 years ago	|		|	
|	templating	|	templating	|	10 months ago	|		|	
|	uwpcommunitytoolkit	|	uwp	|	1 week ago	|	uwp folks, I think	|	
|	fsharp	|	fsharp	|	1 day ago	|		|	
|	interactive-window	|	tmat	|	2 months ago	|	tmat	|	
|	roslyn-analyzers	|	roslyn	|	1 day ago	|	roslyn	|	
|	roslyn-tools	|	roslyn	|	1 day ago	|	roslyn	|	


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CMigrationPlan%5CMyGetFeeds.md)](https://helix.dot.net/f/p/5?p=Documentation%5CMigrationPlan%5CMyGetFeeds.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CMigrationPlan%5CMyGetFeeds.md)</sub>
<!-- End Generated Content-->
