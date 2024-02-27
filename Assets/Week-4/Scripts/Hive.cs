using UnityEngine;

public class Hive : MonoBehaviour
{
    private const int BeesPerHive = 2;
    private const int HoneyCooldown = 500; //How many frames pass before nectar can be converted into honey
    private int countdown = HoneyCooldown; //Keeps track of the countdown for nectar conversion

    public int Nectar, Honey;

    [SerializeField] GameObject beePrefab;

    //Before anything else, makes multiple bees depending on BeesPerHive
    void Start()
    {
        for (int i = 0; i < BeesPerHive; i++)
        {
            var bee = Instantiate(beePrefab, transform); //Instantiate a bee prefab with this hive as the parent
            bee.GetComponent<Bee>().CheckAnyFlower(); //Gets its Bee object and makes it go nectar-searching
        }
    }

    //Updates the nectar/honey situation every frame
    void Update()
    {
        if (Nectar == 0) return; //If the hive has no nectar, nothing happens

        //Decrement the countdown until it hits zero
        if (countdown > 0)
        {
            countdown--;
        }
        //When the countdown hits zero, decrement nectar and increment honey, and restart the countdown
        else
        {
            Nectar--;
            Honey++;
            countdown = HoneyCooldown;
        }
    }
    
    //Increments the amount of Nectar when a bee delivers it
    public void GiveNectar() => Nectar++;
}
