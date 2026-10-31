using UnityEngine;

public class GameManager : MonoBehaviour
{
    CoralSpawn coralSpawn;

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

    GameObject menuPanel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else { Destroy(gameObject); }

        trashPicked = 0;
        fishRescued = 0;
        isGamePlayed = true; //false later
        trashonscreen = 0;
        coralSpawn = GetComponent<CoralSpawn>();
    }

    private void Start()
    {
        player = playerObject.transform;
        coralSpawn.SpawnCorals();

        nextFishSpawnTime = Time.time + fishonSpawnTime;
    }

    private void Update()
    {
        if (!isGamePlayed)
            return;

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

            Instantiate(fishPrefab, spawnPos, Quaternion.identity);

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

            
            Instantiate(trashPrefab, spawnPos, Quaternion.identity);
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

    



    /// menu and panel thingy
        
}
