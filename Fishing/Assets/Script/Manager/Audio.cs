using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[System.Serializable]
public enum SoundType
{
    MOUSEOVER,
    MOUSECLICK,
    BITE,
    BUBBLE,
    HOOKSTART,
    HOOKING,
    SWIRLING,
    OORAL,
    BREAKING
}

[System.Serializable]
public class Sound
{
    public SoundType Type;
    public AudioClip AudioClip;
}
public class Audio : MonoSingleton<Audio>
{
    public List<Sound> AudioList;
    public Dictionary<SoundType, AudioClip> AudioDictionary = new Dictionary<SoundType, AudioClip>();
    public AudioSource BgAudio; // = new AudioSource();
    public AudioSource EffectAudio;
    public AudioSource SoundAudio;

    public GameObject MusicBtn;
    public GameObject SoundBtn;

    public static Audio Instance;
    // Use this for initialization
    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        BgAudio.mute = PlayerPrefs.GetInt("BgAudio", 0) == 1;
        EffectAudio.mute = PlayerPrefs.GetInt("EffectAudio", 0) == 1;
        CollectSound();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CollectSound()
    {
        foreach (var sound in AudioList)
        {
            AudioDictionary.Add(sound.Type, sound.AudioClip);
        }
    }

    public void PlayEffect(SoundType type, bool isLoop, float delay)
    {
        AudioClip temp = AudioDictionary[type];

        if (temp != null && !EffectAudio.isPlaying)
        {
            EffectAudio.clip = temp;
            EffectAudio.loop = isLoop;
            EffectAudio.SetScheduledStartTime(delay);
            EffectAudio.Play();
        }
    }

    public void PlaySound(SoundType type, bool isLoop, float delay)
    {
        AudioClip temp = AudioDictionary[type];

        if (temp != null && !SoundAudio.isPlaying)
        {
            SoundAudio.clip = temp;
            SoundAudio.loop = isLoop;
            SoundAudio.SetScheduledStartTime(delay);
            SoundAudio.Play();
        }
    }

    public void StopEffect()
    {
        EffectAudio.Stop();
    }

    [ContextMenu("test")]
    public void Hook()
    {
        StartCoroutine("HookSound");
    }

    public IEnumerator HookSound()
    {
        EffectAudio.clip = AudioDictionary[SoundType.HOOKSTART];
        EffectAudio.loop = false;
        EffectAudio.Play();
        yield return new WaitForSeconds(EffectAudio.clip.length);
        EffectAudio.clip = AudioDictionary[SoundType.HOOKING];
        EffectAudio.loop = true;
        EffectAudio.Play();
    }

    public void StopHookSound()
    {
        StopCoroutine("HookSound");
        EffectAudio.Stop();
    }

    public void MuteMusic()
    {
        BgAudio.mute = !BgAudio.mute;
        if (BgAudio.mute)
        {
            GameObject.Find("MusicBtn").transform.GetChild(0).gameObject.SetActive(true);
            PlayerPrefs.SetInt("BgAudio", 1);
        }
        else
        {
            GameObject.Find("MusicBtn").transform.GetChild(0).gameObject.SetActive(false);
            PlayerPrefs.SetInt("BgAudio", 0);
        }
    }

    public void MuteEffect()
    {
        EffectAudio.mute = !EffectAudio.mute;
        if (EffectAudio.mute)
        {
            GameObject.Find("SoundBtn").transform.GetChild(0).gameObject.SetActive(true);
            PlayerPrefs.SetInt("EffectAudio", 1);
        }
        else
        {
            GameObject.Find("SoundBtn").transform.GetChild(0).gameObject.SetActive(false);
            PlayerPrefs.SetInt("EffectAudio", 0);
        }
    }

    public void OnLevelWasLoaded()
    {
        StopEffect();
    }
}
