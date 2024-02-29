using UnityEngine;

public class PlayerMovement : PlayerComponent
{
    [SerializeField] private float aimSpeed = 3.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float runSpeed = 8.0f;
    [SerializeField] private float moveSmoothTime = 0.1f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector2 moveInputVector;
    private bool isRunning;
    private float currentSpeed;
    private Vector3 movementVelocity;
    private Vector3 moveDampVelocity;

    [HideInInspector] public bool canRun = true;
    public float MaxSpeed => runSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        moveDirection = new Vector3(moveInputVector.x, 0, moveInputVector.y);

        float targetSpeed;

        if (playerManager.PlayerAiming.IsAiming)
        {
            targetSpeed = aimSpeed;
        }
        else if (canRun && isRunning)
        {
            targetSpeed = runSpeed;
        }
        else
        {
            targetSpeed = moveSpeed;
        }

        movementVelocity = Vector3.SmoothDamp(
            movementVelocity,
            moveDirection * targetSpeed,
            ref moveDampVelocity,
            moveSmoothTime
        );

        rb.velocity = new Vector3(movementVelocity.x, 0.0f, movementVelocity.z);
    }

    public Vector3 GetMovementVelocity()
    {
        return movementVelocity;
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

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
}