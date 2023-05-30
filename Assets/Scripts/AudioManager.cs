using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private AudioSource raceAudio;
    [SerializeField] private AudioSource levelEndAudio;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayBackgroundAudio()
    {
        raceAudio.Stop();
        backgroundAudio.Play();
    }

    public void PlayRaceAudio()
    {
        backgroundAudio.Stop();
        raceAudio.Play();
    }
    public void PlayLevelEndAudio()
    {
        raceAudio.Stop();
        levelEndAudio.Play();
    }

}
