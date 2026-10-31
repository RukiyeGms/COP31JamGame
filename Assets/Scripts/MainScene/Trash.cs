using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] GameDatabase gameDatabase;
    TrashData trashData;
    Animator animator;
    bool canbepicked;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        trashData = GetRandomTrash();
        animator.SetBool(trashData.animName, true);
        canbepicked = false;
    }
    private void OnMouseDown()
    {
        
        Pickup();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canbepicked=true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canbepicked = true;
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
