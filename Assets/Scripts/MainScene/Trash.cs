using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] GameDatabase gameDatabase;
    TrashData trashData;
    SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        trashData = GetRandomTrash();
        spriteRenderer.sprite = trashData.sprite;
    }
    private void OnMouseDown()
    {
        //PlayerPrefs.SetInt(trashData.id, 1);
        Pickup();
    }

    private void Pickup()
    {
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
