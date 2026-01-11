using System.Collections.Generic;
using UnityEngine;

public class SystemDriver : MonoBehaviour
{
#region static
    private static SystemDriver s_instance;

    public static T GetSystem<T>() where T : BaseSystem
    {
        if (s_instance == null)
        {
            return null;
        }
        for (int i = 0; i < s_instance._baseSystems.Count; i++)
        {
            if (s_instance._baseSystems[i] is T sys)
            {
                return sys;
            }
        }
        return null;
    }
#endregion

    private readonly List<BaseSystem> _baseSystems = new()
    {
        new DesignSystem(),
        new BattleTimeSystem(),
        new EntitySystem(),
        new ProcedureSystem(),
    };

    private List<ITickSystem> _tickSystems = new();
    private List<IFixedTickSystem> _fixedTickSystems = new();
    private List<ILateTickSystem> _lateTickSystems = new();

    private void Start()
    {
        DontDestroyOnLoad(this);
        s_instance = this;
        foreach (var sys in _baseSystems)
        {
            sys.Initialize();
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
    }

    private void OnDestroy()
    {
        foreach (var sys in _baseSystems)
        {
            sys.Close();
        }
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
            _fixedTickSystems[i].FixedTick(Time.deltaTime);
        }
    }
}