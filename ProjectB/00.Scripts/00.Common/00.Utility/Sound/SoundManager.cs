using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM,
    SFX,
    VOICE  
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private float _bgmVolume;
    public float bgmVolume        
    {
        get
        {
            return _bgmVolume;
        }
        set
        {
            if (_bgmVolume == value) return;

            _bgmVolume = value;
            HangleOnChangedAudioSound(SoundType.BGM);
        }
    }

    [SerializeField] private float _sfxVolume;
    public float sfxVolume
    {
        get
        {
            return _sfxVolume;
        }
        set
        {
            if (_sfxVolume == value) return;

            _sfxVolume = value;
            HangleOnChangedAudioSound(SoundType.SFX);
        }
    }

    [SerializeField] private float _voiceVolume;
    public float voiceVolume
    {
        get
        {
            return _voiceVolume;
        }
        set
        {
            if (_voiceVolume == value) return;

            _voiceVolume = value;
            HangleOnChangedAudioSound(SoundType.VOICE);
        }
    }

    private Dictionary<SoundType, List<AudioSource>> audioSources = new Dictionary<SoundType, List<AudioSource>>()
    {
        { SoundType.BGM, new List<AudioSource>() },
        { SoundType.SFX, new List<AudioSource>() },
        { SoundType.VOICE, new List<AudioSource>() }
    };

    public Action<SoundType, float> OnVolumeChanged;

    public AudioSource PlaySound(string soundName)
    {
        // SoundData soundData = ResourceManager.instance.Load<SoundData>(soundName);

        //  return SetAudio(soundData);\

        return null;
    }

    public AudioSource PlaySound(SoundData soundData)
    {
        //  return SetAudio(soundData);
        return null;
    }

    private AudioSource SetAudio(SoundData soundData)
    {
        AudioSource audioSource = GetAudioSource(soundData.soundType);
        AudioClip audioClip = soundData.audioClip;

        audioSource.clip = audioClip;

        audioSource.volume = GetVolume(soundData.soundType);
        audioSource.loop = soundData.isLoop;
        audioSource.spatialBlend = soundData.is3DSound ? 1 : 0;

        audioSource.Play();

        return audioSource;
    }

    private AudioSource AddAudioSource(SoundType soundType)
    {
        GameObject audioSourceObj = new GameObject("Audio Source");
        audioSourceObj.transform.SetParent(transform);

        AudioSource audioSource = audioSourceObj.AddComponent<AudioSource>();
        audioSources[soundType].Add(audioSource);

        return audioSource;
    }

    private AudioSource GetAudioSource(SoundType soundType)
    {
        for (int i = 0; i < audioSources[soundType].Count; i++)
        {
            if (!audioSources[soundType][i].isPlaying)
            {
                return audioSources[soundType][i];
            }
        }

        return AddAudioSource(soundType);
    }

    private void HangleOnChangedAudioSound(SoundType changeSoundType)
    {
        for (int i = 0; i < audioSources[changeSoundType].Count; i++)
        {
            if (audioSources[changeSoundType][i].isPlaying)
            {
                audioSources[changeSoundType][i].volume = GetVolume(changeSoundType);
            }
        }

        OnVolumeChanged?.Invoke(changeSoundType, GetVolume(changeSoundType));
    }

    public void ClearAllSound()
    {
        foreach (var audioSource in audioSources)
        {
            for (int i = 0; i < audioSource.Value.Count; i++)
            {
                audioSource.Value[i].Stop();
            }
        }
    }

    private float GetVolume(SoundType soundType)
    {
        float volume = 0;

        switch (soundType)
        {
            case SoundType.BGM:
                volume = bgmVolume;
                break;
            case SoundType.SFX:
                volume = sfxVolume;
                break;
            case SoundType.VOICE:
                volume = voiceVolume;
                break;
        }

        return volume;
    }

    public void SetAllSoundVolume(float sound)
    {
        voiceVolume = sound;
        bgmVolume = sound;
        sfxVolume = sound;
    }
}
