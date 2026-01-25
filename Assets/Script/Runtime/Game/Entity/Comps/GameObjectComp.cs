using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameObjectComp : EntityComp
{
    public override int Priority => Priority_View;

    private string _assetPath;

    private Transform _parent;

    public GameObject View { get; private set; }

    public GameObjectComp(string path, Transform parent)
    {
        _assetPath = path;
        _parent = parent;
    }

    public override async UniTask Initialize()
    {
        View = await SystemDriver.YooSystem.InitializeGameObjectAsync(_parent, _assetPath);
        if (!IsValid)
        {
            Object.Destroy(View);
        }
    }

    public override void Destroy()
    {
        if (View != null) 
        {
            Object.Destroy(View);
        }
    }
}