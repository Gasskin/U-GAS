
using System;
using System.IO;
using ProtoBuf;
namespace U_GAS
{
	public class GameAttribute_Hp : GameAttribute
	{
		private const string _U_ASSET_DATA = "CAEV+Kcr8h34pyty";
		private static byte[] _s_Bytes;
		static GameAttribute_Hp()
		{
			_s_Bytes = Convert.FromBase64String(_U_ASSET_DATA);
		}
		public GameAttribute_Hp()
		{
			uAsset = Serializer.Deserialize<GameAttributeUAsset>(new MemoryStream(_s_Bytes));
			OnInit();
		}
	}
}
