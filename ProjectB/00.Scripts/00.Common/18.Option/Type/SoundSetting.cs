using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [System.Serializable]
    public class SoundSlider
    {
        public Slider slider;
        [SerializeField] private Text sliderValueText;

        public void AddEvent(Action<float> action)
        {
            slider.onValueChanged.AddListener((value) =>
            {
                sliderValueText.text = $"{(int)(GetSliderValue() * 100)}%";
                action?.Invoke(GetSliderValue());
            });

            slider.onValueChanged?.Invoke(GetSliderValue());
        }

        public void RemoveAllEvent()
        {
            slider.onValueChanged.RemoveAllListeners();
        }

        public void SetSliderValue(float value)
        {
            slider.value = slider.maxValue * value;
        }

        private float GetSliderValue()
        {
            return slider.value / slider.maxValue;
        }
    }

    public SoundSlider backgorundSoundSlider;
    public SoundSlider sfxSoundSlider;
    public SoundSlider voiceSoundSlider;

    private void Awake()
    {
        backgorundSoundSlider.SetSliderValue(SoundManager.instance.bgmVolume);
        sfxSoundSlider.SetSliderValue(SoundManager.instance.sfxVolume);
        voiceSoundSlider.SetSliderValue(SoundManager.instance.voiceVolume);

        AddEvent();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }

    private void AddEvent()
    {
        backgorundSoundSlider.AddEvent(HandleOnBackgroundValueChanged);
        sfxSoundSlider.AddEvent(HandleOnSFXValueChanged);
        voiceSoundSlider.AddEvent(HandleOnVoiceValueChanged);
    }

    private void RemoveEvent()
    {
        backgorundSoundSlider.RemoveAllEvent();
        sfxSoundSlider.RemoveAllEvent();
        voiceSoundSlider.RemoveAllEvent();
    }

    private void HandleOnBackgroundValueChanged(float value)
    {
        SoundManager.instance.bgmVolume = value;
    }

    private void HandleOnSFXValueChanged(float value)
    {
        SoundManager.instance.sfxVolume = value;
    }

    private void HandleOnVoiceValueChanged(float value)
    {
        SoundManager.instance.voiceVolume = value;
    }
}
