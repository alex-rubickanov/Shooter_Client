using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerPawn playerPawn;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioManagerChannel sfxAudioChannel;

    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // TEST
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(40);
        }
    }

    public void TakeDamage(float damage)
    {
        var particle = Instantiate(hitParticle, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        Destroy(particle, 3f);

        sfxAudioChannel.RaiseEvent(GetRandomHitClip(), transform.position + new Vector3(0, 5, 0));

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Ragdoll 
        playerPawn.DestroyPawn();
    }

    private AudioClip GetRandomHitClip()
    {
        int randomIndex = UnityEngine.Random.Range(0, hitClips.Length);
        return hitClips[randomIndex];
    }
}