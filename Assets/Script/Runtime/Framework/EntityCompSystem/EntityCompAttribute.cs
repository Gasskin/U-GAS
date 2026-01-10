public class EntityCompAttribute : System.Attribute
{
    public int Priority;
    public bool Update;
    public bool LateUpdate;
    public bool FixedUpdate;

    public EntityCompAttribute(int priority, bool update, bool lateUpdate, bool fixedUpdate)
    {
        this.Priority = priority;
        this.Update = update;
        this.LateUpdate = lateUpdate;
        this.FixedUpdate = fixedUpdate;
    }
}