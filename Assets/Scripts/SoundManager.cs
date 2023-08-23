using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    AudioSource m_AudioSource;
    [SerializeField] Sprite normalImage, muteImage;
    [SerializeField] Button mainSoundButton, pauseSoundButton;
    [SerializeField] Sound[] sounds;
    bool isSoundMute;


    private void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.loop = s.loop;
        }
    }
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        isSoundMute = Progress.Instance.playerInfo.isMute;
        if(isSoundMute)
            AudioListener.volume = 0;
        else AudioListener.volume = 1;
        SwapImage();
    }
    //По кнопке смены звука
    public void switchSound()
    {
        isSoundMute = !isSoundMute;
        if (isSoundMute)
            AudioListener.volume = 0;
        else AudioListener.volume = 1;

        SwapImage();
        Progress.Instance.playerInfo.isMute = isSoundMute;
        YandexSDK.Save();
    }
    //Смена спрайтов на обеих кнопках
    void SwapImage()
    {
        if (isSoundMute)
        {
            mainSoundButton.image.sprite = muteImage;
            pauseSoundButton.image.sprite = muteImage;
        }
        else
        {
            mainSoundButton.image.sprite = normalImage;
            pauseSoundButton.image.sprite = normalImage;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        Silence(!focus);      
    }
    private void OnApplicationPause(bool pause)
    {
        Silence(!pause);
    }
    void Silence(bool silence)
    {
        AudioListener.pause = silence;
    }
 
    public void MuteGame()
    {
        AudioListener.volume = 0;
    }
    public void UnmuteGame()
    {
        AudioListener.volume = 1;
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.audioSource.Play();
    }

    public Sound GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s;
    }
    //По кнопке
    public void MakeClickSound()
    {
        Play("ButtonClick");
    }

}
