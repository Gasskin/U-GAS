using System;
using Luban;

namespace cfg
{
    public partial class Tables
    {
        public Gas.TbSkillTimeline TbSkillTimeline {get; private set; }

        public void ResolveCustomTables(Func<string, byte[]> loader)
        {
            TbSkillTimeline = new Gas.TbSkillTimeline(loader("gas_tbskilltimeline"));
        }

    }
}