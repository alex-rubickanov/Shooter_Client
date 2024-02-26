using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float runSpeed = 8.0f;

    private Vector3 moveDirection;
    private Vector2 moveInputVector;
    private bool isRunning;

    [HideInInspector] public bool canRun = true;

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        moveDirection = new Vector3(moveInputVector.x, 0.0f, moveInputVector.y);
        float speed = isRunning && canRun ? runSpeed : moveSpeed;

        transform.position += moveDirection * speed * Time.deltaTime;
    }

    public Vector3 GetMoveDirection()
    {
        return moveDirection;
    }

    private void OnEnable()
    {
        inputReader.OnMoveEvent += ReadMoveInputVector;
        inputReader.OnRunEvent += InputReaderOnOnRunEvent;
    }

    private void OnDisable()
    {
        inputReader.OnMoveEvent -= ReadMoveInputVector;
        inputReader.OnRunEvent -= InputReaderOnOnRunEvent;
    }

    private void ReadMoveInputVector(Vector2 moveInputVector)
    {
        this.moveInputVector = moveInputVector;
    }

    private void InputReaderOnOnRunEvent(bool isRunning)
    {
        this.isRunning = isRunning;
    }
}