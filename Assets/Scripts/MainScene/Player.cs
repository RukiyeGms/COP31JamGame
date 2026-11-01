using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 target;

    Animator anim;
    [SerializeField] float findAnimationDuration = 0.5f;

    bool isBusy;

    public bool IsBusy => isBusy;

    private void Awake()
    {
        GameManager.playerObject = gameObject;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        target = transform.position;
    }

    private void Update()
    {
        if (GameManager.IsPaused || GameManager.IsIntroActive)
            return;

        if (isBusy || (FishRescueMinigame.Instance != null && FishRescueMinigame.Instance.IsActive))
        {
            target = transform.position;
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        moveToTarget();
    }

    void moveToTarget()
    {
        Vector2 transformPosVector = new Vector2(transform.position.x, transform.position.y);
        if (transformPosVector != target)
        {
            if (!anim.GetBool("isMoving"))
            {
                anim.SetBool("isMoving", true);
            }

            if (target.x > transformPosVector.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (target.x < transformPosVector.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            transform.position = Vector2.MoveTowards(transformPosVector, target, speed * Time.deltaTime);
        }
        else 
        {
            if (anim.GetBool("isMoving"))
            {
                anim.SetBool("isMoving", false);
            }
        }
    }


    public bool StartSearch()
    {
        if (isBusy)
            return false;

        isBusy = true;
        target = transform.position;

        anim.SetBool("isMoving", false);
        anim.SetBool("isSearching", true);

        return true;
    }

    public void EndSearch()
    {
        if (!isBusy)
            return;

        anim.SetBool("isSearching", false);
        anim.SetTrigger("find");

        StartCoroutine(WaitForFind());
    }

    private IEnumerator WaitForFind()
    {
        yield return new WaitForSeconds(findAnimationDuration);

        isBusy = false;
    }

    public void CancelSearch()
    {
        if (!isBusy)
            return;

        anim.SetBool("isSearching", false);
        anim.ResetTrigger("find");
        anim.Play("idle", 0, 0f);

        target = transform.position;
        isBusy = false;
    }
}
