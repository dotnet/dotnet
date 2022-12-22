# Background #
Several of the .NET Core repositories utilize Docker images within their official build definitions for the supported Linux distros.  Utilizing Docker allows a single Linux configuration to be used on all of the build agents yet the product can be built on all of the supported distros.  Using Docker also allows the product teams to easily manage the build prereqs because they specified within the Dockerfiles and don't require VSO service engineers in order to change them.

The following is a summary of the prescribed process currently being used for adding/updating the Docker images.

1. Add a new or update the existing [DockerFiles](https://devdiv.visualstudio.com/DevDiv/_git/DotNetCore?path=%2Fdockerfiles&version=GBmaster&_a=contents) as necessary.
2. Build the new/modified Dockerfiles locally.  Tag the image using the following naming schema `<DistroName&Version>_prereqs_v<ImageVersion>` (e.g. `ubuntu1610_prereqs_v3`)
3. Verify the new/modified image works as expected.
4. Get someone with the appropriate access to push the new/modified image to [Docker Hub](https://hub.docker.com/r/chcosta/dotnetcore/).
5. Update the appropriate build pipelines to reference the new Docker images (e.g. [CoreFx build pipeline](https://github.com/dotnet/corefx/blob/94780d59037393369d22def54466b2e13d81c435/buildpipeline/pipeline.json))

# Problems #

## Out of Date or Missing Dockerfiles ##
Some of the images we are currently using within our build definitions do not have the corresponding Dockerfiles they were generated from checked into SCC.  

1. Some images were created by running a base OS image and then manipulating it as necessary in order to add the required tools and dependencies.  The resulting images were then captured using the [docker commit](https://docs.docker.com/engine/reference/commandline/commit/) command.  
2. Some images were created via Dockerfiles that were never checked into SCC.
3. Some Dockerfiles were updated to produce newer versions of images but the updated Dockerfiles were never checked into SCC.

Because we do not have the Dockerfiles for the images we are building with today, we don't really know with certainty what dependencies and toolsets we are building the product with.  You can gather this information by inspecting the images we are building with but it is a time intensive process and prone to oversight.  More importantly it is possible for the images on Docker Hub to be accidentally replaced or deleted.  If this were to happen we would be in a very bad situation and would have to scramble to get the Docker images recreated.

## Docker Image Versioning ##
Nothing in the process used today enforces versioning of the Docker images.  As described earlier, it is up to the individuals that have access to the Docker Hub repository to increment the Docker image version whenever an update is made.  Hopefully this is being followed but it is susceptible to human judgement and error.

If this process is not being followed, it is possible that updating an image without incrementing the version (e.g. changing the tag) would break the various builds that utilize the shared images.  This issue may not surface itself immediately. For example it may only get surfaced at the time a previous release is serviced.  Tracking down issues like this can be very time consuming and wasteful.

## Docker Image Traceability ##
There is no way to trace back from a Docker image to the Dockerfile it was generated with.  For example, suppose we release version 1.0 of a product that was built with version 4 of a particular Docker image.  A couple months pass and a service patch is needed for the release.  For this particular service fix, a new version of a tool is needed.  How do we correlate a Docker image back to the specific version of the Dockerfile it was generated from in order to update the required tool?  There isn't a way to correlate a Docker image tag/version to a specific Dockerfile without manually inspecting what is installed on the image in relation to the file history of the Dockerfile.

Another problem that has surfaced with our Dockerfiles is that they do not reference a static OS base image.  The base OS images do get revised overtime with service patches.  This has causes issues such as [dotnet/core-setup#1149](https://github.com/dotnet/core-setup/pull/1149).  We should be explicitly making these changes and verifying the changes prior to rolling them into production.

## Docker Hub Repository ##
Currently the Docker images being used are stored in Chris Costa's personal [Docker Hub repository](https://hub.docker.com/r/chcosta/dotnetcore/).  Relying on personally owned artifacts is not a good practice to use.  Chris could leave the team, company or worse, which may lead to issues in administering these artifacts.

## Docker Toolset ##
The Docker toolset being used by the builds is not captured anywhere.  Docker may introduce breaking changes or behavior changes over time that could be detrimental if introduced onto the build agents.  Docker has been known to do this in the past (e.g. [Regression in LTTng behavior on Docker 1.10.2](https://github.com/docker/docker/issues/20818)).   When the product is built, it should be using an explicit version of the Docker toolset so that we can ensure repeatability and reliability.

# Proposed Changes #

## Automated Builds ##
Introducing automated builds would a great way to ensure we will always have the source Dockerfiles for the Docker images we use.  The only way to update a Docker image would be to update the corresponding Dockerfile.  When a Dockerfile change is merged in, a build would get kicked off automatically that would build the Docker image and upload it to Docker Hub.  The build definition would use a service account to upload images to Docker Hub.  This service account would be the only account that would have access to upload images.  Therefore the only way to update the Docker images would be to make a change to the checked-in Dockerfiles.

VSTF build definitions would provide the necessary functionality and flexibility to meet our requirements.  Key vault should be utilized to store the credentials so that they are stored in a centralized place for servicing.  If needed in a pinch, the credentials could be used manually to upload an image.  

## Tagging Scheme ##
In order to provide traceability between a Docker image tag and the Dockerfile it was produced with, we should utilize a tagging scheme similar to the following:

`<OSName>.<OSVersion>-<ImageVariant>-<Timestamp>-<CommitSha>`

**Examples**

- `opensuse.42.1-20170118-c760fcc`
- `Ubuntu.14.04-crossbuild-20161210-b04b497` 

The automated builds would be capable of generating the tag from the Dockerfile location in SCC and the commit that triggered the build.  A timestamp is suggested in addition to a commit sha simply as a means to quickly tell how old an image is and compare two tags in order to tell which one is older.

## Docker Repository ##
A new Docker repository should be used for our Docker images that is owned by an organization (e.g. `microsoft`).  This will ensure that the dotnet organization will always have control over the repository as team members come and go.  If we continue to use Docker Hub, we don't want a repository that could distract users from the official .NET Docker repository (e.g. `microsoft/dotnet`).  Something like `microsoft/dotnet-buildtools-prereqs` could do that. If we wanted to obfuscate it more in order to avoid having the general public find it when looking for the real dotnet images, we could name it `microsoft/dnbpr` which would stand for `dotnet build prereqs`.  This becomes a little unnatural.  Couple this naming issue with the desire we have to be able to build the product without taking a dependency on non-MSFT services, we should use a private Docker registry.  Azure currently has beta support for a [container registry](https://azure.microsoft.com/en-us/services/container-registry/) which would meet our needs.

Using a custom Docker registry is pretty easy. You don't use the Azure CLI, you still use the Docker CLI. The differences are that first you must explicitly login to the private registry (`docker login dotnetcore-microsoft.azurecr.io -u *** -p ***`). Second you must include the registry name in all of the image requests such as run, pull, etc. (`docker pull dotnetcore-microsoft.azurecr.io/build_prereqs:ubuntu.14.04`)

## Reference Base Images via Digest ##
In order to solve the issues with the base images changing overtime due to service fixes, we should be [referencing the base images via digest](https://docs.docker.com/engine/reference/builder/#/from).  This change makes the Dockerfile less readable by itself therefore it is recommended that we add a comment that clearly states what the base image is and the date it was produced.

## Versioned Docker Toolset ##
The solution for capturing the Docker toolset required by our builds and the mechanisms used to automatically acquire them are being covered by the work Matt Mitchell (Versionable Environments) and Ravi Eda ([Cmake versioning](https://github.com/dotnet/arcade/blob/main/Documentation/Project-Docs/cmake-scenarios.md)) are defining.  It is sufficient for the scope of this document to say that whatever pattern comes out of this work should be applied to the Docker toolset.

## Move Dockerfiles to Open ##
Work has been going on recently to check-in the build definitions into the product repositories (e.g. [corefx](https://github.com/dotnet/corefx/tree/master/buildpipeline)).  These build definitions reference our Docker images.  Because of this, it would be beneficial to move the Dockerfiles from the [private repository](https://devdiv.visualstudio.com/DevDiv/_git/DotNetCore?path=%2Fdockerfiles&version=GBmaster&_a=contents) into the open.  There are no trade secrets and they could be useful for others to see.  A natural place to put these shared Dockerfiles would be within the [buildtools repo](https://github.com/dotnet/buildtools).



<!-- Begin Generated Content: Doc Feedback -->
<sub>Was this helpful? [![Yes](https://helix.dot.net/f/ip/5?p=Documentation%5CProject-Docs%5Cdocker-image-usage-improvements.md)](https://helix.dot.net/f/p/5?p=Documentation%5CProject-Docs%5Cdocker-image-usage-improvements.md) [![No](https://helix.dot.net/f/in)](https://helix.dot.net/f/n/5?p=Documentation%5CProject-Docs%5Cdocker-image-usage-improvements.md)</sub>
<!-- End Generated Content-->
