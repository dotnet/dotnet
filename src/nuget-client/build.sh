#!/usr/bin/env bash

while true ; do
	case "$1" in
		-c|--clear-cache) CLEAR_CACHE=1 ; shift ;;
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

# init the repo
git submodule init
git submodule update

# clear caches
if [ "$CLEAR_CACHE" == "1" ]; then
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
