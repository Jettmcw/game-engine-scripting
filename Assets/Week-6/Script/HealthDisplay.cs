using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    private int health = 100;

    private void Awake()
    {
        Trap.trapTriggered.AddListener(RegisterDamage);
        MazeGameManager.resetGame.AddListener(Restart);
    }

    private void RegisterDamage()
    {
        health -= 5;
        UpdateText();
    }

    private void Restart()
    {
        health = 100;
        UpdateText();
    }

    private void UpdateText() => GetComponent<TextMeshProUGUI>().text = $"Health: {health}%"; //Update the text with the new health value
}
