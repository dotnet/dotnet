namespace Company.TestProject1

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type UnitTest1 () =

    [<TestMethod>]
    member this.TestMethod1 () =
        Assert.IsTrue(true);
