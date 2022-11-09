#  Increasing visibility into the time builds & Helix tests take, and Helix errors 

## Goal and Motivation

Our customers are dissatisfied with information about the status of the Helix queues in their given pipeline and the amount of time it takes waiting in queue to obtain a build machine. 

We currently have an awesome Grafana dashboard, but the discoverability is close to zero, and it doesn't clearly identify what is going on with the *overall* status of Helix. It does a great job of putting data into context (is this queue depth normal for this queue).

The data it provides is too much for a typical dev. A dev's main concerns are:

* How long will my build and tests take?
* This queue is taking really long, is everything okay?
* What's the overall status of Helix (outage, infrastructure issues, etc.)

The dashboard doesn't answer these questions directly, but they can be *inferred* from the dashboard.

We want a solution that puts this information right in front of devs.

We can leverage our existing `BuildFailureAnalysis` projects to add another GitHub check that can present clear insights into the status of Helix, directly into the GitHub PR they are working on.


### Stakeholders

* .NET Core Engineering Services (contact: @dnceng)
* Helix Customers (contact: @dncpartners)

### Implementation

**See [Design Mockup](IncreaseVisibilityHelixQueues/design-mockup.md) for a mockup of the overall, final design.**

Our main goal is to add a new check, titled `Helix Queue Insights` that will be included directly in the PR a dev is working on.

#### Subprojects

This a list of all the features this one-pager is for.

* Create a new check in the Github PR
* Show the dev a list of queues (separated by on and off prem)
* Show the top 5 queues with the highest work item wait time.
* Create "insights" into the queues of their pipelines. Tell the dev that queues are:
  1. **Extremely** high work item wait time. Is it likely this queue will timeout?
  2. **Heavier** than usual wait time. This queue wait isn't normal and it will take more time for this queue.
  3. **Lighter** than usual wait time. This test will run faster than normal! 
* Estimated times for:
  * Helix tests to complete
  * How long it will take for a build machine to be obtained
  * How long it will take for the entire pipeline to complete
* Tracking how accurate our estimates are (from the Estimated Times feature)
* A high level overview status of Helix
  * Are Helix, our on-prem and off-prem queues, etc. operating normally?
  * Red/Yellow/Green, similar to the [Azure DevOps Status](https://status.dev.azure.com/)
* Draw a detailed graph of how builds and tests flow to show the user.
  * For example, the roots are the different builds and the leafs are different Helix queues
  * This will show the dev the overall hierarchy of their pipeline and allow them to optimize their pipeline by submitting jobs early to busy pools or reducing queues they use (thanks Stu)

#### Projects in Scope

In the coming 10 weeks of the internship, I will limit my scope to the following projects:

* Create a new check in the Github PR
* Show the dev a list of queues (separated by on and off prem)
* Show the top 5 queues with the highest work item wait time.

The result I'm looking to achieve is [this mockup](IncreaseVisibilityHelixQueues/design-mockup-justin-impl.md).

**Stretch goals:**
* Estimated times for helix tests and getting an AzDO build machine
  * Machine Learning


### Technical Implementation Details

1. Be notified of when a new PR is created.
   1. Our existing code can already do this. Specifically, our `AnalysisProcessor` in our `BuildResultAnalysisProcess` microservice.
   2. This needs to be changed to add a new Checkrun, as the Helix Queue Insights will be its own checkrun to avoid running into the 65k character limit. In addition, the Build Analysis page gets overwritten when any of the pipelines in the repo completes. We also don't want to mix build results with the status of Helix queues.
   
2. Determine which queues the repo uses.
   1. We will use the Matrix of Truth for this data.
   2. Their data has is built from a job that Ilya mentioned at least updates once a week, and we can pull this information programmatically.

3. Query the work item wait time and queue size for that pipeline's list of queues.
   1. Currently Grafana has this data, with Kusto queries that we can pull and use.
   2. We will simply pull the queries that Grafana uses them to present the data.
      1. This creates a small issue of having the same query in two locations, the Grafana and this new feature. Stu and I discussed, and he mentioned that the queries haven't changed much (if at all) since the dashboard was created, so it should be okay.
      2. The other option is to query Grafana for the data (which we can do), but then this creates an unnecessary tight dependency on Grafana,

4. Process the data to compute moving average, and detect abnormalities.
   1. This is interleaved with the previous step, but we'll need to compute the moving average using a Kusto query.
   2. Compute the percent differences between the current work item wait time and the moving average.
  
5. Calculate the time for the entire pipeline to complete
   1. Build Analysis currently can determine what pipelines a PR will trigger
   2. We can compute the max time for the pipelines (which is how long the CI will take), and compute the 95th percentile over a certain time period.
   3. This will yield the time that the CI pipelines *usually* take.
   
6. Build a model for the Markdown template & create the markdown.
   1. We'll need to process the data from the Kusto queries into models that we can format the markdown handlebars template.
   2. We'll also need to generate links to the Grafana dashboard for each queue.
   3. Use the model to turn the handlebars template into markdown.
   4. *It's also possible to include screenshots of the Grafana graphs to embed in the markdown. This should be noted and can be explored later.*

7. Send the checkrun to GitHub.
   1. The markdown will not be refreshed whenever the user opens the page, instead it'll be a "one-shot" when the PR is submitted.

### Proof of Concept (POC)

See https://github.com/maestro-auth-test/helix-queue-insights-test/pull/11/checks?check_run_id=6801735696

### Risk

- Will the new implementation of any existing functionality cause breaking changes for existing consumers?
  - No. This is a new feature that will not cause any breaking changes for our customers.
- What are your assumptions?
  - The design mockup and the information it provides are beneficial to our customers and will allow them to get a high level overview of relevant information about the status of Helix.
- What are your unknowns?
  - The level of satisfaction this will bring to our customers.
  - The accuracy of our data. Queue behavior can wildly fluctuate between outages and large test runs. Ideally we should have a system to track the accuracy of our predictions to have the data for our customers and improve our estimates.
- What dependencies will this epic/feature(s) have?
  - Kusto
  - AzDo
  - Grafana
  - GitHub
  - Matrix of truth
- Is there a goal to have this work completed by, and what is the risk of not hitting that date? (e.g. missed OKRs, increased pain-points for consumers, functionality is required for the next product release, et cetera)
  - August 2022. I've limited my scope of features I will work on to be able to deliver complete features.
- Does anything the new feature depend on consume a limited/throttled API resource? 
  - While not throttled, the preview pipeline API can take ~10 seconds to return.
  - While we are only planning this check run to be a snapshot when the PR is created, GitHub gets grumpy when updating a checkrun numerous times.
- Have you estimated what maximum usage is? 
  - No, but this type of feature is already implemented, and this project will extend off that.
- Are you utilizing any response data that allows intelligent back-off from the service?
  - All API calls in this project are returned synchronously.
- What is the plan for getting more capacity if the feature both must exist and needs more capacity than available?
  - This feature shouldn't require more capacity than I can handle.
  - If so, we can reduce the scope of the project and prioritize key items we think would be beneficial to the customer.

### Usage Telemetry

- How are we measuring the “usefulness” to the stakeholders of the business objectives?
  - After the feature is implemented and initial rolled out, we can ask members of the CI Counsel if the feature is providing usefulness, and things we can change to make it more useful.
  - We will also use the sentiment tracker from [`Helix.Utility.UserSentiment`](https://dev.azure.com/dnceng/internal/_git/dotnet-helix-service?path=/src/Utility/Helix.Utility.UserSentiment) to gather information on whether the new information is helpful.
- How are we tracking the usage of this new feature?
  - The same way we track who has enabled .NET Build Analysis
  - There is currently no plan on tracking how many people have actually viewed this new checkrun.

## Service-ability of Feature

If the format of our database changes, we'll have to change the Kusto queries that this feature uses. In that event, our Grafana dashboard will also be broken.


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cincrease-visibility-helix-queues-arcade8824.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cincrease-visibility-helix-queues-arcade8824.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5COne-Pagers%5Cincrease-visibility-helix-queues-arcade8824.md)</sub>
<!-- End Generated Content-->
