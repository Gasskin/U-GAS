using System;
using System.IO;
using ProtoBuf;
namespace U_GAS
{
    public static class NewGameEffectTemplate
    {
        private static string _base64 = "CAIVAACwQR0AAABASg8VAADIQhoIogYFDQAAoEBKEQgCFQrXL0EaCKoGBQ0AwHlEUhNKEQgCFQAAXkMaCKIGBQ0AAKBA";
        private static byte[] _base64Bytes;
        public static GameEffect New()
        {
            _base64Bytes ??= Convert.FromBase64String(_base64);
            using var ms = new MemoryStream(_base64Bytes);
            return Serializer.Deserialize<GameEffect>(ms);
        }
    }
}
