using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerPawn owner;
    private float damage;

    private PlayerPawn hittedPawn;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent(out hittedPawn))
        {
            if (hittedPawn != owner)
            {
                PlayerHealth health = hittedPawn.GetComponent<PlayerHealth>();
                health.TakeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("You can't shoot yourself!");
            }
        }
        else
        {
            Debug.Log("No player pawn found!");
        }
    }

    public void Initialize(PlayerPawn owner, float damage)
    {
        this.owner = owner;
        this.damage = damage;
    }
}