using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public AudioSource audioSource;
    public AudioClip soundEat;
    public AudioClip soundAlien;

    public void PlaySoundEat()
    {
        audioSource.PlayOneShot(soundEat);
    }

    public void PlaySoundAlien()
    {
        audioSource.PlayOneShot(soundAlien);
    }
}
