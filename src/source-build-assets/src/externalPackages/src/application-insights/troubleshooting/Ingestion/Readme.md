# Troubleshooting Ingestion

## Fiddler

A tool such as Fiddler can be used to inspect raw HTTPS data from an app integrated with SDK to Ingestion Service.

## Networking

If the SDK is unable to send telemetry to the Ingestion Service, you may be experiencing a networking issue.

Please review our guides on [IP Addresses used by Azure Monitor](https://docs.microsoft.com/azure/azure-monitor/app/ip-addresses)

You can test your network by manually sending telemetry using the PowerShell script [PostTelemetry.ps1](PostTelemetry.ps1).
