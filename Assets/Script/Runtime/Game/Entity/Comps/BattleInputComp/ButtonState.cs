public class ButtonState
{
    public bool IsHolding { get; private set; }
    
    public bool IsPressedThisFrame { get; private set; }
    
    public bool IsReleasedThisFrame { get; private set; }

    public void Start()
    {
        if (!IsHolding)
        {
            IsHolding = true;
            IsPressedThisFrame = true;
        }
    }

    public void Cancel()
    {
        if (IsHolding)
        {
            IsHolding = false;
            IsReleasedThisFrame = true;
        }
    }

    public void ResetFrameState()
    {
        IsPressedThisFrame = false;
        IsReleasedThisFrame = false;
    }
}