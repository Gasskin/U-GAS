using System;
using MemoryPack;

[Serializable]
[MemoryPackable]
[MemoryPackUnion(1, typeof(TestDamage))]
public abstract partial class BaseSkillAction
{
}

[Serializable]
[MemoryPackable]
public partial class TestDamage : BaseSkillAction
{
    public int Test;
}