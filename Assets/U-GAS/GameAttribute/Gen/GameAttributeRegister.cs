using System;
using System.Collections.Generic;
namespace U_GAS
{
	public static class GameAttributeRegister
	{
		public static GameAttribute New(EGameAttribute attribute)
		{
			switch (attribute)
			{
				case EGameAttribute.Atk:
					return new GameAttribute_Atk();
				case EGameAttribute.Hp:
					return new GameAttribute_Hp();
				case EGameAttribute.Mp:
					return new GameAttribute_Mp();
				case EGameAttribute.Defence:
					return new GameAttribute_Defence();
			}
			return null;
		}
	}
}
