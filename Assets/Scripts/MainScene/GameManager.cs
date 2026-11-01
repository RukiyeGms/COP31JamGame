using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    CoralSpawn coralSpawn;

    bool isIntroActive;

    public static bool IsIntroActive => instance != null && instance.isIntroActive;

    [SerializeField] GameDatabase gameDatabase;

    List<GameObject> spawnedTrashes = new List<GameObject>();
    List<GameObject> spawnedFishes = new List<GameObject>();

    bool hasStarted;

    bool isGamePlayed;
    public static GameManager instance;
    int trashPicked;
    int fishRescued;

    int totaltrashPicked;
    int totalfishRescued;

    [SerializeField] GameObject fishPrefab;
    [SerializeField] GameObject trashPrefab;

    [SerializeField] int maxTrash = 10;
    int trashonscreen;

    [SerializeField] int maxFish = 2;
    int fishonScreen;
    [SerializeField] float fishonSpawnTime = 5;
    float nextFishSpawnTime;

    [SerializeField] float spawnPosMaxY = 3;
    [SerializeField] float spawnPosMinY = -3;
    [SerializeField] float spawnPosMaxX = 6;
    [SerializeField] float spawnPosMinX = -6;

    public static GameObject playerObject;
    Transform player;

    [SerializeField] LayerMask spawnObstacleMask;
    [SerializeField] float objectCheckRadius = 0.25f;
    [SerializeField] float playerCheckRadius = 2f;
    [SerializeField] int maxSpawnAttempts = 10;

    [SerializeField] float trashSpawnInterval = 0.3f;
    float nextTrashSpawnTime;

    //gnlük sayac
    [SerializeField] float dayDuration = 90f;

    int currentDay = 1;
    float remainingTime;

    int lastCompletedDay = 0;

    int savedDayTrash;
    int savedDayFish;

    int savedTotalTrash;
    int savedTotalFish;

    HashSet<string> savedFishIds = new HashSet<string>();

    public int CurrentDay => currentDay;
    public float RemainingTime => remainingTime;

    public static bool IsGamePlaying => instance != null && instance.isGamePlayed && !instance.isPaused;

    bool isPaused;
    public static bool IsPaused => instance != null && instance.isPaused;

    //

    GameObject menuPanel;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;

        trashPicked = 0;
        fishRescued = 0;
        isGamePlayed = true;
        trashonscreen = 0;

        coralSpawn = GetComponent<CoralSpawn>();
    }

    private void Start()
    {
        player = playerObject.transform;
        coralSpawn.SpawnCorals();

        nextFishSpawnTime = Time.time + fishonSpawnTime;

        remainingTime = dayDuration;

        hasStarted = true;

        LoadCheckpoint();
    }


    private void Update()
    {
        if (!isGamePlayed || isPaused || isIntroActive)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            EndDay();
            return;
        }

        if (FishRescueMinigame.Instance != null &&
            FishRescueMinigame.Instance.IsActive)
            return;

        if (Time.time >= nextTrashSpawnTime)
        {
            SpawnTrash();
            nextTrashSpawnTime = Time.time + trashSpawnInterval;
        }

        if (Time.time >= nextFishSpawnTime)
        {
            SpawnFish();
            nextFishSpawnTime = Time.time + fishonSpawnTime;
        }
    }


    public static void FishRescuedUp()
    {
        instance.fishRescued++;
        instance.trashPicked++;
        instance.totalfishRescued++;
        instance.totaltrashPicked++;
        instance.fishonScreen--;
    }

    public static int AskFishRescued()
    {
        return instance.fishRescued;
    }

    public static void TrashPicked()
    {
        instance.trashPicked++;
        instance.totaltrashPicked++;
        instance.trashonscreen--;
    }

    public static int AskTrashPicked()
    {
        return instance.trashPicked;
    }


    private void SpawnFish()
    {
        if (fishPrefab == null || fishonScreen >= maxFish)
            return;

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector2 spawnPos = GetRandomSpawnPos();

            if (player != null &&
                Vector2.Distance(spawnPos, player.position)
                    < playerCheckRadius)
            {
                continue;
            }

            if (Physics2D.OverlapCircle(
                spawnPos, objectCheckRadius, spawnObstacleMask))
            {
                continue;
            }

            GameObject fish = Instantiate(fishPrefab, spawnPos, Quaternion.identity);
            spawnedFishes.Add(fish);
            fishonScreen++;

            return;
        }
    }



    private void SpawnTrash()
    {
        if (trashonscreen >= maxTrash)
            return;

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector2 spawnPos = GetRandomSpawnPos();

            if (player != null &&
                Vector2.Distance(spawnPos, player.position)
                    < playerCheckRadius)
            {
                continue;
            }
            if (Physics2D.OverlapCircle(
                spawnPos, objectCheckRadius, spawnObstacleMask))
            {
                continue;
            }


            GameObject trash = Instantiate(trashPrefab, spawnPos, Quaternion.identity);
            spawnedTrashes.Add(trash);
            trashonscreen++;

            return;
        }

        
    }


    Vector2 GetRandomSpawnPos()
    {
        float spawnx = Random.Range(spawnPosMinX, spawnPosMaxX);
        float spawny = Random.Range(spawnPosMinY, spawnPosMaxY);

        return new Vector2(spawnx, spawny);
    }

    private void ClearSpawnedObjects()
    {
        foreach (GameObject trash in spawnedTrashes)
        {
            if (trash != null)
            {
                trash.SetActive(false);
                Destroy(trash);
            }
        }

        foreach (GameObject fish in spawnedFishes)
        {
            if (fish != null)
            {
                fish.SetActive(false);
                Destroy(fish);
            }
        }

        spawnedTrashes.Clear();
        spawnedFishes.Clear();

        trashonscreen = 0;
        fishonScreen = 0;
    }

    private void EndDay()
    {
        isGamePlayed = false;


        if (FishRescueMinigame.Instance != null)
        {
            FishRescueMinigame.Instance.CancelRescue();
        }

        foreach (GameObject trash in spawnedTrashes)
        {
            if (trash != null)
            {
                Trash trashScript = trash.GetComponent<Trash>();

                if (trashScript != null)
                    trashScript.CancelPickup();
            }
        }

        SaveCheckpoint();

        if (FeedbackUI.Instance != null)
        {
            FeedbackUI.Instance.Hide();
        }

        EndPanel dayEndUI = FindFirstObjectByType<EndPanel>(  FindObjectsInactive.Include );

        if (dayEndUI != null)
        {
            dayEndUI.Show(currentDay, trashPicked, fishRescued);
        }
        
    }

    public void ReturnToMainMenu()
    {
        if (FishRescueMinigame.Instance != null)
        {
            FishRescueMinigame.Instance.CancelRescue();
        }

        isPaused = false;
        Time.timeScale = 1f;
        isGamePlayed = false;

        LoadCheckpoint(false);

        ClearSpawnedObjects();
        coralSpawn.ClearCorals();

        SceneManager.LoadScene("MenuScene");
    }



    public void NextDay()
    {
        if (isGamePlayed)
            return;

        currentDay++;
        trashPicked = 0;
        fishRescued = 0;

        remainingTime = dayDuration;

        ClearSpawnedObjects();
        coralSpawn.RefreshCorals();

        nextTrashSpawnTime = Time.time + trashSpawnInterval;
        nextFishSpawnTime = Time.time + fishonSpawnTime;

        EndPanel dayEndUI = FindFirstObjectByType<EndPanel>( FindObjectsInactive.Include );

        if (dayEndUI != null)
        {
            dayEndUI.gameObject.SetActive(false);
        }

        isGamePlayed = true;
    }



    public void NewGame()
    {
        if (isGamePlayed)
            return;
        isPaused = false;
        Time.timeScale = 1f;

        if (gameDatabase != null) ///codex reset
        {
            foreach (FishData fish in gameDatabase.fishes)
            {
                if (fish != null)
                {
                    PlayerPrefs.DeleteKey(fish.id);
                }
            }

            PlayerPrefs.Save();
        }

        PlayerPrefs.DeleteKey("Save_HasCheckpoint");
        PlayerPrefs.Save();

        lastCompletedDay = 0;
        savedFishIds.Clear();

        currentDay = 1;

        trashPicked = 0;
        fishRescued = 0;

        totaltrashPicked = 0;
        totalfishRescued = 0;

        remainingTime = dayDuration;

        ClearSpawnedObjects();
        coralSpawn.RefreshCorals();

        nextTrashSpawnTime = Time.time + trashSpawnInterval;
        nextFishSpawnTime = Time.time + fishonSpawnTime;


        EndPanel dayEndUI = FindFirstObjectByType<EndPanel>( FindObjectsInactive.Include);

        if (dayEndUI != null)
        {
            dayEndUI.gameObject.SetActive(false);
        }

        PlayerPrefs.DeleteKey("Save_IntroSeen");

        isGamePlayed = true;

        if (IntroUI.Instance != null)
        {
            IntroUI.Instance.StartIntro();
        }
    }


    private void OnEnable()
    {
        if (instance == this)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (!hasStarted)
            return;

        if (scene.name == "MenuScene")
        {
            player = null;
            playerObject = null;
            return;
        }

        if (scene.name == "MainScene")
        {
            if (playerObject == null)
            {
                return;
            }

            player = playerObject.transform;

            isPaused = false;
            Time.timeScale = 1f;

            bool hasCheckpoint =
                PlayerPrefs.GetInt("Save_HasCheckpoint", 0) == 1;

            LoadCheckpoint();

            if (!hasCheckpoint)
            {
                currentDay = 1;

                trashPicked = 0;
                fishRescued = 0;
                totaltrashPicked = 0;
                totalfishRescued = 0;

                remainingTime = dayDuration;

                nextTrashSpawnTime =
                    Time.time + trashSpawnInterval;

                nextFishSpawnTime =
                    Time.time + fishonSpawnTime;

                coralSpawn.RefreshCorals();

                isGamePlayed = true;
            }
        }
    }


    public void PauseGame()
    {
        if (!isGamePlayed || isPaused)
            return;

        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!isPaused)
            return;

        isPaused = false;
        Time.timeScale = 1f;
    }

    private void SaveCheckpoint()
    {
        lastCompletedDay = currentDay;

        savedDayTrash = trashPicked;
        savedDayFish = fishRescued;

        savedTotalTrash = totaltrashPicked;
        savedTotalFish = totalfishRescued;

        savedFishIds.Clear();

        if (gameDatabase != null)
        {
            foreach (FishData fish in gameDatabase.fishes)
            {
                if (fish != null &&
                    PlayerPrefs.GetInt(fish.id, 0) == 1)
                {
                    savedFishIds.Add(fish.id);
                }
            }
        }


        PlayerPrefs.SetInt("Save_HasCheckpoint", 1);

        PlayerPrefs.SetInt("Save_LastDay", lastCompletedDay);

        PlayerPrefs.SetInt("Save_DayTrash", savedDayTrash);
        PlayerPrefs.SetInt("Save_DayFish", savedDayFish);

        PlayerPrefs.SetInt("Save_TotalTrash", savedTotalTrash);
        PlayerPrefs.SetInt("Save_TotalFish", savedTotalFish);

        if (gameDatabase != null)
        {
            foreach (FishData fish in gameDatabase.fishes)
            {
                if (fish == null)
                    continue;

                PlayerPrefs.SetInt(
                    "Save_Fish_" + fish.id,
                    savedFishIds.Contains(fish.id) ? 1 : 0
                );
            }
        }

        PlayerPrefs.Save();
    }

    private void LoadCheckpoint(bool showResultPanel = true)
    {
        bool hasCheckpoint =
            PlayerPrefs.GetInt("Save_HasCheckpoint", 0) == 1;

        if (gameDatabase != null)
        {
            savedFishIds.Clear();

            foreach (FishData fish in gameDatabase.fishes)
            {
                if (fish == null)
                    continue;

                bool discovered = hasCheckpoint &&
                    PlayerPrefs.GetInt("Save_Fish_" + fish.id, 0) == 1;

                if (discovered)
                {
                    savedFishIds.Add(fish.id);
                    PlayerPrefs.SetInt(fish.id, 1);
                }
                else
                {
                    PlayerPrefs.DeleteKey(fish.id);
                }
            }

            PlayerPrefs.Save();
        }

        if (!hasCheckpoint)
            return;

        lastCompletedDay = PlayerPrefs.GetInt("Save_LastDay", 0);

        savedDayTrash = PlayerPrefs.GetInt("Save_DayTrash", 0);
        savedDayFish = PlayerPrefs.GetInt("Save_DayFish", 0);

        savedTotalTrash = PlayerPrefs.GetInt("Save_TotalTrash", 0);
        savedTotalFish = PlayerPrefs.GetInt("Save_TotalFish", 0);

        currentDay = lastCompletedDay;

        trashPicked = savedDayTrash;
        fishRescued = savedDayFish;

        totaltrashPicked = savedTotalTrash;
        totalfishRescued = savedTotalFish;

        remainingTime = 0;
        isGamePlayed = false;

        if (showResultPanel)
        {
            EndPanel panel = FindFirstObjectByType<EndPanel>(
                FindObjectsInactive.Include
            );

            if (panel != null)
            {
                panel.Show(currentDay, trashPicked, fishRescued);
            }
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            instance = null;
        }
    }
    public void SetIntroActive(bool value)
    {
        isIntroActive = value;
    }

}
