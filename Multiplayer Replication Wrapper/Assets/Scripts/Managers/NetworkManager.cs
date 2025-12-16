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

    
    

    private void OnApplicationQuit()
    {
        Leave();
    }
}
