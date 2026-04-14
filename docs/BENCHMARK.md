# Benchmark

## Current benchmarks

Results have been sorted by type and then by mean time.

| Type                        | Method     | N     | Mean           | Error         | StdDev         | Median         | Ratio | RatioSD | Gen0    | Gen1    | Allocated | Alloc Ratio |
|---------------------------- |----------- |------ |---------------:|--------------:|---------------:|---------------:|------:|--------:|--------:|--------:|----------:|------------:|
| **FactSingleBenchmarks**    |            |      |                |               |                |                |       |         |        |         |           |             |
| FactSingleBenchmarks        | Baseline   | ?     |       3.476 ns |     0.1357 ns |      0.3806 ns |       3.368 ns |  1.01 |    0.16 |  0.0018 |       - |      24 B |        1.00 |
| FactSingleBenchmarks        | AutoMapper | ?     |       3.528 ns |     0.1334 ns |      0.3273 ns |       3.506 ns |  1.03 |    0.15 |  0.0018 |       - |      24 B |        1.00 |
| **EmailSingleBenchmarks**   |            |      |                |               |                |                |       |         |        |         |           |             |
| EmailSingleBenchmarks       | Mapper     | ?     |       7.150 ns |     0.2722 ns |      0.8025 ns |       7.219 ns |  2.08 |    0.32 |  0.0037 |       - |      48 B |        2.00 |
| EmailSingleBenchmarks       | Baseline   | ?     |       7.347 ns |     0.3572 ns |      1.0477 ns |       7.183 ns |  2.14 |    0.38 |  0.0037 |       - |      48 B |        2.00 |
| **WeatherSingleBenchmarks** |            |      |                |               |                |                |       |         |        |         |           |             |
| WeatherSingleBenchmarks     | Mapper     | ?     |       8.014 ns |     0.3635 ns |      1.0719 ns |       8.090 ns |  2.33 |    0.40 |  0.0037 |       - |      48 B |        2.00 |
| WeatherSingleBenchmarks     | Baseline   | ?     |       8.592 ns |     0.3181 ns |      0.9330 ns |       8.550 ns |  2.50 |    0.38 |  0.0037 |       - |      48 B |        2.00 |
| **FactCollectionBenchmarks**|            |      |                |               |                |                |       |         |        |         |           |             |
| FactCollectionBenchmarks    | AutoMapper | 10    |      64.685 ns |     1.6408 ns |      4.8380 ns |      62.134 ns |  0.95 |    0.09 |  0.0342 |       - |     448 B |        1.00 |
| FactCollectionBenchmarks    | Baseline   | 10    |      68.547 ns |     1.4091 ns |      3.5865 ns |      68.186 ns |  1.00 |    0.07 |  0.0342 |       - |     448 B |        1.00 |
| FactCollectionBenchmarks    | Baseline   | 100   |     584.506 ns |    12.8036 ns |     37.7518 ns |     597.124 ns |  1.00 |    0.09 |  0.2546 |  0.0029 |    3328 B |        1.00 |
| FactCollectionBenchmarks    | AutoMapper | 100   |     599.831 ns |    11.9759 ns |     30.6987 ns |     604.169 ns |  1.03 |    0.09 |  0.2546 |  0.0029 |    3328 B |        1.00 |
| FactCollectionBenchmarks    | Baseline   | 1000  |   5,725.938 ns |   118.0825 ns |    313.1381 ns |   5,839.770 ns |  1.00 |    0.08 |  2.4567 |  0.2670 |   32128 B |        1.00 |
| FactCollectionBenchmarks    | AutoMapper | 1000  |   5,769.052 ns |   134.1586 ns |    391.3470 ns |   5,908.965 ns |  1.01 |    0.09 |  2.4567 |  0.2670 |   32128 B |        1.00 |
| FactCollectionBenchmarks    | Baseline   | 10000 |  54,108.891 ns | 1,077.2796 ns |  3,056.0630 ns |  55,068.671 ns |  1.00 |    0.08 | 24.3530 |  8.1177 |  320128 B |        1.00 |
| FactCollectionBenchmarks    | AutoMapper | 10000 |  64,907.698 ns | 1,564.5790 ns |  4,588.6396 ns |  65,215.338 ns |  1.20 |    0.11 | 24.3530 |  8.1177 |  320128 B |        1.00 |
| **EmailCollectionBenchmarks**|           |      |                |               |                |                |       |         |        |         |           |             |
| EmailCollectionBenchmarks   | Baseline   | 10    |      93.806 ns |     2.5251 ns |      7.4056 ns |      91.628 ns |  1.37 |    0.13 |  0.0526 |       - |     688 B |        1.54 |
| EmailCollectionBenchmarks   | Mapper     | 10    |     112.321 ns |     3.0789 ns |      8.9324 ns |     111.840 ns |  1.64 |    0.16 |  0.0526 |       - |     688 B |        1.54 |
| EmailCollectionBenchmarks   | Baseline   | 100   |     849.097 ns |    24.2277 ns |     71.0557 ns |     833.949 ns |  1.46 |    0.16 |  0.4377 |  0.0086 |    5728 B |        1.72 |
| EmailCollectionBenchmarks   | Mapper     | 100   |     905.870 ns |    22.1924 ns |     63.6741 ns |     891.218 ns |  1.56 |    0.15 |  0.4377 |  0.0086 |    5728 B |        1.72 |
| EmailCollectionBenchmarks   | Baseline   | 1000  |   8,315.318 ns |   169.2808 ns |    480.2216 ns |   8,479.020 ns |  1.46 |    0.12 |  4.2877 |  0.7477 |   56128 B |        1.75 |
| EmailCollectionBenchmarks   | Mapper     | 1000  |   8,825.305 ns |   231.2909 ns |    663.6169 ns |   8,802.783 ns |  1.55 |    0.15 |  4.2877 |  0.7477 |   56128 B |        1.75 |
| EmailCollectionBenchmarks   | Mapper     | 10000 | 107,461.390 ns | 2,990.9483 ns |  8,677.2839 ns | 108,611.011 ns |  1.99 |    0.20 | 42.7246 | 14.6484 |  560128 B |        1.75 |
| EmailCollectionBenchmarks   | Baseline   | 10000 | 111,759.175 ns | 3,633.8583 ns | 10,714.5141 ns | 109,262.372 ns |  2.07 |    0.23 | 42.7246 | 14.6484 |  560128 B |        1.75 |
| **WeatherCollectionBenchmarks**|         |      |                |               |                |                |       |         |        |         |           |             |
| WeatherCollectionBenchmarks | Baseline   | 10    |     117.310 ns |     4.9062 ns |     14.4661 ns |     117.075 ns |  1.72 |    0.23 |  0.0525 |       - |     688 B |        1.54 |
| WeatherCollectionBenchmarks | Mapper     | 10    |     122.414 ns |     3.8786 ns |     11.1906 ns |     120.765 ns |  1.79 |    0.19 |  0.0525 |       - |     688 B |        1.54 |
| WeatherCollectionBenchmarks | Baseline   | 100   |     949.318 ns |    26.4735 ns |     75.5304 ns |     946.130 ns |  1.63 |    0.17 |  0.4368 |  0.0076 |    5728 B |        1.72 |
| WeatherCollectionBenchmarks | Mapper     | 100   |   1,035.850 ns |    34.8901 ns |    102.8741 ns |   1,042.369 ns |  1.78 |    0.21 |  0.4377 |  0.0086 |    5728 B |        1.72 |
| WeatherCollectionBenchmarks | Mapper     | 1000  |   9,272.477 ns |   236.6660 ns |    675.2216 ns |   9,216.765 ns |  1.62 |    0.15 |  4.2877 |  0.7477 |   56128 B |        1.75 |
| WeatherCollectionBenchmarks | Baseline   | 1000  |   9,729.273 ns |   236.7032 ns |    679.1459 ns |   9,569.561 ns |  1.70 |    0.15 |  4.2877 |  0.7477 |   56128 B |        1.75 |
| WeatherCollectionBenchmarks | Mapper     | 10000 | 123,580.737 ns | 2,746.6192 ns |  7,836.2597 ns | 123,194.678 ns |  2.29 |    0.20 | 42.7246 | 14.6484 |  560128 B |        1.75 |
| WeatherCollectionBenchmarks | Baseline   | 10000 | 132,163.578 ns | 3,693.0160 ns | 10,830.9775 ns | 132,152.222 ns |  2.45 |    0.25 | 42.7246 | 14.6484 |  560128 B |        1.75 |

## Previous focused benchmarks

Following are the benchmark results for the mapping of weather data using three different methods: Mapper, Manual, and AutoMapper. 
The code related to AutoMapper has been removed and the mapper extension will be used instead.
Manual is baseline means the mapping is done directly in the code without any abstraction. 
Mapper is the custom mapping implemented via extensions (using this).
AutoMapper is, of course, the v14 popular library for object-to-object mapping in .NET.

### RaspberryPi.Benchmarks.WeatherSingleBenchmarks

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

### RaspberryPi.Benchmarks.WeatherCollectionBenchmarks

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
