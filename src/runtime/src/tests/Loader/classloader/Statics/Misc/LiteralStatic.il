// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

.assembly extern System.Console { }
.assembly extern xunit.core {}
// test that LDSFLD of a literal static field throws a MissingFieldException

.assembly extern mscorlib {}
.assembly LiteralStatic {}



.class public auto ansi beforefieldinit Test_LiteralStatic
       extends [mscorlib]System.Object
{
  .field public static literal int32 i = int32(123)

  .method public static void  RunTest()
  {
      ldsfld int32 Test_LiteralStatic::i
      ret
  }


  .method public hidebysig static int32 Main() cil managed
  {
    .custom instance void [xunit.core]Xunit.FactAttribute::.ctor() = (
        01 00 00 00
    )
    .entrypoint
    // Code size       34 (0x22)
    .maxstack  8

    .locals init (int32 V_0)
    IL_0000:  nop
    .try
    {
     
      call void Test_LiteralStatic::RunTest()

      ldstr      "Did not catch expected MissingFieldException"
      call       void [System.Console]System.Console::WriteLine(string)
      ldc.i4 101
      stloc.0

      leave.s    IL_0020

    }  // end .try
    catch [mscorlib]System.MissingFieldException 
    {
      pop

      ldstr      "PASS: Caught correct exception"
      call       void [System.Console]System.Console::WriteLine(string)

      ldc.i4 100
      stloc.0

      leave.s    IL_0020

    }  // end handler
    IL_0020:  nop
    ldloc.0
    ret
  } // end of method Test::Main

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // Code size       15 (0xf)
    .maxstack  8
    ldarg.0
    call       instance void [mscorlib]System.Object::.ctor()
    
    ret
  } // end of method Test::.ctor

} // end of class Test
