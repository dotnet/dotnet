# Troubleshooting

## For Immediate Support 

For immediate support relating to the Application Insights .NET SDK we encourage you to file an [Azure Support Request](https://docs.microsoft.com/azure/azure-portal/supportability/how-to-create-azure-support-request) with Microsoft Azure instead of filing a GitHub Issue in this repository. 
You can do so by going online to the [Azure portal](https://portal.azure.com/) and submitting a support request. Access to subscription management and billing support is included with your Microsoft Azure subscription, and technical support is provided through one of the [Azure Support Plans](https://azure.microsoft.com/support/plans/). For step-by-step guidance for the Azure portal, see [How to create an Azure support request](https://docs.microsoft.com/azure/azure-portal/supportability/how-to-create-azure-support-request). Alternatively, you can create and manage your support tickets programmatically using the [Azure Support ticket REST API](https://docs.microsoft.com/rest/api/support/).

## SDK Internal Logs

The Application Insights .NET SDK uses ETW to expose internal exceptions.

To collect these logs, please review our full guide on [ETW](ETW).

## Networking Issues

The Application Insights .NET SDK has no knowledge of the environment it's deployed in.
The SDK will send telemetry to the configured endpoint. 

If you suspect networking issues, please review our guide on [Troubleshooting Ingestion](Ingestion).

## No Data

Please review our full guide on [Troubleshooting no data](https://docs.microsoft.com/azure/azure-monitor/app/asp-net-troubleshoot-no-data)
