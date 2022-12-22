# Issue tracking

The vstest project tracks issues and feature requests using the [issue template](../.github/ISSUE_TEMPLATE.md) for the vstest repository.

## Submitting an Issue

First, please do a search in [issues](https://github.com/Microsoft/vstest/issues) to see if the issue or feature request has already been filed. Use this [query](https://github.com/Microsoft/vstest/issues?q=is%3Aopen+is%3Aissue+sort%3Areactions-%2B1-desc) to search for the most popular feature requests.
If you find your issue already exists, make relevant comments and add your [reaction](https://github.com/blog/2119-add-reactions-to-pull-requests-issues-and-comments). Use a reaction in place of a "+1" comment.

- 👍 - upvote
- 👎 - downvote

If you cannot find an existing issue that describes your issue, submit it using the [issue template](../.github/ISSUE_TEMPLATE.md). Remember to follow the instruction mentioned therein carefully.

## Issue triage

Please follow the guidelines in the issue template when filing an issue or a pull request.
New issues or pull requests submitted by the community are triaged by a team member using the inbox query below.

## Inbox query

The [inbox query](https://github.com/Microsoft/vstest/issues?utf8=%E2%9C%93&q=is%3Aopen%20no%3Aassignee%20-label%3Abacklog%20-label%3Aenhancement) will return the following:

- Open issues or pull requests that are not enhancements and that have no owner assigned.

## Initial triage - Issue tagging

Issues will then be tagged as follows:

- Is the issue ***invalid***? It will be closed, and the reason will be provided.
- Is the issue a ***general question***, like *are data collector events synchronous*? It will be tagged as a ***question***.
- Is the issue a feature request or an enhancement. It will be tagged as an ***enhancement***.
- Else, the issue will be tagged as a ***bug***.

## Secondary triage – assignment and follow through

As and when an issue get assigned to a team member, the following secondary triage will happen

- When a team member picks up an issue, they will first assign it to themselves.
- Ensure that the issue has an appropriate tag (***question***, ***enhancement***, ***bug***).
- If an issue needs a repro, tag it with ***need-repro*** and ask for a repro in a comment.

## Ongoing issue management

- Issues tagged ***need-repro*** info will be closed if no additional information is provided for 7 days.

Team members will strive to resolve every bug within a stipulated period of time – we would like that to be a period of 14 days from the date the issue was filed.

# Community contributions - up-for-grabs

We strongly encourage the community to contribute enhancements and bug-fixes. Therefore, team members will look to add as much information in the issues as required so that you can make an effective contribution. Such issues will be tagged ***up-for-grabs***.

# Planning

## Triage

Bugs and enhancements will be assigned a milestone, and within a milestone they will be assigned a priority. The priority dictates the order in which issues should be addressed. A important bug (something that we think is critical for the milestone) is to be addressed before the other bugs.

To find out when a bug fix will be available in an update, then please check the milestone that is assigned to the issue.
Please see Issue Tracking for a description of the different workflows we are using.

## Milestone planning

We typically plan for a quarter and establish a set of themes we want to work towards, and prioritize enhancements and bug-fixes accordingly.
During the planning process we prioritize items as follows:

- Important ***bugs*** - crashes, regressions, and issues that do not have a reasonable workaround.
- ***Enhancements*** that have many reactions.

Accordingly, we publish the backlog and update the roadmap.
We will work on the items in sprintly (3 week) iterations. At the end of each iteration, we want to have a version of vstest that can be used by the community.
