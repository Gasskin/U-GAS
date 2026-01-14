using System;
using System.Collections.Generic;
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

public class InputSystem : BaseSystem
{
    private PlayerInput _playerInput;
    private GameObject _input;
    private Dictionary<EInputMapping, BaseMap> _inputMapping = new();

    public override async UniTask Initialize()
    {
        _input = await SystemDriver.YooSystem.InitializeGameObjectAsync(null, "Assets/Bundles/Input/Input.prefab");
        _playerInput = _input.GetComponent<PlayerInput>();
        Object.DontDestroyOnLoad(_input);

        _inputMapping.Add(EInputMapping.Player,
            new PlayerMap(_playerInput.actions.FindActionMap(nameof(EInputMapping.Player))));
        _inputMapping.Add(EInputMapping.UI,
            new UIMap(_playerInput.actions.FindActionMap(nameof(EInputMapping.UI))));

        await UniTask.Yield();
    }

    public override void Destroy()
    {
        if (_input != null)
        {
            Object.Destroy(_input);
        }
        foreach (var input in _inputMapping.Values)
        {
            input.Destroy();
        }
    }
    
    public T GetMap<T>() where T : BaseMap
    {
        foreach (var map in _inputMapping.Values)
        {
            if (map is T m)
            {
                return m;
            }
        }
        return null;
    }
}