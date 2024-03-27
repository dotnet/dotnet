@echo off
echo *** .NET EMSDK path setup ***

REM emscripten (emconfigure, em++, etc)
if "%EMSDK_PATH%"=="" (
  echo %EMSDK_PATH% is empty
  exit /b 1
)
set "TOADD_PATH_EMSCRIPTEN=%EMSDK_PATH%emscripten"
echo PATH += %TOADD_PATH_EMSCRIPTEN%
set "PATH=%TOADD_PATH_EMSCRIPTEN%;%PATH%"

REM python
if "%EMSDK_PYTHON%"=="" (
  echo %EMSDK_PYTHON% is empty
  exit /b 1
)
set "TOADD_PATH_PYTHON=%EMSDK_PYTHON%"
echo PATH += %TOADD_PATH_PYTHON%
set "PATH=%TOADD_PATH_PYTHON%;%PATH%"

REM llvm (clang, etc)
if "%DOTNET_EMSCRIPTEN_LLVM_ROOT%"=="" (
  echo %DOTNET_EMSCRIPTEN_LLVM_ROOT% is empty
  exit /b 1
)
set "TOADD_PATH_LLVM=%DOTNET_EMSCRIPTEN_LLVM_ROOT%"
if not "%TOADD_PATH_EMSCRIPTEN%"=="%TOADD_PATH_LLVM%" (
  echo PATH += %TOADD_PATH_LLVM%
  set "PATH=%TOADD_PATH_LLVM%;%PATH%"
)

REM nodejs (node)
if "%DOTNET_EMSCRIPTEN_NODE_JS%"=="" (
  echo %DOTNET_EMSCRIPTEN_NODE_JS% is empty
  exit /b 1
)
if "%DOTNET_EMSCRIPTEN_NODE_PATH%"=="" (
  echo %DOTNET_EMSCRIPTEN_NODE_PATH% is empty
  exit /b 1
)
set "TOADD_PATH_NODEJS=%DOTNET_EMSCRIPTEN_NODE_JS%"
if not "%TOADD_PATH_EMSCRIPTEN%"=="%TOADD_PATH_NODEJS%" (
  if not "%TOADD_PATH_LLVM%"=="%TOADD_PATH_NODEJS%" (
    echo NODE PATH += %TOADD_PATH_NODEJS%
    set "PATH=%TOADD_PATH_NODEJS%;%PATH%"
    set "PATH=%DOTNET_EMSCRIPTEN_NODE_PATH%;%PATH%"
  )
)

REM binaryen (wasm-opt, etc)
if "%DOTNET_EMSCRIPTEN_BINARYEN_ROOT%"=="" (
  echo %DOTNET_EMSCRIPTEN_BINARYEN_ROOT% is empty
  exit /b 1
)
set "TOADD_PATH_BINARYEN=%DOTNET_EMSCRIPTEN_BINARYEN_ROOT%bin"
if not "%TOADD_PATH_EMSCRIPTEN%"=="%TOADD_PATH_BINARYEN%" (
  if not "%TOADD_PATH_LLVM%"=="%TOADD_PATH_BINARYEN%" (
    if not "%TOADD_PATH_NODEJS%"=="%TOADD_PATH_BINARYEN%" (
      echo PATH += %TOADD_PATH_BINARYEN%
      set "PATH=%TOADD_PATH_BINARYEN%;%PATH%"
    )
  )
)
