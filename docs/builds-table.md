> [!NOTE]
> When acquiring installers from the .NET SDK latest builds table, be aware that the installers are the **latest bits**. With development builds, internal NuGet feeds are necessary for some scenarios (for example, to acquire the runtime pack for self-contained apps). You can use the following NuGet.config to configure these feeds. See the following document [Configuring NuGet behavior](https://docs.microsoft.com/nuget/consume-packages/configuring-nuget-behavior) for more information on where to modify your NuGet.config to apply the changes.

### For .NET 11 builds
```xml
<configuration>
  <packageSources>
    <add key="dotnet11" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet11/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

### For .NET 10 builds
```xml
<configuration>
  <packageSources>
    <add key="dotnet10" value="https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet10/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

### Supported Platforms

--------------------------------------------------------------------------------------------------------------------------------
| Platform | main<br>(11.0.x&nbsp;Runtime) | 11.0.1xx-preview1<br>(11.0-preview1&nbsp;Runtime) | 10.0.3xx<br>(10.0.3xx&nbsp;Runtime) | 10.0.2xx<br>(10.0.2xx&nbsp;Runtime) |
| :--------- | :----------: | :----------: | :----------: | :----------: |
| **Windows x64** | [![][win-x64-badge-main]][win-x64-version-main]<br>[Installer][win-x64-installer-main] - [Checksum][win-x64-installer-checksum-main]<br>[zip][win-x64-zip-main] - [Checksum][win-x64-zip-checksum-main] | [![][win-x64-badge-11.0.1xx-preview1]][win-x64-version-11.0.1xx-preview1]<br>[Installer][win-x64-installer-11.0.1xx-preview1] - [Checksum][win-x64-installer-checksum-11.0.1xx-preview1]<br>[zip][win-x64-zip-11.0.1xx-preview1] - [Checksum][win-x64-zip-checksum-11.0.1xx-preview1] | [![][win-x64-badge-10.0.3xx]][win-x64-version-10.0.3xx]<br>[Installer][win-x64-installer-10.0.3xx] - [Checksum][win-x64-installer-checksum-10.0.3xx]<br>[zip][win-x64-zip-10.0.3xx] - [Checksum][win-x64-zip-checksum-10.0.3xx] | [![][win-x64-badge-10.0.2XX]][win-x64-version-10.0.2XX]<br>[Installer][win-x64-installer-10.0.2XX] - [Checksum][win-x64-installer-checksum-10.0.2XX]<br>[zip][win-x64-zip-10.0.2XX] - [Checksum][win-x64-zip-checksum-10.0.2XX] |
| **Windows x86** | [![][win-x86-badge-main]][win-x86-version-main]<br>[Installer][win-x86-installer-main] - [Checksum][win-x86-installer-checksum-main]<br>[zip][win-x86-zip-main] - [Checksum][win-x86-zip-checksum-main] | [![][win-x86-badge-11.0.1xx-preview1]][win-x86-version-11.0.1xx-preview1]<br>[Installer][win-x86-installer-11.0.1xx-preview1] - [Checksum][win-x86-installer-checksum-11.0.1xx-preview1]<br>[zip][win-x86-zip-11.0.1xx-preview1] - [Checksum][win-x86-zip-checksum-11.0.1xx-preview1] | [![][win-x86-badge-10.0.3xx]][win-x86-version-10.0.3xx]<br>[Installer][win-x86-installer-10.0.3xx] - [Checksum][win-x86-installer-checksum-10.0.3xx]<br>[zip][win-x86-zip-10.0.3xx] - [Checksum][win-x86-zip-checksum-10.0.3xx] | [![][win-x86-badge-10.0.2XX]][win-x86-version-10.0.2XX]<br>[Installer][win-x86-installer-10.0.2XX] - [Checksum][win-x86-installer-checksum-10.0.2XX]<br>[zip][win-x86-zip-10.0.2XX] - [Checksum][win-x86-zip-checksum-10.0.2XX] |
| **Windows arm64** | [![][win-arm64-badge-main]][win-arm64-version-main]<br>[Installer][win-arm64-installer-main] - [Checksum][win-arm64-installer-checksum-main]<br>[zip][win-arm64-zip-main] - [Checksum][win-arm64-zip-checksum-main] | [![][win-arm64-badge-11.0.1xx-preview1]][win-arm64-version-11.0.1xx-preview1]<br>[Installer][win-arm64-installer-11.0.1xx-preview1] - [Checksum][win-arm64-installer-checksum-11.0.1xx-preview1]<br>[zip][win-arm64-zip-11.0.1xx-preview1] - [Checksum][win-arm64-zip-checksum-11.0.1xx-preview1] | [![][win-arm64-badge-10.0.3xx]][win-arm64-version-10.0.3xx]<br>[Installer][win-arm64-installer-10.0.3xx] - [Checksum][win-arm64-installer-checksum-10.0.3xx]<br>[zip][win-arm64-zip-10.0.3xx] - [Checksum][win-arm64-zip-checksum-10.0.3xx] | [![][win-arm64-badge-10.0.2XX]][win-arm64-version-10.0.2XX]<br>[Installer][win-arm64-installer-10.0.2XX] - [Checksum][win-arm64-installer-checksum-10.0.2XX]<br>[zip][win-arm64-zip-10.0.2XX] - [Checksum][win-arm64-zip-checksum-10.0.2XX] |
| **macOS arm64**</br>(M-series CPUs) | [![][osx-arm64-badge-main]][osx-arm64-version-main]<br>[Installer][osx-arm64-installer-main] - [Checksum][osx-arm64-installer-checksum-main]<br>[tar.gz][osx-arm64-targz-main] - [Checksum][osx-arm64-targz-checksum-main] | [![][osx-arm64-badge-11.0.1xx-preview1]][osx-arm64-version-11.0.1xx-preview1]<br>[Installer][osx-arm64-installer-11.0.1xx-preview1] - [Checksum][osx-arm64-installer-checksum-11.0.1xx-preview1]<br>[tar.gz][osx-arm64-targz-11.0.1xx-preview1] - [Checksum][osx-arm64-targz-checksum-11.0.1xx-preview1] | [![][osx-arm64-badge-10.0.3xx]][osx-arm64-version-10.0.3xx]<br>[Installer][osx-arm64-installer-10.0.3xx] - [Checksum][osx-arm64-installer-checksum-10.0.3xx]<br>[tar.gz][osx-arm64-targz-10.0.3xx] - [Checksum][osx-arm64-targz-checksum-10.0.3xx] | [![][osx-arm64-badge-10.0.2XX]][osx-arm64-version-10.0.2XX]<br>[Installer][osx-arm64-installer-10.0.2XX] - [Checksum][osx-arm64-installer-checksum-10.0.2XX]<br>[tar.gz][osx-arm64-targz-10.0.2XX] - [Checksum][osx-arm64-targz-checksum-10.0.2XX] |
| **macOS x64**</br>(Intel CPUs) | [![][osx-x64-badge-main]][osx-x64-version-main]<br>[Installer][osx-x64-installer-main] - [Checksum][osx-x64-installer-checksum-main]<br>[tar.gz][osx-x64-targz-main] - [Checksum][osx-x64-targz-checksum-main] | [![][osx-x64-badge-11.0.1xx-preview1]][osx-x64-version-11.0.1xx-preview1]<br>[Installer][osx-x64-installer-11.0.1xx-preview1] - [Checksum][osx-x64-installer-checksum-11.0.1xx-preview1]<br>[tar.gz][osx-x64-targz-11.0.1xx-preview1] - [Checksum][osx-x64-targz-checksum-11.0.1xx-preview1] | [![][osx-x64-badge-10.0.3xx]][osx-x64-version-10.0.3xx]<br>[Installer][osx-x64-installer-10.0.3xx] - [Checksum][osx-x64-installer-checksum-10.0.3xx]<br>[tar.gz][osx-x64-targz-10.0.3xx] - [Checksum][osx-x64-targz-checksum-10.0.3xx] | [![][osx-x64-badge-10.0.2XX]][osx-x64-version-10.0.2XX]<br>[Installer][osx-x64-installer-10.0.2XX] - [Checksum][osx-x64-installer-checksum-10.0.2XX]<br>[tar.gz][osx-x64-targz-10.0.2XX] - [Checksum][osx-x64-targz-checksum-10.0.2XX] |
| **Linux x64** | [![][linux-badge-main]][linux-version-main]<br>[DEB Installer][linux-DEB-installer-main] - [Checksum][linux-DEB-installer-checksum-main]<br>[RPM Installer][linux-RPM-installer-main] - [Checksum][linux-RPM-installer-checksum-main]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-main] - [Checksum][linux-targz-checksum-main] | [![][linux-badge-11.0.1xx-preview1]][linux-version-11.0.1xx-preview1]<br>[DEB Installer][linux-DEB-installer-11.0.1xx-preview1] - [Checksum][linux-DEB-installer-checksum-11.0.1xx-preview1]<br>[RPM Installer][linux-RPM-installer-11.0.1xx-preview1] - [Checksum][linux-RPM-installer-checksum-11.0.1xx-preview1]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-11.0.1xx-preview1] - [Checksum][linux-targz-checksum-11.0.1xx-preview1] | [![][linux-badge-10.0.3xx]][linux-version-10.0.3xx]<br>[DEB Installer][linux-DEB-installer-10.0.3xx] - [Checksum][linux-DEB-installer-checksum-10.0.3xx]<br>[RPM Installer][linux-RPM-installer-10.0.3xx] - [Checksum][linux-RPM-installer-checksum-10.0.3xx]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-10.0.3xx] - [Checksum][linux-targz-checksum-10.0.3xx] | [![][linux-badge-10.0.2XX]][linux-version-10.0.2XX]<br>[DEB Installer][linux-DEB-installer-10.0.2XX] - [Checksum][linux-DEB-installer-checksum-10.0.2XX]<br>[RPM Installer][linux-RPM-installer-10.0.2XX] - [Checksum][linux-RPM-installer-checksum-10.0.2XX]<br>_see installer note below_<sup>1</sup><br>[tar.gz][linux-targz-10.0.2XX] - [Checksum][linux-targz-checksum-10.0.2XX] |
| **Linux arm** | [![][linux-arm-badge-main]][linux-arm-version-main]<br>[tar.gz][linux-arm-targz-main] - [Checksum][linux-arm-targz-checksum-main] | [![][linux-arm-badge-11.0.1xx-preview1]][linux-arm-version-11.0.1xx-preview1]<br>[tar.gz][linux-arm-targz-11.0.1xx-preview1] - [Checksum][linux-arm-targz-checksum-11.0.1xx-preview1] | [![][linux-arm-badge-10.0.3xx]][linux-arm-version-10.0.3xx]<br>[tar.gz][linux-arm-targz-10.0.3xx] - [Checksum][linux-arm-targz-checksum-10.0.3xx] | [![][linux-arm-badge-10.0.2XX]][linux-arm-version-10.0.2XX]<br>[tar.gz][linux-arm-targz-10.0.2XX] - [Checksum][linux-arm-targz-checksum-10.0.2XX] |
| **Linux arm64** | [![][linux-arm64-badge-main]][linux-arm64-version-main]<br>[tar.gz][linux-arm64-targz-main] - [Checksum][linux-arm64-targz-checksum-main] | [![][linux-arm64-badge-11.0.1xx-preview1]][linux-arm64-version-11.0.1xx-preview1]<br>[tar.gz][linux-arm64-targz-11.0.1xx-preview1] - [Checksum][linux-arm64-targz-checksum-11.0.1xx-preview1] | [![][linux-arm64-badge-10.0.3xx]][linux-arm64-version-10.0.3xx]<br>[tar.gz][linux-arm64-targz-10.0.3xx] - [Checksum][linux-arm64-targz-checksum-10.0.3xx] | [![][linux-arm64-badge-10.0.2XX]][linux-arm64-version-10.0.2XX]<br>[tar.gz][linux-arm64-targz-10.0.2XX] - [Checksum][linux-arm64-targz-checksum-10.0.2XX] |
| **Linux-musl-x64** | [![][linux-musl-x64-badge-main]][linux-musl-x64-version-main]<br>[tar.gz][linux-musl-x64-targz-main] - [Checksum][linux-musl-x64-targz-checksum-main] | [![][linux-musl-x64-badge-11.0.1xx-preview1]][linux-musl-x64-version-11.0.1xx-preview1]<br>[tar.gz][linux-musl-x64-targz-11.0.1xx-preview1] - [Checksum][linux-musl-x64-targz-checksum-11.0.1xx-preview1] | [![][linux-musl-x64-badge-10.0.3xx]][linux-musl-x64-version-10.0.3xx]<br>[tar.gz][linux-musl-x64-targz-10.0.3xx] - [Checksum][linux-musl-x64-targz-checksum-10.0.3xx] | [![][linux-musl-x64-badge-10.0.2XX]][linux-musl-x64-version-10.0.2XX]<br>[tar.gz][linux-musl-x64-targz-10.0.2XX] - [Checksum][linux-musl-x64-targz-checksum-10.0.2XX] |
| **Linux-musl-arm** | [![][linux-musl-arm-badge-main]][linux-musl-arm-version-main]<br>[tar.gz][linux-musl-arm-targz-main] - [Checksum][linux-musl-arm-targz-checksum-main] | [![][linux-musl-arm-badge-11.0.1xx-preview1]][linux-musl-arm-version-11.0.1xx-preview1]<br>[tar.gz][linux-musl-arm-targz-11.0.1xx-preview1] - [Checksum][linux-musl-arm-targz-checksum-11.0.1xx-preview1] | [![][linux-musl-arm-badge-10.0.3xx]][linux-musl-arm-version-10.0.3xx]<br>[tar.gz][linux-musl-arm-targz-10.0.3xx] - [Checksum][linux-musl-arm-targz-checksum-10.0.3xx] | [![][linux-musl-arm-badge-10.0.2XX]][linux-musl-arm-version-10.0.2XX]<br>[tar.gz][linux-musl-arm-targz-10.0.2XX] - [Checksum][linux-musl-arm-targz-checksum-10.0.2XX] |
| **Linux-musl-arm64** | [![][linux-musl-arm64-badge-main]][linux-musl-arm64-version-main]<br>[tar.gz][linux-musl-arm64-targz-main] - [Checksum][linux-musl-arm64-targz-checksum-main] | [![][linux-musl-arm64-badge-11.0.1xx-preview1]][linux-musl-arm64-version-11.0.1xx-preview1]<br>[tar.gz][linux-musl-arm64-targz-11.0.1xx-preview1] - [Checksum][linux-musl-arm64-targz-checksum-11.0.1xx-preview1] | [![][linux-musl-arm64-badge-10.0.3xx]][linux-musl-arm64-version-10.0.3xx]<br>[tar.gz][linux-musl-arm64-targz-10.0.3xx] - [Checksum][linux-musl-arm64-targz-checksum-10.0.3xx] | [![][linux-musl-arm64-badge-10.0.2XX]][linux-musl-arm64-version-10.0.2XX]<br>[tar.gz][linux-musl-arm64-targz-10.0.2XX] - [Checksum][linux-musl-arm64-targz-checksum-10.0.2XX] |

# .NET SDK Daily Builds

## Installation Instructions

### Windows

Download the latest SDK installer or binaries from the builds table above.

Windows SDK and Runtime installers are complete packages and do not require separate dependency installation.

### Linux (DEB and RPM Packages)

#### SDK Packages

Download the appropriate SDK package for your distribution:
- **Debian/Ubuntu:** `dotnet-sdk-<version>-x64.deb`
- **Red Hat/Fedora/CentOS:** `dotnet-sdk-<version>-x64.rpm`

#### Dependencies

The following runtime packages are required dependencies and must be installed before installing the SDK:

**For Debian/Ubuntu (.deb):**
- `dotnet-host-<version>-x64.deb`
- `dotnet-targeting-pack-<version>-x64.deb`
- `dotnet-hostfxr-<version>-x64.deb`
- `dotnet-apphost-pack-<version>-x64.deb`
- `dotnet-runtime-deps-<version>-x64.deb`
- `dotnet-runtime-<version>-x64.deb`

**For Red Hat/Fedora/CentOS (.rpm):**
- `dotnet-host-<version>-x64.rpm`
- `dotnet-targeting-pack-<version>-x64.rpm`
- `dotnet-hostfxr-<version>-x64.rpm`
- `dotnet-apphost-pack-<version>-x64.rpm`
- `dotnet-runtime-deps-<version>-x64.rpm`
- `dotnet-runtime-<version>-x64.rpm`

The following ASP.NET Core packages are required for ASP.NET Core development:

**For Debian/Ubuntu (.deb):**
- `aspnetcore-targeting-pack-<version>-x64.deb`
- `aspnetcore-runtime-<version>-x64.deb`

**For Red Hat/Fedora/CentOS (.rpm):**
- `aspnetcore-targeting-pack-<version>-x64.rpm`
- `aspnetcore-runtime-<version>-x64.rpm`

These packages can be downloaded from:

**Runtime packages:**
```
https://ci.dot.net/public/Runtime/<version>/dotnet-host-<version>-x64.<ext>
https://ci.dot.net/public/Runtime/<version>/dotnet-targeting-pack-<version>-x64.<ext>
https://ci.dot.net/public/Runtime/<version>/dotnet-hostfxr-<version>-x64.<ext>
https://ci.dot.net/public/Runtime/<version>/dotnet-apphost-pack-<version>-x64.<ext>
https://ci.dot.net/public/Runtime/<version>/dotnet-runtime-deps-<version>-x64.<ext>
https://ci.dot.net/public/Runtime/<version>/dotnet-runtime-<version>-x64.<ext>
```

**ASP.NET Core packages:**
```
https://ci.dot.net/public/aspnetcore/Runtime/<version>/aspnetcore-targeting-pack-<version>-x64.<ext>
https://ci.dot.net/public/aspnetcore/Runtime/<version>/aspnetcore-runtime-<version>-x64.<ext>
```

Where:
- `<version>` is the same for both SDK and runtime (can be obtained from the version badge links or productCommit files in the table above)
- `<ext>` is either `deb` for Debian/Ubuntu or `rpm` for Red Hat/Fedora/CentOS

#### Installation Order

Install the packages in the following order:

1. Runtime dependencies (in order):
   - `dotnet-host-<version>-x64.<ext>`
   - `dotnet-targeting-pack-<version>-x64.<ext>`
   - `dotnet-hostfxr-<version>-x64.<ext>`
   - `dotnet-apphost-pack-<version>-x64.<ext>`
   - `dotnet-runtime-deps-<version>-x64.<ext>`
   - `dotnet-runtime-<version>-x64.<ext>`

2. ASP.NET Core dependencies (if needed):
   - `aspnetcore-targeting-pack-<version>-x64.<ext>`
   - `aspnetcore-runtime-<version>-x64.<ext>`

3. Finally, install the SDK:
   - `dotnet-sdk-<version>-x64.<ext>`

**Example installation commands:**

For Debian/Ubuntu:
```bash
sudo dpkg -i dotnet-host-<version>-x64.deb
sudo dpkg -i dotnet-targeting-pack-<version>-x64.deb
sudo dpkg -i dotnet-hostfxr-<version>-x64.deb
sudo dpkg -i dotnet-apphost-pack-<version>-x64.deb
sudo dpkg -i dotnet-runtime-deps-<version>-x64.deb
sudo dpkg -i dotnet-runtime-<version>-x64.deb
sudo dpkg -i aspnetcore-targeting-pack-<version>-x64.deb
sudo dpkg -i aspnetcore-runtime-<version>-x64.deb
sudo dpkg -i dotnet-sdk-<version>-x64.deb
```

For Red Hat/Fedora/CentOS:
```bash
sudo rpm -i dotnet-host-<version>-x64.rpm
sudo rpm -i dotnet-targeting-pack-<version>-x64.rpm
sudo rpm -i dotnet-hostfxr-<version>-x64.rpm
sudo rpm -i dotnet-apphost-pack-<version>-x64.rpm
sudo rpm -i dotnet-runtime-deps-<version>-x64.rpm
sudo rpm -i dotnet-runtime-<version>-x64.rpm
sudo rpm -i aspnetcore-targeting-pack-<version>-x64.rpm
sudo rpm -i aspnetcore-runtime-<version>-x64.rpm
sudo rpm -i dotnet-sdk-<version>-x64.rpm
```

### macOS

Download the latest SDK installer or binaries from the builds table above.

macOS SDK and Runtime installers are complete packages and do not require separate dependency installation.

## Version Information

For the 1xx band, the runtime and SDK are built together in the same build. They will differ on patch version (e.g. SDK version 10.0.100 == runtime patch version 10.0.0). The build suffix (e.g. -rc2.1234.105) will always match.

For the 2xx and later bands, the 1xx runtime flows to 2xx+. The version of the runtime that will be used in the 2xx SDK can be found in sdk's `eng/Version.Details.xml` file. Look for the version of the `Microsoft.NETCore.App.Ref` dependency.

[win-x64-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/win_x64_Release_version_badge.svg?no-cache
[win-x64-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-win-x64.txt
[win-x64-installer-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-x64.exe.sha512
[win-x64-zip-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-x64.zip.sha512

[win-x64-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/win_x64_Release_version_badge.svg?no-cache
[win-x64-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-win-x64.txt
[win-x64-installer-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-x64.exe.sha512
[win-x64-zip-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-x64.zip.sha512

[win-x64-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/win_x64_Release_version_badge.svg?no-cache
[win-x64-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-win-x64.txt
[win-x64-installer-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-x64.exe.sha512
[win-x64-zip-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-x64.zip.sha512

[win-x64-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/win_x64_Release_version_badge.svg?no-cache
[win-x64-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-win-x64.txt
[win-x64-installer-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-x64.exe
[win-x64-installer-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-x64.exe.sha512
[win-x64-zip-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-x64.zip
[win-x64-zip-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-x64.zip.sha512

[win-x86-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/win_x86_Release_version_badge.svg?no-cache
[win-x86-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-win-x86.txt
[win-x86-installer-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-x86.exe.sha512
[win-x86-zip-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-x86.zip.sha512

[win-x86-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/win_x86_Release_version_badge.svg?no-cache
[win-x86-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-win-x86.txt
[win-x86-installer-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-x86.exe.sha512
[win-x86-zip-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-x86.zip.sha512

[win-x86-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/win_x86_Release_version_badge.svg?no-cache
[win-x86-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-win-x86.txt
[win-x86-installer-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-x86.exe.sha512
[win-x86-zip-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-x86.zip.sha512

[win-x86-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/win_x86_Release_version_badge.svg?no-cache
[win-x86-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-win-x86.txt
[win-x86-installer-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-x86.exe
[win-x86-installer-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-x86.exe.sha512
[win-x86-zip-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-x86.zip
[win-x86-zip-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-x86.zip.sha512

[osx-x64-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/osx_x64_Release_version_badge.svg?no-cache
[osx-x64-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-osx-x64.txt
[osx-x64-installer-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-osx-x64.pkg.sha512
[osx-x64-targz-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-osx-x64.tar.gz.sha512

[osx-x64-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/osx_x64_Release_version_badge.svg?no-cache
[osx-x64-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-osx-x64.txt
[osx-x64-installer-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-osx-x64.pkg.sha512
[osx-x64-targz-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-osx-x64.tar.gz.sha512

[osx-x64-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/osx_x64_Release_version_badge.svg?no-cache
[osx-x64-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-osx-x64.txt
[osx-x64-installer-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-osx-x64.pkg.sha512
[osx-x64-targz-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-osx-x64.tar.gz.sha512

[osx-x64-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/osx_x64_Release_version_badge.svg?no-cache
[osx-x64-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-osx-x64.txt
[osx-x64-installer-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-osx-x64.pkg
[osx-x64-installer-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-osx-x64.pkg.sha512
[osx-x64-targz-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-osx-x64.tar.gz
[osx-x64-targz-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-osx-x64.tar.gz.sha512

[osx-arm64-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/osx_arm64_Release_version_badge.svg?no-cache
[osx-arm64-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-osx-arm64.pkg.sha512
[osx-arm64-targz-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-osx-arm64.tar.gz.sha512

[osx-arm64-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/osx_arm64_Release_version_badge.svg?no-cache
[osx-arm64-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-osx-arm64.pkg.sha512
[osx-arm64-targz-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-osx-arm64.tar.gz.sha512

[osx-arm64-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/osx_arm64_Release_version_badge.svg?no-cache
[osx-arm64-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-osx-arm64.pkg.sha512
[osx-arm64-targz-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-osx-arm64.tar.gz.sha512

[osx-arm64-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/osx_arm64_Release_version_badge.svg?no-cache
[osx-arm64-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-osx-arm64.txt
[osx-arm64-installer-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-osx-arm64.pkg
[osx-arm64-installer-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-osx-arm64.pkg.sha512
[osx-arm64-targz-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-osx-arm64.tar.gz
[osx-arm64-targz-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-osx-arm64.tar.gz.sha512

[linux-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/linux_x64_Release_version_badge.svg?no-cache
[linux-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-linux-x64.txt
[linux-DEB-installer-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-x64.deb.sha512
[linux-RPM-installer-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-x64.rpm.sha512
[linux-targz-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-x64.tar.gz.sha512

[linux-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/linux_x64_Release_version_badge.svg?no-cache
[linux-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-linux-x64.txt
[linux-DEB-installer-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-x64.deb.sha512
[linux-RPM-installer-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-x64.rpm.sha512
[linux-targz-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-x64.tar.gz.sha512

[linux-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/linux_x64_Release_version_badge.svg?no-cache
[linux-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-linux-x64.txt
[linux-DEB-installer-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-x64.deb.sha512
[linux-RPM-installer-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-x64.rpm.sha512
[linux-targz-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-x64.tar.gz.sha512

[linux-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/linux_x64_Release_version_badge.svg?no-cache
[linux-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-linux-x64.txt
[linux-DEB-installer-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-x64.deb
[linux-DEB-installer-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-x64.deb.sha512
[linux-RPM-installer-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-x64.rpm
[linux-RPM-installer-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-x64.rpm.sha512
[linux-targz-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-x64.tar.gz
[linux-targz-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-x64.tar.gz.sha512

[linux-arm-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/linux_arm_Release_version_badge.svg?no-cache
[linux-arm-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-linux-arm.txt
[linux-arm-targz-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-arm.tar.gz.sha512

[linux-arm-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/linux_arm_Release_version_badge.svg?no-cache
[linux-arm-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-linux-arm.txt
[linux-arm-targz-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-arm.tar.gz.sha512

[linux-arm-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/linux_arm_Release_version_badge.svg?no-cache
[linux-arm-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-linux-arm.txt
[linux-arm-targz-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-arm.tar.gz.sha512

[linux-arm-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/linux_arm_Release_version_badge.svg?no-cache
[linux-arm-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-linux-arm.txt
[linux-arm-targz-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-arm.tar.gz
[linux-arm-targz-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-arm.tar.gz.sha512

[linux-arm64-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/linux_arm64_Release_version_badge.svg?no-cache
[linux-arm64-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-arm64.tar.gz.sha512

[linux-arm64-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/linux_arm64_Release_version_badge.svg?no-cache
[linux-arm64-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-arm64.tar.gz.sha512

[linux-arm64-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/linux_arm64_Release_version_badge.svg?no-cache
[linux-arm64-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-arm64.tar.gz.sha512

[linux-arm64-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/linux_arm64_Release_version_badge.svg?no-cache
[linux-arm64-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-linux-arm64.txt
[linux-arm64-targz-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-arm64.tar.gz
[linux-arm64-targz-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-arm64.tar.gz.sha512

[linux-musl-x64-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/linux_musl_x64_Release_version_badge.svg?no-cache
[linux-musl-x64-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha512

[linux-musl-x64-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/linux_musl_x64_Release_version_badge.svg?no-cache
[linux-musl-x64-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha512

[linux-musl-x64-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/linux_musl_x64_Release_version_badge.svg?no-cache
[linux-musl-x64-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha512

[linux-musl-x64-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/linux_musl_x64_Release_version_badge.svg?no-cache
[linux-musl-x64-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-linux-musl-x64.txt
[linux-musl-x64-targz-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-musl-x64.tar.gz
[linux-musl-x64-targz-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-musl-x64.tar.gz.sha512

[linux-musl-arm-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/linux_musl_arm_Release_version_badge.svg?no-cache
[linux-musl-arm-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha512

[linux-musl-arm-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/linux_musl_arm_Release_version_badge.svg?no-cache
[linux-musl-arm-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha512

[linux-musl-arm-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/linux_musl_arm_Release_version_badge.svg?no-cache
[linux-musl-arm-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha512

[linux-musl-arm-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/linux_musl_arm_Release_version_badge.svg?no-cache
[linux-musl-arm-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-linux-musl-arm.txt
[linux-musl-arm-targz-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-musl-arm.tar.gz
[linux-musl-arm-targz-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-musl-arm.tar.gz.sha512

[linux-musl-arm64-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/linux_musl_arm64_Release_version_badge.svg?no-cache
[linux-musl-arm64-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha512

[linux-musl-arm64-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/linux_musl_arm64_Release_version_badge.svg?no-cache
[linux-musl-arm64-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha512

[linux-musl-arm64-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/linux_musl_arm64_Release_version_badge.svg?no-cache
[linux-musl-arm64-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha512

[linux-musl-arm64-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/linux_musl_arm64_Release_version_badge.svg?no-cache
[linux-musl-arm64-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-linux-musl-arm64.txt
[linux-musl-arm64-targz-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-musl-arm64.tar.gz
[linux-musl-arm64-targz-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-linux-musl-arm64.tar.gz.sha512

[win-arm64-badge-main]: https://aka.ms/dotnet/11.0.1xx/daily/win_arm64_Release_version_badge.svg?no-cache
[win-arm64-version-main]: https://aka.ms/dotnet/11.0.1xx/daily/productCommit-win-arm64.txt
[win-arm64-installer-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-arm64.exe.sha512
[win-arm64-zip-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-main]: https://aka.ms/dotnet/11.0.1xx/daily/dotnet-sdk-win-arm64.zip.sha512

[win-arm64-badge-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/win_arm64_Release_version_badge.svg?no-cache
[win-arm64-version-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/productCommit-win-arm64.txt
[win-arm64-installer-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-arm64.exe.sha512
[win-arm64-zip-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-11.0.1xx-preview1]: https://aka.ms/dotnet/11.0.1xx-preview1/daily/dotnet-sdk-win-arm64.zip.sha512

[win-arm64-badge-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/win_arm64_Release_version_badge.svg?no-cache
[win-arm64-version-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/productCommit-win-arm64.txt
[win-arm64-installer-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-arm64.exe.sha512
[win-arm64-zip-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-10.0.3xx]: https://aka.ms/dotnet/10.0.3xx/daily/dotnet-sdk-win-arm64.zip.sha512

[win-arm64-badge-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/win_arm64_Release_version_badge.svg?no-cache
[win-arm64-version-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/productCommit-win-arm64.txt
[win-arm64-installer-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-arm64.exe
[win-arm64-installer-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-arm64.exe.sha512
[win-arm64-zip-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-arm64.zip
[win-arm64-zip-checksum-10.0.2XX]: https://aka.ms/dotnet/10.0.2XX/daily/dotnet-sdk-win-arm64.zip.sha512
