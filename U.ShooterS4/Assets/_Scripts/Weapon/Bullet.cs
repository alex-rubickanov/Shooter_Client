using ShooterNetwork;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerPawn owner;
    private PlayerData ownerData;
    private float damage;

    private PlayerPawn hittedPawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out hittedPawn))
        {
            if (hittedPawn != owner)
            {
                PlayerHealth health = hittedPawn.GetComponent<PlayerHealth>();
                health.TakeDamage(damage, ownerData);
                Destroy(gameObject);
            }
            else
            {
                //Debug.Log("You can't shoot yourself!");
            }
        }
        else
        {
            //Debug.Log("No player pawn found!");
        }
    }

    public void Initialize(PlayerPawn owner, PlayerData ownerData, float damage)
    {
        this.owner = owner;
        this.ownerData = ownerData;
        this.damage = damage;
    }
}