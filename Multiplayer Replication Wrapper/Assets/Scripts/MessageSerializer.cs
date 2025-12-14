using System;
using System.Text;
using UnityEngine;

public static class MessageSerializer 
{
    public static byte[] Serialize(object obj)
    {
        string json = JsonUtility.ToJson(obj);
        return Encoding.UTF8.GetBytes(json);
    }

    public static T Deserialize<T>(byte[] data)
    {
        string json = Encoding.UTF8.GetString(data);
        return JsonUtility.FromJson<T>(json);
    }
}

