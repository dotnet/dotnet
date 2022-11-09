# Servicing

## Overview
Servicing (updating) packages and tools that are used in support of the build is an important part of being sustainable over time.  This document touches on the primary policy and mechanical aspects for how Arcade intends to handle servicing.

## Requirements
* Allow updates to tools and packages to quickly flow to all participating repos, with reasonable confidence, and minimum effort.
* Multiple versions must be available
* Tools and packages must be able to fork with the product code where necessary
* Each tool and package must have a specific version. In cases where Arcade builds the tool/package, the hash must be included as well.
* Telemetry must be available to determine who's using which tool/package 

## Policy
* All repos should "roll forward" to whatever the latest tool and package is wherever possible.  This includes product servicing branches.  In short, every (reasonable) effort should be made to stay on the latest version available.
* All repos should be fully subscribed and getting automated updates.
* Breaking changes from Arcade should be 1) rare, 2) have a very strong business case, and 3) have a clear migration/mitigation plan for all affected repos.
  * In cases where very few repos are affected, updating the repo itself is a viable option.
  * In cases where no better option exists (last resort), the tools or package can fork or be quirked.
* Only push a new CLI to the repos in the stack once it has been signed off by our consumer distros.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CServicing.md)](https://helix.dot.net/f/p/5?p=Documentation%5CServicing.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CServicing.md)</sub>
<!-- End Generated Content-->
