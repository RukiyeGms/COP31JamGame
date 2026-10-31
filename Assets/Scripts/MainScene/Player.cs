using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 target;

    private void Start()
    {
        target = transform.position;
    }

    private void Update()
    {
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
            transform.position = Vector2.MoveTowards(transformPosVector, target, speed*Time.deltaTime);
        }
    }
}
