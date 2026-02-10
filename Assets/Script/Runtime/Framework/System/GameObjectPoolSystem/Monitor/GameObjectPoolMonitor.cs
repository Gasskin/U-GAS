#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    public class GameObjectPoolMonitor : MonoBehaviour
    {
    #region static
        [MenuItem("Tools/Monitor/GameObjectPoolMonitor")]
        public static void ShowMonitor()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            if (_monitor != null)
            {
                return;
            }
            var o = new GameObject("[GameObjectPoolMonitor]");
            _monitor = o.AddComponent<GameObjectPoolMonitor>();
            DontDestroyOnLoad(o);
        }

        private static GameObjectPoolMonitor _monitor;
    #endregion
        
        private class ObjectPoolState
        {
            public string Name;
            public int Get;
            public int Release;
        }

        [TextArea(1, 50), HideLabel]
        public string Monitor;

        private Dictionary<string, ObjectPoolState> _states = new();


        [OnInspectorGUI]
        public void OnInspectorGUI()
        {
            _states.Clear();

            var get = SystemDriver.GameObjectPoolSystem.GetCount;
            var release = SystemDriver.GameObjectPoolSystem.ReleaseCount;

            foreach (var key in get.Keys)
            {
                if (!_states.ContainsKey(key))
                {
                    _states[key] = new ObjectPoolState() { Name = key };
                }
            }

            foreach (var key in release.Keys)
            {
                if (!_states.ContainsKey(key))
                {
                    _states[key] = new ObjectPoolState() { Name = key };
                }
            }

            foreach (var pair in get)
            {
                _states[pair.Key].Get = pair.Value;
            }
            foreach (var pair in release)
            {
                _states[pair.Key].Release = pair.Value;
            }

            Monitor = "";
            foreach (var state in _states.Values)
            {
                Monitor += $"{state.Get - state.Release} {state.Name}\n";
            }
        }
    }
}
#endif