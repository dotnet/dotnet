# Toolsets

## Overview
Various tools exist that are not built in [Arcade](Documentation/Overview.md), but are still needed to support both bootstrapping (both managed and native), as well as various build related workloads needed by different repos.  This document attempts to address the requirements for acquiring these tools that will work for Arcade, and thus the rest of the repos in the .NET Core stack.  Please note that these tools may be either internally or externally owned.  This document applies to both.

## Requirements
* Enable the toolset acquisition portion of 'clone' + 'build' (no prereqs)
* Each tool must have a clear owner (see 'ownership' section below)
* Works for all supported OS's
* Ability to specify (or override default) which version of a tool is needed
* Updates are only made after there is reasonable confidence they won't break
* Both managed and native tools (including any dependencies) must be supported
* It should be possible to reasonably get required tools for new platform bring up.  (distros not yet supported by core)

## Ownership
Each tool/package not built by Arcade, but needed by .NET Core must have a clear owner.  This section describes what it means to own a tool which is used by the .NET core stack.
* Current documentation sufficient to onboard and use the tool for the main supported scenarios
* It must be clearly documented how to get needed updates or fixes done.
* All documentation must be included with the tool
* No breaking changes
* It must be apparent what the current (latest) version is.  E.g., which version to use.
* Might need to be source buildable  (please check with engineering services)

## Basic Mechanics
At a high level, toolsets not built by Arcade are managed by:
* Known location in Azure blob storage for all tools not built in Arcade
* Scripts acquire the correct tools as part of the bootstrapping process  ('clone' + 'build'), or build support/validation.
* Scripts kept up to date by updating every participating repo (Maestro++)
* Community uses the same Azure blob storage end points MSFT does to get the tools from

## Servicing
* See the ["Servicing" document](Servicing.md) for a broader discussion of servicing and its policies
* New tools and tool versions are added to the Azure blob location.  (no replacements, only adds)
* Script updates are deployed to each repo using automation
* When absolutely necessary, a repo can "pin" to a specific "channel", or branch/fork.  (see "servicing" document for policy around this)


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CToolsets.md)](https://helix.dot.net/f/p/5?p=Documentation%5CToolsets.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CToolsets.md)</sub>
<!-- End Generated Content-->
