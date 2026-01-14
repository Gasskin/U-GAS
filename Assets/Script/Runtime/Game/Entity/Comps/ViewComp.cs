using Cysharp.Threading.Tasks;
using UnityEngine;

public class ViewComp : EntityComp
{
    public override int Priority => VIEW;

    private string assetPath;

    public GameObject View { get; private set; }

    public ViewComp(string path)
    {
        assetPath = path;
    }

    public override async UniTask Initialize()
    {
        View = await SystemDriver.YooSystem.InitializeGameObjectAsync(null, assetPath);
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