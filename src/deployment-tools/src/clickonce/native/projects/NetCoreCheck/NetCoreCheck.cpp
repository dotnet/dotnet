// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "NetCoreCheck.h"
#include "TempRuntimeConfigFile.h"

#include "..\hostfxr.h"
#include "..\nethost.h"

#include <vector>

// Forward declarations
void* GetExport(HMODULE h, const char* name);
int Exit(int ret);

// Globals
extern Logger *g_log;
HMODULE g_hostfxrLibrary = NULL;

int CheckRuntime(LPCWSTR frameworkName, LPCWSTR frameworkVersion, LPCWSTR rollForwardPolicy, LPCWSTR existingRuntimeConfigPath, bool useTempDirectory)
{
    WCHAR hostfxrPath[MAX_PATH];
    size_t bufferSize = sizeof(hostfxrPath) / sizeof(WCHAR);
    int rc = get_hostfxr_path(hostfxrPath, &bufferSize, nullptr);
    if (rc != 0)
    {
        g_log->Log(TEXT("get_hostfxr_path failed: '%d'"), rc);
        return Exit(EXIT_FAILURE_LOADHOSTFXR);
    }

    g_log->Log(TEXT("Found HostFxr: '%s'"), hostfxrPath);
    
    // Load hostfxr and get desired exports
    g_hostfxrLibrary = ::LoadLibraryW(hostfxrPath);
    if (!g_hostfxrLibrary)
    {
        g_log->Log(TEXT("Failed to load library '%s', error = '%d'"), hostfxrPath, ::GetLastError());
        return Exit(EXIT_FAILURE_HOSTFXREXPORTS);
    }
    
    hostfxr_initialize_for_runtime_config_fn initFptr = (hostfxr_initialize_for_runtime_config_fn)GetExport(g_hostfxrLibrary, "hostfxr_initialize_for_runtime_config");
    hostfxr_close_fn closeFptr = (hostfxr_close_fn)GetExport(g_hostfxrLibrary, "hostfxr_close");
    if (!initFptr || !closeFptr)
    {
        g_log->Log(TEXT("Failed to get exports from hostfxr."));
        return Exit(EXIT_FAILURE_HOSTFXREXPORTS);
    }

    WCHAR runtimeConfigPath[MAX_PATH];
    if (NULL != existingRuntimeConfigPath)
    {
        wcscpy(runtimeConfigPath, existingRuntimeConfigPath);
        g_log->Log(TEXT("Using existing runtimeconfig file '%s'"), runtimeConfigPath);
    }
    else
    {
        g_log->Log(TEXT("Framework Name:      '%s'"), frameworkName);
        g_log->Log(TEXT("Framework Version:   '%s'"), frameworkVersion);
        g_log->Log(TEXT("Roll Forward Policy: '%s'"), (rollForwardPolicy && (wcslen(rollForwardPolicy) > 0)) ? rollForwardPolicy : TEXT("(Default)"));

        DWORD ret = GetTempRuntimeConfigPath(runtimeConfigPath, useTempDirectory);
        if (ret != 0)
        {
            g_log->Log(TEXT("Failed to get runtime config file path."));
            return Exit(ret);
        }

        ret = CreateTempRuntimeConfigFile(runtimeConfigPath, frameworkName, frameworkVersion, rollForwardPolicy);
        if (ret != 0)
        {
            g_log->Log(TEXT("Failed to create temp runtime config file."));
            return Exit(ret);
        }
    }

    hostfxr_handle cxt = nullptr;
    g_log->Log(TEXT("Calling hostfxr_initialize_for_runtime_config..."));
    rc = initFptr(runtimeConfigPath, nullptr, &cxt);
    if (rc != 0 || cxt == nullptr)
    {
        g_log->Log(TEXT("hostfxr_initialize_for_runtime_config failed: '%d'"), rc);
        closeFptr(cxt);
        return Exit(EXIT_FAILURE_INITHOSTFXR);
    }

    g_log->Log(TEXT("hostfxr_initialize_for_runtime_config succeeded."));

    rc = closeFptr(cxt);
    if (rc != 0)
    {
        g_log->Log(TEXT("hostfxr_close failed: '%d'"), rc);
    }

    return Exit(EXIT_SUCCESS);
}

void* GetExport(HMODULE h, const char* name)
{
    void* address = ::GetProcAddress(h, name);
    if (!address)
    {
        DWORD err = ::GetLastError();
        const size_t size = strlen(name) + 1;
        std::vector<WCHAR> wName;
        wName.resize(size);
        mbstowcs(wName.data(), name, size);
        g_log->Log(TEXT("Failed to load library '%s', error = '%d'"), wName.data(), err);
    }
    
    return address;
}

int Exit(int ret)
{
    if (g_hostfxrLibrary != NULL && (!::FreeLibrary(g_hostfxrLibrary)))
    {
        g_log->Log(TEXT("FreeLibrary failed."));
    }

    return ret;
}
