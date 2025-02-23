using StackExchange.Redis;

namespace RedisExampleApp.Cache;

public class RedisService(string connectionString)
{
    private readonly ConnectionMultiplexer _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);

    public IDatabase GetDatabase(int dbIndex = 0)
    {
        return _connectionMultiplexer.GetDatabase(dbIndex);
    }
}