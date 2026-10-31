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
    private FishData GetRandomFish()
    {
        Rarity randomRarity = ChooseRarity();

        if (gameDatabase.fishes.Count == 0)
        {
            return null;
        }
        else
        {
            List<FishData> raritySpesificFishes = new List<FishData>();
            foreach (var fish in gameDatabase.fishes)
            {
                if (fish.rarity == randomRarity)
                {
                    raritySpesificFishes.Add(fish);
                }
            }

            return raritySpesificFishes[Random.Range(0, raritySpesificFishes.Count)];
        }
    }

    private Rarity ChooseRarity()
    {
        int random = Random.Range(0, commonChance + rareChance + legendaryChance);

        if (random <= commonChance)
        {
            return Rarity.Common;
        }
        else if (random <= rareChance)
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
}
