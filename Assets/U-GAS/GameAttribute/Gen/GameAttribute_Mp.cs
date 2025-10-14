
using System;
using System.IO;
using ProtoBuf;
namespace U_GAS
{
	public class GameAttribute_Mp : GameAttribute
	{
		private const string _U_ASSET_DATA = "Hf//f38=";
		private static byte[] _s_Bytes;
		static GameAttribute_Mp()
		{
			_s_Bytes = Convert.FromBase64String(_U_ASSET_DATA);
		}
		public GameAttribute_Mp()
		{
			uAsset = Serializer.Deserialize<GameAttributeUAsset>(new MemoryStream(_s_Bytes));
		}
	}
}
