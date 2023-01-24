# Autoscaler - Debugging 

The autoscaler is running on all Helix subscriptions and HelixStaging . <br>
All the logs, traces, and exceptions of the autoscaler live on Application Insights.

If you want to find any error or log about the running service, for production navigate to [dotnet-eng](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/68672ab8-de0c-40f1-8d1b-ffb20bd62c0f/resourceGroups/dotnet-eng-cluster/providers/microsoft.insights/components/dotnet-eng/logs) and for staging navigate to [dotnet-eng-int](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/cab65fc3-d077-467d-931f-3932eabf36d3/resourceGroups/dotnet-eng-int-cluster/providers/Microsoft.Insights/components/dotnet-eng-int/logs). You can use this [dotnet-eng/dotnet-eng-int guide](#logs-in-dotnet-engdotnet-eng-int) to navigate the information.

If you want to see metrics or more data that is sent by the autoscaler for production navigate to [helix-autoscale-prod](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/68672ab8-de0c-40f1-8d1b-ffb20bd62c0f/resourceGroups/helix-autoscale-prod/providers/microsoft.insights/components/helix-autoscale-prod/logs) and for staging navigate to [helix-autoscale-int](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/cab65fc3-d077-467d-931f-3932eabf36d3/resourceGroups/auto-scale-int/providers/microsoft.insights/components/helix-autoscale-int/logs) for staging. You can use this [helix-autoscale-prod/helix-autoscale-int guide](#data-in-helix-autoscale-inthelix-autoscale-prod) to know which information you can find in there. 

## **Alerts**
The main alerts related to the autoscaler are: <br>

* [Helix AutoScaler Service Stopped Running](#Helix-AutoScaler-Service-Stopped-Running) <br>
* [Cores consumption](#Cores-consumption)
* [Autoscale: Minutes to scale-up from zero machine](#autoscale-minutes-to-scale-up-from-zero-machine)


### **Helix AutoScaler Service Stopped Running**
[Helix AutoScaler Service Stopped Running Grafana](https://dotnet-eng-grafana.westus2.cloudapp.azure.com/d/arcadeAvailability/service-availability?orgId=1&refresh=30s&from=1606937931219&to=1606959531219&panelId=60&fullscreen) <br>
Example: https://github.com/dotnet/core-eng/issues/11478

Step by step:

1. [Restart autoscaler node](#How-to-restart-the-autoscaler) as soon as you can - A lot of issues come after the autoscaler stops running such as the alert [Autoscale: Minutes to scale-up from zero machine](#Autoscale:-Minutes-to-scale-up-from-zero-machine).  
2. Find when the autoscaler stopped running (hour)
3. See if there is any exception that was thrown on that period of time (on [dotnet-eng/dotnet-eng-int](#logs-in-dotnet-engdotnet-eng-int)). This will help you to know what happened. 
4. If there is not an exception, you can see the traces and analyze the last trace that was recorded to have an idea in which step the autoscaler stopped running. 
5. At this point the auto scaler should be running and you should be able to find the issue and fix it.


### **Cores consumption**
[Cores consumption Grafana](https://dotnet-eng-grafana.westus2.cloudapp.azure.com/d/quota/azure-quota-limits?orgId=1&refresh=30s&from=1606937685479&to=1606959285479&var-Resource=cores&var-Resource=standardDv3Family&panelId=30&fullscreen) <br>
Example: https://github.com/dotnet/core-eng/issues/11542

The autoscaler is in charge of enforcing the core consumption while trying to make all the queues meet the SLA, when this alert gets trigger, we should answer the following questions:

1. Are we over-scaling? This means we have more machines than we need. Grafana [queue monitor](https://dotnet-eng-grafana.westus2.cloudapp.azure.com/d/queues/queue-monitor?orgId=1) could be an awesome way to review this. 
2. Are we having problems scaling down? We should check if there are a lot of machines offline and they are not being deleted. (You can look at the [heartbeats table](#Heartbeats-table))
3. Are a lot of machines not working and not getting clean? (The autoscaler always try to have machines heart beating so if there are a lot of machines dying the autoscaler is going to try to replace them which can lead to something similar of over scaling, for this you should check the scale sets)

### **Autoscale: Minutes to scale-up from zero machine**
[Autoscale: Minutes to scale-up from zero machine Grafana](https://dotnet-eng-grafana.westus2.cloudapp.azure.com/d/queues/queue-monitor?orgId=1&from=1606937898053&to=1606959498053&var-QueueName=buildpool.windows.10.amd64.open&var-QueueName=buildpool.windows.10.amd64.vs2017&var-QueueName=buildpool.windows.10.amd64.vs2017.open&var-QueueName=windows.10.amd64.open&var-UntrackedQueues=%22osx%22,%20%22perf%22,%20%22arm%22,%20%22arcade%22,%20%22xaml%22,%20%22appcompat%22&panelId=99&fullscreen) <br>

This alert can get triggered by a bunch of reasons but in some cases, it can be because of the autoscaler. 

There are two scenarios in which the alert normally gets triggered: 
* We are having problems scaling up. 
* The scale set has machines, but the machines are not heartbeating.

#### *Scenario: Scaling up problems*
The scale up problems can come for the autoscaler or the scale set.

1. Check if the alert 'Helix AutoScaler Service Stopped Running' is not active, if this alert is active most likely this is the reason, and you should focus on getting the autoscaler running. 
2. Review the scale set and see if there are machines being created, if this is the case you know that soon there are going to be machines. Even in this case I suggest you take a quick look to see if the problem is not related to [machines not heartbeating](#scenario-the-scale-set-has-machines-but-the-machines-are-not-heartbeating).
3. If there are no machines getting created 
	* Check the `scaling up` traces for the queue in which the alert got trigger on [dotnet-eng/dotnet-eng-int](#logs-in-dotnet-engdotnet-eng-int). If there are not logs you should check the [machines not heartbeating scenario](#scenario-the-scale-set-has-machines-but-the-machines-are-not-heartbeating). 
	* Pick the more recent scaling up trace and go to the scale set and see if the scale up instruction was received and which is the status (Started, Accepted or Succeeded) to know if it is a delay on Azure.

#### *Scenario: The scale set has machines, but the machines are not heartbeating*
The autoscaler is designed to replace the machines that are not heartbeating but this can be triggered if:
* We are out of cores.
* All the machines are offline, and we are not deleting the machines.
* The machines are not starting.
* We are taking too long to replace the machines.

1. Review the scale set and the heartbeats table:
	* If all the machines are offline, we are having problems scaling down. Start for checking if the autoscaler puts those machines offline by reviewing the "OfflineReason", the autoscaler use "Scaling Down" as OfflineReason.  
	* If a machine is on the scale set but is not appearing on the heartbeat table review if the machine ever appears (This information is available on HeartbeatExport table on Kusto) if it appears check the last status. Tha information can give you an idea if it has never started up or if it dies while doing a job. 
2. The system is designed to always reserve cores for the queues so all the queues should be able to have at least one machine so if this happens major changes need to be made to the rebalanced cores logic. 

## **Logs in dotnet-eng/dotnet-eng-int**

There are two tables that have information about the autoscaler `traces` and `exceptions`.

The autoscaler is not the only service that logs data on that App Insights, so you need to filter the information, a straightforward way to filter this is using the cloud_RoleName, the identifier for the autoscaler is 'fabric:/CustomAutoScale', and inside that group there are subgroups:
 
* fabric:/CustomAutoScale/ProcessAutoScaleService - This is the one in charge of managing the actors.
* fabric:/CustomAutoScale/AutoScaleActorService - This is the actor itself.
* fabric:/CustomAutoScale/ProcessTelemetryService - This reports information about the telemetry that we are sending, in most cases you can exclude this one, unless you are investigating something telemetry specific.

When looking for an error starting with the exceptions can give information faster, in most cases, you are going to need information about ProcessAutoScaleService and AutoScaleActorService having a query like the following one:

	exceptions
	| where cloud_RoleName == "fabric:/Helix/ProcessAutoScaleService"
	or cloud_RoleName == "fabric:/Helix/AutoScaleActorService"
	| sort by timestamp desc

Remember to always use `Time range` to limit the information as much as you can.

If you need more information you can always use the traces to get more data.

If the problem is specific to a queue you should start by filtering the message with the queue, this could be done on the message or as part of a customDimensions:
	
	| where message contains "windows.10.amd64.open.rt"
	
	or

	| where customDimensions.queue == "windows.10.amd64.open.rt"
	  or  customDimensions.queueName == "windows.10.amd64.open.rt"
	

If it is related to a specific problem, there are a couple of keywords that we normally use to filter the data, and this can be filtered as part of the message:

* Scaling (or scaling up / scaling down)
* Deleting 
* Machines online
* MaxCapacity (this is related to the cores assigned for queue)

For example:

	traces
	| where cloud_RoleName == "fabric:/CustomAutoScale/ProcessAutoScaleService"
	or cloud_RoleName == "fabric:/CustomAutoScale/AutoScaleActorService"
	| where customDimensions.queue == "windows.10.amd64.open.rt"
	| where message contains "scaling down"
	| sort by timestamp desc


## **Data in helix-autoscale-int/helix-autoscale-prod**
If you want more information about how the autoscaler is behaving, you can look at helix-autoscale-int/helix-autoscale-prod Application Insight in which information is recorded on  `custom events ` table with the following names:

* AdjustCapacity: Every 30 seconds reports all the variables that are important for the autoscaler to decide how many machines need, this are active messages, current capacity, desired capacity, machine creation time, max capacity, work items per machine and SLA.
* AutoScaleReport: Every 30 seconds reports the current capacity (machines heartbeating) and queue depth.
* QueueReport: Every minute reports queue depth and state of the heartbeating machines (initializing, offline, online, busy). This data can be analyzed on Grafana on [Queue Monitor Production](https://dotnet-eng-grafana.westus2.cloudapp.azure.com/d/queues/queue-monitor) or [Queue Monitor Staging](https://dotnet-eng-grafana-staging.westus2.cloudapp.azure.com/d/queues/queue-monitor).
* QueueCapacityChanged: When the capacity of a queue changes send data about current capacity, previous capacity, and future capacity.

## **Additional Info**

### How to restart the autoscaler
1. For production navigate to [Service Fabric Explorer dotnet-eng](https://dotnet-eng.westus2.cloudapp.azure.com:19080/Explorer/index.html#/) and for staging navigate to [Service Fabric Explorer dotnet-eng-int](https://dotnet-eng-int.westus2.cloudapp.azure.com:19080/Explorer/index.html#/). <br> (If you have problems accessing this site, remember that you need a [certificate](#get-the-certificate-to-access-autoscaler-cluster))
2. Find in which node is running ProcessAutoScaleService:
	* Open the following tabs: CustomAutoScaleType -> fabric:/CustomAutoScale -> Service fabric:/CustomAutoScale/ProcessAutoScaleService.
	* Open one more tab and you are going to be able to see the name of the node, is going to look like this: _Primary_3.
2. Open the Nodes tab and find the node name that you got on the previous step.
3. Go to Actions button and click on Restart.

### Get the certificate to access autoscaler cluster
1. Navigate to [HelixProdKV | Certificates](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/68672ab8-de0c-40f1-8d1b-ffb20bd62c0f/resourceGroups/helixinfrarg/providers/Microsoft.KeyVault/vaults/HelixProdKV/certificates) / [HelixStagingKV | Certificates](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/cab65fc3-d077-467d-931f-3932eabf36d3/resourceGroups/helixstagingkvrg/providers/Microsoft.KeyVault/vaults/HelixStagingKV/certificates)
3. Select `dotnet-eng-client-westus2-cloudapp-azure-com` certificate.
4. Open the current version.
5. Download in PFX/PEM format:
	- Leave password empty
	- Certificate Store select Place all certificates in the following store > Browse > Personal

### Heartbeats table
To access the heartbeats table using Azure Portal, follow these steps:
1. Navigate to [helixscripts2](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/68672ab8-de0c-40f1-8d1b-ffb20bd62c0f/resourceGroups/helixinfrarg/providers/Microsoft.Storage/storageAccounts/helixscripts2/storageexplorer)/[helixstagescripts2](https://ms.portal.azure.com/#@microsoft.onmicrosoft.com/resource/subscriptions/cab65fc3-d077-467d-931f-3932eabf36d3/resourceGroups/helixstaginginfrarg/providers/Microsoft.Storage/storageAccounts/helixstagescripts2/storageexplorer)
2. Open the Tables tab.
3. Click on heartbeats.

To access the heartbeats table using Microsoft Azure Storage Explorer, follow these steps:
1. Open Microsoft Azure Storage Explorer.
2. Navigate to Helix/HelixStaging subscription.
3. Find the storage account helixscripts2 or helixstagescripts2. 
4. Open the Tables tab.
5. Click on heartbeats.

<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CAutoScaler%5CAutoScalerInvestigateIssues.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CAutoScaler%5CAutoScalerInvestigateIssues.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CAutoScaler%5CAutoScalerInvestigateIssues.md)</sub>
<!-- End Generated Content-->
