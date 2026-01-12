using System;
using MemoryPack;

[Serializable]
[MemoryPackable]
[MemoryPackUnion(1, typeof(TestDamage))]
[MemoryPackUnion(2, typeof(TestDamage2))]
public abstract partial class BaseSkillAction
{
}

[Serializable]
[MemoryPackable]
public partial class TestDamage : BaseSkillAction
{
    public int Test;
}

[Serializable]
[MemoryPackable]
public partial class TestDamage2 : BaseSkillAction
{
    public int Test1;
    public int Test2;
}