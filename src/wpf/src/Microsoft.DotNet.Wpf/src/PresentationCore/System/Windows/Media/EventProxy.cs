﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MS.Internal;
using MS.Win32;
using System.Runtime.InteropServices;

namespace System.Windows.Media
{
    #region EventProxyDescriptor
    [StructLayout(LayoutKind.Sequential)]
    internal struct EventProxyDescriptor
    {
        internal delegate void Dispose(
            ref EventProxyDescriptor pEPD
            );

        internal delegate int RaiseEvent(
            ref EventProxyDescriptor pEPD,
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] byte[] buffer,
            uint cb
            );

        internal Dispose pfnDispose;
        internal RaiseEvent pfnRaiseEvent;

        internal static void StaticDispose(ref EventProxyDescriptor pEPD)
        {
            Debug.Assert(((IntPtr)pEPD.m_handle) != IntPtr.Zero, "If this asserts fires: Why is it firing? It might be legal in future.");
            EventProxyWrapper epw = (EventProxyWrapper)(pEPD.m_handle.Target);
            ((System.Runtime.InteropServices.GCHandle)(pEPD.m_handle)).Free();
        }

        internal System.Runtime.InteropServices.GCHandle m_handle;
    }
    #endregion

    #region EventProxyStaticPtrs
    /// <summary>
    /// We need to keep the delegates alive.
    /// </summary>
    internal static class EventProxyStaticPtrs
    {
        static EventProxyStaticPtrs()
        {
            EventProxyStaticPtrs.pfnDispose = new EventProxyDescriptor.Dispose(EventProxyDescriptor.StaticDispose);
            EventProxyStaticPtrs.pfnRaiseEvent = new EventProxyDescriptor.RaiseEvent(EventProxyWrapper.RaiseEvent);
}

        internal static EventProxyDescriptor.Dispose pfnDispose;
        internal static EventProxyDescriptor.RaiseEvent pfnRaiseEvent;
    }
    #endregion

    #region EventProxyWrapper
    /// <summary>
    /// Event proxy wrapper will relay events from unmanaged code to managed code
    /// </summary>
    internal class EventProxyWrapper
    {
        private WeakReference target;

        #region Constructor

        private EventProxyWrapper(IInvokable invokable)
        {
            target = new WeakReference(invokable);
        }

        #endregion

        #region Public methods

        public int RaiseEvent(byte[] buffer, uint cb)
        {
            try
            {
                ObjectDisposedException.ThrowIf(target == null, typeof(EventProxyWrapper));
                IInvokable invokable = (IInvokable)target.Target;
                if (invokable != null)
                {
                    invokable.RaiseEvent(buffer, (int)cb);
                }
                else
                {
                    // return E_HANDLE to notify that object is no longer alive

                    return NativeMethods.E_HANDLE;
                }
            }
            catch (Exception e)
            {
                return Marshal.GetHRForException(e);
            }

            return NativeMethods.S_OK;
        }

        #endregion

        #region Delegate Implemetations
        internal static EventProxyWrapper FromEPD(ref EventProxyDescriptor epd)
        {
            Debug.Assert(((IntPtr)epd.m_handle) != IntPtr.Zero, "Stream is disposed.");
            System.Runtime.InteropServices.GCHandle handle = (System.Runtime.InteropServices.GCHandle)(epd.m_handle);
            return (EventProxyWrapper)(handle.Target);
        }

        internal static int RaiseEvent(ref EventProxyDescriptor pEPD, byte[] buffer, uint cb)
        {
            EventProxyWrapper target = EventProxyWrapper.FromEPD(ref pEPD);
            if (target != null)
            {
                return target.RaiseEvent(buffer, cb);
            }
            else
            {
                return NativeMethods.E_HANDLE;
            }
        }

        #endregion

        #region Static Create Method(s)

        internal static SafeMILHandle CreateEventProxyWrapper(IInvokable invokable)
        {
            ArgumentNullException.ThrowIfNull(invokable);

            SafeMILHandle eventProxy = null;

            EventProxyWrapper epw = new EventProxyWrapper(invokable);
            EventProxyDescriptor epd = new EventProxyDescriptor
            {
                pfnDispose = EventProxyStaticPtrs.pfnDispose,
                pfnRaiseEvent = EventProxyStaticPtrs.pfnRaiseEvent,

                m_handle = System.Runtime.InteropServices.GCHandle.Alloc(epw, System.Runtime.InteropServices.GCHandleType.Normal)
            };

            HRESULT.Check(MILCreateEventProxy(ref epd, out eventProxy));

            return eventProxy;
        }

        #endregion

        [DllImport(DllImport.MilCore)]
        private static extern int /* HRESULT */ MILCreateEventProxy(ref EventProxyDescriptor pEPD, out SafeMILHandle ppEventProxy);
    }
    #endregion
}
