using Eidetic.Utility;
using OscJack;
using System.Collections.Generic;
using UnityEngine;

namespace Eidetic.Confluence.Networking
{
    [CreateNodeMenu("Networking/OscReceiver"), NodeTint(Colors.ExternalInputTint)]
    public class OscReceiver : RuntimeNode
    {
        internal static Dictionary<int, OscServer> Servers = new Dictionary<int, OscServer>();
        internal static Dictionary<OscServer, List<OscReceiver>> Receivers = new Dictionary<OscServer, List<OscReceiver>>();

        [SerializeField] public int Port = 9000;
        [SerializeField] public string MessageAddress = "/address-name";

        [Output] public int Integer = 0;
        [Output] public float Float = 0f;
        [Output] public bool Boolean = false;
        bool trigger = false;
        [Output] public bool Trigger
        {
            get
            {
                if (trigger)
                    return !(trigger = false);
                return trigger;
            }
        }

        OscServer Server;

        new public void OnEnable()
        {
            base.OnEnable();

            if (!Servers.ContainsKey(Port))
                Servers.Add(Port, Server = new OscServer(Port));
            else Server = Servers[Port];

            if (Receivers.ContainsKey(Server))
                Receivers[Server].Add(this);
            else Receivers[Server] = new List<OscReceiver>().With(this);

            Server.MessageDispatcher.AddRootNodeCallback("pascal", OnMessageReceived);
        }

        new void OnDestroy()
        {
            Receivers[Server].Remove(this);
            if (Receivers[Server].Count == 0)
            {
                Receivers.Remove(Server);
                Servers.Remove(Port);
                Server.Dispose();
            }

            Server.MessageDispatcher.RemoveRootNodeCallback("pascal", OnMessageReceived);
        }

        internal override void Update()
        { }

        void OnMessageReceived(string address, OscDataHandle data)
        {
            UnityEngine.Debug.Log(address + ": " + data.GetElementAsString(0));
            var subAddressStartIndex = address.IndexOf('/', 1);
            var subAddress = address.Substring(subAddressStartIndex, address.Length - subAddressStartIndex);
            if (subAddress == MessageAddress | subAddress.TrimStart('/') == MessageAddress)
            {
                Integer = data.GetElementAsInt(0);
                Float = data.GetElementAsFloat(0);
                Boolean = Integer != 0;
                trigger = Boolean;

                if (Debug)
                    UnityEngine.Debug.Log(Float);
            }
        }
    }
}