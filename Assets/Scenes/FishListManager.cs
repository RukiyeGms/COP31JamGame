using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FishListManager : MonoBehaviour
{
    [Header("UI Bağlantıları")]
    public Transform fishListContainer;
    public GameObject fishCardPrefab;

    [Header("Veritabanı")]
    public GameDatabase gameDatabase; // Veritabanı referansı eklendi

    void Start()
    {
        GenerateFishList();
    }

    void GenerateFishList()
    {
        if (gameDatabase == null) return;

        // gameDatabase içindeki fishes listesini kullanıyoruz
        foreach (var fish in gameDatabase.fishes)
        {
            GameObject newCard = Instantiate(fishCardPrefab, fishListContainer);

            Image fishImage = newCard.transform.Find("FishImage").GetComponent<Image>();
            TMP_Text fishNameText = newCard.transform.Find("FishNameText").GetComponent<TMP_Text>();
            TMP_Text fishInfoText = newCard.transform.Find("FishInfoText").GetComponent<TMP_Text>();

            fishImage.sprite = fish.sprite;

            if (PlayerPrefs.GetInt(fish.id.ToString(), 0) == 1)
            {
                fishImage.color = Color.white;
                fishNameText.text = fish.fishName;
                fishInfoText.text = "Kurtarıldı!";
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