// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#include "MsiWrapper.h"

MsiWrapper::MsiWrapper(MSIHANDLE msiHandle) noexcept
{
    m_msiHandle = msiHandle;
}

MsiWrapper::~MsiWrapper(void) noexcept
{
}

HRESULT MsiWrapper::GetProperty(LPCWSTR propertyName, LPWSTR* propertyValue)
{
    if (!propertyName || !*propertyName || !propertyValue)
    {
        return E_INVALIDARG;
    }

    DWORD_PTR count = 0;
    WCHAR empty[1] = L"";
    UINT er = ::MsiGetPropertyW(m_msiHandle, propertyName, empty, (DWORD *)&count);
    if (ERROR_MORE_DATA == er || ERROR_SUCCESS == er)
    {
        *propertyValue = new WCHAR[++count];
    }
    else
    {
        return HRESULT_FROM_WIN32(er);
    }

    er = ::MsiGetPropertyW(m_msiHandle, propertyName, *propertyValue, (DWORD *)&count);
    if (er == ERROR_SUCCESS)
    {
        return S_OK;
    }
    else
    {
        FreeStr(*propertyValue);
        return  HRESULT_FROM_WIN32(er);
    }
}

HRESULT MsiWrapper::SetProperty(LPCWSTR propertyName, LPCWSTR propertyValue)
{
    if (!propertyName || !*propertyName || !propertyValue)
    {
        return E_INVALIDARG;
    }

    UINT er = ::MsiSetPropertyW(m_msiHandle, propertyName, propertyValue);
    return er == ERROR_SUCCESS ? S_OK : HRESULT_FROM_WIN32(er);
}

void MsiWrapper::Log(LPCWSTR msg) const noexcept
{
    PMSIHANDLE record = MsiCreateRecord(1);
    if (record)
    {
        ::MsiRecordSetStringW(record, 0, msg);
        ::MsiProcessMessage(m_msiHandle, INSTALLMESSAGE_INFO, record);
    }
}

void MsiWrapper::LogFailure(HRESULT hr, LPCWSTR format, ...) const noexcept
{
    WCHAR failureMessage[LOG_BUFFER];

    va_list args;
    va_start(args, format);
    StringCchVPrintfW(failureMessage, _countof(failureMessage), format, args);
    va_end(args);

    WCHAR buffer[LOG_BUFFER];
    StringCchPrintfW(buffer, _countof(buffer), L"FAILURE: 0x%x. %s", hr, failureMessage);
    Log(buffer);
}
