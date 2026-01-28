using System;

namespace Script.Runtime.Framework.System
{
    public class EntityCompPriorityAttribute : Attribute
    {
        public int Priority;

        public EntityCompPriorityAttribute(int p)
        {
            Priority = p;
        }
    }
}