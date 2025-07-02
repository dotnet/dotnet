#!/usr/bin/env bash

RESET="\033[0m"
RED="\033[0;31m"

print_install_instruction() {
  echo -e "${RED} Please refer to https://github.com/dotnet/cli#add-debian-feed to install the latest dotnet \n\n ${RESET}"
}

test() {
  echo -e "==========================================================="
  echo -e "Test ...... "
  echo -e "===========================================================\n"

  dotnet test WilsonUnix.sln --filter "category!=nonosxtests&category!=nonlinuxtests"
  echo -e "\n"
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
  test
fi

echo -e "done."

