namespace TPLinkSmartDevices.Messaging
{
    public class NoMessageCache : IMessageCache
    {
        public override dynamic Request(SmartHomeProtocolMessage message, string hostname, int port = 9999)
        {
            return message.Execute(hostname, port);
        }
    }
}
