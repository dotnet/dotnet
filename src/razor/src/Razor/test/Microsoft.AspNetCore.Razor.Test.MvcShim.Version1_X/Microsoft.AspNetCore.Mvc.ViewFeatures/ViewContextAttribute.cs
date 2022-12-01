// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

#nullable disable

using System;

namespace Microsoft.AspNetCore.Mvc.ViewFeatures;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class ViewContextAttribute : Attribute
{
}
