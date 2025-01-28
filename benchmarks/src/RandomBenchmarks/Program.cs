using BenchmarkDotNet.Running;
using RandomBenchmarks;

BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
