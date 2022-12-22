// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Pex.Framework.Packages;
using MSTestv2IntelliTestExtension;

[assembly: PexPackageType(typeof(MSTestv2TestFrameworkPackage))]
namespace MSTestv2IntelliTestExtension
{
    [MSTestv2TestFrameworkPackage]
    internal static class MSTestv2TestFrameworkPackage
    {
    }
}
