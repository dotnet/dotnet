# Microsoft.DotNet.GitSync.CommitManager

It runs on maestro triggers. It adds commits that need to be ported to the Azure storage table.
Webhook sends the payload to the maestro after every push event in any of the 3 repositories (corefx, coreclr, corert).
The payload contains the path of the changed files in that commit. If any of these path is in the shared folder, then that commit is a candidate for Azure table.

## Arguments
The Arguments that need to be supplied to this tool are 

- -k = Azure Account Key
- -u = Azure Account Name
- -r = Repository to which commit is made
- -b = Branch to which commit is made
- -c = Sha of Commit(s)

```
dotnet run Microsoft.DotNet.GitSync.CommitManager.csproj -- -u $(AccountName) -k *** -r dotnet/coreclr -c 5d31194880e800a9df8eef76e7a0a53646aa72d3 -b master
```

## How to handle failures
The tool is currently being run as a build definition. You can look at the logs of the build definition in order to get more info about the failure. The information about the build definition is present [here](https://github.com/dotnet/versions/blob/master/Maestro/subscriptions.json#L153)

## How to use it for other repos
In order to use this tool for any other pair of repos, you need to take following actions :-

- You first need an Azure storage account. You then need to create an Azure cosmos db table with columns such as TargetRepo(PartitionKey), commitID(RowKey), Branch, Mirrored, PR and SourceRepo.
- You also need another table with information about repositories i.e. which repos need to be mirrored into which repos. The columns required will be SourceRepo and ReposToMirrorInto.
- You also need to setup the [webhook](https://developer.github.com/webhooks/creating/) in all the repos whose commits need to be mirrored. You then need to check if the commit is in the shared path by looking at the webhook payload.
- If the commit is in the shared path, you need to build and run this tool with appropriate arguments.
- The webhook part could be done through [maestro](https://github.com/dotnet/versions/tree/master/Maestro) or you can write an Azure function which receives the webhook payload and takes the required actions.

[Related Documentation](../../Documentation/GitSyncTools.md )
