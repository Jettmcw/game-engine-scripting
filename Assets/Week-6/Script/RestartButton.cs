using UnityEngine;

public class RestartButton : MonoBehaviour
{
    private void Awake()
    {
        MazeGameManager.resetGame.AddListener(Restart); //Restarting the game should hide the button
        WinArea.gameWon.AddListener(Show); //Winning the game makes the button show
        gameObject.SetActive(false); //Hide reset button at the start
    }

    public void Show() => gameObject.SetActive(true);
    private void Restart() => gameObject.SetActive(false);
}
