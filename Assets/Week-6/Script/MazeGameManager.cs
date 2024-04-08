using UnityEngine;
using UnityEngine.Events;

public class MazeGameManager : MonoBehaviour
{
    public static readonly UnityEvent resetGame = new();
    public void Restart() => resetGame.Invoke(); //Method to restart the game
}
