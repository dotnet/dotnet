// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Runtime.InteropServices;
using EnvDTE;

namespace NuGet.VisualStudio
{
    /// <summary>
    /// The purpose of this interface is to avoid calls to <see cref="Item"> throwing on legacy project systems written in native code when the properties asked for do not exist. Since all DTE interfaces (the ones in the
    /// Visual Studio interop dll) are non-preservesig any failing HRESULTS turn into managed exceptions. This is not an exceptional situation (missing properties) and can result in significant amounts of thrown/caught
    /// exception noise. This trick relies on the fact that a managed cast on an RCW (i.e. the CLR's wrapping of a native COM object, i.e. a legacy project system written in native code) is actually a QueryInterface call
    /// on the underlying native COM object. All QueryInterface cares about is the GUID. So when we say:
    ///
    /// <code>
    /// INonThrowingDTEProjectProperties properties = someProjectProperties as INonThrowingDTEProjectProperties;
    /// </code>
    ///
    /// The CLR really calls QueryInterface with the GUID associated with <see cref="INonThrowingDTEProjectProperties"/>. Since the GUID here matches the GUID on <see cref="Properties"/>, the native object will say 'yes, I do
    /// implement that interface' and the CLR will allow us to talk to it through this interface definition. Since we have changed Item to be preservesig that means failures will come back as failing HRESULTS not as exceptions.
    /// </summary>
#pragma warning disable CA1010 // warning that type implementing IEnumerable should implement IEnumerable<T> as well, which makes no sense here as this is a COM interface decl
    [ComVisible(true)]
    [Guid("4CC8CCF5-A926-4646-B17F-B4940CAED472")]
    public interface INonThrowingDTEProjectProperties : IEnumerable
#pragma warning restore CA1010
    {
        [PreserveSig]
        int Item([In][MarshalAs(UnmanagedType.Struct)] object index, out Property property);

        object Application
        {
            [return: MarshalAs(UnmanagedType.IDispatch)]
            get;
        }

        object Parent
        {
            [return: MarshalAs(UnmanagedType.IDispatch)]
            get;
        }

        int Count
        {
            get;
        }

        [return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "System.Runtime.InteropServices.CustomMarshalers.EnumeratorToEnumVariantMarshaler, CustomMarshalers, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
        new IEnumerator GetEnumerator();

        DTE DTE
        {
            [return: MarshalAs(UnmanagedType.Interface)]
            get;
        }
    }
}
