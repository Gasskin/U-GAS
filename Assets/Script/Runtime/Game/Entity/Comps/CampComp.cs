using Script.Runtime.Framework.System;

namespace Script.Runtime.Game.Entity
{
    public enum ECamp
    {
        Player,
        Enemy,
    }

    [EntityCompPriority(Priority_Camp)]
    public class CampComp : EntityComp
    {
        public ECamp Camp { get; private set; }

        public CampComp(ECamp camp)
        {
            Camp = camp;
        }
    }
}