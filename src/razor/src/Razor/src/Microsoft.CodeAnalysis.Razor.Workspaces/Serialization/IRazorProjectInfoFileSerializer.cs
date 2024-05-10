// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.ProjectSystem;

namespace Microsoft.CodeAnalysis.Razor.Serialization;

internal interface IRazorProjectInfoFileSerializer
{
    Task<string> SerializeToTempFileAsync(RazorProjectInfo projectInfo, CancellationToken cancellationToken);
    Task<RazorProjectInfo> DeserializeFromFileAndDeleteAsync(string filePath, CancellationToken cancellationToken);
}
