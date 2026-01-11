using System;
using UnityEngine;
using YooAsset;

public class YooGameObjectHandler : MonoBehaviour
{
    public AssetHandle Handle;

    private void OnDestroy()
    {
        Handle.Dispose();
    }
}