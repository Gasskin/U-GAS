namespace U_GAS
{
	public class GameAttributeSet_Atk : GameAttributeSet
	{
		public GameAttributeSet_Atk()
		{
			attributes = new()
			{
				{ EGameAttribute.Atk, new GameAttribute_Atk() },
			};
		}
	}
}
