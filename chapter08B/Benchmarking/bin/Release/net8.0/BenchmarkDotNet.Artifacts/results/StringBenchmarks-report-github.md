```

BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.4651/22H2/2022Update)
Intel Core i5-6200U CPU 2.30GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2


```
| Method                  | Mean     | Error    | StdDev   | Ratio | RatioSD |
|------------------------ |---------:|---------:|---------:|------:|--------:|
| StringConcatenationTest | 626.9 ns | 12.54 ns | 19.53 ns |  1.00 |    0.00 |
| StringBuilderTest       | 500.1 ns |  8.86 ns |  8.29 ns |  0.80 |    0.03 |
