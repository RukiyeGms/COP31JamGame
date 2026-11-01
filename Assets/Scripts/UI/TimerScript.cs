using UnityEngine;
using TMPro;
public class TimerScript : MonoBehaviour
{
    TMP_Text timerText;
    [SerializeField] TMP_Text dayText;

    private void Awake()
    {
        timerText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (GameManager.instance == null)
            return;

        int remainingSeconds = Mathf.CeilToInt(
            GameManager.instance.RemainingTime
        );

        int minutes = remainingSeconds / 60;
        int seconds = remainingSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";

        if (dayText == null)
        {
            return;
        }

        dayText.text = "Day " + GameManager.instance.CurrentDay;
    }
}
