using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecursiveResourceLookupCrash
{
    [TestClass]
    public class RecursiveResourceLookupCrashTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            // You need to set non-English culture explicitly to reproduce recursive resource
            // lookup bug in English environment.
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("ja-JP");
        }

        [TestMethod]
        public void CrashesOnResourcesLookupWhenNotHandledByAssemblyResolver()
        {
            try
            {
                // This will internally trigger file not found exception, and try to find ja-JP resources
                // for the string, which will trigger Resolve in AssemblyResolver, which will
                // use File.Exists call and that will trigger another round of looking up ja-JP
                // resources, until this is detected by .NET Framework, and Environment.FailFast
                // is called to crash the testhost.
                var stream = new IsolatedStorageFileStream("non-existent-filename", FileMode.Open);
            }
            catch (Exception)
            {
            }
        }
    }
}
