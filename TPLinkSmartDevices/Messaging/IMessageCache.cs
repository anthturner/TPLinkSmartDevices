namespace TPLinkSmartDevices.Messaging
{
    public abstract class IMessageCache
    {
        public abstract dynamic Request(SmartHomeProtocolMessage message, string hostname, int port = 9999);
    }
}
