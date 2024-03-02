using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float sfxVolume;
    [SerializeField] private AudioManagerChannel sfxChannel;

    private void Start()
    {
        sfxChannel.OnEventRaised += PlaySFX;
    }

    public void PlaySFX(AudioClip audioClip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, sfxVolume);
    }
}
