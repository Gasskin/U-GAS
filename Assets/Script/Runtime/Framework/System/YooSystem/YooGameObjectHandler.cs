using UnityEngine;
using YooAsset;

namespace Script.Runtime.Framework.System
{
    public class YooGameObjectHandler : MonoBehaviour
    {
        public AssetHandle Handle;

        private void OnDestroy()
        {
            Handle.Dispose();
        }
    }
}