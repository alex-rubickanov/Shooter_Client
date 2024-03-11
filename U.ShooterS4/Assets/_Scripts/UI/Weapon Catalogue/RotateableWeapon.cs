using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateableWeapon : MonoBehaviour
{
    [SerializeField] private InputAction pressed, axis;
    [SerializeField] private float speed;

    Coroutine rotationCoroutine;

    private Vector2 rotation;
    
    private void Awake()
    {
        pressed.Enable();
        axis.Enable();

        pressed.performed += _ => { rotationCoroutine = StartCoroutine(Rotate()); };
        pressed.canceled += _ => { StopCoroutine(rotationCoroutine); };
        axis.performed += context => { rotation = context.ReadValue<Vector2>(); }; 
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            transform.Rotate(Vector3.up, rotation.x * speed, Space.World);
            transform.Rotate(Vector3.right, rotation.y * speed, Space.World);
            yield return null;
        }
    }
}