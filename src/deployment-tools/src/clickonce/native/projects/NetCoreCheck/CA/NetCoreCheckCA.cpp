// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "NetCoreCheckCA.h"
#include "MsiLogger.h"

#define CHECKNETRUNTIME_PARAM_FRAMEWORK_PROPERTY_NAME           L"CheckNETRuntime_Framework"
#define CHECKNETRUNTIME_PARAM_VERSION_PROPERTY_NAME             L"CheckNETRuntime_Version"
#define CHECKNETRUNTIME_PARAM_ROLL_FORWARD_POLICY_PROPERTY_NAME L"CheckNETRuntime_RollForwardPolicy"
#define CHECKNETRUNTIME_RESULT_PROPERTY_NAME                    L"CheckNETRuntime_Result"

#define ExitOnFailure(x, s, ...)   if (FAILED(x)) { msiWrapper.LogFailure(x, s, __VA_ARGS__);  goto Exit; }

// Globals
Logger *g_log;

UINT __stdcall CheckNETRuntime(MSIHANDLE hInstall)
{
    int ret = EXIT_SUCCESS;
    LPWSTR frameworkName = NULL;
    LPWSTR frameworkVersion = NULL;
    LPWSTR rollForwardPolicy = NULL;
    MsiWrapper msiWrapper(hInstall);

    MsiLogger logger(&msiWrapper);
    g_log = &logger;

    // Read input properties
    HRESULT hr = msiWrapper.GetProperty(CHECKNETRUNTIME_PARAM_FRAMEWORK_PROPERTY_NAME, &frameworkName);
    ExitOnFailure(hr, L"Failed to read framework name from property '%s'.", CHECKNETRUNTIME_PARAM_FRAMEWORK_PROPERTY_NAME);
    if (NULL == frameworkName || 0 == wcslen(frameworkName))
    {
        ExitOnFailure(hr = E_INVALIDARG, L"Missing framework name property '%s'.", CHECKNETRUNTIME_PARAM_FRAMEWORK_PROPERTY_NAME);
    }

    hr = msiWrapper.GetProperty(CHECKNETRUNTIME_PARAM_VERSION_PROPERTY_NAME, &frameworkVersion);
    ExitOnFailure(hr, L"Failed to read framework version from property '%s'.", CHECKNETRUNTIME_PARAM_VERSION_PROPERTY_NAME);
    if (NULL == frameworkVersion || 0 == wcslen(frameworkVersion))
    {
        ExitOnFailure(hr = E_INVALIDARG, L"Missing framework version property '%s'.", CHECKNETRUNTIME_PARAM_VERSION_PROPERTY_NAME);
    }

    // Roll forward policy is optional, so no need to verify the value
    hr = msiWrapper.GetProperty(CHECKNETRUNTIME_PARAM_ROLL_FORWARD_POLICY_PROPERTY_NAME, &rollForwardPolicy);
    ExitOnFailure(hr, L"Failed to read roll forward policy from property '%s'.", CHECKNETRUNTIME_PARAM_ROLL_FORWARD_POLICY_PROPERTY_NAME);

    // Perform runtime check
    ret = CheckRuntime(frameworkName, frameworkVersion, rollForwardPolicy, NULL, true);
    WCHAR result[10];
    _itow_s(ret, result, _countof(result), 10);
    hr = msiWrapper.SetProperty(CHECKNETRUNTIME_RESULT_PROPERTY_NAME, result);
    ExitOnFailure(hr, L"Failed setting result property '%s'.", CHECKNETRUNTIME_RESULT_PROPERTY_NAME);

Exit:
    FreeStr(frameworkName)
    FreeStr(frameworkVersion)
    FreeStr(rollForwardPolicy)

    return (SUCCEEDED(hr) && EXIT_SUCCESS == ret) ? ERROR_SUCCESS : ERROR_INSTALL_FAILURE;
}

extern "C" BOOL WINAPI DllMain(__in HINSTANCE hInst, __in ULONG ulReason, __in LPVOID)
{
    return TRUE;
}
