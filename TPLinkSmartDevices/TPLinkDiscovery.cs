using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TPLinkSmartDevices.Devices;

namespace TPLinkSmartDevices
{
    public class TPLinkDiscovery
    {
        private int PORT_NUMBER = 9999;

        public List<TPLinkSmartDevice> DiscoveredDevices { get; private set; }

        private UdpClient udp;
        IAsyncResult _asyncResult = null;

        private bool discoveryComplete = false;

        public TPLinkDiscovery()
        {
            DiscoveredDevices = new List<TPLinkSmartDevice>();
        }

        public async Task<List<TPLinkSmartDevice>> Discover(int port=9999, int timeout=5000)
        {
            discoveryComplete = false;

            DiscoveredDevices.Clear();
            PORT_NUMBER = port;

            SendDiscoveryRequest();
            
            udp = new UdpClient(PORT_NUMBER);
            StartListening();

            return await Task.Delay(timeout).ContinueWith(t =>
            {
                discoveryComplete = true;
                udp.Close();
                udp = null;

                return DiscoveredDevices;
            });
        }

        private void StartListening()
        {
            _asyncResult = udp.BeginReceive(Receive, new object());
        }
        private void Receive(IAsyncResult ar)
        {
            if (discoveryComplete) //Prevent ObjectDisposedException/NullReferenceException when the Close() function is called
                return;

                IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
                byte[] bytes = udp.EndReceive(ar, ref ip);
                var message = Encoding.ASCII.GetString(Messaging.SmartHomeProtocolEncoder.Decrypt(bytes));
                var sys_info = ((dynamic)JObject.Parse(message)).system.get_sysinfo;

                TPLinkSmartDevice device = null;
                if (((string)sys_info.model).StartsWith("HS"))
                    device = new TPLinkSmartPlug(ip.Address.ToString());
                else if (((string)sys_info.model).StartsWith("LB"))
                    device = new TPLinkSmartBulb(ip.Address.ToString());

                if (device != null)
                    DiscoveredDevices.Add(device);
            
            if (udp != null)
                StartListening();
        }

        private void SendDiscoveryRequest()
        {
            UdpClient client = new UdpClient(PORT_NUMBER);
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, PORT_NUMBER);

            var discoveryJson = JObject.FromObject(new
            {
                system = new { get_sysinfo = (object)null },
                emeter = new { get_realtime = (object)null }
            }).ToString(Newtonsoft.Json.Formatting.None);
            var discoveryPacket = Messaging.SmartHomeProtocolEncoder.Encrypt(discoveryJson).ToArray();

            var bytes = discoveryPacket.Skip(4).ToArray();
            client.EnableBroadcast = true;
            client.Send(bytes, bytes.Length, ip);
            client.Close();
        }
    }
}
