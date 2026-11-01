
using UnityEngine;
using UnityEngine.UI;

public class FishRescueMinigame : MonoBehaviour
{
    public static FishRescueMinigame Instance;

    [SerializeField] GameObject rescueBarObject;
    [SerializeField] Slider rescueBar;

    [SerializeField] RectTransform rescueCanvas;
    [SerializeField] Vector3 barOffset = new Vector3(1.2f, 1f, 0f);

    [SerializeField] float fillPerClick = 0.18f;
    [SerializeField] float decreasePerSecond = 0.15f;
    [SerializeField] float rescueDuration = 6f;

    Fish currentFish;
    Player currentPlayer;

    float progress;
    float remainingTime;
    int startFrame;

    public bool IsActive { get; private set; }

    private void Awake()
    {
        Instance = this;

        rescueBar.minValue = 0;
        rescueBar.maxValue = 1;
        rescueBar.value = 0;

        rescueBarObject.SetActive(false);
    }


    public void StartRescue(Fish fish)
    {
        if (IsActive || fish == null ||
            GameManager.playerObject == null)
            return;

        Player player =
            GameManager.playerObject.GetComponent<Player>();

        if (player == null || !player.StartSearch())
            return;

        currentPlayer = player;
        currentFish = fish;

        progress = 0;
        remainingTime = rescueDuration;
        startFrame = Time.frameCount;

        rescueBar.value = 0;

        UpdateBarPosition();
        rescueBarObject.SetActive(true);

        IsActive = true;
    }


    private void Update()
    {
        if (!IsActive || GameManager.IsPaused)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            EndRescue(false);
            return;
        }

        // Bar slowly decreases
        progress -= decreasePerSecond * Time.deltaTime;
        progress = Mathf.Max(progress, 0f);

        // Left mouse click increases progress
        if (Time.frameCount != startFrame &&
            Input.GetMouseButtonDown(0))
        {
            progress += fillPerClick;
            progress = Mathf.Min(progress, 1f);
        }

        rescueBar.value = progress;

        if (progress >= 1f)
        {
            EndRescue(true);
        }
    }



    private void EndRescue(bool success)
    {
        Fish fish = currentFish;

        IsActive = false;
        currentFish = null;
        rescueBarObject.SetActive(false);

        if (currentPlayer != null)
        {
            if (success)
            {
                currentPlayer.EndSearch();
            }
            else
            {
                currentPlayer.CancelSearch();
            }
        }

        currentPlayer = null;

        if (success && fish != null)
        {
            fish.Rescue();
        }
        else
        {
            Debug.Log("Rescue failed!");
        }
    }



    private void LateUpdate()
    {
        if (!IsActive)
            return;

        UpdateBarPosition();
    }

    private void UpdateBarPosition()
    {
        if (GameManager.playerObject == null || rescueCanvas == null)
            return;

        rescueCanvas.position =
            GameManager.playerObject.transform.position + barOffset;
    }

    public void CancelRescue()
    {
        if (IsActive)
        {
            EndRescue(false);
        }
    }

}
