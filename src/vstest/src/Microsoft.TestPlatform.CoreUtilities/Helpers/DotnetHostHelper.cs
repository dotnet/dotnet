// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection.PortableExecutable;

using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Resources;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers;
using Microsoft.VisualStudio.TestPlatform.Utilities.Helpers.Interfaces;
using Microsoft.Win32;

namespace Microsoft.VisualStudio.TestPlatform.CrossPlatEngine.Helpers;

public class DotnetHostHelper : IDotnetHostHelper
{
    public const string MONOEXENAME = "mono";

    private readonly IFileHelper _fileHelper;
    private readonly IEnvironment _environment;
    private readonly IWindowsRegistryHelper _windowsRegistryHelper;
    private readonly IEnvironmentVariableHelper _environmentVariableHelper;
    private readonly IProcessHelper _processHelper;
    private readonly string _muxerName;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetHostHelper"/> class.
    /// </summary>
    public DotnetHostHelper() : this(new FileHelper(), new PlatformEnvironment(), new WindowsRegistryHelper(), new EnvironmentVariableHelper(), new ProcessHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetHostHelper"/> class.
    /// </summary>
    /// <param name="fileHelper">File Helper</param>
    /// <param name="environment">Environment Helper</param>
    public DotnetHostHelper(IFileHelper fileHelper, IEnvironment environment) : this(fileHelper, environment, new WindowsRegistryHelper(), new EnvironmentVariableHelper(), new ProcessHelper())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DotnetHostHelper"/> class.
    /// </summary>
    /// <param name="fileHelper">File Helper</param>
    /// <param name="environment">Environment Helper</param>
    /// <param name="windowsRegistryHelper">WindowsRegistry Helper</param>
    /// <param name="environmentVariableHelper">EnvironmentVariable Helper</param>
    /// <param name="processHelper">Process Helper</param>
    internal DotnetHostHelper(
        IFileHelper fileHelper,
        IEnvironment environment,
        IWindowsRegistryHelper windowsRegistryHelper,
        IEnvironmentVariableHelper environmentVariableHelper,
        IProcessHelper processHelper)
    {
        _fileHelper = fileHelper;
        _environment = environment;
        _windowsRegistryHelper = windowsRegistryHelper;
        _environmentVariableHelper = environmentVariableHelper;
        _processHelper = processHelper;
        _muxerName = environment.OperatingSystem == PlatformOperatingSystem.Windows ? "dotnet.exe" : "dotnet";
    }

    /// <inheritdoc />
    public string GetDotnetPath()
    {
        if (!TryGetExecutablePath("dotnet", out var dotnetPath))
        {
            string errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.NoDotnetExeFound, "dotnet");

            EqtTrace.Error(errorMessage);
            throw new FileNotFoundException(errorMessage);
        }

        return dotnetPath;
    }

    public string GetMonoPath()
    {
        if (!TryGetExecutablePath(MONOEXENAME, out var monoPath))
        {
            string errorMessage = string.Format(CultureInfo.CurrentCulture, Resources.NoDotnetExeFound, MONOEXENAME);

            EqtTrace.Error(errorMessage);
            throw new FileNotFoundException(errorMessage);
        }

        return monoPath;
    }

    private bool TryGetExecutablePath(string executableBaseName, [NotNullWhen(true)] out string? executablePath)
    {
        if (_environment.OperatingSystem.Equals(PlatformOperatingSystem.Windows))
        {
            executableBaseName += ".exe";
        }

        var pathString = Environment.GetEnvironmentVariable("PATH")!;
        foreach (string path in pathString.Split(Path.PathSeparator))
        {
            string exeFullPath = Path.Combine(path.Trim(), executableBaseName);
            if (_fileHelper.Exists(exeFullPath))
            {
                executablePath = exeFullPath;
                return true;
            }
        }

        executablePath = null;
        return false;
    }

    public bool TryGetDotnetPathByArchitecture(
        PlatformArchitecture targetArchitecture,
        DotnetMuxerResolutionStrategy dotnetMuxerResolutionStrategy,
        [NotNullWhen(true)] out string? muxerPath)
    {
        muxerPath = null;
        EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Using dotnet muxer resolution strategy: {dotnetMuxerResolutionStrategy}");

        // If current process is the same as the target architecture we return the current process filename.
        if (_processHelper.GetCurrentProcessArchitecture() == targetArchitecture)
        {
            string currentProcessFileName = _processHelper.GetCurrentProcessFileName()!;
            if (Path.GetFileName(currentProcessFileName) == _muxerName)
            {
                muxerPath = currentProcessFileName;
                EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Target architecture is the same as the current process architecture '{targetArchitecture}', and the current process is a muxer, using that: '{muxerPath}'");
                return true;
            }

            EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Target architecture is the same as the current process architecture '{targetArchitecture}', but the current process is not a muxer: '{currentProcessFileName}'");
        }

        // We used similar approach as the runtime resolver.
        // https://github.com/dotnet/runtime/blob/main/src/native/corehost/fxr_resolver.cpp#L55

        bool isWinOs = _environment.OperatingSystem == PlatformOperatingSystem.Windows;
        EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Searching for muxer named '{_muxerName}'");

        string? envKey = null;
        string? envVar = null;
        if (dotnetMuxerResolutionStrategy.HasFlag(DotnetMuxerResolutionStrategy.DotnetRootArchitecture))
        {
            // Try to search using env vars in the order
            // DOTNET_ROOT_{arch}
            // DOTNET_ROOT(x86) if X86 on Win (here we cannot check if current process is WOW64 because this is SDK process arch and not real host arch so it's irrelevant)
            //                  "DOTNET_ROOT(x86) is used instead when running a 32-bit executable on a 64-bit OS."
            // DOTNET_ROOT
            envKey = $"DOTNET_ROOT_{targetArchitecture.ToString().ToUpperInvariant()}";

            // Try on arch specific env var
            envVar = _environmentVariableHelper.GetEnvironmentVariable(envKey);
        }

        if (dotnetMuxerResolutionStrategy.HasFlag(DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess))
        {
            // Try on non virtualized x86 var(should happen only on non-x86 architecture)
            if ((envVar == null || !_fileHelper.DirectoryExists(envVar)) &&
                targetArchitecture == PlatformArchitecture.X86 && _environment.OperatingSystem == PlatformOperatingSystem.Windows)
            {
                envKey = $"DOTNET_ROOT(x86)";
                envVar = _environmentVariableHelper.GetEnvironmentVariable(envKey);
            }

            // Try on default DOTNET_ROOT
            if (envVar == null || !_fileHelper.DirectoryExists(envVar))
            {
                envKey = "DOTNET_ROOT";
                envVar = _environmentVariableHelper.GetEnvironmentVariable(envKey);
            }
        }

        if (envVar != null)
        {
            // If directory specified by env vars does not exists, it's like env var doesn't exists as well.
            if (!_fileHelper.DirectoryExists(envVar))
            {
                EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Folder specified by env variable does not exist: '{envVar}={envKey}'");
            }
            else
            {
                muxerPath = Path.Combine(envVar, _muxerName);
                if (!_fileHelper.Exists(muxerPath))
                {
                    // If environment variable was specified, and the directory it points at exists, but it does not contain a muxer, or the muxer is incompatible with the target architecture
                    // we stop the search to be compliant with the approach that apphost (compiled .NET executables) use to find the muxer.
                    EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Folder specified by env variable does not contain any muxer: '{envVar}={envKey}'");
                    muxerPath = null;
                    return false;
                }

                if (!IsValidArchitectureMuxer(targetArchitecture, muxerPath))
                {
                    EqtTrace.Verbose($"DotnetHostHelper: Invalid muxer resolved using env var key '{envKey}' in '{envVar}'");
                    muxerPath = null;
                    return false;
                }

                EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer compatible with '{targetArchitecture}' resolved from env variable '{envKey}' in '{muxerPath}'");
                return true;
            }
        }

        if (dotnetMuxerResolutionStrategy.HasFlag(DotnetMuxerResolutionStrategy.DotnetRootArchitecture)
            || dotnetMuxerResolutionStrategy.HasFlag(DotnetMuxerResolutionStrategy.DotnetRootArchitectureLess))
        {
            EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer was not found using DOTNET_ROOT* env variables.");
        }

        if (dotnetMuxerResolutionStrategy.HasFlag(DotnetMuxerResolutionStrategy.GlobalInstallationLocation))
        {
            // Try to search for global registration
            muxerPath = isWinOs ? GetMuxerFromGlobalRegistrationWin(targetArchitecture) : GetMuxerFromGlobalRegistrationOnUnix(targetArchitecture);

            if (muxerPath != null)
            {
                if (!_fileHelper.Exists(muxerPath))
                {
                    // If muxer doesn't exists or it's wrong we stop the search
                    EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer file not found for global registration '{muxerPath}'");
                    muxerPath = null;
                    return false;
                }

                if (!IsValidArchitectureMuxer(targetArchitecture, muxerPath))
                {
                    // If muxer is wrong we stop the search
                    EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer resolved using global registration is not compatible with the target architecture: '{muxerPath}'");
                    muxerPath = null;
                    return false;
                }

                EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer compatible with '{targetArchitecture}' resolved from global registration: '{muxerPath}'");
                return true;
            }

            EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer not found using global registrations");
        }

        if (dotnetMuxerResolutionStrategy.HasFlag(DotnetMuxerResolutionStrategy.DefaultInstallationLocation))
        {
            // Try searching in default installation location if it exists
            if (isWinOs)
            {
                // If we're on x64/arm64 SDK and target is x86 we need to search on non virtualized windows folder
                if ((_environment.Architecture == PlatformArchitecture.X64 || _environment.Architecture == PlatformArchitecture.ARM64) &&
                     targetArchitecture == PlatformArchitecture.X86)
                {
                    muxerPath = Path.Combine(_environmentVariableHelper.GetEnvironmentVariable("ProgramFiles(x86)")!, "dotnet", _muxerName);
                }
                else
                {
                    // If we're on ARM and target is x64 we expect correct installation inside x64 folder
                    muxerPath = _environment.Architecture == PlatformArchitecture.ARM64 && targetArchitecture == PlatformArchitecture.X64
                        ? Path.Combine(_environmentVariableHelper.GetEnvironmentVariable("ProgramFiles")!, "dotnet", "x64", _muxerName)
                        : Path.Combine(_environmentVariableHelper.GetEnvironmentVariable("ProgramFiles")!, "dotnet", _muxerName);
                }
            }
            else
            {
                if (_environment.OperatingSystem == PlatformOperatingSystem.OSX)
                {
                    // If we're on ARM and target is x64 we expect correct installation inside x64 folder
                    muxerPath = _environment.Architecture == PlatformArchitecture.ARM64 && targetArchitecture == PlatformArchitecture.X64
                        ? Path.Combine("/usr/local/share/dotnet/x64", _muxerName)
                        : Path.Combine("/usr/local/share/dotnet", _muxerName);
                }
                else
                {
                    muxerPath = Path.Combine("/usr/share/dotnet", _muxerName);
                }
            }
        }

        if (!_fileHelper.Exists(muxerPath))
        {
            // If muxer doesn't exists we stop the search
            EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer was not found in default installation location: '{muxerPath}'");
            muxerPath = null;
            return false;
        }

        if (!IsValidArchitectureMuxer(targetArchitecture, muxerPath))
        {
            // If muxer is wrong we stop the search
            EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer resolved in default installation path is not compatible with the target architecture: '{muxerPath}'");
            muxerPath = null;
            return false;
        }

        EqtTrace.Verbose($"DotnetHostHelper.TryGetDotnetPathByArchitecture: Muxer compatible with '{targetArchitecture}' resolved from default installation path: '{muxerPath}'");
        return true;
    }

    private string? GetMuxerFromGlobalRegistrationWin(PlatformArchitecture targetArchitecture)
    {
        // Installed version are always in 32-bit view of registry
        // https://github.com/dotnet/designs/blob/main/accepted/2020/install-locations.md#globally-registered-install-location-new
        // "Note that this registry key is "redirected" that means that 32-bit processes see different copy of the key than 64bit processes.
        // So it's important that both installers and the host access only the 32-bit view of the registry."
        using IRegistryKey? hklm = _windowsRegistryHelper.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        if (hklm == null)
        {
            EqtTrace.Verbose($@"DotnetHostHelper.GetMuxerFromGlobalRegistrationWin: Missing SOFTWARE\dotnet\Setup\InstalledVersions subkey");
            return null;
        }

        using IRegistryKey? dotnetInstalledVersion = hklm.OpenSubKey(@"SOFTWARE\dotnet\Setup\InstalledVersions");
        if (dotnetInstalledVersion == null)
        {
            EqtTrace.Verbose($@"DotnetHostHelper.GetMuxerFromGlobalRegistrationWin: Missing RegistryHive.LocalMachine for RegistryView.Registry32");
            return null;
        }

        using IRegistryKey? nativeArch = dotnetInstalledVersion.OpenSubKey(targetArchitecture.ToString().ToLowerInvariant());
        string? installLocation = nativeArch?.GetValue("InstallLocation")?.ToString();
        if (installLocation == null)
        {
            EqtTrace.Verbose($@"DotnetHostHelper.GetMuxerFromGlobalRegistrationWin: Missing registry InstallLocation");
            return null;
        }

        string path = Path.Combine(installLocation.Trim(), _muxerName);
        EqtTrace.Verbose($@"DotnetHostHelper.GetMuxerFromGlobalRegistrationWin: Muxer resolved using win registry key 'SOFTWARE\dotnet\Setup\InstalledVersions\{targetArchitecture.ToString().ToLowerInvariant()}\InstallLocation' in '{path}'");
        return path;
    }

    private string? GetMuxerFromGlobalRegistrationOnUnix(PlatformArchitecture targetArchitecture)
    {
        string baseInstallLocation = "/etc/dotnet/";

        // We search for architecture specific installation
        string installLocation = $"{baseInstallLocation}install_location_{targetArchitecture.ToString().ToLowerInvariant()}";

        // We try to load archless install location file
        if (!_fileHelper.Exists(installLocation))
        {
            installLocation = $"{baseInstallLocation}install_location";
        }

        if (!_fileHelper.Exists(installLocation))
        {
            return null;
        }

        try
        {
            using Stream stream = _fileHelper.GetStream(installLocation, FileMode.Open, FileAccess.Read);
            using StreamReader streamReader = new(stream);
            string content = streamReader.ReadToEnd().Trim();
            EqtTrace.Verbose($"DotnetHostHelper: '{installLocation}' content '{content}'");
            string path = Path.Combine(content, _muxerName);
            EqtTrace.Verbose($"DotnetHostHelper: Muxer resolved using '{installLocation}' in '{path}'");
            return path;
        }
        catch (Exception ex)
        {
            EqtTrace.Error($"DotnetHostHelper.GetMuxerFromGlobalRegistrationOnUnix: Exception during '{installLocation}' muxer resolution.\n{ex}");
        }

        return null;
    }

    private PlatformArchitecture? GetMuxerArchitectureByPEHeaderOnWin(string path)
    {
        try
        {
            using Stream stream = _fileHelper.GetStream(path, FileMode.Open, FileAccess.Read);
            using PEReader peReader = new(stream);
            switch (peReader.PEHeaders.CoffHeader.Machine)
            {
                case Machine.Amd64:
                    return PlatformArchitecture.X64;
                case Machine.IA64:
                    return PlatformArchitecture.X64;
                case Machine.Arm64:
                    return PlatformArchitecture.ARM64;
                case Machine.Arm:
                    return PlatformArchitecture.ARM;
                case Machine.I386:
                    return PlatformArchitecture.X86;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            EqtTrace.Error($"DotnetHostHelper.GetMuxerArchitectureByPEHeaderOnWin: Failed to get architecture from PEHeader for '{path}'\n{ex}");
        }

        return null;
    }

    // See https://opensource.apple.com/source/xnu/xnu-2050.18.24/EXTERNAL_HEADERS/mach-o/loader.h
    // https://opensource.apple.com/source/xnu/xnu-4570.41.2/osfmk/mach/machine.h.auto.html
    private PlatformArchitecture? GetMuxerArchitectureByMachoOnMac(string path)
    {
        try
        {
            using var headerReader = _fileHelper.GetStream(path, FileMode.Open, FileAccess.Read);
            var magicBytes = new byte[4];
            var cpuInfoBytes = new byte[4];

            ReadExactly(headerReader, magicBytes, 0, magicBytes.Length);
            ReadExactly(headerReader, cpuInfoBytes, 0, cpuInfoBytes.Length);

            var magic = BitConverter.ToUInt32(magicBytes, 0);
            var cpuInfo = BitConverter.ToUInt32(cpuInfoBytes, 0);
            PlatformArchitecture? architecture = (MacOsCpuType)cpuInfo switch
            {
                MacOsCpuType.Arm64Magic or MacOsCpuType.Arm64Cigam => PlatformArchitecture.ARM64,
                MacOsCpuType.X64Magic or MacOsCpuType.X64Cigam => PlatformArchitecture.X64,
                MacOsCpuType.X86Magic or MacOsCpuType.X86Cigam => PlatformArchitecture.X86,
                _ => null,
            };

            return architecture;
        }
        catch (Exception ex)
        {
            // In case of failure during header reading we must fallback to the next place(default installation path)
            EqtTrace.Error($"DotnetHostHelper.GetMuxerArchitectureByMachoOnMac: Failed to get architecture from Mach-O for '{path}'\n{ex}");
        }

        return null;
    }

#if NET
    private static void ReadExactly(Stream stream, byte[] buffer, int offset, int count)
    {
        stream.ReadExactly(buffer, offset, count);
    }
#else
    private static void ReadExactly(Stream stream, byte[] buffer, int offset, int count)
    {
        while (count > 0)
        {
            int read = stream.Read(buffer, offset, count);
            if (read <= 0)
            {
                throw new EndOfStreamException();
            }
            offset += read;
            count -= read;
        }
    }
#endif

    internal enum MacOsCpuType : uint
    {
        Arm64Magic = 0x0100000c,
        Arm64Cigam = 0x0c000001,
        X64Magic = 0x01000007,
        X64Cigam = 0x07000001,
        X86Magic = 0x00000007,
        X86Cigam = 0x07000000
    }

    private bool IsValidArchitectureMuxer(PlatformArchitecture targetArchitecture, string path)
    {
        PlatformArchitecture? muxerPlatform = null;
        if (_environment.OperatingSystem == PlatformOperatingSystem.Windows)
        {
            muxerPlatform = GetMuxerArchitectureByPEHeaderOnWin(path);
        }
        else if (_environment.OperatingSystem == PlatformOperatingSystem.OSX)
        {
            muxerPlatform = GetMuxerArchitectureByMachoOnMac(path);
        }

        if (targetArchitecture != muxerPlatform)
        {
            EqtTrace.Verbose($"DotnetHostHelper.IsValidArchitectureMuxer: Incompatible architecture muxer, target architecture '{targetArchitecture}', actual '{muxerPlatform}'");
            return false;
        }

        EqtTrace.Verbose($"DotnetHostHelper.IsValidArchitectureMuxer: Compatible architecture muxer, target architecture '{targetArchitecture}', actual '{muxerPlatform}'");
        return true;
    }
}
