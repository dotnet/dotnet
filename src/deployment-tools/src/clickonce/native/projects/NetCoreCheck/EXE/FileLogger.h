// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma once

#include "NetCoreCheckExe.h"

class FileLogger : public Logger
{
public:
    void Initialize(LPCWSTR logFilePath);
    void Log(LPCWSTR pszFormat, ...) const noexcept;

    FileLogger (void) noexcept;
    ~FileLogger (void) noexcept;

private:
    FILE *m_file;
};
