// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;

/// <summary>
/// The portable pdb reader.
/// </summary>
internal class PortablePdbReader : IDisposable
{
    /// <summary>
    /// Use to get method token
    /// </summary>
    private static readonly PropertyInfo MethodInfoMethodTokenProperty =
        typeof(MethodInfo).GetProperty("MetadataToken")!;

    /// <summary>
    /// Metadata reader provider from portable pdb stream
    /// To get Metadata reader
    /// </summary>
    private readonly MetadataReaderProvider _provider;

    /// <summary>
    /// Metadata reader from portable pdb stream
    /// To get method debug info from method info
    /// </summary>
    private readonly MetadataReader _reader;

    /// <summary>
    /// Initializes a new instance of the <see cref="PortablePdbReader"/> class.
    /// </summary>
    /// <param name="stream">
    /// Portable pdb stream
    /// </param>
    /// <exception cref="Exception">
    /// Raises Exception on given stream is not portable pdb stream
    /// </exception>
    public PortablePdbReader(Stream stream)
    {
        if (!IsPortable(stream))
        {
            throw new Exception("Given stream is not portable stream");
        }

        _provider = MetadataReaderProvider.FromPortablePdbStream(stream);
        _reader = _provider.GetMetadataReader();
    }

    /// <summary>
    /// Reads the pdb using a provided metadata reader, when the pdb is embedded in the dll, or found by
    /// path that is in the dll metadata.
    /// </summary>
    /// <param name="metadataReaderProvider"></param>
    public PortablePdbReader(MetadataReaderProvider metadataReaderProvider)
    {
        _provider = metadataReaderProvider ?? throw new ArgumentNullException(nameof(metadataReaderProvider));
        _reader = _provider.GetMetadataReader();
    }

    /// <summary>
    /// Dispose Metadata reader
    /// </summary>
    public void Dispose()
    {
        _provider?.Dispose();
    }

    /// <summary>
    /// Gets dia navigation data from Metadata reader
    /// </summary>
    /// <param name="methodInfo">
    /// Method info.
    /// </param>
    /// <returns>
    /// The <see cref="DiaNavigationData"/>.
    /// </returns>
    public DiaNavigationData? GetDiaNavigationData(MethodInfo? methodInfo)
    {
        if (methodInfo == null)
        {
            return null;
        }

        var handle = GetMethodDebugInformationHandle(methodInfo);

        return GetDiaNavigationData(handle);
    }

    /// <summary>
    /// Checks gives stream is from portable pdb or not
    /// </summary>
    /// <param name="stream">
    /// Stream.
    /// </param>
    /// <returns>
    /// The <see cref="bool"/>.
    /// </returns>
    internal static bool IsPortable(Stream stream)
    {
        // First four bytes should be 'BSJB'
#pragma warning disable IDE0078 // Use pattern matching (may change code meaning)
        var result = (stream.ReadByte() == 'B') && (stream.ReadByte() == 'S') && (stream.ReadByte() == 'J')
                     && (stream.ReadByte() == 'B');
#pragma warning restore IDE0078 // Use pattern matching (may change code meaning)
        stream.Position = 0;
        return result;
    }

    internal static MethodDebugInformationHandle GetMethodDebugInformationHandle(MethodInfo methodInfo)
    {
        var methodToken = (int)MethodInfoMethodTokenProperty.GetValue(methodInfo)!;
        var handle = ((MethodDefinitionHandle)MetadataTokens.Handle(methodToken)).ToDebugInformationHandle();
        return handle;
    }

    private static void GetMethodMinAndMaxLineNumber(
        MethodDebugInformation methodDebugDefinition,
        out int minLineNumber,
        out int maxLineNumber)
    {
        minLineNumber = int.MaxValue;
        maxLineNumber = int.MinValue;
        var orderedSequencePoints = methodDebugDefinition.GetSequencePoints();
        foreach (var sequencePoint in orderedSequencePoints)
        {
            if (sequencePoint.IsHidden)
            {
                // Special sequence point with startLine is Magic number 0xFEEFEE
                // Magic number comes from Potable CodeGen source code
                continue;
            }
            minLineNumber = Math.Min(minLineNumber, sequencePoint.StartLine);
            maxLineNumber = Math.Max(maxLineNumber, sequencePoint.StartLine);
        }
    }

    private DiaNavigationData? GetDiaNavigationData(MethodDebugInformationHandle handle)
    {
        if (_reader == null)
        {
            throw new ObjectDisposedException(nameof(PortablePdbReader));
        }

        DiaNavigationData? diaNavigationData = null;
        try
        {
            var methodDebugDefinition = _reader.GetMethodDebugInformation(handle);
            var fileName = GetMethodFileName(methodDebugDefinition);
            GetMethodMinAndMaxLineNumber(methodDebugDefinition, out var minLineNumber, out var maxLineNumber);

            diaNavigationData = new DiaNavigationData(fileName, minLineNumber, maxLineNumber);
        }
        catch (BadImageFormatException exception)
        {
            EqtTrace.Error("failed to get dia navigation data: {0}", exception);
        }

        return diaNavigationData;
    }

    private string GetMethodFileName(MethodDebugInformation methodDebugDefinition)
    {
        var fileName = string.Empty;
        if (!methodDebugDefinition.Document.IsNil)
        {
            var document = _reader.GetDocument(methodDebugDefinition.Document);
            fileName = _reader.GetString(document.Name);
        }

        return fileName;
    }
}
