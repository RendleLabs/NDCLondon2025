using System.Buffers;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace RandomBenchmarks;

[MemoryDiagnoser]
public class ArrayPoolBenchmarks
{
    private static readonly ArrayPool<byte> Pool = ArrayPool<byte>.Shared;

    [Benchmark(Baseline = true)]
    public long NewArray()
    {
        var array = new byte[1024];
        Random.Shared.NextBytes(array);
        return MemoryMarshal.Read<long>(array);
    }

    [Benchmark]
    public long PooledArray()
    {
        var array = Pool.Rent(1024);
        try
        {
            Random.Shared.NextBytes(array.AsSpan(0, 1024));
            return MemoryMarshal.Read<long>(array);
        }
        finally
        {
            Pool.Return(array);
        }
    }
}