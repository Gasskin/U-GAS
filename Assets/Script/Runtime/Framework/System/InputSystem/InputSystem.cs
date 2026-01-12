using Cysharp.Threading.Tasks;
using UnityEngine;

public class InputSystem : BaseSystem
{
    private GameObject _input;

    public override async UniTask Initialize()
    {
        _input = await SystemDriver.Get<YooSystem>().InitializeGameObjectAsync(null, "Assets/Bundles/Input/PlayerInput.prefab");
        await UniTask.Yield();
    }

    public override void Close()
    {
        if (_input != null)  
        {
            Object.Destroy(_input);
        }
    }
}