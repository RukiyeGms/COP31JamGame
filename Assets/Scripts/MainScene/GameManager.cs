using UnityEngine;

public class GameManager : MonoBehaviour
{
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
        isGamePlayed = false;
        trashonscreen = 0;
    }

    private void Update()
    {
        if ( isGamePlayed)
        {
            
        }
    }

    void GameStart()
    {

    }

    public void FishRescuedUp()
    {
        fishRescued++;
        trashPicked++;
        totalfishRescued++;
        totaltrashPicked++;
    }

    public int AskFishRescued()
    {
        return fishRescued;
    }

    public void TrashPicked()
    {
        trashPicked++;
        totaltrashPicked++;
        trashonscreen--;
    }

    public int AskTrashPicked()
    {
        return trashPicked;
    }

    private void SpawnFish()
    {

    }

    private void SpawnTrash()
    {
        if (trashonscreen < maxTrash)
        {
            trashonscreen++;
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
