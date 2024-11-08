using UnityEngine;

public class AudioManager : GlobaManagerBase<AudioManager>
{
    public AudioSource audioSource;
    public AudioClip coinAudio;

    public void PlayerAudio(AudioClip audioClip,float volum = 1)
    {
        audioSource.PlayOneShot(audioClip, volum);
    }

    public void PlayerAudio()
    {
        PlayerAudio(coinAudio, 0.5f);
    }
}
