using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponConfig weaponConfig;
    [SerializeField] private Transform muzzleTransform;


    private Ray ray;
    private RaycastHit hitInfo;

    private float timer;
    private bool shotFired = false;

    protected void Start()
    {
        timer = weaponConfig.fireRate;
    }

    protected void Update()
    {
        timer += Time.deltaTime;
    }

    public void Shoot()
    {
        if (!weaponConfig.isAutomatic)
        {
            if (shotFired)
            {
                return;
            }

            shotFired = true;
        }

        if (timer < weaponConfig.fireRate) return;
        timer = 0.0f;
        Debug.DrawLine(muzzleTransform.position, muzzleTransform.forward * 30f, Color.red, 1.0f);
    }

    public void StopFiring()
    {
        shotFired = false;
    }
}