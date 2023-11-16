# Microsoft.Windows.Compatibility

The `Microsoft.Windows.Compatibility` package provides Windows-specific APIs to help you port your .NET Framework applications to .NET Core 2.0+, .NET 5+ or .NET Standard. This package offers a smoother transition for those looking to modernize their applications without losing access to familiar Windows functionalities.

## Getting Started

To start using the `Microsoft.Windows.Compatibility` package, you'll first need to install it via NuGet Package Manager, Package Manager Console, or by editing your project file.

## Usage

After installing the package, you can access Windows-specific APIs just like you would in a .NET Framework application. Below are some examples in both C# and VB:

### Writing to the Windows Registry

#### C#
```csharp
using Microsoft.Win32;

class Program
{
    static void Main()
    {
        using (RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\MyApp"))
        {
            key.SetValue("MySetting", "MyValue");
        }
    }
}
```

#### VB
```vb
Imports Microsoft.Win32

Module Program
    Sub Main()
        Using key As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\MyApp")
            key.SetValue("MySetting", "MyValue")
        End Using
    End Sub
End Module
```

### Using the Serial Port

#### C#
```csharp
using System.IO.Ports;

class Program
{
    static void Main()
    {
        using (SerialPort port = new SerialPort("COM1", 9600))
        {
            port.Open();
            port.WriteLine("Hello, world!");
        }
    }
}
```

#### VB
```vb
Imports System.IO.Ports

Module Program
    Sub Main()
        Using port As New SerialPort("COM1", 9600)
            port.Open()
            port.WriteLine("Hello, world!")
        End Using
    End Sub
End Module
```

## Additional Documentation

For more in-depth tutorials and API references, you can check the following resources:

- [Use the Windows Compatibility Pack to port code - .NET](https://learn.microsoft.com/dotnet/core/porting/windows-compat-pack)
- [Announcing the Windows Compatibility Pack for .NET Core](https://devblogs.microsoft.com/dotnet/announcing-the-windows-compatibility-pack-for-net-core/)
- [Installing NuGet client tools | Microsoft Learn](https://learn.microsoft.com/nuget/consume-packages/install-use-packages-nuget-cli)

## Feedback

We value your feedback! Here are ways to get in touch with us:

- Open an issue on our [GitHub repository](https://github.com/dotnet/runtime/issues)
- Reach out on Twitter with the [hashtag #dotnet](https://twitter.com/search?q=%23dotnet)
- Join our Discord channel: [dotnet/Discord](https://discord.com/invite/dotnet)
