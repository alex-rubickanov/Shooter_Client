using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float moveSpeed;


    private Vector3 moveDirection; 

    private Vector2 moveInputVector;
    private void Start()
    {
        inputReader.OnMoveEvent += ReadMoveInputVector;
    }

    private void Update()
    {
        HandleMove();
    }

    private void ReadMoveInputVector(Vector2 moveInputVector)
    {
        this.moveInputVector = moveInputVector;
    }

    private void HandleMove()
    {
        moveDirection = new Vector3(moveInputVector.x, 0.0f, moveInputVector.y);
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }
}