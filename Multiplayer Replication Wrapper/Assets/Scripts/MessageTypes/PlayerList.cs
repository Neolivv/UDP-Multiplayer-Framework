using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerList
{
    public PlayerData[] playerdatas;
}

[Serializable]
public class PlayerData
{
    public string id;
    public string address;
    public int port;
}