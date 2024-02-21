using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input/Input Reader", fileName = "Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    public void OnMove(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }
}