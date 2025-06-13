// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;

namespace Microsoft.CodeAnalysis.Razor.Completion;

internal static class VSInternalCompletionSettingExtensions
{
    public static bool SupportsCompletionListData(this VSInternalCompletionSetting? completionSetting)
    {
        return completionSetting?.CompletionList?.Data == true ||
            completionSetting?.CompletionListSetting?.ItemDefaults?.Contains("data") == true;
    }
}
