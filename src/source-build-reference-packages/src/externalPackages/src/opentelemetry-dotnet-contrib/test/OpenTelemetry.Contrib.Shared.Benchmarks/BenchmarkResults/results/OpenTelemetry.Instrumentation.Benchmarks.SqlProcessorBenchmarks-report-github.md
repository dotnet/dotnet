```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7462/25H2/2025Update/HudsonValley2)
Intel Core Ultra 9 185H 2.50GHz, 1 CPU, 22 logical and 16 physical cores
.NET SDK 10.0.101
  [Host]     : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.1 (10.0.1, 10.0.125.57005), X64 RyuJIT x86-64-v3


```
| Method          | Sql                  | Mean     | Error   | StdDev  | Allocated |
|---------------- |--------------------- |---------:|--------:|--------:|----------:|
| **GetSanitizedSql** | **CREAT(...)s(Id) [56]** | **250.9 ns** | **1.01 ns** | **0.84 ns** |      **72 B** |
| **GetSanitizedSql** | **DELET(...) = 42 [32]** | **169.7 ns** | **1.34 ns** | **1.19 ns** |     **128 B** |
| **GetSanitizedSql** | **INSER(...)3e-5) [76]** | **312.7 ns** | **2.03 ns** | **1.90 ns** |     **192 B** |
| **GetSanitizedSql** | **SELEC(...)ls od [39]** | **202.3 ns** | **1.17 ns** | **1.04 ns** |      **80 B** |
| **GetSanitizedSql** | **SELE(...)tory [111]**  | **354.8 ns** | **2.45 ns** | **2.29 ns** |     **176 B** |
| **GetSanitizedSql** | **SELEC(...)table [69]** | **150.3 ns** | **1.37 ns** | **1.22 ns** |     **128 B** |
| **GetSanitizedSql** | **SELEC(...) c.Id [74]** | **316.1 ns** | **1.30 ns** | **1.22 ns** |      **72 B** |
| **GetSanitizedSql** | **SELE(...)_id) [101]**  | **375.6 ns** | **1.84 ns** | **1.72 ns** |      **88 B** |
| **GetSanitizedSql** | **UPDAT(...) = 42 [44]** | **206.1 ns** | **1.26 ns** | **1.11 ns** |     **144 B** |
