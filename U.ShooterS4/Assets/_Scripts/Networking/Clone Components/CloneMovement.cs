using System;
using UnityEngine;

public class CloneMovement : MonoBehaviour
{
    private Vector2 movementVelocity;

    public void Move(Vector2 position)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.y);
    }

    public void Rotate(float rotY)
    {
        transform.rotation = Quaternion.Euler(0, rotY, 0);
    }
}