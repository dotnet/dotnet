// #Regression #NoMT #CodeGen #Interop 
// Regression test for FSHARP1.0:4040
// "Signature files do not prevent compiler-generated public constructors from leaking out of discriminated unions"
// Note that in this case the .fsi is NOT missing the "| C of int"

#light
module N

type T = | C of int
