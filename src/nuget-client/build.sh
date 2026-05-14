#!/usr/bin/env bash

CONFIGURATION=
CI=0
BUILD_NUMBER=
RELEASE_LABEL=zlocal
ARGUMENTS=()

while true ; do
	case "$1" in
		-c) CONFIGURATION=$2 ; shift ;;
		--ci) CI=1 ; shift ;;
		--clear-cache) CLEAR_CACHE=1 ; shift ;;
		-n) BUILD_NUMBER=$2 ; shift ;;
		-l) RELEASE_LABEL=$2 ; shift ;;
		-p:*) ARGUMENTS+=( "$1" ) ; shift ;;
		/p:*) ARGUMENTS+=( "$1" ) ; shift ;;
		--) shift ; break ;;
		*) shift ; break ;;
	esac
done

# Run configure which installs the .NET SDK
. ./configure.sh
if [ $? -ne 0 ]; then
    echo "configure.sh failed !!"
    exit 1
fi

# clear caches
if [ "$CLEAR_CACHE" == "1" ]; then
	# echo "Clearing the nuget web cache folder"
	# rm -r -f ~/.local/share/NuGet/*

	echo "Clearing the nuget packages folder"
	rm -r -f ~/.nuget/packages/*
fi

if [ "$BUILD_NUMBER" != "" ]; then
	ARGUMENTS+=( "/p:BuildNumber=$BUILD_NUMBER" )
fi

if [ "$RELEASE_LABEL" != "" ]; then
	ARGUMENTS+=( "/p:ReleaseLabel=$RELEASE_LABEL" )
fi

# CI build is Release by default, local builds are Debug by default
if [ "$CONFIGURATION" == "" ]; then
	if [ "$CI" == "1" ]; then
		CONFIGURATION=Release
	else
		CONFIGURATION=Debug
	fi
fi

# restore packages
echo "dotnet msbuild build/build.proj /t:Restore /p:Configuration=$CONFIGURATION" ${ARGUMENTS[@]+"${ARGUMENTS[@]}"}
dotnet msbuild build/build.proj /t:Restore /p:Configuration=$CONFIGURATION ${ARGUMENTS[@]+"${ARGUMENTS[@]}"}

if [ $? -ne 0 ]; then
	echo "Restore failed!!"
	exit 1
fi

# run tests
echo "dotnet msbuild build/build.proj /t:CoreUnitTests /p:Configuration=$CONFIGURATION" ${ARGUMENTS[@]+"${ARGUMENTS[@]}"}
dotnet msbuild build/build.proj /t:CoreUnitTests /p:Configuration=$CONFIGURATION ${ARGUMENTS[@]+"${ARGUMENTS[@]}"}

if [ $? -ne 0 ]; then
	echo "Tests failed!!"
	exit 1
fi
