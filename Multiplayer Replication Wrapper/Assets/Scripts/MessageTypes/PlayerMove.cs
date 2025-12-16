using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerMove 
{
    public PlayerPosition Position;
    public int Sequence;
}

[Serializable]
public class PlayerPosition
{
    public float x, y, z;
}

[Serializable]
public class InputPacket
{
    public string PlayerID;
    public List<InputFrame> Frames;
}
