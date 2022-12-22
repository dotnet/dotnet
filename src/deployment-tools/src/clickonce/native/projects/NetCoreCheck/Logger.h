// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma once

#include <Windows.h>

class Logger
{
public:
    virtual void Log(LPCWSTR pszFormat, ...) const noexcept = 0;

protected:
    void LogStart()
    {
        Log(L"============= NetCoreCheck Start ===============");
    }

    void LogEnd()
    {
        Log(L"=============  NetCoreCheck End  ===============");
    }
};
