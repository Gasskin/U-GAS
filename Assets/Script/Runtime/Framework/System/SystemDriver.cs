using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum EYooAssetsMode
{
    Editor,
    Offline,
}

public class SystemDriver : MonoBehaviour
{
#region static
    public static SystemDriver Instance { get;private set; }

    private static Dictionary<Type, BaseSystem> _baseSystemsDict = new();
    
    private static T Get<T>() where T : BaseSystem
    {
        if (Instance == null)
        {
            return null;
        }
        
        var type = typeof(T);
        if (_baseSystemsDict.TryGetValue(type, out var sys))
        {
            return sys as T;
        }
        
        for (int i = 0; i < Instance._baseSystems.Count; i++)
        {
            if (Instance._baseSystems[i] is T { Initialized: true } t)
            {
                _baseSystemsDict[type] = t;
                return t;
            }
        }
        return null;
    }
#endregion

    public EYooAssetsMode YooAssetsMode;

    public Transform UIRoot;

    // framework
    public static YooSystem YooSystem => Get<YooSystem>();
    public static ConfigSystem ConfigSystem => Get<ConfigSystem>();
    public static TimeSystem TimeSystem => Get<TimeSystem>();
    public static EntitySystem EntitySystem => Get<EntitySystem>();
    public static ProcedureSystem ProcedureSystem => Get<ProcedureSystem>();
    public static InputSystem InputSystem => Get<InputSystem>();

    // game
    public static PlayerDataSystem PlayerDataSystem => Get<PlayerDataSystem>();
    
    private readonly List<BaseSystem> _baseSystems = new()
    {
        // Framework System
        new YooSystem(),
        new ConfigSystem(),
        new EntitySystem(),
        new ProcedureSystem(),
        // Game System
        new TimeSystem(),
        new InputSystem(),
        new PlayerDataSystem(),
    };

    private List<ITickSystem> _tickSystems = new();
    private List<IFixedTickSystem> _fixedTickSystems = new();
    private List<ILateTickSystem> _lateTickSystems = new();

    private void Start()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        StartAsync().Forget();
    }

    private async UniTaskVoid StartAsync()
    {
        foreach (var sys in _baseSystems)
        {
            await sys.Initialize();
            sys.Initialized = true;
            if (sys is ITickSystem tick)
            {
                _tickSystems.Add(tick);
            }
            if (sys is IFixedTickSystem fixedTick)
            {
                _fixedTickSystems.Add(fixedTick);
            }
            if (sys is ILateTickSystem lateTick)
            {
                _lateTickSystems.Add(lateTick);
            }
        }
        
        ProcedureSystem.ChangeProcedure<BattleProcedure>();
    }

    private void OnDestroy()
    {
        for (int i = _baseSystems.Count - 1; i >= 0; i--)
        {
            _baseSystems[i].Destroy();
            _baseSystems[i].Initialized = false;
        }
        Instance = null;
    }

    private void Update()
    {
        for (int i = 0; i < _tickSystems.Count; i++)
        {
            _tickSystems[i].Tick(Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _lateTickSystems.Count; i++)
        {
            _lateTickSystems[i].LateTick(Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _fixedTickSystems.Count; i++)
        {
            _fixedTickSystems[i].FixedTick(Time.fixedDeltaTime);
        }
    }
}