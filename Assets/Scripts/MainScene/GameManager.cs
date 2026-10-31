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

    [SerializeField] float spawnPosMaxY = 3;
    [SerializeField] float spawnPosMinY = -3;
    [SerializeField] float spawnPosMaxX = 6;
    [SerializeField] float spawnPosMinX = -6;

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

    private void Update()
    {
        if ( isGamePlayed)
        {
            SpawnTrash();
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

    }

    private void SpawnTrash()
    {
        if (trashonscreen < maxTrash)
        {
            trashonscreen++;
            Vector2 spawnPos = GetRandomSpawnPos();

            //check if there are nearby trash and if there is spawn away

            Instantiate(trashPrefab, GetRandomSpawnPos() ,Quaternion.identity);

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
