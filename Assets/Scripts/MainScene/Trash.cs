using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] GameDatabase gameDatabase;
    TrashData trashData;
    SpriteRenderer spriteRenderer;
    bool canbepicked;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        trashData = GetRandomTrash();
        spriteRenderer.sprite = trashData.sprite;
        canbepicked = false;
    }
    private void OnMouseDown()
    {
        //PlayerPrefs.SetInt(trashData.id, 1);
        Pickup();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canbepicked=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canbepicked = false;
        }
    }

    private void Pickup()
    {
        if (!canbepicked) {return; }

        GameManager.TrashPicked();
        Destroy(gameObject);
    }

    private TrashData GetRandomTrash()
    {
        if (gameDatabase.trashes.Count == 0) { return null; }
        else 
        {
            int randomindex = Random.Range(0, gameDatabase.trashes.Count);

            return gameDatabase.trashes[randomindex];
        }
    }




}
