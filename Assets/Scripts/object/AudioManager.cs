using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource bc, sound;
    public AudioClip soundClip;

    private void Awake()
    {
        instance = this;
        bc = transform.Find("bc").GetComponent<AudioSource>();
        sound = transform.Find("sound").GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);

        bc.volume = PlayerPrefs.GetFloat(Const.Music);
        sound.volume = PlayerPrefs.GetFloat(Const.Sound);
        bc.loop = true;
        if (bc.isPlaying == true)
        {
            bc.Stop();
            bc.Play();
        }
    }
    //public void PlayBcMusic(AudioClip audioClip)
    //{
    //    bc.clip = audioClip;
    //    bc.loop = true;
    //    bc.Play();
    //}

    public void PlaySound()
    {
        sound.PlayOneShot(soundClip);
        sound.Play();
    }

    public void BcVolumeChange(float f)
    {
        bc.volume = f;
    }

    public void soindVolumeChange(float f)
    {
        sound.volume = f;
    }
}
