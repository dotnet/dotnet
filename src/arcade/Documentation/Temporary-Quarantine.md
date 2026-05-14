# Temporary Quarantine Procedure
In order to allow .NET development to proceed, problems in the build or test that cannot be resolved immediately need a mechanism
to quarantine them in order to unblock the developer workflow of the many developers that contribute to the product.

Quarantine is not the first resort, but it is a tool in order to ensure successful building of the product.

In general if a build or test fails, the steps should be as follows.
1. If the source PR/change can be identified, it should be backed out to restore correct behavior, and then the correction made in a future PR to reinstate the new code
2. If the problem can be definitively fixed quickly, it should be done as soon as possible
3. **If the broken component can be isolated to be removed from PR's and CI builds, it should be quarantined**
4. If none of the above are possible, is should be priority 0 to tackle the situation to get the build unblocked as soon as possible

Step 3 is the focus of this proposal.

## When is something "broken" : \<70% pass rate
We are going to consider something "broken" and in need of remediation **if it has failed 3 of the last 10 builds in the CI pipeline**.
The CI pipeline should be passing 100% of the time, so 3 fails indicates that something needs to be done to unblock PRs.

The quarantine option is meant to be used for issues that are believed to be short term disruptions.  If the fix cannot be determined immediately,
within 5 minutes of the failure, quarantine needs to be enacted to unblock PR workflows.
Permanent unreliability is a different problem not addressed by this procedure.

## What happens when something is quarantined
PR builds will not include the quarantined component.
The primary CI pipelines (e.g. the 'runtime' pipeline) will not include the quarantined component.
A separate pipeline will be run on the same cadence as the CI pipeline in order to execute quarantined components in order to determine when
it is appropriate to unquarantine the affected component.

Owners should be aware every quarantined item, with a tracking issue in the most appropriate repository assigned to them.
The primary purpose of this ownership is to ensure that the quarantined item is being addressed and tracked for reintroduction into the mainline builds.

## How to quarantine
The smallest unit possible should be quarantined, to minimize the coverage gap in PR.

1. A single test in a single configuration
1. A single test in all configurations
1. A test assembly
1. A build "job"
1. An entire pipeline

### A single test in a single configuration
TBD (https://github.com/dotnet/arcade/issues/6661)

### A single test in all configurations
TBD (https://github.com/dotnet/arcade/issues/6661)

### A test assembly
TBD (https://github.com/dotnet/arcade/issues/6662)

### A build job
TBD (https://github.com/dotnet/arcade/issues/6663)

### An entire pipeline
TBD (https://github.com/dotnet/arcade/issues/6663)

## How to reintroduce : Run without failure for 30 days
Once a fix has been introduced, and that component has passed passed for a month, it can be reintroduced into the mainline build by reverting the change
made to quarantine it.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTemporary-Quarantine.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTemporary-Quarantine.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTemporary-Quarantine.md)</sub>
<!-- End Generated Content-->
