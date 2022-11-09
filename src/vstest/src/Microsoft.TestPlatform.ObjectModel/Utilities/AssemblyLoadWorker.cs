// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#if NETFRAMEWORK
using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;

/// <summary>
/// Does the real work of finding references using Assembly.ReflectionOnlyLoadFrom.
/// The caller is supposed to create AppDomain and create instance of given class in there.
/// </summary>
internal class AssemblyLoadWorker : MarshalByRefObject
{
    /// <summary>
    /// Get the target dot net framework string for the assembly
    /// </summary>
    /// <param name="path">Path of the assembly file</param>
    /// <returns> String representation of the target dot net framework e.g. .NETFramework,Version=v4.0 </returns>
    public static string GetTargetFrameworkVersionStringFromPath(string path)
    {
        if (!File.Exists(path))
        {
            return string.Empty;
        }

        try
        {
            var a = Assembly.ReflectionOnlyLoadFrom(path);
            return GetTargetFrameworkStringFromAssembly(a);
        }
        catch (BadImageFormatException)
        {
            EqtTrace.Error("AssemblyLoadWorker:GetTargetFrameworkVersionString() caught BadImageFormatException. Falling to native binary.");
        }
        catch (Exception ex)
        {
            EqtTrace.Error("AssemblyLoadWorker:GetTargetFrameworkVersionString() Returning default. Unhandled exception: {0}.", ex);
        }

        return string.Empty;
    }

    /// <summary>
    /// Get the target dot net framework string for the assembly
    /// </summary>
    /// <param name="assembly">Assembly</param>
    /// <returns>String representation of the target dot net framework e.g. .NETFramework,Version=v4.0 </returns>
    internal static string GetTargetFrameworkStringFromAssembly(Assembly assembly)
    {
        var dotNetVersion = string.Empty;
        foreach (var data in CustomAttributeData.GetCustomAttributes(assembly))
        {
            if (data.NamedArguments?.Count > 0)
            {
                string attributeName = data.NamedArguments[0].MemberInfo.DeclaringType.FullName;
                if (string.Equals(attributeName, Constants.TargetFrameworkAttributeFullName, StringComparison.OrdinalIgnoreCase))
                {
                    dotNetVersion = data.ConstructorArguments[0].Value.ToString();
                    break;
                }
            }
        }

        return dotNetVersion;
    }

    /// <summary>
    /// Returns the full name of the referenced assemblies by the assembly on the specified path.
    ///
    /// Returns null on failure and an empty array if there is no reference in the project.
    /// </summary>
    /// <param name="path">Path to the assembly file to load from.</param>
    public static string[]? GetReferencedAssemblies(string path)
    {
        TPDebug.Assert(!path.IsNullOrEmpty());

        Assembly? a = null;
        try
        {
            // ReflectionOnlyLoadFrom does not use the probing paths and loads from the
            // specified path only and does not let code to be executed by the assembly
            // in the loaded context.
            a = Assembly.ReflectionOnlyLoadFrom(path);
        }
        catch
        {
            return null;
        }
        TPDebug.Assert(a != null);

        AssemblyName[] assemblies = a.GetReferencedAssemblies();
        return assemblies == null || assemblies.Length == 0
            ? (new string[0])
            : (from assembly in assemblies
               select assembly.FullName).ToArray();
    }

    /// <summary>
    /// Returns true if given assembly matched name and public key token.
    /// </summary>
    public static bool? CheckAssemblyReference(string path, string referenceAssemblyName, byte[] publicKeyToken)
    {
        try
        {
            // ReflectionOnlyLoadFrom does not use the probing paths and loads from the
            // specified path only and does not let code to be executed by the assembly
            // in the loaded context.
            //
            Assembly a = Assembly.ReflectionOnlyLoadFrom(path);

            TPDebug.Assert(a != null);

            AssemblyName[] assemblies = a.GetReferencedAssemblies();

            foreach (AssemblyName referencedAssembly in assemblies)
            {
                // Check without version. Only name and public key token.
                if (string.Equals(referencedAssembly.Name, referenceAssemblyName, StringComparison.OrdinalIgnoreCase))
                {
                    byte[] publicKeyToken1 = referencedAssembly.GetPublicKeyToken();

                    bool isMatch = true;
                    if (publicKeyToken1.Length != publicKeyToken.Length)
                    {
                        continue;
                    }

                    for (int i = 0; i < publicKeyToken1.Length; ++i)
                    {
                        if (publicKeyToken1[i] != publicKeyToken[i])
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        return true;
                    }
                }
            }
        }
        catch
        {
            return null; // return null if not able to check.
        }

        return false;
    }

    /// <summary>
    /// Finds platform and .Net framework version for a given container.
    /// In case of errors defaults are returned.
    /// </summary>
    /// <param name="path"></param>
    /// <param name="procArchType"></param>
    /// <param name="frameworkVersion"></param>
    public static void GetPlatformAndFrameworkSettings(string path, out string procArchType, out string frameworkVersion)
    {
        procArchType = nameof(Architecture.Default);
        frameworkVersion = string.Empty;

        try
        {
            // ReflectionOnlyLoadFrom does not use the probing paths and loads from the
            // specified path only and does not let code to be executed by the assembly
            // in the loaded context.

            var a = Assembly.ReflectionOnlyLoadFrom(path);
            TPDebug.Assert(a != null);
            a.ManifestModule.GetPEKind(out var peKind, out var machine);

            // conversion to string type is needed for below reason
            // -- PortableExecutableKinds.Preferred32Bit and ImageFileMachine.ARM is available only
            //    in .Net4.0 and above. Below code is compiled with .Net3.5 but runs in .Net4.0
            string peKindString = peKind.ToString();
            string machineTypeString = machine.ToString();
            if (string.Equals(machineTypeString, "I386", StringComparison.OrdinalIgnoreCase))
            {
                if (peKindString.Contains("Preferred32Bit") || peKindString.Contains("Required32Bit"))
                {
                    procArchType = "X86";
                }
                else if (string.Equals(peKindString, "ILOnly", StringComparison.OrdinalIgnoreCase))
                {
                    procArchType = "AnyCPU";
                }
            }
            else if (string.Equals(machineTypeString, "AMD64", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(machineTypeString, "IA64", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(peKindString, "ILOnly, PE32Plus", StringComparison.OrdinalIgnoreCase))
                {
                    procArchType = "X64";
                }
            }
            else if (string.Equals(machineTypeString, "ARM", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(peKindString, "ILOnly", StringComparison.OrdinalIgnoreCase))
                {
                    procArchType = "ARM";
                }
            }

            if (StringUtils.IsNullOrEmpty(procArchType))
            {
                EqtTrace.Verbose("Unable to find the platform type for image:{0} with PEKind:{1}, Machine:{2}. Returning Default:{3}", path, peKindString, machineTypeString, "AnyCPU");
                procArchType = "AnyCPU";
            }


            frameworkVersion = a.ImageRuntimeVersion.Substring(0, 4).ToUpperInvariant();

            // ImageRuntimeVersion for v4.0 & v4.5 are same and it return v4.0
            // Since there is behavioral difference in both its important to differentiate them
            // Using TargetFrameworkAttribute for the purpose.
            if (string.Equals(frameworkVersion, "v4.0", StringComparison.OrdinalIgnoreCase))
            {
                // Try to determine the exact .NET framework by inspecting custom attributes on assembly.
                string dotNetVersion = GetTargetFrameworkStringFromAssembly(a);
                if (dotNetVersion.StartsWith(Constants.DotNetFramework40, StringComparison.OrdinalIgnoreCase))
                {
                    frameworkVersion = "v4.0";
                }
                else if (dotNetVersion.StartsWith(Constants.DotNetFramework45, StringComparison.OrdinalIgnoreCase))
                {
                    frameworkVersion = "v4.5";
                }
                else if (dotNetVersion.Length > Constants.DotNetFrameWorkStringPrefix.Length)
                {
                    frameworkVersion = dotNetVersion.Substring(Constants.DotNetFrameWorkStringPrefix.Length);
                }
            }

        }
        catch (BadImageFormatException)
        {
            EqtTrace.Error("AssemblyLoadWorker:GetPlatformAndFrameworkSettings() caught BadImageFormatException. Falling to native binary.");
            procArchType = GetArchitectureForSource(path);
        }
        catch (Exception ex)
        {
            EqtTrace.Error("AssemblyLoadWorker:GetPlatformAndFrameworkSettings() Returning default. Unhandled exception: {0}.", ex);
            return;
        }
    }

    private static string GetArchitectureForSource(string imagePath)
    {
        // For details refer to below code available on MSDN.
        // http://code.msdn.microsoft.com/CSCheckExeType-aab06100/sourcecode?fileId=22010&pathId=1874010322

        string archType = "AnyCPU";
        ushort machine = 0;

        uint peHeader;
        const int imageFileMachineAmd64 = 0x8664;
        const int imageFileMachineIa64 = 0x200;
        const int imageFileMachineI386 = 0x14c;
        const int imageFileMachineArm = 0x01c0;  // ARM Little-Endian
        const int imageFileMachineThumb = 0x01c2;  // ARM Thumb/Thumb-2 Little-Endian
        const int imageFileMachineArmnt = 0x01c4; // ARM Thumb-2 Little-Endian

        try
        {
            //get the input stream
            using Stream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
            bool validImage = true;

            BinaryReader reader = new(fs);
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

                UInt32 signature = reader.ReadUInt32(); //peHeaderSignature
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
                    UInt16 magic = reader.ReadUInt16();
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
                            archType = "X86";
                            break;

                        case imageFileMachineAmd64:
                        case imageFileMachineIa64:
                            archType = "X64";
                            break;

                        case imageFileMachineArm:
                        case imageFileMachineThumb:
                        case imageFileMachineArmnt:
                            archType = "ARM";
                            break;
                    }
                }
                else
                {
                    EqtTrace.Verbose("Source path {0} is not a valid image path. Returning default proc arch type {1}.", imagePath, "AnyCPU");
                }
            }
        }
        catch (Exception ex)
        {
            //Ignore all exception
            EqtTrace.Error("AssemblyLoadWorker:GetArchitectureForSource() Returning default:{0}. Unhandled exception: {1}.", "AnyCPU", ex.ToString());
        }

        return archType;
    }
}
#endif
