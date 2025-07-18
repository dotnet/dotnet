project(DesktopClrHost)

set(CMAKE_INCLUDE_CURRENT_DIR ON)

include_directories(inc)
include_directories("$ENV{VSInstallDir}/DIA SDK/include")

add_definitions(-DUSE_STL)

set(DESKTOPCLRHOST_SOURCES
    DesktopClrHost.cpp
)
  
set(DESKTOPCLRHOST_LIBRARY
    ${STATIC_MT_CRT_LIB}
    ${STATIC_MT_CPP_LIB}
    ${STATIC_MT_VCRT_LIB}
    kernel32.lib
    user32.lib
    ole32.lib
    oleaut32.lib
    uuid.lib
    version.lib
    advapi32.lib
    psapi.lib
    ntdll.lib
    mscoree.lib
)

add_library_clr(DesktopClrHost SHARED ${DESKTOPCLRHOST_SOURCES})

# When building with ninja we need to explicitly link against the 4.8.1 version of the .NET Framework SDK because it is the only one that supports ARM64.
if(CLR_CMAKE_TARGET_ARCH_ARM64)
    target_link_directories(DesktopClrHost PRIVATE "$ENV{WindowsSdkDir}/../NETFXSDK/4.8.1/Lib/um/arm64")
endif(CLR_CMAKE_TARGET_ARCH_ARM64)

target_link_libraries(DesktopClrHost ${DESKTOPCLRHOST_LIBRARY})

install_clr(TARGETS DesktopClrHost DESTINATIONS .)
