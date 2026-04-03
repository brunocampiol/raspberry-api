# Benchmark

Following are the benchmark results for the mapping of weather data using three different methods: Mapper, Manual, and AutoMapper. 
The code related to AutoMapper has been removed and the mapper extension will be used instead.
Manual is baseline means the mapping is done directly in the code without any abstraction. 
Mapper is the custom mapping implemented via extensions.
AutoMapper is, of course, the v14 popular library for object-to-object mapping in .NET.

## RaspberryPi.Benchmarks.WeatherSingleBenchmarks

```
Date: 2026-04-02
BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6456/22H2/2022Update) (VirtualBox)
12th Gen Intel Core i7-12700K 3.61GHz, 1 CPU, 4 logical and 4 physical cores
.NET SDK 10.0.201
  [Host]    : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
Job=.NET 10.0  Runtime=.NET 10.0  
```

| Method     | Mean      | Error     | StdDev    | Median    | Gen0   | Allocated |
|----------- |----------:|----------:|----------:|----------:|-------:|----------:|
| Mapper     |  5.705 ns | 0.1668 ns | 0.3764 ns |  5.545 ns | 0.0037 |      48 B |
| Manual     |  6.306 ns | 0.1794 ns | 0.1401 ns |  6.333 ns | 0.0037 |      48 B |
| AutoMapper | 30.002 ns | 0.6619 ns | 1.5858 ns | 30.717 ns | 0.0036 |      48 B |

## RaspberryPi.Benchmarks.WeatherCollectionBenchmarks

```
Date: 2026-04-02
BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6456/22H2/2022Update) (VirtualBox)
12th Gen Intel Core i7-12700K 3.61GHz, 1 CPU, 4 logical and 4 physical cores
.NET SDK 10.0.201
  [Host]    : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
  .NET 10.0 : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
Job=.NET 10.0  Runtime=.NET 10.0  
```


| Method     | N    | Mean          | Error         | StdDev        | Median        | Gen0    | Allocated |
|----------- |----- |--------------:|--------------:|--------------:|--------------:|--------:|----------:|
| Mapper     | 10   |      87.66 ns |      1.426 ns |      1.264 ns |      88.07 ns |  0.0526 |     688 B |
| Manual     | 10   |      96.60 ns |      1.839 ns |      1.536 ns |      96.65 ns |  0.0526 |     688 B |
| AutoMapper | 10   |     167.24 ns |      3.414 ns |      9.403 ns |     172.78 ns |  0.0618 |     808 B |
| Manual     | 100  |     733.95 ns |     11.958 ns |      9.985 ns |     728.69 ns |  0.4368 |    5728 B |
| Mapper     | 100  |     830.37 ns |     18.486 ns |     54.217 ns |     845.24 ns |  0.4377 |    5728 B |
| AutoMapper | 100  |   1,130.33 ns |     22.515 ns |     30.056 ns |   1,131.97 ns |  0.5341 |    6992 B |
| Mapper     | 1000 |   7,653.11 ns |    125.748 ns |    111.473 ns |   7,621.80 ns |  4.2877 |   56128 B |
| Manual     | 1000 |   8,404.71 ns |    164.150 ns |    207.598 ns |   8,459.10 ns |  4.2877 |   56128 B |
| AutoMapper | 1000 |  10,654.59 ns |    212.945 ns |    597.122 ns |  10,905.72 ns |  4.9286 |   64600 B |
| Manual     | 10000|  92,787.16 ns |  1,540.928 ns |  1,365.993 ns |  93,057.93 ns | 42.7246 |  560128 B |
| Mapper     | 10000|  96,411.60 ns |  1,181.043 ns |  1,104.748 ns |  96,543.09 ns | 42.7246 |  560128 B |
| AutoMapper | 10000| 378,961.34 ns |  7,504.137 ns | 16,628.650 ns | 386,476.51 ns | 83.0078 |  742470 B |
