using System;
using System.Configuration;
using StackExchange.Redis;

namespace DotNetCasClient.State
{
    /// <summary>
    /// Represents a singleton implementation of StackExchange.Redis ConnectionMultiplexer.  See https://github.com/StackExchange/StackExchange.Redis
    /// </summary>
    public class RedisClientManager
    {
        private static readonly Lazy<RedisClientManager> lazy = new Lazy<RedisClientManager>(() => new RedisClientManager());

        /// <summary>
        /// Gets the instance of RedisClientManager.
        /// </summary>
        public static RedisClientManager Instance { get { return lazy.Value; } }

        private ConnectionMultiplexer _redis;

        private RedisClientManager()
        {
            _redis = ConnectionMultiplexer.Connect("localhost");
        }

        /// <summary>
        /// Returns the object representing the Redis database implemented by StackExchange.Redis.
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase()

        {
            return _redis.GetDatabase(int.Parse(ConfigurationManager.AppSettings["cas:Redis:DatabaseNumber"]));
        }
    }
}
