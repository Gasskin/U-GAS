using Sirenix.OdinInspector;
using UnityEngine;

namespace Script.Runtime.Framework.System
{
    [DisallowMultipleComponent]
    public abstract class BaseWidget : MonoBehaviour
    {
        [SerializeField]
        private UIRenderLayerSortMono[] _renderLayerSort;
        
        protected abstract void OnTick(float dt);
        protected abstract void OnOpen();
        protected abstract void OnClose();

#if UNITY_EDITOR
        [Button]
        public void ResetRenderLayerSort()
        {
            _renderLayerSort = gameObject.GetComponentsInChildren<UIRenderLayerSortMono>(true);
        }
#endif
    }
}
