namespace U_GAS
{
	public class GameAttribute_Defence : GameAttribute
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
