using UnityEngine;
using UnityEngine.Events;

public class Trap : MonoBehaviour
{
    public static readonly UnityEvent trapTriggered = new();

    private void Awake() => MazeGameManager.resetGame.AddListener(Restart);

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Player _))
        {
            trapTriggered.Invoke(); //Invoke that it's been collided with
            gameObject.SetActive(false); //Make it dissapear
        }
    }

    private void Restart() => gameObject.SetActive(true); //Come back when the game restarts
}
