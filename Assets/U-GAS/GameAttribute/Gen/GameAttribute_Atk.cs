namespace U_GAS
{
	public class GameAttribute_Atk : GameAttribute
	{
		public GameAttribute_Atk()
		{
			minValue = 0f;
			maxValue = 3.402823E+38f;
			eCalculateMode = ECalculateMode.Stacking;
			attributeType = EGameAttribute.Atk;
		}
	}
}
