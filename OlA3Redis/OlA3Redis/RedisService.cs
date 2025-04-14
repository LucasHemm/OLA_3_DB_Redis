namespace OlA3Redis;

using StackExchange.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;

    public RedisService(IConnectionMultiplexer connection)
    {
        _db = connection.GetDatabase();
    }

    public async Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        return await _db.StringSetAsync(key, value, expiry);
    }

    public async Task<string?> GetValueAsync(string key)
    {
        var result = await _db.StringGetAsync(key);
        return result.IsNullOrEmpty ? null : result.ToString();
    }

    public async Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        if (!await _db.KeyExistsAsync(key))
            return false;

        return await _db.StringSetAsync(key, value, expiry);
    }

    public async Task<bool> DeleteValueAsync(string key)
    {
        return await _db.KeyDeleteAsync(key);
    }
}