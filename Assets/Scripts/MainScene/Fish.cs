using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    FishData fishData;
    [SerializeField] SpriteRenderer spriteRendererFish;
    [SerializeField] Sprite DAFISH;
    //text

    [SerializeField] GameDatabase gameDatabase;
    [SerializeField] int commonChance = 70;
    [SerializeField] int rareChance = 25;
    [SerializeField] int legendaryChance = 5;

    [SerializeField] float rescueDistance = 2f;

    private void Awake()
    {
        fishData = GetRandomFish();
        spriteRendererFish.sprite =  DAFISH;
    }

    private void OnMouseDown()
    {
        if (!GameManager.IsGamePlaying)
            return;

        if (FishRescueMinigame.Instance == null || FishRescueMinigame.Instance.IsActive)
            return;

        if (GameManager.playerObject == null)
            return;

        float distance = Vector2.Distance(transform.position, GameManager.playerObject.transform.position);

        if (distance > rescueDistance)
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

    public void Rescue()
    {
        PlayerPrefs.SetInt(fishData.id, 1);

        PlayerPrefs.Save();

        GameManager.FishRescuedUp();

        Destroy(gameObject);
    }
}
