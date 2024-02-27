using System;
using DG.Tweening;
using UnityEngine;
using static UnityEngine.Random;

public class Bee : MonoBehaviour
{
    //Makes the bee fly to a random flower & check if it has nectar
    public void CheckAnyFlower()
    {
        Flower[] flowers = FindObjectsByType<Flower>(FindObjectsSortMode.None); //Gets an array of all the flowers in the scene
        Flower flower = flowers[Range(0, flowers.Length)]; //Selects a random flower from the array
        Move(flower, CheckFlower); //Moves to the random flower, and checks that flower
    }

    //Makes the bee fly to a component in 1 second, and upon arrival, it performs a method using that component
    private void Move(Component target, Action<Component> action)
    {
        transform.DOMove(target.transform.position, 1f).OnComplete(() => action(target)).SetEase(Ease.Linear);
    }

    //Makes the bee try to take nectar from a flower, either returning to its hive or flying to another flower after
    private void CheckFlower(Component flower)
    {
        bool hadNectar = (flower as Flower).GetNectar(); //Takes the nectar from the flower if possible, records if it had nectar
        if (hadNectar) Move(transform.parent, DeliverNectar); //If it had nectar, the bee returns to its hive (its parent object), and delivers nectar
        else CheckAnyFlower(); //If it didn't have nectar, the bee flies to a separate flower
    }

    //Makes the bee return to its hive and gives it nectar
    private void DeliverNectar(Component hive)
    {
        hive.GetComponent<Hive>().GiveNectar(); //Gets the Hive object from the component and gives it nectar
        CheckAnyFlower(); //Goes back to flying around to random flowers
    }
}
