using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    // Open -> Close
    // 如果被全屏界面遮挡 -> Hide
    // 遮挡恢复 -> Show
    [DisallowMultipleComponent]
    public abstract class BaseWindow : MonoBehaviour
    {
        [SerializeField]
        private UIRenderLayerSortMono[] _renderLayerSort;

        public Canvas Canvas { get; private set; }
        public int Depth { get; private set; }

        public UILogic Logic;

        public abstract void OnTick(float dt);
        public abstract void OnOpen();
        public abstract void OnClose();
        public abstract void OnShow();

        public abstract void OnHide();


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

        protected void CloseWindow()
        {
            SystemDriver.UISystem.CloseWindow(Logic.Uid);
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