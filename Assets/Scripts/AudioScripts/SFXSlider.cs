using UnityEngine;
using UnityEngine.UI;

public class SFXSlider : MonoBehaviour
{
    [SerializeField] Slider sSlider;

    private void Start()
    {
        SoundEffectManager.ChangeSFXSlider(sSlider);
    }
}
