using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/Input Reader", fileName = "Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    [SerializeField] private ControlScheme controlScheme;

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
    public event Action OnOpenScoreTabEvent;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            gameInput.Gameplay.SetCallbacks(this);
            gameInput.Gameplay.Enable();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
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
        bool isAiming = context.ReadValueAsButton();
        ControlScheme controlScheme =
            context.control.device.name == "Mouse" ? ControlScheme.Keyboard : ControlScheme.Gamepad;
        OnAimEvent?.Invoke(isAiming, controlScheme);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        bool isFiring = context.ReadValueAsButton();
        OnFireEvent?.Invoke(isFiring);
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        OnRotateEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        bool isRunning = context.ReadValueAsButton();
        OnRunEvent?.Invoke(isRunning);
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        OnReloadEvent?.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
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
        if (context.phase == InputActionPhase.Performed)
        {
            OnNextWeaponEvent?.Invoke();
        }
    }

    public void OnPreviousWeapon(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnPreviousWeaponEvent?.Invoke();
        }
    }

    public void OnOpenScoreTab(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnOpenScoreTabEvent?.Invoke();
        }
    }

    public void DisableInput()
    {
        gameInput.Disable();
    }

    public void EnableInput()
    {
        gameInput.Enable();
    }

}