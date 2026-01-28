using UnityEngine;

namespace Script.Runtime.Framework.System
{
    [DisallowMultipleComponent]
    public abstract class BaseWindow : MonoBehaviour
    {
        public abstract void OnTick(float dt);
        public abstract void OnOpen();
        public abstract void OnClose();
    }
}
