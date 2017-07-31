using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour {

    [SerializeField]
    AudioClip[] talkSounds;
    [SerializeField]
    AudioClip boredSound;
    [SerializeField]
    AudioClip interestSound;
    [SerializeField]
    AudioClip confuseSound;
    [SerializeField]
    float minPitch;
    [SerializeField]
    float maxPitch;

    AudioSource source;

    private void Awake()
    {
        Game.Audio = this;
        source = GetComponent<AudioSource>();
    }

    public void Talk()
    {
        source.pitch = Random.Range(minPitch, maxPitch);
        source.PlayOneShot(talkSounds.RandomItem());
    }

    public void Bored()
    {
        AudioSource.PlayClipAtPoint(boredSound, Vector3.zero, 0.8f);
    }

    public void Interested()
    {
        AudioSource.PlayClipAtPoint(interestSound, Vector3.zero, 0.8f);
    }

    public void Confused()
    {
        AudioSource.PlayClipAtPoint(confuseSound, Vector3.zero, 0.8f);
    }
}
