// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Diagnostics;
using Microsoft.VisualStudio.LanguageServer.Protocol;

[assembly: DebuggerDisplay("{Label} ({Kind})", Target = typeof(CompletionItem))]
