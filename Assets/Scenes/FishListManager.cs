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
    public GameDatabase gameDatabase;

    [Header("Detay Paneli (Pop-Up)")]
    public GameObject detailPanel;      // Açılacak olan detay penceresi
    public Image detailFishImage;       // Penceredeki büyük balık resmi
    public TMP_Text detailFishNameText; // Penceredeki balık adı
    public TMP_Text detailFishInfoText; // Penceredeki detaylı bilgi metni
    public Button closeDetailButton;    // Kapatma butonu

    void Start()
    {
        // Başlangıçta detay panelini gizle
        if (detailPanel != null)
            detailPanel.SetActive(false);

        // Kapat butonuna tıklama dinleyicisi ekle
        if (closeDetailButton != null)
            closeDetailButton.onClick.AddListener(CloseDetailPanel);

        GenerateFishList();
    }

    void GenerateFishList()
    {
        if (gameDatabase == null) return;

        foreach (var fish in gameDatabase.fishes)
        {
            GameObject newCard = Instantiate(fishCardPrefab, fishListContainer);

            Image fishImage = newCard.transform.Find("FishImage").GetComponent<Image>();
            TMP_Text fishNameText = newCard.transform.Find("FishNameText").GetComponent<TMP_Text>();
            TMP_Text fishInfoText = newCard.transform.Find("FishInfoText").GetComponent<TMP_Text>();
            Button cardButton = newCard.GetComponent<Button>(); // Karttaki Button bileşeni

            fishImage.sprite = fish.sprite;

            bool isUnlocked = PlayerPrefs.GetInt(fish.id.ToString(), 0) == 1;

            if (isUnlocked)
            {
                // Keşfedilmiş Balık
                fishImage.color = Color.white;
                fishNameText.text = fish.fishName;
                fishInfoText.text = "Keşfedildi!";

                // Karta tıklama özelliği ekle
                if (cardButton != null)
                {
                    cardButton.interactable = true;
                    // Tıklanınca ilgili balık verisiyle detay panelini aç
                    cardButton.onClick.AddListener(() => OpenDetailPanel(fish));
                }
            }
            else
            {
                // Keşfedilmemiş Balık
                fishImage.color = Color.black;
                fishNameText.text = "???";
                fishInfoText.text = "???";

                if (cardButton != null)
                {
                    cardButton.interactable = false; // Kilitliyse tıklanamaz
                }
            }
        }
    }

    // Detay panelini açan fonksiyon
    public void OpenDetailPanel(FishData fish)
    {
        if (detailPanel == null) return;

        detailFishImage.sprite = fish.sprite;
        detailFishNameText.text = fish.fishName;

        // Eğer FishData içinde detay metni değişkeni (ör. details/description) varsa onu bağla:
        detailFishInfoText.text = "Bu balık hakkında detaylı bilgi ve hikaye buraya gelecek.";

        detailPanel.SetActive(true);
    }

    // Detay panelini kapatan fonksiyon
    public void CloseDetailPanel()
    {
        if (detailPanel != null)
            detailPanel.SetActive(false);
    }
}