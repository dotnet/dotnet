# Rollout Score Card

Each rollout will be scored accordingly. The mechanics are similar to golf scoring, in which the higher the number, the worse the score. The goal is a score of 0. Each item has a threshold associated with it and a score associated for each infraction. The "threshold" is the allowable amount for each item, and any amount over the allowable will incur penalties. 

The score card allows us to count up infractions against our rollouts. A successful rollout should be completed in the time allowed (i.e. threshold), and should not require any sort of intervention, like a hotfix or rollback. Also, we should not receive any customer complaints based on the rollout. A rollout like that would give us a score of 0, which is our goal. We should be able to drill down into the score to see why the penalties were incurred and improve our rollout process (e.g. adding validation, testing, et cetera).

1. Time taken to rollout (Every 15 minutes over [rounded up] is worth 1 point). This value is calculated based on the total time to build and deploy the service (explicitly: the sum of the build pipeline's elapsed time and the release pipeline's elapsed time). End-to-end tests run after the service has been deployed are not added to this value. (Reason for this is that the faster our builds and deployments are, the faster we can make changes and hotfixes, if necessary). 

   A. OS Onboarding Threshold: 1 hour

   B. Helix Threshold: 30 minutes

   C. Arcade-Services Threshold: 1 hour

2. Number of critical/blocking issues as a result of the rollout (1 point per issue). Threshold is 0. 

3. Service downtime (availability and reliability) as a result of the rollout (1 point for every minute of downtime). Threshold is 0. 

4. Number of hotfixes (5 points per hotfix). Threshold is 0. Also, the time it takes to roll this out is cumulatively added to the initial rollout time (see #1 above). 

5. Number of rollbacks (10 points per rollback). Threshold is 0. Also, the time it takes to roll this out is cumulatively added to the initial rollout time (see #1 above). 

6. Failure to rollout (50 points). This is a scenario in which we have made a commitment to our customers (e.g. posted it in release notes), and did not meet the window to rollout. This includes partial rollouts (i.e. rollouts in which some parts of the deployment failed and were not remedied).

Example: A rollout of OS Onboarding took 6 hours (20 points), Helix took less than 30 minutes (0 points), and Arcade-Services took 1 hour (0 points). A critical issue occurred in Helix (1 point), which resulted in the need for a hotfix (5 points) and another rollout of Helix, that took less than 30 minutes (0 points). During that time, the component in Helix that was broken caused a service downtime of an hour (60 points). This score of this rollout would have been 86 points. 

## Dashboard

A Power BI dashboard of all rollout scorecard metrics so far can be found [here](https://msit.powerbi.com/groups/de8c4cb8-b06d-4af8-8609-3182bb4bdc7c/reports/6d2bd5cd-f96f-40df-af3f-33fd4cf1d82d). 

## Pre-Requisites

In order for this to work, we'll need the entire team to be on board with this process: 

* Every hotfix needs to be noted as a hotfix for us to measure those metrics. Same with rollbacks. 
* Issues (critical or not) that are a result of a rollout need to be marked as such with labels.

The full guidelines for these are noted in the [Policy Document](/Documentation/Policy/DeploymentPolicy.md).

## Automation

Rollout Scorecards are automatically created, submitted, and logged by the [Rollout Scorer](https://github.com/dotnet/arcade-services/tree/master/src/RolloutScorer/Readme.md).

## July 24, 2019 Rollout (Example)

The July 2019 rollout served as the basis for this scorecard. Its scorecard can be found [here](Scorecard_2019-07-24.md).


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CREADME.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CREADME.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CREADME.md)</sub>
<!-- End Generated Content-->
