using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] private AudioSource intro;
    [SerializeField] private AudioSource loop;

    void OnEnable()
    {
        if (intro != null)
            intro.Play();
    }

    void Update()
    {
        if (!intro.isPlaying && !loop.isPlaying)
            loop.Play();
    }
}