# 29 September 2021 Rollout Summaries

## dotnet-helix-machines

|              Metric              |   Value  |  Target  |   Score   |
|:--------------------------------:|:--------:|:--------:|:---------:|
| Time to Rollout                  | 03:08:45 | 02:30:00 |     3     |
| Critical/blocking issues created |     1    |    0     |     1     |
| Hotfixes                         |     1    |    0     |     5     |
| Rollbacks                        |     0    |    0     |     0     |
| Service downtime                 | 00:00:00 | 00:00:00 |     0     |
| Failed to rollout                |   FALSE  |   FALSE  |     0     |
| Total                            |          |          |   **9**   |

Relevant GitHub issues: [#14541](https://github.com/dotnet/core-eng/issues/14541)
## dotnet-helix-service

|              Metric              |   Value  |  Target  |   Score   |
|:--------------------------------:|:--------:|:--------:|:---------:|
| Time to Rollout                  | 02:34:51 | 01:30:00 |     5     |
| Critical/blocking issues created |     1    |    0     |     1     |
| Hotfixes                         |     0    |    0     |     0     |
| Rollbacks                        |     1    |    0     |     10     |
| Service downtime                 | 00:00:00 | 00:00:00 |     0     |
| Failed to rollout                |   TRUE  |   FALSE  |     50     |
| Total                            |          |          |   **66**   |

Relevant GitHub issues: [#14552](https://github.com/dotnet/core-eng/issues/14552)
## arcade-services

|              Metric              |   Value  |  Target  |   Score   |
|:--------------------------------:|:--------:|:--------:|:---------:|
| Time to Rollout                  | 02:13:16 | 01:30:00 |     3     |
| Critical/blocking issues created |     0    |    0     |     0     |
| Hotfixes                         |     0    |    0     |     0     |
| Rollbacks                        |     0    |    0     |     0     |
| Service downtime                 | 00:00:00 | 00:00:00 |     0     |
| Failed to rollout                |   FALSE  |   FALSE  |     0     |
| Total                            |          |          |   **3**   |


# Itemized Scorecard

## dotnet-helix-machines

| Metric | [20210929.01](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=1392836) | [20210929.02](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=1393736) |
|:-----:|:-----:|:-----:|
| Time to Rollout | 02:28:44 | 00:40:01 |
| Critical/blocking issues created | 1 | 0 |
| Hotfixes | 0 | 1 |
| Rollbacks | 0 | 0 |
| Service downtime | 00:00:00 | 00:00:00 |


## dotnet-helix-service

| Metric | [2021092901](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=1393285) | [2021093003](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=1396217) | [2021093004](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=1396293) |
|:-----:|:-----:|:-----:|:-----:|
| Time to Rollout | 01:03:23 | 00:27:28 | 01:04:00 |
| Critical/blocking issues created | 1 | 0 | 0 |
| Hotfixes | 0 | 0 | 0 |
| Rollbacks | 0 | 0 | 1 |
| Service downtime | 00:00:00 | 00:00:00 | 00:00:00 |


## arcade-services

| Metric | [20210929.1](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=1393292) | [20210929.2](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=1393447) |
|:-----:|:-----:|:-----:|
| Time to Rollout | 01:07:38 | 01:05:38 |
| Critical/blocking issues created | 0 | 0 |
| Hotfixes | 0 | 0 |
| Rollbacks | 0 | 0 |
| Service downtime | 00:00:00 | 00:00:00 |



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2021-09-29.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2021-09-29.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2021-09-29.md)</sub>
<!-- End Generated Content-->
