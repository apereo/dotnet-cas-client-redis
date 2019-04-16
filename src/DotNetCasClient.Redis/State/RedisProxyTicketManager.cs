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
using DotNetCasClient.Utils;

namespace DotNetCasClient.State
{
    ///<summary>
    /// An IProxyTicketManager implementation that relies on Redis for proxy ticket
    /// storage.  This model allows for distributed caching of proxy tickets in order to
    /// support clustered, load balanced, or round-robin style configurations so that
    /// authentication state can be maintained across multiple servers and recycling of
    /// IIS application pools or server restarts.
    ///</summary>
    ///<author>Jason Kanaris</author>
    public sealed class RedisProxyTicketManager : IProxyTicketManager
    {
        /// <summary>
        /// This prefix is prepended to CAS Proxy Granting Ticket IOU as the key to the cache.
        /// </summary>
        private const string CACHE_PGTIOU_KEY_PREFIX = "PGTIOU::";

        private static readonly TimeSpan DefaultExpiration = new TimeSpan(0, 0, 3, 0); // 180 seconds\

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RedisProxyTicketManager()
        {
        }

        /// <summary>
        /// You retrieve CasAuthentication properties in the constructor or else you will cause
        /// a StackOverflow.  CasAuthentication.Initialize() will call Initialize() on all
        /// relevant controls when its initialization is complete.  In Initialize(), you can
        /// retrieve properties from CasAuthentication.
        /// </summary>
        public void Initialize()
        {
            // Do nothing
        }

        /// <summary>
        /// Removes expired PGTIOU-PGT from the ticket store
        /// </summary>
        public void RemoveExpiredMappings()
        {
            // No-op.  The RedisClient removes expired entries automatically.
        }

        /// <summary>
        /// Method to save the ProxyGrantingTicket to the backing storage facility.
        /// </summary>
        /// <param name="proxyGrantingTicketIou">used as the key</param>
        /// <param name="proxyGrantingTicket">used as the value</param>
        public void InsertProxyGrantingTicketMapping(string proxyGrantingTicketIou, string proxyGrantingTicket)
        {
            CommonUtils.AssertNotNullOrEmpty(proxyGrantingTicketIou, "proxyGrantingTicketIou parameter cannot be null or empty.");

            CommonUtils.AssertNotNullOrEmpty(proxyGrantingTicket, "proxyGrantingTicket parameter cannot be null or empty.");

            var db = RedisClientManager.Instance.GetDatabase();

            db.StringSet(GetTicketKey(proxyGrantingTicketIou), proxyGrantingTicket,
                DefaultExpiration);
        }

        /// <summary>
        /// Method to retrieve a ProxyGrantingTicket based on the
        /// ProxyGrantingTicketIou.  Implementations are not guaranteed to
        /// return the same result if retieve is called twice with the same
        /// proxyGrantingTicketIou.
        /// </summary>
        /// <param name="proxyGrantingTicketIou">used as the key</param>
        /// <returns>the ProxyGrantingTicket Id or null if it can't be found</returns>
        public string GetProxyGrantingTicket(string proxyGrantingTicketIou)
        {
            CommonUtils.AssertNotNullOrEmpty(proxyGrantingTicketIou, "proxyGrantingTicketIou parameter cannot be null or empty.");

            var db = RedisClientManager.Instance.GetDatabase();

            return db.StringGet(GetTicketKey(proxyGrantingTicketIou));
        }

        /// <summary>
        /// Converts a CAS Proxy Granting Ticket IOU to its corresponding key in the ticket manager store (cache provider).
        /// </summary>
        /// <param name="proxyGrantingTicketIou">
        /// The CAS Proxy Granting Ticket IOU to convert.
        /// </param>
        /// <returns>
        /// The cache key associated with the corresponding Proxy Granting Ticket IOU
        /// </returns>
        /// <exception cref="ArgumentNullException">proxyGrantingTicketIou is null</exception>
        /// <exception cref="ArgumentException">proxyGrantingTicketIou is empty</exception>
        private static string GetTicketKey(string proxyGrantingTicketIou)
        {
            CommonUtils.AssertNotNullOrEmpty(proxyGrantingTicketIou, "proxyGrantingTicketIou parameter cannot be null or empty.");

            return CACHE_PGTIOU_KEY_PREFIX + proxyGrantingTicketIou;
        }
    }
}
