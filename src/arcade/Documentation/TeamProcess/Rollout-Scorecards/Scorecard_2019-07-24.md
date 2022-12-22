# 24 July 2019 Rollout Summaries

## Helix

|              Metric              |   Value  |  Target |  Score |
|:--------------------------------:|:--------:|:-------:|:------:|
| Time to Rollout                  | 05:14:06 | 0:30:00 |   19   |
| Critical/blocking issues created |     9    |    0    |   9    |
| Hotfixes                         |     5    |    0    |   25   |
| Rollbacks                        |     1    |    0    |   10   |
| Service downtime                 |  0:00:00 | 0:00:00 |   0    |
| Failed to rollout                |   FALSE  |  FALSE  |   0    |
| **Total**                        |          |         | **63** |

## OS Onboarding

|              Metric              |   Value  |  Target |  Score  |
|:--------------------------------:|:--------:|:-------:|:-------:|
| Time to Rollout                  | 22:47:26 | 1:00:00 |    88   |
| Critical/blocking issues created |     4    |    0    |    4    |
| Hotfixes                         |     4    |    0    |    20   |
| Rollbacks                        |     0    |    0    |    0    |
| Service downtime                 |  0:00:00 | 0:00:00 |    0    |
| Failed to rollout                |   FALSE  |  FALSE  |   0    |
| **Total**                        |          |         | **112** |

## Arcade Services

|              Metric              |   Value  |  Target |  Score |
|:--------------------------------:|:--------:|:-------:|:------:|
| Time to Rollout                  | 03:11:00 | 1:00:00 |    9   |
| Critical/blocking issues created |     2    |    0    |    2   |
| Hotfixes                         |     2    |    0    |   10   |
| Rollbacks                        |     0    |    0    |    0   |
| Service downtime                 |     0    | 0:00:00 |    0   |
| Failed to rollout                |   FALSE  |  FALSE  |   0    |
| **Total**                        |          |         | **21** |

# Breakdowns

## Helix

| Metric | [2019072401.01](https://dev.azure.com/mseng/Tools/_releaseProgress?_a=release-pipeline-progress&releaseId=2672) | [2019072403.01](https://dev.azure.com/mseng/Tools/_releaseProgress?_a=release-pipeline-progress&releaseId=2673) | [2019072404.01](https://dev.azure.com/mseng/Tools/_releaseProgress?_a=release-pipeline-progress&releaseId=2675) | [2019072501.01](https://dev.azure.com/mseng/Tools/_releaseProgress?_a=release-pipeline-progress&releaseId=2676) | [2019072505.01](https://dev.azure.com/mseng/Tools/_releaseProgress?_a=release-pipeline-progress&releaseId=2678) | [2019072506.01](https://dev.azure.com/mseng/Tools/_releaseProgress?_a=release-pipeline-progress&releaseId=2679) | Total |
|:--------------------------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:-------------:|:--------:|
| Time to Rollout | 2:07:08 | 0:28:10 | 0:28:16 | 0:32:02 | 0:37:00 | 1:01:30 | 05:14:06 |
| Critical/blocking issues resolved | 1<br/>SQL column | 1<br/>[c20e1561](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/c20e156129da8169c7d255bffd21ef142e254dae?refName=refs%2Fheads%2Fmaster) | 1<br/>[d0726c2f](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/d0726c2ff82299da276d0d874c5de9afde909eaf?refName=refs%2Fheads%2Fmaster) | 1<br/>[#7124](https://github.com/dotnet/core-eng/issues/7124)/[54f92c93](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/54f92c9389ee7bd0d31989f4594648e61b496fa9?refName=refs%2Fheads%2Fmaster) | 4<br/>[072d7af1](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/072d7af1be25f2a14c6f87b96e55b60df20d3134?refName=refs%2Fheads%2Fmaster)/[e5a768b8](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/e5a768b85ee03a0876ff54461373f4d0b9f72ca3?refName=refs%2Fheads%2Fmaster)<br/>[82a3f7d2](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/82a3f7d2f8b478c18fe0b9d14967fce35ce5baa5?refName=refs%2Fheads%2Fmaster)<br/>[c1fece1b](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/c1fece1b3af0aedc7772cbf78631efdebcbd9d39?refName=refs%2Fheads%2Fmaster)<br/>[dddb7d19](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/dddb7d19cddc0398e2d1a228b2a6a598a12a525a?refName=refs%2Fheads%2Fmaster) | 1<br/>[82b0cfba](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/82b0cfba1a618fc70a3f8f2c35604c6efa972b53?refName=refs%2Fheads%2Fmaster) | 9 |
| Hotfixes | 0 | 1 | 1 | 1 | 1 | 1 | 5 |
| Rollbacks | 0<br/>&nbsp; | 0<br/>&nbsp; | 0<br/>&nbsp; | 0<br/>&nbsp; | 1<br/>[072d7af1](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/072d7af1be25f2a14c6f87b96e55b60df20d3134?refName=refs%2Fheads%2Fmaster)/[e5a768b8](https://dev.azure.com/mseng/Tools/_git/CoreFX%20Engineering%20Infrastructure/commit/e5a768b85ee03a0876ff54461373f4d0b9f72ca3?refName=refs%2Fheads%2Fmaster) | 0<br/>&nbsp; | 1<br/>&nbsp; |
| Service downtime | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 |

## OS Onboarding

| Metric | [2019072401](https://dev.azure.com/dnceng/internal/_build/results?buildId=277460) | [2019072402](https://dev.azure.com/dnceng/internal/_build/results?buildId=277925) | [2019072403](https://dev.azure.com/dnceng/internal/_build/results?buildId=278090) | [2019072404](https://dev.azure.com/dnceng/internal/_build/results?buildId=278216) | [2019072501](https://dev.azure.com/dnceng/internal/_build/results?buildId=279688) | [2019072601](https://dev.azure.com/dnceng/internal/_build/results?buildId=281352) | [2019072602](https://dev.azure.com/dnceng/internal/_build/results?buildId=281850) | Total |
|:--------------------------------:|:----------:|:----------:|:----------:|:----------:|:----------:|:----------:|:----------:|----------|
| Time to Rollout | 06:00:17 | 00:43:44 | 01:23:51 | 03:40:23 | 03:03:58 | 04:56:34 | 02:58:39 | 22:47:26 |
| Critical/blocking issues resolved | 0<br/><br/>&nbsp; | 1<br/>[#7116](https://github.com/dotnet/core-eng/issues/7116)<br/>&nbsp; | 0<br/><br/>&nbsp; | 1<br/>[d3f86f9f](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-machines/commit/d3f86f9faf934a479eefc218e362b0b32c95dc1b?refName=refs%2Fheads%2Fdumps_documetation)<br/>&nbsp; | 1<br/>[7ee45aa7](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-machines/commit/7ee45aa757941a7f0e07b84924f22f9171b2831f?refName=refs%2Fheads%2Fdumps_documetation)<br/>&nbsp; | 2<br/>[2bda61c6](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-machines/commit/2bda61c6da08b3fd95aece392ccbe867d487b89b?refName=refs%2Fheads%2Fdumps_documetation)<br/>[72bc2a73](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-machines/commit/72bc2a7391a3ca2e7f5ac40da39283b743ba16d4?refName=refs%2Fheads%2Fdumps_documetation) | 0<br/><br/>&nbsp; | 5<br/><br/>&nbsp; |
| Hotfixes | 0 | 1 | 1 | 1 | 1 | 1 | 0 | 5 |
| Rollbacks | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Service downtime | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 |

## Arcade Services

| Metric | [20190724.3-1](https://dev.azure.com/dnceng/internal/_releaseProgress?_a=release-pipeline-progress&releaseId=12364) | [20190725.3-1](https://dev.azure.com/dnceng/internal/_releaseProgress?_a=release-pipeline-progress&releaseId=12431) | [20190725.4-1](https://dev.azure.com/dnceng/internal/_releaseProgress?_a=release-pipeline-progress&releaseId=12455) |Total|
|:--------------:|:--------------:|:--------------:|:--------------:|:--:|
| Time to Rollout | 1:00:00 | 1:02:00 | 1:09:00 | 03:11:00 |
| Critical/blocking issues created | 0<br/>&nbsp; | 1<br/>[#547](https://github.com/dotnet/arcade-services/pull/547) | 1<br/>[#548](https://github.com/dotnet/arcade-services/pull/548) | 2 |
| Hotfixes | 0 | 1 | 1 | 2 |
| Rollbacks | 0 | 0 | 0 | 0 |
| Service downtime | 0:00:00 | 0:00:00 | 0:00:00 | 0:00:00 |


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2019-07-24.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2019-07-24.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5CRollout-Scorecards%5CScorecard_2019-07-24.md)</sub>
<!-- End Generated Content-->
