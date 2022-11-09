cmake_minimum_required(VERSION 3.14.2)

project(native)

include(${CLR_ENG_NATIVE_DIR}/configurecompiler.cmake)

if(MSVC)
    add_compile_options(/W1)

    # Host components don't try to handle asynchronous exceptions
    add_compile_options(/EHsc)
endif()

set(APP_HOST_LIB_DIR ${DOTNET_PACKS_DIR}/Microsoft.NETCore.App.Host.win-${CLR_CMAKE_HOST_ARCH}/${NET_CORE_PKG_VER}/runtimes/win-${CLR_CMAKE_HOST_ARCH}/native)

add_subdirectory(projects)
