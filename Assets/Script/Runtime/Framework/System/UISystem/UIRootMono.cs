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
        public List<UILayer> Layers = new();

        public Dictionary<EUILayer, Transform> UIRoots = new();


        private void Awake()
        {
            foreach (var uiLayer in Layers)
            {
                UIRoots.Add(uiLayer.Layer, uiLayer.Root.transform);
                uiLayer.Root.overrideSorting = true;
                uiLayer.Root.sortingOrder = (int)uiLayer.Layer;
            }
        }
    }
}