# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.
# See the LICENSE file in the project root for more information.

project(NETCoreCheckCA)
set(DOTNET_PROJECT_NAME "NETCoreCheckCA")

set(CMAKE_CONFIGURATION_TYPES "Debug;Release"
    CACHE STRING "Configuration types" FORCE)

set(SOURCES
    ./MsiLogger.cpp
    ./MsiWrapper.cpp
    ./NETCoreCheckCA.cpp
    ./NetCoreCheckCA.def 
    ./../NetCoreCheck.cpp
    ./../TempRuntimeConfigFile.cpp
)

link_directories(${APP_HOST_LIB_DIR})

include(../../lib.cmake)

add_definitions(-DUNICODE)
add_definitions(-D_UNICODE)
add_definitions(-DFEATURE_APPHOST=1)
add_definitions(-DNETHOST_USE_AS_STATIC)
remove_definitions(-D_DEBUG)

target_link_libraries(NETCoreCheckCA
        libnethost.lib
        shlwapi.lib
        msi.lib
        Version.lib
    )
