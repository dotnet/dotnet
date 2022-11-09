// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma once

#include <Windows.h>
#include <shlwapi.h>
#include <tchar.h>
#include <stdio.h>

#include "Logger.h"

// Return Codes
#define EXIT_SUCCESS 0                                            // Required runtime is installed
#define NETCORECHECK_BASEERROR 0x3000
#define MAKE_NETCORECHECK_HRESULT(x) NETCORECHECK_BASEERROR+x
#define EXIT_FAILURE_LOADHOSTFXR    MAKE_NETCORECHECK_HRESULT(1)  // No runtime is installed
#define EXIT_FAILURE_INITHOSTFXR    MAKE_NETCORECHECK_HRESULT(2)  // Required runtime is not installed
#define EXIT_FAILURE_HOSTFXREXPORTS MAKE_NETCORECHECK_HRESULT(3)  // Failed to get hostfxr exports
#define EXIT_FAILURE_INVALIDARGS    MAKE_NETCORECHECK_HRESULT(4)  // Invalid Arguments
#define EXIT_FAILURE_TEMPRTJSONPATH MAKE_NETCORECHECK_HRESULT(5)  // Failed to construct temp json file path
#define EXIT_FAILURE_TEMPRTJSONFile MAKE_NETCORECHECK_HRESULT(6)  // Failed to create temp json file

int CheckRuntime(LPCWSTR frameworkName, LPCWSTR frameworkVersion, LPCWSTR rollForwardPolicy, LPCWSTR existingRuntimeConfigPath, bool useTempDirectory);