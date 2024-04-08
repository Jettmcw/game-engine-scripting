using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    private int coins = 0;

    private void Awake()
    {
        Coin.coinCollected.AddListener(Increment);
        MazeGameManager.resetGame.AddListener(Restart);
    }

    private void Increment()
    {
        coins++;
        UpdateText();
    }

    private void Restart()
    {
        coins = 0;
        UpdateText();
    }

    private void UpdateText() => GetComponent<TextMeshProUGUI>().text = $"Coins: {coins}"; //Update the text with the new coin values
}
