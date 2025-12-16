using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent (typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        TickManager.Instance.OnTick += HandleTick;
    }

    private void OnDisable()
    {
        TickManager.Instance.OnTick -= HandleTick;
    }
    void Update()
    {
        Vector2 input = ReadInput();
        if (input == Vector2.zero) return;
        movement.LocalMove(input);
    }

    void HandleTick()
    {
    }

    Vector2 ReadInput()
    {
        float x =
            (Keyboard.current.aKey.isPressed ? -1 : 0) +
            (Keyboard.current.dKey.isPressed ? 1 : 0);

        float y =
            (Keyboard.current.sKey.isPressed ? -1 : 0) +
            (Keyboard.current.wKey.isPressed ? 1 : 0);

        return new Vector2(x, y).normalized;
    }
}
