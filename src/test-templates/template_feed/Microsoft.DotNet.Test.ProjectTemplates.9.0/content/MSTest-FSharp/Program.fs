open Microsoft.VisualStudio.TestTools.UnitTesting

module MSTestSettings =
    [<assembly: Parallelize(Scope = ExecutionScope.MethodLevel)>]
    do()

module Program = let [<EntryPoint>] main _ = 0
