using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Runtime.Game.System;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    public enum EYooAssetsMode
    {
        Editor,
        Offline,
    }

    public class SystemDriver : MonoBehaviour
    {
    #region static
        public static SystemDriver Instance { get; private set; }

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

        public UIRootMono UIRoot;

        // Framework
        public static YooSystem YooSystem => Get<YooSystem>();
        public static ConfigSystem ConfigSystem => Get<ConfigSystem>();
        public static EventSystem EventSystem => Get<EventSystem>();
        public static EntitySystem EntitySystem => Get<EntitySystem>();
        public static ProcedureSystem ProcedureSystem => Get<ProcedureSystem>();
        public static UISystem UISystem => Get<UISystem>();
        public static GameObjectPoolSystem GameObjectPoolSystem => Get<GameObjectPoolSystem>();

        // Game
        public static PlayerDataSystem PlayerDataSystem => Get<PlayerDataSystem>();
        public static InputSystem InputSystem => Get<InputSystem>();
        public static TimeSystem TimeSystem => Get<TimeSystem>();
        public static ColliderSystem ColliderSystem => Get<ColliderSystem>();


        private readonly List<BaseSystem> _baseSystems = new()
        {
            // Framework System
            new YooSystem(),
            new ConfigSystem(),
            new EventSystem(),
            new GameObjectPoolSystem(),
            new UISystem(),
            new EntitySystem(),
            new ProcedureSystem(),
            // Game System
            new TimeSystem(),
            new InputSystem(),
            new PlayerDataSystem(),
            new ColliderSystem(),
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

            ProcedureSystem.ChangeProcedure<InitProcedure>();
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
}