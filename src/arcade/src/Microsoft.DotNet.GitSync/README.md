# Microsoft.DotNet.GitSync

This tool reads records from the table and does a series of further checks to determine if the commit needs to be mirrored.
The first check is whether the commit is a merged commit. If yes, then we skip this commit as we have already ported the individual commits from that push event.
The second check is whether the commit is a mirrored commit (Commit that is made through GitSync Tool). If yes, we skip this too.
It then opens up the PR in the respective repositories, adds the assignees and waits for new candidates.

## Configuration File
The configuration file has many sections. Some of them are described below

```Json
{
  "Repos": [ ],
  "UserName": "",
  "Branches": [ ],
}
```

The properties have the following semantics:
- Repos: It is the list of the repos that need to be synced. It includes basic information about the repositories like shared path, owner, name etc
- UserName: Username of the GitHub account which will be used to open PullRequests in the dotnet repositories to mirror the commits. 
- Branches: It is the list of the branches that need to be synced for all the repos.

## Deployment
In order to deploy the tool you need to take the following steps :-
- You need to clone all the required repos on the machine.
- You can then publish the tool and input all the required settings in the settings.json(configuration) file.

## How to handle missing commits
We are storing all the commits (occurring in the shared path) in the Azure Table. You can go to the Azure table and retrieve the entires with the value of commit id as the missing commit. There is a boolean field called "mirrored", switch its value to be false and then this commit will be picked up by the tool in the next iteration.
There is also another column called "PR" which will contain the reason about why this commit was skipped in the first place. 

## How to resolve merge conflicts
Sometimes the tool encounters merge conflicts. They need to be handled manually. The best way is to look at the .patch file, and find out the files involved in merge conflicts. Then, you need to find which repository has the correct version of these files and replace merge conflict files with correct version. You then need to do ```git add *``` and 
```git am --continue``` from the command line. Then you can go to the tool and resume its execution by pressing any key. 

## How to use this for other repos
In order to use this tool for any other pair of repos, you need to take following actions :-

- You first need an Azure storage account. You then need to create an Azure cosmos db table with columns such as TargetRepo(PartitionKey), commitID(RowKey), Branch, Mirrored, PR and SourceRepo.
- You also need another table with information about repositories i.e. which repos need to be mirrored into which repos. The columns required will be SourceRepo and ReposToMirrorInto.
- You can now follow the steps listed in deployment section.

## How to populate the commit table
This can be done in 2 ways
- Using [Microsoft.DotNet.GitSync.CommitManager](../Microsoft.DotNet.GitSync.CommitManager/README.md) tool.
- Using the LastSynchronisedCommit property of RepositoryInfo object. Although this is sufficient for mirroring all the commits, but it should only be used as a fail safe as it is little bit less reliable and results in more missing commits.

[Related Documentation](../../Documentation/GitSyncTools.md )
