using System;
using System.Collections.Generic;

public class GameTagComp
{
    public EGameTag ToGameTag(string gameTag)
    {
        var tag = GameTagRegister.StringToEnum.GetValueOrDefault(gameTag, EGameTag.None);
        if (tag == EGameTag.None)
        {
            throw new ArgumentOutOfRangeException($"不存在的GameTag：{gameTag}");
        }
        return EGameTag.None;
    }
}