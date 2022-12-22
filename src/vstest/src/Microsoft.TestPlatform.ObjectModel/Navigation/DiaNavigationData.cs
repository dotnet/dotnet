// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel;

/// <summary>
/// A struct that stores the information needed by the navigation: file name, line number, column number.
/// </summary>
public class DiaNavigationData : INavigationData
{
    public string? FileName { get; set; }

    public int MinLineNumber { get; set; }

    public int MaxLineNumber { get; set; }

    public DiaNavigationData(string? fileName, int minLineNumber, int maxLineNumber)
    {
        FileName = fileName;
        MinLineNumber = minLineNumber;
        MaxLineNumber = maxLineNumber;
    }
}
