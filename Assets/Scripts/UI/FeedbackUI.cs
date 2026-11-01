
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedbackUI : MonoBehaviour
{
    public static FeedbackUI Instance { get; private set; }

    [SerializeField] RectTransform feedback;
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] Image fishIcon;
    [SerializeField] CanvasGroup canvasGroup;

    [SerializeField] Vector3 playerOffset = new Vector3(0, 1.5f, 0);
    [SerializeField] float duration = 1.2f;
    [SerializeField] float riseDistance = 50f;

    float remainingTime;
    Vector2 startingPosition;

    private void Awake()
    {
        Instance = this;

        startingPosition = feedback.anchoredPosition;
        feedback.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!feedback.gameObject.activeSelf || GameManager.IsPaused)
            return;

        if (GameManager.playerObject != null)
        {
            transform.position =
                GameManager.playerObject.transform.position + playerOffset;
        }

        remainingTime -= Time.deltaTime;

        float progress = 1f -
            Mathf.Clamp01(remainingTime / duration);

        feedback.anchoredPosition =
            startingPosition + Vector2.up * riseDistance * progress;

        canvasGroup.alpha = 1f - progress;

        if (remainingTime <= 0)
        {
            feedback.gameObject.SetActive(false);
        }
    }

    public void ShowTrash()
    {
        Show("+1 Trash", null);
    }

    public void ShowFish(Sprite fishSprite)
    {
        Show("Rescued!", fishSprite);
    }

    private void Show(string message, Sprite sprite)
    {

        feedbackText.text = message;

        fishIcon.gameObject.SetActive(sprite != null);

        if (sprite != null)
            fishIcon.sprite = sprite;

        feedback.anchoredPosition = startingPosition;
        canvasGroup.alpha = 1f;
        remainingTime = duration;

        feedback.gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void LateUpdate()
    {
        if (!feedback.gameObject.activeSelf)
            return;

        if (GameManager.playerObject != null)
        {
            transform.position =
                GameManager.playerObject.transform.position + playerOffset;
        }
    }

    public void Hide()
    {
        feedback.gameObject.SetActive(false);
    }
}

