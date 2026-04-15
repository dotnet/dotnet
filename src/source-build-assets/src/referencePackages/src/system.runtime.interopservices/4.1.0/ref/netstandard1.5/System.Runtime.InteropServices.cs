// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------
[assembly: System.Runtime.CompilerServices.CompilationRelaxations(8)]
[assembly: System.Runtime.CompilerServices.RuntimeCompatibility(WrapNonExceptionThrows = true)]
[assembly: System.Diagnostics.Debuggable(System.Diagnostics.DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints)]
[assembly: System.Security.AllowPartiallyTrustedCallers]
[assembly: System.Runtime.CompilerServices.ReferenceAssembly]
[assembly: System.Reflection.AssemblyTitle("System.Runtime.InteropServices")]
[assembly: System.Reflection.AssemblyDescription("System.Runtime.InteropServices")]
[assembly: System.Reflection.AssemblyDefaultAlias("System.Runtime.InteropServices")]
[assembly: System.Reflection.AssemblyCompany("Microsoft Corporation")]
[assembly: System.Reflection.AssemblyProduct("Microsoft® .NET Framework")]
[assembly: System.Reflection.AssemblyCopyright("© Microsoft Corporation.  All rights reserved.")]
[assembly: System.Reflection.AssemblyFileVersion("1.0.24212.01")]
[assembly: System.Reflection.AssemblyInformationalVersion("1.0.24212.01. Commit Hash: 9688ddbb62c04189cac4c4a06e31e93377dccd41")]
[assembly: System.CLSCompliant(true)]
[assembly: System.Reflection.AssemblyMetadata(".NETFrameworkAssembly", "")]
[assembly: System.Reflection.AssemblyMetadata("Serviceable", "True")]
[assembly: System.Reflection.AssemblyVersionAttribute("4.1.0.0")]
[assembly: System.Reflection.AssemblyFlagsAttribute((System.Reflection.AssemblyNameFlags)0x70)]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.InteropServices.CriticalHandle))]
[assembly: System.Runtime.CompilerServices.TypeForwardedTo(typeof(System.Runtime.InteropServices.SafeHandle))]
namespace System
{
    public sealed partial class DataMisalignedException : Exception
    {
        public DataMisalignedException() { }

        public DataMisalignedException(string message, Exception innerException) { }

        public DataMisalignedException(string message) { }
    }

    public partial class DllNotFoundException : TypeLoadException
    {
        public DllNotFoundException() { }

        public DllNotFoundException(string message, Exception inner) { }

        public DllNotFoundException(string message) { }
    }
}

namespace System.Reflection
{
    public sealed partial class Missing
    {
        internal Missing() { }

        public static readonly Missing Value;
    }
}

namespace System.Runtime.InteropServices
{
    public partial struct ArrayWithOffset
    {
        public ArrayWithOffset(object array, int offset) { }

        public override bool Equals(object obj) { throw null; }

        public bool Equals(ArrayWithOffset obj) { throw null; }

        public object GetArray() { throw null; }

        public override int GetHashCode() { throw null; }

        public int GetOffset() { throw null; }

        public static bool operator ==(ArrayWithOffset a, ArrayWithOffset b) { throw null; }

        public static bool operator !=(ArrayWithOffset a, ArrayWithOffset b) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, Inherited = false)]
    public sealed partial class BestFitMappingAttribute : Attribute
    {
        public bool ThrowOnUnmappableChar;
        public BestFitMappingAttribute(bool BestFitMapping) { }

        public bool BestFitMapping { get { throw null; } }
    }

    [Obsolete("BStrWrapper and support for marshalling to the VARIANT type may be unavailable in future releases.")]
    public sealed partial class BStrWrapper
    {
        public BStrWrapper(object value) { }

        public BStrWrapper(string value) { }

        public string WrappedObject { get { throw null; } }
    }

    public enum CallingConvention
    {
        Winapi = 1,
        Cdecl = 2,
        StdCall = 3,
        ThisCall = 4
    }

    public enum ClassInterfaceType
    {
        None = 0,
        AutoDispatch = 1,
        AutoDual = 2
    }

    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    public sealed partial class CoClassAttribute : Attribute
    {
        public CoClassAttribute(Type coClass) { }

        public Type CoClass { get { throw null; } }
    }

    [Obsolete("ComAwareEventInfo may be unavailable in future releases.")]
    public partial class ComAwareEventInfo : Reflection.EventInfo
    {
        public ComAwareEventInfo(Type type, string eventName) { }

        public override Reflection.EventAttributes Attributes { get { throw null; } }

        public override Type DeclaringType { get { throw null; } }

        public override string Name { get { throw null; } }

        public override void AddEventHandler(object target, Delegate handler) { }

        public override void RemoveEventHandler(object target, Delegate handler) { }

        public override System.Reflection.MethodInfo GetAddMethod(bool nonPublic) { throw null; }
        public override System.Reflection.MethodInfo GetRaiseMethod(bool nonPublic) { throw null; }
        public override System.Reflection.MethodInfo GetRemoveMethod(bool nonPublic) { throw null; }
    }

    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    [Obsolete("ComEventInterfaceAttribute may be unavailable in future releases.")]
    public sealed partial class ComEventInterfaceAttribute : Attribute
    {
        public ComEventInterfaceAttribute(Type SourceInterface, Type EventProvider) { }

        public Type EventProvider { get { throw null; } }

        public Type SourceInterface { get { throw null; } }
    }

    [Obsolete("ComEventsHelper may be unavailable in future releases.")]
    public static partial class ComEventsHelper
    {
        public static void Combine(object rcw, Guid iid, int dispid, Delegate d) { }

        public static Delegate Remove(object rcw, Guid iid, int dispid, Delegate d) { throw null; }
    }

    public partial class COMException : Exception
    {
        public COMException() { }

        public COMException(string message, Exception inner) { }

        public COMException(string message, int errorCode) { }

        public COMException(string message) { }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed partial class ComImportAttribute : Attribute
    {
    }

    public enum ComInterfaceType
    {
        InterfaceIsDual = 0,
        InterfaceIsIUnknown = 1,
        InterfaceIsIDispatch = 2,
        InterfaceIsIInspectable = 3
    }

    public enum ComMemberType
    {
        Method = 0,
        PropGet = 1,
        PropSet = 2
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    [Obsolete("ComSourceInterfacesAttribute may be unavailable in future releases.")]
    public sealed partial class ComSourceInterfacesAttribute : Attribute
    {
        public ComSourceInterfacesAttribute(string sourceInterfaces) { }

        public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3, Type sourceInterface4) { }

        public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2, Type sourceInterface3) { }

        public ComSourceInterfacesAttribute(Type sourceInterface1, Type sourceInterface2) { }

        public ComSourceInterfacesAttribute(Type sourceInterface) { }

        public string Value { get { throw null; } }
    }

    [Obsolete("CurrencyWrapper and support for marshalling to the VARIANT type may be unavailable in future releases.")]
    public sealed partial class CurrencyWrapper
    {
        public CurrencyWrapper(decimal obj) { }

        public CurrencyWrapper(object obj) { }

        public decimal WrappedObject { get { throw null; } }
    }

    [Obsolete("CustomQueryInterfaceMode and support for ICustomQueryInterface may be unavailable in future releases.")]
    public enum CustomQueryInterfaceMode
    {
        Ignore = 0,
        Allow = 1
    }

    [Obsolete("CustomQueryInterfaceResult and support for ICustomQueryInterface may be unavailable in future releases.")]
    public enum CustomQueryInterfaceResult
    {
        Handled = 0,
        NotHandled = 1,
        Failed = 2
    }

    [AttributeUsage(AttributeTargets.Module, Inherited = false)]
    public sealed partial class DefaultCharSetAttribute : Attribute
    {
        public DefaultCharSetAttribute(CharSet charSet) { }

        public CharSet CharSet { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Method, AllowMultiple = false)]
    public sealed partial class DefaultDllImportSearchPathsAttribute : Attribute
    {
        public DefaultDllImportSearchPathsAttribute(DllImportSearchPath paths) { }

        public DllImportSearchPath Paths { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed partial class DefaultParameterValueAttribute : Attribute
    {
        public DefaultParameterValueAttribute(object value) { }

        public object Value { get { throw null; } }
    }

    [Obsolete("DispatchWrapper and support for marshalling to the VARIANT type may be unavailable in future releases.")]
    public sealed partial class DispatchWrapper
    {
        public DispatchWrapper(object obj) { }

        public object WrappedObject { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event, Inherited = false)]
    public sealed partial class DispIdAttribute : Attribute
    {
        public DispIdAttribute(int dispId) { }

        public int Value { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed partial class DllImportAttribute : Attribute
    {
        public bool BestFitMapping;
        public CallingConvention CallingConvention;
        public CharSet CharSet;
        public string EntryPoint;
        public bool ExactSpelling;
        public bool PreserveSig;
        public bool SetLastError;
        public bool ThrowOnUnmappableChar;
        public DllImportAttribute(string dllName) { }

        public string Value { get { throw null; } }
    }

    [Flags]
    public enum DllImportSearchPath
    {
        LegacyBehavior = 0,
        AssemblyDirectory = 2,
        UseDllDirectoryForDependencies = 256,
        ApplicationDirectory = 512,
        UserDirectories = 1024,
        System32 = 2048,
        SafeDirectories = 4096
    }

    [Obsolete("ErrorWrapper and support for marshalling to the VARIANT type may be unavailable in future releases.")]
    public sealed partial class ErrorWrapper
    {
        public ErrorWrapper(Exception e) { }

        public ErrorWrapper(int errorCode) { }

        public ErrorWrapper(object errorCode) { }

        public int ErrorCode { get { throw null; } }
    }

    public partial struct GCHandle
    {
        public bool IsAllocated { get { throw null; } }

        public object Target { get { throw null; } set { } }

        public IntPtr AddrOfPinnedObject() { throw null; }

        public static GCHandle Alloc(object value, GCHandleType type) { throw null; }

        public static GCHandle Alloc(object value) { throw null; }

        public override bool Equals(object o) { throw null; }

        public void Free() { }

        public static GCHandle FromIntPtr(IntPtr value) { throw null; }

        public override int GetHashCode() { throw null; }

        public static bool operator ==(GCHandle a, GCHandle b) { throw null; }

        public static explicit operator GCHandle(IntPtr value) { throw null; }

        public static explicit operator IntPtr(GCHandle value) { throw null; }

        public static bool operator !=(GCHandle a, GCHandle b) { throw null; }

        public static IntPtr ToIntPtr(GCHandle value) { throw null; }
    }

    public enum GCHandleType
    {
        Weak = 0,
        WeakTrackResurrection = 1,
        Normal = 2,
        Pinned = 3
    }

    public sealed partial class HandleCollector
    {
        public HandleCollector(string name, int initialThreshold, int maximumThreshold) { }

        public HandleCollector(string name, int initialThreshold) { }

        public int Count { get { throw null; } }

        public int InitialThreshold { get { throw null; } }

        public int MaximumThreshold { get { throw null; } }

        public string Name { get { throw null; } }

        public void Add() { }

        public void Remove() { }
    }

    [Obsolete("ICustomAdapter may be unavailable in future releases.")]
    public partial interface ICustomAdapter
    {
        object GetUnderlyingObject();
    }

    [Obsolete("ICustomQueryInterface may be unavailable in future releases.")]
    public partial interface ICustomQueryInterface
    {
        CustomQueryInterfaceResult GetInterface(ref Guid iid, out IntPtr ppv);
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class InAttribute : Attribute
    {
    }

    public partial class InvalidComObjectException : Exception
    {
        public InvalidComObjectException() { }

        public InvalidComObjectException(string message, Exception inner) { }

        public InvalidComObjectException(string message) { }
    }

    public partial class InvalidOleVariantTypeException : Exception
    {
        public InvalidOleVariantTypeException() { }

        public InvalidOleVariantTypeException(string message, Exception inner) { }

        public InvalidOleVariantTypeException(string message) { }
    }

    public static partial class Marshal
    {
        public static readonly int SystemDefaultCharSize;
        public static readonly int SystemMaxDBCSCharSize;
        public static int AddRef(IntPtr pUnk) { throw null; }

        public static IntPtr AllocCoTaskMem(int cb) { throw null; }

        public static IntPtr AllocHGlobal(int cb) { throw null; }

        public static IntPtr AllocHGlobal(IntPtr cb) { throw null; }

        public static bool AreComObjectsAvailableForCleanup() { throw null; }

        public static void Copy(byte[] source, int startIndex, IntPtr destination, int length) { }

        public static void Copy(char[] source, int startIndex, IntPtr destination, int length) { }

        public static void Copy(double[] source, int startIndex, IntPtr destination, int length) { }

        public static void Copy(short[] source, int startIndex, IntPtr destination, int length) { }

        public static void Copy(int[] source, int startIndex, IntPtr destination, int length) { }

        public static void Copy(long[] source, int startIndex, IntPtr destination, int length) { }

        public static void Copy(IntPtr source, byte[] destination, int startIndex, int length) { }

        public static void Copy(IntPtr source, char[] destination, int startIndex, int length) { }

        public static void Copy(IntPtr source, double[] destination, int startIndex, int length) { }

        public static void Copy(IntPtr source, short[] destination, int startIndex, int length) { }

        public static void Copy(IntPtr source, int[] destination, int startIndex, int length) { }

        public static void Copy(IntPtr source, long[] destination, int startIndex, int length) { }

        public static void Copy(IntPtr source, IntPtr[] destination, int startIndex, int length) { }

        public static void Copy(IntPtr source, float[] destination, int startIndex, int length) { }

        public static void Copy(IntPtr[] source, int startIndex, IntPtr destination, int length) { }

        public static void Copy(float[] source, int startIndex, IntPtr destination, int length) { }

        [Obsolete("CreateAggregatedObject(IntPtr, Object) may be unavailable in future releases. Instead, use CreateAggregatedObject<T>(IntPtr, T). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296518")]
        public static IntPtr CreateAggregatedObject(IntPtr pOuter, object o) { throw null; }

        public static IntPtr CreateAggregatedObject<T>(IntPtr pOuter, T o) { throw null; }

        [Obsolete("CreateWrapperOfType(Object, Type) may be unavailable in future releases. Instead, use CreateWrapperOfType<T,T2>(T). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296519")]
        public static object CreateWrapperOfType(object o, Type t) { throw null; }

        public static TWrapper CreateWrapperOfType<T, TWrapper>(T o) { throw null; }

        [Obsolete("DestroyStructure(IntPtr, Type) may be unavailable in future releases. Instead, use DestroyStructure<T>(IntPtr). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296520")]
        public static void DestroyStructure(IntPtr ptr, Type structuretype) { }

        public static void DestroyStructure<T>(IntPtr ptr) { }

        public static int FinalReleaseComObject(object o) { throw null; }

        public static void FreeBSTR(IntPtr ptr) { }

        public static void FreeCoTaskMem(IntPtr ptr) { }

        public static void FreeHGlobal(IntPtr hglobal) { }

        [Obsolete("GetComInterfaceForObject(Object, Type, CustomQueryInterfaceMode) and support for ICustomQueryInterface may be unavailable in future releases.")]
        public static IntPtr GetComInterfaceForObject(object o, Type T, CustomQueryInterfaceMode mode) { throw null; }

        [Obsolete("GetComInterfaceForObject(Object, Type) may be unavailable in future releases. Instead, use GetComInterfaceForObject<T,T2>(T). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296509")]
        public static IntPtr GetComInterfaceForObject(object o, Type T) { throw null; }

        public static IntPtr GetComInterfaceForObject<T, TInterface>(T o) { throw null; }

        [Obsolete("GetDelegateForFunctionPointer(IntPtr, Type) may be unavailable in future releases. Instead, use GetDelegateForFunctionPointer<T>(IntPtr). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296521")]
        public static Delegate GetDelegateForFunctionPointer(IntPtr ptr, Type t) { throw null; }

        public static TDelegate GetDelegateForFunctionPointer<TDelegate>(IntPtr ptr) { throw null; }

        [Obsolete("GetExceptionCode() may be unavailable in future releases.")]
        public static int GetExceptionCode() { throw null; }

        public static Exception GetExceptionForHR(int errorCode, IntPtr errorInfo) { throw null; }

        public static Exception GetExceptionForHR(int errorCode) { throw null; }

        [Obsolete("GetFunctionPointerForDelegate(Delegate) may be unavailable in future releases. Instead, use GetFunctionPointerForDelegate<T>(T). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296522")]
        public static IntPtr GetFunctionPointerForDelegate(Delegate d) { throw null; }

        public static IntPtr GetFunctionPointerForDelegate<TDelegate>(TDelegate d) { throw null; }

        public static int GetHRForException(Exception e) { throw null; }

        public static int GetHRForLastWin32Error() { throw null; }

        public static IntPtr GetIUnknownForObject(object o) { throw null; }

        public static int GetLastWin32Error() { throw null; }

        [Obsolete("GetNativeVariantForObject(Object, IntPtr) may be unavailable in future releases.")]
        public static void GetNativeVariantForObject(object obj, IntPtr pDstNativeVariant) { }

        [Obsolete("GetNativeVariantForObject<T>(T, IntPtr) may be unavailable in future releases.")]
        public static void GetNativeVariantForObject<T>(T obj, IntPtr pDstNativeVariant) { }

        public static object GetObjectForIUnknown(IntPtr pUnk) { throw null; }

        [Obsolete("GetObjectForNativeVariant(IntPtr) may be unavailable in future releases.")]
        public static object GetObjectForNativeVariant(IntPtr pSrcNativeVariant) { throw null; }

        [Obsolete("GetObjectForNativeVariant<T>(IntPtr) may be unavailable in future releases.")]
        public static T GetObjectForNativeVariant<T>(IntPtr pSrcNativeVariant) { throw null; }

        [Obsolete("GetObjectsForNativeVariants(IntPtr, Int32) may be unavailable in future releases.")]
        public static object[] GetObjectsForNativeVariants(IntPtr aSrcNativeVariant, int cVars) { throw null; }

        [Obsolete("GetObjectsForNativeVariants<T>(IntPtr, Int32) may be unavailable in future releases.")]
        public static T[] GetObjectsForNativeVariants<T>(IntPtr aSrcNativeVariant, int cVars) { throw null; }

        public static int GetStartComSlot(Type t) { throw null; }

        public static Type GetTypeFromCLSID(Guid clsid) { throw null; }

        public static string GetTypeInfoName(ComTypes.ITypeInfo typeInfo) { throw null; }

        public static object GetUniqueObjectForIUnknown(IntPtr unknown) { throw null; }

        public static bool IsComObject(object o) { throw null; }

        [Obsolete("OffsetOf(Type, string) may be unavailable in future releases. Instead, use OffsetOf<T>(string). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296511")]
        public static IntPtr OffsetOf(Type t, string fieldName) { throw null; }

        public static IntPtr OffsetOf<T>(string fieldName) { throw null; }

        public static string PtrToStringAnsi(IntPtr ptr, int len) { throw null; }

        public static string PtrToStringAnsi(IntPtr ptr) { throw null; }

        public static string PtrToStringBSTR(IntPtr ptr) { throw null; }

        public static string PtrToStringUni(IntPtr ptr, int len) { throw null; }

        public static string PtrToStringUni(IntPtr ptr) { throw null; }

        [Obsolete("PtrToStructure(IntPtr, Object) may be unavailable in future releases. Instead, use PtrToStructure<T>(IntPtr). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296512")]
        public static void PtrToStructure(IntPtr ptr, object structure) { }

        [Obsolete("PtrToStructure(IntPtr, Type) may be unavailable in future releases. Instead, use PtrToStructure<T>(IntPtr). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296513")]
        public static object PtrToStructure(IntPtr ptr, Type structureType) { throw null; }

        public static void PtrToStructure<T>(IntPtr ptr, T structure) { }

        public static T PtrToStructure<T>(IntPtr ptr) { throw null; }

        public static int QueryInterface(IntPtr pUnk, ref Guid iid, out IntPtr ppv) { throw null; }

        public static byte ReadByte(IntPtr ptr, int ofs) { throw null; }

        public static byte ReadByte(IntPtr ptr) { throw null; }

        [Obsolete("ReadByte(Object, Int32) may be unavailable in future releases.")]
        public static byte ReadByte(object ptr, int ofs) { throw null; }

        public static short ReadInt16(IntPtr ptr, int ofs) { throw null; }

        public static short ReadInt16(IntPtr ptr) { throw null; }

        [Obsolete("ReadInt16(Object, Int32) may be unavailable in future releases.")]
        public static short ReadInt16(object ptr, int ofs) { throw null; }

        public static int ReadInt32(IntPtr ptr, int ofs) { throw null; }

        public static int ReadInt32(IntPtr ptr) { throw null; }

        [Obsolete("ReadInt32(Object, Int32) may be unavailable in future releases.")]
        public static int ReadInt32(object ptr, int ofs) { throw null; }

        public static long ReadInt64(IntPtr ptr, int ofs) { throw null; }

        public static long ReadInt64(IntPtr ptr) { throw null; }

        [Obsolete("ReadInt64(Object, Int32) may be unavailable in future releases.")]
        public static long ReadInt64(object ptr, int ofs) { throw null; }

        public static IntPtr ReadIntPtr(IntPtr ptr, int ofs) { throw null; }

        public static IntPtr ReadIntPtr(IntPtr ptr) { throw null; }

        [Obsolete("ReadIntPtr(Object, Int32) may be unavailable in future releases.")]
        public static IntPtr ReadIntPtr(object ptr, int ofs) { throw null; }

        public static IntPtr ReAllocCoTaskMem(IntPtr pv, int cb) { throw null; }

        public static IntPtr ReAllocHGlobal(IntPtr pv, IntPtr cb) { throw null; }

        public static int Release(IntPtr pUnk) { throw null; }

        public static int ReleaseComObject(object o) { throw null; }

        [Obsolete("SizeOf(Object) may be unavailable in future releases. Instead, use SizeOf<T>(). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296514")]
        public static int SizeOf(object structure) { throw null; }

        [Obsolete("SizeOf(Type) may be unavailable in future releases. Instead, use SizeOf<T>(). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296515")]
        public static int SizeOf(Type t) { throw null; }

        public static int SizeOf<T>() { throw null; }

        public static int SizeOf<T>(T structure) { throw null; }

        public static IntPtr StringToBSTR(string s) { throw null; }

        public static IntPtr StringToCoTaskMemAnsi(string s) { throw null; }

        public static IntPtr StringToCoTaskMemUni(string s) { throw null; }

        public static IntPtr StringToHGlobalAnsi(string s) { throw null; }

        public static IntPtr StringToHGlobalUni(string s) { throw null; }

        [Obsolete("StructureToPtr(Object, IntPtr, Boolean) may be unavailable in future releases. Instead, use StructureToPtr<T>(T, IntPtr, Boolean). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296516")]
        public static void StructureToPtr(object structure, IntPtr ptr, bool fDeleteOld) { }

        public static void StructureToPtr<T>(T structure, IntPtr ptr, bool fDeleteOld) { }

        public static void ThrowExceptionForHR(int errorCode, IntPtr errorInfo) { }

        public static void ThrowExceptionForHR(int errorCode) { }

        [Obsolete("UnsafeAddrOfPinnedArrayElement(Array, Int32) may be unavailable in future releases. Instead, use UnsafeAddrOfPinnedArrayElement<T>(T[], Int32). For more info, go to http://go.microsoft.com/fwlink/?LinkID=296517")]
        public static IntPtr UnsafeAddrOfPinnedArrayElement(Array arr, int index) { throw null; }

        public static IntPtr UnsafeAddrOfPinnedArrayElement<T>(T[] arr, int index) { throw null; }

        public static void WriteByte(IntPtr ptr, byte val) { }

        public static void WriteByte(IntPtr ptr, int ofs, byte val) { }

        [Obsolete("WriteByte(Object, Int32, Byte) may be unavailable in future releases.")]
        public static void WriteByte(object ptr, int ofs, byte val) { }

        public static void WriteInt16(IntPtr ptr, char val) { }

        public static void WriteInt16(IntPtr ptr, short val) { }

        public static void WriteInt16(IntPtr ptr, int ofs, char val) { }

        public static void WriteInt16(IntPtr ptr, int ofs, short val) { }

        [Obsolete("WriteInt16(Object, Int32, Char) may be unavailable in future releases.")]
        public static void WriteInt16(object ptr, int ofs, char val) { }

        [Obsolete("WriteInt16(Object, Int32, Int16) may be unavailable in future releases.")]
        public static void WriteInt16(object ptr, int ofs, short val) { }

        public static void WriteInt32(IntPtr ptr, int ofs, int val) { }

        public static void WriteInt32(IntPtr ptr, int val) { }

        [Obsolete("WriteInt32(Object, Int32, Int32) may be unavailable in future releases.")]
        public static void WriteInt32(object ptr, int ofs, int val) { }

        public static void WriteInt64(IntPtr ptr, int ofs, long val) { }

        public static void WriteInt64(IntPtr ptr, long val) { }

        [Obsolete("WriteInt64(Object, Int32, Int64) may be unavailable in future releases.")]
        public static void WriteInt64(object ptr, int ofs, long val) { }

        public static void WriteIntPtr(IntPtr ptr, int ofs, IntPtr val) { }

        public static void WriteIntPtr(IntPtr ptr, IntPtr val) { }

        [Obsolete("WriteIntPtr(Object, Int32, IntPtr) may be unavailable in future releases.")]
        public static void WriteIntPtr(object ptr, int ofs, IntPtr val) { }

        public static void ZeroFreeBSTR(IntPtr s) { }

        public static void ZeroFreeCoTaskMemAnsi(IntPtr s) { }

        public static void ZeroFreeCoTaskMemUnicode(IntPtr s) { }

        public static void ZeroFreeGlobalAllocAnsi(IntPtr s) { }

        public static void ZeroFreeGlobalAllocUnicode(IntPtr s) { }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
    public sealed partial class MarshalAsAttribute : Attribute
    {
        public UnmanagedType ArraySubType;
        public int IidParameterIndex;
        public string MarshalCookie;
        public string MarshalType;
        public Type MarshalTypeRef;
        public VarEnum SafeArraySubType;
        public Type SafeArrayUserDefinedSubType;
        public int SizeConst;
        public short SizeParamIndex;
        public MarshalAsAttribute(short unmanagedType) { }

        public MarshalAsAttribute(UnmanagedType unmanagedType) { }

        public UnmanagedType Value { get { throw null; } }
    }

    public partial class MarshalDirectiveException : Exception
    {
        public MarshalDirectiveException() { }

        public MarshalDirectiveException(string message, Exception inner) { }

        public MarshalDirectiveException(string message) { }
    }

    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed partial class OptionalAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed partial class PreserveSigAttribute : Attribute
    {
    }

    public partial class SafeArrayRankMismatchException : Exception
    {
        public SafeArrayRankMismatchException() { }

        public SafeArrayRankMismatchException(string message, Exception inner) { }

        public SafeArrayRankMismatchException(string message) { }
    }

    public partial class SafeArrayTypeMismatchException : Exception
    {
        public SafeArrayTypeMismatchException() { }

        public SafeArrayTypeMismatchException(string message, Exception inner) { }

        public SafeArrayTypeMismatchException(string message) { }
    }

    public abstract partial class SafeBuffer : SafeHandle
    {
        protected SafeBuffer(bool ownsHandle) : base(default, default) { }

        [CLSCompliant(false)]
        public ulong ByteLength { get { throw null; } }

        public override bool IsInvalid { get { throw null; } }

        [CLSCompliant(false)]
        public unsafe void AcquirePointer(ref byte* pointer) { }

        [CLSCompliant(false)]
        public void Initialize(uint numElements, uint sizeOfEachElement) { }

        [CLSCompliant(false)]
        public void Initialize(ulong numBytes) { }

        [CLSCompliant(false)]
        public void Initialize<T>(uint numElements)
            where T : struct { }

        [CLSCompliant(false)]
        public T Read<T>(ulong byteOffset)
            where T : struct { throw null; }

        [CLSCompliant(false)]
        public void ReadArray<T>(ulong byteOffset, T[] array, int index, int count)
            where T : struct { }

        public void ReleasePointer() { }

        [CLSCompliant(false)]
        public void Write<T>(ulong byteOffset, T value)
            where T : struct { }

        [CLSCompliant(false)]
        public void WriteArray<T>(ulong byteOffset, T[] array, int index, int count)
            where T : struct { }
    }

    public partial class SEHException : Exception
    {
        public SEHException() { }

        public SEHException(string message, Exception inner) { }

        public SEHException(string message) { }

        public virtual bool CanResume() { throw null; }
    }

    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public sealed partial class TypeIdentifierAttribute : Attribute
    {
        public TypeIdentifierAttribute() { }

        public TypeIdentifierAttribute(string scope, string identifier) { }

        public string Identifier { get { throw null; } }

        public string Scope { get { throw null; } }
    }

    [Obsolete("UnknownWrapper and support for marshalling to the VARIANT type may be unavailable in future releases.")]
    public sealed partial class UnknownWrapper
    {
        public UnknownWrapper(object obj) { }

        public object WrappedObject { get { throw null; } }
    }

    [AttributeUsage(AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
    public sealed partial class UnmanagedFunctionPointerAttribute : Attribute
    {
        public bool BestFitMapping;
        public CharSet CharSet;
        public bool SetLastError;
        public bool ThrowOnUnmappableChar;
        public UnmanagedFunctionPointerAttribute(CallingConvention callingConvention) { }

        public CallingConvention CallingConvention { get { throw null; } }
    }

    public enum UnmanagedType
    {
        Bool = 2,
        I1 = 3,
        U1 = 4,
        I2 = 5,
        U2 = 6,
        I4 = 7,
        U4 = 8,
        I8 = 9,
        U8 = 10,
        R4 = 11,
        R8 = 12,
        Currency = 15,
        BStr = 19,
        LPStr = 20,
        LPWStr = 21,
        LPTStr = 22,
        ByValTStr = 23,
        IUnknown = 25,
        IDispatch = 26,
        Struct = 27,
        Interface = 28,
        SafeArray = 29,
        ByValArray = 30,
        SysInt = 31,
        SysUInt = 32,
        VBByRefStr = 34,
        AnsiBStr = 35,
        TBStr = 36,
        VariantBool = 37,
        FunctionPtr = 38,
        AsAny = 40,
        LPArray = 42,
        LPStruct = 43,
        Error = 45,
        IInspectable = 46,
        HString = 47
    }

    [Obsolete("Marshalling VARIANTs may be unavailable in future releases.")]
    public enum VarEnum
    {
        VT_EMPTY = 0,
        VT_NULL = 1,
        VT_I2 = 2,
        VT_I4 = 3,
        VT_R4 = 4,
        VT_R8 = 5,
        VT_CY = 6,
        VT_DATE = 7,
        VT_BSTR = 8,
        VT_DISPATCH = 9,
        VT_ERROR = 10,
        VT_BOOL = 11,
        VT_VARIANT = 12,
        VT_UNKNOWN = 13,
        VT_DECIMAL = 14,
        VT_I1 = 16,
        VT_UI1 = 17,
        VT_UI2 = 18,
        VT_UI4 = 19,
        VT_I8 = 20,
        VT_UI8 = 21,
        VT_INT = 22,
        VT_UINT = 23,
        VT_VOID = 24,
        VT_HRESULT = 25,
        VT_PTR = 26,
        VT_SAFEARRAY = 27,
        VT_CARRAY = 28,
        VT_USERDEFINED = 29,
        VT_LPSTR = 30,
        VT_LPWSTR = 31,
        VT_RECORD = 36,
        VT_FILETIME = 64,
        VT_BLOB = 65,
        VT_STREAM = 66,
        VT_STORAGE = 67,
        VT_STREAMED_OBJECT = 68,
        VT_STORED_OBJECT = 69,
        VT_BLOB_OBJECT = 70,
        VT_CF = 71,
        VT_CLSID = 72,
        VT_VECTOR = 4096,
        VT_ARRAY = 8192,
        VT_BYREF = 16384
    }

    [Obsolete("VariantWrapper and support for marshalling to the VARIANT type may be unavailable in future releases.")]
    public sealed partial class VariantWrapper
    {
        public VariantWrapper(object obj) { }

        public object WrappedObject { get { throw null; } }
    }
}

namespace System.Runtime.InteropServices.ComTypes
{
    [Flags]
    public enum ADVF
    {
        ADVF_NODATA = 1,
        ADVF_PRIMEFIRST = 2,
        ADVF_ONLYONCE = 4,
        ADVFCACHE_NOHANDLER = 8,
        ADVFCACHE_FORCEBUILTIN = 16,
        ADVFCACHE_ONSAVE = 32,
        ADVF_DATAONSTOP = 64
    }

    public partial struct BINDPTR
    {
        public IntPtr lpfuncdesc;
        public IntPtr lptcomp;
        public IntPtr lpvardesc;
    }

    public partial struct BIND_OPTS
    {
        public int cbStruct;
        public int dwTickCountDeadline;
        public int grfFlags;
        public int grfMode;
    }

    public enum CALLCONV
    {
        CC_CDECL = 1,
        CC_MSCPASCAL = 2,
        CC_PASCAL = 2,
        CC_MACPASCAL = 3,
        CC_STDCALL = 4,
        CC_RESERVED = 5,
        CC_SYSCALL = 6,
        CC_MPWCDECL = 7,
        CC_MPWPASCAL = 8,
        CC_MAX = 9
    }

    public partial struct CONNECTDATA
    {
        public int dwCookie;
        public object pUnk;
    }

    public enum DATADIR
    {
        DATADIR_GET = 1,
        DATADIR_SET = 2
    }

    public enum DESCKIND
    {
        DESCKIND_NONE = 0,
        DESCKIND_FUNCDESC = 1,
        DESCKIND_VARDESC = 2,
        DESCKIND_TYPECOMP = 3,
        DESCKIND_IMPLICITAPPOBJ = 4,
        DESCKIND_MAX = 5
    }

    public partial struct DISPPARAMS
    {
        public int cArgs;
        public int cNamedArgs;
        public IntPtr rgdispidNamedArgs;
        public IntPtr rgvarg;
    }

    [Flags]
    public enum DVASPECT
    {
        DVASPECT_CONTENT = 1,
        DVASPECT_THUMBNAIL = 2,
        DVASPECT_ICON = 4,
        DVASPECT_DOCPRINT = 8
    }

    public partial struct ELEMDESC
    {
        public DESCUNION desc;
        public TYPEDESC tdesc;
        public partial struct DESCUNION
        {
            public IDLDESC idldesc;
            public PARAMDESC paramdesc;
        }
    }

    public partial struct EXCEPINFO
    {
        public string bstrDescription;
        public string bstrHelpFile;
        public string bstrSource;
        public int dwHelpContext;
        public IntPtr pfnDeferredFillIn;
        public IntPtr pvReserved;
        public int scode;
        public short wCode;
        public short wReserved;
    }

    public partial struct FILETIME
    {
        public int dwHighDateTime;
        public int dwLowDateTime;
    }

    public partial struct FORMATETC
    {
        public short cfFormat;
        public DVASPECT dwAspect;
        public int lindex;
        public IntPtr ptd;
        public TYMED tymed;
    }

    public partial struct FUNCDESC
    {
        public CALLCONV callconv;
        public short cParams;
        public short cParamsOpt;
        public short cScodes;
        public ELEMDESC elemdescFunc;
        public FUNCKIND funckind;
        public INVOKEKIND invkind;
        public IntPtr lprgelemdescParam;
        public IntPtr lprgscode;
        public int memid;
        public short oVft;
        public short wFuncFlags;
    }

    [Flags]
    public enum FUNCFLAGS : short
    {
        FUNCFLAG_FRESTRICTED = 1,
        FUNCFLAG_FSOURCE = 2,
        FUNCFLAG_FBINDABLE = 4,
        FUNCFLAG_FREQUESTEDIT = 8,
        FUNCFLAG_FDISPLAYBIND = 16,
        FUNCFLAG_FDEFAULTBIND = 32,
        FUNCFLAG_FHIDDEN = 64,
        FUNCFLAG_FUSESGETLASTERROR = 128,
        FUNCFLAG_FDEFAULTCOLLELEM = 256,
        FUNCFLAG_FUIDEFAULT = 512,
        FUNCFLAG_FNONBROWSABLE = 1024,
        FUNCFLAG_FREPLACEABLE = 2048,
        FUNCFLAG_FIMMEDIATEBIND = 4096
    }

    public enum FUNCKIND
    {
        FUNC_VIRTUAL = 0,
        FUNC_PUREVIRTUAL = 1,
        FUNC_NONVIRTUAL = 2,
        FUNC_STATIC = 3,
        FUNC_DISPATCH = 4
    }

    public partial interface IAdviseSink
    {
        void OnClose();
        void OnDataChange(ref FORMATETC format, ref STGMEDIUM stgmedium);
        void OnRename(IMoniker moniker);
        void OnSave();
        void OnViewChange(int aspect, int index);
    }

    public partial interface IBindCtx
    {
        void EnumObjectParam(out IEnumString ppenum);
        void GetBindOptions(ref BIND_OPTS pbindopts);
        void GetObjectParam(string pszKey, out object ppunk);
        void GetRunningObjectTable(out IRunningObjectTable pprot);
        void RegisterObjectBound(object punk);
        void RegisterObjectParam(string pszKey, object punk);
        void ReleaseBoundObjects();
        void RevokeObjectBound(object punk);
        int RevokeObjectParam(string pszKey);
        void SetBindOptions(ref BIND_OPTS pbindopts);
    }

    public partial interface IConnectionPoint
    {
        void Advise(object pUnkSink, out int pdwCookie);
        void EnumConnections(out IEnumConnections ppEnum);
        void GetConnectionInterface(out Guid pIID);
        void GetConnectionPointContainer(out IConnectionPointContainer ppCPC);
        void Unadvise(int dwCookie);
    }

    public partial interface IConnectionPointContainer
    {
        void EnumConnectionPoints(out IEnumConnectionPoints ppEnum);
        void FindConnectionPoint(ref Guid riid, out IConnectionPoint ppCP);
    }

    public partial struct IDLDESC
    {
        public IntPtr dwReserved;
        public IDLFLAG wIDLFlags;
    }

    [Flags]
    public enum IDLFLAG : short
    {
        IDLFLAG_NONE = 0,
        IDLFLAG_FIN = 1,
        IDLFLAG_FOUT = 2,
        IDLFLAG_FLCID = 4,
        IDLFLAG_FRETVAL = 8
    }

    public partial interface IEnumConnectionPoints
    {
        void Clone(out IEnumConnectionPoints ppenum);
        int Next(int celt, IConnectionPoint[] rgelt, IntPtr pceltFetched);
        void Reset();
        int Skip(int celt);
    }

    public partial interface IEnumConnections
    {
        void Clone(out IEnumConnections ppenum);
        int Next(int celt, CONNECTDATA[] rgelt, IntPtr pceltFetched);
        void Reset();
        int Skip(int celt);
    }

    public partial interface IEnumFORMATETC
    {
        void Clone(out IEnumFORMATETC newEnum);
        int Next(int celt, FORMATETC[] rgelt, int[] pceltFetched);
        int Reset();
        int Skip(int celt);
    }

    public partial interface IEnumMoniker
    {
        void Clone(out IEnumMoniker ppenum);
        int Next(int celt, IMoniker[] rgelt, IntPtr pceltFetched);
        void Reset();
        int Skip(int celt);
    }

    public partial interface IEnumString
    {
        void Clone(out IEnumString ppenum);
        int Next(int celt, string[] rgelt, IntPtr pceltFetched);
        void Reset();
        int Skip(int celt);
    }

    public partial interface IEnumVARIANT
    {
        IEnumVARIANT Clone();
        int Next(int celt, object[] rgVar, IntPtr pceltFetched);
        int Reset();
        int Skip(int celt);
    }

    public partial interface IMoniker
    {
        void BindToObject(IBindCtx pbc, IMoniker pmkToLeft, ref Guid riidResult, out object ppvResult);
        void BindToStorage(IBindCtx pbc, IMoniker pmkToLeft, ref Guid riid, out object ppvObj);
        void CommonPrefixWith(IMoniker pmkOther, out IMoniker ppmkPrefix);
        void ComposeWith(IMoniker pmkRight, bool fOnlyIfNotGeneric, out IMoniker ppmkComposite);
        void Enum(bool fForward, out IEnumMoniker ppenumMoniker);
        void GetClassID(out Guid pClassID);
        void GetDisplayName(IBindCtx pbc, IMoniker pmkToLeft, out string ppszDisplayName);
        void GetSizeMax(out long pcbSize);
        void GetTimeOfLastChange(IBindCtx pbc, IMoniker pmkToLeft, out FILETIME pFileTime);
        void Hash(out int pdwHash);
        void Inverse(out IMoniker ppmk);
        int IsDirty();
        int IsEqual(IMoniker pmkOtherMoniker);
        int IsRunning(IBindCtx pbc, IMoniker pmkToLeft, IMoniker pmkNewlyRunning);
        int IsSystemMoniker(out int pdwMksys);
        void Load(IStream pStm);
        void ParseDisplayName(IBindCtx pbc, IMoniker pmkToLeft, string pszDisplayName, out int pchEaten, out IMoniker ppmkOut);
        void Reduce(IBindCtx pbc, int dwReduceHowFar, ref IMoniker ppmkToLeft, out IMoniker ppmkReduced);
        void RelativePathTo(IMoniker pmkOther, out IMoniker ppmkRelPath);
        void Save(IStream pStm, bool fClearDirty);
    }

    [Flags]
    public enum IMPLTYPEFLAGS
    {
        IMPLTYPEFLAG_FDEFAULT = 1,
        IMPLTYPEFLAG_FSOURCE = 2,
        IMPLTYPEFLAG_FRESTRICTED = 4,
        IMPLTYPEFLAG_FDEFAULTVTABLE = 8
    }

    [Flags]
    public enum INVOKEKIND
    {
        INVOKE_FUNC = 1,
        INVOKE_PROPERTYGET = 2,
        INVOKE_PROPERTYPUT = 4,
        INVOKE_PROPERTYPUTREF = 8
    }

    public partial interface IPersistFile
    {
        void GetClassID(out Guid pClassID);
        void GetCurFile(out string ppszFileName);
        int IsDirty();
        void Load(string pszFileName, int dwMode);
        void Save(string pszFileName, bool fRemember);
        void SaveCompleted(string pszFileName);
    }

    public partial interface IRunningObjectTable
    {
        void EnumRunning(out IEnumMoniker ppenumMoniker);
        int GetObject(IMoniker pmkObjectName, out object ppunkObject);
        int GetTimeOfLastChange(IMoniker pmkObjectName, out FILETIME pfiletime);
        int IsRunning(IMoniker pmkObjectName);
        void NoteChangeTime(int dwRegister, ref FILETIME pfiletime);
        int Register(int grfFlags, object punkObject, IMoniker pmkObjectName);
        void Revoke(int dwRegister);
    }

    public partial interface IStream
    {
        void Clone(out IStream ppstm);
        void Commit(int grfCommitFlags);
        void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);
        void LockRegion(long libOffset, long cb, int dwLockType);
        void Read(byte[] pv, int cb, IntPtr pcbRead);
        void Revert();
        void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);
        void SetSize(long libNewSize);
        void Stat(out STATSTG pstatstg, int grfStatFlag);
        void UnlockRegion(long libOffset, long cb, int dwLockType);
        void Write(byte[] pv, int cb, IntPtr pcbWritten);
    }

    public partial interface ITypeComp
    {
        void Bind(string szName, int lHashVal, short wFlags, out ITypeInfo ppTInfo, out DESCKIND pDescKind, out BINDPTR pBindPtr);
        void BindType(string szName, int lHashVal, out ITypeInfo ppTInfo, out ITypeComp ppTComp);
    }

    public partial interface ITypeInfo
    {
        void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);
        void CreateInstance(object pUnkOuter, ref Guid riid, out object ppvObj);
        void GetContainingTypeLib(out ITypeLib ppTLB, out int pIndex);
        void GetDllEntry(int memid, INVOKEKIND invKind, IntPtr pBstrDllName, IntPtr pBstrName, IntPtr pwOrdinal);
        void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);
        void GetFuncDesc(int index, out IntPtr ppFuncDesc);
        void GetIDsOfNames(string[] rgszNames, int cNames, int[] pMemId);
        void GetImplTypeFlags(int index, out IMPLTYPEFLAGS pImplTypeFlags);
        void GetMops(int memid, out string pBstrMops);
        void GetNames(int memid, string[] rgBstrNames, int cMaxNames, out int pcNames);
        void GetRefTypeInfo(int hRef, out ITypeInfo ppTI);
        void GetRefTypeOfImplType(int index, out int href);
        void GetTypeAttr(out IntPtr ppTypeAttr);
        void GetTypeComp(out ITypeComp ppTComp);
        void GetVarDesc(int index, out IntPtr ppVarDesc);
        void Invoke(object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, out int puArgErr);
        void ReleaseFuncDesc(IntPtr pFuncDesc);
        void ReleaseTypeAttr(IntPtr pTypeAttr);
        void ReleaseVarDesc(IntPtr pVarDesc);
    }

    public partial interface ITypeInfo2 : ITypeInfo
    {
        void AddressOfMember(int memid, INVOKEKIND invKind, out IntPtr ppv);
        void CreateInstance(object pUnkOuter, ref Guid riid, out object ppvObj);
        void GetAllCustData(IntPtr pCustData);
        void GetAllFuncCustData(int index, IntPtr pCustData);
        void GetAllImplTypeCustData(int index, IntPtr pCustData);
        void GetAllParamCustData(int indexFunc, int indexParam, IntPtr pCustData);
        void GetAllVarCustData(int index, IntPtr pCustData);
        void GetContainingTypeLib(out ITypeLib ppTLB, out int pIndex);
        void GetCustData(ref Guid guid, out object pVarVal);
        void GetDllEntry(int memid, INVOKEKIND invKind, IntPtr pBstrDllName, IntPtr pBstrName, IntPtr pwOrdinal);
        void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);
        void GetDocumentation2(int memid, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);
        void GetFuncCustData(int index, ref Guid guid, out object pVarVal);
        void GetFuncDesc(int index, out IntPtr ppFuncDesc);
        void GetFuncIndexOfMemId(int memid, INVOKEKIND invKind, out int pFuncIndex);
        void GetIDsOfNames(string[] rgszNames, int cNames, int[] pMemId);
        void GetImplTypeCustData(int index, ref Guid guid, out object pVarVal);
        void GetImplTypeFlags(int index, out IMPLTYPEFLAGS pImplTypeFlags);
        void GetMops(int memid, out string pBstrMops);
        void GetNames(int memid, string[] rgBstrNames, int cMaxNames, out int pcNames);
        void GetParamCustData(int indexFunc, int indexParam, ref Guid guid, out object pVarVal);
        void GetRefTypeInfo(int hRef, out ITypeInfo ppTI);
        void GetRefTypeOfImplType(int index, out int href);
        void GetTypeAttr(out IntPtr ppTypeAttr);
        void GetTypeComp(out ITypeComp ppTComp);
        void GetTypeFlags(out int pTypeFlags);
        void GetTypeKind(out TYPEKIND pTypeKind);
        void GetVarCustData(int index, ref Guid guid, out object pVarVal);
        void GetVarDesc(int index, out IntPtr ppVarDesc);
        void GetVarIndexOfMemId(int memid, out int pVarIndex);
        void Invoke(object pvInstance, int memid, short wFlags, ref DISPPARAMS pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, out int puArgErr);
        void ReleaseFuncDesc(IntPtr pFuncDesc);
        void ReleaseTypeAttr(IntPtr pTypeAttr);
        void ReleaseVarDesc(IntPtr pVarDesc);
    }

    public partial interface ITypeLib
    {
        void FindName(string szNameBuf, int lHashVal, ITypeInfo[] ppTInfo, int[] rgMemId, ref short pcFound);
        void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);
        void GetLibAttr(out IntPtr ppTLibAttr);
        void GetTypeComp(out ITypeComp ppTComp);
        void GetTypeInfo(int index, out ITypeInfo ppTI);
        int GetTypeInfoCount();
        void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);
        void GetTypeInfoType(int index, out TYPEKIND pTKind);
        bool IsName(string szNameBuf, int lHashVal);
        void ReleaseTLibAttr(IntPtr pTLibAttr);
    }

    public partial interface ITypeLib2 : ITypeLib
    {
        void FindName(string szNameBuf, int lHashVal, ITypeInfo[] ppTInfo, int[] rgMemId, ref short pcFound);
        void GetAllCustData(IntPtr pCustData);
        void GetCustData(ref Guid guid, out object pVarVal);
        void GetDocumentation(int index, out string strName, out string strDocString, out int dwHelpContext, out string strHelpFile);
        void GetDocumentation2(int index, out string pbstrHelpString, out int pdwHelpStringContext, out string pbstrHelpStringDll);
        void GetLibAttr(out IntPtr ppTLibAttr);
        void GetLibStatistics(IntPtr pcUniqueNames, out int pcchUniqueNames);
        void GetTypeComp(out ITypeComp ppTComp);
        void GetTypeInfo(int index, out ITypeInfo ppTI);
        int GetTypeInfoCount();
        void GetTypeInfoOfGuid(ref Guid guid, out ITypeInfo ppTInfo);
        void GetTypeInfoType(int index, out TYPEKIND pTKind);
        bool IsName(string szNameBuf, int lHashVal);
        void ReleaseTLibAttr(IntPtr pTLibAttr);
    }

    [Flags]
    public enum LIBFLAGS : short
    {
        LIBFLAG_FRESTRICTED = 1,
        LIBFLAG_FCONTROL = 2,
        LIBFLAG_FHIDDEN = 4,
        LIBFLAG_FHASDISKIMAGE = 8
    }

    public partial struct PARAMDESC
    {
        public IntPtr lpVarValue;
        public PARAMFLAG wParamFlags;
    }

    [Flags]
    public enum PARAMFLAG : short
    {
        PARAMFLAG_NONE = 0,
        PARAMFLAG_FIN = 1,
        PARAMFLAG_FOUT = 2,
        PARAMFLAG_FLCID = 4,
        PARAMFLAG_FRETVAL = 8,
        PARAMFLAG_FOPT = 16,
        PARAMFLAG_FHASDEFAULT = 32,
        PARAMFLAG_FHASCUSTDATA = 64
    }

    public partial struct STATDATA
    {
        public ADVF advf;
        public IAdviseSink advSink;
        public int connection;
        public FORMATETC formatetc;
    }

    public partial struct STATSTG
    {
        public FILETIME atime;
        public long cbSize;
        public Guid clsid;
        public FILETIME ctime;
        public int grfLocksSupported;
        public int grfMode;
        public int grfStateBits;
        public FILETIME mtime;
        public string pwcsName;
        public int reserved;
        public int type;
    }

    public partial struct STGMEDIUM
    {
        public object pUnkForRelease;
        public TYMED tymed;
        public IntPtr unionmember;
    }

    public enum SYSKIND
    {
        SYS_WIN16 = 0,
        SYS_WIN32 = 1,
        SYS_MAC = 2,
        SYS_WIN64 = 3
    }

    [Flags]
    public enum TYMED
    {
        TYMED_NULL = 0,
        TYMED_HGLOBAL = 1,
        TYMED_FILE = 2,
        TYMED_ISTREAM = 4,
        TYMED_ISTORAGE = 8,
        TYMED_GDI = 16,
        TYMED_MFPICT = 32,
        TYMED_ENHMF = 64
    }

    public partial struct TYPEATTR
    {
        public short cbAlignment;
        public int cbSizeInstance;
        public short cbSizeVft;
        public short cFuncs;
        public short cImplTypes;
        public short cVars;
        public int dwReserved;
        public Guid guid;
        public IDLDESC idldescType;
        public int lcid;
        public IntPtr lpstrSchema;
        public const int MEMBER_ID_NIL = -1;
        public int memidConstructor;
        public int memidDestructor;
        public TYPEDESC tdescAlias;
        public TYPEKIND typekind;
        public short wMajorVerNum;
        public short wMinorVerNum;
        public TYPEFLAGS wTypeFlags;
    }

    public partial struct TYPEDESC
    {
        public IntPtr lpValue;
        public short vt;
    }

    [Flags]
    public enum TYPEFLAGS : short
    {
        TYPEFLAG_FAPPOBJECT = 1,
        TYPEFLAG_FCANCREATE = 2,
        TYPEFLAG_FLICENSED = 4,
        TYPEFLAG_FPREDECLID = 8,
        TYPEFLAG_FHIDDEN = 16,
        TYPEFLAG_FCONTROL = 32,
        TYPEFLAG_FDUAL = 64,
        TYPEFLAG_FNONEXTENSIBLE = 128,
        TYPEFLAG_FOLEAUTOMATION = 256,
        TYPEFLAG_FRESTRICTED = 512,
        TYPEFLAG_FAGGREGATABLE = 1024,
        TYPEFLAG_FREPLACEABLE = 2048,
        TYPEFLAG_FDISPATCHABLE = 4096,
        TYPEFLAG_FREVERSEBIND = 8192,
        TYPEFLAG_FPROXY = 16384
    }

    public enum TYPEKIND
    {
        TKIND_ENUM = 0,
        TKIND_RECORD = 1,
        TKIND_MODULE = 2,
        TKIND_INTERFACE = 3,
        TKIND_DISPATCH = 4,
        TKIND_COCLASS = 5,
        TKIND_ALIAS = 6,
        TKIND_UNION = 7,
        TKIND_MAX = 8
    }

    public partial struct TYPELIBATTR
    {
        public Guid guid;
        public int lcid;
        public SYSKIND syskind;
        public LIBFLAGS wLibFlags;
        public short wMajorVerNum;
        public short wMinorVerNum;
    }

    public partial struct VARDESC
    {
        public DESCUNION desc;
        public ELEMDESC elemdescVar;
        public string lpstrSchema;
        public int memid;
        public VARKIND varkind;
        public short wVarFlags;
        public partial struct DESCUNION
        {
            public IntPtr lpvarValue;
            public int oInst;
        }
    }

    [Flags]
    public enum VARFLAGS : short
    {
        VARFLAG_FREADONLY = 1,
        VARFLAG_FSOURCE = 2,
        VARFLAG_FBINDABLE = 4,
        VARFLAG_FREQUESTEDIT = 8,
        VARFLAG_FDISPLAYBIND = 16,
        VARFLAG_FDEFAULTBIND = 32,
        VARFLAG_FHIDDEN = 64,
        VARFLAG_FRESTRICTED = 128,
        VARFLAG_FDEFAULTCOLLELEM = 256,
        VARFLAG_FUIDEFAULT = 512,
        VARFLAG_FNONBROWSABLE = 1024,
        VARFLAG_FREPLACEABLE = 2048,
        VARFLAG_FIMMEDIATEBIND = 4096
    }

    public enum VARKIND
    {
        VAR_PERINSTANCE = 0,
        VAR_STATIC = 1,
        VAR_CONST = 2,
        VAR_DISPATCH = 3
    }
}