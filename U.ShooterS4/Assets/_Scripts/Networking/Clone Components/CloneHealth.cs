using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneHealth : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioManagerChannel sfxAudioChannel;

    private RagdollController ragdollController;

    private void Awake()
    {
        ragdollController = GetComponentInChildren<RagdollController>();
    }

    public void TakeDamage(int hitSoundIndex)
    {
        var particle = Instantiate(hitParticle, transform.position + new Vector3(0, 0.5f, 0) + GetRandomYVector(), transform.rotation * GetRandomYQuaternion());
        Destroy(particle.gameObject, 1.5f);

        sfxAudioChannel.RaiseEvent(hitClips[hitSoundIndex], transform.position + new Vector3(0, 5, 0));
    }

    public void Die(int deathSoundIndex)
    {
        sfxAudioChannel.RaiseEvent(deathClips[deathSoundIndex], transform.position);
        
        ragdollController.EnableRagdoll();
        
        Destroy(gameObject, 2.0f);
    }

    private Quaternion GetRandomYQuaternion()
    {
        return Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
    }
    
    private Vector3 GetRandomYVector()
    {
        return new Vector3(0, UnityEngine.Random.Range(0.0f, 1.0f), 0);
    }
}
