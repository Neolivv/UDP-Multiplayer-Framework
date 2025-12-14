using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    public UdpTransport transport;

    [HideInInspector] public string PlayerID;

    //Events
    public Action<string> OnPlayerJoined;

    //Ping
    [ReadOnly(true)] float PingInterval = 1f;
    int PingSeq = 0;
    Dictionary<int, double> pendingPings = new Dictionary<int, double>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        transport.Initialize(5600, "127.0.0.1", 5500);

        transport.OnData += OnDataReceived;

        //GenerateID
        PlayerID = (SystemInfo.deviceUniqueIdentifier) + (Time.time.ToString()) + (RandomNumberGenerator.GetInt32(20));

        //Joining
        Join();

        //Ping
        StartCoroutine(PingLoop());
    }
    public void Join()
    {
        OnPlayerJoined?.Invoke(PlayerID);

        var joining_msg = new PlayerJoin { PlayerID = PlayerID };
        byte[] data = MessageFactory.CreateMessage("PlayerJoin", joining_msg);
        transport.Send(data);

    }

    public void Leave()
    {
        var leaving_msg = new PlayerLeave { PlayerID = PlayerID };
        byte[] data = MessageFactory.CreateMessage("PlayerLeave", leaving_msg);
        transport.Send(data);

    }

    public void Send(byte[] bytes) => transport.Send(bytes);
    void OnDataReceived(byte[] data)
    {
        NetMessage net = MessageSerializer.Deserialize<NetMessage>(data);
        MessageDispatcher.Handle(net);
    }

    IEnumerator PingLoop()
    {
        yield return new WaitForSeconds(PingInterval);

        while (true)
        {
            SendPing();
            yield return new WaitForSeconds(PingInterval);
        }
    }
    void SendPing()
    {
        PingSeq++;
        double now = Time.realtimeSinceStartupAsDouble;

        pendingPings[PingSeq] = now;
        NetPing ping = new NetPing { TimeStamp = now, Sequence = PingSeq };

        byte[] data = MessageFactory.CreateMessage<NetPing>("NetPing", ping);
        Send(data);
    }
    public void OnPong(NetPing ping)
    {
        if(ping == null)
        {
            Debug.Log("Ping Received is Null");
            return;
        }
        if(!pendingPings.TryGetValue(ping.Sequence,out double sentTime))
        {
            return;
        }

        double RTT = (Time.realtimeSinceStartupAsDouble - sentTime) * 1000.0f;
        pendingPings.Remove(ping.Sequence);

        UIManager.Instance.UpdatePing(RTT);
    }
    private void OnApplicationQuit()
    {
        Leave();
    }
}
