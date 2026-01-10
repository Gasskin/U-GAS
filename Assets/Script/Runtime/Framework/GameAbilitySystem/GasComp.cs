[EntityComp(EntityCompPriority.c_Gas, true, false, false)]
public class GasComp : EntityComp
{
    public GameTagController GameTagController { get; private set; }
    
    public override void OnAdd()
    {
        GameTagController = new();
        GameTagController.Init(this);
    }

    public override void Tick(float dt)
    {
    }
}