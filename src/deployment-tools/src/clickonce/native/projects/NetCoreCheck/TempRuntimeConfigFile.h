// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma once

#include "NetCoreCheck.h"

DWORD GetTempRuntimeConfigPath(LPWSTR runtimeConfigPath, bool useTempDirectory);
DWORD CreateTempRuntimeConfigFile(LPCWSTR runtimeConfigPath, LPCWSTR frameworkName, LPCWSTR frameworkVersion, LPCWSTR rollForwardPolicy);
