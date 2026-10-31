using UnityEngine;

public class Trash : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Trash Picked");
        Destroy(gameObject);
    }

}
