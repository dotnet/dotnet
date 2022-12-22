#!/usr/bin/env bash
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

SOURCE="${BASH_SOURCE[0]}"
while [ -h "$SOURCE" ]; do # resolve $SOURCE until the file is no longer a symlink
  DIR="$( cd -P "$( dirname "$SOURCE" )" && pwd )"
  SOURCE="$(readlink "$SOURCE")"
  [[ "$SOURCE" != /* ]] && SOURCE="$DIR/$SOURCE" # if $SOURCE was a relative symlink, we need to resolve it relative to the path where the symlink file was located
done

REPO_ROOT="$( cd -P "$( dirname "$SOURCE" )/../" && pwd )"

arcade_partition=

while [[ $# > 0 ]]; do
  opt="$(echo "${1/#--/-}" | awk '{print tolower($0)}')"
  case "$opt" in
    -partition)
      arcade_partition=$2
      shift
      ;;
  esac

  shift
done

export ARCADE_PARTITION=$arcade_partition

if [[ ! -z "$arcade_partition" ]]; then
  arcade_partition_suffix="-$arcade_partition"
fi

export PATH=$REPO_ROOT/.dotnet$arcade_partition_suffix:$PATH
export DOTNET_INSTALL_DIR=$REPO_ROOT/.dotnet$arcade_partition_suffix
export ArtifactsDir=$REPO_ROOT/artifacts$arcade_partition_suffix/

export DOTNET_MULTILEVEL_LOOKUP=0