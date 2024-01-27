using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] Clips;
    public AudioSource AudioSource;

    int i = -1;

    void Update()
    {
        if (!AudioSource.isPlaying)
        {
            i = (i + 1) % Clips.Length;
            AudioSource.clip = Clips[i];
            AudioSource.Play();
        }
    }

    private void OnDestroy()
    {
        AudioSource.Stop();
    }
}
