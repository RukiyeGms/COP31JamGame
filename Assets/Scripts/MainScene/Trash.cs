using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] GameDatabase gameDatabase;
    TrashData trashData;
    Animator animator;
    [SerializeField] float pickupDistance = 2f;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        trashData = GetRandomTrash();
        animator.SetBool(trashData.animName, true);
    }
    private void OnMouseDown()
    {
        Debug.Log("Trash OnMouseDown called!");

        if (FishRescueMinigame.Instance != null &&
            FishRescueMinigame.Instance.IsActive)
            return;

        if (GameManager.playerObject == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            GameManager.playerObject.transform.position
        );

        Debug.Log("Distance to player: " + distance);

        if (distance > pickupDistance)
            return;

        Debug.Log("Trash picked successfully!");
        Pickup();
    }


    private void Pickup()
    {
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
