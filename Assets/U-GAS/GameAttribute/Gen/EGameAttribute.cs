using Sirenix.OdinInspector;
namespace U_GAS
{
	public enum EGameAttribute
	{
		/// <summary>
		/// 生命值
		/// </summary>
		[LabelText("生命值")]
		Hp,
		
		/// <summary>
		/// 法力值
		/// </summary>
		[LabelText("法力值")]
		Mp,
		
		/// <summary>
		/// 攻击力
		/// </summary>
		[LabelText("攻击力")]
		Atk,
		
		/// <summary>
		/// 防御力
		/// </summary>
		[LabelText("防御力")]
		Defence,
		
		Max = 4,
	}
}
