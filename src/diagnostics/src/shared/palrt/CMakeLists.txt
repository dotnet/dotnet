
set(CMAKE_INCLUDE_CURRENT_DIR ON)

set(PALRT_SOURCES
    bstr.cpp
    coguid.cpp
    comem.cpp
    guid.cpp
    memorystream.cpp
    path.cpp
    variant.cpp
)

add_library_clr(palrt
    STATIC
    ${PALRT_SOURCES}
)
