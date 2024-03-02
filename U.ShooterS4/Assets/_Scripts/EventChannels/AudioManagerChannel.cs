using UnityEngine;
using System;

[CreateAssetMenu(fileName = "AudioManagerChannel", menuName = "ScriptableObjects/AudioManagerChannel", order = 1)]
public class AudioManagerChannel : ScriptableObject
{
    public event Action<AudioClip, Vector3> OnEventRaised;
    
    public void RaiseEvent(AudioClip audioClip, Vector3 position)
    {
        OnEventRaised?.Invoke(audioClip, position);
    }
}