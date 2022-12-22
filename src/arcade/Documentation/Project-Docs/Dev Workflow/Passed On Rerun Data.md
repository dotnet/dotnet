# Test passed on rerun data

Analyzing tests that passed on rerun at a granular level as is a build probably sometimes doesn't feel enough, or probably you want to see all the tests that passed on rerun in a quick look.

For the cases in which you want to see all the tests that passed on rerun in your build or repo or look for the most common failures on the test that passed after a rerun, and many other scenarios, you can do the following:

1. Query the PassedOnRerun data using the [AzureDevOpsTests](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata?query=.show%20table%20AzureDevOpsTests) table, located in: [Engsrvprod/engineeringdata](https://dataexplorer.azure.com/clusters/engsrvprod/databases/engineeringdata) <br>


    ```
    AzureDevOpsTests
    | where Outcome == "PassedOnRerun"
    | where BuildId == [buildId]
    | where Repository == [repository]
    ```

1. Use [Build Analysis Reporting](https://msit.powerbi.com/links/crjYD5rwh0?ctid=72f988bf-86f1-41af-91ab-2d7cd011db47&pbi_source=linkShare) under `Test Passed on Rerun` tab to see aggregated data.   


## Example of queries 

1. Query by build Id: <br>
To get the buildId of your build you can navigate to your Azure DevOps build pipeline, see the URL, it should look like this: <br>
`https://dev.azure.com/dnceng/public/_build/results?buildId=1234567&view=results` <br>
Look for the `buildId=`  in this case is `1234567` and use that id in your query. <br>
Ex. 
    ```
    AzureDevOpsTests
    | where Outcome == "PassedOnRerun"
    | where BuildId == 1234567
    ```

2. Query by repository<sup>*</sup>: <br>
In this example you can see all the assemblies in the last week for a repository, ordered by passed on rerun count
    ```
    AzureDevOpsTests
    | where RunCompleted > ago(7d)
    | where Outcome == "PassedOnRerun"
    | where Repository == "dotnet/runtime" 
    | summarize count() by WorkItemFriendlyName
    | order by count_ desc
    ```
    <sub>* Examples of respositories: 'dotnet/roslyn', 'dotnet/runtime', 'dotnet/aspnetcore', 'dotnet/installer'</sub>
    

3. Query tests by assembly: <br>
In this example you can see all the tests in the last week for an assembly, ordered by passed on rerun count <br>
    ```
    AzureDevOpsTests
    | where RunCompleted > ago(7d)
    | where Outcome == "PassedOnRerun"
    | where WorkItemFriendlyName == "System.Net.Http.Functional.Tests"
    | summarize count(), any(Arguments) by TestName, ArgumentHash
    | order by count_ desc
    ```


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CPassed%20On%20Rerun%20Data.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CPassed%20On%20Rerun%20Data.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5CDev%20Workflow%5CPassed%20On%20Rerun%20Data.md)</sub>
<!-- End Generated Content-->
