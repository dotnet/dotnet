@if "%_echo%" neq "on" echo off
rem
rem This file invokes cmake and generates the build system for windows.

set argC=0
for %%x in (%*) do Set /A argC+=1

if NOT %argC%==8 GOTO :USAGE
if %1=="/?" GOTO :USAGE

setlocal
set __sourceDir=%~dp0..
set __VSString=%2
 :: Remove quotes
set __VSString=%__VSString:"=%
set __ExtraCmakeParams=

:: Set the target architecture to a format cmake understands. ANYCPU defaults to x64
set __Arch=%3
if /i "%__Arch%" == "x64"     (set cm_BaseRid=win7&&set __ExtraCmakeParams=%__ExtraCmakeParams% -A x64)
if /i "%__Arch%" == "x86"     (set cm_BaseRid=win7&&set  __ExtraCmakeParams=%__ExtraCmakeParams% -A Win32)
if /i "%__Arch%" == "arm"     (set cm_BaseRid=win8&&set   __ExtraCmakeParams=%__ExtraCmakeParams% -A ARM)
if /i "%__Arch%" == "arm64"   (set cm_BaseRid=win10&&set __ExtraCmakeParams=%__ExtraCmakeParams% -A ARM64)

set __LatestCommit=%4
set __NativeVersion=%5
set __NetCorePkgVersion=%6
set __DotnetInstallDir=%~7
shift

:: Form the base RID to be used if we are doing a portable build
if /i "%8" == "1"       (set cm_BaseRid=win)
set cm_BaseRid=%cm_BaseRid%-%__Arch%
echo "Computed RID for native build is %cm_BaseRid%"

if defined CMakePath goto DoGen

:: Eval the output from probe-win1.ps1
pushd "%__sourceDir%"
for /f "delims=" %%a in ('powershell -NoProfile -ExecutionPolicy ByPass "& .\Windows\probe-win.ps1"') do %%a
popd

:DoGen
set __ExtraCmakeParams=%__ExtraCmakeParams% "-DCMAKE_SYSTEM_VERSION=10.0" "-DCLI_CMAKE_NATIVE_VER=%__NativeVersion%"
set __ExtraCmakeParams=%__ExtraCmakeParams% "-DCLI_CMAKE_PKG_RID=%cm_BaseRid%" "-DCLI_CMAKE_COMMIT_HASH=%__LatestCommit%" "-DCLR_CMAKE_HOST_ARCH=%__Arch%"
set __ExtraCmakeParams=%__ExtraCmakeParams% "-DCMAKE_INSTALL_PREFIX=%__CMakeBinDir%" "-DCLI_CMAKE_RESOURCE_DIR=%__ResourcesDir%" "-DCLR_ENG_NATIVE_DIR=%__sourceDir%\..\..\..\eng\native"
set __ExtraCmakeParams=%__ExtraCmakeParams% "-DNET_CORE_PKG_VER=%__NetCorePkgVersion%" "-DDOTNET_PACKS_DIR=%__DotnetInstallDir%\packs"

echo "%CMakePath%" %__sourceDir% -G "Visual Studio %__VSString%" %__ExtraCmakeParams%
"%CMakePath%" %__sourceDir% -G "Visual Studio %__VSString%" %__ExtraCmakeParams%
endlocal
GOTO :DONE

:USAGE
  echo "Usage..."
  echo "gen-buildsys-win.bat <path to top level CMakeLists.txt> <VSVersion> <Target Architecture>"
  echo "Specify the path to the top level CMake file"
  echo "Specify the VSVersion to be used - 2022"
  echo "Specify the Target Architecture - AnyCPU, x86, x64, ARM, or ARM64."
  echo "Specify latest commit hash"
  echo "Specify the native version"
  EXIT /B 1

:DONE
  EXIT /B 0
