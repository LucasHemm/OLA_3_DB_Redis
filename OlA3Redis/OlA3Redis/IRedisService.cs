namespace OlA3Redis;

public interface IRedisService
{
    Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<string?> GetValueAsync(string key);
    Task<bool> UpdateValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<bool> DeleteValueAsync(string key);
}
