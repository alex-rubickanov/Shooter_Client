using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : PlayerComponent
{
    [SerializeField] private float aimSpeed = 3.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float runSpeed = 8.0f;
    [SerializeField] private float moveSmoothTime = 0.1f;

    private Vector3 moveDirection;
    private Vector2 moveInputVector;
    private bool isRunning;
    private float currentSpeed;

    [HideInInspector] public bool canRun = true;

    private void Update()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        if (moveInputVector == Vector2.zero)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, moveSmoothTime);
            return;
        }

        moveDirection = new Vector3(moveInputVector.x, 0.0f, moveInputVector.y);

        if (playerManager.PlayerAiming.IsAiming)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, aimSpeed, moveSmoothTime);
        }
        else
        {
            float targetSpeed = isRunning && canRun ? runSpeed : moveSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, moveSmoothTime);
        }

        transform.position += moveDirection * currentSpeed * Time.deltaTime;
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