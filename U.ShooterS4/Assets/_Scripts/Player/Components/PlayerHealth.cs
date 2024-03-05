using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerPawn playerPawn;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioManagerChannel sfxAudioChannel;

    private float currentHealth;
    private RagdollController ragdollController;

    private void Awake()
    {
        ragdollController = GetComponentInChildren<RagdollController>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        var particle = Instantiate(hitParticle, transform.position + new Vector3(0, 0.5f, 0) + GetRandomYVector(), transform.rotation * GetRandomYQuaternion());
        Destroy(particle.gameObject, 1.5f);

        sfxAudioChannel.RaiseEvent(GetRandomHitClip(), transform.position + new Vector3(0, 5, 0));

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        sfxAudioChannel.RaiseEvent(deathClips[UnityEngine.Random.Range(0, deathClips.Length)], transform.position);
        
        ragdollController.EnableRagdoll();
        playerPawn.GetInputReader().DisableInput();
        
        playerPawn.Respawn();
    }

    private AudioClip GetRandomHitClip()
    {
        int randomIndex = UnityEngine.Random.Range(0, hitClips.Length);
        return hitClips[randomIndex];
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