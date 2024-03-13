using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.Physics;

public class Player : MonoBehaviour
{
    InputAction moveAction;
    InputAction jumpAction;
    InputAction openAction;
    Rigidbody rb;

    [SerializeField] TextMeshProUGUI HealthUI;
    [SerializeField] TextMeshProUGUI CoinsUI;

    const float interactionDistance = 3;
    const float jumpForce = 200;
    const float SPEED = 5.5f;

    int health = 100;
    int coins = 0;
    int keys = 0;

    private void Awake()
    {
        PlayerContollerMappings mappings = new();
        rb = GetComponent<Rigidbody>();
        jumpAction = mappings.Player.Jump;
        moveAction = mappings.Player.Move;
        openAction = mappings.Player.OpenDoor;
    }

    private void OnEnable()
    {
        moveAction.Enable(); 
        jumpAction.Enable();
        openAction.Enable();
        jumpAction.performed += Jump;
        openAction.performed += Door;
    }

    private void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        openAction.Disable();
        jumpAction.performed -= Jump;
        openAction.performed -= Door;
    }

    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>() * SPEED;
        rb.velocity = new Vector3(input.x, rb.velocity.y, input.y);
    }

    //Upon colliding with an object, do the right key/trap/coin interaction
    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Key":
                keys++; //Give the player a key
                collision.gameObject.SetActive(false); //Make the key disappear
                break;
            case "Trap":
                health -= 5; //Detracts from the player's health
                HealthUI.text = $"Health: {health}%"; //Update the UI
                break;
            case "Coin":
                coins++; //Give the player a coin
                CoinsUI.text = $"Coins: {coins}"; //Update the UI
                collision.gameObject.SetActive(false); //Make the coin disappear
                break;
        }
    }

    //Called when the player presses space to attempt to jump
    void Jump(CallbackContext context)
    {
        bool grounded = Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.1f, 8); //Check if touching the ground
        if (grounded) rb.AddForce(Vector3.up * jumpForce); //Direct upwards force onto the player if they're on the ground
    }

    //Called when the player presses "o" to attempt to open a door
    void Door(CallbackContext context)
    {
        if (keys == 0) return; //If player doesn't have any keys, they can't open any doors

        //Find the nearest door
        GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door");
        GameObject nearestDoor = allDoors.OrderBy(Dist).First();

        if (Dist(nearestDoor) > interactionDistance) return; //If the nearest door is too far, player can't open it

        keys--; //Take a key from the player
        nearestDoor.tag = "Untagged"; //Remove the "door" tag so it isn't opened again

        //Rotate the door 90 degrees around its hinge
        Vector3 hinge = nearestDoor.transform.Find("Hinge").position;
        nearestDoor.transform.RotateAround(hinge, Vector3.down, 90);
    }

    //Determins the distrance between the player and an object
    float Dist(GameObject other) => Vector3.Distance(transform.position, other.transform.position);
}
