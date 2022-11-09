// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "FileLogger.h"

#define DEFAULT_LOG_FILE_NAME_FORMAT L"dd_NetCoreCheck_%I64u.log"

FileLogger::FileLogger() noexcept : m_file(NULL)
{
}

void FileLogger::Initialize(LPCWSTR filePath)
{
    WCHAR logFilePath[MAX_PATH];
    if (filePath)
    {
        // If log path was passed in as a parameter, use it
        wcscpy(logFilePath, filePath);
    }
    else
    {
        // Use default log-file path under %TEMP%.
        DWORD len = ::GetTempPath(MAX_PATH, logFilePath);
        if (len != 0)
        {
            if (logFilePath[len - 1] != L'\\')
            {
                ::_tcscat_s(logFilePath, MAX_PATH, TEXT("\\"));
            }

            WCHAR fileName[MAX_PATH];
            ::_stprintf_s(fileName, MAX_PATH, DEFAULT_LOG_FILE_NAME_FORMAT, GetTickCount64());
            ::_tcscat_s(logFilePath, MAX_PATH, fileName);
        }
    }

    ::_tfopen_s(&m_file, (LPCWSTR)logFilePath, L"a+");
    LogStart();
}

FileLogger::~FileLogger(void) noexcept
{
    LogEnd();
    if (m_file)
    {
        ::fclose(m_file);
        m_file = NULL;
    }
}

void FileLogger::Log(LPCWSTR format, ...) const noexcept
{
    if (!m_file)
    {
        // Instead of having the tool fail if we were unable to create the
        // log file we'll just have all logging calls silently fail.
        return;
    }

    // Start the log line with a timestamp
    const size_t dateTimeCharCount = ARRAYSIZE(TEXT("07/01/20"));
    WCHAR date[dateTimeCharCount] = { TEXT('\0') };
    WCHAR time[dateTimeCharCount] = { TEXT('\0') };
    ::_tstrdate_s(date, dateTimeCharCount);
    ::_tstrtime_s(time, dateTimeCharCount);
    ::_ftprintf(m_file, TEXT("[%s,%s] "), date, time);

    // Then add the formatted message
    va_list args;
    va_start(args, format);
    ::_vftprintf_p(m_file, format, args);
    va_end(args);

    // Finally, start a new line and flush the buffer
    ::_ftprintf(m_file, TEXT("\n"));
    ::fflush(m_file);
}
