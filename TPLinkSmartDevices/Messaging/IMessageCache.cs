// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

namespace TPLinkSmartDevices.Messaging
{
    public abstract class IMessageCache
    {
        public abstract dynamic Request(SmartHomeProtocolMessage message, string hostname, int port = 9999);
    }
}
