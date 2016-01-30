using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameSoundManager : MonoBehaviour {

    public float musicVolume;
    public float soundEffectVolume;
    private ArrayList musicAudioSourceList = new ArrayList();
    private ArrayList soundEffectAudioSourceList = new ArrayList();

    private Slider musicVolumeSlider;
    private Slider soundeffectVolumeSlider;

    // Use this for initialization
    void Start()
    {
        musicVolumeSlider = GameObject.Find("soundMusicSlider").GetComponent<Slider>();
        soundeffectVolumeSlider = GameObject.Find("soundEffectSlider").GetComponent<Slider>();
    }

    public void changeMusicVolume()
    {
        float volume = musicVolumeSlider.value;
        //Debug.Log("set music volume:"+ volume);
        musicVolume = volume;
        controlAllMusicVolumes();
    }
    public void changeSoundEffectVolume()
    {
        float volume = soundeffectVolumeSlider.value;
        //Debug.Log("set se volume:" + volume);
        soundEffectVolume = volume;
        controlAllSoundEffectVolume();
    }

    public void addMusicAudioSource(AudioSource source)
    {
        musicAudioSourceList.Add(source);
    }
    public void removeMusicAudioSource(AudioSource source)
    {
        musicAudioSourceList.Remove(source);
    }

    public void addSoundEffectAudioSource(AudioSource source)
    {
        soundEffectAudioSourceList.Add(source);
    }
    public void removeSoundEffectAudioSource(AudioSource source)
    {
        soundEffectAudioSourceList.Remove(source);
    }

    private void controlAllMusicVolumes()
    {
        foreach (AudioSource source in musicAudioSourceList)
        {
            source.volume = musicVolume ;
        }
    }
    
    private void controlAllSoundEffectVolume()
    {
        foreach (AudioSource source in soundEffectAudioSourceList)
        {
            Debug.Log("set vol changed ->" + soundEffectVolume);
            source.volume = soundEffectVolume;
        }
    }

}
