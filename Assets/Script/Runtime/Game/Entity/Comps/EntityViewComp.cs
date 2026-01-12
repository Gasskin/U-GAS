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
        View = SystemDriver.GetSystem<YooSystem>().InitializeGameObjectSync(SystemDriver.Instance.EntityRoot, _assetPath);
    }

    public override void OnRemove()
    {
    }
}