using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace CleanArchitecture.Application;

public static class DistributedCacheExtensions
{
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        var json = await cache.GetStringAsync(key, token);
        var value = JsonSerializer.Deserialize<T>(json);
        return value;
    }

    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, CancellationToken token = default)
    {
        var json = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, json, token);
    }

    public static async Task SetAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        var json = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, json, options, token);
    }
}
