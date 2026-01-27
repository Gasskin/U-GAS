using System;

namespace cfg.Gas
{
    public partial class ConditionHasTag : ICheckCondition
    {
        public bool Check(GasComp target)
        {
            var tagController = target.GameTagController;

            for (int i = 0; i < AllTag.Count; i++)
            {
                var tag = GameTagController.ToGameTag(AllTag[i]);
                if (!tagController.HasTag(tag))
                {
                    return false;
                }
            }

            for (int i = 0; i < NoTag.Count; i++)
            {
                var tag = GameTagController.ToGameTag(NoTag[i]);
                if (tagController.HasTag(tag))
                {
                    return false;
                }
            }

            var any = AnyTag.Count <= 0;
            for (int i = 0; i < AnyTag.Count; i++)
            {
                var tag = GameTagController.ToGameTag(AnyTag[i]);
                if (tagController.HasTag(tag))
                {
                    any = true;
                    break;
                }
            }
            if (!any)
            {
                return false;
            }

            return true;
        }
    }
}