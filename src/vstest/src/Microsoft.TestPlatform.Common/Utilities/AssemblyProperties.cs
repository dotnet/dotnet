// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection.PortableExecutable;

using Microsoft.VisualStudio.TestPlatform.Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.Common.Utilities;

public class AssemblyProperties : IAssemblyProperties
{
    private readonly IFileHelper _fileHelper;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyProperties"/> class.
    /// </summary>
    public AssemblyProperties() : this(new FileHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyProperties"/> class.
    /// </summary>
    /// <param name="fileHelper">File helper.</param>
    public AssemblyProperties(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    /// <summary>
    /// Determines assembly type from file.
    /// </summary>
    public AssemblyType GetAssemblyType(string filePath)
    {
        var assemblyType = AssemblyType.None;

        try
        {
            using var fileStream = _fileHelper.GetStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var peReader = new PEReader(fileStream);
            // Resources for PEReader:
            // 1. https://msdn.microsoft.com/library/windows/desktop/ms680547(v=vs.85).aspx?id=19509
            // 2. https://github.com/dotnet/runtime/tree/master/src/libraries/System.Reflection.Metadata

            var peHeaders = peReader.PEHeaders;
            var corHeader = peHeaders.CorHeader;
            var corHeaderStartOffset = peHeaders.CorHeaderStartOffset;

            assemblyType = (corHeader != null && corHeaderStartOffset >= 0) ?
                AssemblyType.Managed :
                AssemblyType.Native;
        }
        catch (Exception ex)
        {
            EqtTrace.Warning("PEReaderHelper.GetAssemblyType: failed to determine assembly type: {0} for assembly: {1}", ex, filePath);
        }

        EqtTrace.Info("PEReaderHelper.GetAssemblyType: Determined assemblyType:'{0}' for source: '{1}'", assemblyType, filePath);

        return assemblyType;
    }
}
