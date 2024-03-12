using ShooterNetwork;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private PlayerPawn playerPawn;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private AudioClip[] deathClips;
    [SerializeField] private AudioManagerChannel sfxAudioChannel;

    private float currentHealth;
    private RagdollController ragdollController;
    private bool isDead = false;

    private void Awake()
    {
        ragdollController = GetComponentInChildren<RagdollController>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        GameplayHUD.Instance.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(float damage, PlayerData hitFrom)
    {
        var particle = Instantiate(hitParticle, transform.position + new Vector3(0, 0.5f, 0) + GetRandomYVector(), transform.rotation * GetRandomYQuaternion());
        Destroy(particle.gameObject, 1.5f);

        int randomHitSoundIndex = GetRandomHitClipIndex();
        
        sfxAudioChannel.RaiseEvent(hitClips[randomHitSoundIndex], transform.position + new Vector3(0, 5, 0));

        currentHealth -= damage;
        
        SendHitPacket(randomHitSoundIndex);
        
        GameplayHUD.Instance.UpdateHealth(currentHealth);
        
        if (currentHealth <= 0)
        {
            string message2 = $"{Client.Instance.PlayerData.Name} ID:{Client.Instance.PlayerData.ID} has died. Killer - {hitFrom.Name} ID:{hitFrom.ID} \n";
            Debug.Log(message2);
            SendDebugLogPacket(message2);
            
            Die(hitFrom);
        }
    }

    private void Die(PlayerData killerData)
    {
        if(isDead) return;
        isDead = true;
        int randomDeathSoundIndex = GetRandomDeathClipIndex();
        sfxAudioChannel.RaiseEvent(deathClips[randomDeathSoundIndex], transform.position);
        
        ragdollController.EnableRagdoll();
        playerPawn.GetInputReader().DisableGameplayInput();
        
        SendDeathPacket(int.Parse(killerData.ID), randomDeathSoundIndex);
        KillFeedManager.Instance.AddKill(killerData.Name, Client.Instance.PlayerData.Name);
        ScoreManager.Instance.AddKill(killerData, Client.Instance.PlayerData);
        
        playerPawn.Respawn();
    }

    private int GetRandomHitClipIndex()
    {
        int randomIndex = UnityEngine.Random.Range(0, hitClips.Length);
        return randomIndex;
    }
    
    private int GetRandomDeathClipIndex()
    {
        int randomIndex = UnityEngine.Random.Range(0, deathClips.Length);
        return randomIndex;
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