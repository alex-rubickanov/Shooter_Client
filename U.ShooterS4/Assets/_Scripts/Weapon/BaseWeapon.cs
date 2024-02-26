using System;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private Transform muzzleTransform;


    private Ray ray;
    private RaycastHit hitInfo;

    private float timer;

    private void Start()
    {
        timer = weaponData.fireRate;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void Shoot()
    {
        if (timer < weaponData.fireRate) return;
        timer = 0.0f;
        ray.origin = muzzleTransform.position;
        ray.direction = muzzleTransform.forward;
        Debug.DrawLine(ray.origin, muzzleTransform.forward * 30f, Color.red, 1.0f);
    }

    public GameObject GetWeaponPrefab()
    {
        return weaponPrefab;
    }
}
