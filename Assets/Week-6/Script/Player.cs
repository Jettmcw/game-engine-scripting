using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;

    PlayerContollerMappings mappings;
    Rigidbody rb;

    [SerializeField] float jumpForce;

    const float SPEED = 5.5f;

    private void Awake()
    {
        mappings = new PlayerContollerMappings();
        rb = GetComponent<Rigidbody>();
        jumpAction = mappings.Player.Jump;
        moveAction = mappings.Player.Move;
    }

    private void OnEnable()
    {
        moveAction.Enable(); 
        jumpAction.Enable();
        jumpAction.performed += Jump;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        jumpAction.performed -= Jump;
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>() * SPEED;
        rb.velocity = new Vector3(input.x, rb.velocity.y, input.y);
    }

    bool IsGrounded() => Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.1f, 8);

    void Jump(CallbackContext context)
    {
        if (IsGrounded()) rb.AddForce(Vector3.up * jumpForce);
    }
}
