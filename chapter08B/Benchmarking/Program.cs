using BenchmarkDotNet.Running;
using System.Diagnostics;

//BenchmarkRunner.Run<StringBenchmarks>();

Console.WriteLine($"High-resolution performance counter available: {Stopwatch.IsHighResolution}");
Console.WriteLine($"Timer frequency: {Stopwatch.Frequency} ticks per second");