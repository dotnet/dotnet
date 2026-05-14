// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;

/// <summary>
/// The set of constants used throughout this project.
/// </summary>
public class Constants
{
    // Replace this collection with a list of adapters we want to allow from the "Extensions" folder in Next Major VS Release.
    internal static readonly IList<string> DefaultAdapters = new ReadOnlyCollection<string>(new List<string>
    {
        "executor://CodedWebTestAdapter/v1",
        "executor://MSTestAdapter/v1",
        "executor://WebTestAdapter/v1",
        "executor://CppUnitTestExecutor/v1"
    });

    internal static string DefaultAdapterLocation = Path.Combine(new ProcessHelper().GetCurrentProcessLocation(), "Extensions");
}
