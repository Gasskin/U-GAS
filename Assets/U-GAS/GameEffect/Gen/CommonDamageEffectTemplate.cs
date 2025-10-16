using System;
using System.IO;
using ProtoBuf;
namespace U_GAS
{
    public static class CommonDamageEffectTemplate
    {
        private static string _base64 = @"
FQAAsEEdAAAAQFIWEAEdAAAgQSINogYKDQAAoEAVAACgQFoYUhYIAh0AAF5DIg2iBgoNAA
CgQBUAAKBAYAE=";
        private static GameEffect _template;
        public static GameEffect Get()
        {
            if (_template != null) 
            {
                return _template;
            }
            var data = Convert.FromBase64String(_base64);
            using var ms = new MemoryStream(data);
            _template = Serializer.Deserialize<GameEffect>(ms);
            return _template;
        }
    }
}
