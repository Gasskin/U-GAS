using System;
using UnityEngine.InputSystem;

public partial class InputSystem : InputSystem_Actions.IPlayerActions
{
    public event Action<InputAction.CallbackContext> OnPlayerMove;
    
    public event Action<InputAction.CallbackContext> OnPlayerJump;

    public void OnMove(InputAction.CallbackContext context)
    {
        OnPlayerMove?.Invoke(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        OnPlayerJump?.Invoke(context);
    }
}