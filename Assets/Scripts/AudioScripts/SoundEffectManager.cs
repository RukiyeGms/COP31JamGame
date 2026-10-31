using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    static SoundEffectManager instance;
    AudioSource audioSource;
    static SoundEffectLibrary soundEffectLibrary;
    Slider SFXSlider;
    float sValue = 0.8f;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

    }


    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null) 
        {
            instance.audioSource.pitch = 1;
            instance.audioSource.PlayOneShot(audioClip);
        }
    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;    
    }

    public void OnValueChanged()
    {

        SetVolume(SFXSlider.value);
        sValue = SFXSlider.value;
    }

    public static void ChangeSFXSlider(Slider slider)
    {
        if (slider != null) { slider.value = instance.sValue; }
        instance.SFXSlider = slider;
        instance.SFXSlider.onValueChanged.AddListener((value) => instance.OnValueChanged());
    }

    public static void PlayVoice(AudioClip ac, float pitch = 1f)
    {
        instance.audioSource.pitch = pitch;
        instance.audioSource.PlayOneShot(ac);
    }
}
