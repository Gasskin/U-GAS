using System;
using System.Collections.Generic;
namespace U_GAS
{
	public static class GameAttributeRegister
	{
		public static Dictionary<EGameAttribute, Type> KeyToType = new ()
		{
			{ EGameAttribute.Hp , typeof(GameAttribute_Hp)},
			{ EGameAttribute.Mp , typeof(GameAttribute_Mp)},
			{ EGameAttribute.Atk , typeof(GameAttribute_Atk)},
		};
		public static BaseGameAttribute Get(EGameAttribute attribute)
		{
			switch (attribute)
			{
				case EGameAttribute.Hp:
					return new GameAttribute_Hp();
				case EGameAttribute.Mp:
					return new GameAttribute_Mp();
				case EGameAttribute.Atk:
					return new GameAttribute_Atk();
			}
			return null;
		}
	}
}
