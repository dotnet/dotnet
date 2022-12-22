# 29 July 2020 Rollout Summaries

## arcade-services

|              Metric              |   Value  |  Target  |   Score   |
|:--------------------------------:|:--------:|:--------:|:---------:|
| Time to Rollout                  | 01:09:13 | 00:30:00 |     3     |
| Critical/blocking issues created |     0    |    0     |     0     |
| Hotfixes                         |     0    |    0     |     0     |
| Rollbacks                        |     0    |    0     |     0     |
| Service downtime                 | 00:00:00 | 00:00:00 |     0     |
| Failed to rollout                |   FALSE  |   FALSE  |     0     |
| Total                            |          |          |   **3**   |


## dotnet-helix-machines

|              Metric              |   Value  |  Target  |   Score   |
|:--------------------------------:|:--------:|:--------:|:---------:|
| Time to Rollout                  | 01:06:25 | 01:00:00 |     1     |
| Critical/blocking issues created |     0    |    0     |     0     |
| Hotfixes                         |     0    |    0     |     0     |
| Rollbacks                        |     0    |    0     |     0     |
| Service downtime                 | 00:00:00 | 00:00:00 |     0     |
| Failed to rollout                |   FALSE  |   FALSE  |     0     |
| Total                            |          |          |   **1**   |


## dotnet-helix-service

|              Metric              |   Value  |  Target  |   Score   |
|:--------------------------------:|:--------:|:--------:|:---------:|
| Time to Rollout                  | 02:09:45 | 00:30:00 |     7     |
| Critical/blocking issues created |     2    |    0     |     2     |
| Hotfixes                         |     0    |    0     |     0     |
| Rollbacks                        |     1    |    0     |     10     |
| Service downtime                 | 00:00:00 | 00:00:00 |     0     |
| Failed to rollout                |   TRUE  |   FALSE  |     50     |
| Total                            |          |          |   **69**   |

Relevant GitHub issues: [#10348](https://github.com/dotnet/core-eng/issues/10348), [#10349](https://github.com/dotnet/core-eng/issues/10349)
# Itemized Scorecard

## arcade-services

| Metric | [20200729.1](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=749990) |
|:-----:|:-----:|
| Time to Rollout | 01:09:13 |
| Critical/blocking issues created | 0 |
| Hotfixes | 0 |
| Rollbacks | 0 |
| Service downtime | 00:00:00 |


## dotnet-helix-machines

| Metric | [20200729.03](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=749610) |
|:-----:|:-----:|
| Time to Rollout | 01:06:25 |
| Critical/blocking issues created | 0 |
| Hotfixes | 0 |
| Rollbacks | 0 |
| Service downtime | 00:00:00 |


## dotnet-helix-service

| Metric | [2020072903](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=749988) | [2020072908](https://dev.azure.com/dnceng/7ea9116e-9fac-403d-b258-b31fcf1bb293/_build/results?buildId=750570) |
|:-----:|:-----:|:-----:|
| Time to Rollout | 01:13:11 | 00:56:34 |
| Critical/blocking issues created | 2 | 0 |
| Hotfixes | 0 | 0 |
| Rollbacks | 0 | 1 |
| Service downtime | 00:00:00 | 00:00:00 |



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2020-07-29.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2020-07-29.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2020-07-29.md)</sub>
<!-- End Generated Content-->
