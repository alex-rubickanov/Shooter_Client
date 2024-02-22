using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/Input Reader", fileName = "Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    private GameInput gameInput;
    
    public event Action<Vector2> OnMoveEvent;
    public event Action<bool> OnAimEvent;

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
        } else if (context.phase == InputActionPhase.Canceled)
        {
            moveInputVector = Vector2.zero;
        }
        OnMoveEvent?.Invoke(moveInputVector);
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        bool isAiming = context.ReadValueAsButton();
        OnAimEvent?.Invoke(isAiming);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        
    }
}