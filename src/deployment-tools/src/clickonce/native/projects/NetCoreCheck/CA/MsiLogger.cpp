// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "MsiLogger.h"

MsiLogger::MsiLogger(MsiWrapper *msiWrapper) noexcept : m_msiWrapper(msiWrapper)
{
    LogStart();
}

MsiLogger::~MsiLogger(void) noexcept
{
    LogEnd();
}

void MsiLogger::Log(LPCWSTR format, ...) const noexcept
{
    WCHAR buffer[LOG_BUFFER];

    va_list args;
    va_start(args, format);
    StringCchVPrintfW(buffer, _countof(buffer), format, args);
    va_end(args);

    if (m_msiWrapper)
    {
        m_msiWrapper->Log(buffer);
    }
}
