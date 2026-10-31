using UnityEngine;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    [SerializeField] Slider mslider;

    private void Start()
    {
        MusicManager.ChangeMusicSlider(mslider);
    }
}
