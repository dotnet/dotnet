project(mcc_native)

add_library(native_i0c SHARED native_i0c.cpp)
add_library(native_i1c SHARED native_i1c.cpp)
add_library(native_i3c SHARED native_i3c.cpp)
add_library(native_i5c SHARED native_i5c.cpp)
add_library(native_i6c SHARED native_i6c.cpp)
add_library(native_i7c SHARED native_i7c.cpp)
add_library(native_i8c SHARED native_i8c.cpp)

add_library(native_i0s SHARED native_i0s.cpp)
add_library(native_i1s SHARED native_i1s.cpp)
add_library(native_i3s SHARED native_i3s.cpp)
add_library(native_i5s SHARED native_i5s.cpp)
add_library(native_i6s SHARED native_i6s.cpp)
add_library(native_i7s SHARED native_i7s.cpp)
add_library(native_i8s SHARED native_i8s.cpp)
