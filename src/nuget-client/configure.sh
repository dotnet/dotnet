#!/usr/bin/env bash

CLI_DIR="$(pwd)/cli"

# Download the CLI install script to cli
echo "Installing .NET SDKs..."
mkdir -p $CLI_DIR
curl -o $CLI_DIR/dotnet-install.sh -L https://dot.net/v1/dotnet-install.sh --silent
if (( $? )); then
    echo "Could not download 'dotnet-install.sh' script. Please check your network and try again!"
    return 1
fi

# Run install.sh for cli
chmod +x $CLI_DIR/dotnet-install.sh

# If the DOTNET_SDK_VERSIONS environment variable is set, use its value instead of the ones in DotNetSdkVersions.txt
if [ "$DOTNET_SDK_VERSIONS" != "" ]; then
    echo "Using environment variable DOTNET_SDK_VERSIONS instead of DotNetSdkVersions.txt.  Value: '$DOTNET_SDK_VERSIONS'"
    IFS=';' read -ra array <<< "$DOTNET_SDK_VERSIONS"
    for CliArgs in "${array[@]}";
    do
        echo "'cli/dotnet-install.sh -InstallDir $CLI_DIR -NoPath $CliArgs'"
        
        cli/dotnet-install.sh -InstallDir $CLI_DIR -NoPath $CliArgs
        if (( $? )); then
            echo "The .NET install failed!"
            return 1
        fi
    done
else 
    # Get CLI Branches for testing
    cat build/DotNetSdkVersions.txt | while IFS=$'\r' read -r CliArgs || [[ -n $line ]];
    do
        if [ "${CliArgs:0:1}" != "#" ] || [ "$CliArgs" == "" ]; then
            echo "'cli/dotnet-install.sh -InstallDir $CLI_DIR -NoPath $CliArgs'"

            cli/dotnet-install.sh -InstallDir $CLI_DIR -NoPath $CliArgs
            if (( $? )); then
                echo "The .NET install failed!"
                return 1
            fi
        fi
    done
fi

export DOTNET_ROOT="$CLI_DIR"
export DOTNET_MULTILEVEL_LOOKUP="0"
export "PATH=$CLI_DIR:$PATH"

if [ "$CI" == "true" ]; then
    echo "##vso[task.setvariable variable=DOTNET_ROOT;isOutput=false;issecret=false;]$CLI_DIR"
    echo "##vso[task.setvariable variable=DOTNET_MULTILEVEL_LOOKUP;isOutput=false;issecret=false;]0"
    echo "##vso[task.prependpath]$CLI_DIR"
fi

# Display .NET CLI info
dotnet --info
if [ $? -ne 0 ]; then
    echo "dotnet is not available on the PATH!"
    return 1
fi

echo "=================================================================="

echo "Initializing submodules..."
git submodule init
git submodule update

echo "Restoring NuGet packages..."
dotnet msbuild build/build.proj /Target:Restore "/ConsoleLoggerParameters:Verbosity=Minimal;Summary;ForceNoAlign" /MaxCPUCount /NodeReuse:false
if [ $? -ne 0 ]; then
    echo "Restore packages failed!!"
    return 1
fi
