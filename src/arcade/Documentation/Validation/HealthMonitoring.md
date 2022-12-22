# Health Monitoring

## Features and Processes in Scope

Helix Services
- Helix Client
- Helix API
- Controller
- EventHub
- ServiceBus
- Data Migration Services

Arcade-Services:

- Maestro++
- Darc API
- BAR
- Telemetry Service
- BARViz

## Links to Relevant Pipelines and Builds

MSENG:

- [Helix-PR-Master](https://dev.azure.com/mseng/Tools/_pipeline/analytics/stageawareoutcome?definitionId=6216&contextType=build)
- [Helix-CI](https://dev.azure.com/mseng/Tools/_build?definitionId=6171&_a=summary&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
- [Helix-Daily](https://dev.azure.com/mseng/Tools/_build?definitionId=6843&_a=summary&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
- [Helix Agents - CI](https://dev.azure.com/mseng/Tools/_build?definitionId=6707&_a=summary&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)

DNCENG (internal):

- [Arcade-ci](https://dev.azure.com/dnceng/public/_build?definitionId=208&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
- [Arcade-extensions-ci](https://dev.azure.com/dnceng/public/_build?definitionId=386&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
- [Arcade-minimalci-sample-ci](https://dev.azure.com/dnceng/public/_build?definitionId=209&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
- [Arcade-pool-provider-ci](https://dev.azure.com/dnceng/public/_build?definitionId=411&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
- [Dotnet-arcade-service](https://dev.azure.com/dnceng/public/_build?definitionId=247&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
- [Arcade-validation-ci](https://dev.azure.com/dnceng/public/_build?definitionId=269&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
- Helix-machines
	- [Build-and-deploy-production](https://dev.azure.com/dnceng/internal/_build?definitionId=145&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
	- [Build-and-deploy-staging](https://dev.azure.com/dnceng/internal/_build?definitionId=103&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
	- [Pr](https://dev.azure.com/dnceng/internal/_build?definitionId=3&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)
	- [Pr-prod-queues](https://dev.azure.com/dnceng/internal/_build?definitionId=129&view=ms.vss-pipelineanalytics-web.new-build-definition-pipeline-analytics-view-cardmetrics)

## Existing Functionality and Processes
[Availability and reliability monitoring](https://msit.powerbi.com/groups/de8c4cb8-b06d-4af8-8609-3182bb4bdc7c/reports/09be7698-941b-4df7-966e-d0e1ca96d656/ReportSection) for PROD services already exists in Power BI from data in AppInsights and Kusto. Our focus for Health Reporting will be based on tests and builds in AzDO. 

## Assumptions
All the builds and deployments we care about are either in mseng or dnceng in AzDO

## Concerns
- There may be some "weirdness" with AzDO collecting test results when deploying Helix.
- SSL validation pre and post deployment enough or should we do periodic health checking?
- How to health report for Docker and OnPrem? 

## Use Cases and Solutions
- Build status widgets in AzDO Dashboard using data pulled from Pipelines
- Test Run status widgets in AzDO Dashboard using data pulled from Pipelines
- Deployment status widgets in AzDO Dashboard using data pulled from Pipelines

The above should give the picture needed to know if staging is stable to rollout out to Prod. Health Monitoring of already deployed services in PROD is available in PowerBI and is not covered part of this epic. 
Checks to ensure Service Fabric is reachable, up and running, will not fall over with deployment etc., are validated via tests that will get hooked up to pipelines. 

Sample AzDO Dashboard - https://dev.azure.com/dnceng/internal/_dashboards/dashboard/755b52e7-b7a3-423b-bb60-7a01ff7241b8

## Dependencies
- AzDO (Dashboard, widgets, pipelines)
- Azure


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CValidation%5CHealthMonitoring.md)](https://helix.dot.net/f/p/5?p=Documentation%5CValidation%5CHealthMonitoring.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CValidation%5CHealthMonitoring.md)</sub>
<!-- End Generated Content-->
