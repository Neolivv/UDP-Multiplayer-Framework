using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingCalculator : MonoBehaviour
{
    public static PingCalculator Instance;

    float PingInterval = 1f;
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
        StartCoroutine(PingLoop());
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
        NetworkManager.Instance.Send(data);
    }
    public void OnPong(NetPing ping)
    {
        if (ping == null)
        {
            Debug.Log("Ping Received is Null");
            return;
        }
        if (!pendingPings.TryGetValue(ping.Sequence, out double sentTime))
        {
            return;
        }

        double RTT = (Time.realtimeSinceStartupAsDouble - sentTime) * 1000.0f;
        pendingPings.Remove(ping.Sequence);

        UIManager.Instance.UpdatePing(RTT);
    }
}
