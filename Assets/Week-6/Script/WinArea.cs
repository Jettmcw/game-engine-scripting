using UnityEngine;
using UnityEngine.Events;

public class WinArea : MonoBehaviour
{
    public static readonly UnityEvent gameWon = new();

    //If the win area collides with a player, invoke that the game has been won
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _)) gameWon.Invoke();
    }
}
