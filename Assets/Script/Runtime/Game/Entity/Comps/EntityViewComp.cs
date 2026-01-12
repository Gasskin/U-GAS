using Cysharp.Threading.Tasks;
using UnityEngine;

public class EntityViewComp : EntityComp
{
    public override int Priority => c_EntityView;

    private string _assetPath;

    public GameObject View { get; private set; }

    public EntityViewComp(string path)
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