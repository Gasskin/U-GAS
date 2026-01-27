using Script.Runtime.Framework.System;

namespace cfg.Gas
{
    public interface ICheckCondition
    {
        bool Check(GasComp target);
    }
}
