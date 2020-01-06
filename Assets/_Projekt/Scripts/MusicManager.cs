using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource mainMusic;
    public AudioSource bossMusic;
    public AudioSource deadMusic;
    public AudioSource winMusic;

    private int currentMusic = 0;

    private void Start()
    {
        StopAll();
        mainMusic.Play();
    }

    private void StopAll()
    {
        mainMusic.Stop();
        bossMusic.Stop();
        deadMusic.Stop();
        winMusic.Stop();
    }

    public void PlayBossMusic()
    {
        if (currentMusic == 1) return;
        currentMusic = 1;

        StopAll();
        bossMusic.Play();
    }

    public void PlayWinMusic()
    {
        if (currentMusic == 2) return;
        currentMusic = 2;

        StopAll();
        winMusic.Play();
    }

    public void PlayDeadMusic()
    {
        if (currentMusic == 3) return;
        currentMusic = 3;

        StopAll();
        deadMusic.Play();
    }
}
