using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    [DisallowMultipleComponent]
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField]
        private UIRenderLayerSortMono[] _renderLayerSort;

        public Canvas Canvas { get; private set; }
        public int Depth { get; private set; }

        public abstract void OnTick(float dt);
        public abstract void OnOpen();
        public abstract void OnClose();

        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
        }

        public void OnDepthChange(int depth)
        {
            Depth = depth;
            for (int i = 0; i < _renderLayerSort.Length; i++)
            {
                _renderLayerSort[i].OnDepthChange(depth);
            }
        }

#if UNITY_EDITOR
        [Button]
        public void ResetRenderLayerSort()
        {
            _renderLayerSort = gameObject.GetComponentsInChildren<UIRenderLayerSortMono>(true);
        }
#endif
    }
}