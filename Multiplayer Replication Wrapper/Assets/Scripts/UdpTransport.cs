using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpTransport : MonoBehaviour
{
    public Action<byte[]> OnData;


    UdpClient udpclient;
    IPEndPoint anyIP;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Initialize(int listenPort, string serverIP, int serverPort)
    {
        udpclient = new UdpClient(0);
        udpclient.Connect(serverIP, serverPort);

        anyIP = new IPEndPoint(IPAddress.Any, 0);
        StartListening();
    }
    void StartListening()
    {
        udpclient.BeginReceive(OnUdpDataReceive, null);
    }

    void OnUdpDataReceive(IAsyncResult result)
    {
        try
        {
            byte[] data = udpclient.EndReceive(result, ref anyIP);

            OnData?.Invoke(data);
        }
        catch (Exception ex)
        {
            print("[Client] Exception thronw " + ex.Message);
        }
        finally
        {
            udpclient.BeginReceive(OnUdpDataReceive, null);
        }
    }


    public void Send(byte[] bytes)
    {
        udpclient.Send(bytes, bytes.Length);
    }


}
