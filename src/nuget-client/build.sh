#!/usr/bin/env bash

while true ; do
	case "$1" in
		-c|--clear-cache) CLEAR_CACHE=1 ; shift ;;
		--) shift ; break ;;
		*) shift ; break ;;
	esac
done

RESULTCODE=0

# Download the CLI install script to cli
echo "Installing dotnet CLI"
mkdir -p cli
curl -o cli/dotnet-install.sh -L https://dot.net/v1/dotnet-install.sh

if (( $? )); then
	echo "Could not download 'dotnet-install.sh' script. Please check your network and try again!"
	exit 1
fi

# Run install.sh for cli
chmod +x cli/dotnet-install.sh

# Get recommended version for bootstrapping testing version
cli/dotnet-install.sh -i cli -c 2.2 -nopath

if (( $? )); then
	echo "The .NET CLI Install failed!!"
	exit 1
fi

# Disable .NET CLI Install Lookup
DOTNET_MULTILEVEL_LOOKUP=0

DOTNET="$(pwd)/cli/dotnet"

# Let the dotnet cli expand and decompress first if it's a first-run
$DOTNET --info

# Get CLI Branches for testing
echo "dotnet msbuild build/config.props /restore:false /ConsoleLoggerParameters:Verbosity=Minimal;NoSummary;ForceNoAlign /nologo /target:GetCliBranchForTesting"

IFS=$'\n'
CMD_OUT_LINES=(`dotnet msbuild build/config.props /restore:false /ConsoleLoggerParameters:Verbosity=Minimal;NoSummary;ForceNoAlign /nologo /target:GetCliBranchForTesting`)
# Take only last the line which has the version information and strip all the spaces
DOTNET_BRANCHES=${CMD_OUT_LINES[-1]//[[:space:]]}
unset IFS

IFS=$';'
for DOTNET_BRANCH in ${DOTNET_BRANCHES[@]}
do
	echo $DOTNET_BRANCH

	IFS=$':'
	ChannelAndVersion=($DOTNET_BRANCH)
	Channel=${ChannelAndVersion[0]}
	if [ ${#ChannelAndVersion[@]} -eq 1 ]
	then
		Version="latest"
	else
		Version=${ChannelAndVersion[1]}
	fi
	unset IFS

	echo "Channel is: $Channel"
	echo "Version is: $Version"
	cli/dotnet-install.sh -i cli -c $Channel -v $Version -nopath

	if (( $? )); then
		echo "The .NET CLI Install for $DOTNET_BRANCH failed!!"
		exit 1
	fi
done

# Display .NET CLI info
$DOTNET --info

echo "================="

# init the repo
git submodule init
git submodule update

# clear caches
if [ "$CLEAR_CACHE" == "1" ]
then
	# echo "Clearing the nuget web cache folder"
	# rm -r -f ~/.local/share/NuGet/*

	echo "Clearing the nuget packages folder"
	rm -r -f ~/.nuget/packages/*
fi

# restore packages
echo "dotnet msbuild build/bootstrap.proj /t:Restore"
dotnet msbuild build/bootstrap.proj /t:Restore
echo "dotnet msbuild build/build.proj /t:Restore /p:Configuration=Release /p:BuildNumber=1 /p:ReleaseLabel=beta"
dotnet msbuild build/build.proj /t:Restore /p:Configuration=Release /p:BuildNumber=1 /p:ReleaseLabel=beta

if [ $? -ne 0 ]; then
	echo "Restore failed!!"
	exit 1
fi

# run tests
echo "dotnet msbuild build/build.proj /t:CoreUnitTests /p:Configuration=Release /p:BuildNumber=1 /p:ReleaseLabel=beta"
dotnet msbuild build/build.proj /t:CoreUnitTests /p:Configuration=Release /p:BuildNumber=1 /p:ReleaseLabel=beta

if [ $? -ne 0 ]; then
	echo "Tests failed!!"
	exit 1
fi

exit $RESULTCODE
