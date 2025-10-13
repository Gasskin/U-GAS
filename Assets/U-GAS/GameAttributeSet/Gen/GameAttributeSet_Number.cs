namespace U_GAS
{
	public class GameAttributeSet_Number : GameAttributeSet
	{
		public GameAttributeSet_Number()
		{
			attributes = new()
			{
				{ EGameAttribute.Hp, new GameAttribute_Hp() },
				{ EGameAttribute.Mp, new GameAttribute_Mp() },
			};
		}
	}
}
