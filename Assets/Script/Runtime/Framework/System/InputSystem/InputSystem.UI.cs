using System;
using UnityEngine.InputSystem;

namespace Script.Runtime.Framework.System
{
    public partial class InputSystem : InputSystem_Actions.IUIActions
    {
        public event Action<InputAction.CallbackContext> OnUIClose;

        public void OnClose(InputAction.CallbackContext context)
        {
            OnUIClose?.Invoke(context);
        }
    }
}