<!--
########  ########    ###    ########     ######## ##     ## ####  ######
##     ## ##         ## ##   ##     ##       ##    ##     ##  ##  ##    ##
##     ## ##        ##   ##  ##     ##       ##    ##     ##  ##  ##
########  ######   ##     ## ##     ##       ##    #########  ##   ######
##   ##   ##       ######### ##     ##       ##    ##     ##  ##        ##
##    ##  ##       ##     ## ##     ##       ##    ##     ##  ##  ##    ##
##     ## ######## ##     ## ########        ##    ##     ## ####  ######
-->

This Codespace allows you to debug or make changes to the .NET SDK product. The build takes about
45 up to 75 minutes (depending on the machine and OS) and, after completion, produces an archived
.NET SDK located in `/workspaces/dotnet/artifacts/assets/Release`.

## Build the SDK

To build the repository, run one of the following:

```bash
# Microsoft based build
./build.sh
```
or

```bash
# Building from source only
./prep-source-build.sh && ./build.sh -sb
```

> Please note that, at this time, the build modifies some of the checked-in sources so it might
be preferential to rebuild the Codespace between attempts (or reset the working tree changes).

For more details, see the instructions at https://github.com/dotnet/dotnet#building.
