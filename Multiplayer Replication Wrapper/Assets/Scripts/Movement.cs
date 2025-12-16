using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public float Speed = 1f;

    const int MaxBufferedInputs = 64;
    List<InputFrame> inputBuffer = new();
    int Sequence = 0;

    private void OnEnable()
    {
        TickManager.Instance.OnTick += SendInputs;
    }
    private void OnDisable()
    {
        TickManager.Instance.OnTick -= SendInputs;
    }
    public void LocalMove(Vector2 move)
    {
        var frame = new InputFrame()
        {
            Sequence = ++Sequence,
            Move = move
        };
        if (inputBuffer.Count > MaxBufferedInputs)
            inputBuffer.RemoveAt(0);
        inputBuffer.Add(frame);
        Simulate(frame);
    }
    public void Simulate(InputFrame input)
    {
        Vector3 dir = new Vector3(input.Move.x, 0f, input.Move.y);

        Vector3 velocity = dir * Speed;

        rb.position += velocity * TickManager.Instance.TickDelta;
    }
    void SendInputs()
    {
        if (inputBuffer.Count > 0) return;

        print("sending inputs");
        InputPacket packet = new InputPacket()
        {
            PlayerID = NetworkManager.Instance.PlayerID,
            Frames = inputBuffer
        };

        byte[] data = MessageFactory.CreateMessage<InputPacket>("InputPacket", packet);
        NetworkManager.Instance.Send(data);
    }
    public void Reconciliate(Vector3 Position,int LastSequence)
    {
        rb.position = Position;

        inputBuffer.RemoveAll(f => f.Sequence <= LastSequence);

        foreach (var frame in inputBuffer)
        {
            Simulate(frame);
        }
    }

}

[Serializable]
public class InputFrame
{
    public int Sequence;
    public Vector2 Move;
}
