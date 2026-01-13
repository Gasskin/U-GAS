using Cysharp.Threading.Tasks;
using UnityEngine;

public class ViewComp : EntityComp
{
    public override int Priority => c_View;

    private string _assetPath;

    public GameObject View { get; private set; }

    public ViewComp(string path)
    {
        _assetPath = path;
    }

    public override void OnAdd()
    {
        AddViewAsync().Forget();
    }

    public override void OnRemove()
    {
        if (View != null) 
        {
            Object.Destroy(View);
        }
    }

    private async UniTaskVoid AddViewAsync()
    {
        View = await SystemDriver.Get<YooSystem>().InitializeGameObjectAsync(SystemDriver.Instance.EntityRoot, _assetPath);
        if (!IsValid)
        {
            Object.Destroy(View);
        }
    }
}