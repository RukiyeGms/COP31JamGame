using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 target;

    private void Awake()
    {
        GameManager.playerObject = gameObject;
    }

    private void Start()
    {
        target = transform.position;
    }

    private void Update()
    {
        if (FishRescueMinigame.Instance != null && FishRescueMinigame.Instance.IsActive)
        {
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
            if (target.x > transformPosVector.x)
            {
                transform.rotation = Quaternion.Euler(0, 180 , 0);
            }
            else if (target.x < transformPosVector.x)
            {
                transform.rotation = Quaternion.Euler(0, 0 , 0);    
            }
                transform.position = Vector2.MoveTowards(transformPosVector, target, speed * Time.deltaTime);
        }
    }
}
