using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerPawn playerPawn;
    [SerializeField] private float maxHealth = 100f;
    
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
}
