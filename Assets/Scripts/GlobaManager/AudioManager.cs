using UnityEngine;

public class AudioManager : GlobaManagerBase<AudioManager>
{
    public AudioSource audioSource;
    public AudioClip coinAudio;
    public float volume;

    public void PlayerAudio(AudioClip audioClip,float volum = 1)
    {
        audioSource.PlayOneShot(audioClip, volum*this.volume);
    }

    public void PlayerAudio()
    {
        PlayerAudio(coinAudio, 0.5f);
    }

    public void SetVolume(float volume)
    {
        this.volume = volume;
    }
}
