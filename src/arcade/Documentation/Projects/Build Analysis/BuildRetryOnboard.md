# Build Retry

## How it works?
![](./Resources/BuildRetryWorkflow.png?raw=true)

The build retry feature is composed of two parts.

The first one is having the build configuration file as part of the build artifacts. In order to have this in place it is necessary to follow the [onboarding process](#how-to-onboard).

The second part is what is done by the Build Result Analysis. When the build finishes, it starts getting processed. If it failed, the build is analyzed to see if it can be retried. In order to be retried, at least one of the rules specified in a build configuration file needs to be met. 
## How to onboard?
1. In order to use the Build Retry feature it's necessary to have the `.NET Helix` GitHub application installed in the repo in which you intend to use the feature. </br>
If you want to get the application installed, you can contact the [.NET Core Engineering Services team](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/107/How-to-get-a-hold-of-Engineering-Servicing)


1. Create the [build configuration file](#Build-configuration-file-structure). The name of the file should be: `build-configuration.json` and you can create the file inside any folder. That folder is the one that you are going to publish. </br>
Ex. \eng\BuildConfiguration\build-configuration.json
1. [Publish an artifact having the `build-configuration.json` file to Azure Pipeline](https://docs.microsoft.com/en-us/azure/devops/pipelines/artifacts/pipeline-artifacts) in the repo that you are expecting to get retry. The name of the artifact should be: `BuildConfiguration`

	Ex.
	``` 
	- publish: $(Build.SourcesDirectory)\eng\BuildConfiguration
	  artifact: BuildConfiguration
	``` 

## Build configuration file structure
```json 
{
   "RetryCountLimit":1,
   "RetryByAnyError":false,
   "RetryByErrors":[
      {
         "ErrorRegex":"Regex"
      }
   ],
   "RetryByPipeline":{
      "RetryJobs":[
         {
            "JobName":"JobName"
         }
      ],
      "RetryStages":[
         {
            "StageName":"StageName"
         }
    ],
    "RetryJobsInStage":[
      {
        "StageName":"StageName",
        "JobsNames":["JobName"]
      }
    ]
  },
  "RetryByErrorsInPipeline":{
    "ErrorInPipelineByStage":[
      {
        "StageName":"StageName",
        "ErrorRegex":"Regex"
      }
    ],
    "ErrorInPipelineByJobs":[
      {
        "JobsNames":["JobName"],
        "ErrorRegex":"Regex"
      }
    ],
    "ErrorInPipelineByJobsInStage":[
      {
        "StageName":"StageName",
        "JobsNames":["JobName"],
        "ErrorRegex":"Regex"
      }
      ]
   }
}
```

- **RetryCountLimit:** Number of retries that can be done on your build. <br/> 
Ex. "RetryCountLimit": 1 means is going to be retried 1 time by the Build Result Analysis giving a total of 2 executions. <br> 
Default value: 0. Max internal value: 10.

- **RetryByAnyError:** The build is going to be retried independently of which was the reason of the failure. <br> 
Default value: False. 

- **RetryByErrors:** List of 'ErrorRegex' (.NET flavor) that will look for a match on the build pipeline errors. In case there is an error matching the regex the build is going to be retried.

	Ex.<br/>
	In order to retry a build with the following pipeline errors
	![](./Resources/PipelineErrorsExample.png?raw=true)
	you could have a file with the following information:
	```json 
    {
       "RetryCountLimit":1,
       "RetryByErrors":[
          {
             "ErrorRegex":"Vstest failed with error.*"
          }
       ]
    }
	```

- **RetryByPipeline:** The retry by pipeline is expecting a list of Jobs or/and Stages names which in case on fail should cause a retry by the Build Result Analysis.

	The job names reference are the ones on the pipeline:<br/>
	Ex. 
	![](./Resources/JobNameErrorsExample.png?raw=true)
	```json 
    {
       "RetryCountLimit":1,
       "RetryByPipeline":{
          "RetryJobs":[
             {
                "JobName":"Validate_Test_Failing"
             }
          ]
       }
    }
	```
	The stage name references are the ones on the yaml </br>
	Ex.
	![](./Resources/StageNameExample.png?raw=true)
	```json
    {
       "RetryCountLimit":1,
       "RetryByPipeline":{
          "RetryStages":[
             {
                "StageName":"StageName"
             }
          ]
        }
    }
	```
     If you want to retry jobs under a specific stage, you can do that by defining the stage name and then the jobs that you want to retry that are under that stage. You will need to define each stage separately. 
   ```json
   {
      "RetryCountLimit":1,
      "RetryByPipeline":{
         "RetryJobsInStage":[
            {
               "StageName":"StageName",
               "JobsNames":[ "JobNameA", "JobNameB" ]
            }
         ]
      }
   }
   ```
- **RetryByErrorsInPipeline:** This is a combination of retry by error and retry by pipeline, in which you will be able to retry a build base on errors found in specific places on the pipeline. <br>  
This gives you a more granular control on which error do you want that get retried.<br>  
For example, imagine that you have an error: "Vstest failed with error."  that most of the time happens in StageA, when it happens on StageA and you retry the build it usually finishes successfully. Unfortunately, the same error could happen on StageB and when that happens it is unusual. So, if you want the build only gets retried when the error happens in StageA you can define that: 

   ```json
   {
      "RetryCountLimit":1,
      "RetryByErrorsInPipeline":{
         "ErrorInPipelineByStage":[
            {
            "StageName":"StageA",
            "ErrorRegex":"Vstest failed with error.*"
            }
         ]
      }
   }
   ```

   This same principle can be applied to errors under specific job names: 

   ```json
  {
    "RetryCountLimit":1,
    "RetryByErrorsInPipeline":{
      "ErrorInPipelineByJobs":[
        {
          "JobsNames":["JobNameA","JobNameB"],
          "ErrorRegex":"Regex"
        }
      ]
    }
  }
   ```

   and errors for Jobs under a specific Stage:
   ```json
  {
    "RetryCountLimit":1,
    "RetryByErrorsInPipeline":{
      "ErrorInPipelineByJobsInStage":[
        {
          "StageName":"StageName",
          "JobsNames":["JobNameA", "JobNameB"],
          "ErrorRegex":"Regex"
             }
          ]
        }
    }
	```


<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CBuildRetryOnboard.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CBuildRetryOnboard.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProjects%5CBuild%20Analysis%5CBuildRetryOnboard.md)</sub>
<!-- End Generated Content-->
