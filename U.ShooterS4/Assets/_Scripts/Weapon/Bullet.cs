using ShooterNetwork;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public PlayerPawn owner;
    protected PlayerData ownerData;
    public float damage;

    protected PlayerPawn hittedPawn;

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out hittedPawn))
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
            Destroy(gameObject);
        }
    }

    public void Initialize(PlayerPawn owner, PlayerData ownerData, float damage)
    {
        this.owner = owner;
        this.ownerData = ownerData;
        this.damage = damage;
    }
}