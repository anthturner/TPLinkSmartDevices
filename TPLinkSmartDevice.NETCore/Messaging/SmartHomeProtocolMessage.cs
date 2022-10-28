// Copyright (c) Anthony Turner
// Licensed under the Apache License, v2.0
// https://github.com/anthturner/TPLinkSmartDevices

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace TPLinkSmartDevices.Messaging
{
    public class SmartHomeProtocolMessage
    {
        public int MessageHash
        {
            get
            {
                var data = Encoding.ASCII.GetBytes(JSON);
                unchecked
                {
                    const int p = 16777619;
                    int hash = (int)2166136261;

                    for (int i = 0; i < data.Length; i++)
                        hash = (hash ^ data[i]) * p;

                    hash += hash << 13;
                    hash ^= hash >> 7;
                    hash += hash << 3;
                    hash ^= hash >> 17;
                    hash += hash << 5;
                    return hash;
                }
            }
        }

        public string JSON
        {
            get
            {
                object argObject;
                if (Value != null)
                    argObject = Argument != null? new JObject { new JProperty(Argument, Value) }: Value;
                else
                    argObject = Argument;

                var root = new JObject
                {
                    
                };

                root.Add(System, new JObject
                {
                    new JProperty(Command, (argObject != null ? JToken.FromObject(argObject) : null))
                });

                if (ChildIdS != null)
                {
                    var childerIDs = new JArray();

                    ChildIdS.ForEach(s =>
                    {
                        childerIDs.Add(s);
                    });

                    var children = new JObject
                    {
                        ["child_ids"] = childerIDs
                    };


                    root.Add("context", children);
                }

                return root.ToString(Newtonsoft.Json.Formatting.None);
            }
        }

        public string System { get; set; }
        public string Command { get; set; }
        public string Argument { get; set; }
        public object Value { get; set; }
        public List<string> ChildIdS = new List<string>();

        internal SmartHomeProtocolMessage(string system, string command, string argument, object value, List<string> childIdS)
        {
            System = system;
            Command = command;
            Argument = argument;
            Value = value;
            ChildIdS = childIdS;
        }

        internal dynamic Execute(string hostname, int port)
        {
            var messageToSend = SmartHomeProtocolEncoder.Encrypt(JSON);

            var client = new TcpClient(hostname, port);
            byte[] packet = new byte[0];
            using (var stream = client.GetStream())
            {
                stream.Write(messageToSend, 0, messageToSend.Length);

                int targetSize = 0;
                var buffer = new List<byte>();
                while (true)
                {
                    var chunk = new byte[1024];
                    var bytesReceived = stream.Read(chunk, 0, chunk.Length);

                    if (!buffer.Any())
                    {
                        var lengthBytes = chunk.Take(4).ToArray();
                        if (BitConverter.IsLittleEndian) // this value needs to be in big-endian
                            lengthBytes = lengthBytes.Reverse().ToArray();
                        targetSize = (int)BitConverter.ToUInt32(lengthBytes, 0);
                    }
                    buffer.AddRange(chunk.Take(bytesReceived));

                    if (buffer.Count == targetSize + 4)
                        break;
                }

                packet = buffer.Skip(4).Take(targetSize).ToArray();
            }
            client.Close();

            var decrypted = Encoding.UTF8.GetString(SmartHomeProtocolEncoder.Decrypt(packet)).Trim('\0');

            var subResult = (dynamic)((JObject)JObject.Parse(decrypted)[System])[Command];
            if (subResult["err_code"] != null && subResult.err_code != 0)
                throw new Exception($"Protocol error {subResult.err_code} ({subResult.err_msg})");

            return subResult;
        }
    }
}
