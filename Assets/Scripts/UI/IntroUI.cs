using UnityEngine;

public class IntroUI : MonoBehaviour
{
    public static IntroUI Instance { get; private set; }

    [SerializeField] GameObject comic1Panel;
    [SerializeField] GameObject comic2Panel;
    [SerializeField] GameObject howToPlayPanel;

    bool introActive;
    bool openedFromPause;
    int comicStep;

    public bool IsShowing => introActive || openedFromPause;

    private void Awake()
    {
        Instance = this;

        comic1Panel.SetActive(false);
        comic2Panel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    private void Start()
    {
        bool hasCheckpoint =
            PlayerPrefs.GetInt("Save_HasCheckpoint", 0) == 1;

        bool introSeen =
            PlayerPrefs.GetInt("Save_IntroSeen", 0) == 1;

        if (!hasCheckpoint && !introSeen &&
            GameManager.instance.CurrentDay == 1)
        {
            StartIntro();
        }
    }

    public void StartIntro()
    {
        introActive = true;
        openedFromPause = false;
        comicStep = 0;

        GameManager.instance.SetIntroActive(true);
        Time.timeScale = 0f;

        comic1Panel.SetActive(true);
        comic2Panel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    public void NextComic()
    {
        if (!introActive)
            return;

        if (comicStep == 0)
        {
            comic1Panel.SetActive(false);
            comic2Panel.SetActive(true);
            comicStep = 1;
        }
        else
        {
            comic2Panel.SetActive(false);
            howToPlayPanel.SetActive(true);
        }
    }

    public void OpenHowToFromPause()
    {
        if (!GameManager.IsPaused)
            return;

        openedFromPause = true;
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);

        if (openedFromPause)
        {
            openedFromPause = false;
            return;
        }

        if (introActive)
        {
            introActive = false;

            PlayerPrefs.SetInt("Save_IntroSeen", 1);
            PlayerPrefs.Save();

            GameManager.instance.SetIntroActive(false);
            Time.timeScale = 1f;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}

