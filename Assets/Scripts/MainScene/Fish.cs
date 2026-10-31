using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    FishData fishData;
    TrashData trashData;
    [SerializeField] SpriteRenderer spriteRendererFish;
    //text

    [SerializeField] GameDatabase gameDatabase;
    [SerializeField] int commonChance = 70;
    [SerializeField] int rareChance = 25;
    [SerializeField] int legendaryChance = 5;

    private void Awake()
    {
        fishData = GetRandomFish();
        trashData = GetRandomTrash();
        spriteRendererFish.sprite =  fishData.sprite;
    }

    private void OnMouseDown()
    {
        if (FishRescueMinigame.Instance.IsActive)
            return;

        FishRescueMinigame.Instance.StartRescue(this);
    }


    private FishData GetRandomFish()
    {
        if (gameDatabase == null || gameDatabase.fishes.Count == 0)
        {
            return null;
        }

        Rarity randomRarity = ChooseRarity();

        List<FishData> raritySpecificFishes = new List<FishData>();

        foreach (FishData fish in gameDatabase.fishes)
        {
            if (fish != null && fish.rarity == randomRarity)
            {
                raritySpecificFishes.Add(fish);
            }
        }
        if (raritySpecificFishes.Count == 0)
        {

            foreach (FishData fish in gameDatabase.fishes)
            {
                if (fish != null)
                {
                    raritySpecificFishes.Add(fish);
                }
            }
        }

        if (raritySpecificFishes.Count == 0)
            return null;

        int index = Random.Range(0, raritySpecificFishes.Count);
        return raritySpecificFishes[index];
    }


    private Rarity ChooseRarity()
    {
        int random = Random.Range(
            0, commonChance + rareChance + legendaryChance
        );

        if (random < commonChance)
        {
            return Rarity.Common;
        }
        else if (random < commonChance + rareChance)
        {
            return Rarity.Rare;
        }
        else
        {
            return Rarity.Legendary;
        }
    }

    private TrashData GetRandomTrash()
    {
        if (gameDatabase.trashes.Count == 0) { return null; }
        else
        {
            int randomindex = Random.Range(0, gameDatabase.trashes.Count);

            return gameDatabase.trashes[randomindex];
        }
    }

    public void Rescue()
    {

    }
}
