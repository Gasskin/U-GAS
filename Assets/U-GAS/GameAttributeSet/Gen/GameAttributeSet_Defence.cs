namespace U_GAS
{
	public class GameAttributeSet_Defence : GameAttributeSet
	{
		public GameAttributeSet_Defence()
		{
			attributes = new()
			{
				{ EGameAttribute.Defence, new GameAttribute_Defence() },
			};
		}
	}
}
