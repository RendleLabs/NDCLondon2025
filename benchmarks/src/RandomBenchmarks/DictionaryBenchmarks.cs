using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace RandomBenchmarks;

[MemoryDiagnoser]
public class DictionaryBenchmarks
{
    private static readonly string[] _numbers = Enumerable.Range(0, 10000)
        .Select(x => (x % 16).ToString())
        .ToArray();

    [Benchmark(Baseline = true)]
    public Dictionary<string, int> TryGetValue()
    {
        var dict = new Dictionary<string, int>();

        foreach (var number in _numbers)
        {
            if (dict.TryGetValue(number, out var value))
            {
                dict[number] = value + 1;
            }
            else
            {
                dict[number] = 1;
            }
        }

        return dict;
    }

    [Benchmark]
    public Dictionary<string, int> CollectionsMarshalRef()
    {
        var dict = new Dictionary<string, int>();
        
        foreach (var number in _numbers)
        {
            CollectionsMarshal.GetValueRefOrAddDefault(dict, number, out _) += 1;
        }

        return dict;
    }
}