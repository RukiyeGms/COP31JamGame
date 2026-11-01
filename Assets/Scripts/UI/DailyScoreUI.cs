using UnityEngine;
using TMPro;

public class DailyScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text trashScoreText;
    [SerializeField] TMP_Text fishScoreText;

    private void Update()
    {
        if (GameManager.instance == null)
            return;

        trashScoreText.text =
            ": " + GameManager.AskTrashPicked();

        fishScoreText.text =
            ": " + GameManager.AskFishRescued();
    }
}

