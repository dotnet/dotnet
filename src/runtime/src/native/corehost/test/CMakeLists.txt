add_subdirectory(fx_ver)
add_subdirectory(mockcoreclr)
add_subdirectory(mockhostfxr/2_2)
add_subdirectory(mockhostfxr/5_0)
add_subdirectory(mockhostpolicy)
add_subdirectory(nativehost)
if (NOT RUNTIME_FLAVOR STREQUAL Mono)
    if (CLR_CMAKE_TARGET_WIN32)
        add_subdirectory(comsxs)
        add_subdirectory(ijw)
        add_subdirectory(typelibs)
    endif()
endif()
