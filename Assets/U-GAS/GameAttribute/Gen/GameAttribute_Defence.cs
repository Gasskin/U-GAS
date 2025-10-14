
using System;
using System.IO;
using ProtoBuf;
namespace U_GAS
{
	public class GameAttribute_Defence : GameAttribute
	{
		private const string _U_ASSET_DATA = "Ff//f/8d//9/fw==";
		private static byte[] _s_Bytes;
		static GameAttribute_Defence()
		{
			_s_Bytes = Convert.FromBase64String(_U_ASSET_DATA);
		}
		public GameAttribute_Defence()
		{
			uAsset = Serializer.Deserialize<GameAttributeUAsset>(new MemoryStream(_s_Bytes));
		}
	}
}
