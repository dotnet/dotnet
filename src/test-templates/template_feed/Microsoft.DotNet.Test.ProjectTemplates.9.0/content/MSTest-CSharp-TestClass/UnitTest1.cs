#if (ImplicitUsings != "enable")
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endif
#if (csharpFeature_FileScopedNamespaces)
namespace Company.TestProject1;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
    }
}
#else
namespace Company.TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
#endif
