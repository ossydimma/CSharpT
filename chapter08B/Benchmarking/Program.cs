using BenchmarkDotNet.Running;
using System.Diagnostics;

BenchmarkRunner.Run<StringBenchmarks>(); //running benchmark

// checking if high Resolution is available on my system and checking the frequency of the timer in ticks per second.
Console.WriteLine($"High-resolution performance counter available: {Stopwatch.IsHighResolution}");
Console.WriteLine($"Timer frequency: {Stopwatch.Frequency} ticks per second");