/*
 * Licensed to Apereo under one or more contributor license
 * agreements. See the NOTICE file distributed with this work
 * for additional information regarding copyright ownership.
 * Apereo licenses this file to you under the Apache License,
 * Version 2.0 (the "License"); you may not use this file
 * except in compliance with the License. You may obtain a
 * copy of the License at:
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on
 * an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

using System;
using System.Configuration;
using StackExchange.Redis;

namespace DotNetCasClient.State
{
    /// <summary>
    /// Represents a singleton implementation of StackExchange.Redis ConnectionMultiplexer.  See https://github.com/StackExchange/StackExchange.Redis
    /// </summary>
    ///<author>Jason Kanaris</author>
    public sealed class RedisClientManager
    {
        private static readonly Lazy<RedisClientManager> lazy = new Lazy<RedisClientManager>(() => new RedisClientManager());

        /// <summary>
        /// Gets the instance of RedisClientManager.
        /// </summary>
        public static RedisClientManager Instance { get { return lazy.Value; } }

        private ConnectionMultiplexer _redis;

        private RedisClientManager()
        {
            _redis = ConnectionMultiplexer.Connect(ConfigurationManager.AppSettings["cas:Redis:ConnectionString"]);
        }

        /// <summary>
        /// Returns the object representing the Redis database implemented by StackExchange.Redis.
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDatabase()

        {
            return _redis.GetDatabase();
        }
    }
}
