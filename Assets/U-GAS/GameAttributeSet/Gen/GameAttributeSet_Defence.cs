
using System;
using System.IO;
using ProtoBuf;
namespace U_GAS
{
	public class GameAttributeSet_Defence : GameAttributeSet
	{
		private const string _U_ASSET_DATA = "CAM=";
		private static byte[] _s_Bytes;
		static GameAttributeSet_Defence()
		{
			_s_Bytes = Convert.FromBase64String(_U_ASSET_DATA);
		}
		public GameAttributeSet_Defence()
		{
			uAsset = Serializer.Deserialize<GameAttributeSetUAsset>(new MemoryStream(_s_Bytes));
			OnInit();
		}
	}
}
