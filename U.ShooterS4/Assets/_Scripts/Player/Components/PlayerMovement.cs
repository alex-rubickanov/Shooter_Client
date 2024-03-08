using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerAiming playerAiming;

    [SerializeField] private float aimSpeed = 3.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float runSpeed = 8.0f;
    [SerializeField] private float moveSmoothTime = 0.1f;
    [SerializeField] private float dashForce = 10.0f;
    [SerializeField] private float dashTime = 2.0f;
    [SerializeField] private float dashReload;
    [SerializeField] private AudioManagerChannel audioManagerChannel;
    [SerializeField] private AudioClip dashAudioClip;
    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector2 moveInputVector;
    private bool isRunning;
    private float currentSpeed;
    private Vector3 movementVelocity;
    private Vector3 moveDampVelocity;
    private bool canDash = true;
    private bool isDashing;
    private Coroutine dashCoroutine;

    [HideInInspector] public bool canRun = true;
    public float MaxSpeed => runSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude <= 1.0f && !playerAiming.IsAiming) return;
        SendMovePacket(transform.position.x, transform.position.z, transform.eulerAngles.y);
    }

    private void FixedUpdate()
    {
        HandleMove();
    }

    private void HandleMove()
    {
        if (isDashing) return;
        moveDirection = new Vector3(moveInputVector.x, 0, moveInputVector.y);

        float targetSpeed;

        if (playerAiming.IsAiming)
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


    private void Dash()
    {
        if (!canDash) return;
        dashCoroutine = StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        audioManagerChannel.RaiseEvent(dashAudioClip, transform.position + new Vector3(0, 10, 0));
        canDash = false;
        isDashing = true;
        rb.velocity = moveDirection * dashForce;
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector3.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashReload);
        canDash = true;
    }

    private void OnEnable()
    {
        inputReader.OnMoveEvent += ReadMoveInputVector;
        inputReader.OnRunEvent += InputReaderOnOnRunEvent;
        inputReader.OnDashEvent += Dash;
    }

    private void OnDisable()
    {
        inputReader.OnMoveEvent -= ReadMoveInputVector;
        inputReader.OnRunEvent -= InputReaderOnOnRunEvent;
        inputReader.OnDashEvent -= Dash;
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

    private void OnDestroy()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
    }
}