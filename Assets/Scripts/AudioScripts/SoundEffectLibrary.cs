using System.Collections.Generic;
using UnityEngine;
public class SoundEffectLibrary : MonoBehaviour
{
    [SerializeField] SoundEffectGroup[] soundEffectGroups;
    Dictionary<string, List<AudioClip>> audioDictionary;

    void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        audioDictionary =  new Dictionary<string, List<AudioClip>>();
        foreach (SoundEffectGroup soundEffectGroup in soundEffectGroups)
        {
            audioDictionary[soundEffectGroup.name] = soundEffectGroup.AudioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if (audioDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = audioDictionary[name];
            if (audioClips.Count > 0)
            {
                return audioClips[ Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }

}


[System.Serializable]
public struct SoundEffectGroup
{
    public string name;
    public List<AudioClip> AudioClips;
}
