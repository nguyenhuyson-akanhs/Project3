using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingController : MonoBehaviour
{
    private Rigidbody2D RB2D;
    public float MoveSpeed;
    private void Awake()
    {
        RB2D = gameObject.GetComponent<Rigidbody2D>();
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            RB2D.linearVelocity = Vector3.zero;
        }
        else if (context.performed)
        {
            RB2D.linearVelocity = context.ReadValue<Vector2>().normalized * MoveSpeed;
        }
    }
}
// TODO: learn inputaction for entity