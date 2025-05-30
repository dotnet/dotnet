// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

// Various tests for the:
// Microsoft.FSharp.Control.Async type

namespace FSharp.Core.UnitTests.Control

open System
open FSharp.Core.UnitTests.LibraryTestFx
open Xunit
open System.Threading
open System.Threading.Tasks

// Cancels default token.
[<Collection(nameof FSharp.Test.NotThreadSafeResourceCollection)>]
module AsyncType =

    type ExpectedContinuation = Success | Exception | Cancellation

    [<Fact>]
    let startWithContinuations() =

        let cont actual expected _ =
            if expected <> actual then
                failwith $"expected {expected} continuation, but ran {actual}"

        let onSuccess = cont Success
        let onException = cont Exception
        let onCancellation = cont Cancellation

        let expect expected computation =
            Async.StartWithContinuations(computation, onSuccess expected, onException expected, onCancellation expected)

        async {
            Async.CancelDefaultToken()
            return () 
        } |> expect Cancellation

        async { failwith "computation failed" } |> expect Exception

        async { return () } |> expect Success

[<Collection(nameof FSharp.Test.NotThreadSafeResourceCollection)>]
type AsyncType() =

    let ignoreSynchCtx f =
        f ()

    [<VolatileField>]
    let mutable spinloop = true
        
    let waitASec (t:Task) =
        let result = t.Wait(TimeSpan(hours=0,minutes=0,seconds=1))
        Assert.True(result, "Task did not finish after waiting for a second.")

    [<Fact>]
    member _.AsyncRunSynchronouslyReusesThreadPoolThread() =
        let action _ =
            async {
                return
                    async { return Thread.CurrentThread.ManagedThreadId }
                    |> Async.RunSynchronously
            }
        // This test needs approximately 1000 ThreadPool threads
        // if Async.RunSynchronously doesn't reuse them.
        let usedThreads =
            Seq.init 1000 action
            |> Async.Parallel
            |> Async.RunSynchronously
            |> Set.ofArray
        printfn $"RunSynchronously used {usedThreads.Count} threads. Environment.ProcessorCount is {Environment.ProcessorCount}."
        // Some arbitrary large number but in practice it should not use more threads than there are CPU cores.
        Assert.True(usedThreads.Count < 256, $"RunSynchronously used {usedThreads.Count} threads.")

    [<Theory>]
    [<InlineData("int32")>]
    [<InlineData("timespan")>]
    member _.AsyncSleepCancellation1(sleepType) =
        ignoreSynchCtx (fun () ->
            let computation =
                match sleepType with
                | "int32"    -> Async.Sleep(10000000)
                | "timespan" -> Async.Sleep(10000000.0 |> TimeSpan.FromMilliseconds)
                | unknown    -> raise (NotImplementedException(unknown))
            let mutable result = ""
            use cts = new CancellationTokenSource()
            Async.StartWithContinuations(computation,
                                            (fun _ -> result <- "Ok"),
                                            (fun _ -> result <- "Exception"),
                                            (fun _ -> result <- "Cancel"),
                                            cts.Token)
            cts.Cancel()
            Async.Sleep(1000) |> Async.RunSynchronously
            Assert.AreEqual("Cancel", result)
        )

    [<Theory>]
    [<InlineData("int32")>]
    [<InlineData("timespan")>]
    member _.AsyncSleepCancellation2(sleepType) =
        ignoreSynchCtx (fun () ->
            let computation =
                match sleepType with
                | "int32"    -> Async.Sleep(10)
                | "timespan" -> Async.Sleep(10.0 |> TimeSpan.FromMilliseconds)
                | unknown    -> raise (NotImplementedException(unknown))
            for i in 1..100 do
                let mutable result = ""
                use completedEvent = new ManualResetEvent(false)
                use cts = new CancellationTokenSource()
                Async.StartWithContinuations(computation,
                                                (fun _ -> result <- "Ok"; completedEvent.Set() |> ignore),
                                                (fun _ -> result <- "Exception"; completedEvent.Set() |> ignore),
                                                (fun _ -> result <- "Cancel"; completedEvent.Set() |> ignore),
                                                cts.Token)
                sleep(10)
                cts.Cancel()
                completedEvent.WaitOne() |> Assert.True
                Assert.True(result = "Cancel" || result = "Ok")
        )

    [<Theory>]
    [<InlineData("int32")>]
    [<InlineData("timespan")>]
    member _.AsyncSleepThrowsOnNegativeDueTimes(sleepType) =
        async {
            try
                do! match sleepType with
                    | "int32"    -> Async.Sleep(-100)
                    | "timespan" -> Async.Sleep(-100.0 |> TimeSpan.FromMilliseconds)
                    | unknown    -> raise (NotImplementedException(unknown))
                failwith "Expected ArgumentOutOfRangeException"
            with
            | :? ArgumentOutOfRangeException -> ()
        } |> Async.RunSynchronously

    [<Fact>]
    member _.AsyncSleepInfinitely() =
        ignoreSynchCtx (fun () ->
            let computation = Async.Sleep(System.Threading.Timeout.Infinite)
            let result = TaskCompletionSource<string>()
            use cts = new CancellationTokenSource(TimeSpan.FromSeconds(1.0)) // there's a long way from 1 sec to infinity, but it'll have to do.
            Async.StartWithContinuations(computation,
                                            (fun _ -> result.TrySetResult("Ok")        |> ignore),
                                            (fun _ -> result.TrySetResult("Exception") |> ignore),
                                            (fun _ -> result.TrySetResult("Cancel")    |> ignore),
                                            cts.Token)
            let result = result.Task |> Async.AwaitTask |> Async.RunSynchronously
            Assert.AreEqual("Cancel", result)
        )

    [<Fact>]
    member _.CreateTask () =
        let s = "Hello tasks!"
        let a = async { return s }
        use t : Task<string> = Async.StartAsTask a
        waitASec t
        Assert.True (t.IsCompleted)
        Assert.AreEqual(s, t.Result)

    [<Fact>]
    member _.StartAsTaskCancellation () =
        let cts = new CancellationTokenSource()
        let doSpinloop () = while spinloop do ()
        let a = async {
            cts.CancelAfter (100)
            doSpinloop()
        }

        use t : Task<unit> = Async.StartAsTask(a, cancellationToken = cts.Token)
        // Should not finish, we don't eagerly mark the task done just because it's been signaled to cancel.
        try
            let result = t.Wait(300)
            Assert.False (result)
        with :? AggregateException -> Assert.Fail "Task should not finish, yet"

        spinloop <- false

        try
            waitASec t
        with :? AggregateException as a ->
            match a.InnerException with
            | :? TaskCanceledException as t -> ()
            | _ -> reraise()

        Assert.True (t.IsCompleted, "Task is not completed")


    [<Fact>]
    member _.``AwaitTask ignores Async cancellation`` () =
        let cts = new CancellationTokenSource()
        let tcs = new TaskCompletionSource<unit>()
        let innerTcs = new TaskCompletionSource<unit>()
        let a = innerTcs.Task |> Async.AwaitTask

        Async.StartWithContinuations(a, tcs.SetResult, tcs.SetException, ignore >> tcs.SetCanceled, cts.Token)

        cts.CancelAfter(100)
        try
            let result = tcs.Task.Wait(300)
            Assert.False (result)
        with :? AggregateException -> Assert.Fail "Should not finish, yet"

        innerTcs.SetResult ()

        try
            waitASec tcs.Task
        with :? AggregateException as a ->
            match a.InnerException with
            | :? TaskCanceledException -> ()
            | _ -> reraise()
        Assert.True (tcs.Task.IsCompleted, "Task is not completed")

    [<Fact>]
    member _.RunSynchronouslyCancellationWithDelayedResult () =
        let cts = new CancellationTokenSource()
        let tcs = TaskCompletionSource<int>()
        let _ = cts.Token.Register(fun () -> tcs.SetResult 42)
        let a = async {
            cts.CancelAfter (100)
            let! result = tcs.Task |> Async.AwaitTask
            return result }

        let cancelled =
            try
                Async.RunSynchronously(a, cancellationToken = cts.Token) |> ignore
                false
            with :? OperationCanceledException as o -> true
                 | _ -> false

        Assert.True (cancelled, "Task is not cancelled")

    [<Fact>]
    member _.ExceptionPropagatesToTask () =
        let a = async {
            do raise (Exception ())
         }
        use t = Async.StartAsTask a
        let mutable exceptionThrown = false
        try
            // waitASec t
            t.Wait()
        with
            e -> exceptionThrown <- true
        Assert.True (t.IsFaulted)
        Assert.True(exceptionThrown)

    [<Fact>]
    member _.CancellationPropagatesToTask () =
        let a = async {
                while true do ()
            }
        use t = Async.StartAsTask a
        Async.CancelDefaultToken ()
        let mutable exceptionThrown = false
        try
            waitASec t
        with e -> exceptionThrown <- true
        Assert.True (exceptionThrown)
        Assert.True(t.IsCanceled)

    [<Fact>]
    member _.CancellationPropagatesToGroup () =
        let ewh = new ManualResetEvent(false)
        let mutable cancelled = false
        let a = async {
                use! holder = Async.OnCancel (fun _ -> cancelled <- true)
                ewh.Set() |> Assert.True
                while true do ()
            }
        let cts = new CancellationTokenSource()
        let token = cts.Token
        use t = Async.StartAsTask(a, cancellationToken=token)
//        printfn "%A" t.Status
        ewh.WaitOne() |> Assert.True
        cts.Cancel()
//        printfn "%A" t.Status
        let mutable exceptionThrown = false
        try
            t.Wait()
        with e -> exceptionThrown <- true
        Assert.True (exceptionThrown)
        Assert.True(t.IsCanceled)
        Assert.True(cancelled)

    [<Fact>]
    member _.CreateImmediateAsTask () =
        let s = "Hello tasks!"
        let a = async { return s }
        use t : Task<string> = Async.StartImmediateAsTask a
        waitASec t
        Assert.True (t.IsCompleted)
        Assert.AreEqual(s, t.Result)

    [<Fact>]
    member _.StartImmediateAsTask () =
        let s = "Hello tasks!"
        let a = async { return s }
        use t = Async.StartImmediateAsTask a
        waitASec t
        Assert.True (t.IsCompleted)
        Assert.AreEqual(s, t.Result)


    [<Fact>]
    member _.ExceptionPropagatesToImmediateTask () =
        let a = async {
            do raise (Exception ())
         }
        use t = Async.StartImmediateAsTask a
        let mutable exceptionThrown = false
        try
            t.Wait()
        with
            e -> exceptionThrown <- true
        Assert.True (t.IsFaulted)
        Assert.True(exceptionThrown)

    [<Fact>]
    member _.CancellationPropagatesToImmediateTask () =
        let a = async {
                while true do
                    do! Async.Sleep 100
            }
        use t = Async.StartImmediateAsTask a
        Async.CancelDefaultToken ()
        let mutable exceptionThrown = false
        try
            t.Wait()
        with e -> exceptionThrown <- true
        Assert.True (exceptionThrown)
        Assert.True(t.IsCanceled)

    [<Fact>]
    member _.CancellationPropagatesToGroupImmediate () =
        let ewh = new ManualResetEvent(false)
        let mutable cancelled = false
        let a = async {
                use! holder = Async.OnCancel (fun _ -> cancelled <- true)
                ewh.Set() |> Assert.True
                while true do
                    do! Async.Sleep 100
            }
        let cts = new CancellationTokenSource()
        let token = cts.Token
        use t =
            Async.StartImmediateAsTask(a, cancellationToken=token)
        ewh.WaitOne() |> Assert.True
        cts.Cancel()
        let mutable exceptionThrown = false
        try
            t.Wait()
        with e -> exceptionThrown <- true
        Assert.True (exceptionThrown)
        Assert.True(t.IsCanceled)
        Assert.True(cancelled)

    [<Fact>]
    member _.TaskAsyncValue () =
        let s = "Test"
        use t = Task.Factory.StartNew(Func<_>(fun () -> s))
        let a = async {
                let! s1 = Async.AwaitTask(t)
                return s = s1
            }
        Async.RunSynchronously(a) |> Assert.True

    [<Fact>]
    member _.AwaitTaskCancellation () =
        let test() = async {
            let tcs = new System.Threading.Tasks.TaskCompletionSource<unit>()
            tcs.SetCanceled()
            try
                do! Async.AwaitTask tcs.Task
                return false
            with :? System.OperationCanceledException -> return true
        }

        Async.RunSynchronously(test()) |> Assert.True

    [<Fact>]
    member _.AwaitCompletedTask() =
        let test() = async {
            let threadIdBefore = Thread.CurrentThread.ManagedThreadId
            do! Async.AwaitTask Task.CompletedTask
            let threadIdAfter = Thread.CurrentThread.ManagedThreadId
            return threadIdBefore = threadIdAfter
        }

        Async.RunSynchronously(test()) |> Assert.True

    [<Fact>]
    member _.AwaitTaskCancellationUntyped () =
        let test() = async {
            let tcs = new System.Threading.Tasks.TaskCompletionSource<unit>()
            tcs.SetCanceled()
            try
                do! Async.AwaitTask (tcs.Task :> Task)
                return false
            with :? System.OperationCanceledException -> return true
        }

        Async.RunSynchronously(test()) |> Assert.True

    [<Fact>]
    member _.TaskAsyncValueException () =
        use t = Task.Factory.StartNew(Func<unit>(fun () -> raise <| Exception()))
        let a = async {
                try
                    let! v = Async.AwaitTask(t)
                    return false
                with e -> return true
              }
        Async.RunSynchronously(a) |> Assert.True

    [<Fact>]
    member _.TaskAsyncValueCancellation () =
        use ewh = new ManualResetEvent(false)
        let cts = new CancellationTokenSource()
        let token = cts.Token
        use t : Task<unit> = Task.Factory.StartNew(Func<unit>(fun () -> while not token.IsCancellationRequested do ()), token)
        let cancelled = ref true
        let a = 
            async {
                try
                    use! _holder = Async.OnCancel(fun _ -> ewh.Set() |> ignore)
                    let! v = Async.AwaitTask(t)
                    return v
                // AwaitTask raises TaskCanceledException when it is canceled, it is a valid result of this test
                with
                   :? TaskCanceledException -> 
                      ewh.Set() |> ignore // this is ok
            }
        let t1 = Async.StartAsTask a
        cts.Cancel()
        ewh.WaitOne(10000) |> ignore
        // Don't leave unobserved background tasks, because they can crash the test run.
        t1.Wait()

    [<Fact>]
    member _.NonGenericTaskAsyncValue () =
        let mutable hasBeenCalled = false
        use t = Task.Factory.StartNew(Action(fun () -> hasBeenCalled <- true))
        let a = async {
                do! Async.AwaitTask(t)
                return true
            }
        let result = Async.RunSynchronously(a)
        (hasBeenCalled && result) |> Assert.True

    [<Fact>]
    member _.NonGenericTaskAsyncValueException () =
        use t = Task.Factory.StartNew(Action(fun () -> raise <| Exception()))
        let a = async {
                try
                    let! v = Async.AwaitTask(t)
                    return false
                with e -> return true
              }
        Async.RunSynchronously(a) |> Assert.True

    [<Fact>]
    member _.NonGenericTaskAsyncValueCancellation () =
        use ewh = new ManualResetEvent(false)
        let cts = new CancellationTokenSource()
        let token = cts.Token
        use t = Task.Factory.StartNew(Action(fun () -> while not token.IsCancellationRequested do ()), token)
        let a =
            async {
                try
                    use! _holder = Async.OnCancel(fun _ -> ewh.Set() |> ignore)
                    let! v = Async.AwaitTask(t)
                    return v
                // AwaitTask raises TaskCanceledException when it is canceled, it is a valid result of this test
                with
                   :? TaskCanceledException -> 
                      ewh.Set() |> ignore // this is ok
            }
        let t1 = Async.StartAsTask a
        cts.Cancel()
        ewh.WaitOne(10000) |> ignore
        t1.Wait()

    [<Fact>]
    member _.CancellationExceptionThrown () =
        use ewh = new ManualResetEventSlim(false)
        let cts = new CancellationTokenSource()
        let token = cts.Token
        let mutable hasThrown = false
        token.Register(fun () -> ewh.Set() |> ignore) |> ignore
        let a = async {
            try
                while true do token.ThrowIfCancellationRequested()
            with _ -> hasThrown <- true
        }
        Async.Start(a, token)
        cts.Cancel()
        ewh.Wait(10000) |> ignore
        Assert.False hasThrown

    [<Fact>]
    member _.NoStackOverflowOnRecursion() =

        let mutable hasThrown = false
        let rec loop (x: int) = async {
            do! Task.CompletedTask |> Async.AwaitTask
            Console.WriteLine (if x = 10000 then failwith "finish" else x)
            return! loop(x+1)
        }
    
        try 
           Async.RunSynchronously (loop 0)
           hasThrown <- false
        with Failure "finish" -> 
            hasThrown <- true
        Assert.True hasThrown
