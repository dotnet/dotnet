# Build Analysis

The Build Analysis offers a set of features to make build and pull request results more actionable. 

Explore all the features offered by build analysis:

- [Build Analysis Website](https://helix.dot.net/BuildAnalysis): Find the builds that are impacted by an error, that occurs either in tests or in timelines, across the dotnet organization, in the same way as Runfo does. You can also obtain the latest report for any known issue and a list of all open issues and their impact.
- [Build Analysis GitHub Check](Introduction.md): Makes GitHub pull request results more actionable by highlighting build and test information most helpful to .NET developers.
- [Known Issues](KnownIssues.md): Report known issues so these can be automatically identified and reported on failed builds.
- [Automatic Test Retries](Test%20Retry%20Documentation.md): Configure tests in your repo's pull requests to automatically retry when they encounter specific errors.
- [Automatic Build Retries](BuildRetryOnboard.md): Configure your repo's pull request builds to automatically retry when they encounter specific errors.
- [Test Reporting Queries](../AzureDevOps/TestReportingQueries.md):  We've collected your test results data into Kusto for ease of querying and put together a few queries to get started.
- [Build Analysis Pipelines Filtering](FilteringPipelines.md): This feature allows you to specify which pipelines are analyzed by Build Analysis for each target branch.
- [Escape Mechanism for Build Analysis](EscapeMechanismforBuildAnalysis.md): This feature allows users to override the build analysis conclusion and set it to a successful conclusion
