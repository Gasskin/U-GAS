namespace U_GAS
{
	public class GameAttribute_Hp : BaseGameAttribute
	{
		public GameAttribute_Hp()
		{
			minValue = 0f;
			maxValue = 3.402823E+38f;
			eCalculateMode = ECalculateMode.Stacking;
			attributeType = EGameAttribute.Hp;
		}
	}
	public class GameAttribute_Mp : BaseGameAttribute
	{
		public GameAttribute_Mp()
		{
			minValue = 0f;
			maxValue = 3.402823E+38f;
			eCalculateMode = ECalculateMode.Stacking;
			attributeType = EGameAttribute.Mp;
		}
	}
	public class GameAttribute_Atk : BaseGameAttribute
	{
		public GameAttribute_Atk()
		{
			minValue = 0f;
			maxValue = 3.402823E+38f;
			eCalculateMode = ECalculateMode.Stacking;
			attributeType = EGameAttribute.Atk;
		}
	}
	public class GameAttribute_Defence : BaseGameAttribute
	{
		public GameAttribute_Defence()
		{
			minValue = -3.402823E+38f;
			maxValue = 3.402823E+38f;
			eCalculateMode = ECalculateMode.Stacking;
			attributeType = EGameAttribute.Defence;
		}
	}
}
