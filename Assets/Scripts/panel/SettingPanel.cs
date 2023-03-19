using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    public Slider slider_sound;
    public Slider slider_music;

    public void OnSoundValueChange(float f)
    {
        AudioManager.instance.soindVolumeChange(f);
        PlayerPrefs.SetFloat(Const.Sound, f);
    }

    public void OnMusicValueChange(float f)
    {
        AudioManager.instance.BcVolumeChange(f);
        PlayerPrefs.SetFloat(Const.Music, f);
    }

    public  void Show()
    {
        slider_music.value = PlayerPrefs.GetFloat(Const.Music, 0);
        slider_sound.value = PlayerPrefs.GetFloat(Const.Sound, 0);
    }
}
