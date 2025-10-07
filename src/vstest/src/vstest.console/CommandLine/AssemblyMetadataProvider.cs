// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.Versioning;
using System.Text;

using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;

namespace Microsoft.VisualStudio.TestPlatform.CommandLine.Processors.Utilities;

internal class AssemblyMetadataProvider : IAssemblyMetadataProvider
{
    private static AssemblyMetadataProvider? s_instance;
    private readonly IFileHelper _fileHelper;

    /// <summary>
    /// Gets the instance.
    /// </summary>
    public static AssemblyMetadataProvider Instance => s_instance ??= new AssemblyMetadataProvider(new FileHelper());

    internal AssemblyMetadataProvider(IFileHelper fileHelper)
    {
        _fileHelper = fileHelper;
    }

    /// <inheritdoc />
    public FrameworkName GetFrameworkName(string filePath)
    {
        FrameworkName frameworkName = new(Framework.DefaultFramework.Name);
        try
        {
            using var assemblyStream = _fileHelper.GetStream(filePath, FileMode.Open, FileAccess.Read);
            frameworkName = GetFrameworkNameFromAssemblyMetadata(assemblyStream);
        }
        catch (Exception ex)
        {
            EqtTrace.Warning("AssemblyMetadataProvider.GetFrameworkName: failed to determine TargetFrameworkVersion exception: {0} for assembly: {1}", ex, filePath);
        }

        EqtTrace.Info("AssemblyMetadataProvider.GetFrameworkName: Determined framework:'{0}' for source: '{1}'", frameworkName, filePath);

        return frameworkName;
    }

    /// <inheritdoc />
    public Architecture GetArchitecture(string assemblyPath)
    {
        Architecture archType = Architecture.AnyCPU;
        try
        {
            // AssemblyName won't load the assembly into current domain.
            var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);

            var processorArchitecture =
#if NET7_0_OR_GREATER
                // AssemblyName doesn't include ProcessorArchitecture in net7.
                // It will always be ProcessorArchitecture.None.
                ProcessorArchitecture.None;
#else
                assemblyName.ProcessorArchitecture;
#endif

            archType = MapToArchitecture(processorArchitecture, assemblyPath);
        }
        catch (Exception ex)
        {
            // AssemblyName will throw Exception if assembly contains native code or no manifest.

            EqtTrace.Verbose("AssemblyMetadataProvider.GetArchitecture: Failed get ProcessorArchitecture using AssemblyName API with exception: {0}", ex);

            try
            {
                archType = GetArchitectureForSource(assemblyPath);
            }
            catch (Exception e)
            {
                EqtTrace.Info("AssemblyMetadataProvider.GetArchitecture: Failed to determine Assembly Architecture with exception: {0}", e);
            }
        }

        EqtTrace.Info("AssemblyMetadataProvider.GetArchitecture: Determined architecture:{0} info for assembly: {1}", archType,
            assemblyPath);

        return archType;
    }

    private Architecture GetArchitectureFromAssemblyMetadata(string path)
    {
        Architecture arch = Architecture.AnyCPU;
        using (Stream stream = _fileHelper.GetStream(path, FileMode.Open, FileAccess.Read))
        using (PEReader peReader = new(stream))
        {
            switch (peReader.PEHeaders.CoffHeader.Machine)
            {
                case Machine.Amd64:
                case Machine.IA64:
                    return Architecture.X64;
                case Machine.Arm64:
                    return Architecture.ARM64;
                case Machine.Arm:
                    return Architecture.ARM;
                case Machine.I386:
                    // We can distinguish AnyCPU only from the set of CorFlags.Requires32Bit, but in case of Ready
                    // to Run image that flag is not "updated" and ignored. So we check if the module is IL only or not.
                    // If it's not IL only it means that is a R2R (Ready to Run) and we're already in the correct architecture x86.
                    // In all other cases the architecture will end inside the correct switch branch.
                    var corflags = peReader.PEHeaders.CorHeader?.Flags;
                    return (corflags & CorFlags.Requires32Bit) != 0 || (corflags & CorFlags.ILOnly) == 0
                        ? Architecture.X86 : Architecture.AnyCPU;
                default:
                    {
                        EqtTrace.Error($"AssemblyMetadataProvider.GetArchitecture: Unhandled architecture '{peReader.PEHeaders.CoffHeader.Machine}'.");
                        break;
                    }
            }
        }

        return arch;
    }

    private static FrameworkName GetFrameworkNameFromAssemblyMetadata(Stream assemblyStream)
    {
        FrameworkName frameworkName = new(Framework.DefaultFramework.Name);
        using (var peReader = new PEReader(assemblyStream))
        {
            var metadataReader = peReader.GetMetadataReader();

            foreach (var customAttributeHandle in metadataReader.CustomAttributes)
            {
                var attr = metadataReader.GetCustomAttribute(customAttributeHandle);
                var result = Encoding.UTF8.GetString(metadataReader.GetBlobBytes(attr.Value));
                if (result.Contains(".NET") && result.Contains(",Version="))
                {
                    var fxStartIndex = result.IndexOf(".NET", StringComparison.Ordinal);
                    var fxEndIndex = result.IndexOf("\u0001", fxStartIndex, StringComparison.Ordinal);
                    if (fxStartIndex > -1 && fxEndIndex > fxStartIndex)
                    {
                        // Using -3 because custom attribute values separated by unicode characters.
                        result = result.Substring(fxStartIndex, fxEndIndex - 3);
                        frameworkName = new FrameworkName(result);
                        break;
                    }
                }
            }
        }

        return frameworkName;
    }

    private Architecture MapToArchitecture(ProcessorArchitecture processorArchitecture, string assemblyPath)
    {
        Architecture arch = Architecture.AnyCPU;
        // Mapping to Architecture based on https://msdn.microsoft.com/en-us/library/system.reflection.processorarchitecture(v=vs.110).aspx

        if (processorArchitecture.Equals(ProcessorArchitecture.Amd64)
            || processorArchitecture.Equals(ProcessorArchitecture.IA64))
        {
            arch = Architecture.X64;
        }
        else if (processorArchitecture.Equals(ProcessorArchitecture.X86))
        {
            arch = Architecture.X86;
        }
        else if (processorArchitecture.Equals(ProcessorArchitecture.MSIL))
        {
            arch = Architecture.AnyCPU;
        }
        else if (processorArchitecture.Equals(ProcessorArchitecture.Arm))
        {
            arch = Architecture.ARM;
        }
        else if (processorArchitecture.Equals(ProcessorArchitecture.None))
        {
            // In case of None we fallback to PEReader
            // We don't use only PEReader for back compatibility.
            // An AnyCPU returned by AssemblyName.GetAssemblyName(assemblyPath) will result in a I386 for PEReader.
            // Also MSIL processor architecture is missing with PEReader.
            // For now it should fix the issue for the missing ARM64 architecture.
            arch = GetArchitectureFromAssemblyMetadata(assemblyPath);
        }
        else
        {
            EqtTrace.Info("Unable to map to Architecture, using platform: {0}", arch);
        }

        return arch;
    }

    public Architecture GetArchitectureForSource(string imagePath)
    {
        // For details refer to below code available on MSDN.
        //https://code.msdn.microsoft.com/windowsapps/CSCheckExeType-aab06100#content

        var archType = Architecture.AnyCPU;
        ushort machine = 0;

        uint peHeader;
        const int imageFileMachineAmd64 = 0x8664;
        const int imageFileMachineIa64 = 0x200;
        const int imageFileMachineI386 = 0x14c;
        const int imageFileMachineArm = 0x01c0; // ARM Little-Endian
        const int imageFileMachineThumb = 0x01c2; // ARM Thumb/Thumb-2 Little-Endian
        const int imageFileMachineArmnt = 0x01c4; // ARM Thumb-2 Little-Endian
        const int imageFileMachineArm64 = 0xAA64; // ARM64 Little-Endian

        try
        {
            //get the input stream
            using Stream fs = _fileHelper.GetStream(imagePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fs);
            var validImage = true;

            //PE Header starts @ 0x3C (60). Its a 4 byte header.
            fs.Position = 0x3C;
            peHeader = reader.ReadUInt32();

            // Check if the offset is invalid
            if (peHeader > fs.Length - 5)
            {
                validImage = false;
            }

            if (validImage)
            {
                //Moving to PE Header start location...
                fs.Position = peHeader;

                var signature = reader.ReadUInt32(); //peHeaderSignature
                // 0x00004550 is the letters "PE" followed by two terminating zeros.
                if (signature != 0x00004550)
                {
                    validImage = false;
                }

                if (validImage)
                {
                    //Read the image file header.
                    machine = reader.ReadUInt16();
                    reader.ReadUInt16(); //NumberOfSections
                    reader.ReadUInt32(); //TimeDateStamp
                    reader.ReadUInt32(); //PointerToSymbolTable
                    reader.ReadUInt32(); //NumberOfSymbols
                    reader.ReadUInt16(); //SizeOfOptionalHeader
                    reader.ReadUInt16(); //Characteristics

                    // magic number.32bit or 64bit assembly.
                    var magic = reader.ReadUInt16();
                    if (magic is not 0x010B and not 0x020B)
                    {
                        validImage = false;
                    }
                }

                if (validImage)
                {
                    switch (machine)
                    {
                        case imageFileMachineI386:
                            archType = Architecture.X86;
                            break;

                        case imageFileMachineAmd64:
                        case imageFileMachineIa64:
                            archType = Architecture.X64;
                            break;

                        case imageFileMachineArm:
                        case imageFileMachineThumb:
                        case imageFileMachineArmnt:
                            archType = Architecture.ARM;
                            break;

                        case imageFileMachineArm64:
                            archType = Architecture.ARM64;
                            break;
                    }
                }
                else
                {
                    EqtTrace.Info(
                        "GetArchitectureForSource: Source path {0} is not a valid image path. Returning default proc arch type: {1}.",
                        imagePath, archType);
                }
            }
        }
        catch (Exception ex)
        {
            //Ignore all exception
            EqtTrace.Info(
                "GetArchitectureForSource: Returning default:{0}. Unhandled exception: {1}.",
                archType, ex);
        }

        return archType;
    }
}
