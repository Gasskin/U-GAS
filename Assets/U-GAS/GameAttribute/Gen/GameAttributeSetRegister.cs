using System;
using System.Collections.Generic;
namespace U_GAS
{
    public static class GameAttributeSetRegister
    {
        public static BaseGameAttributeSet New(EGameAttributeSet set)
        {
            switch (set)
            {
                case EGameAttributeSet.Atk:
                    return new GameAttributeSet_Atk();
                case EGameAttributeSet.Defence:
                    return new GameAttributeSet_Defence();
                case EGameAttributeSet.Number:
                    return new GameAttributeSet_Number();
            }
            return null;
        }
    }
}