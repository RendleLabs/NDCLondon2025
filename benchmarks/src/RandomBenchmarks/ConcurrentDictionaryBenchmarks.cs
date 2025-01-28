using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace RandomBenchmarks;

[MemoryDiagnoser]
public class ConcurrentDictionaryBenchmarks
{
    private static readonly string[] _numbers = Enumerable.Range(0, 10000)
        .Select(x => (x % 16).ToString())
        .ToArray();
    
    [Benchmark(Baseline = true)]
    public ConcurrentDictionary<string, int> Concurrent()
    {
        var dict = new ConcurrentDictionary<string, int>();

        foreach (var number in _numbers)
        {
            dict.AddOrUpdate(number, 1, (key, value) => value + 1);
        }

        return dict;
    }

    [Benchmark]
    public Dictionary<string, int> LockOldFashioned()
    {
        var dict = new Dictionary<string, int>();
        var mutex = new object();
        
        foreach (var number in _numbers)
        {
            lock (mutex)
            {
                CollectionsMarshal.GetValueRefOrAddDefault(dict, number, out _) += 1;
            }
        }

        return dict;
    }

    [Benchmark]
    public Dictionary<string, int> LockNewFashioned()
    {
        var dict = new Dictionary<string, int>();
        var mutex = new Lock();
        
        foreach (var number in _numbers)
        {
            lock (mutex)
            {
                CollectionsMarshal.GetValueRefOrAddDefault(dict, number, out _) += 1;
            }
        }

        return dict;
    }
}