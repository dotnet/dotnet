// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.VisualStudio.LanguageServer.ContainedLanguage.MessageInterception;

internal interface IInterceptMethodMetadata
{
    // this must match the name from InterceptMethodAttribute
    IEnumerable<string> InterceptMethods { get; }

    // this must match the name from ContentTypeAttribute
    IEnumerable<string> ContentTypes { get; }
}
