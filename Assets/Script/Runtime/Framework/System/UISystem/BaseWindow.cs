using UnityEngine;

namespace Script.Runtime.Framework.System
{
    [DisallowMultipleComponent]
    public abstract class BaseWindow : MonoBehaviour
    {
        protected abstract void OnTick(float dt);
        protected abstract void OnOpen();
        protected abstract void OnClose();
    }
}
