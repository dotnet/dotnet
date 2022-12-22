# Rollout Scorer Proposal

This is a proposal for the Rollout Scorer which will assist in generating rollout scorecards. Given a few inputs, it will scrape AzDO (and later, telemetry sources) to calculate a score and then generate a markdown file in a PR to core-eng and upload the data to Kusto.

## Tool Description
The Rollout Scorer will be a command-line tool. The arguments it will accept are as follows:

|            Argument            |  Required?   |              Description              |
|:------------------------------:|:------------:|:--------------------------------------|
|       `--repo` or `-r`         | **Required** | The repository to score               |
|      `--branch` or `-b`        |  *Optional*  | The branch of the repo to score(e.g. servicing or prod); defaults to production |
| `--rollout-start-date` or `-s` | **Required** | The date on which the rollout started |
|  `--rollout-end-date` or `-e`  |  *Optional*  | The date on which the rollout ended; defaults to current date |
|    `--number-of-rollbacks`     |  *Optional*  | The number of rollbacks which occurred as part of the rollout; defaults to 0 |
|    `--downtime` or `-d`        |  *Optional*  | Specifies an amount of downtime which occurred |
|     `--failed` or `-f`         |  *Optional*  | Indicates a failed rollout (50 points) |
|     `--output` or `-o`         |  *Optional*  | File which the generated csv will be outputted to; defaults to `./scorecard.csv` |
|       `--skip-output`          |  *Optional*  | Skips the output step and directly uploads results |
|     `--upload` or `-u`         |  *Optional*  | Replaces all other parameters; uploads csv file to Kusto and makes PR in core-eng |

The flow for using the Rollout Scorer is as follows:
* Run `RolloutScorer.exe` and specify the repo, rollout start date, and any optional parameters
* The Rollout Scorer will scrape AzDO for the appropriate data and create a CSV file containing the scorecard data
* User can make manual corrections to the CSV file as necessary
* Run `RolloutScorer.exe --upload {csv}` and the Rollout Scorer will upload the CSV file to Kusto and AzDO

As shown in the parameters table, the user can optionally choose to skip the manual CSV adjustment stage.

## Score Calculation
The Rollout Scorer will reference an INI file which will contain a map from repository name to the URI of the AzDO release or build definition. It will scrape this definition for all of the builds (targeting the production branch) or releases that occurred within the specified timeframe. From this data it will calculate:

* **Total rollout time** &mdash; The sum of all build/release times
* **Number of critical issues** &mdash; Calculated from the number of commits in each hotfix release/build
* **Number of hotfixes** &mdash; Calculated from number of release/builds after the first one
* **Number of rollbacks** &mdash; Manually specified by the user
* **Downtime** &mdash; Manually specified by the user, but eventually will be calculated from telemetry
* **Failure to rollout** &mdash; Manually specified by the user 


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CRollout_Scorer_Proposal.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CRollout_Scorer_Proposal.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CRollout_Scorer_Proposal.md)</sub>
<!-- End Generated Content-->
