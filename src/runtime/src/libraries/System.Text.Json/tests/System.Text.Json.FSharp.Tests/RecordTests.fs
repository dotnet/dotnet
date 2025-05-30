﻿module System.Text.Json.Tests.FSharp.RecordTests

open System.Text.Json
open System.Text.Json.Serialization
open System.Text.Json.Tests.FSharp.Helpers
open Xunit

type MyRecord =
    {
        Name : string
        MiddleName : string option
        LastName : string
        Age : int
        IsActive : bool
    }
with
    static member Value = { Name = "John" ; MiddleName = None ; LastName = "Doe" ; Age = 34 ; IsActive = true }
    static member ExpectedJson = """{"Name":"John","MiddleName":null,"LastName":"Doe","Age":34,"IsActive":true}"""

[<Fact>]
let ``Support F# record serialization``() =
    let actualJson = JsonSerializer.Serialize(MyRecord.Value)
    Assert.Equal(MyRecord.ExpectedJson, actualJson)

[<Fact>]
let ``Support F# record deserialization``() =
    let result = JsonSerializer.Deserialize<MyRecord>(MyRecord.ExpectedJson)
    Assert.Equal(MyRecord.Value, result)

[<Struct>]
type MyStructRecord =
    {
        Name : string
        MiddleName : string option
        LastName : string
        Age : int
        IsActive : bool
    }
with
    static member Value = { Name = "John" ; MiddleName = None ; LastName = "Doe" ; Age = 34 ; IsActive = true }
    static member ExpectedJson = """{"Name":"John","MiddleName":null,"LastName":"Doe","Age":34,"IsActive":true}"""

[<Fact>]
let ``Support F# struct record serialization``() =
    let actualJson = JsonSerializer.Serialize(MyStructRecord.Value)
    Assert.Equal(MyStructRecord.ExpectedJson, actualJson)

[<Fact>]
let ``Support F# struct record deserialization``() =
    let result = JsonSerializer.Deserialize<MyStructRecord>(MyStructRecord.ExpectedJson)
    Assert.Equal(MyStructRecord.Value, result)

type RecursiveRecord =
    {
        Next : RecursiveRecord option
    }

[<Fact>]
let ``Recursive records are supported``() =
    let value = { Next = Some { Next = None } }
    let json = JsonSerializer.Serialize(value)
    Assert.Equal("""{"Next":{"Next":null}}""", json)
    let deserializedValue = JsonSerializer.Deserialize<RecursiveRecord>(json)
    Assert.Equal(value, deserializedValue)

[<Struct>]
type RecursiveStructRecord =
    {
        Next : RecursiveStructRecord voption []
    }

[<Fact>]
let ``Recursive struct records are supported``() =
    let value = { Next = [| ValueSome { Next = [| ValueNone |] } |] }
    let json = JsonSerializer.Serialize(value)
    Assert.Equal("""{"Next":[{"Next":[null]}]}""", json)
    let deserializedValue = JsonSerializer.Deserialize<RecursiveStructRecord>(json)
    Assert.Equal(value, deserializedValue)


[<JsonDerivedType(typeof<DerivedClass>, "derived")>]
type BaseClass(x : int) =
    member _.X = x

and DerivedClass(x : int, y : bool) = 
    inherit BaseClass(x)
    member _.Y = y

[<Fact>]
let ``Support F# class hierarchies`` () =
    let value = DerivedClass(42, true) :> BaseClass
    let json = JsonSerializer.Serialize(value)
    Assert.Equal("""{"$type":"derived","Y":true,"X":42}""", json)
    let deserializedValue = JsonSerializer.Deserialize<BaseClass>(json)
    let derived = Assert.IsType<DerivedClass>(deserializedValue)
    Assert.Equal(42, deserializedValue.X)
    Assert.Equal(true, derived.Y)
