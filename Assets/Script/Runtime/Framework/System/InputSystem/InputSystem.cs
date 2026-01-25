using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public enum EInputMapping
{
    None = 0,
    Player,
    UI,
}

public partial class InputSystem : BaseSystem
{
    // private PlayerInput _playerInput;
    // private GameObject _input;
    
    private InputSystem_Actions _actions;

    public override async UniTask Initialize()
    {
        _actions = new InputSystem_Actions();
        _actions.Player.SetCallbacks(this);
        _actions.UI.SetCallbacks(this);
        
        SwitchMapping(EInputMapping.Player);
        
        await UniTask.Yield();
    }

    public override void Destroy()
    {
        _actions.Disable();
    }

    public void SwitchMapping(EInputMapping mapping)
    {
        _actions.Disable();
        switch (mapping)
        {
            case EInputMapping.Player:
                _actions.Player.Enable();
                break;
            case EInputMapping.UI:
                _actions.UI.Enable();
                break;
        }
    }
}