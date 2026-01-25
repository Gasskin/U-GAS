using System;
using UnityEngine.InputSystem;

public partial class InputSystem : InputSystem_Actions.IPlayerActions
{
    public event Action<InputAction.CallbackContext> OnPlayerMove;

    public event Action<InputAction.CallbackContext> OnPlayerJump;
    public event Action<InputAction.CallbackContext> OnPlayerDash;
    public event Action<InputAction.CallbackContext> OnPlayerAttack;

    public void OnMove(InputAction.CallbackContext context)
    {
        OnPlayerMove?.Invoke(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        OnPlayerJump?.Invoke(context);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        OnPlayerDash?.Invoke(context);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        OnPlayerAttack?.Invoke(context);
    }
}