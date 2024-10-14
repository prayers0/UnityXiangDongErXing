using System.Collections;
using UnityEngine;

public class AudioManager : GlobaManagerBase<AudioManager>
{
    [SerializeField] private AudioSource audioSource;

    public void PlayerAudio(AudioClip audioClip,float volum = 1)
    {
        audioSource.PlayOneShot(audioClip, volum);
    }
}
