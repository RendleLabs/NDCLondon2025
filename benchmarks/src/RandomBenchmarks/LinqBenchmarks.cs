using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;

namespace RandomBenchmarks;

[DisassemblyDiagnoser(printSource: true)]
public class LinqBenchmarks
{
    private static readonly string[] _words = ["One", "Two", "Three", "Seventeen", "Foo"];

    // [Benchmark(Baseline = true)]
    // public int LinqMax()
    // {
    //     return _words.Max(x => x.Length);
    // }
    //
    // [Benchmark]
    // public int LinqSelectMax()
    // {
    //     return _words.Select(x => x.Length).Max();
    // }

    [Benchmark]
    public int Aggregate()
    {
        return _words.Aggregate(0, (max, word) => word.Length > max ? word.Length : max);
    }
    
    [Benchmark]
    public int Loop()
    {
        int max = 0;
        
        foreach (var word in _words)
        {
            if (word.Length > max)
            {
                max = word.Length;
            }
        }
        
        return max;
    }
    
    // [Benchmark]
    // public int LoopMath()
    // {
    //     int max = 0;
    //     
    //     foreach (var word in _words)
    //     {
    //         max = Math.Max(max, word.Length);
    //     }
    //     
    //     return max;
    // }
}