using System;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public static TickManager Instance;

    public Action OnTick;

    public float TickFrequency = 20f;
    float Timer = 0f;
    public float TickDelta { get; private set; }
    float TickCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        TickDelta = 1 / TickFrequency;
    }
    private void Update()
    {
        Timer += Time.deltaTime;

        while(Timer >= TickDelta)
        {
            Timer -= TickDelta;
            Tick();
        }
    }

    void Tick()
    {
        TickCount++;
        OnTick?.Invoke();
    }
}
