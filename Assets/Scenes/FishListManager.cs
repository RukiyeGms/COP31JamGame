using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class FishListManager : MonoBehaviour
{
    [Header("UI Elemanları")]
    public Transform commonListContent;   
    public Transform rareListContent;     
    public Transform legendaryListContent; 

    public GameObject fishCardPrefab;     

    void Start()
    {
        GenerateFishList();
    }

    void GenerateFishList()
    {
        foreach (var fish in GameDatabase.fishes)
        {
            Transform targetContent = null;

            if (fish.rarity == Rarity.Common)
                targetContent = commonListContent;
            else if (fish.rarity == Rarity.Rare)
                targetContent = rareListContent;
            else if (fish.rarity == Rarity.Legendary)
                targetContent = legendaryListContent;

            if (targetContent == null) continue;

            GameObject newCard = Instantiate(fishCardPrefab, targetContent);

            Image fishImage = newCard.transform.Find("FishImage").GetComponent<Image>();
            TMP_Text fishNameText = newCard.transform.Find("FishNameText").GetComponent<TMP_Text>();
            TMP_Text fishInfoText = newCard.transform.Find("FishInfoText").GetComponent<TMP_Text>();

            fishImage.sprite = fish.icon;

            if (PlayerPrefs.GetInt(fish.id.ToString(), 0) == 1)
            {
    
                fishImage.color = Color.white; 
                fishNameText.text = fish.fishName;
                fishInfoText.text = fish.description;
            }
            else
            {

                fishImage.color = Color.black;
                fishNameText.text = "???";
                fishInfoText.text = "???";
            }
        }
    }
}