using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerAudio(AudioClip audioClip,float volum = 1)
    {
        audioSource.PlayOneShot(audioClip, volum);
    }
}
