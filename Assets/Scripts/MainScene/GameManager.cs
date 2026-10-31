using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool isGamePlayed;
    public static GameManager instance;
    int trashPicked;
    int fishRescued;

    int totaltrashPicked;
    int totalfishRescued;

    [SerializeField] int maxTrash = 10;
    int trashonscreen;

    [SerializeField] float spawnPosMaxY = 3;
    [SerializeField] float spawnPosMinY = -3;
    [SerializeField] float spawnPosMaxX = 6;
    [SerializeField] float spawnPosMinX = -6;
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

    public void FishRescuedUp()
    {
        fishRescued++;
        trashPicked++;
    }

    public int AskFishRescued()
    {
        return fishRescued;
    }

    public void TrashPicked()
    {
        trashPicked++;
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
        }
    }

    Vector2 GetRandomSpawnPos()
    {
        float spawnx = Random.Range(spawnPosMinX, spawnPosMaxX);
        float spawny = Random.Range(spawnPosMinY, spawnPosMaxY);

        return new Vector2(spawnx, spawny);
    }
        
}
