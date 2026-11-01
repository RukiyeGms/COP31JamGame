using UnityEngine;
using System.Collections;

public class Trash : MonoBehaviour
{
    [SerializeField] GameDatabase gameDatabase;
    TrashData trashData;
    Animator animator;
    [SerializeField] float pickupDistance = 2f;

    [SerializeField] float searchDuration = 1f;
    bool isBeingPicked;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        trashData = GetRandomTrash();
        animator.SetBool(trashData.animName, true);
    }
    private void OnMouseDown()
    {

        if (!GameManager.IsGamePlaying)
        { return; }

        if (isBeingPicked)
            return;

        if (FishRescueMinigame.Instance != null &&
            FishRescueMinigame.Instance.IsActive)
            return;

        if (GameManager.playerObject == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            GameManager.playerObject.transform.position
        );

        if (distance > pickupDistance)
            return;

        Player player = GameManager.playerObject.GetComponent<Player>();

        if (player == null || !player.StartSearch())
            return;

        isBeingPicked = true;
        StartCoroutine(PickupRoutine(player));
    }


    private void Pickup()
    {
        GameManager.TrashPicked();
        Destroy(gameObject);
    }

    private IEnumerator PickupRoutine(Player player)
    {
        yield return new WaitForSeconds(searchDuration);

        player.EndSearch();
        Pickup();
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

    public void CancelPickup()
    {
        if (!isBeingPicked)
            return;

        StopAllCoroutines();
        isBeingPicked = false;

        if (GameManager.playerObject != null)
        {
            Player player =
                GameManager.playerObject.GetComponent<Player>();

            if (player != null)
                player.CancelSearch();
        }
    }



}
