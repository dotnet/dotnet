# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.
# See the LICENSE file in the project root for more information.

project (${DOTNET_PROJECT_NAME})

cmake_policy(SET CMP0011 NEW)
cmake_policy(SET CMP0083 NEW)

include(${CMAKE_CURRENT_LIST_DIR}/common.cmake)

add_executable(${DOTNET_PROJECT_NAME} ${SOURCES} ${RESOURCES})

if(NOT CLR_CMAKE_TARGET_WIN32)
    disable_pax_mprotect(${DOTNET_PROJECT_NAME})
endif()

install_with_stripped_symbols(${DOTNET_PROJECT_NAME} TARGETS native)

set_common_libs("exe")
