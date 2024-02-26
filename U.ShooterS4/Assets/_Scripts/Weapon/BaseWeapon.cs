using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform muzzleTransform;


    private Ray ray;
    private RaycastHit hitInfo;

    private float timer;
    private bool shotFired = false;
    protected  void Start()
    {
        timer = weaponData.fireRate;
    }

    protected void Update()
    {
        timer += Time.deltaTime;
    }

    public void Shoot()
    {
        // if (!weaponData.isAutomatic)
        // {
        //     if (shotFired)
        //     {
        //         return;
        //     }
        //     shotFired = true;
        // }
        // if (timer < weaponData.fireRate) return;
        timer = 0.0f;
        Debug.DrawLine(muzzleTransform.position, muzzleTransform.forward * 30f, Color.red, 1.0f);
        Debug.Log($"{gameObject.name} shot");
    }

    public GameObject GetWeaponPrefab()
    {
        return weaponPrefab;
    }

    public void StopFiring()
    {
        shotFired = false;
    }
}
