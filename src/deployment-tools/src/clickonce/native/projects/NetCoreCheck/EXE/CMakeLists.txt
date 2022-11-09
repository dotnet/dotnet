# Licensed to the .NET Foundation under one or more agreements.
# The .NET Foundation licenses this file to you under the MIT license.
# See the LICENSE file in the project root for more information.

project(NetCoreCheck)
set(DOTNET_PROJECT_NAME "NetCoreCheck")

set(CMAKE_CONFIGURATION_TYPES "Debug;Release"
    CACHE STRING "Configuration types" FORCE)

set(SOURCES
    ./FileLogger.cpp
    ./NetCoreCheckExe.cpp
    ./../NetCoreCheck.cpp
    ./../TempRuntimeConfigFile.cpp
)

link_directories(${APP_HOST_LIB_DIR})

include(../../exe.cmake)

add_definitions(-DUNICODE)
add_definitions(-D_UNICODE)
add_definitions(-DFEATURE_APPHOST=1)
add_definitions(-DNETHOST_USE_AS_STATIC)
remove_definitions(-D_DEBUG)

target_link_libraries(NetCoreCheck
        shlwapi.lib
        libnethost.lib
    )

# Disable manifest generation into the file .exe on Windows
if(CLR_CMAKE_TARGET_WIN32)
    set_property(TARGET ${PROJECT_NAME} PROPERTY
            LINK_FLAGS "/MANIFEST:NO"
        )
endif()

