// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma once

#include <verrsrc.h>

#define BUILD_VERSION_STR_BASE( v1, v2, v3, v4 )	#v1 "." #v2 "." #v3 "." #v4
#define BUILD_VERSION_STR( v1, v2, v3, v4 )	BUILD_VERSION_STR_BASE( v1, v2, v3, v4 )

#define VER_PRODUCT_MAJOR           1
#define VER_PRODUCT_MINOR           0
#define VER_BUILD_MAJOR             2
#define VER_BUILD_MINOR             0

#define VER_FILEVERSION             VER_PRODUCT_MAJOR,VER_PRODUCT_MINOR,VER_BUILD_MAJOR,VER_BUILD_MINOR
#define VER_FILEVERSION_STR         BUILD_VERSION_STR(VER_PRODUCT_MAJOR,VER_PRODUCT_MINOR,VER_BUILD_MAJOR,VER_BUILD_MINOR)
#define VER_PRODUCTVERSION          VER_FILEVERSION
#define VER_PRODUCTVERSION_STR      VER_FILEVERSION_STR

#define VER_COMPANYNAME_STR         "Microsoft Corporation"
#define VER_LEGALCOPYRIGHT_STR      "Copyright (C) Microsoft Corporation. All rights reserved."

#define VER_PRODUCTNAME_STR         "Microsoft NetCoreCheck Custom Actions"
#define VER_FILEDESCRIPTION_STR     "Microsoft .NET Core Custom Actions"

#ifdef VER_INTERNALNAME_STR
#undef  VER_INTERNALNAME_STR
#endif
#define VER_INTERNALNAME_STR        "NetCoreCheckCA.dll"
#define VER_ORIGINALFILENAME_STR    "NetCoreCheckCA.dll"

#ifndef DEBUG
#define VER_DEBUG                   0
#else
#define VER_DEBUG                   VS_FF_DEBUG
#endif
