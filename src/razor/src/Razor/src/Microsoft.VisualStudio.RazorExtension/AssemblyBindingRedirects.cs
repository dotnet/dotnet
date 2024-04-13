// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using Microsoft.VisualStudio.Shell;

[assembly: ProvideBindingRedirection(
    AssemblyName = "Microsoft.Extensions.DependencyInjection",
    GenerateCodeBase = true,
    PublicKeyToken = "adb9793829ddae60",
    OldVersionLowerBound = "0.0.0.0",
    OldVersionUpperBound = "6.0.0.0",
    NewVersion = "6.0.0.0")]
[assembly: ProvideBindingRedirection(
    AssemblyName = "Microsoft.Extensions.DependencyInjection.Abstractions",
    GenerateCodeBase = true,
    PublicKeyToken = "adb9793829ddae60",
    OldVersionLowerBound = "0.0.0.0",
    OldVersionUpperBound = "6.0.0.0",
    NewVersion = "6.0.0.0")]
[assembly: ProvideBindingRedirection(
    AssemblyName = "Microsoft.Extensions.ObjectPool",
    GenerateCodeBase = true,
    PublicKeyToken = "adb9793829ddae60",
    OldVersionLowerBound = "0.0.0.0",
    OldVersionUpperBound = "8.0.0.0",
    NewVersion = "8.0.0.0")]
