# Stateless Pools - A Cost Analysis

## Purpose
In order to determine the costs of switching from stateful pools to stateless ones, the following study was conducted.
Switching to stateless pools will resolve bug [dotnet/core-eng#14683](https://github.com/dotnet/core-eng/issues/14683), which
involves machines running out of disk space due to multiple runs on the same machine. 1ES has informed us that the working
directories on stateful machines will not be cleaned between runs and that we should switch to stateless pools in order to
avoid this problem.

# Time Cost

## Methodology
Three pipelines were examined as part of this study: runtime (ID 686), runtime-dev-innerloop (ID 700), and runtime-staging (ID 924).
These particular pipelines were picked because they could all be triggered via a single runtime PR. 
To determine a baseline for the stateful 1ES pools, runs between 8 Oct 2021 and 22 Oct 2021 in the TimelineBuilds table in Kusto were queried.
Specifically, this this study only concerned itself with runs which met the following criteria:
* _Successful_: in order to make sure builds were as similar as possible, all failing runs were excluded from the results
* _Main_: once again for the purpose of consistency, only runs targeting the `main` branch were used
* _Inlier_: builds which took longer than five hours to complete were excluded from the dataset in interest of eliminating outliers

To acquire the data for the stateless pool, 10 runs of these pipelines were triggered manually targeting the 
NetCore-Public-Int pool, which was manually set to be stateless and capable of scaling out to 300 machines.
The sample size for these runs is necessarily much smaller than the baseline due to time constraints. The runs were
triggered on 22 Oct 2021 and 25 Oct 2021. 

BuildIds of stateless runs are as follows:
* runtime: `(1436605, 1436874, 1437134, 1439516, 1439517, 1439738, 1439739, 1439773, 1439774, 1439775)`
* runtime-dev-innerloop: `(1436606, 1436862, 1437004, 1437087, 1439512, 1439618, 1439703, 1439745, 1439776, 1439777)`
* runtime-staging: `(1436608, 1436875, 1437086, 1439514, 1439545, 1439779, 1439781, 1439782, 1439783, 1439784)`

### Caveats and Limitations

A few notable differences exist between our two samples:
* The sample size differs massively. A large sample size can be obtained for the baseline and it was, but the smaller sample 
size for the delta inherently makes conclusions from that data less reliable.
* The baseline data, while having a large sample size, contains runs over many different commits. While they are likely 
still similar enough to be useful for analysis, this differs from the delta data which contains runs over only a single 
commit.
* While many different jobs were being run all at once during the delta data, this still does not reflect realistic usage
in prod.
* Not every run in the delta data was 100% successful -- while none presented a catastrophic failure, several had minor
failures due to test flakiness.
* This study still only reflects the usage of one (albeit very large) repo.

### Queries Used
To determine average & standard deviation:
```
TimelineBuilds
| where DefinitionId == [def] and QueueTime > datetime(10/8/2021) and QueueTime < datetime(10/22/2021) and Result == "succeeded" and TargetBranch  == "main"
| extend BuildTime =  StartTime - QueueTime
| where BuildTime < 5h
| distinct *
| summarize avg(BuildTime), totimespan(stdev(BuildTime))
```

To obtain count data bucketed in fifteen minute intervals:
```
TimelineBuilds
| where DefinitionId == [def] and QueueTime > datetime(10/8/2021) and QueueTime < datetime(10/22/2021) and Result == "succeeded" and TargetBranch  == "main"
| extend BuildTime =  StartTime - QueueTime
| where BuildTime < 5h
| distinct *
| summarize count() by bin(BuildTime, 15m)
| order by BuildTime asc 
```

To obtain percentile data:
```
TimelineBuilds
| where DefinitionId == [def] and QueueTime > datetime(10/8/2021) and QueueTime < datetime(10/22/2021) and Result == "succeeded" and TargetBranch  == "main"
| extend BuildTime =  StartTime - QueueTime
| where BuildTime < 5h
| distinct *
| summarize count() by bin(BuildTime, 15m)
| summarize percentilesw(BuildTime, count_, 50, 75, 95)
```

To obtain job start time:
```
TimelineBuilds
| where DefinitionId == [def] and QueueTime > datetime(10/8/2021) and QueueTime < datetime(10/22/2021) and Result == "succeeded" and TargetBranch  == "main"
| extend BuildTime =  FinishTime - QueueTime
| where BuildTime < 5h
| distinct *
| summarize arg_max(FinishTime, *) by BuildId
| join kind=inner (TimelineRecords
                    | where Order != 0
                    | where strlen( Path ) == 11
                    | where WorkerName has "NetCore1ESPool"
                    | summarize arg_max(FinishTime, *) by RecordId, BuildId ) on BuildId
| extend JobStartTime = StartTime1 - QueueTime
| summarize count(), min(JobStartTime), avg(JobStartTime), totimespan(stdev(JobStartTime))
```

To obtain job percentile data:
```
TimelineBuilds
| where DefinitionId == [def] and QueueTime > datetime(10/8/2021) and QueueTime < datetime(10/22/2021) and TargetBranch  == "main"
| extend BuildTime =  FinishTime - QueueTime
| where BuildTime < 5h
| summarize arg_max(FinishTime, *) by BuildId
| join kind=inner (TimelineRecords
                    | where Order != 0
                    | where strlen( Path ) == 11
                    | where WorkerName has "NetCore1ESPool"
                    | summarize arg_max(FinishTime, *) by RecordId, BuildId ) on BuildId
| distinct *
| extend JobStartTime = StartTime1 - QueueTime
| summarize count() by bin(JobStartTime, 1m)
| summarize percentilesw(JobStartTime, count_, 50, 75, 95)
```

## Results
The percentiles indicate the percentage of builds that finish in less than the given time (bucketed in 15 minute increments for build times
and one minute increments for job start times).

### runtime

#### Build Times
| **Metric**  | **Baseline** | **Delta** |
|-------------|--------------|-----------|
| Sample Size | 154          | 10        |
| Mean        | 1:52:48      | 2:42:32   |
| StDev       | 0:40:56      | 0:32:48   |
| 50th %ile   | 1:45:00      | 2:45:00   |
| 75th %ile   | 2:15:00      | 3:15:00   |
| 95th %ile   | 3:30:00      | 3:30:00   |

#### Job Start Times
| **Metric**  | **Baseline** | **Delta** |
|-------------|--------------|-----------|
| Sample Size | 11186        | 1122      |
| Minimum     | 0:02:17      | 0:03:33   |
| Mean        | 0:18:53      | 0:29:03   |
| StDev       | 0:15:12      | 0:20:01   |
| 50th %ile   | 0:14:00      | 0:17:00   |
| 75th %ile   | 0:31:00      | 0:50:00   |
| 95th %ile   | 0:41:00      | 1:00:00   |

### runtime-dev-innerloop

#### Build Times
| **Metric**  | **Baseline** | **Delta** |
|-------------|--------------|-----------|
| Sample Size | 552          | 10        |
| Mean        | 0:57:58      | 1:02:09   |
| StDev       | 0:09:11      | 0:04:32   |
| 50th %ile   | 1:00:00      | 1:15:00   |
| 75th %ile   | 1:15:00      | 1:15:00   |
| 95th %ile   | 1:15:00      | 1:15:00   |

#### Job Start Times
| **Metric**  | **Baseline** | **Delta** |
|-------------|--------------|-----------|
| Sample Size | 3865         | 70        |
| Minimum     | 0:00:19      | 0:03:15   |
| Mean        | 0:03:56      | 0:11:33   |
| StDev       | 0:04:14      | 0:03:39   |
| 50th %ile   | 0:03:00      | 0:12:00   |
| 75th %ile   | 0:06:00      | 0:14:00   |
| 95th %ile   | 0:11:00      | 0:18:00   |

### runtime-staging

#### Build Times
| **Metric**  | **Baseline** | **Delta** |
|-------------|--------------|-----------|
| Sample Size | 196          | 10        |
| Mean        | 1:17:38      | 3:15:24   |
| StDev       | 0:49:57      | 0:51:29   |
| 50th %ile   | 1:00:00      | 3:30:00   |
| 75th %ile   | 2:15:00      | 4:15:00   |
| 95th %ile   | 2:45:00      | 4:15:00   |

Unfortunately, runtime-staging might not be the most useful pipeline for drawing conclusions as the build time
distribution appears bimodal which means the mean build time is not as helpful a measurement.

#### Job Start Times
| **Metric**  | **Baseline** | **Delta** |
|-------------|--------------|-----------|
| Sample Size | 746          | 80        |
| Minimum     | 0:01:46      | 0:03:44   |
| Mean        | 0:12:48      | 0:20:43   |
| StDev       | 0:04:19      | 0:10:23   |
| 50th %ile   | 0:13:00      | 0:13:00   |
| 75th %ile   | 0:16:00      | 0:16:00   |
| 95th %ile   | 0:20:00      | 0:19:00   |

## Conclusions

While it is difficult to fully attribute one hundred percent of the build time increases to the stateless pools (especially with the delta
data having a larger standard deviation in the runtime job start data and both the runtime-staging data sets), there is still a marked
increase in both the build time and the job queue time for all observed pipelines.

Of particular note is the runtime-dev-innerloop pipeline, which has very tight data sets for both the baseline and the delta. With lower
standard deviations for both builds and jobs, we saw mean build time increase by five minutes and mean job start time increase by
**nearly eight minutes**. In worst-case scenario runs, such an increase would translate to very high increases in overall build time,
which may be what is reflected in the runtime and runtime-staging mean build time increases.

Given this data, it is the opinion of the author that stateless pools do not represent a reasonable alternative to stateful pools with
workspace cleanup.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cstateless-pools-cost-analysis.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cstateless-pools-cost-analysis.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cstateless-pools-cost-analysis.md)</sub>
<!-- End Generated Content-->
