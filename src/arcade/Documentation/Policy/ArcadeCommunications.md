# Types of Engineering Services Change Communication

## Goals and Success Criteria
The primary goal is to communicate updates, new features, and anything of note that is changing.  Success looks like:
- No surprises
- New value is known
- Breaking changes are expected, and proper runway is given to mitigate
- All changes (big or small) available to help with transparency that enables easy diagnosability

## Areas to Communicate Changes 
There are several areas around which communication will occur.  The nature of these updates remain the same for each of these areas.
- DNCeng Services (Helix, Darc, etc) 
- Shared Tools (Arcade SDK, Signing, etc)
- Backing Resources (Azure storage, ScaleSets, Microbuild, etc)
- Guidance and best practices

## Classes of Updates to Communicate
- **Breaking** - Breaks something.  This type is the most severe and should be a rare occurrence
- **Disruptive** - Change doesn’t break, but either adds debt with the product teams OR is considered risky
  - Examples: release pipelines, licensing, cert removal
- **Minimal risk** - Some risk, but likely will be fine (these might also be called “updates of note”)
  - Examples: new feature, notable bug fix, etc. 
- **Minor, low risk** - Very low risk, and most folks won't care.  
  - Most changes are these last two bullets.
  - Examples: security update, minor bug fixes, etc

## Engineering Services Policy for Updates
- .NET Core Product release schedules are published and kept "top of mind" for the team
- Tools will **not** automatically flow to release/shipping branches (only to master/dev)
- No updates to shared services during stabilization period (often a week) leading up to release
- Published bar for when we’ll take changes [Arcade Change Bar](ChangeBar.md) 
- Published bar used for determining class of update/change [Arcade Change Bar](ChangeBar.md) 

## Communication Mechanics/Methods
- Breaking changes follow procedure [Arcade Changes Policy document](ChangesPolicy.md) 
- Disruptive changes follow procedure [Arcade Changes Policy document](ChangesPolicy.md) 
- Release notes with “updates of note” (human crafted)
  - Done every time an update is deployed for our services
  - Done at most weekly for tools (e.g. Arcade SDK)
  - Done when items of note occur in one of our backing resources.  For example, public feeds for Azure Artifacts
- Blog entry for larger features/updates of interest

## Automation
The intent is to provide access to a change log which contains all PR’s and the latest SHAs which is always available. (*NOTE: this is not yet implemented*)
- Log is auto generated and posted (not emailed)
- Is not always categorized by type of change because of automation, e.g. breaking etc  (add ML in the future?)
- Resource updates and 2nd party services are out of scope for this automation piece



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CPolicy%5CArcadeCommunications.md)](https://helix.dot.net/f/p/5?p=Documentation%5CPolicy%5CArcadeCommunications.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CPolicy%5CArcadeCommunications.md)</sub>
<!-- End Generated Content-->
