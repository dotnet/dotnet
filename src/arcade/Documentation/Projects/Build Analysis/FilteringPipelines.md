## Build Analysis: Filtering Pipelines
This feature filters Azure pipelines from Build Analysis. By default, all pipelines are analyzed. If you want to analyze only specific pipelines, you need to define them in a configuration file. The filtered pipelines are exclusive to target branches, so you need to specify which pipelines are analyzed for each target branch.

### Step by step getting on board.

1. Create a file with the name `build-analysis-configuration.json`  on the path `eng/`  of your repository

2.  Add the following code to your JSON file
```json
{
    "PipelinesToAnalyze":[
       {
          "PipelineId": <your pipeline id>,
          "PipelineName": "<your pipeline name>"
       }
    ]
}
```
3. Fill the pipeline id with your pipeline id. To locate your pipeline id, please follow these steps:
    - Navigate to your pipeline in Azure ([Pipelines](https://dev.azure.com/dnceng-public/public/_build)) 
    - Look at the URL and find the parameter `definitionId=<id>`
    - This is the main identifier for your pipeline and the one Build Analysis will use, so it’s important to have it right

    Example: https://dev.azure.com/dnceng-public/public/_build?definitionId=4, in that case the id is 4
4. Fill the `PipelineName` with your pipeline name. This is optional, but recommended for future reference.
5. Create the pull request to merge the file on your target branch. Feel free to tag @dnceng on your pull request if you need someone to review the pull request.
6. Repeat this process for every target branch in which you want to filter pipelines

After this has been done and the pull request has been merged, the pipeline will start to get filtered for builds of the pull requests targeting that target branch.


