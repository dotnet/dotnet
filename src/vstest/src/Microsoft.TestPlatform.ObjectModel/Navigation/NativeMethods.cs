// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;

namespace Microsoft.VisualStudio.TestPlatform.ObjectModel.Navigation;

internal static class HResult
{
    public static bool Failed(int hr)
    {
        return hr < 0;
    }

    public static bool Succeeded(int hr)
    {
        return !Failed(hr);
    }
}

/// <summary>
/// Some GUID constants we use to instantiate COM objects.
/// </summary>
internal static class Guids
{
    internal static Guid CLSID_DiaSource = new("79F1BB5F-B66E-48E5-B6A9-1545C323CA3D");
}

/// <summary>
/// DIA's IDiaEnumLineNumbers used for enumerating a symbol's line numbers.
/// </summary>
[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("FE30E878-54AC-44f1-81BA-39DE940F6052")]
internal interface IDiaEnumLineNumbers
{
    int Stub1();

    [PreserveSig]
    int GetCount(out uint count);

    [PreserveSig]
    int GetItem(uint index, out IDiaLineNumber line);

    [PreserveSig]
    int GetNext(uint celt, out IDiaLineNumber rgelt, out uint pceltFetched);

    int Stub5();

    int Stub6();

    int Stub7();
}

/// <summary>
/// DIA's IDiaLineNumber used for retrieving line information.
/// </summary>
[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B388EB14-BE4D-421d-A8A1-6CF7AB057086")]
internal interface IDiaLineNumber
{
    int Stub1();

    [PreserveSig]
    int GetSourceFile(out IDiaSourceFile file);

    [PreserveSig]
    int GetLineNumber(out uint line);

    [PreserveSig]
    int GetLineNumberEnd(out uint line);

    [PreserveSig]
    int GetColumnNumber(out uint line);

    int Stub6();

    int Stub7();

    int Stub8();

    int Stub9();

    int Stub10();

    int Stub11();

    int Stub12();

    int Stub13();

    int Stub14();
}

/// <summary>
/// DIA's IDiaSession used for locating symbols.
/// </summary>
[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("2F609EE1-D1C8-4E24-8288-3326BADCD211")]
internal interface IDiaSession
{
    int Stub1();

    int Stub2();

    [PreserveSig]
    int GetGlobalScope(out IDiaSymbol diaSymbol);

    int Stub4();

    int Stub5();

    int Stub6();

    int Stub7();

    int Stub8();

    int Stub9();

    int Stub10();

    int Stub11();

    int Stub12();

    int Stub13();

    [PreserveSig]
    int FindSymbolByToken(uint token, SymTagEnum tag, out IDiaSymbol symbol);

    int Stub15();

    int Stub16();

    int Stub17();

    int Stub18();

    int Stub19();

    int Stub20();

    int Stub21();

    [PreserveSig]
    int FindLinesByAddress(uint section, uint offset, uint length, out IDiaEnumLineNumbers enumerator);

    int Stub23();

    int Stub24();

    int Stub25();

    int Stub26();

    int Stub27();
}

/// <summary>
/// DIA's IDiaSourceFile used for getting source filenames.
/// </summary>
[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("A2EF5353-F5A8-4eb3-90D2-CB526ACB3CDD")]
internal interface IDiaSourceFile
{
    int Stub1();

    [PreserveSig]
    int GetFilename([MarshalAs(UnmanagedType.BStr)] out string filename);

    int Stub3();

    int Stub4();

    int Stub5();
}

/// <summary>
/// Represents the DIA symbol tags.
/// </summary>
internal enum SymTagEnum : uint
{
    SymTagNull,
    SymTagExe,
    SymTagCompiland,
    SymTagCompilandDetails,
    SymTagCompilandEnv,
    SymTagFunction,
    SymTagBlock,
    SymTagData,
    SymTagAnnotation,
    SymTagLabel,
    SymTagPublicSymbol,
    SymTagUDT,
    SymTagEnum,
    SymTagFunctionType,
    SymTagPointerType,
    SymTagArrayType,
    SymTagBaseType,
    SymTagTypedef,
    SymTagBaseClass,
    SymTagFriend,
    SymTagFunctionArgType,
    SymTagFuncDebugStart,
    SymTagFuncDebugEnd,
    SymTagUsingNamespace,
    SymTagVTableShape,
    SymTagVTable,
    SymTagCustom,
    SymTagThunk,
    SymTagCustomType,
    SymTagManagedType,
    SymTagDimension
}

/// <summary>
/// DIA's IDiaSymbol used for getting the address of function symbols.
/// </summary>
[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("cb787b2f-bd6c-4635-ba52-933126bd2dcd")]
internal interface IDiaSymbol
{
    int Stub1();

    [PreserveSig]
    int GetSymTag(out SymTagEnum tag);

    int GetName(out string name);

    int Stub4();

    int Stub5();

    int Stub6();

    int Stub7();

    int Stub8();

    [PreserveSig]
    int GetAddressSection(out uint section);

    [PreserveSig]
    int GetAddressOffset(out uint offset);

    int Stub11();

    int Stub12();

    int Stub13();

    int Stub14();

    [PreserveSig]
    int GetLength(out long length);

    int Stub16();

    int Stub17();

    int Stub18();

    int Stub19();

    int Stub20();

    int Stub21();

    int Stub22();

    int Stub23();

    int Stub24();

    int Stub25();

    int Stub26();

    int Stub27();

    int Stub28();

    int Stub29();

    int Stub30();

    int Stub31();

    int Stub32();

    int Stub33();

    int Stub34();

    int Stub35();

    int Stub36();

    int Stub37();

    int Stub38();

    int Stub39();

    int Stub40();

    int Stub41();

    int Stub42();

    int Stub43();

    int Stub44();

    int Stub45();

    int Stub46();

    int Stub47();

    int Stub48();

    int Stub49();

    int Stub50();

    int Stub51();

    int Stub52();

    int Stub53();

    int Stub54();

    int Stub55();

    int Stub56();

    int Stub57();

    int Stub58();

    int Stub59();

    int Stub60();

    int Stub61();

    int Stub62();

    int Stub63();

    int Stub64();

    int Stub65();

    int Stub66();

    int Stub67();

    int Stub68();

    int Stub69();

    int Stub70();

    int Stub71();

    int Stub72();

    int Stub73();

    int Stub74();

    int Stub75();

    int Stub76();

    int Stub77();

    int Stub78();

    int Stub79();

    int Stub80();

    int Stub81();

    int Stub82();

    [PreserveSig]
    int FindChildren(SymTagEnum tag, string? str, int flags, out IDiaEnumSymbols symbol);

    int Stub84();

    int Stub85();

    int Stub86();

    int Stub87();

    int Stub88();

    int Stub89();

    int Stub90();

    int Stub91();

    int Stub92();

    int Stub93();

    int Stub94();

    int Stub95();

    int Stub96();

    int Stub97();

    int Stub98();

    int Stub99();

    int Stub100();

    int Stub101();

    int Stub102();

    int Stub103();

    int Stub104();

    int Stub105();

    int Stub106();

    int Stub107();

    int Stub108();

    int Stub109();

    int Stub110();

    int Stub111();

    int Stub112();

    int Stub113();

    int Stub114();

    int Stub115();

    int Stub116();

    int Stub117();

    int Stub118();

    int Stub119();

    int Stub120();

    int Stub121();

    int Stub122();

    int Stub123();

    int Stub124();

    int Stub125();

    int Stub126();

    int Stub127();

    int Stub128();

    int Stub129();

    int Stub130();

    int Stub131();

    int Stub132();

    int Stub133();

    int Stub134();

    int Stub135();

    int Stub136();

    int Stub137();

    int Stub138();

    int Stub139();

    int Stub140();

    int Stub141();

    int Stub142();

    int Stub143();

    int Stub144();

    int Stub145();

    int Stub146();

    int Stub147();

    int Stub148();

    int Stub149();

    int Stub150();

    int Stub151();

    int Stub152();

    int Stub153();

    int Stub154();

    int Stub155();
}

// The definition for DiaSource COM object is present InternalApis\vctools\inc\dia2.h
// The GUID here must match what is present in dia2.h
[ComImport, CoClass(typeof(DiaSourceClass)), Guid("79F1BB5F-B66E-48E5-B6A9-1545C323CA3D")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "COM interface.")]
internal interface DiaSource : IDiaDataSource
{
}

// The definition for DiaSourceClass COM object is present InternalApis\vctools\inc\dia2.h
// The GUID here must match what is present in dia2.h
[ComImport, ClassInterface((short)0), Guid("E6756135-1E65-4D17-8576-610761398C3C")]
internal class DiaSourceClass
{
}

internal static class DiaSourceObject
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr LoadLibraryEx(string lpFileName, IntPtr hReservedNull, int dwFlags);

    public static IDiaDataSource? GetDiaSourceObject()
    {
        var nativeDllDirectory = new ProcessHelper().GetNativeDllDirectory();

        IntPtr modHandle = LoadLibraryEx(Path.Combine(nativeDllDirectory, "msdia140.dll"), IntPtr.Zero, 0);

        if (modHandle == IntPtr.Zero)
        {
            throw new COMException(Resources.Resources.FailedToLoadMsDia);
        }

        var diaSourceClassGuid = new Guid("{E6756135-1E65-4D17-8576-610761398C3C}");
        var comClassFactory = (IClassFactory)DllGetClassObject(diaSourceClassGuid, new Guid("00000001-0000-0000-C000-000000000046"));

        Guid iDataDataSourceGuid = new("79F1BB5F-B66E-48E5-B6A9-1545C323CA3D");
        comClassFactory.CreateInstance(null, ref iDataDataSourceGuid, out object comObject);
        return (comObject as IDiaDataSource);
    }

    [ComImport, ComVisible(false), Guid("00000001-0000-0000-C000-000000000046"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IClassFactory
    {
        void CreateInstance(
            [MarshalAs(UnmanagedType.Interface)] object? aggregator,
            ref Guid refiid,
            [MarshalAs(UnmanagedType.Interface)] out object createdObject);

        void LockServer(bool incrementRefCount);
    }

    [return: MarshalAs(UnmanagedType.Interface)]
    [DllImport("msdia140.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
    internal static extern object DllGetClassObject(
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid rclsid,
        [In, MarshalAs(UnmanagedType.LPStruct)] Guid riid);

}

/// <summary>
/// DIA's IDiaDataSource used for opening symbols.
/// </summary>
[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("79F1BB5F-B66E-48E5-B6A9-1545C323CA3D")]
internal interface IDiaDataSource
{
    int Stub1();

    int Stub2();

    int Stub3();

    [PreserveSig]
    int LoadDataForExe(
        [MarshalAs(UnmanagedType.LPWStr)] string executable,
        [MarshalAs(UnmanagedType.LPWStr)] string? searchPath,
        IntPtr callback);

    int Stub5();

    [PreserveSig]
    int OpenSession(out IDiaSession session);
}

[ComImport, Guid("CAB72C48-443B-48F5-9B0B-42F0820AB29A"), InterfaceType(1)]
internal interface IDiaEnumSymbols
{
    int Stub1();

    [PreserveSig]
    int GetCount(out uint count);

    [PreserveSig]
    int GetItem(uint index, out IDiaSymbol symbol);

    int GetNext(uint index, out IDiaSymbol symbol, out uint pceltFetched);

    int Stub5();

    int Stub6();

    int Stub7();
}
