#!/usr/bin/env bash

RESET="\033[0m"
RED="\033[0;31m"

print_install_instruction() {
  echo -e "${RED} Please refer to https://github.com/dotnet/cli#add-debian-feed to install the latest dotnet \n\n ${RESET}"
}

restore() {
  echo -e "==========================================================="
  echo -e "Restore ...... "
  echo -e "===========================================================\n"

  dotnet restore WilsonUnix.sln
  echo -e "\n"
}

build() {
  echo -e "==========================================================="
  echo -e "Build ...... "
  echo -e "===========================================================\n"

  dotnet build --no-restore WilsonUnix.sln
  echo -e "\n"
}

pack() {
  echo -e "==========================================================="
  echo -e "Pack ...... "
  echo -e "===========================================================\n"

  dotnet pack --no-build WilsonUnix.sln -c Debug
  
  echo -e "\n"
  echo -e "==========================================================="
  echo -e "Moving nuget packages to 'artifacts' folder ...... "
  echo -e "===========================================================\n"
  rm -rf artifacts
  mkdir artifacts
  mv src/*/bin/Debug/*.nupkg artifacts
}

echo -e "==========================================================="
echo -e "Check the installation and the version of dotnet ...... "
echo -e "===========================================================\n"

if ! type "dotnet" > /dev/null 2>&1; then
  echo -e "${RED}Error: dotnet is not installed\n ${RESET}"
  print_install_instruction
else
  VERSION="$(dotnet --version)"
  echo -e "  dotnet version ${VERSION} is found.\n"
  restore
  build
  pack
fi

echo -e "done."

