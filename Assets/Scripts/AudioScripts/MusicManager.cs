using UnityEngine;
using UnityEngine.UI;


public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    AudioSource audioSource;
    public AudioClip musicAudio;

    Slider slider;
    float sValue = 0.8f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
        }
        else { Destroy(gameObject); }
    }
    void Start()
    {
        if (musicAudio != null) { PlayMusic(false, musicAudio); }
        
    }

    //=null makes the parameter optional
    public static void PlayMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            instance.audioSource.clip = audioClip;
        }
        if (instance.audioSource.clip != null)
        {
            if (!resetSong) { instance.audioSource.Stop(); }
            instance.audioSource.Play();
        }
    }

    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(slider.value);
        sValue = slider.value;
    }


    public static void PauseMusic()
    {
        instance.audioSource.Pause();
    }

    public static void ChangeMusicSlider(Slider slider)
    {
        if (slider != null) { slider.value = instance.sValue; }
        instance.slider = slider;
        instance.slider.onValueChanged.AddListener((value) => instance.OnValueChanged());
    }
}
