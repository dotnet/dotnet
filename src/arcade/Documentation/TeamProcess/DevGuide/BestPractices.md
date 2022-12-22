# Best Practices for .NET Engineering Services 

The intent of this document is to help the team learn and grow by sharing best practices that we have found. 

## Keeping Our Repos Healthy and Ready for Roll Out
- Anyone who checks in a change still needs to monitor the next main run, in any repo.  
    -	PR validation is not the same as deployment to the staging environment and there will always be problems missed by PR validation unless we deploy an entire environment for every PR, which is not currently possible. 
    -	The goal is to have a vendor monitoring our important pipelines - [Helix Machine Lifecycle Daily Process](https://dnceng.visualstudio.com/internal/_wiki/wikis/DNCEng%20Services%20Wiki/952/Helix-Machine-Lifecycle-Processes?anchor=daily%3A) - but everyone on the team should still make sure we are able roll out at any time.
    -	It’s a good principle to ask people to look at the next main run, it’s an even better one to not allow oneself to be broken for days at a time unnecessarily.
- Verify deployment and close issues you placed in the "waiting for rollout" column on our Project Board
   - This is especially true for anything associated with grafana alerts. We may miss new alerts as they are concatinated to an existing issue.
   - It is not the responsiblity of the individuals performing deployments to verify your issue is complete and closed out.
- The autoscaler is quite different from everything else in dotnet-helix-machines
    - It is the only service within this repo and it causes us to duplicate any efforts involving Service Fabric changes

## First Responder/Operations Work 
- The Operations v-team - including our vendor resource - is responsible for triaging any internal work that come in from S360 and other internal notifications (i.e. emails from security, policy notifications from PM, etc). Any work that they determine as meeting the First Responder bar will be tagged for FR and be addressed by that virtual team. 
- Current First Responder responsibilities, best practices and how to documentation can be found at our [Team Wiki](https://dev.azure.com/dnceng/internal/_wiki/wikis/DNCEng%20Services%20Wiki/889/Home)



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CTeamProcess%5CDevGuide%5CBestPractices.md)](https://helix.dot.net/f/p/5?p=Documentation%5CTeamProcess%5CDevGuide%5CBestPractices.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CTeamProcess%5CDevGuide%5CBestPractices.md)</sub>
<!-- End Generated Content-->
