# Benchmark

Results for automapper v14

```
BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6456/22H2/2022Update) (VirtualBox)
12th Gen Intel Core i7-12700K 3.61GHz, 1 CPU, 4 logical and 4 physical cores
.NET SDK 10.0.201
  [Host]     : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
```
| Method                                                | CollectionSize | Mean      | Error    | StdDev   | Median    | Gen0   | Allocated |
|------------------------------------------------------ |--------------- |----------:|---------:|---------:|----------:|-------:|----------:|
| Map_FactInfraResponse_To_FactViewModel                | 10             |  26.17 ns | 0.575 ns | 1.286 ns |  25.93 ns | 0.0018 |      24 B |
| Map_FactInfraResponseList_To_IEnumerableFactViewModel | 10             | 139.26 ns | 2.818 ns | 6.642 ns | 136.96 ns | 0.0536 |     704 B |

```
BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6456/22H2/2022Update) (VirtualBox)
12th Gen Intel Core i7-12700K 3.61GHz, 1 CPU, 4 logical and 4 physical cores
.NET SDK 10.0.201
  [Host]     : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
```
| Method                                                | CollectionSize | Mean      | Error    | StdDev   | Median    | Gen0   | Allocated |
|------------------------------------------------------ |--------------- |----------:|---------:|---------:|----------:|-------:|----------:|
| Map_FactInfraResponse_To_FactViewModel                | 10             |  27.65 ns | 0.609 ns | 1.658 ns |  28.48 ns | 0.0018 |      24 B |
| Map_FactInfraResponseList_To_IEnumerableFactViewModel | 10             | 139.68 ns | 3.282 ns | 9.626 ns | 138.76 ns | 0.0534 |     704 B |
