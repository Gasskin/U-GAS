using Script.Runtime.Framework.System;

namespace Script.Runtime.Game
{
    public enum ECamp
    {
        Player,
        Enemy,
    }


    public class CampComp : EntityComp
    {
        public override int Priority => Priority_Camp;

        public ECamp Camp { get; private set; }

        public CampComp(ECamp camp)
        {
            Camp = camp;
        }
    }
}