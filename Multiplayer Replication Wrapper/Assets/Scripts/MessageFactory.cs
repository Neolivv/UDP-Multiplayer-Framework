using UnityEngine;

public static class MessageFactory
{
    public static byte[] CreateMessage<T>(string type, T payload)
    {
        NetMessage wrapper = new NetMessage();
        wrapper.type = type;
        wrapper.payload = JsonUtility.ToJson(payload);

        return MessageSerializer.Serialize(wrapper);
    }

    public static T DecodePayLoad<T>(NetMessage message)
    {
        return JsonUtility.FromJson<T>(message.payload);
    }
}
