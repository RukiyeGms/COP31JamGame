
using UnityEngine;
using TMPro;

public class EndPanel : MonoBehaviour
{
    [SerializeField] TMP_Text dayText;
    [SerializeField] TMP_Text trashText;
    [SerializeField] TMP_Text fishText;

    [SerializeField] GameObject newGameButton;
    [SerializeField] TMP_Text nextDayButtonText;

    public void Show(int day, int trash, int fish)
    {
        dayText.text = "Day " + day + " Complete!";
        trashText.text = "Trash Collected: " + trash;
        fishText.text = "Fish Rescued: " + fish;

        bool sevenDaysCompleted = day >= 7;

        newGameButton.SetActive(sevenDaysCompleted);

        nextDayButtonText.text =
            sevenDaysCompleted ? "Continue" : "Next Day";

        gameObject.SetActive(true);
    }

    public void OnNextDayClicked()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.NextDay();
        }
    }

    public void OnMainMenuClicked()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.ReturnToMainMenu();
        }
    }

    public void OnNewGameClicked()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.NewGame();
        }
    }
}

