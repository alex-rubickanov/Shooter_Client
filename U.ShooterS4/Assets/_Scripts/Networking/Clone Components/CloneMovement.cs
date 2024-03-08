using UnityEngine;

public class CloneMovement : MonoBehaviour
{
    private Vector2 movementVelocity;

    public void Move(Vector2 position, Vector2 movementVelocity)
    {
        transform.position = new Vector3(position.x, transform.position.y, position.y);
        this.movementVelocity = movementVelocity;
    }

    public void Rotate(float rotY)
    {
        transform.rotation = Quaternion.Euler(0, rotY, 0);
    }

    public Vector3 GetMovementVelocity()
    {
        return movementVelocity;
    }
}