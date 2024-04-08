using UnityEngine;
using System.Linq;
using static UnityEngine.InputSystem.InputAction;
using static UnityEngine.Physics;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public UnityEvent death;

    const float interactionDistance = 3;
    const float jumpForce = 200;
    const float SPEED = 5.5f;

    PlayerContollerMappings mappings;
    Rigidbody rb;
    Vector3 startPosition;

    int keys = 0;
    int health = 100;

    private void Awake()
    {
        Key.keyCollected.AddListener(CollectKey);
        Trap.trapTriggered.AddListener(TakeDamage);
        MazeGameManager.resetGame.AddListener(Restart);
        WinArea.gameWon.AddListener(GoInactive);

        startPosition = transform.position;

        mappings = new();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        mappings.Enable();
        mappings.Player.Jump.performed += Jump;
        mappings.Player.OpenDoor.performed += OpenDoor;
    }

    private void OnDisable()
    {
        mappings.Disable();
        mappings.Player.Jump.performed -= Jump;
        mappings.Player.OpenDoor.performed -= OpenDoor;
    }

    void Update()
    {
        Vector2 input = mappings.Player.Move.ReadValue<Vector2>() * SPEED;
        rb.velocity = new Vector3(input.x, rb.velocity.y, input.y);
    }

    //Called when the player presses space to attempt to jump
    void Jump(CallbackContext context)
    {
        bool grounded = Raycast(transform.position, transform.TransformDirection(Vector3.down), 1.1f, 8); //Check if touching the ground
        if (grounded) rb.AddForce(Vector3.up * jumpForce); //Direct upwards force onto the player if they're on the ground
    }

    void CollectKey() => keys++;

    //Called when the player presses "o" to attempt to open a door
    void OpenDoor(CallbackContext context)
    {
        if (keys == 0) return; //If player doesn't have any keys, they can't open any doors

        //Find the nearest door
        Door[] allDoors = FindObjectsByType<Door>(FindObjectsSortMode.None);
        var openDoors = allDoors.Where(door => !door.opened);
        var nearestOpenDoor = openDoors.OrderBy(Dist).First();

        if (Dist(nearestOpenDoor) > interactionDistance) return; //If the nearest door is too far, player can't open it

        keys--; //Take a key from the player

        nearestOpenDoor.GetComponent<Door>().Open(); //Open the door
    }

    private void TakeDamage()
    {
        health -= 5;
        if (health <= 0) //If health drops below zero...
        {
            GetComponent<MeshRenderer>().enabled = false; //Hide the player
            GoInactive();
            death.Invoke(); //Invoke that the player has died
        }
    }

    private void GoInactive() => GetComponent<Player>().enabled = false; //Prevent the player from moving

    //Determines the distrance between the player and an object
    private float Dist(Component other) => Vector3.Distance(transform.position, other.transform.position);

    //Restart all variables and make the player visible & movable again
    private void Restart()
    {
        health = 100;
        keys = 0;
        transform.position = startPosition;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Player>().enabled = true;
    }
}
