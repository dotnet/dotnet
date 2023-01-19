<!--
########  ########    ###    ########     ######## ##     ## ####  ######
##     ## ##         ## ##   ##     ##       ##    ##     ##  ##  ##    ##
##     ## ##        ##   ##  ##     ##       ##    ##     ##  ##  ##
########  ######   ##     ## ##     ##       ##    #########  ##   ######
##   ##   ##       ######### ##     ##       ##    ##     ##  ##        ##
##    ##  ##       ##     ## ##     ##       ##    ##     ##  ##  ##    ##
##     ## ######## ##     ## ########        ##    ##     ## ####  ######
-->

This Codespace can help you debug the source build of .NET. In case you have run this from a
`dotnet/installer` PR branch, it will contain the VMR (`dotnet/dotnet`) checked out into
`/workspaces/dotnet` with the PR changes pulled into it. You can then attempt to source-build
the VMR which is what the VMR leg in the installer PR build doing. This build takes about 45
minutes and, after completion, produces an archived .NET SDK in `/workspaces/artifacts/x64/Release`.

To build the VMR, run following:
```bash
cd /workspaces/dotnet
unset RepositoryName
./build.sh --online
```

> Please note that, at this time, the build modifies some of the checked-in sources so it might
be preferential to rebuild the Codespace between attempts.

For more details, see the instructions at https://github.com/dotnet/dotnet.
