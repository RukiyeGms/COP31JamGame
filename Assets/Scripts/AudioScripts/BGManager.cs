using UnityEngine;
using UnityEngine.UI;

public class BGManager : MonoBehaviour
{
    private static BGManager instance;
    AudioSource audioSource;
    public AudioClip backgroundAudio;
    Slider slider;
    float sValue = 1;

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
        if (backgroundAudio != null) { PlayBackground(false , backgroundAudio); }
       
        
    }

    //=null makes the parameter optional
    public static void PlayBackground(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            instance.audioSource.clip = audioClip;
        }
        if (instance.audioSource.clip != null) 
        {
            if (!resetSong) { instance.audioSource.Stop();  }
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


    public static void PauseBackground()
    {
            instance.audioSource.Pause();
    }
    public static void ChangeBgSlider(Slider slider)
    {
        if (slider != null) { slider.value = instance.sValue; }
        instance.slider = slider;
        instance.slider.onValueChanged.AddListener((value) => instance.OnValueChanged());
    }

}
