using System.Text;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.ObjectPool;

namespace RandomBenchmarks;

[MemoryDiagnoser]
public class StringBuilderBenchmarks
{
    private static readonly ThreadLocal<StringBuilder> LocalBuilder = new(() => new StringBuilder());

    private static readonly ObjectPool<StringBuilder> Pool =
        new DefaultObjectPool<StringBuilder>(new StringBuilderPooledObjectPolicy());
    
    [Benchmark(Baseline = true)]
    public string NewBuilder()
    {
        var builder = new StringBuilder();

        for (char c = 'a'; c <= 'z'; c++)
        {
            builder.Append(c);
        }
        
        return builder.ToString();
    }
    
    [Benchmark]
    public string ThreadLocalBuilder()
    {
        var builder = LocalBuilder.Value ??= new StringBuilder();
        builder.Clear();

        for (char c = 'a'; c <= 'z'; c++)
        {
            builder.Append(c);
        }
        
        return builder.ToString();
    }
    
    [Benchmark]
    public string PooledBuilder()
    {
        var builder = Pool.Get();

        try
        {
            for (char c = 'a'; c <= 'z'; c++)
            {
                builder.Append(c);
            }
        
            return builder.ToString();
        }
        finally
        {
            Pool.Return(builder);
        }
    }
}