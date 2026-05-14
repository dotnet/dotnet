# Arcade Breaking Changes Policy

## Overview
In a nutshell, no breaking changes within a major version (SemVer rules) except where absolutely necessary.  To be sure there are some nuances to this, but in general - Arcade doesn't do breaking changes.  

## Validation:
In order to not break Arcade consumers, there must be comprehensive validation *before* pushing to the latest channel.  From there, it's expected that the repo's PR validation would catch things specific to that repo.  More specific here. [Arcade Validation Document](../Validation/Overview.md)

## Policy:
Again, the intent is to not break consumers outside of major versions wherever possible.  A bit more on this:
-	Note the last principle: [Arcade Overview](../Overview.md)
-	Arcade updates: [Arcade Servicing](../Servicing.md)
-	Toolsets consumed by Arcade: [Arcade Toolsets](../Toolsets.md)

## Process for when Breaking Changes are required
1. Email 'arcadewg' with the breaking change particulars
1. A day later email 'dncpartners' and give them a minimum of a 2 day warning 
1. Two days later, a 'we're doing this now' email to ‘dncpartners’
1. Proactively provide support and/or help as required 

# Arcade Disruptive Changes Policy

## Overview
From time to time, it is necessary or desirable to make a change which is not technically breaking, but is likely disruptive. The process is largely the same as breaking changes.

## Validation:
In order to not break Arcade consumers, there must be comprehensive validation *before* pushing to the latest channel.  From there, it's expected that the repo's PR validation would catch things specific to that repo.  More specific here. [Arcade Validation Document](../Validation/Overview.md)

## Policy:
Disruptive changes should be minimal.  However, it should be understood that they will happen, although very infrequently.  As a rule of thumb, if there is more than one (1) disruptive change per month, a root cause investigation should be initiated.

## Process for disruptive changes
1. Email 'arcadewg' with the disruptive change particulars
1. A day later email 'dncpartners' and give them a minimum of a 2 day warning 
1. Two days later, a 'we're doing this now' email to ‘dncpartners’
1. Proactively provide support and/or help as required 


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CPolicy%5CChangesPolicy.md)](https://helix.dot.net/f/p/5?p=Documentation%5CPolicy%5CChangesPolicy.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CPolicy%5CChangesPolicy.md)</sub>
<!-- End Generated Content-->
