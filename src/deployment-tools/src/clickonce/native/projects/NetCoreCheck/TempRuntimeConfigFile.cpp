// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "TempRuntimeConfigFile.h"

#define RUNTIMECONFIG_TEXT_FORMAT_STR TEXT("{ \"runtimeOptions\": { \"framework\": { \"name\": \"%s\", \"version\": \"%s\" } } }")
#define RUNTIMECONFIG_TEXT_FORMAT_STR_WITH_ROLL_FORWARD TEXT("{ \"runtimeOptions\": { \"rollForward\": \"%s\", \"framework\": { \"name\": \"%s\", \"version\": \"%s\" } } }")
#define RUNTIMECONFIG_NAME_FORMAT_STR TEXT("Test%I64u.runtimeconfig.json")

// Forward declarations
DWORD WriteFile(LPCWSTR filePath, LPCWSTR fileText);

// Globals
extern Logger *g_log;

DWORD GetTempRuntimeConfigPath(LPWSTR runtimeConfigPath, bool useTempDirectory)
{
    WCHAR fileName[MAX_PATH];
    ::_stprintf_s(fileName, MAX_PATH, RUNTIMECONFIG_NAME_FORMAT_STR, GetTickCount64());

    if (useTempDirectory)
    {
        DWORD len = ::GetTempPath(MAX_PATH, runtimeConfigPath);
        if (len == 0)
        {
            g_log->Log(TEXT("Couldn't get temp path."));
            return EXIT_FAILURE_TEMPRTJSONPATH;
        }
    }
    else
    {
        if (!GetModuleFileName(NULL, runtimeConfigPath, MAX_PATH))
        {
            g_log->Log(TEXT("Couldn't get module name."));
            return ::GetLastError();
        }

        PathRemoveFileSpec(runtimeConfigPath);
    }

    if (!PathAppend(runtimeConfigPath, fileName))
    {
        g_log->Log(TEXT("Couldn't append file."));
        return EXIT_FAILURE_TEMPRTJSONPATH;
    }

    g_log->Log(TEXT("Temporary runtime config file path: '%s'."), runtimeConfigPath);
    return 0;
}

DWORD CreateTempRuntimeConfigFile(LPCWSTR runtimeConfigPath, LPCWSTR frameworkName, LPCWSTR frameworkVersion, LPCWSTR rollForwardPolicy)
{
    if (PathFileExists(runtimeConfigPath))
    {
        if (!DeleteFile(runtimeConfigPath))
        {
            g_log->Log(TEXT("Failed to delete existing file '%s'."), runtimeConfigPath);
            return EXIT_FAILURE_TEMPRTJSONFile;
        }
    }

    WCHAR fileText[MAX_PATH];
    int ret = (NULL == rollForwardPolicy || (wcslen(rollForwardPolicy) == 0)) ?
        swprintf_s(fileText, MAX_PATH, RUNTIMECONFIG_TEXT_FORMAT_STR, frameworkName, frameworkVersion) :
        swprintf_s(fileText, MAX_PATH, RUNTIMECONFIG_TEXT_FORMAT_STR_WITH_ROLL_FORWARD, rollForwardPolicy, frameworkName, frameworkVersion);

    if (ret <= 0)
    {
        g_log->Log(TEXT("Failed to format file text."));
        return EXIT_FAILURE_TEMPRTJSONFile;
    }

    g_log->Log(TEXT("Temp runtime config file text: '%s'."), fileText);
    return WriteFile(runtimeConfigPath, fileText);
}

DWORD WriteFile(LPCWSTR filePath, LPCWSTR fileText)
{
    FILE* file;
    DWORD ret = ::_tfopen_s(&file, filePath, L"a+");
    if (0 != ret)
    {
        g_log->Log(TEXT("Open file failed : '%s'."), ret);
        return ret;
    }

    ::_ftprintf(file, fileText);
    ::_ftprintf(file, TEXT("\n"));
    ::fflush(file);
    ::fclose(file);

    return 0;
}
