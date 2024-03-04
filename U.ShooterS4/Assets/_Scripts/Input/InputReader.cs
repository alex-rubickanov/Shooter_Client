using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/Input Reader", fileName = "Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    [SerializeField] private bool isOriginalInputReader = false;
    [SerializeField] private ControlScheme controlScheme;
    private List<InputDevice> availableDevices;

    private GameInput gameInput;
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnRotateEvent;
    public event Action<bool, ControlScheme> OnAimEvent;
    public event Action<bool> OnRunEvent;
    public event Action<bool> OnFireEvent;
    public event Action OnReloadEvent;
    public event Action OnDashEvent;
    public event Action OnNextWeaponEvent;
    public event Action OnPreviousWeaponEvent;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Gameplay.SetCallbacks(this);
            gameInput.Gameplay.Enable();
        }
    }

    private void Awake()
    {
        CheckAvailableDevices();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;

        Vector2 moveInputVector = Vector2.zero;
        if (context.phase == InputActionPhase.Performed)
        {
            moveInputVector = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            moveInputVector = Vector2.zero;
        }

        OnMoveEvent?.Invoke(moveInputVector);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;

        bool isAiming = context.ReadValueAsButton();
        ControlScheme controlScheme =
            context.control.device.name == "Mouse" ? ControlScheme.Keyboard : ControlScheme.Gamepad;
        OnAimEvent?.Invoke(isAiming, controlScheme);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;

        bool isFiring = context.ReadValueAsButton();
        OnFireEvent?.Invoke(isFiring);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;

        OnRotateEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;

        bool isRunning = context.ReadValueAsButton();
        OnRunEvent?.Invoke(isRunning);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;

        OnReloadEvent?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;

        if (context.phase == InputActionPhase.Performed)
        {
            OnDashEvent?.Invoke();
        }
    }

    public void OnJoin(InputAction.CallbackContext context)
    {
    }

    public void OnNextWeapon(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;
        
        if (context.phase == InputActionPhase.Performed)
        {
            OnNextWeaponEvent?.Invoke();
        }
    }

    public void OnPreviousWeapon(InputAction.CallbackContext context)
    {
        if (!CanBeProceed(context)) return;
        
        if (context.phase == InputActionPhase.Performed)
        {
            OnNextWeaponEvent?.Invoke();
        }
    }

    private void CheckAvailableDevices()
    {
        switch (controlScheme)
        {
            case ControlScheme.Keyboard:
                availableDevices = new List<InputDevice>
                {
                    Mouse.current,
                    Keyboard.current
                };
                break;
            case ControlScheme.Gamepad:
                availableDevices = new List<InputDevice>();
                availableDevices.AddRange(Gamepad.all);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private bool CanBeProceed(InputAction.CallbackContext context)
    {
        if (isOriginalInputReader) return true;
        return availableDevices.Contains(context.control.device);
    }

    public void DisableInput()
    {
        gameInput.Disable();
    }

    public void EnableInput()
    {
        gameInput.Enable();
    }

    private void OnValidate()
    {
        CheckAvailableDevices();
    }
}