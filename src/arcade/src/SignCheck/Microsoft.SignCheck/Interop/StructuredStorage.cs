// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Deployment.WindowsInstaller;
using Microsoft.VisualStudio.OLE.Interop;
using STATSTG = Microsoft.VisualStudio.OLE.Interop.STATSTG;

namespace Microsoft.SignCheck.Interop
{
    // This code is a C# adaptation of MSIX:
    // https://blogs.msdn.microsoft.com/heaths/2006/02/27/identifying-windows-installer-file-types/
    // https://blogs.msdn.microsoft.com/heaths/2006/04/07/patch-files-extractor/
    public class StructuredStorage
    {
        // CLSID for MSP storage
        private const string MSP_CLSID = "000C1086-0000-0000-C000-000000000046";

        public const int S_OK = 0;

        /// <summary>
        /// Returns true if the storage represents a patch (MSP)
        /// </summary>
        /// <param name="storage">The store to check.</param>
        /// <returns>true if the storage is an MSP, false otherwise.</returns>
        public static bool IsPatch(IStorage storage)
        {
            if (storage == null)
            {
                throw new ArgumentNullException("storage");
            }

            STATSTG[] stg = new STATSTG[] { new STATSTG() };
            storage.Stat(stg, (uint)STATFLAG.STATFLAG_NONAME);

            return String.Equals(stg[0].clsid.ToString(), MSP_CLSID, StringComparison.OrdinalIgnoreCase);
        }

        public static void OpenAndExtractStorages(string filename, string dir)
        {
            IStorage rootStorage = null;
            int hresult = Ole32.StgOpenStorage(filename, null, STGM.STGM_READ | STGM.STGM_SHARE_EXCLUSIVE, IntPtr.Zero, 0, out rootStorage);

            if ((hresult == S_OK) && (rootStorage != null))
            {
                if (IsPatch(rootStorage))
                {
                    try
                    {
                        IEnumSTATSTG rootStorageEnum = null;
                        rootStorage.EnumElements(0, IntPtr.Zero, 0, out rootStorageEnum);

                        STATSTG[] enumStg = new STATSTG[] { new STATSTG() };
                        uint numFetched = 0;
                        rootStorageEnum.Next(1, enumStg, out numFetched);

                        while (numFetched == 1)
                        {
                            if (enumStg[0].type == (uint)STGTY.STGTY_STORAGE)
                            {
                                // Save the nested transform storages with an .mst extension
                                SaveStorage(rootStorage, dir, enumStg[0].pwcsName, ".mst");
                            }

                            rootStorageEnum.Next(1, enumStg, out numFetched);
                        }

                        if (enumStg != null)
                        {
                            Marshal.ReleaseComObject(rootStorageEnum);
                        }

                        if (rootStorage != null)
                        {
                            Marshal.ReleaseComObject(rootStorage);
                        }

                        using (Database installDatabase = new Database(filename, DatabaseOpenMode.ReadOnly))
                        using (View view = installDatabase.OpenView("SELECT `Name`, `Data` FROM `_Streams`"))
                        {
                            view.Execute();

                            foreach (Record record in view)
                            {
                                SaveStream(record, dir);
                                record.Close();
                            }
                        }
                    }
                    finally
                    {
                        if (rootStorage != null)
                        {
                            Marshal.ReleaseComObject(rootStorage);
                        }
                    }
                }
            }
        }

        public static void SaveStorage(IStorage rootStorage, string storageDir, string storageName, string storageExtension)
        {
            IStorage stg;
            IStorage fileStg;
            int hr = StructuredStorage.S_OK;

            rootStorage.OpenStorage(storageName, null, (STGM.STGM_READ | STGM.STGM_SHARE_EXCLUSIVE), IntPtr.Zero, 0, out stg);

            if (stg != null)
            {
                string storageFullName = Path.Combine(storageDir, storageName + storageExtension);
                if (!Directory.Exists(storageDir))
                {
                    Directory.CreateDirectory(storageDir);
                }

                hr = Ole32.StgCreateDocfile(storageFullName,
                    STGM.STGM_WRITE | STGM.STGM_SHARE_EXCLUSIVE | STGM.STGM_CREATE,
                    0,
                    out fileStg);

                if (fileStg != null)
                {
                    stg.CopyTo(0, null, IntPtr.Zero, fileStg);
                    Marshal.ReleaseComObject(fileStg);
                }

                Marshal.ReleaseComObject(stg);
            }
        }

        public static void SaveStream(Record record, string dir)
        {
            if (record == null)
            {
                throw new ArgumentNullException("record");
            }
            Stream recordStream = (Stream)record["Data"];
            string path = Path.Combine(dir, (string)record["Name"]);

            if (path.IndexOfAny(Path.GetInvalidPathChars()) == -1)
            {
                using (FileStream fs = new FileStream(path, FileMode.CreateNew))
                {
                    recordStream.CopyTo(fs);
                }
            }
        }
    }
}
