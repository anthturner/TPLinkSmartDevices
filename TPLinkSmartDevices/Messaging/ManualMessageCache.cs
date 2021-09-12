// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using System.Collections.Generic;
using System.Linq;

namespace TPLinkSmartDevices.Messaging
{
    public class ManualMessageCache : IMessageCache
    {
        private List<MessageCacheItem> _cache = new List<MessageCacheItem>();

        public void Flush()
        {
            _cache.Clear();
        }

        public override dynamic Request(SmartHomeProtocolMessage message, string hostname, int port = 9999)
        {
            var cachedMessage = _cache.FirstOrDefault(c => c.Matches(message, hostname, port));

            if (cachedMessage != null)
                return cachedMessage;

            var result = message.Execute(hostname, port);
            _cache.Add(new MessageCacheItem(result, hostname, port));
            return result;
        }

        protected class MessageCacheItem
        {
            internal int Hash { get; set; }
            internal string Hostname { get; set; }
            internal int Port { get; set; }
            internal dynamic MessageResult { get; set; }

            internal MessageCacheItem(dynamic messageResult, string hostname, int port)
            {
                MessageResult = messageResult;
                Hostname = hostname;
                Port = port;
            }

            internal bool Matches(SmartHomeProtocolMessage message, string hostname, int port)
            {
                if (Hostname != hostname || Port != port)
                    return false;

                return message.MessageHash == Hash;
            }
        }
    }
}
