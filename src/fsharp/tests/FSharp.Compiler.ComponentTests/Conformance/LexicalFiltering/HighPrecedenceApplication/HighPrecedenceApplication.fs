// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

namespace Conformance.LexicalFiltering

open Xunit
open FSharp.Test
open FSharp.Test.Compiler

module HighPrecedenceApplication =

    // This test was automatically generated (moved from FSharpQA suite - Conformance/LexicalFiltering/HighPrecedenceApplication)
    //<Expects status="success"></Expects>
    [<Theory; FileInlineData("RangeOperator01.fs")>]
    let ``RangeOperator01_fs`` compilation =
        compilation
        |> getCompilation
        |> asFs
        |> withOptions ["-a"]
        |> withNoWarn 3873 // This construct is deprecated. Sequence expressions should be of the form 'seq { ... }
        |> compile
        |> shouldSucceed
        |> ignore

