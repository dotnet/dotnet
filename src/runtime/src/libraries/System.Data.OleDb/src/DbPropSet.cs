// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.OleDb
{
    internal sealed class DBPropSet : SafeHandle
    {
        private readonly int propertySetCount;

        // stores the exception with last error.HRESULT from IDBProperties.GetProperties
        private Exception? lastErrorFromProvider;

        public DBPropSet() : base(IntPtr.Zero, true)
        {
            propertySetCount = 0;
        }

        internal DBPropSet(int propertysetCount) : this()
        {
            this.propertySetCount = propertysetCount;
            nuint countOfBytes = (nuint)(propertysetCount * ODB.SizeOf_tagDBPROPSET);
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            { }
            finally
            {
                base.handle = Interop.Ole32.CoTaskMemAlloc(countOfBytes);
                if (IntPtr.Zero != base.handle)
                {
                    SafeNativeMethods.ZeroMemory(base.handle, (int)countOfBytes);
                }
            }
            if (IntPtr.Zero == base.handle)
            {
                throw new OutOfMemoryException();
            }
        }

        internal DBPropSet(UnsafeNativeMethods.IDBProperties properties, PropertyIDSet? propidset, out OleDbHResult hr) : this()
        {
            Debug.Assert(null != properties, "null IDBProperties");

            int propidsetcount = 0;
            if (null != propidset)
            {
                propidsetcount = propidset.Count;
            }
            hr = properties.GetProperties(propidsetcount, propidset, out this.propertySetCount, out base.handle);

            if (hr < 0)
            {
                // remember the last HRESULT. Note we do not want to raise exception now to avoid breaking change from Orcas RTM/SP1
                SetLastErrorInfo(hr);
            }
        }

        internal DBPropSet(UnsafeNativeMethods.IRowsetInfo properties, PropertyIDSet? propidset, out OleDbHResult hr) : this()
        {
            Debug.Assert(null != properties, "null IRowsetInfo");

            int propidsetcount = 0;
            if (null != propidset)
            {
                propidsetcount = propidset.Count;
            }
            hr = properties.GetProperties(propidsetcount, propidset, out this.propertySetCount, out base.handle);

            if (hr < 0)
            {
                // remember the last HRESULT. Note we do not want to raise exception now to avoid breaking change from Orcas RTM/SP1
                SetLastErrorInfo(hr);
            }
        }

        internal DBPropSet(UnsafeNativeMethods.ICommandProperties properties, PropertyIDSet? propidset, out OleDbHResult hr) : this()
        {
            Debug.Assert(null != properties, "null ICommandProperties");

            int propidsetcount = 0;
            if (null != propidset)
            {
                propidsetcount = propidset.Count;
            }
            hr = properties.GetProperties(propidsetcount, propidset, out this.propertySetCount, out base.handle);

            if (hr < 0)
            {
                // remember the last HRESULT. Note we do not want to raise exception now to avoid breaking change from Orcas RTM/SP1
                SetLastErrorInfo(hr);
            }
        }

        private void SetLastErrorInfo(OleDbHResult lastErrorHr)
        {
            // note: OleDbHResult is actually a simple wrapper over HRESULT with OLEDB-specific codes
            string message = string.Empty;

            OleDbHResult errorInfoHr = UnsafeNativeMethods.GetErrorInfo(0, out UnsafeNativeMethods.IErrorInfo? errorInfo);  // 0 - IErrorInfo exists, 1 - no IErrorInfo
            if ((errorInfoHr == OleDbHResult.S_OK) && (errorInfo != null))
            {
                try
                {
                    ODB.GetErrorDescription(errorInfo, lastErrorHr, out message);
                    // note that either GetErrorInfo or GetErrorDescription might fail in which case we will have only the HRESULT value in exception message
                }
                finally
                {
                    UnsafeNativeMethods.ReleaseErrorInfoObject(errorInfo);
                }
            }

            lastErrorFromProvider = new COMException(message, (int)lastErrorHr);
        }

        public override bool IsInvalid
        {
            get
            {
                return (IntPtr.Zero == base.handle);
            }
        }

        protected override bool ReleaseHandle()
        {
            // NOTE: The SafeHandle class guarantees this will be called exactly once and is non-interrutible.
            IntPtr ptr = base.handle;
            base.handle = IntPtr.Zero;
            if (IntPtr.Zero != ptr)
            {
                int count = this.propertySetCount;
                for (int i = 0, offset = 0; i < count; ++i, offset += ODB.SizeOf_tagDBPROPSET)
                {
                    IntPtr rgProperties = Marshal.ReadIntPtr(ptr, offset);
                    if (IntPtr.Zero != rgProperties)
                    {
                        int cProperties = Marshal.ReadInt32(ptr, offset + IntPtr.Size);

                        IntPtr vptr = ADP.IntPtrOffset(rgProperties, ODB.OffsetOf_tagDBPROP_Value);
                        for (int k = 0; k < cProperties; ++k, vptr = ADP.IntPtrOffset(vptr, ODB.SizeOf_tagDBPROP))
                        {
                            Interop.OleAut32.VariantClear(vptr);
                        }
                        Interop.Ole32.CoTaskMemFree(rgProperties);
                    }
                }
                Interop.Ole32.CoTaskMemFree(ptr);
            }
            return true;
        }

        internal int PropertySetCount
        {
            get
            {
                return this.propertySetCount;
            }
        }

        internal ItagDBPROP[] GetPropertySet(int index, out Guid propertyset)
        {
            if ((index < 0) || (PropertySetCount <= index))
            {
                if (lastErrorFromProvider != null)
                {
                    // add extra error information for CSS/stress troubleshooting.
                    // We need to keep same exception type to avoid breaking change with Orcas RTM/SP1.
                    throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer, lastErrorFromProvider);
                }
                else
                {
                    throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer);
                }
            }

            tagDBPROPSET propset = new tagDBPROPSET();
            ItagDBPROP[]? properties = null;

            bool mustRelease = false;
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                DangerousAddRef(ref mustRelease);
                IntPtr propertySetPtr = ADP.IntPtrOffset(DangerousGetHandle(), index * ODB.SizeOf_tagDBPROPSET);
                Marshal.PtrToStructure(propertySetPtr, propset);
                propertyset = propset.guidPropertySet;

                properties = new ItagDBPROP[propset.cProperties];
                for (int i = 0; i < properties.Length; ++i)
                {
                    properties[i] = OleDbStructHelpers.CreateTagDbProp();
                    IntPtr ptr = ADP.IntPtrOffset(propset.rgProperties, i * ODB.SizeOf_tagDBPROP);
                    Marshal.PtrToStructure(ptr, properties[i]);
                }
            }
            finally
            {
                if (mustRelease)
                {
                    DangerousRelease();
                }
            }
            return properties;
        }

        internal void SetPropertySet(int index, Guid propertySet, ItagDBPROP[] properties)
        {
            if ((index < 0) || (PropertySetCount <= index))
            {
                if (lastErrorFromProvider != null)
                {
                    // add extra error information for CSS/stress troubleshooting.
                    // We need to keep same exception type to avoid breaking change with Orcas RTM/SP1.
                    throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer, lastErrorFromProvider);
                }
                else
                {
                    throw ADP.InternalError(ADP.InternalErrorCode.InvalidBuffer);
                }
            }
            Debug.Assert(Guid.Empty != propertySet, "invalid propertySet");
            Debug.Assert((null != properties) && (0 < properties.Length), "invalid properties");

            nuint countOfBytes = (nuint)(properties.Length * ODB.SizeOf_tagDBPROP);
            tagDBPROPSET propset = new tagDBPROPSET(properties.Length, propertySet);

            bool mustRelease = false;
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                DangerousAddRef(ref mustRelease);

                IntPtr propsetPtr = ADP.IntPtrOffset(DangerousGetHandle(), index * ODB.SizeOf_tagDBPROPSET);

                RuntimeHelpers.PrepareConstrainedRegions();
                try
                { }
                finally
                {
                    // must allocate and clear the memory without interruption
                    propset.rgProperties = Interop.Ole32.CoTaskMemAlloc(countOfBytes);
                    if (IntPtr.Zero != propset.rgProperties)
                    {
                        // clearing is important so that we don't treat existing
                        // garbage as important information during releaseHandle
                        SafeNativeMethods.ZeroMemory(propset.rgProperties, (int)countOfBytes);

                        // writing the structure to native memory so that it knows to free the referenced pointers
                        Marshal.StructureToPtr(propset, propsetPtr, false/*deleteold*/);
                    }
                }
                if (IntPtr.Zero == propset.rgProperties)
                {
                    throw new OutOfMemoryException();
                }

                for (int i = 0; i < properties.Length; ++i)
                {
                    Debug.Assert(null != properties[i], $"null tagDBPROP {i.ToString(CultureInfo.InvariantCulture)}");
                    IntPtr propertyPtr = ADP.IntPtrOffset(propset.rgProperties, i * ODB.SizeOf_tagDBPROP);
                    Marshal.StructureToPtr(properties[i], propertyPtr, false/*deleteold*/);
                }
            }
            finally
            {
                if (mustRelease)
                {
                    DangerousRelease();
                }
            }
        }

        internal static DBPropSet CreateProperty(Guid propertySet, int propertyId, bool required, object value)
        {
            ItagDBPROP dbprop = OleDbStructHelpers.CreateTagDbProp(propertyId, required, value);
            DBPropSet propertyset = new DBPropSet(1);
            propertyset.SetPropertySet(0, propertySet, new ItagDBPROP[1] { dbprop });
            return propertyset;
        }
    }
}
