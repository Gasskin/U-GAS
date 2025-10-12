namespace U_GAS
{
	public class GameAttributeSet_Atk : BaseGameAttributeSet
	{
		public GameAttributeSet_Atk()
		{
			attributes = new()
			{
				{ EGameAttribute.Atk, new GameAttribute_Atk() },
			};
		}
	}
	public class GameAttributeSet_Defence : BaseGameAttributeSet
	{
		public GameAttributeSet_Defence()
		{
			attributes = new()
			{
				{ EGameAttribute.Defence, new GameAttribute_Defence() },
			};
		}
	}
	public class GameAttributeSet_Number : BaseGameAttributeSet
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
