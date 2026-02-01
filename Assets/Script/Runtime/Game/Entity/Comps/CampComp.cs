using System;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;

namespace Script.Runtime.Game.Entity
{
    [Flags]
    public enum ECamp
    {
        Player = 1 << 0,
        Enemy = 1 << 1,
    }

    [EntityCompPriority(Priority_Camp)]
    public class CampComp : EntityComp
    {
        public ECamp Camp { get; private set; }

        public static CampComp Get(ECamp camp)
        {
            var comp = ObjectPool.Get<CampComp>();
            comp.Camp = camp;
            return comp;
        }
    }
}