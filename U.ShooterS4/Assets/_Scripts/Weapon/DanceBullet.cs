using UnityEngine;

public class DanceBullet : Bullet
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out hittedPawn))
        {
            if (hittedPawn != owner)
            {
                DanceHandler danceHandler = hittedPawn.GetComponent<DanceHandler>();
                danceHandler.Dance();
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
}
