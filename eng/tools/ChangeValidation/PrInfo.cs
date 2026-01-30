// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Immutable;
using System.Linq;

namespace ChangeValidation;

internal record PrInfo(
    string TargetBranch, 
    ImmutableList<string> ChangedFiles);
