# Overview
The requirement for Platform/OS onboarding is that it should be relatively painless to add/update/delete Queues/Scalesets and doable by all customers. 

# Requirements
- All Exisiting queue/scaleset combos are described in one or more yaml files in a VSTS/Github Repo which looks something like this.
```
- &Windows10
  Name: Windows.10
  AzureImage:
    Name: Windows
    Version: 10
  Artifacts:
    - vs_15_08
    - helix_runtime
  MaximumScale: 50
  Owner: abc@
  
- <<: *Windows10
  Name: Windows.10.Open
  Public: true
  MaximumScale: 20
  Owner: abc@

- Name: Windows.7.Amd64
  Public: true
  MaximumScale: 20
  location: westus
  tags: {
        "QueueId": "Windows.7.Amd64",
        "ResourceGroupName": "Windows.7.Amd64.WestUS",
        "WorkspacePath": "D:\\\\j",
        "IsAvailable": "true",
        "IsInternalOnly": "true",
        "UserList": "all",
        "OperatingSystemGroup": "windows"
      }
  scaling rule: <JSON>
  Owner: abc@
  
- Name: Windows.7.Client
  BaseImage: http://dotnet-eng-images.storage.azure.net/base-images/Windows.7.Client/15.6.750.vhd
  Artifacts:
    - vs_15_08
    - helix_runtime
  MaximumScale: 3000
  Owner: abc@
 
- Name: OSX.1012
  Unmanaged: true
  Owner: abc@
  
- Name: TOF.External
  Unmonitored: true
  Owner: abc@
```
- User makes a PR to the repo to add/edit/delete scalesets in one of the yaml files. 
- Validation Service in the Repo runs sanity checks on like “Does that image exist” and “Are those artifacts known artifacts”, if possible. 
- If Validation succeeds, send a PR to Image Creation Factory with specific parameters (TBD) to create Image /Artifcacts .
- Wait for the image to be created, Image Factory notifies via a webhook with a status ("image complete"/"failed to create an image" etc.)
- If the image is created, validate that the image works by creating a scaleset and deploying to INT/staging with one machine and test with a sample job. If artifacts are requested, quick validation per artifact to make sure the artifact is operating as expected (e.g. if someone wanted VS, make sure a “msbuild test.proj” does the right stuff, if helix is request, make sure it reads a queued item and processes a job)
- If anything failed, mark the PR as failed
- Maintain an image mapping yaml, which contains a mapping against commit# and the Image/Artifact created by the Image Creation Factory.
- When the User-initated PR or PR to update image mapping yaml merges, initiate CI build/release that
     -	Creates any required queue
     -	Transforms every defined scaleset/queue combo into an Azure ARM template and pushes all those, which will update any existing scale set, and create new ones as necessary. This might involve fetching some temporary secrets to initialize things
     -	Delete any scale/set queue that is defined but not in this repository anymore (so we can decommission things)
- To handle updating existing scale sets (some VMs running “newer” images than others), we need to augment the VM cleanup tool to detect when there are “older” VM’s in the scale set that contains newer ones and mark them as “unhealthy” so the get deleted, and newer images take their place 
- Cleanup service runs periodically every n days to clean up outdated VMs/scalesets, update Image Mapping yaml accordingly. 
- Add Helix as an Artifact to ImageFactory

# Dev Work Flow
![](./Images/DevWorkFlow.JPG?raw=true)



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5COS%20Onboarding%5CRequirements.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5COS%20Onboarding%5CRequirements.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5COS%20Onboarding%5CRequirements.md)</sub>
<!-- End Generated Content-->
