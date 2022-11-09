#!/usr/bin/env bash

set -ue

source="${BASH_SOURCE[0]}"

# resolve $source until the file is no longer a symlink
while [[ -h "$source" ]]; do
  scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"
  source="$(readlink "$source")"
  # if $source was a relative symlink, we need to resolve it relative to the path where the
  # symlink file was located
  [[ $source != /* ]] && source="$scriptroot/$source"
done
scriptroot="$( cd -P "$( dirname "$source" )" && pwd )"

usage()
{
  echo "Common settings:"
  echo "  --arch                          Target platform: x86, x64, arm, armel, arm64 or wasm."
  echo "                                  [Default: Your machine's architecture.]"
  echo "  --binaryLog (-bl)               Output binary log."
  echo "  --cross                         Optional argument to signify cross compilation."
  echo "  --configuration (-c)            Build configuration: Debug, Release or Checked."
  echo "                                  Checked is exclusive to the CLR subset. It is the same as Debug, except code is"
  echo "                                  compiled with optimizations enabled."
  echo "                                  [Default: Debug]"
  echo "  --help (-h)                     Print help and exit."
  echo "  --os                            Target operating system: Windows_NT, Linux, FreeBSD, OSX, tvOS, iOS, Android,"
  echo "                                  Browser, NetBSD, illumos or Solaris."
  echo "                                  [Default: Your machine's OS.]"
  echo "  --projects <value>              Project or solution file(s) to build."
  echo "  --subset (-s)                   Build a subset, print available subsets with -subset help."
  echo "                                  [Default: Builds the entire repo.]"
  echo "  --verbosity (-v)                MSBuild verbosity: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]."
  echo "                                  [Default: Minimal]"
  echo ""

  echo "Actions (defaults to --restore --build):"
  echo "  --build (-b)               Build all source projects."
  echo "                             This assumes --restore has been run already."
  echo "  --clean                    Clean the solution."
  echo "  --pack                     Package build outputs into NuGet packages."
  echo "  --publish                  Publish artifacts (e.g. symbols)."
  echo "                             This assumes --build has been run already."
  echo "  --rebuild                  Rebuild all source projects."
  echo "  --restore (-r)             Restore dependencies."
  echo "  --sign                     Sign build outputs."
  echo "  --test (-t)                Incrementally builds and runs tests."
  echo "                             Use in conjuction with --testnobuild to only run tests."
  echo ""

  echo "Libraries settings:"
  echo "  --allconfigurations        Build packages for all build configurations."
  echo "  --coverage                 Collect code coverage when testing."
  echo "  --framework (-f)           Build framework: net5.0 or net472."
  echo "                             [Default: net5.0]"
  echo "  --testnobuild              Skip building tests when invoking -test."
  echo "  --testscope                Test scope, allowed values: innerloop, outerloop, all."
  echo ""

  echo "Native build settings:"
  echo "  --clang                    Optional argument to build using clang in PATH (default)."
  echo "  --clangx                   Optional argument to build using clang version x (used for Clang 7 and newer)."
  echo "  --clangx.y                 Optional argument to build using clang version x.y (used for Clang 6 and older)."
  echo "  --cmakeargs                User-settable additional arguments passed to CMake."
  echo "  --gcc                      Optional argument to build using gcc in PATH (default)."
  echo "  --gccx.y                   Optional argument to build using gcc version x.y."
  echo "  --portablebuild            Optional argument: set to false to force a non-portable build."
  echo ""

  echo "Command line arguments starting with '/p:' are passed through to MSBuild."
  echo "Arguments can also be passed in with a single hyphen."
  echo ""

  echo "Here are some quick examples. These assume you are on a Linux x64 machine:"
  echo ""
  echo "There are no projects that build for Linux at the moment - these are just examples."
  echo ""
  echo "* Build ClickOnce for Linux x64 on Release configuration:"
  echo "./build.sh -subset clickonce -c release"
  echo ""
  echo "For more general information, check out https://github.com/dotnet/runtime/blob/main/docs/workflow/README.md"
}

initDistroRid()
{
    source "$scriptroot"/native/init-distro-rid.sh

    local passedRootfsDir=""
    local targetOs="$1"
    local buildArch="$2"
    local isCrossBuild="$3"
    local isPortableBuild="$4"

    # Only pass ROOTFS_DIR if __DoCrossArchBuild is specified.
    if (( isCrossBuild == 1 )); then
        passedRootfsDir=${ROOTFS_DIR}
    fi
    initDistroRidGlobal ${targetOs} ${buildArch} ${isPortableBuild} ${passedRootfsDir}
}

showSubsetHelp()
{
  "$scriptroot/common/build.sh" "-restore" "-build" "/p:Subset=help" "/clp:nosummary"
}

arguments=''
cmakeargs=''
extraargs=''
crossBuild=0
portableBuild=1

source $scriptroot/native/init-os-and-arch.sh

# Check if an action is passed in
declare -a actions=("b" "build" "r" "restore" "rebuild" "testnobuild" "sign" "publish" "clean")
actInt=($(comm -12 <(printf '%s\n' "${actions[@]/#/-}" | sort) <(printf '%s\n' "${@/#--/-}" | sort)))
firstArgumentChecked=0

while [[ $# > 0 ]]; do
  opt="$(echo "${1/#--/-}" | awk '{print tolower($0)}')"

  if [[ $firstArgumentChecked -eq 0 && $opt =~ ^[a-zA-Z.+]+$ ]]; then
    if [ $opt == "help" ]; then
      showSubsetHelp
      exit 0
    fi

    arguments="$arguments /p:Subset=$1"
    shift 1
    continue
  fi

  firstArgumentChecked=1

  case "$opt" in
     -help|-h)
      usage
      exit 0
      ;;

     -subset|-s)
      if [ -z ${2+x} ]; then
        showSubsetHelp
        exit 0
      else
        passedSubset="$(echo "$2" | awk '{print tolower($0)}')"
        if [ $passedSubset == "help" ]; then
          showSubsetHelp
          exit 0
        fi
        arguments="$arguments /p:Subset=$2"
        shift 2
      fi
      ;;

     -arch)
      if [ -z ${2+x} ]; then
        echo "No architecture supplied. See help (--help) for supported architectures." 1>&2
        exit 1
      fi
      passedArch="$(echo "$2" | awk '{print tolower($0)}')"
      case "$passedArch" in
        x64|x86|arm|armel|arm64|wasm)
          arch=$passedArch
          ;;
        *)
          echo "Unsupported target architecture '$2'."
          echo "The allowed values are x86, x64, arm, armel, arm64, and wasm."
          exit 1
          ;;
      esac
      shift 2
      ;;

     -configuration|-c)
      if [ -z ${2+x} ]; then
        echo "No configuration supplied. See help (--help) for supported configurations." 1>&2
        exit 1
      fi
      passedConfig="$(echo "$2" | awk '{print tolower($0)}')"
      case "$passedConfig" in
        debug|release|checked)
          val="$(tr '[:lower:]' '[:upper:]' <<< ${passedConfig:0:1})${passedConfig:1}"
          ;;
        *)
          echo "Unsupported target configuration '$2'."
          echo "The allowed values are Debug, Release, and Checked."
          exit 1
          ;;
      esac
      arguments="$arguments -configuration $val"
      shift 2
      ;;

     -framework|-f)
      if [ -z ${2+x} ]; then
        echo "No framework supplied. See help (--help) for supported frameworks." 1>&2
        exit 1
      fi
      val="$(echo "$2" | awk '{print tolower($0)}')"
      arguments="$arguments /p:BuildTargetFramework=$val"
      shift 2
      ;;

     -os)
      if [ -z ${2+x} ]; then
        echo "No target operating system supplied. See help (--help) for supported target operating systems." 1>&2
        exit 1
      fi
      passedOS="$(echo "$2" | awk '{print tolower($0)}')"
      case "$passedOS" in
        windows_nt)
          os="Windows_NT" ;;
        linux)
          os="Linux" ;;
        freebsd)
          os="FreeBSD" ;;
        osx)
          os="OSX" ;;
        tvos)
          os="tvOS" ;;
        ios)
          os="iOS" ;;
        android)
          os="Android" ;;
        browser)
          os="Browser" ;;
        illumos)
          os="illumos" ;;
        solaris)
          os="Solaris" ;;
        *)
          echo "Unsupported target OS '$2'."
          echo "The allowed values are Windows_NT, Linux, FreeBSD, OSX, tvOS, iOS, Android, Browser, illumos and Solaris."
          exit 1
          ;;
      esac
      arguments="$arguments /p:TargetOS=$os"
      shift 2
      ;;

     -allconfigurations)
      arguments="$arguments /p:BuildAllConfigurations=true"
      shift 1
      ;;

     -testscope)
      if [ -z ${2+x} ]; then
        echo "No test scope supplied. See help (--help) for supported test scope values." 1>&2
        exit 1
      fi
      arguments="$arguments /p:TestScope=$2"
      shift 2
      ;;

     -testnobuild)
      arguments="$arguments /p:TestNoBuild=true"
      shift 1
      ;;

     -coverage)
      arguments="$arguments /p:Coverage=true"
      shift 1
      ;;

     -cross)
      crossBuild=1
      arguments="$arguments /p:CrossBuild=True"
      shift 1
      ;;

     -clang*)
      arguments="$arguments /p:Compiler=$opt"
      shift 1
      ;;

     -cmakeargs)
      if [ -z ${2+x} ]; then
        echo "No cmake args supplied." 1>&2
        exit 1
      fi
      cmakeargs="${cmakeargs} ${opt} $2"
      shift 2
      ;;

     -gcc*)
      arguments="$arguments /p:Compiler=$opt"
      shift 1
      ;;

     -portablebuild)
      if [ -z ${2+x} ]; then
        echo "No value for portablebuild is supplied. See help (--help) for supported values." 1>&2
        exit 1
      fi
      passedPortable="$(echo "$2" | awk '{print tolower($0)}')"
      if [ "$passedPortable" = false ]; then
        portableBuild=0
        arguments="$arguments /p:PortableBuild=false"
      fi
      shift 2
      ;;

      *)
      extraargs="$extraargs $1"
      shift 1
      ;;
  esac
done

if [ ${#actInt[@]} -eq 0 ]; then
    arguments="-restore -build $arguments"
fi

initDistroRid $os $arch $crossBuild $portableBuild

# URL-encode space (%20) to avoid quoting issues until the msbuild call in /eng/common/tools.sh.
# In *proj files (XML docs), URL-encoded string are rendered in their decoded form.
cmakeargs="${cmakeargs// /%20}"
arguments="$arguments /p:TargetArchitecture=$arch"
arguments="$arguments /p:CMakeArgs=\"$cmakeargs\" $extraargs"
"$scriptroot/common/build.sh" $arguments
