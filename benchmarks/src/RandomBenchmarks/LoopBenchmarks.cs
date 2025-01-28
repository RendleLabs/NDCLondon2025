using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace RandomBenchmarks;

[SimpleJob(RuntimeMoniker.Net48)]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[MemoryDiagnoser]
public class LoopBenchmarks
{
    private static readonly int[] _numbers = Enumerable.Range(0, 10000).ToArray();
    private static readonly List<int> _numbers2 = Enumerable.Range(0, 10000).ToList();

    [Benchmark(Baseline = true)]
    public long ForLoop()
    {
        long sum = 0;
        
        for (int i = 0; i < _numbers.Length; i++)
        {
            sum += _numbers[i];
        }

        return sum;
    }
    
    [Benchmark]
    public long Linq() => _numbers.Sum();
    //
    // [Benchmark]
    // public long LinqList() => _numbers2.Sum();

    // [Benchmark]
    // public long ForeachLoop()
    // {
    //     long sum = 0;
    //     
    //     foreach (var number in _numbers)
    //     {
    //         sum += number;
    //     }
    //
    //     return sum;
    // }
    
    /*
    [Benchmark]
    public long ForLoopList()
    {
        long sum = 0;
        
        for (int i = 0; i < _numbers2.Count; i++)
        {
            sum += _numbers2[i];
        }

        return sum;
    }

    [Benchmark]
    public long ForLoopListToArray()
    {
        long sum = 0;
        var numbers = _numbers2.ToArray();
        
        for (int i = 0; i < numbers.Length; i++)
        {
            sum += numbers[i];
        }

        return sum;
    }
    */

    // [Benchmark]
    // public long ForLoopListSpan()
    // {
    //     long sum = 0;
    //     var numbers = CollectionsMarshal.AsSpan(_numbers2);
    //     
    //     for (int i = 0; i < numbers.Length; i++)
    //     {
    //         sum += numbers[i];
    //     }
    //
    //     return sum;
    // }
    //
    //
    // [Benchmark]
    // public long ForeachLoopListSpan()
    // {
    //     long sum = 0;
    //     var numbers = CollectionsMarshal.AsSpan(_numbers2);
    //     
    //     foreach (var number in numbers)
    //     {
    //         sum += number;
    //     }
    //
    //     return sum;
    // }
    // [Benchmark]
    // public long ForeachLoopList()
    // {
    //     long sum = 0;
    //     
    //     foreach (var number in _numbers2)
    //     {
    //         sum += number;
    //     }
    //
    //     return sum;
    // }
}