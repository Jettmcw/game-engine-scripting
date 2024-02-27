using UnityEngine;
using UnityEngine.UI;

public class Flower : MonoBehaviour
{
    private const int NectarCooldown = 1000; //How many frames pass before the flower replenishes its nectar
    private int countdown = NectarCooldown; //Keeps track of the countdown for nectar

    public Color nectarColor, emptyColor; //Holds the two colors for when there is and isn't nectar

    //Updates the flower's appearance and nectar countdown every frame
    void Update()
    {
        //Give the flower the empty color & decrement the countdown until it hits zero
        if (countdown > 0)
        {
            GetComponent<Image>().color = emptyColor;
            countdown--;
        }
        //Give the flower the nectar color when the countdown hits zero
        else
        {
            GetComponent<Image>().color = nectarColor;
        }
    }

    //Lets the bee attempt to take nectar
    public bool GetNectar()
    {
        if (countdown > 0) return false; //If it doesn't have nectar, inform the bee
        countdown = NectarCooldown; //If the nectar is taken, reset the countdown
        return true; //Inform the bee that it took the nectar
    }
}
