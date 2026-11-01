using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    CoralSpawn coralSpawn;

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

    public int CurrentDay => currentDay;
    public float RemainingTime => remainingTime;

    public static bool IsGamePlaying => instance != null && instance.isGamePlayed;

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
    }


    private void Update()
    {
        if (!isGamePlayed)
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


    void GameStart()
    {
        isGamePlayed=true;
    }

    void GameStop()
    {
        isGamePlayed=false;
        Time.timeScale = 0;
    }
    void GameEnd()
    {
        isGamePlayed=false;//an more
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

        EndPanel dayEndUI = FindFirstObjectByType<EndPanel>(  FindObjectsInactive.Include );

        if (dayEndUI != null)
        {
            dayEndUI.Show(currentDay, trashPicked, fishRescued);
        }
        else
        {
            Debug.LogWarning("DayEndUI not found!");
        }
    }


    public void ReturnToMainMenu()
    {
        isGamePlayed = false;
        Time.timeScale = 1f;

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

        Debug.Log("Day " + currentDay + " started!");
    }



    public void NewGame()
    {
        if (isGamePlayed)
            return;

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

        isGamePlayed = true;

        Debug.Log("New Game Started!");
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
                Debug.LogError("Player not found!");
                return;
            }

            player = playerObject.transform;

            if (!isGamePlayed)
            {
                EndPanel panel =FindFirstObjectByType<EndPanel>(FindObjectsInactive.Include );

                if (panel != null)
                {
                    panel.Show(currentDay, trashPicked, fishRescued);
                }
            }
        }
    }



}
