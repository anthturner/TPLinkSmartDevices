// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

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
        public event EventHandler<DeviceFoundEventArgs> DeviceFound;

        private int PORT_NUMBER = 9999;

        public List<TPLinkSmartDevice> DiscoveredDevices { get; private set; }

        private UdpClient udp;
        IAsyncResult _asyncResult = null;

        private bool discoveryComplete = false;
        private int maxNumberOfDevicesToDiscover = -1;
        private CancellationTokenSource delayCancellationToken = new CancellationTokenSource();
        public TPLinkDiscovery()
        {
            DiscoveredDevices = new List<TPLinkSmartDevice>();
        }
      
        public async Task<List<TPLinkSmartDevice>> Discover(int port=9999, int timeout=5000, int maxNumberOfDevicesToDiscover=-1)
        {
            discoveryComplete = false;
            this.maxNumberOfDevicesToDiscover = maxNumberOfDevicesToDiscover;

            DiscoveredDevices.Clear();
            PORT_NUMBER = port;

            var host = await Dns.GetHostEntryAsync(Dns.GetHostName());

            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    SendDiscoveryRequest(ip);
                }
            }

            udp = new UdpClient(PORT_NUMBER);
            StartListening();

            return await Task.Delay(TimeSpan.FromMilliseconds(timeout), delayCancellationToken.Token).ContinueWith(t =>
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
            var message = Encoding.UTF8.GetString(Messaging.SmartHomeProtocolEncoder.Decrypt(bytes));
            var sys_info = ((dynamic)JObject.Parse(message)).system.get_sysinfo;

            TPLinkSmartDevice device = null;
            if (((JObject) sys_info).Count != 0 && sys_info.model != null)
            {
                string model = (string) sys_info.model;

                if (model.StartsWith("HS110"))
                    device = new TPLinkSmartMeterPlug(ip.Address.ToString());
                else if (model.StartsWith("HS"))
                    device = new TPLinkSmartPlug(ip.Address.ToString());
                else if (model.StartsWith("LB"))
                    device = new TPLinkSmartBulb(ip.Address.ToString());
                else if (model.StartsWith("KP303"))
                    device = new TPLinkSmartStrip(ip.Address.ToString());
                else if (model.StartsWith("KL400"))
                    device = new TPLinkSmartLightStrip(ip.Address.ToString());
            }

            if (device != null)
            {
                DiscoveredDevices.Add(device);
                OnDeviceFound(device);

                // If the caller has specified a maximum number of devices to discover, stop waiting after we've
                // reached this count
                if (maxNumberOfDevicesToDiscover > 0 && DiscoveredDevices.Count >= maxNumberOfDevicesToDiscover)
                {
                    delayCancellationToken.Cancel();
                }
            }
            if (udp != null)
                StartListening();
        }

        private void SendDiscoveryRequest(IPAddress IP)
        {
            var ServerEp = new IPEndPoint(IP, PORT_NUMBER);

            UdpClient client = new UdpClient(ServerEp);
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

        private void OnDeviceFound(TPLinkSmartDevice device)
        {
            DeviceFound?.Invoke(this, new DeviceFoundEventArgs(device));
        }
    }

    public class DeviceFoundEventArgs : EventArgs
    {
        public TPLinkSmartDevice Device;

        public DeviceFoundEventArgs(TPLinkSmartDevice device)
        {
            Device = device;
        }
    }
}