using UnityEngine;

public class PlayerAiming : PlayerComponent
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private LayerMask groundMask;

    private PlayerMovement playerMovement;
    private bool isAiming;
    private ControlScheme controlScheme;
    private Vector3 aimDirection;

    public bool IsAiming => isAiming;
    
    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (isAiming)
        {
            Aim(controlScheme);
        }
        else
        {
            LookInMovementDirection();
        }
    }

    private void LookInMovementDirection()
    {
        Vector3 direction = playerMovement.GetMoveDirection();
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }

    private void Aim(ControlScheme controlScheme1)
    {
        if (controlScheme == ControlScheme.Keyboard)
        {
            AimMouse();
        }
        else if (controlScheme == ControlScheme.Gamepad)
        {
            AimGamepad();
        }
    }

    private void AimGamepad()
    {
        if (aimDirection == Vector3.zero) return;
        transform.forward = new Vector3(aimDirection.x, 0.0f, aimDirection.y);
    }

    private void AimMouse()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            // Calculate the direction
            aimDirection = position - transform.position;

            // You might want to delete this line.
            // Ignore the height difference.
            aimDirection.y = 0;

            // Make the transform look in the direction.
            transform.forward = aimDirection;
        }
    }
    
    private (bool success, Vector3 position) GetMousePosition()
    {
        var ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
        {
            // The Raycast hit something, return with the position.
            
            return (success: true, position: hitInfo.point);
        }
        else
        {
            // The Raycast did not hit anything.
            return (success: false, position: Vector3.zero);
        }
    }

    private void OnEnable()
    {
        inputReader.OnAimEvent += ReadAimInput;
        inputReader.OnRotateEvent += ReadRotateInputVector;
    }

    private void OnDisable()
    {
        inputReader.OnAimEvent -= ReadAimInput;
        inputReader.OnRotateEvent -= ReadRotateInputVector;
    }

    private void ReadAimInput(bool isAiming, ControlScheme controlScheme)
    {
        this.isAiming = isAiming;
        playerMovement.canRun = !isAiming;
        this.controlScheme = controlScheme;
    }

    private void ReadRotateInputVector(Vector2 direction)
    {
        aimDirection = direction;
    }
}