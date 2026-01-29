using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    public enum EUILayer
    {
        Background = 0,
        Normal = 1000,
        Pop = 3000,
        System = 4000,
    }

    [Serializable]
    public class UILayer
    {
        public EUILayer Layer;
        public Canvas Root;
    }

    public class UIRootMono : MonoBehaviour
    {
        [SerializeField]
        private List<UILayer> _layers = new();

        public Dictionary<EUILayer, Transform> UIRoots = new();


        private void Awake()
        {
            foreach (var uiLayer in _layers)
            {
                UIRoots.Add(uiLayer.Layer, uiLayer.Root.transform);
                uiLayer.Root.overrideSorting = true;
                uiLayer.Root.sortingOrder = (int)uiLayer.Layer;
            }
        }
    }
}