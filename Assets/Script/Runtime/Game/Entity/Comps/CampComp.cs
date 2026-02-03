using System;
using Script.Runtime.Framework.ObjectPool;
using Script.Runtime.Framework.System;

namespace Script.Runtime.Game.Entity
{
    public enum ECamp
    {
        Player,
        Monster,
    }

    public enum ECampRelation
    {
        None,
        Self,
        Friend,
        Enemy,
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

        public ECampRelation RelationTo(CampComp other)
        {
            if (other.Entity == Entity)
            {
                return ECampRelation.Self;
            }
            if (other.Camp == Camp)
            {
                return ECampRelation.Friend;
            }
            if (other.Camp != Camp)
            {
                return ECampRelation.Enemy;
            }
            return ECampRelation.None;
        }
    }
}