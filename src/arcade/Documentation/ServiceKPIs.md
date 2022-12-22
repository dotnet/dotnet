# Service KPIs and Metrics

## Mission

Contribute to the successful release of .NET Core 3.0 by improving developer productivity through efficient build/test iterations.

## New Service Guidelines

Health telemetry is a fundamental monitoring tool and should be considered early in the design of a service.

Telemetry storage must be enterprise grade, highly available, and redundant. The data must be made available to reporting services. Application Insights is a preferred channel with easy integration for Azure services. Other storage solutions, like Azure Data Explorer and SQL Server, may also be used as appropriate for the particular needs of a service.

As general guidance, consider:

- Telemetry related to the _value_ the service provides to its users, example:

  - Number of events or triggers
  - Queries of a particular endpoint
  - Count of completed tasks

- Telemetry related to the _health_ of the service, example:

  - Successful and failed operations
  - Unknown events or triggers
  - Malformed payloads
  - Recoverable and unrecoverable errors

Document the service's telemetry. Include for each metric:

- Technical detail on _how_ the data is collected (to help provide context)
- _Why_ the data is collected.
- What is _good_ and _bad_ for the data? How will a monitor determine when to act?
- What _action_ should be taken if telemetry indicates "bad"?

Work with Leadership to identify any additional telemetry for current business efforts.

## First Responder

As GitHub issues is the primary means of work management for DNCEng, it is expected that all First Responder efforts of note be captured as a GitHub issue and added to the "First Responder" Epic. Telemetry and workload data is collected through this channel.

Customers may initiate communication with the First Responder team through other means, such as the [First Responder Teams channel](https://teams.microsoft.com/l/channel/19%3aafba3d1545dd45d7b79f34c1821f6055%40thread.skype/First%2520Responders?groupId=4d73664c-9f2f-450d-82a5-c2f02756606d&tenantId=72f988bf-86f1-41af-91ab-2d7cd011db47), [email alias](mailto:dnceng@microsoft.com) and direct communication. Substantive communication should be moved to GitHub to allow telemetery gathering, improve workflow clarity, and increase information sharing.

Open a new issue for any customer support effort taking more than fifteen minutes.

Ensure issues are labeled appropriately.

These telemetry guidelines are meant to extend but not supersede operational guidelines set in [First Responder Responsibilities](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/163/-int-First-Responders).

### Metrics

Name: Count of issues opened by members outside of DNCEng

Goal: Understand FR workload balance

Action:
- Significant external customer load indicates issue in documentation or service quality
- Significant internal customer load indicate technical debt


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CServiceKPIs.md)](https://helix.dot.net/f/p/5?p=Documentation%5CServiceKPIs.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CServiceKPIs.md)</sub>
<!-- End Generated Content-->
